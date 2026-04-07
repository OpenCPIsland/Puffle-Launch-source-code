using UnityEngine;

public class AboutCP : BaseMonoScreen
{
	private enum Button
	{
		ePlayVideo = 0,
		eButton_COUNT = 1
	}

	private enum Label
	{
		ePageText = 0,
		eLabel_COUNT = 1
	}

	public GameObject[] m_AboutCPRefObj;

	public string[] m_TexturePaths;

	public string[] m_PageTextsIds;

	public Vector2 m_TouchPosition = new Vector2(0f, 0f);

	public Vector2 m_PreviousTouchPosition = new Vector2(0f, 0f);

	public bool m_WasTouchDown;

	public Vector2 m_StartTouchPosition;

	public Rect m_ScrollAreaDetectZone;

	public bool m_ScrollAreaSelected;

	private Texture2D mo_firstTexture;

	protected override void OnBack()
	{
		GameFlowManager.Instance.GUIManager.UnregisterAboutCPCurrentPage();
		GameFlowManager.Instance.AudioManager.PlayUISFx(GameFlowManager.Instance.MenuClick24);
		base.MainScreen.StopGUI();
		GameFlowManager.Instance.LoadScene("!Loader_MainMenu", false);
	}

	protected override void CreateMainScreenLayouts()
	{
		GUIDefines.ButtonData[] aCustomButtons = null;
		string customOnDemandFontName = string.Empty;
		switch (ResolutionManager.Instance.LayoutSize)
		{
		case ResolutionManager.eLayoutSize.eLowres:
			aCustomButtons = new GUIDefines.ButtonData[1]
			{
				new GUIDefines.ButtonData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.855625f,
						topRatio = 0.02120833f,
						widthRatio = 0.12291667f,
						heightRatio = 0.075f
					},
					detectZoneScale = 1.5f,
					content = new GUIDefines.ContentInfo
					{
						textId = "TXT_Credits"
					},
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customNormal = new GUIDefines.Texture2DInfo
						{
							name = "GUI/AboutCP/creditsbutton_iphone4_normal"
						},
						customActive = new GUIDefines.Texture2DInfo
						{
							name = "GUI/AboutCP/creditsbutton_iphone4_pressed"
						},
						customFontSize = GUIDefines.FontSize.eSmall
					}
				}
			};
			customOnDemandFontName = GameFlowManager.Instance.GUIManager.GetLowResFontName(GUIDefines.FontType.eCPMenus, GUIDefines.FontSize.eMini);
			break;
		case ResolutionManager.eLayoutSize.eOriginal:
			aCustomButtons = new GUIDefines.ButtonData[1]
			{
				new GUIDefines.ButtonData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.855625f,
						topRatio = 0.02120833f,
						widthRatio = 0.12291667f,
						heightRatio = 0.075f
					},
					detectZoneScale = 1.5f,
					content = new GUIDefines.ContentInfo
					{
						textId = "TXT_Credits"
					},
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customNormal = new GUIDefines.Texture2DInfo
						{
							name = "GUI/AboutCP/creditsbutton_iphone4_normal"
						},
						customActive = new GUIDefines.Texture2DInfo
						{
							name = "GUI/AboutCP/creditsbutton_iphone4_pressed"
						},
						customFontSize = GUIDefines.FontSize.eSmall
					}
				}
			};
			customOnDemandFontName = GameFlowManager.Instance.GUIManager.GetLowResFontName(GUIDefines.FontType.eCPMenus, GUIDefines.FontSize.eSmall);
			break;
		case ResolutionManager.eLayoutSize.eIPad:
			aCustomButtons = new GUIDefines.ButtonData[1]
			{
				new GUIDefines.ButtonData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.855625f,
						topRatio = 0.02120833f,
						widthRatio = 0.11523438f,
						heightRatio = 0.0625f,
						IPad = new GUIDefines.RectIPadInfo
						{
							keepSizeRatio = true
						}
					},
					detectZoneScale = 1.5f,
					content = new GUIDefines.ContentInfo
					{
						textId = "TXT_Credits"
					},
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customNormal = new GUIDefines.Texture2DInfo
						{
							name = "GUI/AboutCP/creditsbutton_iphone4_normal"
						},
						customActive = new GUIDefines.Texture2DInfo
						{
							name = "GUI/AboutCP/creditsbutton_iphone4_pressed"
						},
						customFontSize = GUIDefines.FontSize.eSmall
					}
				}
			};
			customOnDemandFontName = GameFlowManager.Instance.GUIManager.GetLowResFontName(GUIDefines.FontType.eCPMenus, GUIDefines.FontSize.eSmall);
			break;
		}
		TopBarButtonCallback[] aCustomCallbacks = new TopBarButtonCallback[1] { CreditsCallback };
		SetTopBarData("TXT_Back", "TXT_AboutCP", aCustomButtons, aCustomCallbacks);
		base.MainScreen.TextureData = new GUIDefines.TextureData[1]
		{
			new GUIDefines.TextureData
			{
				pos = new GUIDefines.RectInfo
				{
					topRatio = GetTopBarHeightRatio() - 0.01f,
					widthRatio = 1f,
					heightRatio = 0.125f
				},
				icon = new GUIDefines.TextureInfo
				{
					name = "GUI/AboutCP/AboutCP_Text_Transparency"
				}
			}
		};
		base.MainScreen.ButtonData = new GUIDefines.ButtonData[1]
		{
			new GUIDefines.ButtonData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.915f,
					topRatio = 0.814f,
					widthRatio = 0.0708333f,
					heightRatio = 0.103125f,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true,
						leftOffset = -3f,
						topOffset = 16f,
						heightScale = -9f
					}
				},
				detectZoneScale = 1.5f,
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customNormal = new GUIDefines.Texture2DInfo
					{
						name = string.Empty
					},
					customActive = new GUIDefines.Texture2DInfo
					{
						name = "GUI/AboutCP/PlayAboutCPMovie_pressed"
					}
				}
			}
		};
		base.MainScreen.LabelData = new GUIDefines.LabelData[3]
		{
			new GUIDefines.LabelData
			{
				pos = new GUIDefines.RectInfo
				{
					topRatio = GetTopBarHeightRatio() - 0.01f,
					widthRatio = 1f,
					heightRatio = 0.125f
				},
				content = new GUIDefines.ContentInfo
				{
					textId = m_PageTextsIds[0]
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
					leftRatio = 0.6f,
					topRatio = 0.925f,
					widthRatio = 0.4f,
					heightRatio = 0.05f,
					IPad = new GUIDefines.RectIPadInfo
					{
						topOffset = 5f
					}
				},
				content = new GUIDefines.ContentInfo
				{
					textId = "TXT_AboutCP_pg5b"
				},
				disableDropShadow = true,
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customWordWrap = true,
					customFontType = GUIDefines.FontType.eOnDemand,
					customOnDemandFontName = customOnDemandFontName,
					customNormalTextColor = GUIConstants.kLessDarkGreyColor,
					useCustomTextAlignment = true,
					customTextAlignment = TextAnchor.MiddleCenter
				}
			},
			new GUIDefines.LabelData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.701042f,
					topRatio = 19f / 32f,
					widthRatio = 0.286458f,
					heightRatio = 7f / 160f,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true,
						topOffset = 41f,
						heightScale = -9f
					}
				},
				content = new GUIDefines.ContentInfo
				{
					textId = "TXT_AboutCP_Video"
				},
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customFontSize = GUIDefines.FontSize.eMedium
				}
			}
		};
	}

	private Texture2D LoadTexture(string path)
	{
		switch (ResolutionManager.Instance.AssetResolution)
		{
		case ResolutionManager.eAssetResolution.eIPad:
			path += "_iPad";
			break;
		case ResolutionManager.eAssetResolution.eLowres:
			path += "_lowres";
			break;
		}
		return GUIUtil.LoadTexture2D(path);
	}

	private void Awake()
	{
		Init(base.gameObject);
		string path = m_TexturePaths[GameFlowManager.Instance.GUIManager.AboutCPCurrentPage];
		mo_firstTexture = LoadTexture(path);
		for (int i = 0; i < m_AboutCPRefObj.Length; i++)
		{
			m_AboutCPRefObj[i].GetComponent<Renderer>().material.mainTexture = mo_firstTexture;
		}
		m_ScrollAreaDetectZone = new Rect(0f, GetTopBarHeightPixels(), Screen.width, (float)Screen.height - GetTopBarHeightPixels());
		if (ResolutionManager.Instance.AssetResolution == ResolutionManager.eAssetResolution.eIPad)
		{
			TextMesh component = GameObject.Find("Page1/Disclaimer").GetComponent<TextMesh>();
			component.characterSize = 0.24f;
			TextMesh component2 = GameObject.Find("Page2/Disclaimer").GetComponent<TextMesh>();
			component2.characterSize = 0.24f;
		}
	}

	private void Start()
	{
	}

	private new void Update()
	{
		base.HandleButtonSelect();
	}

	private void OnGUI()
	{
		if (base.MainScreen.CanDraw())
		{
			base.MainScreen.Draw();
			DrawTopBar();
		}
	}

	private void OnDestroy()
	{
		string aValue = "Player Not Logged In";
		if (ProfileManager.Instance != null && ProfileManager.Instance.CurrentProfile != null && ProfileManager.Instance.CurrentProfile.ProfileName != null)
		{
			aValue = ProfileManager.Instance.CurrentProfile.ProfileName;
		}
		BizIntel.ContextualEvent contextualEvent = new BizIntel.ContextualEvent("view-aboutcp");
		contextualEvent.AddContextItem("player-id", aValue);
		contextualEvent.AddContextItem("elapsed-time", (int)Time.timeSinceLevelLoad);
		contextualEvent.Log();
	}

	protected override void OnMainScreenButtonSelect()
	{
		GameFlowManager.Instance.AudioManager.PlayUISFx(GameFlowManager.Instance.MenuClick24);
		if (base.MainScreen.SelectedButton == 0 && CinematicManager.Instance != null)
		{
			CinematicManager.Instance.ShowFullscreenBgWhenPlaying = true;
			CinematicManager.Instance.Play(CinematicManager.MovieId.eAboutCP);
		}
	}

	public void CreditsCallback()
	{
		GameFlowManager.Instance.AudioManager.PlayUISFx(GameFlowManager.Instance.MenuClick24);
		base.MainScreen.StopGUI();
		GameFlowManager.Instance.LoadScene("CreditsNew", false);
	}
}
