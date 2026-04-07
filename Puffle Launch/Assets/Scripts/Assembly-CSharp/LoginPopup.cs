using UnityEngine;

public class LoginPopup : BasePopup
{
	private enum Button
	{
		eLogin = 0,
		eCreateAccount = 1,
		eLogout = 2,
		eTOU = 3,
		ePP = 4,
		eBack = 5,
		eButton_COUNT = 6
	}

	private enum Label
	{
		eTitle = 0,
		eCommonLabel_COUNT = 1,
		eLoginErrorBubble = 1,
		eLoginLabel_COUNT = 2
	}

	private enum Texture
	{
		eBackground = 0,
		eTopNavBar = 1,
		eCommonTexture_COUNT = 2,
		eLoginCPLogo = 2,
		eLoginErrorHighlight = 3,
		eLoginSeperator = 4,
		eLoginTexture_COUNT = 5,
		eAlreadyLoggedInCPLogo = 2,
		eAlreadyLoggedInTexture_COUNT = 3
	}

	private enum TextField
	{
		eAccount = 0,
		ePassword = 1,
		eTextField_COUNT = 2
	}

	private enum ReturnCode
	{
		eSuccess = 0,
		eFail = 1,
		eReturnCode_COUNT = 2
	}

	private const float kFirstTextFieldPosY = 0.221875f;

	private const float kNextTextFieldOffsetY = 0.1453125f;

	private const float kFirstErrorBubblePosY = 0.2125f;

	private const float kf_TOUButtonHeight = 0.1f;

	private TermsOfUsePopup mo_termsOfUsePopup;

	private PrivacyPolicyPopup mo_privacyPolicyPopup;

	private string mPassword;

	private string mUsername;

	private GUIDefines.TextureData mBackgroundTexture;

	private GUIDefines.TextureData mTopNavBarTexture;

	private GUIDefines.ButtonData mBackButton;

	private GUIDefines.LabelData mTitleLabel;

	private static Vector2[][] mto_TOUButtonGroupHorizontalOffset = new Vector2[4][]
	{
		new Vector2[3]
		{
			new Vector2(0.19f, 0.05f),
			new Vector2(0.25f, -0.025f),
			new Vector2(0.25f, -0.025f)
		},
		new Vector2[3]
		{
			new Vector2(0.29f, 0.025f),
			new Vector2(0.3575f, -0.025f),
			new Vector2(0.35f, -0.025f)
		},
		new Vector2[3]
		{
			new Vector2(0.21f, 0.05f),
			new Vector2(0.275f, -0.025f),
			new Vector2(0.275f, -0.025f)
		},
		new Vector2[3]
		{
			new Vector2(0.21f, 0.05f),
			new Vector2(0.275f, -0.025f),
			new Vector2(0.275f, -0.025f)
		}
	};

	private static float[][] mto_TOUButtonWidth = new float[4][]
	{
		new float[3] { 0.1f, 0.1f, 0.1f },
		new float[3] { 0.25f, 0.2f, 0.2f },
		new float[3] { 0.2f, 0.14f, 0.14f },
		new float[3] { 0.2f, 0.14f, 0.14f }
	};

	private static float[][] mto_PPButtonWidth = new float[4][]
	{
		new float[3] { 0.1f, 0.1f, 0.1f },
		new float[3] { 0.2f, 0.125f, 0.125f },
		new float[3] { 0.275f, 0.175f, 0.175f },
		new float[3] { 0.275f, 0.175f, 0.175f }
	};

	public LoginPopup(GameObject aRefObj)
		: base(aRefObj)
	{
		mo_termsOfUsePopup = new TermsOfUsePopup(aRefObj);
		mo_termsOfUsePopup.RegisterCallback(TermsOfUsePopupCallback);
		mo_privacyPolicyPopup = new PrivacyPolicyPopup(aRefObj);
		mo_privacyPolicyPopup.RegisterCallback(PrivacyPolicyPopupCallback);
	}

