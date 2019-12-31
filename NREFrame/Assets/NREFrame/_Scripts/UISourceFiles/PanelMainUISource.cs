/* UISource File Create Data: 2019/1/15 16:27:34*/

using UnityEngine;

public partial class PanelMainUIController : UIControllerBase
{
	private GameObject panelBackGround;
	private Vector3 uiOriginalPositionPanelBackGround;
	private GameObject panelBottom;
	private Vector3 uiOriginalPositionPanelBottom;
	private GameObject panelPopupWindow;
	private Vector3 uiOriginalPositionPanelPopupWindow;
	private GameObject panelPopupTip;
	private Vector3 uiOriginalPositionPanelPopupTip;
	private GameObject panelTop;
	private Vector3 uiOriginalPositionPanelTop;
	private GameObject panelDebug;
	private Vector3 uiOriginalPositionPanelDebug;


	public GameObject PanelBackGround
	{
		get
		{
			if (panelBackGround == null)
			{
				panelBackGround = this.transform.Find("PanelBackGround").gameObject;
				uiOriginalPositionPanelBackGround = panelBackGround.transform.localPosition;
			}
			return panelBackGround;
		} 
	}
	public Vector3 UIOriginalPositionPanelBackGround
	{
		get
		{
			if (panelBackGround == null)
			{
				return PanelBackGround.transform.localPosition;
			}
			return uiOriginalPositionPanelBackGround;
		} 
	}
	public GameObject PanelBottom
	{
		get
		{
			if (panelBottom == null)
			{
				panelBottom = this.transform.Find("PanelBottom").gameObject;
				uiOriginalPositionPanelBottom = panelBottom.transform.localPosition;
			}
			return panelBottom;
		} 
	}
	public Vector3 UIOriginalPositionPanelBottom
	{
		get
		{
			if (panelBottom == null)
			{
				return PanelBottom.transform.localPosition;
			}
			return uiOriginalPositionPanelBottom;
		} 
	}
	public GameObject PanelPopupWindow
	{
		get
		{
			if (panelPopupWindow == null)
			{
				panelPopupWindow = this.transform.Find("PanelPopupWindow").gameObject;
				uiOriginalPositionPanelPopupWindow = panelPopupWindow.transform.localPosition;
			}
			return panelPopupWindow;
		} 
	}
	public Vector3 UIOriginalPositionPanelPopupWindow
	{
		get
		{
			if (panelPopupWindow == null)
			{
				return PanelPopupWindow.transform.localPosition;
			}
			return uiOriginalPositionPanelPopupWindow;
		} 
	}
	public GameObject PanelPopupTip
	{
		get
		{
			if (panelPopupTip == null)
			{
				panelPopupTip = this.transform.Find("PanelPopupTip").gameObject;
				uiOriginalPositionPanelPopupTip = panelPopupTip.transform.localPosition;
			}
			return panelPopupTip;
		} 
	}
	public Vector3 UIOriginalPositionPanelPopupTip
	{
		get
		{
			if (panelPopupTip == null)
			{
				return PanelPopupTip.transform.localPosition;
			}
			return uiOriginalPositionPanelPopupTip;
		} 
	}
	public GameObject PanelTop
	{
		get
		{
			if (panelTop == null)
			{
				panelTop = this.transform.Find("PanelTop").gameObject;
				uiOriginalPositionPanelTop = panelTop.transform.localPosition;
			}
			return panelTop;
		} 
	}
	public Vector3 UIOriginalPositionPanelTop
	{
		get
		{
			if (panelTop == null)
			{
				return PanelTop.transform.localPosition;
			}
			return uiOriginalPositionPanelTop;
		} 
	}
	public GameObject PanelDebug
	{
		get
		{
			if (panelDebug == null)
			{
				panelDebug = this.transform.Find("PanelDebug").gameObject;
				uiOriginalPositionPanelDebug = panelDebug.transform.localPosition;
			}
			return panelDebug;
		} 
	}
	public Vector3 UIOriginalPositionPanelDebug
	{
		get
		{
			if (panelDebug == null)
			{
				return PanelDebug.transform.localPosition;
			}
			return uiOriginalPositionPanelDebug;
		} 
	}
}
