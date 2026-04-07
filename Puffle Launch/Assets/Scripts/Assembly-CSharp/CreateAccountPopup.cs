using UnityEngine;

public class CreateAccountPopup : BasePopup
{
	public enum Button
	{
		eColorPicker = 0,
		eCreatePenguin = 1,
		eTOU = 2,
		ePP = 3,
		eBack = 4,
		eButton_COUNT = 5
	}

	public enum CreateAccountTextField
	{
		eName = 0,
		eEmail = 1,
		ePassword = 2,
		eRetypePassword = 3,
		eCount = 4,
		eNone = 5
	}

	public enum CreateAccountTexture
	{
		eErrorHighlight = 0,
		ePenguinGreyNoEyes = 1,
		ePenguinDarkShadow = 2,
		ePenguinHighlight = 3,
		ePenguinInside = 4,
		eCount = 5,
		eNone = 6
	}

	public enum CreateAccountLabel
	{
		eErrorBubble = 0,
		eChoseYourColor = 1,
		eCount = 2,
		eNone = 3
	}

	private GUIDefines.TextureData[] mto_textFieldErrorHighlights;

	private GUIDefines.TextureData[] mto_bgTexture;

	private ColorPickerPopup m_ColorPickerPopup;

	private ColorPickerZone m_ColorPickerZone;

	private TermsOfUsePopup mo_termsOfUsePopup;

	private PrivacyPolicyPopup mo_privacyPolicyPopup;

	private GUIDefines.RectInfo[] mto_textFieldPositions;

	private GUIDefines.RectInfo[] mto_errorBubblePositions;

	private static Vector2[][] mto_TOUButtonGroupHorizontalOffset = new Vector2[4][]
	{
		new Vector2[3]
		{
			new Vector2(0.175f, 0f),
			new Vector2(0.175f, -0.005f),
			new Vector2(0.165f, 0f)
		},
		new Vector2[3]
		{
			new Vector2(0.285f, 0f),
			new Vector2(0.26f, -0.005f),
			new Vector2(0.225f, 0f)
		},
		new Vector2[3]
		{
			new Vector2(0.195f, 0f),
			new Vector2(0.195f, -0.005f),
			new Vector2(0.185f, 0f)
		},
		new Vector2[3]
		{
			new Vector2(0.185f, 0f),
			new Vector2(0.185f, -0.005f),
			new Vector2(0.18f, 0f)
		}
	};

	private static float[][] mto_TOUButtonWidth = new float[4][]
	{
		new float[3] { 0.15f, 0.15f, 0.075f },
		new float[3] { 0.275f, 0.275f, 0.175f },
		new float[3] { 0.175f, 0.175f, 0.125f },
		new float[3] { 0.175f, 0.175f, 0.125f }
	};

	private static float[][] mto_PPButtonWidth = new float[4][]
	{
		new float[3] { 0.2f, 0.2f, 0.15f },
		new float[3] { 0.2f, 0.2f, 0.15f },
		new float[3] { 0.3f, 0.25f, 0.175f },
		new float[3] { 0.3f, 0.25f, 0.175f }
	};

	private static float[] mtf_TOUButtonHeight = new float[3] { 0.085f, 0.085f, 0.065f };

	public CreateAccountPopup(GameObject aRefObj)
		: base(aRefObj)
	{
		if (ResolutionManager.Instance.LayoutSize == ResolutionManager.eLayoutSize.eIPad)
		{
			m_ColorPickerZone = new ColorPickerZone(aRefObj, new GUIDefines.RectInfo
			{
				leftRatio = 0.5614974f,
				topRatio = 0.64427084f,
				widthRatio = 0.416042f,
				heightRatio = 0.15760417f
			}, new Vector2(0.05371094f, 0.06901042f), null, "GUI/CreateAccountNew/IPad/ColorPicker/Create_Account_ColorBox", null, "GUI/CreateAccountNew/IPad/ColorPicker/Create_Account_ColorBox_HiLite", 7, 2);
			m_ColorPickerZone.RegisterCallback(ColorPickupPopupCallback);
		}
		else
		{
			m_ColorPickerPopup = new ColorPickerPopup(aRefObj, new GUIDefines.RectInfo
			{
				leftRatio = 0.04166667f,
				topRatio = 0.1921875f,
				widthRatio = 11f / 12f,
				heightRatio = 0.615625f
			}, new GUIDefines.RectInfo
			{
				leftRatio = 3f / 32f,
				topRatio = 0.2684375f,
				widthRatio = 0.8125f,
				heightRatio = 5f / 64f
			}, GUIConstants.kWhiteColor, new GUIDefines.RectInfo
			{
				leftRatio = 0.09256133f,
				topRatio = 0.3921875f,
				widthRatio = 0.81562465f,
				heightRatio = 0.3625f
			}, new Vector2(0.10520833f, 0.1515625f), "GUI/CreateAccountNew/ColorPicker/Create_Account_ColorPopUpWindow", null, "GUI/CreateAccountNew/ColorPicker/Create_Account_ColorBox", null, "GUI/CreateAccountNew/ColorPicker/Create_Account_ColorBox_HiLite", 7, 2);
			m_ColorPickerPopup.RegisterCallback(ColorPickupPopupCallback);
		}
		mo_termsOfUsePopup = new TermsOfUsePopup(aRefObj);
		mo_termsOfUsePopup.RegisterCallback(TermsOfUsePopupCallback);
		mo_privacyPolicyPopup = new PrivacyPolicyPopup(aRefObj);
		mo_privacyPolicyPopup.RegisterCallback(PrivacyPolicyPopupCallback);
	}