	protected void ResetLayouts()
	{
		CreateLayouts();
		m_IsPopupInitialized = false;
	}

	protected override void CreateLayouts()
	{
		CreateCommonScreenLayout();
		if (NetManager.Instance.IsPlayerLoggedIn())
		{
			CreateAlreadyLoggedInScreenLayout();
		}
		else
		{
			CreateLoginScreenLayout();
		}
	}

	private void CreateCommonScreenLayout()
	{
		m_WindowBackground = null;
		base.WindowData = new GUIDefines.WindowData
		{
			pos = new GUIDefines.RectInfo
			{
				widthRatio = 1f,
				heightRatio = 1f,
				detatchFromRefObject = true
			},
			id = 12
		};
		if (mBackgroundTexture == null)
		{
			mBackgroundTexture = new GUIDefines.TextureData
			{
				pos = new GUIDefines.RectInfo
				{
					widthRatio = 1f,
					heightRatio = 1f,
					detatchFromRefObject = true
				},
				icon = new GUIDefines.TextureInfo
				{
					name = "GUI/CreditsNew/Credits_BG_BlueGradient"
				}
			};
		}
		if (mTopNavBarTexture == null)
		{
			if (ResolutionManager.Instance.AssetResolution == ResolutionManager.eAssetResolution.eIPad)
			{
				mTopNavBarTexture = new GUIDefines.TextureData
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
				};
			}
			else
			{
				mTopNavBarTexture = new GUIDefines.TextureData
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
				};
			}
		}
		if (mBackButton == null)
		{
			if (ResolutionManager.Instance.LayoutSize == ResolutionManager.eLayoutSize.eIPad)
			{
				mBackButton = new GUIDefines.ButtonData
				{
					buttonId = 5,
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
				};
			}
			else
			{
				mBackButton = new GUIDefines.ButtonData
				{
					buttonId = 5,
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 3f / 160f,
						topRatio = 0.021875f,
						widthRatio = 7f / 64f,
						heightRatio = 0.0796875f,
						IPad = new GUIDefines.RectIPadInfo()
					},
					detectZoneScale = 1.5f,
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
						}
					}
				};
			}
		}
		if (mTitleLabel == null)
		{
			if (LocalizationManager.GetLanguageCode() == "fr" && ResolutionManager.Instance.AssetResolution == ResolutionManager.eAssetResolution.eOriginal)
			{
				mTitleLabel = new GUIDefines.LabelData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.015f,
						topRatio = 7f / 160f,
						widthRatio = 1f,
						heightRatio = 7f / 160f,
						IPad = new GUIDefines.RectIPadInfo
						{
							keepSizeRatio = true,
							topOffset = -10f
						}
					},
					content = new GUIDefines.ContentInfo(),
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customFontSize = GUIDefines.FontSize.eMedium
					}
				};
			}
			else
			{
				mTitleLabel = new GUIDefines.LabelData
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
					content = new GUIDefines.ContentInfo(),
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customFontSize = GUIDefines.FontSize.eMedium
					}
				};
			}
		}
	}

	private void CreateLoginScreenLayout()
	{
		GUIDefines.RectInfo rectInfo = null;
		string customOnDemandFontName = string.Empty;
		string textId = string.Empty;
		switch (ResolutionManager.Instance.LayoutSize)
		{
		case ResolutionManager.eLayoutSize.eLowres:
		{
			GUIDefines.RectInfo rectInfo2 = new GUIDefines.RectInfo();
			rectInfo2.leftRatio = GUIUtil.FindHorizontalPositionToAlign(0.5f, 2, 0);
			rectInfo2.topRatio = 0.815f;
			rectInfo2.widthRatio = 0.5f;
			rectInfo2.heightRatio = 0.125f;
			rectInfo2.IPad = new GUIDefines.RectIPadInfo
			{
				keepSizeRatio = true,
				topOffset = -48f
			};
			rectInfo = rectInfo2;
			customOnDemandFontName = GameFlowManager.Instance.GUIManager.GetLowResFontName(GUIDefines.FontType.eCPMenus, GUIDefines.FontSize.eMini);
			textId = "TXT_TermsAndPrivacyTap2lines";
			break;
		}
		case ResolutionManager.eLayoutSize.eOriginal:
		{
			GUIDefines.RectInfo rectInfo2 = new GUIDefines.RectInfo();
			rectInfo2.leftRatio = GUIUtil.FindHorizontalPositionToAlign(0.5f, 2, 0);
			rectInfo2.topRatio = 143f / 160f;
			rectInfo2.widthRatio = 0.5f;
			rectInfo2.heightRatio = 3f / 64f;
			rectInfo2.IPad = new GUIDefines.RectIPadInfo
			{
				keepSizeRatio = true,
				topOffset = -48f
			};
			rectInfo = rectInfo2;
			customOnDemandFontName = GameFlowManager.Instance.GUIManager.GetLowResFontName(GUIDefines.FontType.eCPMenus, GUIDefines.FontSize.eSmall);
			textId = "TXT_TermsAndPrivacyTap";
			break;
		}
		case ResolutionManager.eLayoutSize.eIPad:
		{
			GUIDefines.RectInfo rectInfo2 = new GUIDefines.RectInfo();
			rectInfo2.leftRatio = GUIUtil.FindHorizontalPositionToAlign(0.5f, 2, 0);
			rectInfo2.topRatio = 0.878125f;
			rectInfo2.widthRatio = 0.5f;
			rectInfo2.heightRatio = 3f / 64f;
			rectInfo2.IPad = new GUIDefines.RectIPadInfo
			{
				keepSizeRatio = true,
				topOffset = -48f
			};
			rectInfo = rectInfo2;
			customOnDemandFontName = GameFlowManager.Instance.GUIManager.GetLowResFontName(GUIDefines.FontType.eCPMenus, GUIDefines.FontSize.eSmall);
			textId = "TXT_TermsAndPrivacyTap";
			break;
		}
		}
		base.TextFieldData = new GUIDefines.TextFieldData[2]
		{
			new GUIDefines.TextFieldData
			{
				controlName = "Account",
				defaultTextId = "TXT_PenguinName",
				titleCase = true,
				maxLength = 20,
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.53325f,
					topRatio = 0.221875f,
					widthRatio = 0.3645833f,
					heightRatio = 0.128125f,
					IPad = new GUIDefines.RectIPadInfo
					{
						topOffset = 43f
					}
				},
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
					customFontSize = GUIDefines.FontSize.eMedium,
					customActiveTextColor = GUIConstants.kBlueColor,
					customFocusedTextColor = GUIConstants.kBlueColor
				}
			},
			new GUIDefines.TextFieldData
			{
				controlName = "Password",
				defaultTextId = "TXT_Password",
				isPassword = true,
				maxLength = 20,
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.53325f,
					topRatio = 47f / 128f,
					widthRatio = 0.3645833f,
					heightRatio = 0.128125f,
					IPad = new GUIDefines.RectIPadInfo
					{
						topOffset = 23f
					}
				},
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
					customFontSize = GUIDefines.FontSize.eMedium,
					customActiveTextColor = GUIConstants.kBlueColor,
					customFocusedTextColor = GUIConstants.kBlueColor
				}
			}
		};
		base.TextureData = new GUIDefines.TextureData[5]
		{
			mBackgroundTexture,
			mTopNavBarTexture,
			new GUIDefines.TextureData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.02395836f,
					topRatio = 0.3109375f,
					widthRatio = 0.4604167f,
					heightRatio = 27f / 64f,
					IPad = new GUIDefines.RectIPadInfo
					{
						leftOffset = 14f,
						topOffset = 25f
					}
				},
				icon = new GUIDefines.TextureInfo
				{
					name = "GUI/CreditsNew/CP_Logo_HiRes"
				}
			},
			new GUIDefines.TextureData
			{
				pos = base.TextFieldData[0].pos,
				icon = new GUIDefines.TextureInfo
				{
					name = "GUI/CreateAccountNew/Create_Account_FormBox_Error"
				},
				invisible = true
			},
			new GUIDefines.TextureData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 53f / 96f,
					topRatio = 23f / 32f,
					widthRatio = 0.3260417f,
					heightRatio = 1f / 160f,
					IPad = new GUIDefines.RectIPadInfo
					{
						topOffset = -23f
					}
				},
				icon = new GUIDefines.TextureInfo
				{
					name = "GUI/CoinTransfer/Textures/Login_seperator"
				}
			}
		};
		base.ButtonData = new GUIDefines.ButtonData[5]
		{
			mBackButton,
			new GUIDefines.ButtonData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.5262501f,
					topRatio = 0.5187525f,
					widthRatio = 0.3708333f,
					heightRatio = 0.165625f,
					IPad = new GUIDefines.RectIPadInfo
					{
						topOffset = 6f
					}
				},
				content = new GUIDefines.ContentInfo
				{
					textId = "TXT_Login"
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
					customFontSize = GUIDefines.FontSize.eLarge
				},
				useAutoResizeGroup = true
			},
			new GUIDefines.ButtonData
			{
				buttonId = 1,
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.5262501f,
					topRatio = 0.7562525f,
					widthRatio = 0.3708333f,
					heightRatio = 0.165625f,
					IPad = new GUIDefines.RectIPadInfo
					{
						topOffset = -33f
					}
				},
				content = new GUIDefines.ContentInfo
				{
					textId = "TXT_CreateAccount"
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
				useAutoResizeGroup = true
			},
			new GUIDefines.ButtonData
			{
				buttonId = 3,
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
				pos = GetTermsOfUseButtonPos(rectInfo, Button.ePP),
				detectZoneScale = 1.1f,
				content = new GUIDefines.ContentInfo(),
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true
				}
			}
		};
		base.LabelData = new GUIDefines.LabelData[3]
		{
			mTitleLabel,
			new GUIDefines.LabelData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.01041664f,
					topRatio = 0.2125f,
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
		SetLabelTextId(0, "TXT_TransferYourCoins");
	}

	private void CreateAlreadyLoggedInScreenLayout()
	{
		base.TextureData = new GUIDefines.TextureData[3]
		{
			mBackgroundTexture,
			mTopNavBarTexture,
			new GUIDefines.TextureData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.2947917f,
					topRatio = 0.121875f,
					widthRatio = 0.4197917f,
					heightRatio = 0.3796875f,
					IPad = new GUIDefines.RectIPadInfo
					{
						leftOffset = 14f,
						topOffset = 25f
					}
				},
				icon = new GUIDefines.TextureInfo
				{
					name = "GUI/CreditsNew/CP_Logo_HiRes"
				}
			}
		};
		base.ButtonData = new GUIDefines.ButtonData[2]
		{
			mBackButton,
			new GUIDefines.ButtonData
			{
				buttonId = 2,
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.2533334f,
					topRatio = 0.7375025f,
					widthRatio = 49f / 96f,
					heightRatio = 0.165625f,
					IPad = new GUIDefines.RectIPadInfo
					{
						topOffset = -80f
					}
				},
				content = new GUIDefines.ContentInfo
				{
					textId = "TXT_LogOut"
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
					customFontSize = GUIDefines.FontSize.eLarge
				}
			}
		};
		base.LabelData = new GUIDefines.LabelData[2]
		{
			mTitleLabel,
			new GUIDefines.LabelData
			{
				pos = new GUIDefines.RectInfo
				{
					topRatio = 0.5171875f,
					widthRatio = 1f,
					heightRatio = 0.125f,
					IPad = new GUIDefines.RectIPadInfo
					{
						topOffset = -47f
					}
				},
				content = new GUIDefines.ContentInfo
				{
					text = LocalizationManager.Instance.GetString("TXT_Transferring", ProfileManager.Instance.CurrentProfile.ProfileName)
				},
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customFontSize = GUIDefines.FontSize.eMedium
				}
			}
		};
		base.TextFieldData = null;
		SetLabelTextId(0, "TXT_ClubPenguin");
	}

	public override void Draw()
	{
		if (CanDraw())
		{
			base.Draw();
			BlockControl(mo_termsOfUsePopup.IsShowing || mo_privacyPolicyPopup.IsShowing);
			mo_termsOfUsePopup.Draw();
			mo_privacyPolicyPopup.Draw();
		}
	}

	public void Update()
	{
		mo_termsOfUsePopup.Update();
		mo_privacyPolicyPopup.Update();
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
		case Button.eLogin:
			AttemptLogin();
			break;
		case Button.eCreateAccount:
			GameFlowManager.Instance.GUIManager.ShowLoginPopup(false);
			GameFlowManager.Instance.GUIManager.ShowCreateAccountPopup(true);
			break;
		case Button.eLogout:
			NetManager.Instance.ResetAuthToken();
			ResetLayouts();
			break;
		case Button.eTOU:
			mo_termsOfUsePopup.Show(true);
			break;
		case Button.ePP:
			mo_privacyPolicyPopup.Show(true);
			break;
		case Button.eBack:
			GameFlowManager.Instance.GUIManager.LoginPopupToBackTraceScene();
			break;
		}
	}

	private void TermsOfUsePopupCallback(int aSelectedButton)
	{
	}

	private void PrivacyPolicyPopupCallback(int aSelectedButton)
	{
	}

	private void AttemptLogin()
	{
		mUsername = base.TextFieldData[0].editedText;
		mPassword = base.TextFieldData[1].editedText;
		NetManager.Instance.Login(mUsername, mPassword, LoginCompleteCallback);
	}

	public void LoginCompleteCallback(bool aSuccess)
	{
		HideInLineError();
		if (aSuccess)
		{
			ProfileManager.Instance.CurrentProfile.ProfileName = mUsername;
			ProfileManager.Instance.SaveCurrentProfile();
			GameFlowManager.Instance.GUIManager.LoginPopupToBackTraceScene();
		}
		else if (NetError.IsUserNameRelatedError(NetManager.Instance.GetLastErrorCode(NetManager.Request.eLogin)))
		{
			ShowInLineError(TextField.eAccount);
		}
		else if (NetError.IsPasswordRelatedError(NetManager.Instance.GetLastErrorCode(NetManager.Request.eLogin)))
		{
			ShowInLineError(TextField.ePassword);
		}
		else
		{
			NetManager.Instance.ShowError(NetManager.Instance.GetLastErrorMsg(NetManager.Request.eLogin), false);
		}
	}

	private void ShowInLineError(TextField aErrorField)
	{
		switch (aErrorField)
		{
		default:
			return;
		case TextField.eAccount:
			base.LabelData[1].pos.topRatio = 0.2125f;
			break;
		case TextField.ePassword:
			base.LabelData[1].pos.topRatio = 0.35781252f;
			break;
		}
		for (int i = 0; i < base.TextFieldData.Length; i++)
		{
			base.TextFieldData[i].isFocused = false;
		}
		base.TextureData[3].pos = base.TextFieldData[(int)aErrorField].pos;
		SetTextureInvisible(3, false);
		base.LabelData[1].pos.IPad.topOffset = base.TextFieldData[(int)aErrorField].pos.IPad.topOffset;
		base.LabelData[1].pos.Init();
		SetLabelInvisible(1, false);
		SetLabelText(1, NetManager.Instance.GetLastErrorMsg(NetManager.Request.eLogin));
	}

	private void HideInLineError()
	{
		SetTextureInvisible(3, true);
		SetLabelInvisible(1, true);
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
}
