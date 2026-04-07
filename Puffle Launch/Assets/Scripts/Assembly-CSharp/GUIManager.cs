using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
	public enum Scene
	{
		eNone = 0,
		eLoadingScreen = 1,
		eSplashScene = 2,
		eMainMenu = 3,
		eLevelSelect = 4,
		eInGameHud = 5,
		ePauseMenu = 6,
		eTallyMenu = 7,
		eUnlockPopup = 8,
		eLoginPopup = 9,
		eCreateAccountPopup = 10,
		eUpsellPopup = 11,
		eAppQuitPopup = 12,
		eRateMyApp = 13,
		eScene_COUNT = 14
	}

	public GUISkin m_Skin;

	public Color m_WhiteTextDropShadowColor;

	public Color m_DarkBrownTextDropShadowColor;

	public Color m_LightBrownTextDropShadowColor;

	public Color m_DarkerBrownTextDropShadowColor;

	public Color m_GreyBrownTextDropShadowColor;

	public Vector2 m_DropShadowOffsetOriginal;

	public Vector2 m_DropShadowOffsetLowRes;

	public Vector2 m_DropShadowOffsetIPad;

	public List<BasePopup> m_Popups = new List<BasePopup>();

	public Matrix4x4 m_NewResMatrix;

	private LoadingScreen m_LoadingScreen;

	private HudManager m_HudManager;

	private PauseMenu m_PauseMenu;

	private TallyMenu m_TallyMenu;

	private LevelSelectPopup mo_unlockPopup;

	private LoginPopup m_LoginPopup;

	private UpsellPopup m_UpsellPopup;

	private AppQuitPopup m_AppQuitPopup;

	private RateMyAppPopup m_RateMyAppPopup;

	private Scene m_PrevSceneRateMyApp;

	private CreateAccountPopup mo_createAccountPopup;

	private Scene m_CurrentScene;

	private string m_CurrentSceneName = string.Empty;

	private Scene m_LoginBackTraceScene;

	private int m_AboutCPCurrentPage;

	private Font[] m_InGameFonts;

	private Font[] m_CPFonts;

	private Dictionary<string, Font> m_OnDemandFonts = new Dictionary<string, Font>();

	private Font m_SafeFallbackFont;

	private int m_DropShadowOffsetX;

	private int m_DropShadowOffsetY;

	private GameManager.Unlock mo_currentUnlock;

	private bool m_IsPauseMenu;

	private bool m_UpdateLoadingScreen;

	private bool m_EnableAutoResize;

	public bool IsPauseMenu
	{
		get
		{
			return m_IsPauseMenu;
		}
	}

	public GUISkin Skin
	{
		get
		{
			Utilities.AssertMsg(m_Skin != null, "Missing GUI Skin in GUIManager!");
			return m_Skin;
		}
	}

	public HudManager HudManager
	{
		get
		{
			return m_HudManager;
		}
	}

	public LoadingScreen LoadingScreen
	{
		get
		{
			return m_LoadingScreen;
		}
	}

	public Scene CurrentScene
	{
		get
		{
			return m_CurrentScene;
		}
		set
		{
			m_CurrentScene = value;
		}
	}

	public string CurrentSceneName
	{
		get
		{
			return m_CurrentSceneName;
		}
	}

	public Color WhiteDropShadowColor
	{
		get
		{
			return m_WhiteTextDropShadowColor;
		}
	}

	public Color DarkBrownDropShadowColor
	{
		get
		{
			return m_DarkBrownTextDropShadowColor;
		}
	}

	public Color DarkerBrownDropShadowColor
	{
		get
		{
			return m_DarkerBrownTextDropShadowColor;
		}
	}

	public Color GreyBrownDropShadowColor
	{
		get
		{
			return m_GreyBrownTextDropShadowColor;
		}
	}

	public Color LightBrownDropShadowColor
	{
		get
		{
			return m_LightBrownTextDropShadowColor;
		}
	}

	public int DropShadowOffsetX
	{
		get
		{
			return m_DropShadowOffsetX;
		}
	}

	public int DropShadowOffsetY
	{
		get
		{
			return m_DropShadowOffsetY;
		}
	}

	public bool IsLoginPopupShowing
	{
		get
		{
			return m_LoginPopup != null && m_LoginPopup.IsShowing;
		}
	}

	public bool IsUpsellPopupShowing
	{
		get
		{
			return m_UpsellPopup != null && m_UpsellPopup.IsShowing;
		}
	}

	public bool IsAppQuitPopupShowing
	{
		get
		{
			return m_AppQuitPopup != null && m_AppQuitPopup.IsShowing;
		}
	}

	public bool IsRateMyAppPopupShowing
	{
		get
		{
			return m_RateMyAppPopup != null && m_RateMyAppPopup.IsShowing;
		}
	}

	public bool IsCreateAccountPopupShowing
	{
		get
		{
			return mo_createAccountPopup != null && mo_createAccountPopup.IsShowing;
		}
	}

	public int AboutCPCurrentPage
	{
		get
		{
			return m_AboutCPCurrentPage;
		}
	}

	public bool EnableAutoResize
	{
		get
		{
			return m_EnableAutoResize;
		}
	}

	private void Awake()
	{
		Object.DontDestroyOnLoad(this);
	}

	private void Start()
	{
		LanguageInitialization();
		SetGlobalFont();
		SetStyleFont();
		SetDropShadowOffset();
		m_LoadingScreen = new LoadingScreen(base.gameObject);
		m_HudManager = new HudManager(base.gameObject);
		GUIStyleContainer.Init();
		if (LocalizationManager.GetLanguageCode() == "de")
		{
			m_EnableAutoResize = true;
		}
		m_NewResMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3((float)Screen.width / GUIConstants.kReferenceScreenWidth, (float)Screen.height / GUIConstants.kReferenceScreenHeight, 1f));
	}

	private void Update()
	{
		if (m_UpdateLoadingScreen)
		{
			m_LoadingScreen.Update();
		}
		else if (CurrentScene == Scene.eCreateAccountPopup)
		{
			if (mo_createAccountPopup != null)
			{
				mo_createAccountPopup.Update();
			}
		}
		else if (CurrentScene == Scene.eLoginPopup && m_LoginPopup != null)
		{
			m_LoginPopup.Update();
		}
		if (CurrentScene == Scene.eInGameHud || CurrentScene == Scene.ePauseMenu || CurrentScene == Scene.eUnlockPopup || CurrentScene == Scene.eTallyMenu)
		{
			m_HudManager.Update();
		}
	}

	private void OnGUI()
	{
		GUI.matrix = m_NewResMatrix;
		GUI.depth = 0;
		if (m_UpdateLoadingScreen)
		{
			m_LoadingScreen.Draw();
		}
		switch (CurrentScene)
		{
		case Scene.eInGameHud:
			if (!(StartOfGameDelay.Instance == null))
			{
			}
			break;
		case Scene.ePauseMenu:
			m_PauseMenu.Draw();
			break;
		case Scene.eTallyMenu:
			m_TallyMenu.Draw();
			break;
		case Scene.eUnlockPopup:
			mo_unlockPopup.Draw();
			break;
		case Scene.eLoginPopup:
			m_LoginPopup.Draw();
			break;
		case Scene.eUpsellPopup:
			m_UpsellPopup.Draw();
			break;
		case Scene.eCreateAccountPopup:
			if (mo_createAccountPopup != null)
			{
				mo_createAccountPopup.Draw();
			}
			break;
		case Scene.eAppQuitPopup:
			m_AppQuitPopup.Draw();
			break;
		case Scene.eRateMyApp:
			m_RateMyAppPopup.Draw();
			break;
		}
		if ((bool)NetManager.Instance)
		{
			NetManager.Instance.Draw();
		}
	}

	private void LanguageInitialization()
	{
		if (LocalizationManager.IsJapanese)
		{
			GUIConstants.kFontNames = GUIConstants.kFontNamesJA;
		}
		if (LocalizationManager.IsGerman || LocalizationManager.IsJapanese)
		{
			m_EnableAutoResize = true;
		}
	}

	private void SetGlobalFont()
	{
		m_InGameFonts = new Font[4];
		m_CPFonts = new Font[4];
		if (ResolutionManager.Instance.AssetResolution == ResolutionManager.eAssetResolution.eLowres)
		{
			for (int i = 0; i < 4; i++)
			{
				string text = string.Format("{0}{1}_{2}_{3}", "Font/", "LowRes", GUIConstants.kFontNames[0], GUIConstants.kLowResFontSizes[i].ToString());
				m_CPFonts[i] = LoadFontResource(text);
				Utilities.AssertMsg(m_CPFonts[i] != null, "Fail to load Font: " + text);
			}
			for (int j = 0; j < 4; j++)
			{
				string text2 = string.Format("{0}{1}_{2}_{3}", "Font/", "LowRes", GUIConstants.kFontNames[1], GUIConstants.kLowResFontSizes[j].ToString());
				m_InGameFonts[j] = LoadFontResource(text2);
				Utilities.AssertMsg(m_InGameFonts[j] != null, "Fail to load Font: " + text2);
			}
		}
		else
		{
			for (int k = 0; k < 4; k++)
			{
				string text3 = string.Format("{0}{1}_{2}", "Font/", GUIConstants.kFontNames[0], GUIConstants.kFontSizes[k].ToString());
				m_CPFonts[k] = LoadFontResource(text3);
				Utilities.AssertMsg(m_CPFonts[k] != null, "Fail to load Font: " + text3);
			}
			for (int l = 0; l < 4; l++)
			{
				string text4 = string.Format("{0}{1}_{2}", "Font/", GUIConstants.kFontNames[1], GUIConstants.kFontSizes[l].ToString());
				m_InGameFonts[l] = LoadFontResource(text4);
				Utilities.AssertMsg(m_InGameFonts[l] != null, "Fail to load Font: " + text4);
			}
		}
	}

	private void SetStyleFont()
	{
		if (ResolutionManager.Instance.AssetResolution == ResolutionManager.eAssetResolution.eLowres)
		{
			m_Skin.font = GetLowResFont(m_Skin.font);
			m_Skin.label.font = GetLowResFont(m_Skin.label.font);
			m_Skin.textField.font = GetLowResFont(m_Skin.label.font);
			m_Skin.textArea.font = GetLowResFont(m_Skin.label.font);
		}
		else
		{
			m_Skin.font = GetOriginalFont(m_Skin.font);
			m_Skin.label.font = GetOriginalFont(m_Skin.label.font);
			m_Skin.textField.font = GetOriginalFont(m_Skin.label.font);
			m_Skin.textArea.font = GetOriginalFont(m_Skin.label.font);
		}
	}

	private Font GetLowResFont(Font aFont)
	{
		if (aFont == null)
		{
			return GetSafeFallbackFont();
		}
		string[] array = aFont.name.Split('_');
		if (array.Length >= 2 && (array.Length != 3 || !(array[0] == "LowRes")))
		{
			int num;
			if (!int.TryParse(array[1], out num))
			{
				return aFont;
			}
			for (int i = 0; i < 4; i++)
			{
				if (num == GUIConstants.kFontSizes[i])
				{
					return m_CPFonts[i];
				}
			}
		}
		return aFont;
	}

	private Font GetOriginalFont(Font aFont)
	{
		if (aFont == null)
		{
			return GetSafeFallbackFont();
		}
		string[] array = aFont.name.Split('_');
		if (array.Length == 3)
		{
			int num;
			if (!int.TryParse(array[2], out num))
			{
				return aFont;
			}
			for (int i = 0; i < 4; i++)
			{
				if (num == GUIConstants.kLowResFontSizes[i])
				{
					return m_CPFonts[i];
				}
			}
		}
		return aFont;
	}

	public Font GetFont(GUIDefines.FontSize aFontSize, GUIDefines.FontType aFontType)
	{
		switch (aFontType)
		{
		case GUIDefines.FontType.eCPMenus:
			return m_CPFonts[(int)aFontSize];
		case GUIDefines.FontType.eInGame:
			return m_InGameFonts[(int)aFontSize];
		default:
			return m_CPFonts[(int)aFontSize];
		}
	}

	public Font GetOnDemandFont(string aOnDemandFontName)
	{
		Font value;
		if (!m_OnDemandFonts.TryGetValue(aOnDemandFontName, out value))
		{
			value = LoadFontResource("Font/" + aOnDemandFontName);
			if (Utilities.AssertMsg(value != null, "Fail to load on demand font: " + aOnDemandFontName))
			{
				m_OnDemandFonts.Add(aOnDemandFontName, value);
			}
		}
		return value;
	}

	public string GetLowResFontName(GUIDefines.FontType aFontType, GUIDefines.FontSize aFontSize)
	{
		if (aFontType < GUIDefines.FontType.eCPMenus || (int)aFontType >= GUIConstants.kFontNames.Length || aFontSize < GUIDefines.FontSize.eMini || (int)aFontSize >= GUIConstants.kLowResFontSizes.Length)
		{
			Utilities.AssertMsg(false, string.Concat("Fail to Get lowres font name for type: ", aFontType, ", and size: ", aFontSize));
			return string.Empty;
		}
		return string.Format("{0}_{1}_{2}", "LowRes", GUIConstants.kFontNames[(int)aFontType], GUIConstants.kLowResFontSizes[(int)aFontSize].ToString());
	}

	private void SetDropShadowOffset()
	{
		switch (ResolutionManager.Instance.LayoutSize)
		{
		case ResolutionManager.eLayoutSize.eLowres:
			m_DropShadowOffsetX = (int)m_DropShadowOffsetLowRes.x;
			m_DropShadowOffsetY = (int)m_DropShadowOffsetLowRes.y;
			break;
		case ResolutionManager.eLayoutSize.eOriginal:
			m_DropShadowOffsetX = (int)m_DropShadowOffsetOriginal.x;
			m_DropShadowOffsetY = (int)m_DropShadowOffsetOriginal.y;
			break;
		case ResolutionManager.eLayoutSize.eIPad:
			m_DropShadowOffsetX = (int)m_DropShadowOffsetIPad.x;
			m_DropShadowOffsetY = (int)m_DropShadowOffsetIPad.y;
			break;
		}
	}

	public void ChangeCurrentScene(string aSceneName)
	{
		CleanUp();
		m_HudManager.CleanUp();
		GUIUtil.CleanUp();
		switch (aSceneName)
		{
		case "!Loader_MainMenu":
			CurrentScene = Scene.eMainMenu;
			break;
		case "Gameplay":
			m_HudManager.ShowInGameHud(true);
			m_PauseMenu = new PauseMenu(base.gameObject);
			CurrentScene = Scene.eInGameHud;
			break;
		case "LevelSelect":
		case "LevelSelect_Lite":
			CurrentScene = Scene.eLevelSelect;
			break;
		default:
			CurrentScene = Scene.eNone;
			break;
		}
		m_CurrentSceneName = aSceneName;
	}

	private void CleanUp()
	{
		m_PauseMenu = null;
		m_TallyMenu = null;
		mo_unlockPopup = null;
		m_LoginPopup = null;
		m_AppQuitPopup = null;
		m_RateMyAppPopup = null;
		mo_createAccountPopup = null;
		m_OnDemandFonts.Clear();
	}

	public void ShowLoadingScreen()
	{
		CurrentScene = Scene.eLoadingScreen;
		if (m_LoadingScreen != null)
		{
			m_LoadingScreen.Start();
		}
		m_UpdateLoadingScreen = true;
	}

	public void HideLoadingScreen()
	{
		HideLoadingScreen(true);
	}

	public void HideLoadingScreen(bool aResetCurrentScreen)
	{
		if (m_LoadingScreen != null)
		{
			m_LoadingScreen.Stop();
		}
		if (aResetCurrentScreen)
		{
			CurrentScene = Scene.eNone;
		}
		m_UpdateLoadingScreen = false;
	}

	public void UpdateLoadingScreen()
	{
		if (m_LoadingScreen != null)
		{
			m_LoadingScreen.Update();
		}
	}

	public bool CanPause()
	{
		return !GameFlowManager.Instance.GUIManager.IsPauseMenu && GameFlowManager.Instance.GUIManager != null && GameFlowManager.Instance.GUIManager.CurrentScene == Scene.eInGameHud;
	}

	public void ShowPauseMenu(bool aShow)
	{
		if (aShow)
		{
			if (CanPause())
			{
				if (m_PauseMenu == null)
				{
					m_PauseMenu = new PauseMenu(base.gameObject);
				}
				CurrentScene = Scene.ePauseMenu;
				GameManager.Instance.Pause(true);
				AudioManager.Instance.Mute();
				m_IsPauseMenu = true;
			}
		}
		else
		{
			CurrentScene = Scene.eInGameHud;
			GameManager.Instance.Pause(false);
			AudioManager.Instance.Unmute();
			m_IsPauseMenu = false;
		}
	}

	public void ShowTallyMenu(bool aShow)
	{
		if (aShow)
		{
			m_HudManager.ShowInGameHud(false);
			if (m_TallyMenu == null)
			{
				m_TallyMenu = new TallyMenu(base.gameObject);
			}
			Object @object = Resources.Load("Prefabs/GUI/TallyMenuCoinTransfer", typeof(Object));
			if (@object == null)
			{
				@object = Resources.Load(GUIUtil.NormalizeResourcePath("Prefabs/GUI/TallyMenuCoinTransfer"), typeof(Object));
			}
			GameObject coinTransfer3DObject = Object.Instantiate(@object) as GameObject;
			m_TallyMenu.SetCoinTransfer3DObject(coinTransfer3DObject);
			if (Camera.main != null)
			{
				Camera.main.GetComponent<CameraFollow>().ZoomEnabled = false;
			}
			CurrentScene = Scene.eTallyMenu;
		}
		else
		{
			if (Camera.main != null)
			{
				Camera.main.GetComponent<CameraFollow>().ZoomEnabled = true;
			}
			m_HudManager.ShowInGameHud(true);
			CurrentScene = Scene.eInGameHud;
		}
	}

	public void ShowCreateAccountPopup(bool aShow)
	{
		if (aShow)
		{
			mo_createAccountPopup = new CreateAccountPopup(base.gameObject);
			mo_createAccountPopup.Show(true);
			CurrentScene = Scene.eCreateAccountPopup;
		}
		else
		{
			mo_createAccountPopup = null;
			CurrentScene = Scene.eNone;
		}
	}

	public void ShowUnlockPopups(bool aShow)
	{
		if (aShow)
		{
			mo_currentUnlock = GameManager.Instance.FindNextUnlock(GameManager.Unlock.eUnlock_None);
			if (mo_currentUnlock != GameManager.Unlock.eUnlock_None)
			{
				mo_unlockPopup = new LevelSelectPopup(base.gameObject);
				mo_unlockPopup.RegisterCallback(OnUnlockPopupDismissCallback);
				SetPopupPageID(mo_currentUnlock);
				mo_unlockPopup.Show(true);
			}
			CurrentScene = Scene.eUnlockPopup;
		}
		else
		{
			CurrentScene = Scene.eInGameHud;
		}
	}

	private void SetPopupPageID(GameManager.Unlock ae_unlock)
	{
		switch (ae_unlock)
		{
		case GameManager.Unlock.eUnlock_TimeTrial:
			mo_unlockPopup.SetPageID(LevelSelectPopup.PageID.TimeTrialUnlocked);
			break;
		case GameManager.Unlock.eUnlock_TimeTrialSilver:
			mo_unlockPopup.SetPageID(LevelSelectPopup.PageID.TimeTrialSilverAchieved);
			break;
		case GameManager.Unlock.eUnlock_TimeTrialGold:
			mo_unlockPopup.SetPageID(LevelSelectPopup.PageID.TimeTrialGoldAchieved);
			break;
		case GameManager.Unlock.eUnlock_TurboMode:
			mo_unlockPopup.SetPageID(LevelSelectPopup.PageID.TurboModeUnlocked);
			break;
		case GameManager.Unlock.eUnlock_SlowMotion:
			mo_unlockPopup.SetPageID(LevelSelectPopup.PageID.SlowMotionUnlocked);
			break;
		}
	}

	private void OnUnlockPopupDismissCallback(int aButtonSelected)
	{
		mo_currentUnlock = GameManager.Instance.FindNextUnlock(mo_currentUnlock);
		if (mo_currentUnlock != GameManager.Unlock.eUnlock_None)
		{
			SetPopupPageID(mo_currentUnlock);
			mo_unlockPopup.Show(true);
		}
		else
		{
			GameFlowManager.Instance.GUIManager.ShowTallyMenu(true);
			mo_unlockPopup = null;
		}
	}

	public void ShowLoginPopup(bool aShow)
	{
		if (aShow)
		{
			CurrentScene = Scene.eLoginPopup;
			m_LoginPopup = new LoginPopup(base.gameObject);
			m_LoginPopup.Show(true);
		}
		else
		{
			CurrentScene = Scene.eNone;
			m_LoginPopup = null;
		}
	}

	public void ShowUpsellPopup(bool aShow)
	{
		if (aShow)
		{
			CurrentScene = Scene.eUpsellPopup;
			m_UpsellPopup = new UpsellPopup(base.gameObject);
			m_UpsellPopup.Show(true);
		}
		else
		{
			CurrentScene = Scene.eNone;
			m_UpsellPopup = null;
		}
	}

	public void ShowAppQuitPopup(bool aShow)
	{
		if (aShow)
		{
			CurrentScene = Scene.eAppQuitPopup;
			m_AppQuitPopup = new AppQuitPopup(base.gameObject);
			m_AppQuitPopup.Show(true);
		}
		else
		{
			CurrentScene = Scene.eNone;
			m_AppQuitPopup.Show(false);
			m_AppQuitPopup = null;
		}
	}

	public void ShowRateMyAppPopup(bool aShow)
	{
		if (aShow)
		{
			m_PrevSceneRateMyApp = CurrentScene;
			CurrentScene = Scene.eRateMyApp;
			m_RateMyAppPopup = new RateMyAppPopup(base.gameObject);
			m_RateMyAppPopup.Show(true);
		}
		else
		{
			CurrentScene = m_PrevSceneRateMyApp;
			m_RateMyAppPopup.Show(false);
			m_RateMyAppPopup = null;
		}
	}

	public void LoginPopupToBackTraceScene()
	{
		ShowLoginPopup(false);
		CurrentScene = m_LoginBackTraceScene;
		m_LoginBackTraceScene = Scene.eNone;
	}

	public void CreateAccountPopupToBackTraceScene()
	{
		ShowCreateAccountPopup(false);
		CurrentScene = m_LoginBackTraceScene;
		m_LoginBackTraceScene = Scene.eNone;
	}

	public void RegisterLoginBackTraceScene()
	{
		m_LoginBackTraceScene = CurrentScene;
	}

	public void RegisterAboutCPCurrentPage(int aCurrentPage)
	{
		m_AboutCPCurrentPage = aCurrentPage;
	}

	public void UnregisterAboutCPCurrentPage()
	{
		m_AboutCPCurrentPage = 0;
	}

	private Font GetSafeFallbackFont()
	{
		if (m_SafeFallbackFont == null)
		{
			m_SafeFallbackFont = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
		}
		return m_SafeFallbackFont;
	}

	private Font LoadFontResource(string aResourcePath)
	{
		Font font = Resources.Load(aResourcePath, typeof(Font)) as Font;
		if (font == null)
		{
			font = Resources.Load(GUIUtil.NormalizeResourcePath(aResourcePath), typeof(Font)) as Font;
		}
		if (font == null)
		{
			font = GetSafeFallbackFont();
		}
		return font;
	}
}