	protected override void CreateLayouts()
	{
		GUIDefines.RectInfo rectInfo = null;
		string customOnDemandFontName = string.Empty;
		switch (ResolutionManager.Instance.LayoutSize)
		{
		case ResolutionManager.eLayoutSize.eLowres:
		{
			GUIDefines.RectInfo rectInfo2 = new GUIDefines.RectInfo();
			rectInfo2.leftRatio = 0.51f;
			rectInfo2.topRatio = 93f / 128f;
			rectInfo2.widthRatio = 0.48f;
			rectInfo2.heightRatio = 0.071875f;
			rectInfo = rectInfo2;
			customOnDemandFontName = GameFlowManager.Instance.GUIManager.GetLowResFontName(GUIDefines.FontType.eCPMenus, GUIDefines.FontSize.eMini);
			break;
		}
		case ResolutionManager.eLayoutSize.eOriginal:
		{
			GUIDefines.RectInfo rectInfo2 = new GUIDefines.RectInfo();
			rectInfo2.leftRatio = 0.52f;
			rectInfo2.topRatio = 93f / 128f;
			rectInfo2.widthRatio = 0.46f;
			rectInfo2.heightRatio = 0.071875f;
			rectInfo = rectInfo2;
			customOnDemandFontName = GameFlowManager.Instance.GUIManager.GetLowResFontName(GUIDefines.FontType.eCPMenus, GUIDefines.FontSize.eMedium);
			break;
		}
		case ResolutionManager.eLayoutSize.eIPad:
		{
			GUIDefines.RectInfo rectInfo2 = new GUIDefines.RectInfo();
			rectInfo2.leftRatio = 0.5688281f;
			rectInfo2.topRatio = 0.7994272f;
			rectInfo2.widthRatio = 0.406f;
			rectInfo2.heightRatio = 9f / 128f;
			rectInfo2.IPad = new GUIDefines.RectIPadInfo
			{
				keepSizeRatio = true
			};
			rectInfo = rectInfo2;
			customOnDemandFontName = GameFlowManager.Instance.GUIManager.GetLowResFontName(GUIDefines.FontType.eCPMenus, GUIDefines.FontSize.eSmall);
			break;
		}
		}
		base.WindowData = new GUIDefines.WindowData
		{
			pos = new GUIDefines.RectInfo
			{
				widthRatio = 1f,
				heightRatio = 1f,
				detatchFromRefObject = true
			},
			id = 11
		};
		m_WindowBackground = null;
		ResolutionManager.eLayoutSize layoutSize = ResolutionManager.Instance.LayoutSize;
		if (layoutSize == ResolutionManager.eLayoutSize.eIPad)
		{
			mto_textFieldPositions = new GUIDefines.RectInfo[4]
			{
				new GUIDefines.RectInfo
				{
					leftRatio = 0.5458984f,
					topRatio = 0.121875f,
					widthRatio = 0.42285156f,
					heightRatio = 0.10677083f,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true
					}
				},
				new GUIDefines.RectInfo
				{
					leftRatio = 0.5458984f,
					topRatio = 0.2390625f,
					widthRatio = 0.42285156f,
					heightRatio = 0.10677083f,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true
					}
				},
				new GUIDefines.RectInfo
				{
					leftRatio = 0.5458984f,
					topRatio = 57f / 160f,
					widthRatio = 0.42285156f,
					heightRatio = 0.10677083f,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true
					}
				},
				new GUIDefines.RectInfo
				{
					leftRatio = 0.5458984f,
					topRatio = 0.4734375f,
					widthRatio = 0.42285156f,
					heightRatio = 0.10677083f,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true
					}
				}
			};
			mto_errorBubblePositions = new GUIDefines.RectInfo[4];
			float widthRatio = 0.5145833f;
			float heightRatio = 19f / 128f;
			float leftRatio = 0.01041664f;
			float num = -0.02604167f;
			for (int i = 0; i < 4; i++)
			{
				mto_errorBubblePositions[i] = new GUIDefines.RectInfo
				{
					leftRatio = leftRatio,
					topRatio = mto_textFieldPositions[i].topRatio + num,
					widthRatio = widthRatio,
					heightRatio = heightRatio,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true
					}
				};
			}
			mto_bgTexture = new GUIDefines.TextureData[1]
			{
				new GUIDefines.TextureData
				{
					pos = new GUIDefines.RectInfo
					{
						widthRatio = 1f,
						heightRatio = 1f,
						detatchFromRefObject = true
					},
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/CreateAccountNew/Create_Account_BG"
					}
				}
			};
			for (int j = 0; j < mto_bgTexture.Length; j++)
			{
				mto_bgTexture[j].Init();
			}
			base.ButtonData = new GUIDefines.ButtonData[4]
			{
				new GUIDefines.ButtonData
				{
					buttonId = 1,
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.5489583f,
						topRatio = 0.8625f,
						widthRatio = 0.41308594f,
						heightRatio = 0.12369792f,
						IPad = new GUIDefines.RectIPadInfo
						{
							keepSizeRatio = true
						}
					},
					content = new GUIDefines.ContentInfo
					{
						textId = "TXT_CreateYourPenguin"
					},
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customNormal = new GUIDefines.Texture2DInfo
						{
							name = "GUI/Common/button"
						},
						customActive = new GUIDefines.Texture2DInfo
						{
							name = "GUI/Common/button_pressed"
						},
						customFontSize = GUIDefines.FontSize.eMedium
					},
					autoResizeAllignment = GUIDefines.AutoResizeAllignment.eCenter
				},
				new GUIDefines.ButtonData
				{
					buttonId = 2,
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
					buttonId = 3,
					pos = GetTermsOfUseButtonPos(rectInfo, Button.ePP),
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
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.02133333f,
						topRatio = 0.02589583f,
						widthRatio = 0.10839844f,
						heightRatio = 0.06640625f,
						IPad = new GUIDefines.RectIPadInfo
						{
							keepSizeRatio = true,
							topOffset = -4f
						}
					},
					detectZoneScale = 1.5f,
					content = new GUIDefines.ContentInfo(),
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customNormal = new GUIDefines.Texture2DInfo
						{
							name = "GUI/Common/iPad/Create_Account_BackBtn_iPad"
						},
						customActive = new GUIDefines.Texture2DInfo
						{
							name = "GUI/Common/iPad/Create_Account_BackBtn_pressed_iPad"
						}
					}
				}
			};
			GUIDefines.RectInfo rectInfo2 = new GUIDefines.RectInfo();
			rectInfo2.leftRatio = 0.03645833f;
			rectInfo2.topRatio = 0.196875f;
			rectInfo2.widthRatio = 0.40039062f;
			rectInfo2.heightRatio = 2f / 3f;
			rectInfo2.IPad = new GUIDefines.RectIPadInfo
			{
				keepSizeRatio = true
			};
			GUIDefines.RectInfo pos = rectInfo2;
			base.TextureData = new GUIDefines.TextureData[6]
			{
				new GUIDefines.TextureData
				{
					pos = mto_textFieldPositions[2],
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/CreateAccountNew/Create_Account_FormBox_Error"
					},
					invisible = true
				},
				new GUIDefines.TextureData
				{
					pos = pos,
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/CreateAccountNew/Penguin/fat_penguin_grey_noeyes"
					},
					bgInfo = new GUIDefines.BackgroundInfo
					{
						useBgColor = true,
						bgColor = Utilities.m_cPenguinColors[0]
					}
				},
				new GUIDefines.TextureData
				{
					pos = pos,
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/CreateAccountNew/Penguin/fat_penguin_dark_shadow"
					},
					bgInfo = new GUIDefines.BackgroundInfo
					{
						useBgColor = true,
						bgColor = Utilities.m_cPenguinShadowColors[0]
					}
				},
				new GUIDefines.TextureData
				{
					pos = pos,
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/CreateAccountNew/Penguin/fat_penguin_highlight"
					},
					bgInfo = new GUIDefines.BackgroundInfo
					{
						useBgColor = true,
						bgColor = Utilities.m_cPenguinHightlightColors[0]
					}
				},
				new GUIDefines.TextureData
				{
					pos = pos,
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/CreateAccountNew/Penguin/fat_penguin_inside"
					}
				},
				new GUIDefines.TextureData
				{
					pos = new GUIDefines.RectInfo
					{
						widthRatio = 1f,
						heightRatio = 0.10677083f,
						IPad = new GUIDefines.RectIPadInfo
						{
							keepSizeRatio = true
						}
					},
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/Common/iPad/Create_Account_NavBar"
					}
				}
			};
			base.LabelData = new GUIDefines.LabelData[4]
			{
				new GUIDefines.LabelData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.01041664f,
						widthRatio = 0.5145833f,
						heightRatio = 19f / 128f,
						IPad = new GUIDefines.RectIPadInfo
						{
							leftOffset = 34f
						}
					},
					content = new GUIDefines.ContentInfo(),
					invisible = true,
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customNormal = new GUIDefines.Texture2DInfo
						{
							name = "GUI/CreateAccountNew/Create_Account_ErrorBox"
						},
						customFontSize = GUIDefines.FontSize.eSmall,
						customNormalTextColor = GUIConstants.kRedColor,
						customWordWrap = true,
						customPadding = new GUIDefines.Vector2Info
						{
							xRatio = 0.009375f,
							yRatio = 1f / 128f
						},
						customPadding2 = new GUIDefines.Vector2Info
						{
							xRatio = 0.03645833f
						}
					}
				},
				new GUIDefines.LabelData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.55334675f,
						topRatio = 0.5869793f,
						widthRatio = 0.5f,
						heightRatio = 7f / 160f,
						IPad = new GUIDefines.RectIPadInfo
						{
							keepSizeRatio = true
						}
					},
					content = new GUIDefines.ContentInfo
					{
						textId = "TXT_ChooseColor"
					},
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customFontSize = GUIDefines.FontSize.eSmall,
						customNormalTextColor = GUIConstants.kWhiteColor,
						useCustomTextAlignment = true,
						customTextAlignment = TextAnchor.MiddleLeft,
						customWordWrap = true
					}
				},
				new GUIDefines.LabelData
				{
					pos = new GUIDefines.RectInfo
					{
						topRatio = 7f / 160f,
						widthRatio = 1f,
						heightRatio = 7f / 160f,
						IPad = new GUIDefines.RectIPadInfo
						{
							keepSizeRatio = true,
							topOffset = -10f
						}
					},
					content = new GUIDefines.ContentInfo
					{
						textId = "TXT_CPAccount"
					}
				},
				new GUIDefines.LabelData
				{
					pos = rectInfo,
					content = new GUIDefines.ContentInfo
					{
						textId = "TXT_TOU_Link"
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
			base.TextFieldData = new GUIDefines.TextFieldData[4]
			{
				new GUIDefines.TextFieldData
				{
					controlName = "Name",
					pos = mto_textFieldPositions[0],
					maxLength = 20,
					defaultTextId = "TXT_PenguinName",
					titleCase = true,
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customNormal = new GUIDefines.Texture2DInfo
						{
							name = "GUI/CreateAccountNew/Create_Account_FormBox"
						},
						customActive = new GUIDefines.Texture2DInfo
						{
							name = "GUI/CreateAccountNew/Create_Account_FormBox_Focus"
						},
						customActiveTextColor = GUIConstants.kBlueColor,
						customFocusedTextColor = GUIConstants.kBlueColor
					}
				},
				new GUIDefines.TextFieldData
				{
					controlName = "Email",
					pos = mto_textFieldPositions[1],
					maxLength = 40,
					defaultTextId = "TXT_Email",
					keyboardType = TouchScreenKeyboardType.EmailAddress,
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customNormal = new GUIDefines.Texture2DInfo
						{
							name = "GUI/CreateAccountNew/Create_Account_FormBox"
						},
						customActive = new GUIDefines.Texture2DInfo
						{
							name = "GUI/CreateAccountNew/Create_Account_FormBox_Focus"
						},
						customActiveTextColor = GUIConstants.kBlueColor,
						customFocusedTextColor = GUIConstants.kBlueColor
					}
				},
				new GUIDefines.TextFieldData
				{
					controlName = "Password",
					pos = mto_textFieldPositions[2],
					isPassword = true,
					maxLength = 20,
					defaultTextId = "TXT_Password",
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customNormal = new GUIDefines.Texture2DInfo
						{
							name = "GUI/CreateAccountNew/Create_Account_FormBox"
						},
						customActive = new GUIDefines.Texture2DInfo
						{
							name = "GUI/CreateAccountNew/Create_Account_FormBox_Focus"
						},
						customActiveTextColor = GUIConstants.kBlueColor,
						customFocusedTextColor = GUIConstants.kBlueColor
					}
				},
				new GUIDefines.TextFieldData
				{
					controlName = "ReTypePassword",
					pos = mto_textFieldPositions[3],
					isPassword = true,
					maxLength = 20,
					defaultTextId = "TXT_ReTypePassword",
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customNormal = new GUIDefines.Texture2DInfo
						{
							name = "GUI/CreateAccountNew/Create_Account_FormBox"
						},
						customActive = new GUIDefines.Texture2DInfo
						{
							name = "GUI/CreateAccountNew/Create_Account_FormBox_Focus"
						},
						customActiveTextColor = GUIConstants.kBlueColor,
						customFocusedTextColor = GUIConstants.kBlueColor
					}
				}
			};
			mto_textFieldErrorHighlights = new GUIDefines.TextureData[4];
			for (int k = 0; k < 4; k++)
			{
				mto_textFieldErrorHighlights[k] = new GUIDefines.TextureData
				{
					pos = mto_textFieldPositions[k],
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/CreateAccountNew/Create_Account_FormBox_Error"
					}
				};
				mto_textFieldErrorHighlights[k].Init();
				base.TextFieldData[k].pos = mto_textFieldPositions[k];
			}
		}
		else
		{
			mto_textFieldPositions = new GUIDefines.RectInfo[4]
			{
				new GUIDefines.RectInfo
				{
					leftRatio = 0.530208f,
					topRatio = 9f / 64f,
					widthRatio = 0.4354167f,
					heightRatio = 0.1234375f
				},
				new GUIDefines.RectInfo
				{
					leftRatio = 0.530208f,
					topRatio = 19f / 64f,
					widthRatio = 0.4354167f,
					heightRatio = 0.1234375f
				},
				new GUIDefines.RectInfo
				{
					leftRatio = 0.530208f,
					topRatio = 29f / 64f,
					widthRatio = 0.4354167f,
					heightRatio = 0.1234375f
				},
				new GUIDefines.RectInfo
				{
					leftRatio = 0.530208f,
					topRatio = 39f / 64f,
					widthRatio = 0.4354167f,
					heightRatio = 0.1234375f
				}
			};
			mto_errorBubblePositions = new GUIDefines.RectInfo[4];
			float widthRatio2 = 0.5145833f;
			float heightRatio2 = 19f / 128f;
			float leftRatio2 = 0.01041664f;
			float num2 = -1f / 64f;
			for (int l = 0; l < 4; l++)
			{
				mto_errorBubblePositions[l] = new GUIDefines.RectInfo
				{
					leftRatio = leftRatio2,
					topRatio = mto_textFieldPositions[l].topRatio + num2,
					widthRatio = widthRatio2,
					heightRatio = heightRatio2,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true
					}
				};
			}
			mto_bgTexture = new GUIDefines.TextureData[1]
			{
				new GUIDefines.TextureData
				{
					pos = new GUIDefines.RectInfo
					{
						widthRatio = 1f,
						heightRatio = 1f,
						detatchFromRefObject = true
					},
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/CreateAccountNew/Create_Account_BG"
					}
				}
			};
			for (int m = 0; m < mto_bgTexture.Length; m++)
			{
				mto_bgTexture[m].Init();
			}
			base.ButtonData = new GUIDefines.ButtonData[5]
			{
				new GUIDefines.ButtonData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.025f,
						topRatio = 5f / 32f,
						widthRatio = 0.10833333f,
						heightRatio = 0.1625f
					},
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customNormal = new GUIDefines.Texture2DInfo
						{
							name = "GUI/CreateAccountNew/Create_Account_ColorPicker"
						},
						customActive = new GUIDefines.Texture2DInfo
						{
							name = "GUI/CreateAccountNew/Create_Account_ColorPicker_pressed"
						}
					}
				},
				new GUIDefines.ButtonData
				{
					buttonId = 2,
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
					buttonId = 3,
					pos = GetTermsOfUseButtonPos(rectInfo, Button.ePP),
					detectZoneScale = 1.1f,
					content = new GUIDefines.ContentInfo(),
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true
					}
				},
				new GUIDefines.ButtonData
				{
					buttonId = 1,
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 8f / 15f,
						topRatio = 103f / 128f,
						widthRatio = 0.425f,
						heightRatio = 0.165625f
					},
					content = new GUIDefines.ContentInfo
					{
						textId = "TXT_CreateYourPenguin"
					},
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customNormal = new GUIDefines.Texture2DInfo
						{
							name = "GUI/Common/button"
						},
						customActive = new GUIDefines.Texture2DInfo
						{
							name = "GUI/Common/button_pressed"
						},
						customFontSize = GUIDefines.FontSize.eMedium
					},
					autoResizeAllignment = GUIDefines.AutoResizeAllignment.eCenter
				},
				new GUIDefines.ButtonData
				{
					buttonId = 4,
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 3f / 160f,
						topRatio = 0.021875f,
						widthRatio = 7f / 64f,
						heightRatio = 0.0796875f
					},
					detectZoneScale = 1.5f,
					content = new GUIDefines.ContentInfo(),
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customNormal = new GUIDefines.Texture2DInfo
						{
							name = "GUI/Common/Create_Account_BackBtn"
						},
						customActive = new GUIDefines.Texture2DInfo
						{
							name = "GUI/Common/Create_Account_BackBtn_pressed"
						},
						customFontSize = GUIDefines.FontSize.eMedium
					}
				}
			};
			GUIDefines.RectInfo rectInfo2 = new GUIDefines.RectInfo();
			rectInfo2.leftRatio = 0.03645833f;
			rectInfo2.topRatio = 0.196875f;
			rectInfo2.widthRatio = 0.42916667f;
			rectInfo2.heightRatio = 0.8f;
			GUIDefines.RectInfo pos2 = rectInfo2;
			base.TextureData = new GUIDefines.TextureData[6]
			{
				new GUIDefines.TextureData
				{
					pos = mto_textFieldPositions[2],
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/CreateAccountNew/Create_Account_FormBox_Error"
					},
					invisible = true
				},
				new GUIDefines.TextureData
				{
					pos = pos2,
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/CreateAccountNew/Penguin/fat_penguin_grey_noeyes"
					},
					bgInfo = new GUIDefines.BackgroundInfo
					{
						useBgColor = true,
						bgColor = Utilities.m_cPenguinColors[0]
					}
				},
				new GUIDefines.TextureData
				{
					pos = pos2,
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/CreateAccountNew/Penguin/fat_penguin_dark_shadow"
					},
					bgInfo = new GUIDefines.BackgroundInfo
					{
						useBgColor = true,
						bgColor = Utilities.m_cPenguinShadowColors[0]
					}
				},
				new GUIDefines.TextureData
				{
					pos = pos2,
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/CreateAccountNew/Penguin/fat_penguin_highlight"
					},
					bgInfo = new GUIDefines.BackgroundInfo
					{
						useBgColor = true,
						bgColor = Utilities.m_cPenguinHightlightColors[0]
					}
				},
				new GUIDefines.TextureData
				{
					pos = pos2,
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/CreateAccountNew/Penguin/fat_penguin_inside"
					}
				},
				new GUIDefines.TextureData
				{
					pos = new GUIDefines.RectInfo
					{
						widthRatio = 1f,
						heightRatio = 0.125f
					},
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/Common/Create_Account_NavBar"
					}
				}
			};
			base.LabelData = new GUIDefines.LabelData[3]
			{
				new GUIDefines.LabelData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.01041664f,
						widthRatio = 0.5145833f,
						heightRatio = 19f / 128f,
						IPad = new GUIDefines.RectIPadInfo
						{
							leftOffset = 34f
						}
					},
					content = new GUIDefines.ContentInfo(),
					invisible = true,
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customNormal = new GUIDefines.Texture2DInfo
						{
							name = "GUI/CreateAccountNew/Create_Account_ErrorBox"
						},
						customFontSize = GUIDefines.FontSize.eSmall,
						customNormalTextColor = GUIConstants.kRedColor,
						customWordWrap = true,
						customPadding = new GUIDefines.Vector2Info
						{
							xRatio = 0.009375f,
							yRatio = 1f / 128f
						},
						customPadding2 = new GUIDefines.Vector2Info
						{
							xRatio = 0.03645833f
						}
					}
				},
				new GUIDefines.LabelData
				{
					pos = new GUIDefines.RectInfo
					{
						topRatio = 7f / 160f,
						widthRatio = 1f,
						heightRatio = 7f / 160f,
						IPad = new GUIDefines.RectIPadInfo
						{
							keepSizeRatio = true,
							topOffset = -10f
						}
					},
					content = new GUIDefines.ContentInfo
					{
						textId = "TXT_CPAccount"
					}
				},
				new GUIDefines.LabelData
				{
					pos = rectInfo,
					content = new GUIDefines.ContentInfo
					{
						textId = "TXT_TOU_Link"
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
			base.TextFieldData = new GUIDefines.TextFieldData[4]
			{
				new GUIDefines.TextFieldData
				{
					controlName = "Name",
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.530208f,
						topRatio = 0.1703125f,
						widthRatio = 0.4333333f,
						heightRatio = 0.1109375f
					},
					maxLength = 20,
					defaultTextId = "TXT_PenguinName",
					titleCase = true,
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customNormal = new GUIDefines.Texture2DInfo
						{
							name = "GUI/CreateAccountNew/Create_Account_FormBox"
						},
						customActive = new GUIDefines.Texture2DInfo
						{
							name = "GUI/CreateAccountNew/Create_Account_FormBox_Focus"
						},
						customActiveTextColor = GUIConstants.kBlueColor,
						customFocusedTextColor = GUIConstants.kBlueColor
					}
				},
				new GUIDefines.TextFieldData
				{
					controlName = "Email",
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.53020835f,
						topRatio = 0.3234375f,
						widthRatio = 0.4333333f,
						heightRatio = 0.1109375f
					},
					maxLength = 40,
					defaultTextId = "TXT_Email",
					keyboardType = TouchScreenKeyboardType.EmailAddress,
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customNormal = new GUIDefines.Texture2DInfo
						{
							name = "GUI/CreateAccountNew/Create_Account_FormBox"
						},
						customActive = new GUIDefines.Texture2DInfo
						{
							name = "GUI/CreateAccountNew/Create_Account_FormBox_Focus"
						},
						customActiveTextColor = GUIConstants.kBlueColor,
						customFocusedTextColor = GUIConstants.kBlueColor
					}
				},
				new GUIDefines.TextFieldData
				{
					controlName = "Password",
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.530208f,
						topRatio = 0.475f,
						widthRatio = 0.4333333f,
						heightRatio = 0.1109375f
					},
					isPassword = true,
					maxLength = 20,
					defaultTextId = "TXT_Password",
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customNormal = new GUIDefines.Texture2DInfo
						{
							name = "GUI/CreateAccountNew/Create_Account_FormBox"
						},
						customActive = new GUIDefines.Texture2DInfo
						{
							name = "GUI/CreateAccountNew/Create_Account_FormBox_Focus"
						},
						customActiveTextColor = GUIConstants.kBlueColor,
						customFocusedTextColor = GUIConstants.kBlueColor
					}
				},
				new GUIDefines.TextFieldData
				{
					controlName = "ReTypePassword",
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.530208f,
						topRatio = 0.621875f,
						widthRatio = 0.4333333f,
						heightRatio = 0.1109375f
					},
					isPassword = true,
					maxLength = 20,
					defaultTextId = "TXT_ReTypePassword",
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customNormal = new GUIDefines.Texture2DInfo
						{
							name = "GUI/CreateAccountNew/Create_Account_FormBox"
						},
						customActive = new GUIDefines.Texture2DInfo
						{
							name = "GUI/CreateAccountNew/Create_Account_FormBox_Focus"
						},
						customActiveTextColor = GUIConstants.kBlueColor,
						customFocusedTextColor = GUIConstants.kBlueColor
					}
				}
			};
			mto_textFieldErrorHighlights = new GUIDefines.TextureData[4];
			for (int n = 0; n < 4; n++)
			{
				mto_textFieldErrorHighlights[n] = new GUIDefines.TextureData
				{
					pos = mto_textFieldPositions[n],
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/CreateAccountNew/Create_Account_FormBox_Error"
					}
				};
				mto_textFieldErrorHighlights[n].Init();
				base.TextFieldData[n].pos = mto_textFieldPositions[n];
			}
		}
	}

	protected void OnBack()
	{
		GameFlowManager.Instance.GUIManager.ShowCreateAccountPopup(false);
		GameFlowManager.Instance.GUIManager.ShowLoginPopup(true);
	}

	public override void Draw()
	{
		if (CanDraw())
		{
			base.Draw();
			if (ResolutionManager.Instance.LayoutSize != ResolutionManager.eLayoutSize.eIPad)
			{
				m_ColorPickerPopup.Draw();
				BlockControl(m_ColorPickerPopup.IsShowing || mo_termsOfUsePopup.IsShowing || mo_privacyPolicyPopup.IsShowing);
			}
			mo_termsOfUsePopup.Draw();
			mo_privacyPolicyPopup.Draw();
		}
	}

	protected override void DrawWindowContent(int aWindowId)
	{
		GUICompoundControls.Textures(base.LocalTransform.position, mto_bgTexture);
		if (ResolutionManager.Instance.LayoutSize == ResolutionManager.eLayoutSize.eIPad)
		{
			m_ColorPickerZone.Draw();
			BlockControl(mo_termsOfUsePopup.IsShowing || mo_privacyPolicyPopup.IsShowing);
		}
		base.DrawWindowContent(aWindowId);
	}

	public void Update()
	{
		mo_termsOfUsePopup.Update();
		mo_privacyPolicyPopup.Update();
	}

	protected override void OnButtonSelect()
	{
		if (base.TextFieldData != null)
		{
			for (int i = 0; i < base.TextFieldData.Length; i++)
			{
				base.TextFieldData[i].isFocused = false;
			}
		}
		GameFlowManager.Instance.AudioManager.PlayUISFx(GameFlowManager.Instance.MenuClick24);
		switch ((Button)base.SelectedButton)
		{
		case Button.eBack:
			OnBack();
			break;
		case Button.eColorPicker:
			m_ColorPickerPopup.Show(true);
			break;
		case Button.eTOU:
			mo_termsOfUsePopup.Show(true);
			break;
		case Button.ePP:
			mo_privacyPolicyPopup.Show(true);
			break;
		case Button.eCreatePenguin:
		{
			string aUserName = string.Empty;
			string aEmail = string.Empty;
			string aPassword = string.Empty;
			string aPasswordConfirm = string.Empty;
			if (base.TextFieldData[0].editedText != null)
			{
				aUserName = base.TextFieldData[0].editedText;
			}
			if (base.TextFieldData[0].editedText != null)
			{
				aEmail = base.TextFieldData[1].editedText;
			}
			if (base.TextFieldData[0].editedText != null)
			{
				aPassword = base.TextFieldData[2].editedText;
			}
			if (base.TextFieldData[0].editedText != null)
			{
				aPasswordConfirm = base.TextFieldData[3].editedText;
			}
			int num = 0;
			num = (int)((ResolutionManager.Instance.LayoutSize != ResolutionManager.eLayoutSize.eIPad) ? m_ColorPickerPopup.SelectedColor : m_ColorPickerZone.SelectedColor);
			NetManager.Instance.CreateCPAccount(aUserName, aPassword, aPasswordConfirm, aEmail, num, CreateAccountCompleteCallback);
			break;
		}
		}
	}

	private void UpdatePenguinColor(int aColorID)
	{
		if (aColorID < 16)
		{
			base.TextureData[2].bgInfo.bgColor = Utilities.m_cPenguinShadowColors[aColorID];
			base.TextureData[1].bgInfo.bgColor = Utilities.m_cPenguinColors[aColorID];
			base.TextureData[3].bgInfo.bgColor = Utilities.m_cPenguinHightlightColors[aColorID];
		}
	}

	private void GoToNextSceneAfterSuccess()
	{
		GameFlowManager.Instance.GUIManager.CreateAccountPopupToBackTraceScene();
	}

	private void ColorPickupPopupCallback(int aSelectedButton)
	{
		if (ResolutionManager.Instance.LayoutSize == ResolutionManager.eLayoutSize.eIPad)
		{
			UpdatePenguinColor((int)m_ColorPickerZone.SelectedColor);
		}
		else
		{
			UpdatePenguinColor((int)m_ColorPickerPopup.SelectedColor);
		}
	}

	private void TermsOfUsePopupCallback(int aSelectedButton)
	{
	}

	private void PrivacyPolicyPopupCallback(int aSelectedButton)
	{
	}

	private void CreateAccountCompleteCallback(bool aSuccess)
	{
		if (aSuccess)
		{
			ProfileManager.Instance.CurrentProfile.ProfileName = base.TextFieldData[0].editedText;
			ProfileManager.Instance.SaveCurrentProfile();
			GoToNextSceneAfterSuccess();
		}
		else if (NetError.IsUserNameRelatedError(NetManager.Instance.GetLastErrorCode(NetManager.Request.eCreateAccount)))
		{
			ShowInLineError(CreateAccountTextField.eName);
		}
		else if (NetError.IsEmailRelatedError(NetManager.Instance.GetLastErrorCode(NetManager.Request.eCreateAccount)))
		{
			ShowInLineError(CreateAccountTextField.eEmail);
		}
		else if (NetError.IsPasswordRelatedError(NetManager.Instance.GetLastErrorCode(NetManager.Request.eCreateAccount)))
		{
			ShowInLineError(CreateAccountTextField.ePassword);
		}
		else if (NetError.IsPasswordMismatchError(NetManager.Instance.GetLastErrorCode(NetManager.Request.eCreateAccount)))
		{
			ShowInLineError(CreateAccountTextField.eRetypePassword);
		}
		else
		{
			NetManager.Instance.ShowError(NetManager.Instance.GetLastErrorMsg(NetManager.Request.eCreateAccount), false);
		}
	}

	private void ShowInLineError(CreateAccountTextField aErrorField)
	{
		for (int i = 0; i < base.TextFieldData.Length; i++)
		{
			base.TextFieldData[i].isFocused = false;
		}
		base.TextureData[0].pos = base.TextFieldData[(int)aErrorField].pos;
		SetTextureInvisible(0, false);
		base.LabelData[0].pos = mto_errorBubblePositions[(int)aErrorField];
		base.LabelData[0].pos.Init();
		SetLabelInvisible(0, false);
		SetLabelText(0, NetManager.Instance.GetLastErrorMsg(NetManager.Request.eCreateAccount));
	}

	private void HideInLineError()
	{
		SetTextureInvisible(0, true);
		SetLabelInvisible(0, true);
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
		rectInfo.heightRatio = mtf_TOUButtonHeight[layoutSize];
		rectInfo.IPad = o_textPos.IPad;
		return rectInfo;
	}
}
