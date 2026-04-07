using UnityEngine;

public class GenericPopup : BasePopup
{
	public enum Button
	{
		eFirst = 0,
		eButton_COUNT = 1
	}

	public enum Label
	{
		eMessage = 0,
		eLabel_COUNT = 1
	}

	public enum Texture
	{
		eBackround = 0,
		eSensei = 1,
		eProgressIndicator = 2,
		eTexture_COUNT = 3
	}

	protected GUIDefines.StyleInfo m_LabelStyle = new GUIDefines.StyleInfo
	{
		useCustomStyle = true,
		customFontSize = GUIDefines.FontSize.eMedium,
		customNormalTextColor = GUIConstants.kLightBrownColor,
		customWordWrap = true
	};

	public GenericPopup(GameObject aRefObj)
		: base(aRefObj)
	{
		base.Priority = GUIPriority.eHigh;
	}

	protected override void CreateLayouts()
	{
		if (ResolutionManager.Instance.LayoutSize == ResolutionManager.eLayoutSize.eIPad)
		{
			base.WindowData = new GUIDefines.WindowData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.150391f,
					topRatio = 0.22070312f,
					widthRatio = 0.69921875f,
					heightRatio = 0.558594f,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true
					}
				},
				id = 10,
				respectIpadSizeRatio = true,
				style = new GUIDefines.StyleInfo
				{
					styleName = "ErrorPopupWindow"
				}
			};
		}
		else
		{
			base.WindowData = new GUIDefines.WindowData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.127083f,
					topRatio = 0.16484375f,
					widthRatio = 0.74583f,
					heightRatio = 0.6703125f
				},
				id = 10,
				respectIpadSizeRatio = true,
				style = new GUIDefines.StyleInfo
				{
					styleName = "ErrorPopupWindow"
				}
			};
		}
		base.ButtonData = new GUIDefines.ButtonData[1]
		{
			new GUIDefines.ButtonData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.235417f,
					topRatio = 0.396875f,
					widthRatio = 0.275f,
					heightRatio = 0.1557292f,
					IPad = new GUIDefines.RectIPadInfo
					{
						leftOffset = -16f,
						topOffset = -51f
					}
				},
				content = new GUIDefines.ContentInfo
				{
					textId = "TXT_Ok"
				},
				style = new GUIDefines.StyleInfo
				{
					styleName = "SmallButton"
				}
			}
		};
		base.TextureData = new GUIDefines.TextureData[1]
		{
			new GUIDefines.TextureData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.3208333f,
					topRatio = 0.2296875f,
					widthRatio = 0.03958333f,
					heightRatio = 7f / 128f
				},
				icon = new GUIDefines.TextureInfo
				{
					name = "GUI/BlueTooth/bluetooth_progress-indicator"
				},
				invisible = true,
				rotate = GUIDefines.RotateDirection.eClockwise
			}
		};
		base.LabelData = new GUIDefines.LabelData[2]
		{
			new GUIDefines.LabelData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.08020833f,
					topRatio = 0.2046875f,
					widthRatio = 0.5749996f,
					heightRatio = 0.146875f,
					IPad = new GUIDefines.RectIPadInfo
					{
						topOffset = -23f
					}
				},
				content = new GUIDefines.ContentInfo(),
				style = m_LabelStyle
			},
			new GUIDefines.LabelData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.08020833f,
					topRatio = 0.0828125f,
					widthRatio = 0.5749996f,
					heightRatio = 0.1f,
					IPad = new GUIDefines.RectIPadInfo
					{
						topOffset = -9f
					}
				},
				content = new GUIDefines.ContentInfo
				{
					textId = "TXT_Error"
				},
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customFontSize = GUIDefines.FontSize.eLarge,
					customNormalTextColor = GUIConstants.kDarkBrownColor
				}
			}
		};
	}

	public void ShowProgressing(bool aShow)
	{
		if (aShow)
		{
			SetTextureInvisible(2, false);
			SetLabelTextId(0, "TXT_Waiting");
			SetButtonTextId(0, "TXT_Cancel");
		}
		else
		{
			SetTextureInvisible(2, true);
			SetLabelText(0, string.Empty);
		}
		Show(aShow);
	}

	public void ShowText(string aText)
	{
		SetTextureInvisible(2, true);
		SetLabelText(0, aText);
		SetButtonTextId(0, "TXT_Ok");
		Show(true);
	}

	public void ShowTextId(string aTextId)
	{
		SetTextureInvisible(2, true);
		SetLabelTextId(0, aTextId);
		SetButtonTextId(0, "TXT_Ok");
		Show(true);
	}
}
