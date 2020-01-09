/*
 * 版本号：
 * Modify：修改日期
 * Modifier：刘伟
 * Modify Reason：修改原因
 * Modify Content：修改内容说明
*/

using UnityEngine;

public abstract class UIRootBase : RootBase
{
    #region -- 变量定义
    protected CanvasGroup canvasGroup;
    #endregion

    #region -- 系统函数
    protected override void Start()
    {
        base.Start();
        InitUIEvent();
    }
    #endregion

    #region -- 自定义函数
    public CanvasGroup CanvasGroup
    {
        get
        {
            if (canvasGroup == null)
            {
                canvasGroup = this.GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
                }
            }
            return canvasGroup;
        }
    }

    /// <summary>
    /// 初始化UI节点所有包含UI组件的事件
    /// </summary>
    protected abstract void InitUIEvent();

    public override void Destroy()
    {
        base.Destroy();

        if (APPEngine.UIRootDict.ContainsKey(this.rootID))
        {
            APPEngine.UIRootDict.Remove(rootID);
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