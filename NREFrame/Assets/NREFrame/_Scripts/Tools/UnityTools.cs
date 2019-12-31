/*
 * Unity3D 开发常用工具方法。
*/
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UnityTools
{
    #region -- 自定义函数
    /// <summary>
    /// 判断鼠标是否点击在UI。
    /// </summary>
    public static bool IsClickUI
    {
        get
        {
            if (EventSystem.current == null)
            {
                return false;
            }
            return EventSystem.current.IsPointerOverGameObject();
        }
    }
    /// <summary>
    /// 获取指定路径下，指定文件格式的所有文件路径，并返回。
    /// </summary>
    /// <param name="_url">指定路径</param>
    /// <param name="_fileFormats">指定格式。如：*.png</param>
    /// <returns></returns>
    public static List<string> GetAllFilePaths(string _url, string[] _fileFormats)
    {
        List<string> _filePaths = new List<string>();
        for (int i = 0; i < _fileFormats.Length; i++)
        {
            string[] dirs = Directory.GetFiles(_url, _fileFormats[i]);
            for (int j = 0; j < dirs.Length; j++)
            {
                _filePaths.Add(dirs[j]);
            }
        }
        return _filePaths;
    }
    /// <summary>
    /// 获取指定路径下，指定文件格式的所有文件名称，并返回
    /// </summary>
    /// <param name="_url">指定路径</param>
    /// <param name="_fileFormat">指定格式。如：*.png</param>
    /// <returns></returns>
    public static List<string> GetAllFileName(string _url, string _fileFormat)
    {
        List<string> _fileNames = new List<string>();
        int _fileFormatLeng = _fileFormat.Length - 1;
        DirectoryInfo _folder = new DirectoryInfo(_url);
        foreach (FileInfo _file in _folder.GetFiles(_fileFormat))
        {
            string _fileName = _file.Name;
            _fileNames.Add(_fileName.Substring(0, _fileName.Length - _fileFormatLeng));
        }
        return _fileNames;
    }
    /// <summary>
    /// 给 EventTrigger 添加指定类型的监听事件
    /// </summary>
    /// <param name="_eventTrigger"></param>
    /// <param name="eventType">监听事件类型</param>
    /// <param name="unityAction">监听事件触发的函数</param>
    public static void AddEventTrigger(EventTrigger _eventTrigger, EventTriggerType eventType, UnityAction<BaseEventData> unityAction)
    {
        if (_eventTrigger == null)
        {
            Debug.Log("_eventTrigger 为 Null");
            return;
        }
        //定义回掉函数，委托
        UnityAction<BaseEventData> action = new UnityAction<BaseEventData>(unityAction);
        //判断 EventTrigger 组件上是否已经存在监听事件，如果有就查找是否存在eventType类型的监听事件
        //如果找到了，就赋值，并返回
        if (_eventTrigger.triggers.Count != 0)
        {
            for (int i = 0; i < _eventTrigger.triggers.Count; i++)
            {
                if (_eventTrigger.triggers[i].eventID == eventType)
                {
                    _eventTrigger.triggers[i].callback.AddListener(unityAction);
                    return;
                }
            }
        }
        //定义所要绑定的事件类型
        EventTrigger.Entry entry = new EventTrigger.Entry();
        //设置事件类型
        entry.eventID = eventType;
        //设置回掉函数
        entry.callback.AddListener(action);
        //添加触发事件到EventTrigger组件上
        _eventTrigger.triggers.Add(entry);
    }
    /// <summary>
    /// 找到最外层的父物体并返回。
    /// </summary>
    /// <param name="_child"></param>
    /// <returns></returns>
    public static Transform FindOutParent(Transform _child)
    {
        if (_child.parent == null)
            return _child;
        else
            return FindOutParent(_child.parent);
    }
    /// <summary>
    /// 打乱列表顺序。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_sourceList">源数据列表</param>
    /// <returns></returns>
    public static List<T> UpsetList<T>(List<T> _sourceList)
    {
        List<T> _resultList = new List<T>();
        List<int> _id = new List<int>();
        int _index;

        for (int i = 0; i < _sourceList.Count; i++)
        {
            _id.Add(i);
        }

        for (int i = 0; i < _sourceList.Count; i++)
        {
            _index = UnityEngine.Random.Range(0, _id.Count);
            _resultList.Add(_sourceList[_id[_index]]);
            _id.Remove(_id[_index]);
        }
        return _resultList;
    }
    /// <summary>
    /// 异步加载场景并将加载进度同步到一个 Slider 上。如果场景太小，异步加载速度很快，会自动根据 _speed 保证进度以
    /// 一种比较平稳的方式增长，不会一闪而过。
    /// </summary>
    /// <param name="_progressSlider">需要同步的 Slider</param>
    /// <param name="_sceneName">场景名称</param>
    /// <param name="_speed">加载速度。只会在异步加载速度很快时生效</param>
    /// <returns></returns>
    public static IEnumerator LoadSceneAsync(Slider _progressSlider, string _sceneName, float _speed = 1f)
    {
        if (_progressSlider == null)
        {
            Debug.LogError("_progressSlider 为 Null");
            yield break;
        }

        AsyncOperation _asyncOperation = SceneManager.LoadSceneAsync(_sceneName);
        _asyncOperation.allowSceneActivation = false;
        yield return null;

        float _targetValue = 0;
        _progressSlider.value = _targetValue;
        while (true)
        {
            _targetValue += Time.deltaTime * _speed;
            if (_targetValue >= _asyncOperation.progress && _asyncOperation.progress < 0.9f)
            {
                _progressSlider.value = _asyncOperation.progress;
                _targetValue = _asyncOperation.progress;
            }
            else
            {
                _progressSlider.value = _targetValue;
            }
            yield return null;

            if (_progressSlider.value >= 1)
            {
                _asyncOperation.allowSceneActivation = true;
                yield break;
            }
        }
    }
    /// <summary>
    /// 求一条向量与一个面的夹角
    /// </summary>
    /// <param name="_vector">向量</param>
    /// <param name="_planeNormal">平面法向量</param>
    /// <returns>返回向量与平面到夹角，且返回值一定为锐角。与平面法向量相反的一边，返回值为负值。</returns>
    public static float DeflectionAngle(Vector3 _vector, Vector3 _planeNormal)
    {
        float _result = Vector3.Dot(_planeNormal, _vector);
        float radians = Mathf.Acos(_result);
        return radians * Mathf.Rad2Deg - 90;
    }
    /// <summary>
    /// 求一个向量 _origin 绕着 _axis 轴旋转 _angle 度后的向量。
    /// </summary>
    /// <param name="_origin">初始向量</param>
    /// <param name="_axis">旋转轴</param>
    /// <param name="_angle">旋转角度</param>
    /// <returns></returns>
    public static Vector3 Vector3Rotate(Vector3 _origin, Vector3 _axis, float _angle)
    {
        return Quaternion.AngleAxis(_angle, _axis) * _origin;
    }
    /// <summary>
    /// 求一个向量 _origin 分别绕着 _angle 的 Z 轴旋转 _angle.z 度，_angle 的 X 轴旋转 _angle.x 度，
    /// _angle 的 Y 轴旋转 _angle.y 度后的向量。
    /// </summary>
    /// <param name="_origin"></param>
    /// <param name="_angle"></param>
    /// <returns></returns>
    public static Vector3 Vector3Rotate(Vector3 _origin, Vector3 _angle)
    {
        return Quaternion.Euler(_angle) * _origin;
    }
    #endregion
}
