/*
 * 相机控制器。
 * 由于相机控制有很多种方式，这里只提供一些API。具体实现还是需要根据具体需求来做一些更改。
 */
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraControl : MonoBehaviour
{
    #region --变量定义
    [SerializeField] private bool ignoreUILayer = false;//是否忽略UI层。
    public Transform cameraTarget;//相机围绕旋转目标点

    public float distance = 4;//相机距离目标点的距离
    public float minDistance = 2;//相机距离目标点的最小距离
    public float maxDistance = 5;//相机距离目标点的最大距离

    public float xRotateSpeed = 200;//X轴方向旋转速度
    public float yRotateSpeed = 200;//Y轴方向旋转速度
    public float pushSpeed = 5;//滚轮拉近、拉远视野速度
    public float moveSpeed = 0.2f;//相机移动速度

    public float yRotateMinAngle = -50;//Y轴方向旋转最小角度
    public float yRotateMaxAngle = 50;//Y轴方向旋转最大角度

    public bool useDamping = true;//是否使用阻力
    public float damping = 5.0f;//阻力系数

    public bool restrictMouseArea = false;//是否启用鼠标区域限制
    public Rect mouseControlAreaRect = new Rect(0, 0, 1920, 1080);//鼠标在屏幕上可控制的区域

    private float x = 0.0f;//相机X轴旋转角度
    private float y = 0.0f;//相机Y轴旋转角度
    private Vector3 TargetInitialPos;//相机目标点的初始位置
    #endregion

    #region --系统函数
    private void Start()
    {
        Init();
    }
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            UpdateViewAngle();
        }
        if (Input.GetMouseButton(1))
        {
            MoveCamera();
        }
        UpdateViewDistance();
    }
    private void LateUpdate()
    {
        UpdateCameraTransform();

    }
    #endregion

    #region --自定义函数
    public bool IgnoreUILayer
    {
        set { ignoreUILayer = value; }
    }
    /// <summary>
    /// 判断鼠标是否点击在UI。当 ignoreUILayer 为 false 时才生效，否则只会返回true。
    /// </summary>
    private bool IsClickUI
    {
        get
        {
            if (!ignoreUILayer)
            {
                if (EventSystem.current == null)
                {
                    return true;
                }
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
    }
    /// <summary>
    /// 判断鼠标是否在控制区域内。当 UsedAreaRect 为true时才生效，否则只会返回true。
    /// </summary>
    private bool IsInArea
    {
        get
        {
            if (restrictMouseArea)
            {
                if (Input.mousePosition.x > mouseControlAreaRect.x && Input.mousePosition.y > mouseControlAreaRect.y &&
                    Input.mousePosition.x < mouseControlAreaRect.width && Input.mousePosition.y < mouseControlAreaRect.height)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
    }

    private void Init()
    {
        if (cameraTarget == null)
        {
            Debug.LogError("cameraTarget 为 Null");
            return;
        }
        else
        {
            TargetInitialPos = cameraTarget.position;
        }
    }

    #region --方案一：适合围绕一个物体进行操作。
    /// <summary>
    /// 相机围绕目标点，以相机自身轴旋转。
    /// </summary>
    private void CameraRotate()
    {
        float _mouseX = Input.GetAxis("Mouse X");
        float _mouseY = -Input.GetAxis("Mouse Y");
        if (Mathf.Abs(_mouseX) >= Mathf.Abs(_mouseY))
        {
            _mouseY = 0;
        }
        else
        {
            _mouseX = 0;
        }
        transform.RotateAround(cameraTarget.position, this.transform.up, _mouseX * 5);
        transform.RotateAround(cameraTarget.position, this.transform.right, _mouseY * 5);
    }
    /// <summary>
    /// 相机根据视野距离，更新自己位置坐标。
    /// </summary>
    private void UpdatePositionByDistance()
    {
        if (cameraTarget != null)
        {
            Vector3 _position = cameraTarget.position + distance * this.transform.forward * -1;
            transform.position = Vector3.Lerp(transform.position, _position, Time.fixedDeltaTime * damping);//线性
            //transform.position = Vector3.Slerp(transform.position, _position, Time.fixedDeltaTime * damping);//弧形插值
        }
    }
    /// <summary>
    /// 相机从当前位置以弧线的形式运动到目标位置，并始终看向目标点。
    /// </summary>
    /// <param name="_endPoint"></param>
    /// <returns></returns>
    public IEnumerator ToEndPoint(Vector3 _endPoint)
    {
        Vector3 _startpoint = this.transform.position;
        //两者中心点
        Vector3 center = (_startpoint + _endPoint) * 0.5f;
        center -= new Vector3(0, 1, 0);
        Vector3 _start = _startpoint - center;
        Vector3 _end = _endPoint - center;
        bool _move = true;
        float _time = 0;
        yield return null;

        while (_move)
        {
            this.transform.LookAt(cameraTarget);
            //弧形插值
            transform.position = Vector3.Slerp(_start, _end, _time);
            transform.position += center;
            if (Vector3.Distance(this.transform.position, _endPoint) < 0.01f)
            {
                _move = false;
            }
            yield return null;
            _time += Time.deltaTime;
        }
    }
    #endregion

    #region --方案二：适合不停的改变目标点使用（主方案）。
    private void UpdateViewAngle()
    {
        if (cameraTarget != null && IsClickUI && IsInArea)
        {
            x += Input.GetAxis("Mouse X") * xRotateSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y") * yRotateSpeed * 0.02f;
            y = ClampAngle(y, yRotateMinAngle, yRotateMaxAngle);
        }
    }
    private void UpdateViewDistance()
    {
        if (cameraTarget != null && IsClickUI && IsInArea)
        {
            distance -= Input.GetAxis("Mouse ScrollWheel") * pushSpeed;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
        }
    }
    private void UpdateCameraTransform()
    {
        if (cameraTarget != null)
        {
            Quaternion rotation = Quaternion.Euler(y, x, 0.0f);
            Vector3 disVector = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * disVector + cameraTarget.position;

            if (useDamping)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * damping);
                transform.position = Vector3.Slerp(transform.position, position, Time.fixedDeltaTime * damping);
            }
            else
            {
                transform.rotation = rotation;
                transform.position = position;
            }
        }
    }
    private void MoveCamera()
    {
        if (cameraTarget != null && IsClickUI && IsInArea)
        {
            transform.position = transform.position - transform.right * Input.GetAxis("Mouse X") * moveSpeed - transform.up * Input.GetAxis("Mouse Y") * moveSpeed;
            cameraTarget.position = cameraTarget.position - transform.right * Input.GetAxis("Mouse X") * moveSpeed - transform.up * Input.GetAxis("Mouse Y") * moveSpeed;
        }
    }
    #endregion

    /// <summary>
    /// 设置相机目标点
    /// </summary>
    /// <param name="_x"></param>
    /// <param name="_y"></param>
    /// <param name="_z"></param>
    public void SetCameraView(float _x, float _y, float _z)
    {
        cameraTarget.position = new Vector3(_x, _y, _z);
    }
    /// <summary>
    /// 设置相机目标点
    /// </summary>
    /// <param name="newTarget"></param>
    public void SetCameraView(Transform newTarget)
    {
        if (newTarget != null)
        {
            cameraTarget = newTarget;
        }
    }
    /// <summary>
    /// 设置相机目标点
    /// </summary>
    /// <param name="_pos"></param>
    public void SetCameraView(Vector3 _pos)
    {
        cameraTarget.position = _pos;
    }
    /// <summary>
    /// 重置相机目标点为初始目标点
    /// </summary>
    public void ResetCameraToInitialTarget()
    {
        cameraTarget.position = TargetInitialPos;
    }
    /// <summary>
    /// 限制角度在min和max之间
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    private static float ClampAngle(float _angle, float _minAngle, float _maxAngle)
    {
        if (_angle < -360)
        {
            _angle += 360;
        }
        if (_angle > 360)
        {
            _angle -= 360;
        }
        return Mathf.Clamp(_angle, _minAngle, _maxAngle);
    }
    #endregion
}