/*
 * 工具类：内存池。
 * 
 * 注释中的说的预制体名字，其实是指存放预制体的路径。
 */
using UnityEngine;
using System.Collections.Generic;
using System;

public class PoolManager
{
    private static Dictionary<string, GameObject> _prefabs;//预制体集合
    private static Dictionary<string, GameObject> prefabs
    {
        get
        {
            if (_prefabs == null)
            {
                _prefabs = new Dictionary<string, GameObject>();
            }
            return _prefabs;
        }
    }

    private static Dictionary<string, ObjPool> _poolObjs;//内存池集合
    private static Dictionary<string, ObjPool> poolObjs
    {
        get
        {
            if (_poolObjs == null)
            {
                _poolObjs = new Dictionary<string, ObjPool>();
            }
            return _poolObjs;
        }
    }

    /// <summary>
    /// 通过预制体的名字查找预制体。
    /// </summary>
    /// <param name="name">预制体的名字，也是 prefabs 的键</param>
    /// <returns>返回找到的预制体</returns>
    public static GameObject GetPrefab(string name)
    {
        GameObject go = null;
        if (prefabs.ContainsKey(name))//查找集合中是否存在这个键
        {
            go = prefabs[name];
        }
        else//如果不存在
        {
            go = Resources.Load<GameObject>(name);//在 Resources 文件夹中读取预制体。
            if (go == null)//如果 Resources 文件夹中读取不到名为 name 的预制体。
            {
                throw new Exception("不存在该预设");
            }
            prefabs.Add(name, go);////将找到的预制体添加到 prefabs 中
        }
        return go;
    }

    /// <summary>
    /// 通过预制体的名字克隆一个预制体并返回。
    /// </summary>
    /// <param name="name">预制体的名字</param>
    /// <returns></returns>
    public static GameObject CreatObj(string name)
    {
        ObjPool objPool = GetObjPool(name);
        GameObject obj = objPool.GetUnUesdObj(name);
        return obj;
    }
    /// <summary>
    /// 实例化一个 ObjPool 对象并添加到 poolObjs 中。
    /// </summary>
    /// <param name="name">预制体的名字，作为 poolObjs 的键</param>
    private static void AddObjPool(string name)
    {
        ObjPool objPool = new ObjPool();
        poolObjs.Add(name, objPool);
    }
    /// <summary>
    /// 通过预制体的名字获取 poolObjs 的 ObjPool 对象并返回。
    /// </summary>
    /// <param name="name">预制体的名字，作为 poolObjs 的键</param>
    /// <returns>返回获取到的 ObjPool 对象</returns>
    public static ObjPool GetObjPool(String name)
    {
        if (poolObjs.ContainsKey(name) == false)//如果 poolObjs 中获取不到对应的对象
        {
            AddObjPool(name);
        }
        return poolObjs[name];
    }

    /// <summary>
    /// 关闭预制体
    /// </summary>
    /// <param name="go">需要关闭的预制体对象</param>
    public static void Destory(GameObject go)
    {
        go.SetActive(false);
    }
}

public class ObjPool
{
    //存储某个预制体的所有克隆体
    private List<GameObject> _objList;
    public List<GameObject> objList
    {
        get
        {
            if (_objList == null)
            {
                _objList = new List<GameObject>();
            }
            return _objList;
        }
    }

    /// <summary>
    /// 获取名为 name 的预制体的克隆体中未使用的对象并返回。
    /// </summary>
    /// <param name="name">预制体的名字</param>
    public GameObject GetUnUesdObj(string name)
    {
        GameObject go = null;
        for (int i = 0; i < objList.Count; i++)//遍历克隆体列表
        {
            if (objList[i].activeSelf == false)//如果找到未使用的克隆体
            {
                objList[i].SetActive(true);
                go = objList[i];
                break;
            }
        }
        if (go == null)//如果不存在未使用的克隆体
        {
            go = GameObject.Instantiate(PoolManager.GetPrefab(name));
            ObjPool objPool = PoolManager.GetObjPool(name);
            objPool.objList.Add(go);
        }
        return go;
    }
}
