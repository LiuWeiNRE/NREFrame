/*
 * 工具类：读取XML文件
 */
using System.IO;
using System.Xml;
using UnityEngine;

public class XMLOperate
{
    /// <summary>
    /// 读取 XML 文件指定子节点数据
    /// </summary>
    /// <param name="_path">文件读取路径</param>
    /// <param name="_xmlNodes">节点数组</param>
    /// <returns></returns>
    public static string Read(string _path, string[] _xmlNodes)
    {
        string _innerstr = "";//返回值
        XmlDocument _xmlDoc = new XmlDocument();
        XmlReaderSettings _set = new XmlReaderSettings();
        _set.IgnoreComments = true;//忽略注释
        XmlReader _xmlReader = null;
        //判断文件是否存在
        if (File.Exists(_path))
        {
            _xmlReader = XmlReader.Create(_path, _set);
            _xmlDoc.Load(_xmlReader);
        }
        else
        {
            Debug.LogError("目标文件不存在，请检查路径是否有误");
            return null;
        }
        int _nodeNum = _xmlNodes.Length;
        int _counter = 1;
        XmlNodeList _nodelist = _xmlDoc.SelectSingleNode(_xmlNodes[0]).ChildNodes;
        if (_nodelist == null)
        {
            if (_xmlReader != null)
            {
                _xmlReader.Close();
            }
            return _innerstr;
        }
        while (_counter < _nodeNum)
        {
            bool _check = false;
            foreach (XmlNode _element in _nodelist)
            {
                if (_element.Name == _xmlNodes[_counter])
                {
                    _check = true;
                    _counter++;
                    if (_counter >= _nodeNum)
                    {
                        _innerstr = _element.InnerText;
                        _nodelist = null;
                        break;
                    }
                    _nodelist = _element.ChildNodes;
                    break;
                }
            }
            if (!_check)
            {
                Debug.LogError("请检查节点信息是否输入正确！");
                break;
            }
        }

        if (_xmlReader != null)
        {
            _xmlReader.Close();
        }

        return _innerstr;
    }
    /// <summary>
    /// 读取指定节点下的所有子节点数据
    /// </summary>
    /// <param name="_path">文件读取路径</param>
    /// <param name="_xmlNodes">节点数组</param>
    /// <returns></returns>
    public static string[] ReadItems(string _path, string[] _xmlNodes)
    {
        string[] _innerstrs = null;//返回值
        XmlDocument _xmlDoc = new XmlDocument();
        XmlReaderSettings _set = new XmlReaderSettings();
        _set.IgnoreComments = true;
        XmlReader _xmlReader = null;
        //判断文件是否存在
        if (File.Exists(_path))
        {
            _xmlReader = XmlReader.Create(_path, _set);
            _xmlDoc.Load(_xmlReader);
        }
        else
        {
            Debug.LogError("目标文件不存在，请检查路径是否有误");
            return null;
        }
        int _nodeNum = _xmlNodes.Length;
        int _counter = 1;
        XmlNodeList _nodelist = _xmlDoc.SelectSingleNode(_xmlNodes[0]).ChildNodes;
        if (_nodelist == null)
        {
            if (_xmlReader != null)
            {
                _xmlReader.Close();
            }
            return _innerstrs;
        }
        while (_counter < _nodeNum)
        {
            bool _check = false;
            foreach (XmlNode _element in _nodelist)
            {
                if (_element.Name == _xmlNodes[_counter])
                {
                    _check = true;
                    _counter++;
                    if (_counter >= _nodeNum)
                    {
                        _innerstrs = new string[_element.ChildNodes.Count];
                        int _tc = 0;
                        foreach (XmlNode _item in _element.ChildNodes)
                        {
                            _innerstrs[_tc] = _item.InnerText;
                            _tc++;
                        }
                        _nodelist = null;
                        break;
                    }
                    _nodelist = _element.ChildNodes;
                    break;
                }
            }
            if (!_check)
            {
                Debug.LogError("请检查节点信息是否输入正确！");
                break;
            }
        }

        if (_xmlReader != null)
        {
            _xmlReader.Close();
        }
        return _innerstrs;
    }
    /// <summary>
    /// 向 XML 文件指定子节点写入数据，并覆盖旧数据。
    /// </summary>
    /// <param name="_path">文件读取路径</param>
    /// <param name="_xmlNodes">节点数据</param>
    /// <param name="_data">需要写入的数据</param>
    /// <returns></returns>
    public static bool Write(string _path, string[] _xmlNodes, string _data)
    {
        XmlDocument _xmlDoc = new XmlDocument();
        XmlReaderSettings _set = new XmlReaderSettings();
        _set.IgnoreComments = true;//忽略注释
        XmlReader _xmlReader = null;
        //判断文件是否存在
        if (File.Exists(_path))
        {
            _xmlReader = XmlReader.Create(_path, _set);
            _xmlDoc.Load(_xmlReader);
        }
        else
        {
            Debug.LogError("目标文件不存在，请检查路径是否有误");
            return false;
        }
        int _nodeNum = _xmlNodes.Length;
        int _counter = 1;
        XmlNodeList _nodelist = _xmlDoc.SelectSingleNode(_xmlNodes[0]).ChildNodes;
        if (_nodelist == null)
        {
            Debug.LogError("获取节点信息失败，请检查" + _path + "格式是否正确");
            if (_xmlReader != null)
            {
                _xmlReader.Close();
            }
            return false;
        }
        while (_counter < _nodeNum)
        {
            bool _check = false;
            foreach (XmlNode _element in _nodelist)
            {
                if (_element.Name == _xmlNodes[_counter])
                {
                    _check = true;
                    _counter++;
                    if (_counter >= _nodeNum)
                    {
                        _element.InnerText = _data;
                        if (_xmlReader != null)
                        {
                            _xmlReader.Close();
                        }
                        _xmlDoc.Save(_path);
                        _nodelist = null;
                        return true;
                    }
                    _nodelist = _element.ChildNodes;
                    break;
                }
            }
            if (!_check)
            {
                Debug.LogError("请检查节点信息是否输入正确！");
                if (_xmlReader != null)
                {
                    _xmlReader.Close();
                }
                return false;
            }
        }
        if (_xmlReader != null)
        {
            _xmlReader.Close();
        }
        return false;
    }
    /// <summary>
    /// 向 XML 文件指定子节点下新增字节点，并写入数据。
    /// </summary>
    /// <param name="_path">文件读取路径</param>
    /// <param name="_xmlNodes">节点数组</param>
    /// <param name="_nodeName">新增节点名称</param>
    /// <param name="_data">新增数据</param>
    /// <returns></returns>
    public static bool AddNode(string _path, string[] _xmlNodes, string _nodeName, string _data)
    {
        XmlDocument _xmlDoc = new XmlDocument();
        XmlReaderSettings _set = new XmlReaderSettings();
        _set.IgnoreComments = true;//忽略注释
        XmlReader _xmlReader = null;
        //判断文件是否存在
        if (File.Exists(_path))
        {
            _xmlReader = XmlReader.Create(_path, _set);
            _xmlDoc.Load(_xmlReader);
        }
        else
        {
            Debug.LogError("目标文件不存在，请检查路径是否有误");
            return false;
        }
        int _nodeNum = _xmlNodes.Length;
        int _counter = 1;
        XmlNodeList _nodelist = _xmlDoc.SelectSingleNode(_xmlNodes[0]).ChildNodes;
        if (_nodelist == null)
        {
            Debug.LogError("获取节点信息失败，请检查" + _path + "格式是否正确");
            if (_xmlReader != null)
            {
                _xmlReader.Close();
            }
            return false;
        }
        while (_counter < _nodeNum)
        {
            bool _check = false;
            foreach (XmlNode _element in _nodelist)
            {
                if (_element.Name == _xmlNodes[_counter])
                {
                    _check = true;
                    _counter++;
                    if (_counter >= _nodeNum)
                    {
                        XmlElement _node = _xmlDoc.CreateElement(_nodeName);
                        _node.InnerText = _data;
                        _element.AppendChild(_node);
                        if (_xmlReader != null)
                        {
                            _xmlReader.Close();
                        }
                        _xmlDoc.Save(_path);
                        _nodelist = null;
                        return true;
                    }
                    _nodelist = _element.ChildNodes;
                    break;
                }
            }
            if (!_check)
            {
                Debug.LogError("请检查节点信息是否输入正确！");
                if (_xmlReader != null)
                {
                    _xmlReader.Close();
                }
                return false;
            }
        }
        if (_xmlReader != null)
        {
            _xmlReader.Close();
        }
        return false;
    }
}
