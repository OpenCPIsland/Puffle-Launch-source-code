public class MainMenu : BaseMonoScreen
{
	private enum Button
	{
		ePlay = 0,
		eInfo = 1,
		eLogin = 2,
		eButton_COUNT = 3
	}

	protected override void CreateMainScreenLayouts()
	{
		base.MainScreen.TextureData = new GUIDefines.TextureData[1]
		{
			new GUIDefines.TextureData
			{
				pos = new GUIDefines.RectInfo
				{
					topRatio = 117f / 160f,
					widthRatio = 1f,
					heightRatio = 43f / 160f,
					IPad = new GUIDefines.RectIPadInfo
					{
						topOffset = 16f,
						heightScale = 19f
					}
				},
				icon = new GUIDefines.TextureInfo
				{
					name = "GUI/MainMenu/Textures/lower_band"
				}
			}
		};
		string text = "appstore_button";
		text = "androidMarket_button";
		base.MainScreen.ButtonData = new GUIDefines.ButtonData[2]
		{
			new GUIDefines.ButtonData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.35f,
					topRatio = 0.775f,
					widthRatio = 0.3010417f,
					heightRatio = 0.1921875f,
					IPad = new GUIDefines.RectIPadInfo
					{
						leftOffset = 1f,
						topOffset = 12f,
						widthScale = 19f,
						heightScale = 6f
					}
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
					},
					customFontSize = GUIDefines.FontSize.eLarge,
					customFontType = GUIDefines.FontType.eInGame
				},
				content = new GUIDefines.ContentInfo
				{
					textId = "TXT_Play"
				}
			},
			new GUIDefines.ButtonData
			{
				buttonId = 1,
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.0125f,
					topRatio = 0.8859375f,
					widthRatio = 0.06354167f,
					heightRatio = 0.0921875f,
					IPad = new GUIDefines.RectIPadInfo
					{
						leftOffset = 4f,
						topOffset = 6f
					}
				},
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customNormal = new GUIDefines.Texture2DInfo
					{
						name = "GUI/MainMenu/Textures/info_button"
					},
					customActive = new GUIDefines.Texture2DInfo
					{
						name = "GUI/MainMenu/Textures/info_button_pressed"
					}
				}
			}
		};
	}

	private void Awake()
	{
		Init(base.gameObject);
	}

	private void Start()
	{
		AudioManager.Instance.PlayMusic(AudioManager.MusicTrack.eMusic_Menu);
		AmazonHeroWidgetManager.Init();
	}

	private void OnGUI()
	{
		if (base.MainScreen.CanDraw() && !GameFlowManager.Instance.GUIManager.IsLoginPopupShowing && !GameFlowManager.Instance.GUIManager.IsCreateAccountPopupShowing)
		{
			base.MainScreen.Draw();
			BlockControl(GameFlowManager.Instance.GUIManager.IsAppQuitPopupShowing);
		}
	}

	protected override void OnMainScreenButtonSelect()
	{
		GameFlowManager.Instance.AudioManager.PlayUISFx(GameFlowManager.Instance.MenuClick24);
		switch ((Button)base.MainScreen.SelectedButton)
		{
		case Button.ePlay:
			base.MainScreen.StopGUI();
			GameFlowManager.Instance.LoadScene("LevelSelect", false);
			break;
		case Button.eInfo:
			base.MainScreen.StopGUI();
			GameFlowManager.Instance.LoadScene("CreditsNew", false);
			break;
		case Button.eLogin:
			GameFlowManager.Instance.GUIManager.RegisterLoginBackTraceScene();
			GameFlowManager.Instance.GUIManager.ShowLoginPopup(true);
			break;
		}
	}

	protected override void OnBack()
	{
		if (GameFlowManager.Instance.GUIManager.IsLoginPopupShowing)
		{
			GameFlowManager.Instance.GUIManager.LoginPopupToBackTraceScene();
		}
		else if (GameFlowManager.Instance.GUIManager.IsCreateAccountPopupShowing)
		{
			GameFlowManager.Instance.GUIManager.ShowCreateAccountPopup(false);
			GameFlowManager.Instance.GUIManager.ShowLoginPopup(true);
		}
		else
		{
			GameFlowManager.Instance.GUIManager.ShowAppQuitPopup(true);
		}
	}
}
