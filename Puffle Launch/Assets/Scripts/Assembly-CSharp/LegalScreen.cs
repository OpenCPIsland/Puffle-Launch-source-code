using UnityEngine;

public class LegalScreen : MonoBehaviour
{
	public float m_TimeToDisplay = 2f;

	public GameObject nextObject;

	public GameObject Logo;

	private static LegalScreen m_cInstance;

	private float m_Timer;

	private bool m_IsDone;

	private Transform m_LogoTransform;

	private Texture2D m_SplashScreenBg;

	private Rect m_LoadingBottomBarRect;

	private Texture2D m_LoadingBarFrameBg;

	private Rect m_LoadingBarFrameBgRect;

	private Texture2D m_LoadingBar;

	private Rect m_LoadingBarRect;

	private Texture2D m_LoadingBarFrame;

	private Rect m_LoadingBarFrameRect;

	private float m_LoadingBarTotalWidth;

	private GameObject m_GameFlowObject;

	private GameObject m_AssetLoaderObject;

	public static LegalScreen Instance
	{
		get
		{
			return m_cInstance;
		}
	}

	public bool IsDone
	{
		get
		{
			return m_IsDone;
		}
	}

	private void Awake()
	{
		ResolutionManager.Instance.CheckDeviceOrientation();
		Object.DestroyImmediate(GameObject.Find("LITE"));
		if (GameFlowManager.mLoadingDone)
		{
			nextObject.gameObject.SetActiveRecursively(true);
			base.gameObject.SetActiveRecursively(false);
		}
		else
		{
			m_GameFlowObject = LoadGameObjectResource("Prefabs/GameFlowManager");
			if (Utilities.AssertMsgCritical(m_GameFlowObject != null, "Danger, Will Robinson! Danger!\nCannot Load the GameFlowManger Object...!!"))
			{
				Object.Instantiate(m_GameFlowObject);
			}
			m_AssetLoaderObject = LoadGameObjectResource("Prefabs/AssetLoader");
			if (m_AssetLoaderObject != null)
			{
				Object.Instantiate(m_AssetLoaderObject);
			}
		}
		m_cInstance = this;
		m_Timer = 0f;
		m_LogoTransform = Logo.transform;
		SetLogoScaling();
		string text = "GUI/MainMenu/Textures/Loading/BottomBar";
		switch (ResolutionManager.Instance.AssetResolution)
		{
		case ResolutionManager.eAssetResolution.eLowres:
			text += "_lowres";
			break;
		case ResolutionManager.eAssetResolution.eIPad:
			text += "_iPad";
			break;
		}
		m_SplashScreenBg = GUIUtil.LoadTexture2D(text);
		if (m_SplashScreenBg == null)
		{
			m_SplashScreenBg = GUIUtil.LoadTexture2D("GUI/MainMenu/Textures/Loading/BottomBar");
		}
		m_LoadingBarFrame = GUIUtil.LoadTexture2D("GUI/LoadingScreen/AnimatedLoadingScreen/bar_frame");
		m_LoadingBar = GUIUtil.LoadTexture2D("GUI/LoadingScreen/AnimatedLoadingScreen/bar_filler_slice");
		if (ResolutionManager.Instance.LayoutSize == ResolutionManager.eLayoutSize.eIPad)
		{
			m_LoadingBarFrameRect = new Rect(0.34863f * (float)Screen.width, 0.89973f * (float)Screen.height, 0.33984f * (float)Screen.width, 0.05989f * (float)Screen.height);
			m_LoadingBarRect = new Rect(0.3623f * (float)Screen.width, 0.91147f * (float)Screen.height, 0f, 0.02864f * (float)Screen.height);
			m_LoadingBarTotalWidth = 0.31445f * (float)Screen.width;
			m_LoadingBottomBarRect = new Rect(0f, 0.8138f * (float)Screen.height, Screen.width, 0.1862f * (float)Screen.height);
		}
		else
		{
			m_LoadingBarFrameRect = new Rect(0.34062f * (float)Screen.width, 143f / 160f * (float)Screen.height, 0.3625f * (float)Screen.width, 0.07031f * (float)Screen.height);
			m_LoadingBarRect = new Rect(0.35521f * (float)Screen.width, 0.90469f * (float)Screen.height, 0f, 0.04844f * (float)Screen.height);
			m_LoadingBarTotalWidth = 0.33438f * (float)Screen.width;
			m_LoadingBottomBarRect = new Rect(0f, 0.79999f * (float)Screen.height, Screen.width, 0.20313f * (float)Screen.height);
		}
	}

