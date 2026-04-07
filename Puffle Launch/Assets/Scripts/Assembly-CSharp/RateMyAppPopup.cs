using UnityEngine;

public class RateMyAppPopup : BasePopup
{
	private enum Button
	{
		eRateNow = 0,
		eRemindMeLater = 1,
		eNoRate = 2,
		eButton_COUNT = 3
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

	public RateMyAppPopup(GameObject aRefObj)
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
					leftRatio = 0.2447916f,
					topRatio = 0.053125f,
					widthRatio = 49f / 96f,
					heightRatio = 0.8421875f
				},
				icon = new GUIDefines.TextureInfo
				{
					name = "GUI/MainMenu/Textures/exit_popup"
				}
			}
		};
		base.ButtonData = new GUIDefines.ButtonData[3]
		{
			new GUIDefines.ButtonData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.345833f,
					topRatio = 61f / 128f,
					widthRatio = 49f / 160f,
					heightRatio = 0.0828125f
				},
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
					leftRatio = 0.345833f,
					topRatio = 0.571875f,
					widthRatio = 49f / 160f,
					heightRatio = 0.0828125f
				},
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
				buttonId = 2,
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.345833f,
					topRatio = 0.665625f,
					widthRatio = 49f / 160f,
					heightRatio = 0.0828125f
				},
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
		base.LabelData = new GUIDefines.LabelData[5]
		{
			new GUIDefines.LabelData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.322917f,
					topRatio = 0.178125f,
					widthRatio = 0.353125f,
					heightRatio = 0.0921875f
				},
				content = new GUIDefines.ContentInfo
				{
					textId = "TXT_RateMyAppTitle"
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
					leftRatio = 0.322917f,
					topRatio = 0.259375f,
					widthRatio = 0.353125f,
					heightRatio = 33f / 160f
				},
				content = new GUIDefines.ContentInfo
				{
					textId = "TXT_RateMyAppMsgAndroid"
				},
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customWordWrap = true,
					customFontSize = GUIDefines.FontSize.eSmall,
					customTextAlignment = TextAnchor.MiddleCenter
				}
			},
			new GUIDefines.LabelData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.4416666f,
					topRatio = 77f / 160f,
					widthRatio = 7f / 64f,
					heightRatio = 0.0796875f
				},
				content = new GUIDefines.ContentInfo
				{
					textId = "TXT_RateMyAppRateButton"
				},
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customFontSize = GUIDefines.FontSize.eSmall
				}
			},
			new GUIDefines.LabelData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.4458333f,
					topRatio = 0.575f,
					widthRatio = 7f / 64f,
					heightRatio = 0.0796875f
				},
				content = new GUIDefines.ContentInfo
				{
					textId = "TXT_RateMyAppRemindButton"
				},
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customFontSize = GUIDefines.FontSize.eSmall
				}
			},
			new GUIDefines.LabelData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.449999f,
					topRatio = 107f / 160f,
					widthRatio = 7f / 64f,
					heightRatio = 0.0796875f
				},
				content = new GUIDefines.ContentInfo
				{
					textId = "TXT_RateMyAppNoButton"
				},
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customFontSize = GUIDefines.FontSize.eSmall
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
		case Button.eRateNow:
			PlayerPrefs.SetInt("RateMyApp", 10);
			PlayerPrefs.Save();
			Application.OpenURL("http://www.amazon.com/gp/mas/dl/android?p=com.disney.PuffleLaunch");
			GameFlowManager.Instance.GUIManager.ShowRateMyAppPopup(false);
			break;
		case Button.eRemindMeLater:
			PlayerPrefs.SetInt("RateMyApp", 0);
			PlayerPrefs.Save();
			GameFlowManager.Instance.GUIManager.ShowRateMyAppPopup(false);
			break;
		case Button.eNoRate:
			PlayerPrefs.SetInt("RateMyApp", 10);
			PlayerPrefs.Save();
			GameFlowManager.Instance.GUIManager.ShowRateMyAppPopup(false);
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
