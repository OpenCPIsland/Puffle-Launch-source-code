using UnityEngine;

public class CreditsNew : BaseMonoScreen
{
	private enum Button
	{
		eBack = 0,
		eTOU = 1,
		ePP = 2,
		eButton_COUNT = 3,
		eSupport = 4
	}

	private const float kf_TOUButtonHeight = 0.1f;

	private TermsOfUsePopup mo_termsOfUsePopup;

	private PrivacyPolicyPopup mo_privacyPolicyPopup;

	private static Vector2[][] mto_TOUButtonGroupHorizontalOffset = new Vector2[4][]
	{
		new Vector2[3]
		{
			new Vector2(0.5f, -0.025f),
			new Vector2(0.5f, -0.025f),
			new Vector2(0.5f, -0.025f)
		},
		new Vector2[3]
		{
			new Vector2(0.66f, -0.025f),
			new Vector2(0.64f, -0.025f),
			new Vector2(0.635f, -0.025f)
		},
		new Vector2[3]
		{
			new Vector2(0.53f, -0.025f),
			new Vector2(0.53f, -0.025f),
			new Vector2(0.53f, -0.025f)
		},
		new Vector2[3]
		{
			new Vector2(0.525f, -0.025f),
			new Vector2(0.525f, -0.025f),
			new Vector2(0.525f, -0.025f)
		}
	};

	private static float[][] mto_TOUButtonWidth = new float[4][]
	{
		new float[3] { 0.1f, 0.1f, 0.1f },
		new float[3] { 0.3f, 0.3f, 0.3f },
		new float[3] { 0.2f, 0.2f, 0.2f },
		new float[3] { 0.2f, 0.2f, 0.2f }
	};

	private static float[][] mto_PPButtonWidth = new float[4][]
	{
		new float[3] { 0.1f, 0.1f, 0.1f },
		new float[3] { 0.2f, 0.175f, 0.175f },
		new float[3] { 0.25f, 0.25f, 0.25f },
		new float[3] { 0.25f, 0.25f, 0.25f }
	};

