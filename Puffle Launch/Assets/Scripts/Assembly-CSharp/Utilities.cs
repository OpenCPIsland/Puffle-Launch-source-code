using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityDebug = UnityEngine.Debug;

public static class Utilities
{
	public enum PenguinColors
	{
		eDefaultBlue = 0,
		eBlue = 1,
		eGreen = 2,
		ePink = 3,
		eBlack = 4,
		eRed = 5,
		eOrange = 6,
		eYellowMustard = 7,
		eDarkPurple = 8,
		eBrown = 9,
		ePeach = 10,
		eDarkGreen = 11,
		eLightBlue = 12,
		eLimeGreen = 13,
		eGray = 14,
		eAqua = 15,
		ePenguinColor_COUNT = 16
	}

	private static int m_cCurrentBuildVersion = 13;

	public static float m_cTargetWidthIphone = 960f;

	public static float m_cTargetHeightIphone = 640f;

	public static float m_cTargetWidth = 1024f;

	public static float m_cTargetHeight = 768f;

	public static Color[] m_cPenguinColors = new Color[16]
	{
		new Color(0f, 0.2f, 0.4f),
		new Color(0f, 0.2f, 0.4f),
		new Color(0f, 0.6f, 0f),
		new Color(1f, 0.2f, 0.6f),
		new Color(0.2f, 0.2f, 0.2f),
		new Color(0.8f, 0f, 0f),
		new Color(1f, 0.4f, 0f),
		new Color(1f, 0.8f, 0f),
		new Color(0.4f, 0f, 0.6f),
		new Color(0.6f, 0.4f, 0f),
		new Color(1f, 0.4f, 0.4f),
		new Color(0f, 0.4f, 0f),
		new Color(0f, 0.6f, 0.8f),
		new Color(46f / 85f, 0.8901961f, 0.007843138f),
		new Color(49f / 85f, 32f / 51f, 0.6431373f),
		new Color(0.007843138f, 0.654902f, 0.5921569f)
	};

	public static Color[] m_cPenguinHightlightColors = new Color[16]
	{
		new Color(0.12156863f, 0.4f, 0.6156863f),
		new Color(0.12156863f, 0.4f, 0.6156863f),
		new Color(8f / 51f, 0.75686276f, 8f / 51f),
		new Color(1f, 22f / 51f, 71f / 85f),
		new Color(1f / 3f, 1f / 3f, 1f / 3f),
		new Color(0.95686275f, 8f / 51f, 8f / 51f),
		new Color(1f, 47f / 85f, 8f / 51f),
		new Color(1f, 0.95686275f, 8f / 51f),
		new Color(0.5568628f, 0.16078432f, 0.7607843f),
		new Color(0.75686276f, 47f / 85f, 8f / 51f),
		new Color(1f, 47f / 85f, 47f / 85f),
		new Color(8f / 51f, 47f / 85f, 8f / 51f),
		new Color(8f / 51f, 0.75686276f, 0.95686275f),
		new Color(26f / 51f, 1f, 14f / 85f),
		new Color(0.49803922f, 28f / 51f, 48f / 85f),
		new Color(0.24313726f, 0.7921569f, 11f / 15f)
	};

	public static Color[] m_cPenguinShadowColors = new Color[16]
	{
		new Color(0f, 0f, 0.2f),
		new Color(0f, 0f, 0.2f),
		new Color(0f, 0.40392157f, 0f),
		new Color(41f / 51f, 0f, 0.40392157f),
		new Color(8f / 51f, 8f / 51f, 8f / 51f),
		new Color(0.6039216f, 0f, 0f),
		new Color(41f / 51f, 0.2f, 0f),
		new Color(41f / 51f, 0.6039216f, 0f),
		new Color(0.2f, 0f, 0.4f),
		new Color(0.40392157f, 0.2f, 0f),
		new Color(41f / 51f, 0.2f, 0.2f),
		new Color(0f, 0.2f, 0f),
		new Color(0f, 37f / 85f, 0.64705884f),
		new Color(0.4f, 11f / 15f, 0f),
		new Color(0.654902f, 0.7058824f, 0.72156864f),
		new Color(0f, 43f / 85f, 0.4862745f)
	};

