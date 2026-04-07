using UnityEngine;

public class SplashScreen : MonoBehaviour
{
	private Texture2D m_SplashScreenBg;

	private Rect m_SplashScreenBgRect;

	public bool mb_isInitialized;

	private void Awake()
	{
		mb_isInitialized = false;
	}

	private void Init()
	{
		m_SplashScreenBgRect = new Rect(0f, 0f, Screen.width, Screen.height);
		string text = "GUI/SplashScreens/CP_Splash2_1024x768";
		switch (ResolutionManager.Instance.AssetResolution)
		{
		case ResolutionManager.eAssetResolution.eLowres:
			text += "_lowres";
			break;
		case ResolutionManager.eAssetResolution.eIPad:
			text += "_iPad";
			break;
		}
		m_SplashScreenBg = Resources.Load(text, typeof(Texture2D)) as Texture2D;
		if (m_SplashScreenBg == null)
		{
			m_SplashScreenBg = Resources.Load("GUI/SplashScreens/CP_Splash2_1024x768", typeof(Texture2D)) as Texture2D;
		}
		mb_isInitialized = true;
	}

	private void OnGUI()
	{
		if (mb_isInitialized)
		{
			GUI.DrawTexture(m_SplashScreenBgRect, m_SplashScreenBg);
		}
	}

	private void Update()
	{
		if (!mb_isInitialized && GameFlowManager.Instance != null && ResolutionManager.Instance.ResolutionInfoSet)
		{
			Init();
		}
	}
}
