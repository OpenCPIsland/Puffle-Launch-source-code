using UnityEngine;

public class LevelSelectPopup : BasePopup
{
	public enum PageID
	{
		TurboModeLocked = 0,
		TurboModeUnlocked = 1,
		TurboModeInstructions = 2,
		TimeTrialLocked = 3,
		TimeTrialUnlocked = 4,
		TimeTrialSilverAchieved = 5,
		TimeTrialGoldAchieved = 6,
		SlowMotionLocked = 7,
		SlowMotionUnlocked = 8,
		SlowMotionInstructions = 9
	}

	public enum Button
	{
		eOk = 0,
		eTurboMode = 1,
		eButton_COUNT = 2
	}

	public LevelSelectPopup(GameObject aRefObj)
		: base(aRefObj)
	{
	}

	public void SetPageID(PageID ae_pageID)
	{
		ResetToDefaults();
		switch (ae_pageID)
		{
		case PageID.TurboModeLocked:
			base.LabelData[0].content.text = LocalizationManager.Instance.GetString("TXT_TurboModeLocked", GameManager.GetTimeFormatedString(GameManager.kTimeTrialTimes[(int)GameManager.Instance.CurrentWorld, 3]));
			base.LabelData[0].style.customFontSize = GUIDefines.FontSize.eMedium;
			base.LabelData[0].pos = new GUIDefines.RectInfo
			{
				leftRatio = 0.22458333f,
				topRatio = 0.425f,
				widthRatio = 0.55083334f,
				heightRatio = 15f / 64f,
				IPad = new GUIDefines.RectIPadInfo
				{
					leftOffset = 10f,
					topOffset = -35f
				}
			};
			base.TextureData[1].icon.name = "GUI/LevelSelect/Popups/TurboMode";
			base.TextureData[1].pos = new GUIDefines.RectInfo
			{
				leftRatio = 0.42604166f,
				topRatio = 15f / 128f,
				widthRatio = 0.14791667f,
				heightRatio = 0.309375f
			};
			break;
		case PageID.TurboModeUnlocked:
			base.LabelData[0].content.textId = "TXT_TurboModeUnlocked";
			base.LabelData[0].style.customFontSize = GUIDefines.FontSize.eMedium;
			base.LabelData[0].pos = new GUIDefines.RectInfo
			{
				leftRatio = 23f / 96f,
				topRatio = 0.4375f,
				widthRatio = 25f / 48f,
				heightRatio = 15f / 64f,
				IPad = new GUIDefines.RectIPadInfo
				{
					leftOffset = 10f,
					topOffset = -23f
				}
			};
			base.TextureData[1].icon.name = "GUI/LevelSelect/Popups/TurboMode";
			base.TextureData[1].pos = new GUIDefines.RectInfo
			{
				leftRatio = 0.42604166f,
				topRatio = 15f / 128f,
				widthRatio = 0.14791667f,
				heightRatio = 0.309375f
			};
			break;
		case PageID.TurboModeInstructions:
			base.LabelData[0].content.textId = "TXT_TurboModeAllCaps";
			base.LabelData[0].style.customFontSize = GUIDefines.FontSize.eMedium;
			base.LabelData[0].pos = new GUIDefines.RectInfo
			{
				leftRatio = 23f / 96f,
				topRatio = 0.4375f,
				widthRatio = 25f / 48f,
				heightRatio = 15f / 64f,
				IPad = new GUIDefines.RectIPadInfo
				{
					leftOffset = 10f,
					topOffset = -23f
				}
			};
			base.TextureData[1].icon.name = "GUI/LevelSelect/Popups/TurboMode";
			base.TextureData[1].pos = new GUIDefines.RectInfo
			{
				leftRatio = 0.42604166f,
				topRatio = 15f / 128f,
				widthRatio = 0.14791667f,
				heightRatio = 0.309375f
			};
			AddTurboCheckbox();
			AddTurboText();
			break;
		case PageID.TimeTrialLocked:
			base.LabelData[0].content.textId = "TXT_TimeTrialLocked";
			base.LabelData[0].style.customFontSize = GUIDefines.FontSize.eMedium;
			base.LabelData[0].pos = new GUIDefines.RectInfo
			{
				leftRatio = 23f / 96f,
				topRatio = 0.41f,
				widthRatio = 25f / 48f,
				heightRatio = 15f / 64f,
				IPad = new GUIDefines.RectIPadInfo
				{
					leftOffset = 10f,
					topOffset = -23f
				}
			};
			base.TextureData[1].icon.name = "GUI/LevelSelect/Popups/Clock_Blue";
			base.TextureData[1].pos = new GUIDefines.RectInfo
			{
				leftRatio = 0.40989584f,
				topRatio = 19f / 128f,
				widthRatio = 0.18020833f,
				heightRatio = 0.2703125f
			};
			break;
		case PageID.TimeTrialUnlocked:
			base.LabelData[0].content.textId = "TXT_TimeTrialUnlocked";
			base.LabelData[0].style.customFontSize = GUIDefines.FontSize.eMedium;
			base.LabelData[0].pos = new GUIDefines.RectInfo
			{
				leftRatio = 23f / 96f,
				topRatio = 0.41f,
				widthRatio = 25f / 48f,
				heightRatio = 15f / 64f,
				IPad = new GUIDefines.RectIPadInfo
				{
					leftOffset = 10f,
					topOffset = -23f
				}
			};
			base.TextureData[1].icon.name = "GUI/LevelSelect/Popups/Clock_Blue";
			base.TextureData[1].pos = new GUIDefines.RectInfo
			{
				leftRatio = 0.40989584f,
				topRatio = 19f / 128f,
				widthRatio = 0.18020833f,
				heightRatio = 0.2703125f
			};
			break;
		case PageID.TimeTrialSilverAchieved:
			base.LabelData[0].content.textId = "TXT_TimeTrialSilver";
			base.LabelData[0].style.customFontSize = GUIDefines.FontSize.eMedium;
			base.LabelData[0].pos = new GUIDefines.RectInfo
			{
				leftRatio = 23f / 96f,
				topRatio = 0.41f,
				widthRatio = 25f / 48f,
				heightRatio = 15f / 64f,
				IPad = new GUIDefines.RectIPadInfo
				{
					leftOffset = 10f,
					topOffset = -23f
				}
			};
			base.TextureData[1].icon.name = "GUI/LevelSelect/Popups/Clock_Silver";
			base.TextureData[1].pos = new GUIDefines.RectInfo
			{
				leftRatio = 0.40989584f,
				topRatio = 19f / 128f,
				widthRatio = 0.18020833f,
				heightRatio = 0.2703125f
			};
			break;
		case PageID.TimeTrialGoldAchieved:
			base.LabelData[0].content.textId = "TXT_TimeTrialGold";
			base.LabelData[0].style.customFontSize = GUIDefines.FontSize.eMedium;
			base.LabelData[0].pos = new GUIDefines.RectInfo
			{
				leftRatio = 23f / 96f,
				topRatio = 0.41f,
				widthRatio = 25f / 48f,
				heightRatio = 15f / 64f,
				IPad = new GUIDefines.RectIPadInfo
				{
					leftOffset = 10f,
					topOffset = -23f
				}
			};
			base.TextureData[1].icon.name = "GUI/LevelSelect/Popups/Clock_Gold";
			base.TextureData[1].pos = new GUIDefines.RectInfo
			{
				leftRatio = 0.40989584f,
				topRatio = 19f / 128f,
				widthRatio = 0.18020833f,
				heightRatio = 0.2703125f
			};
			break;
		case PageID.SlowMotionLocked:
			base.LabelData[0].content.textId = "TXT_SlowMotionLocked";
			base.LabelData[0].style.customFontSize = GUIDefines.FontSize.eMedium;
			base.LabelData[0].pos = new GUIDefines.RectInfo
			{
				leftRatio = 23f / 96f,
				topRatio = 0.415f,
				widthRatio = 25f / 48f,
				heightRatio = 15f / 64f,
				IPad = new GUIDefines.RectIPadInfo
				{
					leftOffset = 10f,
					topOffset = -23f
				}
			};
			base.TextureData[1].icon.name = "GUI/LevelSelect/Popups/Clock_Inactive";
			base.TextureData[1].pos = new GUIDefines.RectInfo
			{
				leftRatio = 0.4015625f,
				topRatio = 19f / 128f,
				widthRatio = 0.196875f,
				heightRatio = 39f / 160f
			};
			break;
		case PageID.SlowMotionUnlocked:
			base.LabelData[0].content.textId = "TXT_SlowMotionUnlocked";
			base.LabelData[0].style.customFontSize = GUIDefines.FontSize.eMedium;
			base.LabelData[0].pos = new GUIDefines.RectInfo
			{
				leftRatio = 0.18958333f,
				topRatio = 0.415f,
				widthRatio = 0.62083334f,
				heightRatio = 15f / 64f,
				IPad = new GUIDefines.RectIPadInfo
				{
					leftOffset = 10f,
					topOffset = -23f
				}
			};
			base.TextureData[1].icon.name = "GUI/LevelSelect/Popups/Clock_Inactive";
			base.TextureData[1].pos = new GUIDefines.RectInfo
			{
				leftRatio = 0.4015625f,
				topRatio = 19f / 128f,
				widthRatio = 0.196875f,
				heightRatio = 39f / 160f
			};
			break;
		case PageID.SlowMotionInstructions:
			base.LabelData[0].content.textId = "TXT_SlowMotionInstructions";
			base.LabelData[0].style.customFontSize = GUIDefines.FontSize.eMedium;
			base.LabelData[0].pos = new GUIDefines.RectInfo
			{
				leftRatio = 23f / 96f,
				topRatio = 0.415f,
				widthRatio = 25f / 48f,
				heightRatio = 15f / 64f,
				IPad = new GUIDefines.RectIPadInfo
				{
					leftOffset = 10f,
					topOffset = -23f
				}
			};
			base.TextureData[1].icon.name = "GUI/LevelSelect/Popups/Clock_Inactive";
			base.TextureData[1].pos = new GUIDefines.RectInfo
			{
				leftRatio = 0.4015625f,
				topRatio = 19f / 128f,
				widthRatio = 0.196875f,
				heightRatio = 39f / 160f
			};
			break;
		}
		base.LabelData[0].pos.Init();
		base.TextureData[1].pos.Init();
		base.TextureData[1].icon.Init();
	}

