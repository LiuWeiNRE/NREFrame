/*
 * 加载各种资源
 */
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameResourcesManager
{
    #region --变量定义
    /// <summary>
    /// UI预制体的路径
    /// </summary>
    public const string UI_PATH = "Prefabs/UI/";
    /// <summary>
    /// 材质球的路径
    /// </summary>
    /// </summary>
    public const string MATERIAL_PATH = "Materials/";
    #endregion

    #region --自定义函数
    /// <summary>
    /// 从Resources文件夹下加载UI预制体
    /// </summary>
    /// <param name="_name">预制体名称</param>
    /// <param name="_parent">父物体</param>
    /// <returns></returns>
    public static GameObject LoadUIPrefab(string _name, Transform _parent = null)
    {
        GameObject _uiPrefab = ResourcesManager.Load<GameObject>(UI_PATH + _name);
        return GameObject.Instantiate<GameObject>(_uiPrefab, _parent);
    }
    /// <summary>
    /// 从Resources文件夹下加载材质球
    /// </summary>
    /// <param name="_name">材质球名称</param>
    /// <returns></returns>
    public static Material LoadMaterial(string _name)
    {
        Material _material = ResourcesManager.Load<Material>(MATERIAL_PATH + _name);
        return _material;
    }

    #region -- 图片加载
    public delegate void TextureOperate(Texture _texture);//图片操作委托
    /// <summary>
    /// 使用 WWW 加载图片，并赋值给 _rawImage
    /// </summary>
    /// <param name="_url">图片地址</param>
    /// <param name="_rawImage"></param>
    /// <returns></returns>
    public static IEnumerator LoadTexture2DByWWW(string _url, RawImage _rawImage)
    {
        WWW _www = new WWW(_url);
        yield return _www;
        if (_www.error == null)
        {
            _rawImage.texture = _www.texture;
        }
        else
        {
            Debug.LogError(_www.error);
        }
    }
    /// <summary>
    /// 使用 WWW 加载图片，并赋值给 _meshRenderer 的 shader 中的 _protertiesName 变量
    /// </summary>
    /// <param name="_url">图片地址</param>
    /// <param name="_meshRenderer"></param>
    /// <param name="_protertiesName">shader 变量名</param>
    /// <returns></returns>
    public static IEnumerator LoadTexture2DByWWW(string _url, MeshRenderer _meshRenderer, string _protertiesName)
    {
        WWW _www = new WWW(_url);
        yield return _www;
        if (_www.error == null)
        {
            _meshRenderer.sharedMaterial.SetTexture(_protertiesName, _www.texture);
        }
        else
        {
            Debug.LogError(_www.error);
        }
    }
    /// <summary>
    /// 使用 WWW 加载图片，并将图片传递给 _textureOperage 委托方法
    /// </summary>
    /// <param name="_url">图片地址</param>
    /// <param name="_textureOperage"></param>
    /// <returns></returns>
    public static IEnumerator LoadTexture2DByWWW(string _url, TextureOperate _textureOperage)
    {
        WWW _www = new WWW(_url);
        yield return _www;
        if (_www.error == null)
        {
            _textureOperage(_www.texture);
        }
        else
        {
            Debug.LogError(_www.error);
        }
    }
    /// <summary>
    /// 使用 WWW 加载图片，并将图片转换成 Sprite 类型赋值给 _image
    /// </summary>
    /// <param name="_url">图片地址</param>
    /// <param name="_image"></param>
    /// <returns></returns>
    public static IEnumerator LoadSpriteByWWW(string _url, Image _image)
    {
        WWW _www = new WWW(_url);
        yield return _www;
        if (_www.error == null)
        {
            _image.sprite = Sprite.Create(_www.texture, new Rect(0, 0, _www.texture.width, _www.texture.height), new Vector2(0.5f, 0.5f));
        }
        else
        {
            Debug.LogError(_www.error);
        }
    }
    /// <summary>
    /// 使用 IO 流加载图片，并返回。
    /// </summary>
    /// <param name="_url">图片地址</param>
    /// <returns></returns>
    public static Texture2D LoadTexture2DByIO(string _url)
    {
        //创建文件读取流
        FileStream _fileStream = new FileStream(_url, FileMode.Open, FileAccess.Read);
        _fileStream.Seek(0, SeekOrigin.Begin);
        //创建文件长度缓冲区
        byte[] _bytes = new byte[_fileStream.Length];
        _fileStream.Read(_bytes, 0, (int)_fileStream.Length);
        _fileStream.Close();
        _fileStream.Dispose();
        //创建Texture
        Texture2D _texture2D = new Texture2D(1, 1);
        _texture2D.LoadImage(_bytes);
        return _texture2D;
    }
    /// <summary>
    /// 使用 IO 流加载图片，并将图片转换成 Sprite 类型返回
    /// </summary>
    /// <param name="_url">图片地址</param>
    /// <returns></returns>
    public static Sprite LoadSpriteByIO(string _url)
    {
        //创建文件读取流
        FileStream _fileStream = new FileStream(_url, FileMode.Open, FileAccess.Read);
        _fileStream.Seek(0, SeekOrigin.Begin);
        //创建文件长度缓冲区
        byte[] _bytes = new byte[_fileStream.Length];
        _fileStream.Read(_bytes, 0, (int)_fileStream.Length);
        _fileStream.Close();
        _fileStream.Dispose();
        //创建Texture
        Texture2D _texture2D = new Texture2D(1, 1);
        _texture2D.LoadImage(_bytes);
        //将 Texture2D 转为 Sprite 类型
        Sprite _sprite = Sprite.Create(_texture2D, new Rect(0, 0, _texture2D.width, _texture2D.height), new Vector2(0.5f, 0.5f));
        return _sprite;
    }
    /// <summary>
    /// 使用 IO 流加载图片，并转换成 Base64 格式的字符串数据返回。
    /// </summary>
    /// <param name="_filePath">图片地址</param>
    /// <returns></returns>
    public static string LoadTextureToString(string _filePath)
    {
        Stream _stream = null;
        string _textureString = string.Empty;
        byte[] _buffer = null;

        try
        {
            _stream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            _buffer = new Byte[_stream.Length];
            _stream.Read(_buffer, 0, _buffer.Length);
        }
        catch (Exception _e)
        {
            Debug.Log(_e.Message);
        }
        finally
        {
            _stream.Flush();
            _stream.Close();
        }

        _textureString = Convert.ToBase64String(_buffer);
        return _textureString;
    }
    /// <summary>
    /// 将 Base64 格式的字符串数据转换为 Texture2D 图片文件返回。
    /// </summary>
    /// <param name="_textureString">由图片转换成的 Base64 格式的字符串数据</param>
    /// <returns></returns>
    public static Texture2D TextureStringToTexture2D(string _textureString)
    {
        byte[] _buffer =  Convert.FromBase64String(_textureString);
        Texture2D _texture2D = new Texture2D(1, 1);
        _texture2D.LoadImage(_buffer);
        Resources.UnloadUnusedAssets(); //一定要清理游离资源。
        return _texture2D;
    }
    #endregion

    #endregion
}