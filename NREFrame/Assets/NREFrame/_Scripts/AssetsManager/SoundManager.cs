/*
 * 工具类：声音管理器 V1.0.20181205
 * 注：本版本只适用于从Resources文件夹下加载声音文件。
 */
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 声音管理器
/// </summary>
public class SoundManager
{
    #region --变量定义
    private static GameObject soundManagerRoot;//声音管理器根节点，所有播放声音的声源节点，都是在这个节点下。

    //string：声音加载路径
    private static Dictionary<string, AudioSource> musics;//管理所有的音乐（例如：背景音乐）
    private static Dictionary<string, AudioSource> soundEffects;//管理所有的音效（例如：按钮声音）

    private static bool musicIsMute = false;//音乐是否静音
    private static bool effectIsMute = false;//音效是否静音
    #endregion

    #region --自定义函数
    /// <summary>
    /// 初始化声音管理器
    /// </summary>
    public static void Init()
    {
        //创建声音管理器根节点，并设置为跳转场景不删除
        soundManagerRoot = new GameObject("SoundManagerRoot");
        GameObject.DontDestroyOnLoad(soundManagerRoot);

        musics = new Dictionary<string, AudioSource>();
        soundEffects = new Dictionary<string, AudioSource>();

        // 从本地获取音乐和音效是否设置为静音
        if (PlayerPrefs.HasKey("musicMute"))
        {
            int _value = PlayerPrefs.GetInt("musicMute");
            musicIsMute = (_value == 1);
        }
        if (PlayerPrefs.HasKey("effectMute"))
        {
            int _value = PlayerPrefs.GetInt("effectMute");
            effectIsMute = (_value == 1);
        }
    }
    /// <summary>
    /// 播放音乐
    /// </summary>
    /// <param name="_url">音乐路径</param>
    /// <param name="_isLoop">是否循环，默认 ture</param>
    public static void PlayMusic(string _url, bool _isLoop = true)
    {
        AudioSource _audioSource = null;
        if (musics.ContainsKey(_url))
        {
            _audioSource = musics[_url];
        }
        else
        {
            GameObject _go = new GameObject(_url);
            _go.transform.parent = soundManagerRoot.transform;
            _audioSource = _go.AddComponent<AudioSource>();
            AudioClip _clip = Resources.Load<AudioClip>(_url);
            _audioSource.clip = _clip;
            _audioSource.loop = _isLoop;
            _audioSource.playOnAwake = true;
            _audioSource.spatialBlend = 0.0f;
            musics.Add(_url, _audioSource);
        }
        _audioSource.mute = musicIsMute;
        if (!_audioSource.mute)
        {
            _audioSource.enabled = true;
            _audioSource.Play();
        }
    }
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="_url">音效路径</param>
    /// <param name="_isLoop">是否循环，默认 false</param>
    public static void PlaySoundEffect(string _url, bool _isLoop = false)
    {
        AudioSource _audioSource = null;
        if (soundEffects.ContainsKey(_url))
        {
            _audioSource = soundEffects[_url];
        }
        else
        {
            GameObject _go = new GameObject(_url);
            _go.transform.parent = soundManagerRoot.transform;
            _audioSource = _go.AddComponent<AudioSource>();
            AudioClip _clip = Resources.Load<AudioClip>(_url);
            _audioSource.clip = _clip;
            _audioSource.loop = _isLoop;
            _audioSource.playOnAwake = true;
            _audioSource.spatialBlend = 0.0f;
            soundEffects.Add(_url, _audioSource);
        }
        _audioSource.mute = effectIsMute;
        if (!_audioSource.mute)
        {
            _audioSource.enabled = true;
            _audioSource.Play(); 
        }
    }
    /// <summary>
    /// 播放3D音效
    /// </summary>
    /// <param name="_url">音效路径</param>
    /// <param name="_pos">音效位置</param>
    /// <param name="_isLoop">是否循环，默认 false</param>
    public static void PlayEffect3D(string _url, Vector3 _pos, bool _isLoop = false)
    {
        AudioSource _audioSource = null;
        if (soundEffects.ContainsKey(_url))
        {
            _audioSource = soundEffects[_url];
        }
        else
        {
            GameObject _go = new GameObject(_url);
            _go.transform.parent = soundManagerRoot.transform;
            _go.transform.position = _pos;
            _audioSource = _go.AddComponent<AudioSource>();
            AudioClip _clip = Resources.Load<AudioClip>(_url);
            _audioSource.clip = _clip;
            _audioSource.loop = _isLoop;
            _audioSource.playOnAwake = true;
            _audioSource.spatialBlend = 1.0f;
            soundEffects.Add(_url, _audioSource);
        }
        _audioSource.mute = effectIsMute;
        if (!_audioSource.mute)
        {
            _audioSource.enabled = true;
            _audioSource.Play(); 
        }
    }
    /// <summary>
    /// 关闭音乐
    /// </summary>
    /// <param name="_url">音乐路径</param>
    public static void StopMusic(string _url)
    {
        AudioSource _audioSource = null;
        if (!musics.ContainsKey(_url))
        {
            return;
        }
        _audioSource = musics[_url];
        _audioSource.Stop();
    }
    /// <summary>
    /// 关闭音效
    /// </summary>
    /// <param name="_url">音效路径</param>
    public static void StopSoundEffect(string _url)
    {
        AudioSource _audioSource = null;
        if (!soundEffects.ContainsKey(_url))
        {
            return;
        }
        _audioSource = soundEffects[_url];
        _audioSource.Stop();
    }
    /// <summary>
    /// 关闭所有音乐
    /// </summary>
    public static void StopAllMusic()
    {
        foreach (AudioSource _audioSource in musics.Values)
        {
            _audioSource.Stop();
        }
    }
    /// <summary>
    /// 关闭所有音效
    /// </summary>
    public static void StopAllSoundEffect()
    {
        foreach (AudioSource _audioSource in soundEffects.Values)
        {
            _audioSource.Stop();
        }
    }
    /// <summary>
    /// 清除音乐。删除绑定播放该音乐的节点
    /// </summary>
    /// <param name="_url"></param>
    public static void ClearMusic(string _url)
    {
        AudioSource _audioSource = null;
        if (!musics.ContainsKey(_url))
        {
            return;
        }
        _audioSource = musics[_url];
        musics[_url] = null;
        GameObject.Destroy(_audioSource.gameObject);
    }
    /// <summary>
    /// 清除音效。删除绑定播放该音效的节点
    /// </summary>
    /// <param name="_url"></param>
    public static void ClearSoundEffect(string _url)
    {
        AudioSource _audioSource = null;
        if (!soundEffects.ContainsKey(_url))
        {
            return;
        }
        _audioSource = soundEffects[_url];
        soundEffects[_url] = null;
        GameObject.Destroy(_audioSource.gameObject);
    }
    /// <summary>
    /// 设置音乐是否静音
    /// </summary>
    public static bool MusicMute
    {
        get
        {
            return musicIsMute;
        }
        set
        {
            if (musicIsMute != value)
            {
                musicIsMute = value;
                int _mute = (musicIsMute) ? 1 : 0;
                PlayerPrefs.SetInt("musicMute", _mute);
                foreach (AudioSource _audioSource in musics.Values)
                {
                    _audioSource.mute = musicIsMute;
                }
            }
        }
    }
    /// <summary>
    /// 设置音效是否静音
    /// </summary>
    public static bool SoundEffectMute
    {
        get
        {
            return effectIsMute;
        }
        set
        {
            if (effectIsMute != value)
            {
                effectIsMute = value;
                int _mute = (effectIsMute) ? 1 : 0;
                PlayerPrefs.SetInt("effectMute", _mute);
                foreach (AudioSource _audioSource in soundEffects.Values)
                {
                    _audioSource.mute = effectIsMute;
                }
            }
        }
    }
    /// <summary>
    /// 将所有没有播放的音乐或者音效的节点的AudioSource组件禁用
    /// </summary>
    public static void DisableOverAudio()
    {
        foreach (AudioSource _audioSource in soundEffects.Values)
        {
            if (!_audioSource.isPlaying)
            {
                _audioSource.enabled = false;
            }
        }
        foreach (AudioSource _audioSource in musics.Values)
        {
            if (!_audioSource.isPlaying)
            {
                _audioSource.enabled = false;
            }
        }
    }
    #endregion
}