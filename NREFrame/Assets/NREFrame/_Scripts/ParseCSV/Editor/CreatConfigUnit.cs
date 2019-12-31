/*
 * 生成配置文件信息脚本
 */
using System.IO;
using UnityEditor;
using UnityEngine;

public class CreatConfigUnit
{
    public static void CreatConfigFile(Object selectObj, string writePath)
    {
        string fileName = selectObj.name;
        string className = fileName;
        StreamWriter sw = new StreamWriter(Application.dataPath + writePath + "/" + className + "Source.cs");

        sw.WriteLine("using UnityEngine;");
        sw.WriteLine("using System.Collections;\n");

        sw.WriteLine("public partial class " + className + " : GameConfigDataBase");
        sw.WriteLine("{");

        string filePath = AssetDatabase.GetAssetPath(selectObj);
        CsvStreamReader csr = new CsvStreamReader(filePath);
        for (int colNum = 1; colNum < csr.ColCount + 1; colNum++)
        {
            string fieldName = csr[1, colNum];
            string fieldType = csr[2, colNum];
            sw.WriteLine("\t" + "public " + fieldType + " " + fieldName + ";" + "");
        }

        sw.WriteLine("\n");

        sw.WriteLine("\t" + "protected override string GetFilePath ()");
        sw.WriteLine("\t" + "{");
        sw.WriteLine("\t\t" + "return " + "\"" + fileName + "\";");
        sw.WriteLine("\t" + "}");

        sw.WriteLine("}");

        sw.Flush();
        sw.Close();
        AssetDatabase.Refresh();
    }
}
