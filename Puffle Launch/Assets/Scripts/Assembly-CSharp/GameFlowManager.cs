using System.Collections;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
	public static bool mLoadingDone;

	public bool m_DoWindowBack;

	private static GameFlowManager m_cInstance;

	private GUIManager m_GUIManager;

	private InputController m_InputController;

	private AudioClip m_MenuClick24;

	private AudioManager mAudioManager;

	private string m_UnlockScreen = "Unlock phone";

	private AndroidJavaObject m_LockScreen;

	public static GameFlowManager Instance
	{
		get
		{
			return m_cInstance;
		}
	}

	public GUIManager GUIManager
	{
		get
		{
			return m_GUIManager;
		}
	}

	public AudioManager AudioManager
	{
		get
		{
			return mAudioManager;
		}
	}

	public InputController InputController
	{
		get
		{
			return m_InputController;
		}
	}

	public AudioClip MenuClick24
	{
		get
		{
			return m_MenuClick24;
		}
	}

	private void Awake()
	{
		BizIntel.StartBizIntel();
		if (ResolutionManager.Instance.AssetResolution == ResolutionManager.eAssetResolution.eLowres)
		{
			QualitySettings.currentLevel = QualityLevel.Fastest;
		}
		m_cInstance = this;
		m_InputController = GetComponent<InputController>();
		Object.DontDestroyOnLoad(m_cInstance);
		mAudioManager = GetComponent<AudioManager>();
	}

	private void Start()
	{
		m_MenuClick24 = Resources.Load("Sounds/UI/Menu_Click24", typeof(AudioClip)) as AudioClip;
		Object.DontDestroyOnLoad(m_MenuClick24);
		base.gameObject.name = GetType().ToString();
		if (Application.platform == RuntimePlatform.Android && !Application.isEditor)
		{
			try
			{
				AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
				if (androidJavaObject != null)
				{
					m_LockScreen = new AndroidJavaObject("com.bhvr.LockScreen.LockScreen", androidJavaObject, base.gameObject.name, "LockScreenMsg", m_UnlockScreen);
				}
			}
			catch
			{
				Debug.LogWarning("GameFlowManager: Android lock-screen bridge unavailable.");
			}
		}
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			AudioManager.PlayUISFx(MenuClick24);
			if (GUIManager.m_Popups.Count > 0)
			{
				int index = GUIManager.m_Popups.Count - 1;
				GUIManager.m_Popups[index].ClosePopup();
			}
			else
			{
				m_DoWindowBack = true;
			}
		}
		ResolutionManager.Instance.CheckDeviceOrientation();
		if (m_GUIManager == null)
		{
			GameObject gameObject = Object.Instantiate(Resources.Load("Prefabs/Managers/GUIManager", typeof(Object))) as GameObject;
			if (Utilities.Assert(gameObject != null))
			{
				m_GUIManager = gameObject.GetComponent(typeof(GUIManager)) as GUIManager;
			}
		}
	}

	private void OnDestroy()
	{
		if (m_GUIManager != null)
		{
			Object.DestroyImmediate(m_GUIManager.gameObject);
			m_GUIManager = null;
		}
		BizIntel.StopBizIntel();
	}

	public void LoadSceneImmediate(string aSceneName, bool aLeaveLoadingScreen)
	{
		m_GUIManager.ShowLoadingScreen();
		Application.LoadLevel(aSceneName);
		m_GUIManager.ChangeCurrentScene(aSceneName);
	}

	public void LoadScene(string aSceneName, bool aLeaveLoadingScreen)
	{
		StartCoroutine(LoadNewSceneASync(aSceneName, aLeaveLoadingScreen));
	}

	private IEnumerator LoadNewSceneASync(string aSceneName, bool aLeaveLoadingScreen)
	{
		m_GUIManager.ShowLoadingScreen();
		AsyncOperation asyncInfo = Application.LoadLevelAsync(aSceneName);
		while (!asyncInfo.isDone)
		{
			yield return null;
		}
		if (!aLeaveLoadingScreen)
		{
			m_GUIManager.HideLoadingScreen();
		}
		m_GUIManager.ChangeCurrentScene(aSceneName);
	}

	public IEnumerator UnloadUnusedResources()
	{
		AsyncOperation unload = Resources.UnloadUnusedAssets();
		while (!unload.isDone)
		{
			yield return null;
		}
	}

	private void OnApplicationPause(bool aState)
	{
		if (aState && CinematicManager.Instance == null)
		{
			AudioManager.Instance.ForceMute();
		}
	}

	public void LockScreenMsg(string msg)
	{
		if (msg == m_UnlockScreen)
		{
			AudioManager.Instance.ResetMute();
		}
	}
}
