using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ProfileManager : MonoBehaviour
{
	private class FormatterBinder : SerializationBinder
	{
		public override Type BindToType(string aAssemblyName, string aTypeName)
		{
			if (Profile.IsValidForDeserialization(aAssemblyName, aTypeName))
			{
				Profile.ExtractDeserializedVersionIndex(aAssemblyName);
				return typeof(Profile);
			}
			return null;
		}
	}

	public enum Result
	{
		eSucceeded = 0,
		eUserNameEmpty = 1,
		eUserNameConflict = 2,
		eExceedMaxSlots = 3,
		eCOUNT = 4
	}

	private Profile m_CurrentProfile;

	public List<Profile> m_Profiles;

	private static ProfileManager mInstance;

	public static ProfileManager Instance
	{
		get
		{
			return mInstance;
		}
	}

	public Profile CurrentProfile
	{
		get
		{
			return m_CurrentProfile;
		}
	}

	public int CurrentProfileID
	{
		get
		{
			return m_CurrentProfile.m_ProfileID;
		}
		set
		{
			m_CurrentProfile.m_ProfileID = value;
		}
	}

	public void SerializeProfile(string aFilename, Profile aProfileToSerialize)
	{
		Stream stream = File.Open(aFilename, FileMode.Create);
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		binaryFormatter.Serialize(stream, aProfileToSerialize);
		stream.Close();
	}

	public Profile DeSerializeProfile(string aFilename, ref bool aSucceeded)
	{
		aSucceeded = true;
		Profile result = Profile.CreateProfile();
		Stream stream = File.Open(aFilename, FileMode.Open);
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		binaryFormatter.Binder = new FormatterBinder();
		try
		{
			result = (Profile)binaryFormatter.Deserialize(stream);
		}
		catch
		{
			aSucceeded = false;
		}
		stream.Close();
		return result;
	}

	public void SaveCurrentProfile()
	{
		string profileFilePath = GetProfileFilePath(CurrentProfileID);
		SerializeProfile(profileFilePath, CurrentProfile);
	}

	public bool LoadProfile(int aProfileID, ref Profile aProfileToLoad)
	{
		string profileFilePath = GetProfileFilePath(aProfileID);
		if (File.Exists(profileFilePath))
		{
			bool aSucceeded = false;
			aProfileToLoad = DeSerializeProfile(profileFilePath, ref aSucceeded);
			aProfileToLoad.m_ProfileID = aProfileID;
			if (!aSucceeded)
			{
				RemoveProfile(aProfileID);
			}
			return aSucceeded;
		}
		return false;
	}

	public void UnLoadAllProfiles()
	{
		m_Profiles.Clear();
	}

	public void SetCurrentProfile(Profile aNewProfile)
	{
		m_CurrentProfile = aNewProfile;
		CurrentProfileID = m_CurrentProfile.m_ProfileID;
	}

	public Result ValidateProfileName(string aName)
	{
		if (aName == null || aName.Length == 0)
		{
			return Result.eUserNameEmpty;
		}
		return Result.eSucceeded;
	}

	public Result CreateNewCurrentProfile()
	{
		if (!File.Exists(GetProfileFilePath(0)))
		{
			m_CurrentProfile = Profile.CreateProfile();
			SaveCurrentProfile();
			return Result.eSucceeded;
		}
		return Result.eExceedMaxSlots;
	}

	public void RemoveProfile(int aProfileID)
	{
		string profileFilePath = GetProfileFilePath(aProfileID);
		if (File.Exists(profileFilePath))
		{
			File.Delete(profileFilePath);
		}
	}

	public string GetProfileFilePath(int aProfileID)
	{
		return Path.Combine(Application.persistentDataPath, "Profile" + aProfileID + ".dat");
	}

	public bool DoesProfileExist(int aProfileID)
	{
		return File.Exists(GetProfileFilePath(aProfileID));
	}

	public bool DoesCurrentProfileExist()
	{
		return DoesProfileExist(CurrentProfileID);
	}

	private void Awake()
	{
		mInstance = this;
		m_CurrentProfile = Profile.CreateProfile();
		LoadProfile(CurrentProfileID, ref m_CurrentProfile);
		if (PlayerPrefs.GetInt("ClearedData", 0) == 0)
		{
			if (CurrentProfile.BuildVersion >= Utilities.CurrentBuildNumber)
			{
				RemoveProfile(0);
				CreateNewCurrentProfile();
				PlayerPrefs.DeleteAll();
			}
			PlayerPrefs.SetInt("ClearedData", 1);
			PlayerPrefs.Save();
		}
		CurrentProfile.m_LevelData[0].LevelUnlocked = true;
		for (int i = 1; i <= 23; i++)
		{
			if (!CurrentProfile.m_LevelData[i].LevelUnlocked && CurrentProfile.m_LevelData[i - 1].LevelComplete)
			{
				CurrentProfile.m_LevelData[i].LevelUnlocked = true;
				break;
			}
		}
		CurrentProfile.m_LevelData[24].LevelUnlocked = true;
		for (int j = 25; j <= 59; j++)
		{
			if (!CurrentProfile.m_LevelData[j].LevelUnlocked && CurrentProfile.m_LevelData[j - 1].LevelComplete)
			{
				CurrentProfile.m_LevelData[j].LevelUnlocked = true;
				break;
			}
		}
		CurrentProfile.BuildVersion = Utilities.CurrentBuildNumber;
		SaveCurrentProfile();
	}
}
