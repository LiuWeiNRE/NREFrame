/*
 * 版本号：
 * Modify：修改日期
 * Modifier：刘伟
 * Modify Reason：修改原因
 * Modify Content：修改内容说明
*/

using System.Collections.Generic;
using UnityEngine;

public class APPEngine : MonoBehaviour
{
    #region -- 变量定义
    private static APPEngine instance;

    private static Dictionary<string, UIRootBase> uiRootDict = new Dictionary<string, UIRootBase>();
    private static Dictionary<string, GameObjectRootBase> gameObjectRootDict = new Dictionary<string, GameObjectRootBase>();
    #endregion

    #region -- 系统函数
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        Init();
    }
    private void OnDestroy()
    {
        instance = null;
    }
    #endregion

    #region -- 自定义函数
    public static APPEngine Instance
    {
        get { return instance; }
    }
    public static Dictionary<string, UIRootBase> UIRootDict
    {
        get { return uiRootDict; }
    }
    public static Dictionary<string, GameObjectRootBase> GameObjectRootDict
    {
        get { return gameObjectRootDict; }
    }

    private void Init()
    { }
    public static T CreateUIRoot<T>(string _uiName, UILayer _layer) where T : UIRootBase
    {
        GameObject _go = GameResourcesManager.LoadUIPrefab(_uiName);
        //_go.transform.SetParent(GetUILayer(_layer).transform, false);
        T _uiRoot = _go.GetComponent<T>();
        _uiRoot.rootID = _uiName;
        UIRootDict.Add(_uiRoot.rootID, _uiRoot);
        return _uiRoot;
    }
    public static T CreateGameObjectRoot<T>(string _gameObjectName) where T : GameObjectRootBase
    {
        GameObject _go = GameResourcesManager.LoadUIPrefab(_gameObjectName);
        T _gameObjectRoot = _go.GetComponent<T>();
        _gameObjectRoot.rootID = _gameObjectName;
        GameObjectRootDict.Add(_gameObjectRoot.rootID, _gameObjectRoot);
        return _gameObjectRoot;
    }
    public static T CreateGameObjectRoot<T>(string _gameObjectName, Transform _parent) where T : GameObjectRootBase
    {
        GameObject _go = GameResourcesManager.LoadUIPrefab(_gameObjectName);
        _go.transform.SetParent(_parent, false);
        T _gameObjectRoot = _go.GetComponent<T>();
        _gameObjectRoot.rootID = _gameObjectName;
        GameObjectRootDict.Add(_gameObjectRoot.rootID, _gameObjectRoot);
        return _gameObjectRoot;
    }
    public static T GetUIRoot<T>(string _uiName) where T : UIRootBase
    {
        if (UIRootDict.ContainsKey(_uiName))
        {
            return UIRootDict[_uiName] as T;
        }
        return null;
    }
    public static T GetGameObjectRoot<T>(string _gameObjectName) where T : GameObjectRootBase
    {
        if (GameObjectRootDict.ContainsKey(_gameObjectName))
        {
            return GameObjectRootDict[_gameObjectName] as T;
        }
        return null;
    }
    #endregion
}
