using UnityEngine;

public class DeviceSpecificManager
{
	private uint m_TileSize = 1024u;

	private string m_AnimationDataPath;

	private string m_DataPath;

	private bool m_DownloadInGameplay = true;

	private static DeviceSpecificManager s_Instance;

	private bool m_IsBluetoothSupported;

	public DeviceSpecificManager()
	{
		Debug.Log("DeviceSpecificManager\n");
		switch (Application.platform)
		{
		case RuntimePlatform.IPhonePlayer:
			m_DataPath = Application.dataPath + "/Raw/";
			break;
		case RuntimePlatform.OSXEditor:
		case RuntimePlatform.OSXPlayer:
		case RuntimePlatform.WindowsPlayer:
		case RuntimePlatform.WindowsEditor:
			m_DataPath = Application.dataPath + "/StreamingAssets_PC/";
			break;
		default:
			m_DataPath = Application.dataPath + "/";
			break;
		}
		m_AnimationDataPath = m_DataPath;
		m_TileSize = 1024u;
	}

	private static DeviceSpecificManager get()
	{
		if (s_Instance == null)
		{
			s_Instance = new DeviceSpecificManager();
		}
		return s_Instance;
	}

	public static string GetBasePath()
	{
		return get().m_DataPath;
	}

	public static string GetAnimationBasePath()
	{
		return get().m_AnimationDataPath;
	}

	public static uint GetTileSize()
	{
		return get().m_TileSize;
	}

	public static bool CanDownloadInGameplay()
	{
		return get().m_DownloadInGameplay;
	}

	public static bool IsBluetoothSupported()
	{
		return get().m_IsBluetoothSupported;
	}
}
