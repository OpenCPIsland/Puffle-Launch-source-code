using UnityEngine;

public class LevelButtonController : MonoBehaviour
{
	private const float kDesktopClickDebounce = 0.25f;

	public enum State
	{
		eLock = 0,
		eUnlock = 1,
		eCompleted = 2,
		eState_COUNT = 3
	}

	protected SizeCategory.CategoryId m_AssetSizeCategoryId = SizeCategory.CategoryId.eUnknown;

	protected LocalizationManager.Language m_AssetLanguage;

	public int buttonID;

	public GameObject mo_EmptyPuffleOTexture;

	public GameObject mo_PuffleORankTexture;

	public GameObject mo_ClockTexture;

	public GameObject mo_PadlockTexture;

	public GameObject mo_LevelNumber;

	public GameObject mo_NewText;

	public GameObject mo_PuffleOCount;

	public GameObject mo_Time;

	private BHUIButton m_ezButton;

	private int mCurrentButtonID;

	private string timeStr;

	private AutoAdjustSpriteText mLevelNumberColor;

	private string materialName = string.Empty;

	private string buttonMaterialName = string.Empty;

	private bool isPositionInitialized;

	private bool mDesktopPressActive;

	private float mLastLoadRequestTime = -1f;

	private void Start()
	{
		m_ezButton = GetComponent<BHUIButton>();
		Initialize();
		SetButtonElements();
		EnsureDesktopClickCollider();
	}

	private void Update()
	{
		if (!isPositionInitialized)
		{
			SetPosition();
			SetText();
			EnsureDesktopClickCollider();
			isPositionInitialized = true;
		}
	}

	private void Initialize()
	{
		if (mLevelNumberColor == null)
		{
			mLevelNumberColor = mo_LevelNumber.GetComponent<AutoAdjustSpriteText>();
		}
		if (GameManager.Instance.CurrentWorld == GameManager.World.eWorld_BlueSky)
		{
			mLevelNumberColor.m_SpriteTextColor = AutoAdjustSpriteText.SpriteTextColor.eBlue;
			materialName = "PadlockBlue";
		}
		else if (GameManager.Instance.CurrentWorld == GameManager.World.eWorld_SodaSunset)
		{
			mLevelNumberColor.m_SpriteTextColor = AutoAdjustSpriteText.SpriteTextColor.eOrange;
			materialName = "PadlockRed";
		}
		else
		{
			mLevelNumberColor.m_SpriteTextColor = AutoAdjustSpriteText.SpriteTextColor.eOrange;
			materialName = "PadlockYellow";
		}
		mLevelNumberColor.AutoAdjust();
		ChangeMaterial(mo_PadlockTexture, materialName);
	}

	private void SetButtonElements()
	{
		if (ShouldHighlight())
		{
			switch (GameManager.Instance.CurrentWorld)
			{
			default:
				buttonMaterialName = "LevelSelectButtonBlueHighlight";
				break;
			case GameManager.World.eWorld_SodaSunset:
				buttonMaterialName = "LevelSelectButtonRedHighlight";
				break;
			case GameManager.World.eWorld_BonusWorld:
				buttonMaterialName = "LevelSelectButtonYellowHighlight";
				break;
			}
			mo_PadlockTexture.GetComponent<Renderer>().enabled = false;
			mo_EmptyPuffleOTexture.GetComponent<Renderer>().enabled = false;
			mo_ClockTexture.GetComponent<Renderer>().enabled = false;
			mo_NewText.GetComponent<Renderer>().enabled = false;
			mo_Time.GetComponent<Renderer>().enabled = false;
			mo_PuffleORankTexture.GetComponent<Renderer>().enabled = true;
			mo_PuffleOCount.GetComponent<Renderer>().enabled = true;
			materialName = "PuffleORankFire";
			if (!ProfileManager.Instance.CurrentProfile.m_LevelData[mCurrentButtonID - 1].TurboLevelComplete)
			{
				float num = (float)ProfileManager.Instance.CurrentProfile.m_LevelData[mCurrentButtonID - 1].BestRingCount / (float)GameManager.smMaxRingInLevel[mCurrentButtonID - 1];
				if (num >= 1f)
				{
					materialName = "PuffleORankGold";
				}
				else if (num >= 0.5f)
				{
					materialName = "PuffleORankSilver";
				}
				else
				{
					materialName = "PuffleORankBronze";
				}
			}
			ChangeMaterial(mo_PuffleORankTexture, materialName);
			if (GameManager.HasCollectedAllRings(GameManager.Instance.CurrentWorld) && ProfileManager.Instance.CurrentProfile.m_LevelData[mCurrentButtonID - 1].BestTimeCount != float.MaxValue && GameManager.Instance.CurrentWorld != GameManager.World.eWorld_BonusWorld)
			{
				mo_ClockTexture.GetComponent<Renderer>().enabled = true;
				mo_Time.GetComponent<Renderer>().enabled = true;
				timeStr = GameManager.GetTimeFormatedString(ProfileManager.Instance.CurrentProfile.m_LevelData[mCurrentButtonID - 1].BestTimeCount).ToString();
			}
		}
		else
		{
			switch (GameManager.Instance.CurrentWorld)
			{
			default:
				buttonMaterialName = "LevelSelectButtonBlue";
				break;
			case GameManager.World.eWorld_SodaSunset:
				buttonMaterialName = "LevelSelectButtonRed";
				break;
			case GameManager.World.eWorld_BonusWorld:
				buttonMaterialName = "LevelSelectButtonYellow";
				break;
			}
			if (IsLevelUnlocked())
			{
				mo_PadlockTexture.GetComponent<Renderer>().enabled = false;
				mo_PuffleORankTexture.GetComponent<Renderer>().enabled = false;
				mo_ClockTexture.GetComponent<Renderer>().enabled = false;
				mo_PuffleOCount.GetComponent<Renderer>().enabled = false;
				mo_EmptyPuffleOTexture.GetComponent<Renderer>().enabled = true;
				mo_NewText.GetComponent<Renderer>().enabled = true;
				mo_Time.GetComponent<Renderer>().enabled = false;
			}
			else
			{
				mo_EmptyPuffleOTexture.GetComponent<Renderer>().enabled = false;
				mo_PuffleORankTexture.GetComponent<Renderer>().enabled = false;
				mo_ClockTexture.GetComponent<Renderer>().enabled = false;
				mo_NewText.GetComponent<Renderer>().enabled = false;
				mo_PuffleOCount.GetComponent<Renderer>().enabled = false;
				mo_Time.GetComponent<Renderer>().enabled = false;
				mo_PadlockTexture.GetComponent<Renderer>().enabled = true;
			}
		}
		ChangeMaterial(base.gameObject, buttonMaterialName);
	}