	private void AddTurboCheckbox()
	{
		GUIDefines.ButtonData[] buttonData = base.ButtonData;
		base.ButtonData = new GUIDefines.ButtonData[2];
		base.ButtonData[0] = buttonData[0];
		base.ButtonData[1] = new GUIDefines.ButtonData
		{
			isTogglable = true,
			buttonId = 1,
			pos = new GUIDefines.RectInfo
			{
				leftRatio = 0.3989583f,
				topRatio = 0.6671875f,
				widthRatio = 11f / 120f,
				heightRatio = 0.1140625f,
				IPad = new GUIDefines.RectIPadInfo
				{
					leftOffset = -13f,
					topOffset = -48f
				}
			},
			style = new GUIDefines.StyleInfo
			{
				styleName = "TurboButton"
			}
		};
		base.ButtonData[1].toggleState = GameManager.Instance.EnableTurboMode && GameManager.Instance.HasAchievedTimeTrialFire(GameManager.Instance.CurrentWorld);
		base.ButtonData[1].Init();
		base.ButtonData[0].pos = new GUIDefines.RectInfo
		{
			leftRatio = 0.565625f,
			topRatio = 0.6421875f,
			widthRatio = 31f / 160f,
			heightRatio = 23f / 128f,
			IPad = new GUIDefines.RectIPadInfo
			{
				leftOffset = -27f,
				topOffset = -49f
			}
		};
		base.ButtonData[0].Init();
	}

