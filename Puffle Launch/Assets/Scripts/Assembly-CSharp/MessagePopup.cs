using UnityEngine;

public class MessagePopup : BasePopup
{
	public enum Button
	{
		eFirst = 0,
		eButton_COUNT = 1
	}

	public enum Label
	{
		eTitle = 0,
		eMessage = 1,
		eLabel_COUNT = 2
	}

	public MessagePopup(GameObject aRefObj)
		: base(aRefObj)
	{
		base.Priority = GUIPriority.eHigh;
	}

	protected override void CreateLayouts()
	{
		base.WindowData = new GUIDefines.WindowData
		{
			pos = new GUIDefines.RectInfo
			{
				leftRatio = 0.1833333f,
				topRatio = 0.228125f,
				widthRatio = 0.6291667f,
				heightRatio = 0.5609375f,
				IPad = new GUIDefines.RectIPadInfo
				{
					keepSizeRatio = true
				}
			},
			id = 10,
			style = new GUIDefines.StyleInfo
			{
				styleName = "ErrorPopupWindow"
			}
		};
		base.ButtonData = new GUIDefines.ButtonData[1]
		{
			new GUIDefines.ButtonData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.1647917f,
					topRatio = 0.3703175f,
					widthRatio = 19f / 64f,
					heightRatio = 0.125f,
					IPad = new GUIDefines.RectIPadInfo
					{
						leftOffset = 15f,
						topOffset = 15f
					}
				},
				content = new GUIDefines.ContentInfo
				{
					textId = "TXT_OKAllCaps"
				},
				style = new GUIDefines.StyleInfo
				{
					styleName = "SmallButton"
				}
			}
		};
		base.LabelData = new GUIDefines.LabelData[2]
		{
			new GUIDefines.LabelData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.05687501f,
					topRatio = 0.0696875f,
					widthRatio = 0.5114583f,
					heightRatio = 0.1124975f,
					IPad = new GUIDefines.RectIPadInfo
					{
						leftOffset = 15f
					}
				},
				content = new GUIDefines.ContentInfo
				{
					textId = "TXT_ErrorTitle"
				},
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customFontSize = GUIDefines.FontSize.eLarge
				}
			},
			new GUIDefines.LabelData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.01133334f,
					topRatio = 0.1846875f,
					widthRatio = 0.6037483f,
					heightRatio = 0.1624975f,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true
					}
				},
				content = new GUIDefines.ContentInfo(),
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customFontSize = GUIDefines.FontSize.eMedium,
					customWordWrap = true
				}
			}
		};
	}

	public void ShowText(string aText)
	{
		SetLabelText(1, aText);
		Show(true);
	}

	public void ShowTextId(string aTextId)
	{
		SetLabelTextId(1, aTextId);
		Show(true);
	}
}