	protected override void CreateMainScreenLayouts()
	{
		SetTopBarData("TXT_Back", "TXT_Credits");
		if (ResolutionManager.Instance.LayoutSize == ResolutionManager.eLayoutSize.eIPad)
		{
			base.MainScreen.TextureData = new GUIDefines.TextureData[6]
			{
				new GUIDefines.TextureData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = GUIUtil.FindHorizontalPositionToAlign(0.35253906f, 2, 0),
						topRatio = 0.2765625f,
						widthRatio = 0.35253906f,
						heightRatio = 55f / 192f,
						IPad = new GUIDefines.RectIPadInfo
						{
							keepSizeRatio = true
						}
					},
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/CreditsNew/CP_Logo_HiRes"
					}
				},
				new GUIDefines.TextureData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = GUIUtil.FindHorizontalPositionToAlign(0.15429688f, 4, 0),
						topRatio = 0.6036458f,
						widthRatio = 0.15429688f,
						heightRatio = 0.17317708f,
						IPad = new GUIDefines.RectIPadInfo
						{
							keepSizeRatio = true
						}
					},
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/CreditsNew/Behaviour_Logo_HiRes"
					}
				},
				new GUIDefines.TextureData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = GUIUtil.FindHorizontalPositionToAlign(0.253125f, 4, 1),
						topRatio = 0.6036458f,
						widthRatio = 0.23730469f,
						heightRatio = 0.17447917f,
						IPad = new GUIDefines.RectIPadInfo
						{
							keepSizeRatio = true
						}
					},
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/CreditsNew/DisneyMobile_Logo_HiRes"
					}
				},
				new GUIDefines.TextureData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = GUIUtil.FindHorizontalPositionToAlign(0.13085938f, 1, 0),
						topRatio = 0.9505208f,
						widthRatio = 0.13085938f,
						heightRatio = 0.02994792f,
						IPad = new GUIDefines.RectIPadInfo
						{
							keepSizeRatio = true
						}
					},
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/CreditsNew/Disney_Copyright_HiRes"
					}
				},
				new GUIDefines.TextureData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = GUIUtil.FindHorizontalPositionToAlign(0.00416667f, 1, 0) + 0.01041167f,
						topRatio = 0.240625f,
						widthRatio = 0.00416667f,
						heightRatio = 39f / 64f,
						IPad = new GUIDefines.RectIPadInfo
						{
							keepSizeRatio = true
						}
					},
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/CreditsNew/Divider_HiRes"
					}
				},
				new GUIDefines.TextureData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = GUIUtil.FindHorizontalPositionToAlign(31f / 128f, 2, 1),
						topRatio = 65f / 128f,
						widthRatio = 31f / 128f,
						heightRatio = 0.08723958f,
						IPad = new GUIDefines.RectIPadInfo
						{
							keepSizeRatio = true
						}
					},
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/CreditsNew/Version1_Box_HiRes"
					}
				}
			};
		}
		else
		{
			base.MainScreen.TextureData = new GUIDefines.TextureData[6]
			{
				new GUIDefines.TextureData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = GUIUtil.FindHorizontalPositionToAlign(0.37604168f, 2, 0),
						topRatio = 0.2375f,
						widthRatio = 0.37604168f,
						heightRatio = 11f / 32f,
						IPad = new GUIDefines.RectIPadInfo()
					},
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/CreditsNew/CP_Logo_HiRes"
					}
				},
				new GUIDefines.TextureData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = GUIUtil.FindHorizontalPositionToAlign(0.16458333f, 4, 0),
						topRatio = 0.6375f,
						widthRatio = 0.16458333f,
						heightRatio = 0.2078125f,
						IPad = new GUIDefines.RectIPadInfo()
					},
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/CreditsNew/Behaviour_Logo_HiRes"
					}
				},
				new GUIDefines.TextureData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = GUIUtil.FindHorizontalPositionToAlign(0.253125f, 4, 1) - 0.01041167f,
						topRatio = 0.6375f,
						widthRatio = 0.253125f,
						heightRatio = 0.209375f,
						IPad = new GUIDefines.RectIPadInfo()
					},
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/CreditsNew/DisneyMobile_Logo_HiRes"
					}
				},
				new GUIDefines.TextureData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = GUIUtil.FindHorizontalPositionToAlign(0.13958333f, 1, 0),
						topRatio = 121f / 128f,
						widthRatio = 0.13958333f,
						heightRatio = 0.0359375f,
						IPad = new GUIDefines.RectIPadInfo()
					},
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/CreditsNew/Disney_Copyright_HiRes"
					}
				},
				new GUIDefines.TextureData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = GUIUtil.FindHorizontalPositionToAlign(0.00416667f, 1, 0) + 0.01041167f,
						topRatio = 0.240625f,
						widthRatio = 0.00416667f,
						heightRatio = 39f / 64f,
						IPad = new GUIDefines.RectIPadInfo()
					},
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/CreditsNew/Divider_HiRes"
					}
				},
				new GUIDefines.TextureData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = GUIUtil.FindHorizontalPositionToAlign(31f / 120f, 2, 1),
						topRatio = 65f / 128f,
						widthRatio = 31f / 120f,
						heightRatio = 0.1046875f,
						IPad = new GUIDefines.RectIPadInfo
						{
							keepSizeRatio = true
						}
					},
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/CreditsNew/Version1_Box_HiRes"
					}
				}
			};
		}
		GUIDefines.RectInfo rectInfo = null;
		string customOnDemandFontName = string.Empty;
		string textId = null;
		switch (ResolutionManager.Instance.LayoutSize)
		{
		case ResolutionManager.eLayoutSize.eLowres:
		{
			GUIDefines.RectInfo rectInfo2 = new GUIDefines.RectInfo();
			rectInfo2.leftRatio = GUIUtil.FindHorizontalPositionToAlign(1f, 1, 0);
			rectInfo2.topRatio = 0.9025f;
			rectInfo2.widthRatio = 1f;
			rectInfo2.heightRatio = 3f / 64f;
			rectInfo2.IPad = new GUIDefines.RectIPadInfo
			{
				keepSizeRatio = true,
				topOffset = -48f
			};
			rectInfo = rectInfo2;
			customOnDemandFontName = GameFlowManager.Instance.GUIManager.GetLowResFontName(GUIDefines.FontType.eCPMenus, GUIDefines.FontSize.eMini);
			textId = "TXT_TermsAndPrivacyTap";
			break;
		}
		case ResolutionManager.eLayoutSize.eOriginal:
		{
			GUIDefines.RectInfo rectInfo2 = new GUIDefines.RectInfo();
			rectInfo2.leftRatio = GUIUtil.FindHorizontalPositionToAlign(1f, 1, 0);
			rectInfo2.topRatio = 0.9f;
			rectInfo2.widthRatio = 1f;
			rectInfo2.heightRatio = 3f / 64f;
			rectInfo2.IPad = new GUIDefines.RectIPadInfo
			{
				keepSizeRatio = true,
				topOffset = -48f
			};
			rectInfo = rectInfo2;
			customOnDemandFontName = GameFlowManager.Instance.GUIManager.GetLowResFontName(GUIDefines.FontType.eCPMenus, GUIDefines.FontSize.eMedium);
			textId = "TXT_TermsAndPrivacyTap";
			break;
		}
		case ResolutionManager.eLayoutSize.eIPad:
		{
			GUIDefines.RectInfo rectInfo2 = new GUIDefines.RectInfo();
			rectInfo2.leftRatio = GUIUtil.FindHorizontalPositionToAlign(1f, 1, 0);
			rectInfo2.topRatio = 0.97f;
			rectInfo2.widthRatio = 1f;
			rectInfo2.heightRatio = 3f / 64f;
			rectInfo2.IPad = new GUIDefines.RectIPadInfo
			{
				keepSizeRatio = true,
				topOffset = -48f
			};
			rectInfo = rectInfo2;
			customOnDemandFontName = GameFlowManager.Instance.GUIManager.GetLowResFontName(GUIDefines.FontType.eCPMenus, GUIDefines.FontSize.eMedium);
			textId = "TXT_TermsAndPrivacyTap";
			break;
		}
		}
		base.MainScreen.ButtonData = new GUIDefines.ButtonData[3]
		{
			new GUIDefines.ButtonData
			{
				buttonId = 1,
				pos = GetTermsOfUseButtonPos(rectInfo, Button.eTOU),
				detectZoneScale = 1.1f,
				content = new GUIDefines.ContentInfo(),
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true
				}
			},
			new GUIDefines.ButtonData
			{
				buttonId = 4,
				pos = GetSupportButtonPos(rectInfo, Button.eSupport),
				detectZoneScale = 1.1f,
				content = new GUIDefines.ContentInfo(),
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true
				}
			},
			new GUIDefines.ButtonData
			{
				buttonId = 2,
				pos = GetTermsOfUseButtonPos(rectInfo, Button.ePP),
				detectZoneScale = 1.1f,
				content = new GUIDefines.ContentInfo(),
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true
				}
			}
		};
		base.MainScreen.LabelData = new GUIDefines.LabelData[10]
		{
			new GUIDefines.LabelData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = GUIUtil.FindHorizontalPositionToAlign(0.5f, 2, 0),
					topRatio = 29f / 128f,
					widthRatio = 0.5f,
					heightRatio = 3f / 64f,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true,
						topOffset = 28f
					}
				},
				content = new GUIDefines.ContentInfo
				{
					textId = "TXT_Credits_1"
				},
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customWordWrap = true,
					customFontSize = GUIDefines.FontSize.eSmall,
					useCustomTextAlignment = true,
					customTextAlignment = TextAnchor.MiddleCenter
				}
			},
			new GUIDefines.LabelData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = GUIUtil.FindHorizontalPositionToAlign(0.25f, 4, 0),
					topRatio = 0.5625f,
					widthRatio = 0.25f,
					heightRatio = 3f / 64f,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true,
						topOffset = -11f
					}
				},
				content = new GUIDefines.ContentInfo
				{
					textId = "TXT_Credits_2"
				},
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customWordWrap = true,
					customFontSize = GUIDefines.FontSize.eSmall,
					useCustomTextAlignment = true,
					customTextAlignment = TextAnchor.MiddleCenter
				}
			},
			new GUIDefines.LabelData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = GUIUtil.FindHorizontalPositionToAlign(0.25f, 4, 1),
					topRatio = 0.5625f,
					widthRatio = 0.25f,
					heightRatio = 3f / 64f,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true,
						topOffset = -11f
					}
				},
				content = new GUIDefines.ContentInfo
				{
					textId = "TXT_Credits_3"
				},
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customWordWrap = true,
					customFontSize = GUIDefines.FontSize.eSmall,
					useCustomTextAlignment = true,
					customTextAlignment = TextAnchor.MiddleCenter
				}
			},
			new GUIDefines.LabelData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = GUIUtil.FindHorizontalPositionToAlign(0.5f, 2, 1),
					topRatio = 29f / 128f,
					widthRatio = 0.5f,
					heightRatio = 3f / 64f,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true,
						topOffset = 30f
					}
				},
				content = new GUIDefines.ContentInfo
				{
					textId = "TXT_Credits_4"
				},
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customWordWrap = true,
					customFontSize = GUIDefines.FontSize.eSmall,
					useCustomTextAlignment = true,
					customTextAlignment = TextAnchor.MiddleCenter
				}
			},
			new GUIDefines.LabelData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = GUIUtil.FindHorizontalPositionToAlign(0.5f, 2, 1),
					topRatio = 47f / 160f,
					widthRatio = 0.5f,
					heightRatio = 3f / 64f,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true,
						topOffset = 20f
					}
				},
				content = new GUIDefines.ContentInfo
				{
					textId = "TXT_Credits_5"
				},
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customWordWrap = true,
					customFontSize = GUIDefines.FontSize.eMedium,
					useCustomTextAlignment = true,
					customTextAlignment = TextAnchor.MiddleCenter
				}
			},
			new GUIDefines.LabelData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = GUIUtil.FindHorizontalPositionToAlign(0.5f, 2, 1),
					topRatio = 23f / 64f,
					widthRatio = 0.5f,
					heightRatio = 3f / 64f,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true,
						topOffset = 10f
					}
				},
				content = new GUIDefines.ContentInfo
				{
					textId = "TXT_Credits_6"
				},
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customWordWrap = true,
					customFontSize = GUIDefines.FontSize.eMedium,
					useCustomTextAlignment = true,
					customTextAlignment = TextAnchor.MiddleCenter
				}
			},
			new GUIDefines.LabelData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = GUIUtil.FindHorizontalPositionToAlign(31f / 120f, 2, 1),
					topRatio = 65f / 128f,
					widthRatio = 31f / 120f,
					heightRatio = 0.1046875f,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true,
						topOffset = -6f
					}
				},
				content = new GUIDefines.ContentInfo
				{
					text = LocalizationManager.Instance.GetString("TXT_Credits_7", Utilities.CurrentBuildString)
				},
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customWordWrap = true,
					customFontSize = GUIDefines.FontSize.eSmall,
					useCustomTextAlignment = true,
					customTextAlignment = TextAnchor.MiddleCenter
				}
			},
			new GUIDefines.LabelData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = GUIUtil.FindHorizontalPositionToAlign(0.5f, 2, 1),
					topRatio = 0.7375f,
					widthRatio = 0.5f,
					heightRatio = 3f / 64f,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true,
						topOffset = -38f
					}
				},
				content = new GUIDefines.ContentInfo
				{
					textId = "TXT_Credits_8"
				},
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customWordWrap = true,
					customFontSize = GUIDefines.FontSize.eSmall,
					useCustomTextAlignment = true,
					customTextAlignment = TextAnchor.MiddleCenter
				}
			},
			new GUIDefines.LabelData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = GUIUtil.FindHorizontalPositionToAlign(0.5f, 2, 1),
					topRatio = 129f / 160f,
					widthRatio = 0.5f,
					heightRatio = 3f / 64f,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true,
						topOffset = -48f
					}
				},
				content = new GUIDefines.ContentInfo
				{
					textId = "TXT_Credits_9"
				},
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customWordWrap = true,
					customFontSize = GUIDefines.FontSize.eSmall,
					useCustomTextAlignment = true,
					customTextAlignment = TextAnchor.MiddleCenter
				}
			},
			new GUIDefines.LabelData
			{
				pos = rectInfo,
				content = new GUIDefines.ContentInfo
				{
					textId = textId
				},
				disableDropShadow = true,
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customWordWrap = true,
					customFontType = GUIDefines.FontType.eOnDemand,
					customOnDemandFontName = customOnDemandFontName,
					customNormalTextColor = GUIConstants.kTOULinkColorColor,
					useCustomTextAlignment = true,
					customTextAlignment = TextAnchor.MiddleCenter
				}
			}
		};
	}

	private void Awake()
	{
		Init(base.gameObject);
		mo_termsOfUsePopup = new TermsOfUsePopup(base.gameObject);
		mo_termsOfUsePopup.RegisterCallback(TermsOfUsePopupCallback);
		mo_privacyPolicyPopup = new PrivacyPolicyPopup(base.gameObject);
		mo_privacyPolicyPopup.RegisterCallback(PrivacyPolicyPopupCallback);
	}

	private void TermsOfUsePopupCallback(int aSelectedButton)
	{
	}

	private void PrivacyPolicyPopupCallback(int aSelectedButton)
	{
	}

	private void OnGUI()
	{
		if (base.MainScreen.CanDraw())
		{
			base.MainScreen.Draw();
			DrawTopBar();
			mo_termsOfUsePopup.Draw();
			mo_privacyPolicyPopup.Draw();
			BlockControl(mo_termsOfUsePopup.IsShowing || mo_privacyPolicyPopup.IsShowing);
		}
	}

	private new void Update()
	{
		HandleButtonSelect();
		if (mo_termsOfUsePopup != null)
		{
			mo_termsOfUsePopup.Update();
		}
		if (mo_privacyPolicyPopup != null)
		{
			mo_privacyPolicyPopup.Update();
		}
	}

	protected override void OnMainScreenButtonSelect()
	{
		switch ((Button)base.MainScreen.SelectedButton)
		{
		case Button.eTOU:
			Application.OpenURL("http://disneytermsofuse.com/");
			break;
		case Button.ePP:
			Application.OpenURL("https://disneyprivacycenter.com/");
			break;
		case Button.eSupport:
			Application.OpenURL("http://help.disney.com/clubpenguin");
			break;
		case Button.eButton_COUNT:
			break;
		}
	}

	protected override void OnBack()
	{
		GameFlowManager.Instance.AudioManager.PlayUISFx(GameFlowManager.Instance.MenuClick24);
		base.MainScreen.StopGUI();
		GameFlowManager.Instance.LoadScene("!Loader_MainMenu", false);
	}

	private static GUIDefines.RectInfo GetTermsOfUseButtonPos(GUIDefines.RectInfo o_textPos, Button e_button)
	{
		int num = 0;
		switch (LocalizationManager.GetLanguageCode())
		{
		case "en":
			num = 0;
			break;
		case "fr":
			num = 1;
			break;
		case "pt":
			num = 2;
			break;
		case "es":
			num = 3;
			break;
		}
		int layoutSize = (int)ResolutionManager.Instance.LayoutSize;
		float num2 = 0f;
		float num3 = 0f;
		if (e_button == Button.eTOU)
		{
			num2 = mto_TOUButtonWidth[num][layoutSize];
			num3 = 0f - num2;
		}
		else
		{
			num2 = mto_PPButtonWidth[num][layoutSize];
		}
		GUIDefines.RectInfo rectInfo = new GUIDefines.RectInfo();
		rectInfo.leftRatio = o_textPos.leftRatio + mto_TOUButtonGroupHorizontalOffset[num][layoutSize].x + num3;
		rectInfo.topRatio = o_textPos.topRatio + mto_TOUButtonGroupHorizontalOffset[num][layoutSize].y;
		rectInfo.widthRatio = num2;
		rectInfo.heightRatio = 0.1f;
		rectInfo.IPad = o_textPos.IPad;
		return rectInfo;
	}

	private static GUIDefines.RectInfo GetSupportButtonPos(GUIDefines.RectInfo o_textPos, Button e_button)
	{
		GUIDefines.RectInfo rectInfo = new GUIDefines.RectInfo();
		rectInfo.leftRatio = GUIUtil.FindHorizontalPositionToAlign(0.5f, 2, 1);
		rectInfo.topRatio = 129f / 160f;
		rectInfo.widthRatio = 0.5f;
		rectInfo.heightRatio = 3f / 64f;
		rectInfo.IPad = new GUIDefines.RectIPadInfo
		{
			keepSizeRatio = true,
			topOffset = -48f
		};
		return rectInfo;
	}
}
