/*
 * 基于 IPointerDownHandler 接口实现的鼠标双击与单击事件响应组件。（还有待优化）
 */
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PointerDownEvent : MonoBehaviour, IPointerDownHandler
{
    #region -- 变量定义
    [SerializeField] private float doubleClickInterval = 0.2F;// 鼠标双击间隔时间

    /* 单击事件是否每次都执行。
     * 为true时，单击事件每次点击都会执行，即双击时，会执行两次单击事件，一次双击事件。
     * 为false时，执行单击事件时，不会执行双击事件；执行双击事件时，不会执行单击事件;
     * 如果点击三次，则会执行一次双击事件，然后再执行一次单击事件。
     */
    [SerializeField] private bool singleClickEachExecution = true;

    [SerializeField] private UnityEvent singleClick;// 单击事件
    [SerializeField] private UnityEvent doubleClick;// 双击事件

    private float firstClickedTime = 0;//第一次点击时间
    private float secondClickedTime = 0;//第二次点击时间
    #endregion

    #region -- 自定义函数
    /// <summary>
    /// 鼠标双击间隔时间。
    /// </summary>
    public float DoubleClickInterval
    {
        get { return doubleClickInterval; }
        set
        {
            if (value <= 0F)
            {
                value = 0.1f;
            }
            doubleClickInterval = value;
        }
    }
    /// <summary>
    /// 单击事件是否每次都执行。
    /// </summary>
    public bool SingleClickEachExecution
    {
        get { return singleClickEachExecution; }
        set { singleClickEachExecution = value; }
    }
    /// <summary>
    /// 单击事件。
    /// </summary>
    public UnityEvent SingleClick
    {
        get
        {
            if (singleClick == null)
            {
                singleClick = new UnityEvent();
            }
            return singleClick;
        }
    }
    /// <summary>
    /// 双击事件。
    /// </summary>
    public UnityEvent DoubleClick
    {
        get
        {
            if (doubleClick == null)
            {
                doubleClick = new UnityEvent();
            }
            return doubleClick;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (singleClickEachExecution)
        {
            singleClick.Invoke();
        }
        else
        {
            if (firstClickedTime == 0)
            {
                Invoke("ExecuteSingleClick", doubleClickInterval);
            }
        }

        secondClickedTime = Time.realtimeSinceStartup;
        if (secondClickedTime - firstClickedTime < doubleClickInterval)
        {
            if (!singleClickEachExecution)
            {
                CancelInvoke("ExecuteSingleClick");
            }
            doubleClick.Invoke();
            firstClickedTime = 0;
        }
        else
        {
            firstClickedTime = secondClickedTime;
        }
    }

    /// <summary>
    /// 执行单击事件，并将 firstClickedTime 置0。
    /// </summary>
    private void ExecuteSingleClick()
    {
        singleClick.Invoke();
        firstClickedTime = 0;
    }
    #endregion
}