	private void AddTurboText()
	{
		GUIDefines.LabelData[] labelData = base.LabelData;
		base.LabelData = new GUIDefines.LabelData[3];
		base.LabelData[0] = labelData[0];
		base.LabelData[1] = new GUIDefines.LabelData
		{
			pos = new GUIDefines.RectInfo
			{
				leftRatio = 0.2864583f,
				topRatio = 43f / 64f,
				widthRatio = 0.1041667f,
				heightRatio = 0.1078125f,
				IPad = new GUIDefines.RectIPadInfo
				{
					leftOffset = ((!(LocalizationManager.GetLanguageCode() == "es")) ? 9f : (-9f)),
					topOffset = -47f
				}
			},
			content = new GUIDefines.ContentInfo(),
			style = new GUIDefines.StyleInfo
			{
				useCustomStyle = true,
				customFontSize = GUIDefines.FontSize.eMedium,
				customFontType = GUIDefines.FontType.eInGame,
				useCustomTextAlignment = true,
				customNormalTextColor = GUIConstants.kLightGreyColor,
				customTextAlignment = TextAnchor.MiddleCenter
			}
		};
		base.LabelData[1].content.textId = "TXT_On";
		base.LabelData[1].Init();
		base.LabelData[2] = new GUIDefines.LabelData
		{
			pos = new GUIDefines.RectInfo
			{
				leftRatio = 23f / 96f,
				topRatio = 0.5125f,
				widthRatio = 25f / 48f,
				heightRatio = 15f / 64f,
				IPad = new GUIDefines.RectIPadInfo
				{
					leftOffset = 7f,
					topOffset = -24f
				}
			},
			content = new GUIDefines.ContentInfo
			{
				textId = "TXT_BestTime"
			},
			style = new GUIDefines.StyleInfo
			{
				useCustomStyle = true,
				customFontSize = GUIDefines.FontSize.eMedium,
				customFontType = GUIDefines.FontType.eInGame,
				useCustomTextAlignment = true,
				customNormalTextColor = GUIConstants.kLightGreyColor,
				customTextAlignment = TextAnchor.UpperCenter
			}
		};
		float timeTrialBestTime = GameManager.GetTimeTrialBestTime(GameManager.Instance.CurrentWorld);
		base.LabelData[2].content.suffixText = GameManager.GetTimeFormatedString(timeTrialBestTime);
		base.LabelData[2].invisible = timeTrialBestTime <= 0f;
		base.LabelData[2].Init();
	}

