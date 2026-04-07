using UnityEngine;

public class AppQuitPopup : BasePopup
{
	private enum Button
	{
		eYes = 0,
		eNo = 1,
		eButton_COUNT = 2
	}

	private enum Label
	{
		eTitle = 0,
		eLabel_COUNT = 1
	}

	private enum Texture
	{
		eBackground = 0,
		eTexture_COUNT = 1
	}

	private enum ReturnCode
	{
		eSuccess = 0,
		eFail = 1,
		eReturnCode_COUNT = 2
	}

	public AppQuitPopup(GameObject aRefObj)
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
		base.TextureData = new GUIDefines.TextureData[1]
		{
			new GUIDefines.TextureData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.25f,
					topRatio = 23f / 128f,
					widthRatio = 0.5f,
					heightRatio = 0.5f
				},
				icon = new GUIDefines.TextureInfo
				{
					name = "GUI/MainMenu/Textures/exit_popup"
				}
			}
		};
		base.ButtonData = new GUIDefines.ButtonData[2]
		{
			new GUIDefines.ButtonData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.3260417f,
					topRatio = 0.5015625f,
					widthRatio = 7f / 64f,
					heightRatio = 0.0796875f
				},
				detectZoneScale = 1.5f,
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customNormal = new GUIDefines.Texture2DInfo
					{
						name = "GUI/MainMenu/Textures/play_button"
					},
					customActive = new GUIDefines.Texture2DInfo
					{
						name = "GUI/MainMenu/Textures/play_button_pressed"
					}
				}
			},
			new GUIDefines.ButtonData
			{
				buttonId = 1,
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.5635416f,
					topRatio = 0.5015625f,
					widthRatio = 7f / 64f,
					heightRatio = 0.0796875f
				},
				detectZoneScale = 1.5f,
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customNormal = new GUIDefines.Texture2DInfo
					{
						name = "GUI/MainMenu/Textures/play_button"
					},
					customActive = new GUIDefines.Texture2DInfo
					{
						name = "GUI/MainMenu/Textures/play_button_pressed"
					}
				}
			}
		};
		base.LabelData = new GUIDefines.LabelData[3]
		{
			new GUIDefines.LabelData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.3125f,
					topRatio = 0.2484375f,
					widthRatio = 0.3708334f,
					heightRatio = 0.2078125f
				},
				content = new GUIDefines.ContentInfo
				{
					textId = "TXT_AndroidExit"
				},
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customWordWrap = true,
					customFontSize = GUIDefines.FontSize.eMedium,
					customTextAlignment = TextAnchor.MiddleCenter
				}
			},
			new GUIDefines.LabelData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.3239583f,
					topRatio = 0.5046875f,
					widthRatio = 7f / 64f,
					heightRatio = 0.0796875f
				},
				content = new GUIDefines.ContentInfo
				{
					textId = "TXT_Yes"
				},
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customFontSize = GUIDefines.FontSize.eMedium
				}
			},
			new GUIDefines.LabelData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.5635416f,
					topRatio = 0.5046875f,
					widthRatio = 7f / 64f,
					heightRatio = 0.0796875f
				},
				content = new GUIDefines.ContentInfo
				{
					textId = "TXT_No"
				},
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customFontSize = GUIDefines.FontSize.eMedium
				}
			}
		};
	}

	protected override void OnButtonSelect()
	{
		GameFlowManager.Instance.AudioManager.PlayUISFx(GameFlowManager.Instance.MenuClick24);
		if (base.TextFieldData != null)
		{
			for (int i = 0; i < base.TextFieldData.Length; i++)
			{
				base.TextFieldData[i].isFocused = false;
			}
		}
		switch ((Button)base.SelectedButton)
		{
		case Button.eYes:
			Application.Quit();
			break;
		case Button.eNo:
			GameFlowManager.Instance.GUIManager.ShowAppQuitPopup(false);
			break;
		}
	}

	public override void Show(bool aShow)
	{
		base.Show(aShow);
		if (aShow)
		{
			GameFlowManager.Instance.GUIManager.m_Popups.Add(this);
		}
		else
		{
			GameFlowManager.Instance.GUIManager.m_Popups.Remove(this);
		}
	}

	public override void ClosePopup()
	{
		Show(false);
		if (m_Callback != null)
		{
			m_Callback(1);
		}
	}
}
