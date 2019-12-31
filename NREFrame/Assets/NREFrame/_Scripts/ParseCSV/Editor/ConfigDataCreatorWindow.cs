/*
 * 为每一个配置文件自动生成相对应代码
*/
using UnityEditor;
using UnityEngine;

public class ConfigDataCreatorWindow : EditorWindow
{
    #region --变量定义
    private const string configDataFilePath = "/_Scripts/Data";

    private static Object selectObject;
    private static bool canCreator = false;
    #endregion

    #region --系统函数
    private void OnGUI()
    {
        GUILayout.Label("Create Path：" + configDataFilePath);
        GUILayout.Label("选择需要生成 Data 文件的 CSV 文件:");

        if (GUILayout.Button("Create"))
        {
            if (selectObject != null && canCreator)
            {
                CreatConfigUnit.CreatConfigFile(selectObject, configDataFilePath);
            }
            else
            {
                Debug.Log("请正确选择CSV文件");
            }
        }
        if (selectObject == null)
        {
            canCreator = false;
            GUILayout.Label("Target CSV File：Null");
        }
        else
        {
            string _path = AssetDatabase.GetAssetPath(selectObject);
            if (_path.Length < 4)
            {
                canCreator = false;
                GUILayout.Label("请选择正确的CSV文件");
            }
            else if (_path.ToLower().Substring(_path.Length - 4, 4) == ".csv")
            {
                canCreator = true;
                GUILayout.Label("Target CSV File：" + selectObject.name);
            }
            else
            {
                canCreator = false;
                GUILayout.Label("请选择正确的CSV文件");
            }
        }

    }
    private void OnSelectionChange()
    {
        selectObject = Selection.activeObject;
        this.Repaint();
    }
    #endregion

    #region --自定义函数
    [MenuItem("NRE/Config Data Create")]
    private static void UISourceCreate()
    {
        ConfigDataCreatorWindow _window = EditorWindow.GetWindow<ConfigDataCreatorWindow>(false, "ConfigData", true);
        _window.Show();
    }
    #endregion
}
