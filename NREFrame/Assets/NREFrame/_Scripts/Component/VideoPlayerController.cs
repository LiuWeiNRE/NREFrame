/*
 * 基于 VideoPlayer 封装的一个视频播放控制器。（API不是很全，还有待完善。）
 */
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class VideoPlayerController : MonoBehaviour
{
    #region -- 变量定义
    private VideoPlayer videoPlayer;
    #endregion

    #region -- 系统函数
    private void Awake()
    {
        Init();
    }
    #endregion

    #region -- 自定义函数
    private void Init()
    {
        GetVideoPlayer();
    }
    private void GetVideoPlayer()
    {
        videoPlayer = this.GetComponent<VideoPlayer>();
    }

    /// <summary>
    /// 视频播放器
    /// </summary>
    public VideoPlayer VideoPlayer
    {
        get { return videoPlayer; }
    }
    /// <summary>
    /// 视频总时长。单位：秒。
    /// </summary>
    public double TotalDuration
    {
        get
        {
            if (videoPlayer == null)
            {
                GetVideoPlayer();
            }
            if (videoPlayer.clip == null)
            {
                //总时长=总帧数/帧率
                return videoPlayer.frameCount / videoPlayer.frameRate;
            }
            else
            {
                return videoPlayer.clip.length;
            }
        }
    }
    /// <summary>
    /// 格式化后的视频总时长。格式：00：00：00。
    /// </summary>
    public string FormatTotalDuration
    {
        get
        {
            int _hour = (int)TotalDuration / 3600;
            int _minute = (int)(TotalDuration - _hour * 3600) / 60;
            int _second = (int)(TotalDuration - _hour * 3600 - _minute * 60);
            return string.Format("{0:D2}:{1:D2}:{2:D2}", _hour, _minute, _second);
        }
    }
    /// <summary>
    /// 当前播放时长
    /// </summary>
    public double CurrentDuration
    {
        get { return videoPlayer.time; }
        set { videoPlayer.time = value; }
    }
    /// <summary>
    /// 格式化后的当前播放时长。格式：00：00：00。
    /// </summary>
    public string FormatCurrentDuration
    {
        get
        {
            int _hour = (int)CurrentDuration / 3600;
            int _minute = (int)(CurrentDuration - _hour * 3600) / 60;
            int _second = (int)(CurrentDuration - _hour * 3600 - _minute * 60);
            return string.Format("{0:D2}:{1:D2}:{2:D2}", _hour, _minute, _second);
        }
    }
    /// <summary>
    /// 是否正在播放。
    /// </summary>
    public bool IsPlaying
    {
        get { return videoPlayer.isPlaying; }
    }

    /// <summary>
    /// 播放
    /// </summary>
    public void Play()
    {
        videoPlayer.Play();
    }
    /// <summary>
    /// 暂停
    /// </summary>
    public void Pause()
    {
        videoPlayer.Pause();
    }
    /// <summary>
    /// 设置视频源
    /// </summary>
    /// <param name="_videoUrl">视频路径</param>
    /// <param name="_isPlay">是否播放。默认播放。</param>
    public void SetVideoSource(string _videoUrl, bool _isPlay = true)
    {
        if (videoPlayer == null)
        {
            GetVideoPlayer();
        }
        videoPlayer.url = _videoUrl;
        if (_isPlay)
        {
            Play();
        }
    }
    #endregion
}