/*
 * Windows 窗口操作句柄。打开Windows的文件打开窗口和文件保存窗口。
*/
using System;
using System.Runtime.InteropServices;

public class SystemWindowOperate
{
    public string filter = "All File (*.*)\0*.*";
    public string windowTitle = "窗口";
    public string defaultExt = "txt";
    public string defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    public string defaultFileName = string.Empty;

    /// <summary>
    /// 打开项目
    /// </summary>
    /// <returns>项目路径</returns>
    public string OpenProject(Action<string> _event)
    {
        OpenFileDlg _openFileDlg = new OpenFileDlg();
        _openFileDlg.structSize = Marshal.SizeOf(_openFileDlg);
        _openFileDlg.filter = filter;
        _openFileDlg.file = new string(new char[256]);
        _openFileDlg.maxFile = _openFileDlg.file.Length;
        _openFileDlg.fileTitle = new string(new char[64]);
        _openFileDlg.maxFileTitle = _openFileDlg.fileTitle.Length;
        _openFileDlg.initialDir = defaultPath;
        _openFileDlg.title = windowTitle;
        _openFileDlg.defExt = defaultExt;
        _openFileDlg.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;

        if (OpenFileDialog.GetOpenFileName(_openFileDlg))
        {
            string _filepath = _openFileDlg.file;//选择的文件路径;  
            _event(_filepath);
            return _filepath;
        }
        return null;
    }
    /// <summary>
    /// 保存项目
    /// </summary>                                                                       
    public string SaveProject(Action<string> _event)
    {
        SaveFileDlg _openFileDlg = new SaveFileDlg();
        _openFileDlg.structSize = Marshal.SizeOf(_openFileDlg);
        _openFileDlg.filter = filter;
        _openFileDlg.file = defaultFileName + new string(new char[256 - defaultFileName.Length]);
        _openFileDlg.maxFile = _openFileDlg.file.Length;
        _openFileDlg.fileTitle = new string(new char[64]);
        _openFileDlg.maxFileTitle = _openFileDlg.fileTitle.Length;
        _openFileDlg.initialDir = defaultPath; //默认路径
        _openFileDlg.title = windowTitle;
        _openFileDlg.defExt = defaultExt;
        _openFileDlg.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
        if (SaveFileDialog.GetSaveFileName(_openFileDlg))
        {
            string _filepath = _openFileDlg.file;//选择的文件路径;  
            _event(_filepath);
            return _filepath;
        }
        return null;
    }
}

/// <summary>
/// 文件日志类
/// </summary>
//[特性(布局种类.有序,字符集=字符集.自动)]
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class ChinarFileDlog
{
    public int structSize = 0;
    public IntPtr dlgOwner = IntPtr.Zero;
    public IntPtr instance = IntPtr.Zero;
    public String filter = null;
    public String customFilter = null;
    public int maxCustFilter = 0;
    public int filterIndex = 0;
    public String file = null;
    public int maxFile = 0;
    public String fileTitle = null;
    public int maxFileTitle = 0;
    public String initialDir = null;
    public String title = null;
    public int flags = 0;
    public short fileOffset = 0;
    public short fileExtension = 0;
    /// <summary>
    /// 默认文件后缀
    /// </summary>
    public String defExt = null;
    public IntPtr custData = IntPtr.Zero;
    public IntPtr hook = IntPtr.Zero;
    public String templateName = null;
    public IntPtr reservedPtr = IntPtr.Zero;
    public int reservedInt = 0;
    public int flagsEx = 0;
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class OpenFileDlg : ChinarFileDlog { }
public class OpenFileDialog
{
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] OpenFileDlg ofd);
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class SaveFileDlg : ChinarFileDlog { }
public class SaveFileDialog
{
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetSaveFileName([In, Out] SaveFileDlg ofd);
}