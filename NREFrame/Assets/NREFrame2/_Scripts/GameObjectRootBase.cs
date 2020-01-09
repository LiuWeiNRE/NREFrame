/*
 * 版本号：
 * Modify：修改日期
 * Modifier：刘伟
 * Modify Reason：修改原因
 * Modify Content：修改内容说明
*/

public abstract class GameObjectRootBase : RootBase 
{
    #region -- 变量定义

    #endregion

    #region -- 系统函数
    protected override void Start()
    {
        base.Start();
    }
    #endregion

    #region -- 自定义函数
    public override void Destroy()
    {
        base.Destroy();

        if (APPEngine.GameObjectRootDict.ContainsKey(this.rootID))
        {
            APPEngine.GameObjectRootDict.Remove(rootID);
        }
    }
    #endregion
}
