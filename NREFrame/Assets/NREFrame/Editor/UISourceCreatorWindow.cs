﻿/* 
 * 为每一个UI预制体自动生成第一级子物体获取部分代码
 * 1、编辑器工具
 * 2、每次选中一个UI预制体，界面刷新。
 * 3、点击Create按钮生成文件。
 * 4、文件内容，包括每个预制体的变量引用，以及初始局部坐标变量引用。并且都使用属性实现，达到优化目的。
 */
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class UISourceCreatorWindow : EditorWindow
{
    #region --变量定义
    private const string uiSourceFilesPath = "/_Scripts/UISourceFiles";

    private static GameObject selectGameObject;
    private static bool canCreator = false;
    #endregion

    #region --系统函数
    private void OnGUI()
    {
        GUILayout.Label("Create Path：" + uiSourceFilesPath);
        GUILayout.Label("选择需要生成Source文件的UI预制体:");

        if (GUILayout.Button("Create"))
        {
            if (selectGameObject != null && canCreator)
            {
                CreateSourceFiel();
            }
        }
        if (selectGameObject == null)
        {
            canCreator = false;
            GUILayout.Label("Target Prefeb：Null");
        }
        else
        {
            if (PrefabUtility.GetPrefabType(selectGameObject) != PrefabType.None)
            {
                RectTransform _rectTransform = selectGameObject.GetComponent<RectTransform>();
                if (_rectTransform == null)
                {
                    canCreator = false;
                    GUILayout.Label("请选择UI预制体");
                }
                else
                {
                    canCreator = true;
                    GUILayout.Label("Target Prefeb：" + selectGameObject.name);
                }
            }
            else
            {
                canCreator = false;
                GUILayout.Label("请选择UI预制体");
            }
        }

    }
    private void OnSelectionChange()
    {
        selectGameObject = Selection.activeGameObject;
        this.Repaint();
    }
    #endregion

    #region --自定义函数
    [MenuItem("NRE/UI Source Create")]
    private static void UISourceCreate()
    {
        UISourceCreatorWindow _window = EditorWindow.GetWindow<UISourceCreatorWindow>(false, "UISource", true);
        _window.Show();
    }
    private static void CreateSourceFiel()
    {
        string _className = selectGameObject.name + "UIController";
        if (!Directory.Exists(Application.dataPath + uiSourceFilesPath))
        {
            Directory.CreateDirectory(Application.dataPath + uiSourceFilesPath);
        }
        StreamWriter _streamWriter = File.CreateText(Application.dataPath + uiSourceFilesPath + "/" + selectGameObject.name + "UISource.cs");

        _streamWriter.WriteLine("/* UISource File Create Data: " + DateTime.Now + "*/\n");

        _streamWriter.WriteLine("using UnityEngine;\n");

        _streamWriter.WriteLine("public partial class " + _className + " : UIControllerBase" + "\n{");

        foreach (Transform item in selectGameObject.transform)
        {
            string _childName = item.gameObject.name.Substring(0, 1).ToLower() +
                item.gameObject.name.Substring(1, item.gameObject.name.Length - 1);
            _streamWriter.WriteLine("\t" + "private GameObject " + _childName + ";");
            _streamWriter.WriteLine("\t" + "private Vector3 uiOriginalPosition" + item.gameObject.name + ";");
        }

        _streamWriter.WriteLine(string.Empty);

        foreach (Transform item in selectGameObject.transform)
        {
            string _childName = item.gameObject.name.Substring(0, 1).ToLower() +
                item.gameObject.name.Substring(1, item.gameObject.name.Length - 1);
            _streamWriter.WriteLine("\t" + "public GameObject " + item.name + "\n" + "\t{");
            _streamWriter.WriteLine("\t\t" + "get" + "\n" + "\t\t{");
            _streamWriter.WriteLine("\t\t\t" + "if (" + _childName + " == null)" + "\n" + "\t\t\t{");
            _streamWriter.WriteLine("\t\t\t\t" + _childName + " = this.transform.Find(\"" + item.name + "\").gameObject;");
            _streamWriter.WriteLine("\t\t\t\t" + "uiOriginalPosition" + item.name + " = " + _childName + ".transform.localPosition;");
            _streamWriter.WriteLine("\t\t\t" + "}");
            _streamWriter.WriteLine("\t\t\t" + "return " + _childName + ";");
            _streamWriter.WriteLine("\t\t" + "} ");
            _streamWriter.WriteLine("\t" + "}");

            _streamWriter.WriteLine("\t" + "public Vector3 UIOriginalPosition" + item.name + "\n" + "\t{");
            _streamWriter.WriteLine("\t\t" + "get" + "\n" + "\t\t{");
            _streamWriter.WriteLine("\t\t\t" + "if (" + _childName + " == null)" + "\n" + "\t\t\t{");
            _streamWriter.WriteLine("\t\t\t\t" + "return " + item.name + ".transform.localPosition;");
            _streamWriter.WriteLine("\t\t\t" + "}");
            _streamWriter.WriteLine("\t\t\t" + "return uiOriginalPosition" + item.name + ";");
            _streamWriter.WriteLine("\t\t" + "} ");
            _streamWriter.WriteLine("\t" + "}");
        }

        _streamWriter.WriteLine("}");

        _streamWriter.Dispose();
        _streamWriter.Close();

        AssetDatabase.Refresh();
    }
    #endregion
}
