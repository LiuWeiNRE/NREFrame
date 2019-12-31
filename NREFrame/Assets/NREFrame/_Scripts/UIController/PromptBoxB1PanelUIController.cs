/*
 * B1类型提示框控制器。B1类型：只有一个确定按钮。
*/
using UnityEngine.UI;

public partial class PromptBoxB1PanelUIController : UIControllerBase
{
    #region -- 变量定义

    #endregion

    #region -- 系统函数
    private void Start()
    {
        InitUIEvent();
    }
    #endregion

    #region -- 自定义函数
    public string PromptInfo
    {
        set { PromptText.GetComponent<Text>().text = value; }
    }
    protected override void Init() { }
    protected override void InitUIEvent()
    {
        OKButton.GetComponent<Button>().onClick.AddListener(this.Destroy);
    }
    #endregion

}
