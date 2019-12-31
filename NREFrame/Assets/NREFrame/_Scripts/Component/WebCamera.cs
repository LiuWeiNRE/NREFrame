/*
 * 启用设备相机组件。
 */

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WebCamera : MonoBehaviour
{
    #region -- 变量定义
    [SerializeField] private Vector2 webCamSize = new Vector2(1920, 1080);
    [SerializeField] private int webCamFPS = 20;
    [SerializeField] private RawImage targetImage;

    private WebCamTexture webCamTexture;
    private WebCamDevice[] devices;
    private int camIndex = -1;
    private bool isPlaying = false;
    #endregion

    #region -- 系统函数
    private void Awake()
    {
        Init();
    }
    private void OnDestroy()
    {
        if (isPlaying)
        {
            Stop(); 
        }
    }
    #endregion

    #region -- 自定义函数
    private void Init()
    {
        Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            devices = WebCamTexture.devices;
        }
        else
        {
            devices = new WebCamDevice[] { };
        }
    }
    /// <summary>
    ///  激活相机，并渲染画面到 targetImage 上面。
    /// </summary>
    public void EnableWebCamera()
    {
        if (isPlaying)
        {
            return;
        }

        camIndex = 0;
        if (devices.Length <= 0 || camIndex >= devices.Length)
        {
            Debug.Log("未检测到索引为" + 0 + "的相机");
            return;
        }

        webCamTexture = new WebCamTexture(devices[camIndex].name, (int)webCamSize.x, (int)webCamSize.y, webCamFPS);
        if (targetImage != null)
        {
            targetImage.texture = webCamTexture;
        }
        else
        {
            Debug.LogError("targetImage is null");
        }

        Play();
    }
    /// <summary>
    /// 激活相机，并渲染画面到 targetImage 上面。
    /// </summary>
    /// <param name="_webCameraIndex">相机索引。设备上有可能有多个相机</param>
    public void EnableWebCamera(int _webCameraIndex)
    {
        if (isPlaying)
        {
            return;
        }

        camIndex = _webCameraIndex;
        if (devices.Length <= 0 || camIndex >= devices.Length)
        {
            Debug.Log("未检测到索引为" + camIndex + "的相机");
            return;
        }

        webCamTexture = new WebCamTexture(devices[camIndex].name, (int)webCamSize.x, (int)webCamSize.y, webCamFPS);
        if (targetImage != null)
        {
            targetImage.texture = webCamTexture;
        }
        else
        {
            Debug.LogError("targetImage is null");
        }

        Play();
    }
    /// <summary>
    /// 相机画面分辨率
    /// </summary>
    public Vector2 WebCamSize
    {
        get { return webCamSize; }
    }
    /// <summary>
    /// 相机画面渲染帧率
    /// </summary>
    public int WebCamFPS
    {
        get { return webCamFPS; }
    }
    /// <summary>
    /// 相机渲染目标 RawImage
    /// </summary>
    public RawImage TargetImage
    {
        set
        {
            targetImage.texture = null;
            targetImage = value;
            targetImage.texture = webCamTexture;
        }
    }
    /// <summary>
    /// 相机是否正在渲染画面。
    /// </summary>
    public bool IsPlaying
    {
        get { return isPlaying; }
    }
    /// <summary>
    /// 相机数组
    /// </summary>
    public WebCamDevice[] WebCameras
    {
        get { return devices; }
    }
    /// <summary>
    /// 开始渲染相机画面
    /// </summary>
    public void Play()
    {
        if (webCamTexture == null)
        {
            Debug.LogError("webCamTexture is null");
            return;
        }
        webCamTexture.Play();
        isPlaying = true;
    }
    /// <summary>
    /// 暂停渲染相机画面
    /// </summary>
    public void Pause()
    {
        if (webCamTexture == null)
        {
            Debug.LogError("webCamTexture is null");
            return;
        }
        webCamTexture.Pause();
        isPlaying = false;
    }
    /// <summary>
    /// 停止渲染相机画面
    /// </summary>
    public void Stop()
    {
        if (webCamTexture == null)
        {
            Debug.LogError("webCamTexture is null");
            return;
        }
        webCamTexture.Stop();
        isPlaying = false;
    }
    /// <summary>
    /// 获取相机当前的画面，并转成 Texture2D 格式。
    /// </summary>
    /// <param name="_textureOperationEvent">图片操作事件。</param>
    /// <returns></returns>
    public IEnumerator GetTexture2D(Action<Texture2D> _textureOperationEvent)
    {
        if (webCamTexture == null || !isPlaying)
        {
            yield break;
        }
        else
        {
            webCamTexture.Pause();
        }

        yield return new WaitForEndOfFrame();
        if (webCamTexture == null)
        {
            yield break;
        }
        Texture2D _texture2D = new Texture2D(webCamTexture.requestedWidth, webCamTexture.requestedHeight);
        _texture2D.SetPixels(webCamTexture.GetPixels());
        _texture2D.Apply();
        webCamTexture.Play();

        _textureOperationEvent(_texture2D);
    }
    /// <summary>
    /// 自动适配 targetImage,与父物体大小一致。当父物体为画布时，可以全屏显示。
    /// </summary>
    public void AutoAdaptationTargetImage()
    {
        
            if (targetImage == null)
            {
                Debug.LogError("targetImage is null");
                return;
            }

            targetImage.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
            targetImage.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
            targetImage.rectTransform.anchorMin = Vector2.zero;
            targetImage.rectTransform.anchorMax = Vector2.one;
            targetImage.rectTransform.localPosition = Vector3.zero;


        if (Application.platform == RuntimePlatform.Android)
        {
            targetImage.rectTransform.localEulerAngles = new Vector3(0, 0, -90);
            RectTransform _parent = targetImage.rectTransform.parent as RectTransform;
            float _val = (_parent.sizeDelta.y - _parent.sizeDelta.x) / 2f;
            if (_val < 0)
            {
                _val *= -1;
            }
            targetImage.rectTransform.offsetMin = new Vector2(_val * -1, _val);
            targetImage.rectTransform.offsetMax = new Vector2(_val, _val * -1);
        }
        else
        {
            targetImage.rectTransform.offsetMin = Vector2.zero;
            targetImage.rectTransform.offsetMax = Vector2.zero;
        }
    }
    #endregion
}