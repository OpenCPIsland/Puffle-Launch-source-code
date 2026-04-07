using UnityEngine;

public class CreateAccountMessagePopup : GenericPopup
{
	public CreateAccountMessagePopup(GameObject aRefObj)
		: base(aRefObj)
	{
	}

	protected override void CreateLayouts()
	{
		base.WindowData = new GUIDefines.WindowData
		{
			pos = new GUIDefines.RectInfo
			{
				widthRatio = 1f,
				heightRatio = 1f
			},
			id = 10
		};
		if (ResolutionManager.Instance.LayoutSize == ResolutionManager.eLayoutSize.eIPad)
		{
			base.TextureData = new GUIDefines.TextureData[2]
			{
				new GUIDefines.TextureData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.20507812f,
						topRatio = 0.27734375f,
						widthRatio = 0.58984375f,
						heightRatio = 57f / 128f,
						IPad = new GUIDefines.RectIPadInfo
						{
							keepSizeRatio = true
						}
					},
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/CreateAccount/error_popup_bg"
					}
				},
				new GUIDefines.TextureData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.3208333f,
						topRatio = 0.2296875f,
						widthRatio = 0.03958333f,
						heightRatio = 7f / 128f,
						IPad = new GUIDefines.RectIPadInfo
						{
							keepSizeRatio = true
						}
					},
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/BlueTooth/bluetooth_progress-indicator"
					},
					invisible = true,
					rotate = GUIDefines.RotateDirection.eClockwise
				}
			};
			base.ButtonData = new GUIDefines.ButtonData[1]
			{
				new GUIDefines.ButtonData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.33984375f,
						topRatio = 0.566875f,
						widthRatio = 41f / 128f,
						heightRatio = 13f / 96f,
						IPad = new GUIDefines.RectIPadInfo
						{
							keepSizeRatio = true
						}
					},
					content = new GUIDefines.ContentInfo
					{
						textId = "TXT_Ok"
					},
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customNormal = new GUIDefines.Texture2DInfo
						{
							name = "GUI/CreateAccount/error_popup_button"
						},
						customFontSize = GUIDefines.FontSize.eMedium
					}
				}
			};
			base.LabelData = new GUIDefines.LabelData[2]
			{
				new GUIDefines.LabelData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.21923828f,
						topRatio = 55f / 128f,
						widthRatio = 0.56152344f,
						heightRatio = 15f / 128f,
						IPad = new GUIDefines.RectIPadInfo
						{
							keepSizeRatio = true
						}
					},
					content = new GUIDefines.ContentInfo(),
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customFontSize = GUIDefines.FontSize.eSmall,
						customNormalTextColor = GUIConstants.kWhiteColor,
						useCustomTextAlignment = true,
						customTextAlignment = TextAnchor.MiddleCenter,
						customWordWrap = true
					}
				},
				new GUIDefines.LabelData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.18541667f,
						topRatio = 0.299479f,
						widthRatio = 0.62916666f,
						heightRatio = 3f / 32f,
						IPad = new GUIDefines.RectIPadInfo
						{
							keepSizeRatio = true
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
						customNormalTextColor = GUIConstants.kWhiteColor,
						useCustomTextAlignment = true,
						customTextAlignment = TextAnchor.MiddleCenter,
						customWordWrap = true
					}
				}
			};
		}
		else
		{
			base.TextureData = new GUIDefines.TextureData[2]
			{
				new GUIDefines.TextureData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.18541667f,
						topRatio = 0.232813f,
						widthRatio = 0.62916666f,
						heightRatio = 0.534375f,
						IPad = new GUIDefines.RectIPadInfo
						{
							keepSizeRatio = true
						}
					},
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/CreateAccount/error_popup_bg"
					}
				},
				new GUIDefines.TextureData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.3208333f,
						topRatio = 0.2296875f,
						widthRatio = 0.03958333f,
						heightRatio = 7f / 128f,
						IPad = new GUIDefines.RectIPadInfo
						{
							keepSizeRatio = true
						}
					},
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/BlueTooth/bluetooth_progress-indicator"
					},
					invisible = true,
					rotate = GUIDefines.RotateDirection.eClockwise
				}
			};
			base.ButtonData = new GUIDefines.ButtonData[1]
			{
				new GUIDefines.ButtonData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.32916668f,
						topRatio = 0.566875f,
						widthRatio = 41f / 120f,
						heightRatio = 0.1625f
					},
					content = new GUIDefines.ContentInfo
					{
						textId = "TXT_Ok"
					},
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customNormal = new GUIDefines.Texture2DInfo
						{
							name = "GUI/CreateAccount/error_popup_button"
						},
						customFontSize = GUIDefines.FontSize.eMedium
					}
				}
			};
			base.LabelData = new GUIDefines.LabelData[2]
			{
				new GUIDefines.LabelData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.20052083f,
						topRatio = 25f / 64f,
						widthRatio = 115f / 192f,
						heightRatio = 0.1875f,
						IPad = new GUIDefines.RectIPadInfo
						{
							keepSizeRatio = true
						}
					},
					content = new GUIDefines.ContentInfo(),
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customFontSize = GUIDefines.FontSize.eSmall,
						customNormalTextColor = GUIConstants.kWhiteColor,
						useCustomTextAlignment = true,
						customTextAlignment = TextAnchor.MiddleCenter,
						customWordWrap = true
					}
				},
				new GUIDefines.LabelData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.18541667f,
						topRatio = 33f / 128f,
						widthRatio = 0.62916666f,
						heightRatio = 3f / 32f,
						IPad = new GUIDefines.RectIPadInfo
						{
							keepSizeRatio = true
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
						customNormalTextColor = GUIConstants.kWhiteColor,
						useCustomTextAlignment = true,
						customTextAlignment = TextAnchor.MiddleCenter,
						customWordWrap = true
					}
				}
			};
		}
	}
}