	public void LoadLevel()
	{
		if (mLastLoadRequestTime >= 0f && Time.realtimeSinceStartup - mLastLoadRequestTime < kDesktopClickDebounce)
		{
			return;
		}
		if (IsLevelUnlocked())
		{
			mLastLoadRequestTime = Time.realtimeSinceStartup;
			AssetLoader.Instance.ScrollList.SetActiveRecursively(false);
			if (mCurrentButtonID == 1 && CinematicManager.Instance != null)
			{
				LevelSelect.Instance.MainScreen.StopGUI();
				CinematicManager.Instance.ShowFullscreenBgWhenPlaying = true;
				CinematicManager.Instance.playCompleted += MoviePlayCompleted;
				CinematicManager.Instance.Play(CinematicManager.MovieId.eIntro);
			}
			else
			{
				StartSelectedLevel();
			}
		}
	}

	private void MoviePlayCompleted(bool aSuccess)
	{
		AudioManager.Instance.ResetMute();
		Resources.UnloadUnusedAssets();
		StartSelectedLevel();
	}

	private void StartSelectedLevel()
	{
		LevelSelect.Instance.MainScreen.StopGUI();
		LevelSelect.SelectedLevel = mCurrentButtonID;
		GameManager.Instance.StartLevel((GameManager.Level)(mCurrentButtonID - 1));
		GameFlowManager.Instance.LoadScene("Gameplay", true);
	}

	private void SetText()
	{
		if (base.transform.Find("LevelNumber") != null)
		{
			mo_LevelNumber.GetComponent<BHUILabel>().Text = mCurrentButtonID.ToString();
			mo_LevelNumber.GetComponent<BHUILabel>().UpdateDropShadow();
		}
		string text = string.Format("{0}/{1}", ProfileManager.Instance.CurrentProfile.m_LevelData[mCurrentButtonID - 1].BestRingCount, GameManager.smMaxRingInLevel[mCurrentButtonID - 1]);
		if (base.transform.Find("PuffleOCount") != null)
		{
			mo_PuffleOCount.GetComponent<BHUILabel>().Text = text;
			mo_PuffleOCount.GetComponent<BHUILabel>().UpdateDropShadow();
		}
		if (base.transform.Find("Time") != null && timeStr != null)
		{
			mo_Time.GetComponent<BHUILabel>().Text = timeStr;
			mo_Time.GetComponent<BHUILabel>().UpdateDropShadow();
		}
	}

