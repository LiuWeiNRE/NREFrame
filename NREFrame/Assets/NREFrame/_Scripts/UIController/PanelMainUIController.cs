/*
 * 主面板（层级父物体）控制器。
 */
using System;
using UnityEngine;

public partial class PanelMainUIController : UIControllerBase
{
    #region --变量定义
    private static PanelMainUIController instance;
    private static readonly object lockHelper = new object();
    #endregion

    #region --系统函数
    private void OnDestroy()
    {
        instance = null;
    }
    #endregion

    #region --属性
    public static PanelMainUIController Instance
    {
        get
        {
            if (instance == null)
            {
                lock (lockHelper)//单例有出现2个UIMain的低概率
                {
                    if (instance == null)
                    {
                        GameObject _uiMain = GameObject.Find(UIControllerConst.CANVAS_NAME);
                        if (_uiMain == null)
                        {
                            _uiMain = GameObject.Find(UIControllerConst.CANVAS_NAME + "(Clone)");
                        }
                        if (_uiMain == null)
                        {
                            _uiMain = GameResourcesManager.LoadUIPrefab(UIControllerConst.CANVAS_NAME);
                        }
                        instance = _uiMain.transform.Find(UIControllerConst.PANEL_MAIN).GetComponent<PanelMainUIController>();
                    }
                }
            }
            return instance;
        }
    }
    #endregion

    #region --自定义函数
    protected override void Init() { }
    protected override void InitUIEvent() { }

    /// <summary>
    /// 从 Resources 文件夹下加载 UI 界面，并添加到指定层。
    /// </summary>
    /// <param name="_layer">UI层</param>
    /// <param name="_uiName">UI界面名称</param>
    /// <returns></returns>
    public GameObject AddUIPanel(UILayer _layer, string _uiName)
    {
        GameObject _addUI = GameResourcesManager.LoadUIPrefab(_uiName);
        _addUI.transform.SetParent(GetUILayer(_layer).transform, false);
        return _addUI;
    }
    /// <summary>
    /// 清除所有的UI界面，但不包括 UILayer.Debug 层下的UI，UILayer.BackGround是否清除受_containBGLayer参数影响。
    /// </summary>
    /// <param name="_containBGLayer">是否清除背景层下面的UI界面</param>
    public void CleanAllUI(bool _containBGLayer = true)
    {
        if (_containBGLayer)
        {
            foreach (Transform item in this.PanelBackGround.transform)
            {
                Destroy(item.gameObject);
            }
        }
        foreach (Transform item in this.PanelBottom.transform)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in this.PanelPopupWindow.transform)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in this.PanelPopupTip.transform)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in this.PanelTop.transform)
        {
            Destroy(item.gameObject);
        }
    }
    /// <summary>
    /// 弹出B1类型提示框。B1类型：只有一个确定按钮。
    /// </summary>
    /// <param name="_promptInfo">提示信息</param>
    /// <returns></returns>
    public static GameObject PopupPromptBoxB1(string _promptInfo)
    {
        GameObject _go = Instance.AddUIPanel(UILayer.PopupTip, UIControllerConst.PROMPT_BOX_B1_PANEL);
        PromptBoxB1PanelUIController _promptBoxB1PanelUIController = _go.GetComponent<PromptBoxB1PanelUIController>();
        _promptBoxB1PanelUIController.PromptInfo = _promptInfo;
        return _go;
    }
    /// <summary>
    /// 弹出B2类型提示框。B2类型：有一个确定按钮和一个取消按钮。
    /// </summary>
    /// <param name="_promptInfo">提示信息</param>
    /// <param name="_OKButtonEvent">确认按钮响应事件</param>
    /// <returns></returns>
    public static GameObject PopupPromptBoxB2(string _promptInfo, Action _OKButtonEvent = null)
    {
        GameObject _go = Instance.AddUIPanel(UILayer.PopupTip, UIControllerConst.PROMPT_BOX_B2_PANEL);
        PromptBoxB2PanelUIController _promptBoxB2PanelUIController = _go.GetComponent<PromptBoxB2PanelUIController>();
        _promptBoxB2PanelUIController.SetPromptMessage(_promptInfo, _OKButtonEvent);
        return _go;
    }
    /// <summary>
    /// 获取UI层级父物体
    /// </summary>
    /// <param name="_layer">UI层</param>
    /// <returns></returns>
    private GameObject GetUILayer(UILayer _layer)
    {
        switch (_layer)
        {
            case UILayer.Debug:
                return this.PanelDebug;
            case UILayer.BackGround:
                return this.PanelBackGround;
            case UILayer.Bottom:
                return this.PanelBottom;
            case UILayer.PopupWindow:
                return this.PanelPopupWindow;
            case UILayer.PopupTip:
                return this.PanelPopupTip;
            case UILayer.Top:
                return this.PanelTop;
            default:
                return null;
        }
    }
    #endregion
}

/// <summary>
/// UI层级
/// </summary>
public enum UILayer : int
{
    Debug = 0,
    BackGround = 1,
    Bottom = 2,
    PopupWindow = 3,
    PopupTip = 4,
    Top = 5,
}