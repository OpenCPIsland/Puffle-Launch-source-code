using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public enum Level
	{
		eLevel_1 = 0,
		eLevel_2 = 1,
		eLevel_3 = 2,
		eLevel_4 = 3,
		eLevel_5 = 4,
		eLevel_6 = 5,
		eLevel_EndLite = 5,
		eLevel_7 = 6,
		eLevel_8 = 7,
		eLevel_9 = 8,
		eLevel_10 = 9,
		eLevel_11 = 10,
		eLevel_12 = 11,
		eLevel_EndWorld1 = 11,
		eLevel_13 = 12,
		eLevel_14 = 13,
		eLevel_15 = 14,
		eLevel_16 = 15,
		eLevel_17 = 16,
		eLevel_18 = 17,
		eLevel_19 = 18,
		eLevel_20 = 19,
		eLevel_21 = 20,
		eLevel_22 = 21,
		eLevel_23 = 22,
		eLevel_24 = 23,
		eLevel_EndWorld2 = 23,
		eLevel_FirstBonusLevel = 24,
		eLevel_25 = 24,
		eLevel_26 = 25,
		eLevel_27 = 26,
		eLevel_28 = 27,
		eLevel_29 = 28,
		eLevel_30 = 29,
		eLevel_31 = 30,
		eLevel_32 = 31,
		eLevel_33 = 32,
		eLevel_34 = 33,
		eLevel_35 = 34,
		eLevel_36 = 35,
		eLevel_37 = 36,
		eLevel_38 = 37,
		eLevel_39 = 38,
		eLevel_40 = 39,
		eLevel_41 = 40,
		eLevel_42 = 41,
		eLevel_43 = 42,
		eLevel_44 = 43,
		eLevel_45 = 44,
		eLevel_46 = 45,
		eLevel_47 = 46,
		eLevel_48 = 47,
		eLevel_49 = 48,
		eLevel_50 = 49,
		eLevel_51 = 50,
		eLevel_52 = 51,
		eLevel_53 = 52,
		eLevel_54 = 53,
		eLevel_55 = 54,
		eLevel_56 = 55,
		eLevel_57 = 56,
		eLevel_58 = 57,
		eLevel_59 = 58,
		eLevel_60 = 59,
		eLevel_61 = 60,
		eLevel_62 = 61,
		eLevel_63 = 62,
		eLevel_64 = 63,
		eLevel_65 = 64,
		eLevel_66 = 65,
		eLevel_67 = 66,
		eLevel_68 = 67,
		eLevel_69 = 68,
		eLevel_70 = 69,
		eLevel_71 = 70,
		eLevel_72 = 71,
		eLevel_EndBonusWorld = 59,
		eLevel_COUNT = 60
	}

	public enum World
	{
		eWorld_BlueSky = 0,
		eWorld_SodaSunset = 1,
		eWorld_BonusWorld = 2,
		eWorld_COUNT = 3
	}

	public enum Unlock
	{
		eUnlock_None = -1,
		eUnlock_TimeTrial = 0,
		eUnlock_TimeTrialSilver = 1,
		eUnlock_TimeTrialGold = 2,
		eUnlock_TurboMode = 3,
		eUnlock_SlowMotion = 4,
		eUnlock_Num = 5
	}

	public enum LevelTimes
	{
		eTime_None = 0,
		eTime_Silver = 1,
		eTime_Gold = 2,
		eTime_Fire = 3
	}

	public const double kPlayerControlledSlowMoDuration = 6.0;

	public static int[] kLevelsPerWorld = new int[3] { 12, 12, 34 };

	public static int[,] kTimeTrialTimes = new int[3, 4]
	{
		{ 540, 480, 420, 360 },
		{ 960, 900, 840, 780 },
		{ 450, 390, 330, 270 }
	};

	public static int[] kTotalRingCount = new int[3];

	public static Level smCurrentLevel = Level.eLevel_1;

	public static int smCurrentLevelRingCount = 0;

	public static float smCurrentTimeCount = 0f;

	public static bool smIsCurrentNewRingRecord = false;

	public static bool smIsCurrentNewTimeRecord = false;

	private int[] levelSeparation = new int[3] { 12, 24, 58 };

	public static int[] smMaxRingInLevel = new int[72]
	{
		34, 46, 99, 90, 115, 39, 84, 42, 120, 123,
		183, 54, 59, 75, 243, 88, 284, 135, 122, 172,
		153, 113, 203, 69, 231, 148, 115, 262, 112, 123,
		124, 123, 131, 87, 179, 131, 219, 232, 135, 148,
		325, 102, 135, 121, 153, 115, 148, 283, 284, 166,
		118, 175, 202, 161, 168, 137, 159, 227, 104, 170,
		153, 152, 172, 336, 147, 225, 63, 137, 157, 337,
		458, 116
	};

	private static GameManager m_cInstance;

	private bool m_Paused;

	private bool m_EnableTurboMode;

	private World m_CurrentWorld;

	private bool m_EnableTiming;

	private bool[] mte_unlockFlags;

	private int m_CoinsBeforeTransfer = -1;

	private bool m_DuringCutscene;

	private bool m_IsInLevel;

	private string m_HeadPhonesPlugged = "Headphones are Plugged";

	private string m_HeadPhonesUnplugged = "Headphones are not Plugged";

	private AndroidJavaObject m_Headphones;

	public static GameManager Instance
	{
		get
		{
			return m_cInstance;
		}
	}

	public bool EnableTurboMode
	{
		get
		{
			string text = null;
			text = ((CurrentWorld != World.eWorld_BlueSky) ? "TurboMode_2" : "TurboMode_1");
			if (PlayerPrefs.HasKey(text))
			{
				return PlayerPrefs.GetInt(text) == 1;
			}
			return false;
		}
		set
		{
			string text = null;
			text = ((CurrentWorld != World.eWorld_BlueSky) ? "TurboMode_2" : "TurboMode_1");
			if (value)
			{
				PlayerPrefs.SetInt(text, 1);
			}
			else
			{
				PlayerPrefs.SetInt(text, 0);
			}
		}
	}

	public World CurrentWorld
	{
		get
		{
			return m_CurrentWorld;
		}
		set
		{
			m_CurrentWorld = value;
		}
	}

	public bool EnableTiming
	{
		get
		{
			return m_EnableTiming;
		}
		set
		{
			m_EnableTiming = value;
		}
	}

	public int CoinsBeforeTransfer
	{
		get
		{
			return m_CoinsBeforeTransfer;
		}
		set
		{
			m_CoinsBeforeTransfer = value;
		}
	}

	public bool DuringCutscene
	{
		get
		{
			return m_DuringCutscene;
		}
		set
		{
			m_DuringCutscene = value;
		}
	}

	private void Awake()
	{
		m_cInstance = this;
		mte_unlockFlags = new bool[5];
		ResetUnlockFlags();
		base.gameObject.name = GetType().ToString();
		if (Application.platform == RuntimePlatform.Android && !Application.isEditor)
		{
			try
			{
				AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
				if (androidJavaObject != null)
				{
					m_Headphones = new AndroidJavaObject("com.bhvr.Headphones.HeadphoneUtils", androidJavaObject, base.gameObject.name, "HeadphoneMsg", m_HeadPhonesPlugged, m_HeadPhonesUnplugged);
				}
			}
			catch
			{
				Debug.LogWarning("GameManager: Android headphone bridge unavailable.");
			}
		}
		int i = 0;
		for (int j = 0; j < kLevelsPerWorld.Length; j++)
		{
			for (; i < levelSeparation[j]; i++)
			{
				kTotalRingCount[j] += smMaxRingInLevel[i];
			}
		}
	}

	private void Start()
	{
		m_CoinsBeforeTransfer = ProfileManager.Instance.CurrentProfile.TotalCoins;
	}

	private void Update()
	{
		CheckAndroidBackButton();
		if (!m_Paused && EnableTiming)
		{
			smCurrentTimeCount += Time.deltaTime / Time.timeScale;
		}
	}

	private void OnDestroy()
	{
	}

	public void HeadphoneMsg(string msg)
	{
		if (msg == m_HeadPhonesUnplugged && !m_Paused && m_IsInLevel)
		{
			GameFlowManager.Instance.GUIManager.ShowPauseMenu(true);
		}
	}

	public void Pause(bool aPause)
	{
		m_Paused = aPause;
		if ((bool)TimeManager.Instance)
		{
			TimeManager.Instance.Pause(aPause);
		}
	}

	public bool IsPause()
	{
		return m_Paused;
	}

	public static bool HasCollectedAllRings(World aWorld)
	{
		if (aWorld == World.eWorld_BonusWorld)
		{
			return false;
		}
		int num = 0;
		int num2 = (int)aWorld * 12;
		int num3 = (int)(aWorld + 1) * 12 - 1;
		int num4 = 0;
		Profile.LevelData[] levelData = ProfileManager.Instance.CurrentProfile.m_LevelData;
		foreach (Profile.LevelData levelData2 in levelData)
		{
			if (num4 >= num2 && num4 <= num3)
			{
				num += levelData2.BestRingCount;
			}
			num4++;
		}
		return num == kTotalRingCount[(int)aWorld];
	}

	public static int GetLevelCompletion(World aWorld)
	{
		int num = (int)aWorld * 12;
		int num2 = (int)(aWorld + 1) * 12 - 1;
		if (aWorld == World.eWorld_BonusWorld)
		{
			num2 = 59;
		}
		int num3 = 0;
		int num4 = 0;
		Profile.LevelData[] levelData = ProfileManager.Instance.CurrentProfile.m_LevelData;
		foreach (Profile.LevelData levelData2 in levelData)
		{
			if (num3 >= num && num3 <= num2 && levelData2.LevelComplete)
			{
				num4++;
			}
			num3++;
		}
		return num4;
	}

	public static float GetRingCompletion(World aWorld)
	{
		int num = (int)aWorld * 12;
		int num2 = (int)(aWorld + 1) * 12 - 1;
		int num3 = 0;
		float num4 = 0f;
		Profile.LevelData[] levelData = ProfileManager.Instance.CurrentProfile.m_LevelData;
		foreach (Profile.LevelData levelData2 in levelData)
		{
			if (num3 >= num && num3 <= num2)
			{
				num4 += (float)levelData2.BestRingCount;
			}
			num3++;
		}
		return num4 / (float)kTotalRingCount[(int)aWorld];
	}

	public static float GetTurboModeCompletion(World aWorld)
	{
		if (!Instance.HasAchievedTimeTrialFire(aWorld))
		{
			return 0f;
		}
		int num = (int)aWorld * 12;
		int num2 = (int)(aWorld + 1) * 12 - 1;
		int num3 = 0;
		float num4 = 0f;
		Profile.LevelData[] levelData = ProfileManager.Instance.CurrentProfile.m_LevelData;
		foreach (Profile.LevelData levelData2 in levelData)
		{
			if (num3 >= num && num3 <= num2)
			{
				num4 += ((!levelData2.TurboLevelComplete) ? 0f : 1f);
			}
			num3++;
		}
		return num4 / 30f;
	}

	public static float GetTimeTrialBestTime(World aWorld)
	{
		int num = (int)aWorld * 12;
		int num2 = (int)(aWorld + 1) * 12 - 1;
		int num3 = 0;
		int num4 = 0;
		Profile.LevelData[] levelData = ProfileManager.Instance.CurrentProfile.m_LevelData;
		foreach (Profile.LevelData levelData2 in levelData)
		{
			if (num3 >= num && num3 <= num2)
			{
				if (levelData2.BestTimeCount == float.MaxValue)
				{
					return 0f;
				}
				num4 += (int)levelData2.BestTimeCount;
			}
			num3++;
		}
		return num4;
	}

	public static float GetTimeTrialNewGoal(World aWorld, float aBestTimeTotal)
	{
		if (aBestTimeTotal > 0f && aBestTimeTotal <= (float)kTimeTrialTimes[(int)aWorld, 3])
		{
			return kTimeTrialTimes[(int)aWorld, 3];
		}
		if (aBestTimeTotal > 0f && aBestTimeTotal <= (float)kTimeTrialTimes[(int)aWorld, 2])
		{
			return kTimeTrialTimes[(int)aWorld, 3];
		}
		if (aBestTimeTotal > 0f && aBestTimeTotal <= (float)kTimeTrialTimes[(int)aWorld, 1])
		{
			return kTimeTrialTimes[(int)aWorld, 2];
		}
		return kTimeTrialTimes[(int)aWorld, 1];
	}

	public static void RetrieveTimeTrialCompletion(World aWorld, out LevelTimes aCompletedLevelTime, out float aCompletedPercentage)
	{
		if (!HasCollectedAllRings(aWorld))
		{
			aCompletedLevelTime = LevelTimes.eTime_None;
			aCompletedPercentage = 0f;
			return;
		}
		float timeTrialBestTime = GetTimeTrialBestTime(aWorld);
		if (timeTrialBestTime > (float)kTimeTrialTimes[(int)aWorld, 0] || timeTrialBestTime == 0f)
		{
			aCompletedLevelTime = LevelTimes.eTime_None;
			aCompletedPercentage = 0f;
			return;
		}
		if (timeTrialBestTime <= (float)kTimeTrialTimes[(int)aWorld, 3])
		{
			aCompletedLevelTime = LevelTimes.eTime_Fire;
		}
		else if (timeTrialBestTime <= (float)kTimeTrialTimes[(int)aWorld, 2])
		{
			aCompletedLevelTime = LevelTimes.eTime_Gold;
		}
		else if (timeTrialBestTime <= (float)kTimeTrialTimes[(int)aWorld, 1])
		{
			aCompletedLevelTime = LevelTimes.eTime_Silver;
		}
		else
		{
			aCompletedLevelTime = LevelTimes.eTime_None;
		}
		aCompletedPercentage = GetTimeTrialCompletedPercentage(aWorld, timeTrialBestTime, aCompletedLevelTime);
	}

	public static float GetTimeTrialCompletedPercentage(World aWorld, float aBestTimeTotal, LevelTimes aCompletedLevelTime)
	{
		if (aCompletedLevelTime == LevelTimes.eTime_Fire)
		{
			return 1f;
		}
		float num = kTimeTrialTimes[(int)aWorld, (int)aCompletedLevelTime];
		float num2 = kTimeTrialTimes[(int)aWorld, (int)(aCompletedLevelTime + 1)];
		return 1f - (aBestTimeTotal - num2) / (num - num2);
	}

	public static bool HasCompletedTurboMode(World aWorld)
	{
		if (!HasCollectedAllRings(aWorld) || !Instance.HasAchievedTimeTrialFire(aWorld))
		{
			return false;
		}
		int num = (int)aWorld * 12;
		int num2 = (int)(aWorld + 1) * 12 - 1;
		int num3 = 0;
		Profile.LevelData[] levelData = ProfileManager.Instance.CurrentProfile.m_LevelData;
		foreach (Profile.LevelData levelData2 in levelData)
		{
			if (num3 >= num && num3 <= num2 && !levelData2.TurboLevelComplete)
			{
				return false;
			}
			num3++;
		}
		return true;
	}

	public void ActivatePlayerSlowMo()
	{
		TimeManager.Instance.SlowmoOverride = true;
		TimeManager.Instance.ActivateSlowmo();
	}

	public void StopPlayerSlowMo()
	{
		TimeManager.Instance.SlowmoOverride = false;
		TimeManager.Instance.StopSlowmo();
	}

	public void StartCutscene(bool aSlowmo)
	{
		if (HasCompletedTurboMode(CurrentWorld))
		{
			StopPlayerSlowMo();
			GameFlowManager.Instance.GUIManager.HudManager.InGameHud.SetSlowmoButtonState(false);
			GameFlowManager.Instance.GUIManager.HudManager.InGameHud.SetSlowmoButtonVisible(false);
		}
		if (aSlowmo)
		{
			TimeManager.Instance.SlowmoOverride = true;
			TimeManager.Instance.ActivateSlowmo();
		}
	}

	public void EndCutscene()
	{
		if (HasCompletedTurboMode(CurrentWorld))
		{
			GameFlowManager.Instance.GUIManager.HudManager.InGameHud.SetSlowmoButtonVisible(true);
		}
		TimeManager.Instance.SlowmoOverride = false;
		TimeManager.Instance.StopSlowmo();
	}

	public void StartLevel(Level aSelectedLevel)
	{
		GameFlowManager.Instance.AudioManager.PlayMusic(AudioManager.MusicTrack.eMusic_Gameplay);
		UpdateUnlockFlags();
		smCurrentLevelRingCount = 0;
		smCurrentTimeCount = 0f;
		smCurrentLevel = aSelectedLevel;
		string empty = string.Empty;
		switch (m_CurrentWorld)
		{
		case World.eWorld_BonusWorld:
			empty = "bonus";
			break;
		case World.eWorld_SodaSunset:
			empty = "sodasunset";
			break;
		default:
			empty = "bluesky";
			break;
		}
		string aTextureName = "GUI/LoadingScreen/" + SizeCategory.Instance.Category + "/" + empty + "_loading";
		GameFlowManager.Instance.GUIManager.LoadingScreen.TextureData[0].icon.image = GUIUtil.LoadTexture2D(aTextureName);
		GameFlowManager.Instance.GUIManager.LoadingScreen.StartLoadingBar();
		if (EnableTurboMode && HasAchievedTimeTrialFire(m_CurrentWorld))
		{
			TimeManager.Instance.ActivateTurbo();
		}
		else
		{
			TimeManager.Instance.StopTurbo();
		}
		StopPlayerSlowMo();
		ProfileManager.Instance.CurrentProfile.LastLevelPlayed = (int)smCurrentLevel;
		ProfileManager.Instance.SaveCurrentProfile();
		m_IsInLevel = true;
	}

	private void ResetUnlockFlags()
	{
		for (int i = 0; i < 5; i++)
		{
			mte_unlockFlags[i] = false;
		}
	}

	private void UpdateUnlockFlags()
	{
		for (int i = 0; i < 5; i++)
		{
			mte_unlockFlags[i] = CheckUnlock((Unlock)i);
		}
	}

	private void CheckAndroidBackButton()
	{
		if (!GameFlowManager.Instance.m_DoWindowBack)
		{
			return;
		}
		if (m_IsInLevel)
		{
			if (GameFlowManager.Instance.GUIManager.IsPauseMenu)
			{
				GameFlowManager.Instance.GUIManager.ShowPauseMenu(false);
				GameFlowManager.Instance.m_DoWindowBack = false;
			}
			else if (!m_Paused && TutorialPopup.Instance == null)
			{
				GameFlowManager.Instance.GUIManager.ShowPauseMenu(true);
				GameFlowManager.Instance.m_DoWindowBack = false;
			}
		}
		else if (GameFlowManager.Instance.GUIManager.IsLoginPopupShowing || GameFlowManager.Instance.GUIManager.IsCreateAccountPopupShowing)
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
			GameFlowManager.Instance.m_DoWindowBack = false;
		}
	}

	public Unlock FindNextUnlock(Unlock ae_Unlock)
	{
		for (int i = (int)(ae_Unlock + 1); i < 5; i++)
		{
			if (i >= 0 && !mte_unlockFlags[i] && CheckUnlock((Unlock)i))
			{
				return (Unlock)i;
			}
		}
		return Unlock.eUnlock_None;
	}

	private bool CheckUnlock(Unlock ae_unlock)
	{
		switch (ae_unlock)
		{
		case Unlock.eUnlock_SlowMotion:
			return HasCompletedTurboMode(m_CurrentWorld);
		case Unlock.eUnlock_TurboMode:
			return HasAchievedTimeTrialFire(m_CurrentWorld);
		case Unlock.eUnlock_TimeTrial:
			return HasCollectedAllRings(m_CurrentWorld);
		case Unlock.eUnlock_TimeTrialSilver:
			return HasAchievedTimeTrialSilver(m_CurrentWorld);
		case Unlock.eUnlock_TimeTrialGold:
			return HasAchievedTimeTrialGold(m_CurrentWorld);
		default:
			return false;
		}
	}

	public bool HasAchievedTimeTrialSilver(World aWorld)
	{
		float timeTrialBestTime = GetTimeTrialBestTime(aWorld);
		return timeTrialBestTime > 0f && (int)timeTrialBestTime <= kTimeTrialTimes[(int)aWorld, 1] && HasCollectedAllRings(m_CurrentWorld);
	}

	public bool HasAchievedTimeTrialGold(World aWorld)
	{
		float timeTrialBestTime = GetTimeTrialBestTime(aWorld);
		return timeTrialBestTime > 0f && (int)timeTrialBestTime <= kTimeTrialTimes[(int)aWorld, 2] && HasCollectedAllRings(m_CurrentWorld);
	}

	public bool HasAchievedTimeTrialFire(World aWorld)
	{
		float timeTrialBestTime = GetTimeTrialBestTime(aWorld);
		return timeTrialBestTime > 0f && (int)timeTrialBestTime <= kTimeTrialTimes[(int)aWorld, 3] && HasCollectedAllRings(m_CurrentWorld);
	}

	public void ShowEndLevelScreens()
	{
		Instance.CompleteLevel();
		if (Instance.FindNextUnlock(Unlock.eUnlock_None) != Unlock.eUnlock_None)
		{
			GameFlowManager.Instance.GUIManager.ShowUnlockPopups(true);
		}
		else
		{
			GameFlowManager.Instance.GUIManager.ShowTallyMenu(true);
		}
	}

	public void QuitLevel()
	{
		GameFlowManager.Instance.InputController.enabled = true;
		CommonEndLevel(false);
	}

	public void CompleteLevel()
	{
		smIsCurrentNewRingRecord = false;
		smIsCurrentNewTimeRecord = false;
		ProfileManager.Instance.CurrentProfile.m_LevelData[(int)smCurrentLevel].LevelComplete = true;
		if (smCurrentLevel != Level.eLevel_60)
		{
			ProfileManager.Instance.CurrentProfile.m_LevelData[(int)(smCurrentLevel + 1)].LevelUnlocked = true;
		}
		if (IsNewRingRecord(smCurrentLevel, smCurrentLevelRingCount))
		{
			ProfileManager.Instance.CurrentProfile.m_LevelData[(int)smCurrentLevel].BestRingCount = smCurrentLevelRingCount;
			smIsCurrentNewRingRecord = true;
		}
		if (IsNewTimeRecord(smCurrentLevel, smCurrentTimeCount))
		{
			ProfileManager.Instance.CurrentProfile.m_LevelData[(int)smCurrentLevel].BestTimeCount = (int)smCurrentTimeCount;
			smIsCurrentNewTimeRecord = true;
		}
		if (EnableTurboMode && HasAchievedTimeTrialFire(m_CurrentWorld))
		{
			ProfileManager.Instance.CurrentProfile.m_LevelData[(int)smCurrentLevel].TurboLevelComplete = true;
		}
		ProfileManager.Instance.CurrentProfile.TotalCoins += smCurrentLevelRingCount;
		ProfileManager.Instance.SaveCurrentProfile();
		if (NetManager.Instance.IsPlayerLoggedIn())
		{
			NetManager.Instance.TransferCoins(ProfileManager.Instance.CurrentProfile.TotalCoins, TransferCallback, true);
		}
		else
		{
			m_CoinsBeforeTransfer = ProfileManager.Instance.CurrentProfile.TotalCoins;
		}
		CommonEndLevel(true);
	}

	private void TransferCallback(bool aSuccess)
	{
	}

	private void CommonEndLevel(bool aLevelComplete)
	{
		m_IsInLevel = false;
		EnableTiming = false;
		TimeManager.Instance.SlowmoOverride = false;
		TimeManager.Instance.StopSlowmo();
		BizIntel.ContextualEvent contextualEvent = new BizIntel.ContextualEvent("play-level");
		contextualEvent.AddContextItem("level-id", (int)smCurrentLevel);
		contextualEvent.AddContextItem("elapsed-time", (int)Time.timeSinceLevelLoad);
		contextualEvent.AddContextItem("coins-collected", smCurrentLevelRingCount);
		contextualEvent.AddContextItem("max-coins", smMaxRingInLevel[(int)smCurrentLevel]);
		contextualEvent.AddContextItem("level-passed", aLevelComplete);
		contextualEvent.AddContextItem("number-deaths", Puffle.Instance.respawnCount);
		contextualEvent.Log();
		GameFlowManager.Instance.GUIManager.LoadingScreen.TextureData[0].icon.image = GUIUtil.LoadTexture2D("GUI/LoadingScreen/BlackScreen");
		Resources.UnloadUnusedAssets();
	}

	public static bool IsNewRingRecord(Level aLevel, int aRingCount)
	{
		return aRingCount > ProfileManager.Instance.CurrentProfile.m_LevelData[(int)aLevel].BestRingCount;
	}

	public static bool IsNewTimeRecord(Level aLevel, double aTimeCount)
	{
		return aTimeCount < (double)ProfileManager.Instance.CurrentProfile.m_LevelData[(int)aLevel].BestTimeCount;
	}

	public static void CollectAllRings(World aWorld)
	{
		if (aWorld != World.eWorld_BonusWorld)
		{
			int num = (int)aWorld * 12;
			int num2 = (int)(aWorld + 1) * 12 - 1;
			int num3 = 0;
			Profile.LevelData[] levelData = ProfileManager.Instance.CurrentProfile.m_LevelData;
			foreach (Profile.LevelData levelData2 in levelData)
			{
				if (num3 >= num && num3 <= num2)
				{
					levelData2.LevelComplete = true;
					levelData2.LevelUnlocked = true;
					levelData2.BestRingCount = smMaxRingInLevel[num3];
				}
				if (Instance.CurrentWorld == World.eWorld_BlueSky && num3 == num2 + 1)
				{
					levelData2.LevelUnlocked = true;
				}
				num3++;
			}
			return;
		}
		int num4 = 24;
		int num5 = 59;
		int num6 = 0;
		Profile.LevelData[] levelData3 = ProfileManager.Instance.CurrentProfile.m_LevelData;
		foreach (Profile.LevelData levelData4 in levelData3)
		{
			if (num6 >= num4 && num6 <= num5)
			{
				levelData4.LevelComplete = true;
				levelData4.LevelUnlocked = true;
				levelData4.BestRingCount = smMaxRingInLevel[num6];
			}
			num6++;
		}
	}

	public static void CompleteTimeTrial(World aWorld, float aLevelTime)
	{
		CollectAllRings(aWorld);
		int num = (int)aWorld * 12;
		int num2 = (int)(aWorld + 1) * 12 - 1;
		int num3 = 0;
		Profile.LevelData[] levelData = ProfileManager.Instance.CurrentProfile.m_LevelData;
		foreach (Profile.LevelData levelData2 in levelData)
		{
			if (num3 >= num && num3 <= num2)
			{
				levelData2.BestTimeCount = aLevelTime;
			}
			num3++;
		}
	}

	public static void CompleteTurboMode(World aWorld)
	{
		CompleteTimeTrial(aWorld, 20f);
		int num = (int)aWorld * 12;
		int num2 = (int)(aWorld + 1) * 12 - 1;
		int num3 = 0;
		Profile.LevelData[] levelData = ProfileManager.Instance.CurrentProfile.m_LevelData;
		foreach (Profile.LevelData levelData2 in levelData)
		{
			if (num3 >= num && num3 <= num2)
			{
				levelData2.TurboLevelComplete = true;
			}
			num3++;
		}
	}

	public static string GetTimeFormatedString(float aSeconds)
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds(aSeconds);
		return string.Format("{0}:{1:D2}", (int)timeSpan.TotalMinutes, timeSpan.Seconds);
	}
}