	private void ResetToDefaults()
	{
		if (base.ButtonData.Length > 1)
		{
			base.ButtonData = new GUIDefines.ButtonData[1] { base.ButtonData[0] };
			base.ButtonData[0].pos = new GUIDefines.RectInfo
			{
				leftRatio = 0.40208334f,
				topRatio = 125f / 192f,
				widthRatio = 0.19583333f,
				heightRatio = 0.1859375f,
				IPad = new GUIDefines.RectIPadInfo
				{
					topOffset = -55f
				}
			};
			base.ButtonData[0].Init();
		}
		if (base.LabelData.Length > 1)
		{
			base.LabelData = new GUIDefines.LabelData[1] { base.LabelData[0] };
		}
		base.LabelData[0].content.text = null;
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
		base.TextureData = new GUIDefines.TextureData[2]
		{
			new GUIDefines.TextureData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.1453125f,
					topRatio = 0.02265625f,
					widthRatio = 0.709375f,
					heightRatio = 0.9546875f,
					IPad = new GUIDefines.RectIPadInfo
					{
						leftOffset = 17f,
						topOffset = 23f
					}
				},
				icon = new GUIDefines.TextureInfo
				{
					name = "GUI/LevelSelect/Popups/Dialog_dropShadow"
				}
			},
			new GUIDefines.TextureData
			{
				pos = new GUIDefines.RectInfo(),
				icon = new GUIDefines.TextureInfo()
			}
		};
		base.LabelData = new GUIDefines.LabelData[1]
		{
			new GUIDefines.LabelData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 23f / 96f,
					topRatio = 0.4375f,
					widthRatio = 25f / 48f,
					heightRatio = 15f / 64f,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true
					}
				},
				content = new GUIDefines.ContentInfo(),
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customWordWrap = true,
					customFontSize = GUIDefines.FontSize.eSmall,
					customFontType = GUIDefines.FontType.eInGame,
					useCustomTextAlignment = true,
					customNormalTextColor = GUIConstants.kLightGreyColor,
					customTextAlignment = TextAnchor.UpperCenter
				}
			}
		};
		base.ButtonData = new GUIDefines.ButtonData[1]
		{
			new GUIDefines.ButtonData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.40208334f,
					topRatio = 125f / 192f,
					widthRatio = 0.19583333f,
					heightRatio = 0.1859375f,
					IPad = new GUIDefines.RectIPadInfo
					{
						topOffset = -55f
					}
				},
				content = new GUIDefines.ContentInfo
				{
					textId = "TXT_OKAllCaps"
				},
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customFontType = GUIDefines.FontType.eInGame,
					customNormalTextColor = GUIConstants.kLightGreyColor,
					customNormal = new GUIDefines.Texture2DInfo
					{
						name = "GUI/LevelSelect/Popups/Button_OK"
					},
					customActive = new GUIDefines.Texture2DInfo
					{
						name = "GUI/LevelSelect/Popups/Button_OK_pressed"
					},
					customFontSize = GUIDefines.FontSize.eLarge
				}
			}
		};
		InitPopup();
	}

	protected override void OnButtonSelect()
	{
		GameFlowManager.Instance.AudioManager.PlayUISFx(GameFlowManager.Instance.MenuClick24);
		if (base.SelectedButton == 0)
		{
			Show(false);
			if (GameObject.Find("LevelSelect") != null)
			{
				GameObject.Find("LevelSelect").GetComponent<LevelSelectManager>().RequestChangeButtonsState(true);
			}
			if (m_Callback != null)
			{
				m_Callback(base.SelectedButton);
			}
		}
		else
		{
			GameManager.Instance.EnableTurboMode = !GameManager.Instance.EnableTurboMode;
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
			m_Callback(0);
		}
	}
}
