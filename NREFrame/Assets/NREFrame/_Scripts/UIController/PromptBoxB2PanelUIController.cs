/*
 * B2类型提示框控制器。B2类型：有一个确定按钮和一个取消按钮。
*/
using System;
using UnityEngine.UI;

public partial class PromptBoxB2PanelUIController : UIControllerBase 
{
    #region -- 变量定义
    private Action okButtonOnClick;
    private Text prompt;
    #endregion

    #region -- 系统函数
    private void Start()
    {
        Init();
        InitUIEvent();
    }
    #endregion

    #region -- 自定义函数
    protected override void Init()
    {
        if (prompt == null)
        {
            prompt = this.PromptText.GetComponent<Text>();
        }
    }
    protected override void InitUIEvent()
    {
        OKButton.GetComponent<Button>().onClick.AddListener(OKButtonOnClick);
        CancelButton.GetComponent<Button>().onClick.AddListener(this.Destroy);
    }

    /// <summary>
    /// 设置提示信息
    /// </summary>
    /// <param name="_message"></param>
    public void SetPromptMessage(string _message, Action _OKButtonEvent = null)
    {
        if (prompt == null)
        {
            prompt = this.PromptText.GetComponent<Text>();
        }
        prompt.text = _message;
        okButtonOnClick += _OKButtonEvent;
    }

    /// <summary>
    /// 确认按钮响应事件
    /// </summary>
    private void OKButtonOnClick()
    {
        okButtonOnClick += this.Destroy;
        okButtonOnClick();
    }
    #endregion
}