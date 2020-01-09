/* 
 * UI控制器基类
 */
using UnityEngine;

public abstract class UIControllerBase : MonoBehaviour
{
    #region --变量定义
    protected CanvasGroup canvasGroup;
    #endregion

    #region --自定义函数
    protected abstract void Init();
    protected abstract void InitUIEvent();

    /// <summary>
    /// 界面的 CanvasGroup 组件
    /// </summary>
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
    /// 销毁界面
    /// </summary>
    public void Destroy()
    {
        Destroy(this.gameObject);
    }
    #endregion
}