	public static int CurrentBuildNumber
	{
		get
		{
			return m_cCurrentBuildVersion;
		}
	}

	public static string CurrentBuildString
	{
		get
		{
			if (Application.platform == RuntimePlatform.Android && !Application.isEditor)
			{
				try
				{
					AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
					AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
					if (androidJavaObject != null)
					{
						AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getPackageManager", new object[0]);
						string text = androidJavaObject.Call<string>("getPackageName", new object[0]);
						AndroidJavaObject androidJavaObject3 = androidJavaObject2.Call<AndroidJavaObject>("getPackageInfo", new object[2] { text, 0 });
						return androidJavaObject3.Get<string>("versionName");
					}
				}
				catch
				{
				}
			}
			if (!string.IsNullOrEmpty(Application.version))
			{
				return Application.version;
			}
			return m_cCurrentBuildVersion.ToString();
		}
	}

	public static float AspectRatio
	{
		get
		{
			return (float)Screen.width / (float)Screen.height;
		}
	}

	public static float ReferenceAspectRatio
	{
		get
		{
			return GUIConstants.kReferenceScreenWidth / GUIConstants.kReferenceScreenHeight;
		}
	}

	public static bool RectHitTest(Vector3 aHitPosition, Rect aRect)
	{
		if (aHitPosition.x >= aRect.x && aHitPosition.x <= aRect.x + aRect.width && aHitPosition.y >= aRect.y && aHitPosition.y <= aRect.y + aRect.height)
		{
			return true;
		}
		return false;
	}

	public static bool Assert(bool aCondition)
	{
		return true;
	}

	public static bool AssertMsg(bool aCondition, string aMsg)
	{
		return true;
	}

	public static bool AssertMsgCritical(bool aCondition, string aMsg)
	{
		return true;
	}

	private static bool AssertMsgHelper(bool aCondition, string aMsg, StackFrame aStackFrame, bool aIsCritical)
	{
		return true;
	}

	private static string FilepathTrimmed(string aFilename)
	{
		string text = "\\Unity\\";
		int num = aFilename.IndexOf(text);
		num += text.Length;
		return aFilename.Substring(num);
	}

	public static int RandomRange(int aLow, int aHigh)
	{
		return Random.Range(aLow, aHigh);
	}

	public static bool IsFloatEqual(float aLHS, float aRHS)
	{
		return IsFloatEqual(aLHS, aRHS, 0.01f);
	}

	public static bool IsFloatEqual(float aLHS, float aRHS, float epsilon)
	{
		float num = Mathf.Abs(aRHS - aLHS);
		return num <= epsilon;
	}

	public static void CreateFolderPath(string aCurrentPath)
	{
		if (!Directory.Exists(aCurrentPath))
		{
			CreateFolderPath(aCurrentPath.Remove(aCurrentPath.LastIndexOf("/")));
			Directory.CreateDirectory(aCurrentPath);
			UnityDebug.Log("Utilities::CreateFolderPath - Created folder path: " + aCurrentPath);
		}
	}

	public static bool ArrayContains(string[] aArray, string aElem)
	{
		foreach (string text in aArray)
		{
			if (text == aElem)
			{
				return true;
			}
		}
		return false;
	}

	[DllImport("Texture")]
	public static extern string getSupportedTextureFormat();

	public static void GetSupportedTextureFormats()
	{
		string supportedTextureFormat = getSupportedTextureFormat();
		if (supportedTextureFormat.Contains("GL_AMD_compressed_ATC_texture"))
		{
			UnityDebug.Log("GATES --- This is a Qualcom ATI Device");
		}
		if (supportedTextureFormat.Contains("EXT_texture_compression_dxt1"))
		{
			UnityDebug.Log("GATES --- This is a NVIDIA Tegra Device");
		}
		if (supportedTextureFormat.Contains("GL_IMG_texture_compression_pvrtc"))
		{
			UnityDebug.Log("GATES --- This is a TI PowerVR Device");
		}
	}
}