	private void Update()
	{
		if (m_Timer >= m_TimeToDisplay && !GameFlowManager.mLoadingDone)
		{
			m_Timer = 0f;
			m_IsDone = true;
			Logo.GetComponent<Dynamic3DBackground>().mb_forceToEnglish = false;
			Logo.GetComponent<Dynamic3DBackground>().LoadNewTexture("GUI/Logo/", "pl_logo");
			SetLogoScaling();
			GameFlowManager.mLoadingDone = true;
			nextObject.gameObject.SetActiveRecursively(true);
			base.gameObject.SetActiveRecursively(false);
		}
		if (m_LoadingBar != null && m_LoadingBarFrame != null)
		{
			m_LoadingBarRect.width += m_LoadingBarTotalWidth / m_TimeToDisplay * Time.deltaTime;
			m_LoadingBarRect.width = Mathf.Clamp(m_LoadingBarRect.width, 0f, m_LoadingBarTotalWidth);
		}
		m_Timer += Time.deltaTime;
	}

	private void OnGUI()
	{
		if (m_SplashScreenBg != null)
		{
			GUI.DrawTexture(m_LoadingBottomBarRect, m_SplashScreenBg);
		}
		if (m_LoadingBar != null && m_LoadingBarFrame != null)
		{
			GUI.DrawTexture(m_LoadingBarRect, m_LoadingBar);
			GUI.DrawTexture(m_LoadingBarFrameRect, m_LoadingBarFrame);
		}
	}

	private void SetLogoScaling()
	{
		switch (ResolutionManager.Instance.AssetResolution)
		{
		case ResolutionManager.eAssetResolution.eLowres:
			switch (LocalizationManager.GetLanguageCode())
			{
			case "en":
				m_LogoTransform.position = new Vector3(0.25f, 2.76f, -3f);
				m_LogoTransform.localScale = new Vector3(1.23f, 1f, 1.31f);
				break;
			case "fr":
				m_LogoTransform.position = new Vector3(0.25f, 2.4f, -3f);
				m_LogoTransform.localScale = new Vector3(1.42f, 1f, 1.47f);
				break;
			case "es":
				m_LogoTransform.position = new Vector3(-0.21f, 2.02f, -3f);
				m_LogoTransform.localScale = new Vector3(1.6f, 1f, 1.43f);
				break;
			case "pt":
				m_LogoTransform.position = new Vector3(-0.3f, 2.32f, -3f);
				m_LogoTransform.localScale = new Vector3(1.48f, 1f, 1.46f);
				break;
			}
			break;
		case ResolutionManager.eAssetResolution.eIPad:
			switch (LocalizationManager.GetLanguageCode())
			{
			case "en":
				m_LogoTransform.position = new Vector3(0.19f, 1.56f, -3f);
				m_LogoTransform.localScale = new Vector3(1.05f, 1f, 1.05f);
				break;
			case "fr":
				m_LogoTransform.position = new Vector3(0.42f, 1.35f, -3f);
				m_LogoTransform.localScale = new Vector3(1f, 1f, 1.16f);
				break;
			case "es":
				m_LogoTransform.position = new Vector3(0.42f, 1.48f, -3f);
				m_LogoTransform.localScale = new Vector3(1.11f, 1f, 1.12f);
				break;
			case "pt":
				m_LogoTransform.position = new Vector3(-0.25f, 1.45f, -3f);
				m_LogoTransform.localScale = new Vector3(1.08f, 1f, 1.18f);
				break;
			}
			break;
		default:
			switch (LocalizationManager.GetLanguageCode())
			{
			case "en":
				m_LogoTransform.position = new Vector3(0.25f, 2.73f, -3f);
				m_LogoTransform.localScale = new Vector3(1.17f, 1f, 1.23f);
				break;
			case "fr":
				m_LogoTransform.position = new Vector3(0.25f, 2.56f, -3f);
				m_LogoTransform.localScale = new Vector3(1.2f, 1f, 1.4f);
				break;
			case "es":
				m_LogoTransform.position = new Vector3(0f, 2.49f, -3f);
				m_LogoTransform.localScale = new Vector3(1.33f, 1f, 1.32f);
				break;
			case "pt":
				m_LogoTransform.position = new Vector3(-0.37f, 2.84f, -3f);
				m_LogoTransform.localScale = new Vector3(1.28f, 1f, 1.43f);
				break;
			}
			break;
		}
	}

	private static GameObject LoadGameObjectResource(string aResourcePath)
	{
		GameObject gameObject = Resources.Load(aResourcePath, typeof(GameObject)) as GameObject;
		if (gameObject == null)
		{
			gameObject = Resources.Load(GUIUtil.NormalizeResourcePath(aResourcePath), typeof(GameObject)) as GameObject;
		}
		return gameObject;
	}
}
