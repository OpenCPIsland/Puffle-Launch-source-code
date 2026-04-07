using System;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class Profile : ISerializable
{
	public class LevelData
	{
		private int mBestRingCount;

		private float mBestTimeCount;

		private bool mTurboLevelComplete;

		private bool mLevelComplete;

		private bool mLevelUnlocked;

		public int BestRingCount
		{
			get
			{
				return mBestRingCount;
			}
			set
			{
				mBestRingCount = value;
			}
		}

		public float BestTimeCount
		{
			get
			{
				return mBestTimeCount;
			}
			set
			{
				mBestTimeCount = value;
			}
		}

		public bool TurboLevelComplete
		{
			get
			{
				return mTurboLevelComplete;
			}
			set
			{
				mTurboLevelComplete = value;
			}
		}

		public bool LevelComplete
		{
			get
			{
				return mLevelComplete;
			}
			set
			{
				mLevelComplete = value;
			}
		}

		public bool LevelUnlocked
		{
			get
			{
				return mLevelUnlocked;
			}
			set
			{
				mLevelUnlocked = value;
			}
		}

		public LevelData()
		{
			mBestRingCount = 0;
			mBestTimeCount = float.MaxValue;
			mTurboLevelComplete = false;
			mLevelComplete = false;
			mLevelUnlocked = false;
		}
	}

	private const string BUILD_VERSION = "BuildVersion";

	private const string PROFILE_NAME = "ProfileName";

	private const string AUTH_TOKEN = "AuthToken";

	private const string TOTAL_COINS = "TotalCoins";

	private const string LEVEL_DATA_BEST_RING_COUNT = "LevelDataBestRingCount";

	private const string LEVEL_DATA_BEST_TIME_COUNT = "LevelDataBestTimeCount";

	private const string LEVEL_DATA_TURBO_LEVEL_COMPLETE = "LevelDataTurboLevelComplete";

	private const string LEVEL_DATA_LEVEL_COMPLETE = "LevelDataLevelComplete";

	private const string LEVEL_DATA_LEVEL_UNLOCKED = "LevelDataLevelUnlocked";

	private static int VersionIndex = 0;

	private static string[] Versions = new string[2] { "0", "0.0.1" };

	private static int DeserializedVersionIndex;

	public int m_ProfileID;

	public LevelData[] m_LevelData;

	private int m_BuildVersion;

	private string m_ProfileName;

	private string m_AuthToken = string.Empty;

	private int m_TotalCoins;

	private int m_LastLevelPlayed;

	private static string HeaderLine
	{
		get
		{
			return "Profile data at version ";
		}
	}

	private static string FullTypeName
	{
		get
		{
			return "Profile";
		}
	}

	public int BuildVersion
	{
		get
		{
			return m_BuildVersion;
		}
		set
		{
			m_BuildVersion = value;
		}
	}

	public string AuthToken
	{
		get
		{
			return m_AuthToken;
		}
		set
		{
			m_AuthToken = value;
		}
	}

	public string ProfileName
	{
		get
		{
			return m_ProfileName;
		}
		set
		{
			m_ProfileName = value;
		}
	}

	public int TotalCoins
	{
		get
		{
			return m_TotalCoins;
		}
		set
		{
			m_TotalCoins = value;
		}
	}

	public int LastLevelPlayed
	{
		get
		{
			return m_LastLevelPlayed;
		}
		set
		{
			m_LastLevelPlayed = value;
		}
	}

	public Profile()
	{
	}

	protected Profile(SerializationInfo aInfo, StreamingContext aTxt)
	{
		Init();
		if (DeserializedVersionIndex >= 0)
		{
			try
			{
				m_BuildVersion = (int)aInfo.GetValue("BuildVersion", typeof(int));
			}
			catch (SerializationException)
			{
				m_BuildVersion = 0;
			}
			try
			{
				m_ProfileName = (string)aInfo.GetValue("ProfileName", typeof(string));
			}
			catch (SerializationException)
			{
				ProfileName = "New Profile";
			}
			try
			{
				m_AuthToken = (string)aInfo.GetValue("AuthToken", typeof(string));
			}
			catch (SerializationException)
			{
				m_AuthToken = string.Empty;
			}
			try
			{
				m_TotalCoins = (int)aInfo.GetValue("TotalCoins", typeof(int));
			}
			catch (SerializationException)
			{
				m_TotalCoins = 0;
			}
			int[] array;
			try
			{
				array = (int[])aInfo.GetValue("LevelDataBestRingCount", typeof(int[]));
			}
			catch (SerializationException)
			{
				array = new int[60];
			}
			float[] array2;
			try
			{
				array2 = (float[])aInfo.GetValue("LevelDataBestTimeCount", typeof(float[]));
			}
			catch (SerializationException)
			{
				array2 = new float[60];
			}
			bool[] array3;
			try
			{
				array3 = (bool[])aInfo.GetValue("LevelDataTurboLevelComplete", typeof(bool[]));
			}
			catch (SerializationException)
			{
				array3 = new bool[60];
			}
			bool[] array4;
			try
			{
				array4 = (bool[])aInfo.GetValue("LevelDataLevelComplete", typeof(bool[]));
			}
			catch (SerializationException)
			{
				array4 = new bool[60];
			}
			bool[] array5;
			try
			{
				array5 = (bool[])aInfo.GetValue("LevelDataLevelUnlocked", typeof(bool[]));
			}
			catch (SerializationException)
			{
				array5 = new bool[60];
			}
			int num = Mathf.Min(array.Length, m_LevelData.Length);
			for (int i = 0; i < num; i++)
			{
				m_LevelData[i].BestRingCount = array[i];
				m_LevelData[i].BestTimeCount = array2[i];
				m_LevelData[i].TurboLevelComplete = array3[i];
				m_LevelData[i].LevelComplete = array4[i];
				m_LevelData[i].LevelUnlocked = array5[i];
			}
		}
	}

	private static bool IsValidHeaderLine(string aAssemblyName)
	{
		for (int i = 0; i < Versions.Length; i++)
		{
			if (aAssemblyName == HeaderLine + Versions[i])
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsValidForDeserialization(string aAssemblyName, string aTypeName)
	{
		return FullTypeName == aTypeName && IsValidHeaderLine(aAssemblyName);
	}

	public static void ExtractDeserializedVersionIndex(string aAssemblyName)
	{
		DeserializedVersionIndex = -1;
		string text = aAssemblyName.Remove(0, HeaderLine.Length);
		for (int i = 0; i < Versions.Length; i++)
		{
			if (text == Versions[i])
			{
				DeserializedVersionIndex = i;
				break;
			}
		}
		Utilities.AssertMsg(DeserializedVersionIndex != -1, "Invalid Version Number: " + text);
	}

	public static Profile CreateProfile()
	{
		Profile profile = new Profile();
		profile.Init();
		return profile;
	}

	public void GetObjectData(SerializationInfo aInfo, StreamingContext aTxt)
	{
		aInfo.AssemblyName = HeaderLine + Versions[VersionIndex];
		aInfo.FullTypeName = FullTypeName;
		if (VersionIndex >= 0)
		{
			aInfo.AddValue("BuildVersion", m_BuildVersion);
			aInfo.AddValue("ProfileName", m_ProfileName);
			aInfo.AddValue("AuthToken", m_AuthToken);
			aInfo.AddValue("TotalCoins", m_TotalCoins);
			int[] array = new int[60];
			float[] array2 = new float[60];
			bool[] array3 = new bool[60];
			bool[] array4 = new bool[60];
			bool[] array5 = new bool[60];
			for (int i = 0; i < 60; i++)
			{
				array[i] = m_LevelData[i].BestRingCount;
				array2[i] = m_LevelData[i].BestTimeCount;
				array3[i] = m_LevelData[i].TurboLevelComplete;
				array4[i] = m_LevelData[i].LevelComplete;
				array5[i] = m_LevelData[i].LevelUnlocked;
			}
			aInfo.AddValue("LevelDataBestRingCount", array);
			aInfo.AddValue("LevelDataBestTimeCount", array2);
			aInfo.AddValue("LevelDataTurboLevelComplete", array3);
			aInfo.AddValue("LevelDataLevelComplete", array4);
			aInfo.AddValue("LevelDataLevelUnlocked", array5);
		}
	}

	public bool HasAuthToken()
	{
		return m_AuthToken != null && m_AuthToken.Length > 0;
	}

	private void Init()
	{
		m_ProfileID = 0;
		m_LevelData = new LevelData[60];
		for (int i = 0; i < 60; i++)
		{
			m_LevelData[i] = new LevelData();
		}
		m_TotalCoins = 0;
		m_LastLevelPlayed = -1;
	}
}
