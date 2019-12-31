/* UISource File Create Data: 3/29/2019 4:35:27 PM*/

using UnityEngine;

public partial class PromptBoxB2PanelUIController : UIControllerBase
{
	private GameObject maskBg;
	private Vector3 uiOriginalPositionMaskBg;
	private GameObject backGround;
	private Vector3 uiOriginalPositionBackGround;
	private GameObject promptText;
	private Vector3 uiOriginalPositionPromptText;
	private GameObject oKButton;
	private Vector3 uiOriginalPositionOKButton;
	private GameObject cancelButton;
	private Vector3 uiOriginalPositionCancelButton;


	public GameObject MaskBg
	{
		get
		{
			if (maskBg == null)
			{
				maskBg = this.transform.Find("MaskBg").gameObject;
				uiOriginalPositionMaskBg = maskBg.transform.localPosition;
			}
			return maskBg;
		} 
	}
	public Vector3 UIOriginalPositionMaskBg
	{
		get
		{
			if (maskBg == null)
			{
				return MaskBg.transform.localPosition;
			}
			return uiOriginalPositionMaskBg;
		} 
	}
	public GameObject BackGround
	{
		get
		{
			if (backGround == null)
			{
				backGround = this.transform.Find("BackGround").gameObject;
				uiOriginalPositionBackGround = backGround.transform.localPosition;
			}
			return backGround;
		} 
	}
	public Vector3 UIOriginalPositionBackGround
	{
		get
		{
			if (backGround == null)
			{
				return BackGround.transform.localPosition;
			}
			return uiOriginalPositionBackGround;
		} 
	}
	public GameObject PromptText
	{
		get
		{
			if (promptText == null)
			{
				promptText = this.transform.Find("PromptText").gameObject;
				uiOriginalPositionPromptText = promptText.transform.localPosition;
			}
			return promptText;
		} 
	}
	public Vector3 UIOriginalPositionPromptText
	{
		get
		{
			if (promptText == null)
			{
				return PromptText.transform.localPosition;
			}
			return uiOriginalPositionPromptText;
		} 
	}
	public GameObject OKButton
	{
		get
		{
			if (oKButton == null)
			{
				oKButton = this.transform.Find("OKButton").gameObject;
				uiOriginalPositionOKButton = oKButton.transform.localPosition;
			}
			return oKButton;
		} 
	}
	public Vector3 UIOriginalPositionOKButton
	{
		get
		{
			if (oKButton == null)
			{
				return OKButton.transform.localPosition;
			}
			return uiOriginalPositionOKButton;
		} 
	}
	public GameObject CancelButton
	{
		get
		{
			if (cancelButton == null)
			{
				cancelButton = this.transform.Find("CancelButton").gameObject;
				uiOriginalPositionCancelButton = cancelButton.transform.localPosition;
			}
			return cancelButton;
		} 
	}
	public Vector3 UIOriginalPositionCancelButton
	{
		get
		{
			if (cancelButton == null)
			{
				return CancelButton.transform.localPosition;
			}
			return uiOriginalPositionCancelButton;
		} 
	}
}
