/*
 * 版本号：
 * Modify：修改日期
 * Modifier：刘伟
 * Modify Reason：修改原因
 * Modify Content：修改内容说明
*/

using UnityEngine;

public abstract class RootBase : MonoBehaviour
{
    #region -- 变量定义
    public string rootID = string.Empty;
    #endregion

    #region -- 系统函数
    protected virtual void Awake() { }
    protected virtual void OnEnable() { }
    protected virtual void Start()
    {
        Init();
    }
    protected virtual void Update() { }
    protected virtual void FixedUpdate() { }
    protected virtual void LateUpdate() { }
    protected virtual void OnDisable() { }
    protected virtual void OnDestroy() { }
    #endregion

    #region -- 自定义函数
    /// <summary>
    /// 初始化节点
    /// </summary>
    public abstract void Init();

    /// <summary>
    /// 隐藏节点
    /// </summary>
    public void Hide()
    {
        this.gameObject.SetActive(true);
    }
    /// <summary>
    /// 显示节点
    /// </summary>
    public void Show()
    {
        this.gameObject.SetActive(false);
    }
    /// <summary>
    /// 销毁节点
    /// </summary>
    public virtual void Destroy()
    {
        Destroy(this.gameObject);
    }
    #endregion
}
