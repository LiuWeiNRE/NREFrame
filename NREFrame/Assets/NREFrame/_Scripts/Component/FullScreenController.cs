/*此脚本用来设置程序全屏无边框，用来解决发布后，输入法不显示选词框的问题*/

using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;

public class FullScreenController : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);
    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    private const uint SWP_SHOWWINDOW = 0x0040;
    private const int GWL_STYLE = -16;//边框用的
    private const int WS_BORDER = 1;
    private const int WS_POPUP = 0x800000;

    int posX = 0;
    int posY = 0;   
    public int screenWidth = 1920;//在这里设置你想要的窗口宽
    public int screenHeight = 1080;//在这里设置你想要的窗口高

    public void Awake()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            StartCoroutine("Setposition");
            Screen.SetResolution(screenWidth, screenHeight, false);//这个是Unity里的设置屏幕大小，
        }
    }
    private IEnumerator Setposition()
    {
        yield return new WaitForSeconds(0.1f);//不知道为什么发布后运行，设置位置的不会生效，延迟0.1秒就可以
        SetWindowLong(GetForegroundWindow(), GWL_STYLE, WS_POPUP);//无边框
        SetWindowPos(GetForegroundWindow(), 0, posX, posY, screenWidth, screenHeight, SWP_SHOWWINDOW);//设置屏幕大小和位置
    }
}