	private void SetPosition()
	{
		if (mo_ClockTexture != null)
		{
			mo_ClockTexture.transform.localPosition = new Vector3(-0.15f * m_ezButton.width, -0.187f * m_ezButton.height, -1f);
		}
		if (mo_PuffleORankTexture != null)
		{
			mo_PuffleORankTexture.transform.localPosition = new Vector3(0f, 0.15f * m_ezButton.height, -1f);
		}
		if (mo_EmptyPuffleOTexture != null)
		{
			mo_EmptyPuffleOTexture.transform.localPosition = new Vector3(0f, 0.09f * m_ezButton.height, -1f);
		}
		if (mo_PadlockTexture != null)
		{
			mo_PadlockTexture.transform.localPosition = new Vector3(0f, 0f, -1f);
		}
		if (mo_LevelNumber != null)
		{
			mo_LevelNumber.transform.localPosition = new Vector3(-0.29f * m_ezButton.width, 0.32f * m_ezButton.height, -0.1f);
		}
		if (mo_NewText != null)
		{
			mo_NewText.transform.localPosition = new Vector3(0f, -0.15f * m_ezButton.height, -0.5f);
			if (LocalizationManager.GetLanguageCode() == "fr" && ResolutionManager.Instance.LayoutSize == ResolutionManager.eLayoutSize.eIPad)
			{
				mo_NewText.GetComponent<BHUILabel>().pixelPerfect = false;
				mo_NewText.GetComponent<BHUILabel>().SetCharacterSize(0.84f);
				mo_NewText.GetComponent<BHUILabel>().UpdateDropShadow();
			}
		}
		if (mo_PuffleOCount != null)
		{
			mo_PuffleOCount.transform.localPosition = new Vector3(0f, -0.033f * m_ezButton.height, -0.5f);
			if (ResolutionManager.Instance.LayoutSize == ResolutionManager.eLayoutSize.eLowres)
			{
				mo_PuffleOCount.GetComponent<BHUILabel>().pixelPerfect = false;
				mo_PuffleOCount.GetComponent<BHUILabel>().SetCharacterSize(1f);
				mo_PuffleOCount.GetComponent<BHUILabel>().UpdateDropShadow();
			}
		}
		if (mo_Time != null)
		{
			mo_Time.transform.localPosition = new Vector3(-0.042f * m_ezButton.width, -0.123f * m_ezButton.height, -0.5f);
		}
	}

	private bool ShouldHighlight()
	{
		return ProfileManager.Instance.CurrentProfile.m_LevelData[mCurrentButtonID - 1].LevelComplete;
	}

	private bool IsLevelUnlocked()
	{
		return ProfileManager.Instance.CurrentProfile.m_LevelData[mCurrentButtonID - 1].LevelUnlocked;
	}

	public void ChangeList()
	{
		mCurrentButtonID = buttonID + 12 * (int)GameManager.Instance.CurrentWorld;
		Initialize();
		SetButtonElements();
		EnsureDesktopClickCollider();
		if (isPositionInitialized)
		{
			SetText();
		}
	}

	private bool CanUseDesktopMouseFallback()
	{
		return Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.LinuxPlayer;
	}

	private bool CanInteract()
	{
		return m_ezButton != null && m_ezButton.controlIsEnabled && base.gameObject.activeInHierarchy;
	}

	private void EnsureDesktopClickCollider()
	{
		if (m_ezButton == null)
		{
			return;
		}
		BoxCollider boxCollider = GetComponent<BoxCollider>();
		if (boxCollider == null)
		{
			boxCollider = (BoxCollider)base.gameObject.AddComponent(typeof(BoxCollider));
		}
		boxCollider.center = m_ezButton.GetCenterPoint();
		boxCollider.size = new Vector3(Mathf.Max(m_ezButton.width, 0.001f), Mathf.Max(m_ezButton.height, 0.001f), 0.25f);
		boxCollider.isTrigger = true;
	}

	private void OnMouseDown()
	{
		if (!CanUseDesktopMouseFallback() || !CanInteract())
		{
			return;
		}
		mDesktopPressActive = true;
		m_ezButton.SetControlState(UIButton.CONTROL_STATE.ACTIVE);
	}

	private void OnMouseExit()
	{
		if (!mDesktopPressActive || m_ezButton == null)
		{
			return;
		}
		mDesktopPressActive = false;
		m_ezButton.SetControlState((!CanInteract()) ? UIButton.CONTROL_STATE.DISABLED : UIButton.CONTROL_STATE.NORMAL);
	}

	private void OnMouseUp()
	{
		if (!mDesktopPressActive || m_ezButton == null)
		{
			return;
		}
		mDesktopPressActive = false;
		m_ezButton.SetControlState((!CanInteract()) ? UIButton.CONTROL_STATE.DISABLED : UIButton.CONTROL_STATE.NORMAL);
	}

	private void OnMouseUpAsButton()
	{
		if (!CanUseDesktopMouseFallback() || !CanInteract())
		{
			return;
		}
		mDesktopPressActive = false;
		m_ezButton.SetControlState(UIButton.CONTROL_STATE.OVER);
		LoadLevel();
	}

	private void ChangeMaterial(GameObject aGameObject, string aMaterialName)
	{
		aGameObject.GetComponent<MeshRenderer>().material = Resources.Load("EZGUI/LevelSelect/" + aMaterialName, typeof(Material)) as Material;
		ResourceLoader.Instance.SetMaterialTexture(aGameObject, "EZGUI/LevelSelect/", false, out m_AssetSizeCategoryId, out m_AssetLanguage);
	}
}
