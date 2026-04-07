using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class LocalizationManager
{
	public enum Language
	{
		eEnglish = 0,
		eFrench = 1,
		eSpanish = 2,
		ePortuguese = 3,
		eGerman = 4,
		eJanpanies = 5,
		eLanguage_COUNT = 6
	}

	public const string kEnglishLocale = "en";

	public const string kFrenchLocale = "fr";

	public const string kSpanishLocale = "es";

	public const string kPortugueseLocale = "pt";

	public const string kGermanLocale = "de";

	public const string kJapaneseLocale = "ja";

	public const string kArgentinaRegion = "es_AR";

	public const string kUnknownRegion = "";

	public byte[] kUTF8ByteOrederMarks = new byte[3] { 239, 187, 191 };

	private static LocalizationManager m_cInstance;

	private Dictionary<string, string> m_TextDict;

	private Dictionary<string, string> m_TermsOfUseTextDict;

	private string m_FilePathPrefix = "Text/LocalizedText_";

	private string m_TermsOfUseFilePathPrefix = "Text/tou_clubpios_";

	public static LocalizationManager Instance
	{
		get
		{
			if (m_cInstance == null)
			{
				m_cInstance = new LocalizationManager();
				m_cInstance.LoadData();
			}
			return m_cInstance;
		}
	}

	public static bool IsFrench
	{
		get
		{
			return GetLanguageCode() == "fr";
		}
	}

	public static bool IsPortuguese
	{
		get
		{
			return GetLanguageCode() == "pt";
		}
	}

	public static bool IsSpanish
	{
		get
		{
			return GetLanguageCode() == "es";
		}
	}

	public static bool IsEnglish
	{
		get
		{
			return GetLanguageCode() == "en";
		}
	}

	public static bool IsGerman
	{
		get
		{
			return GetLanguageCode() == "de";
		}
	}

	public static bool IsJapanese
	{
		get
		{
			return GetLanguageCode() == "ja";
		}
	}

	public static string GetLanguageCode()
	{
		switch (Application.systemLanguage)
		{
		case SystemLanguage.French:
			return "fr";
		case SystemLanguage.Spanish:
			return "es";
		case SystemLanguage.Portuguese:
			return "pt";
		case SystemLanguage.German:
			return "de";
		default:
			return "en";
		}
	}

	public static string GetRegionCode()
	{
		if (Application.platform != RuntimePlatform.Android || Application.isEditor)
		{
			return string.Empty;
		}
		try
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("java.util.Locale");
			AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getDefault", new object[0]);
			switch (androidJavaObject.Call<string>("getCountry", new object[0]))
			{
			case "AR":
				return "es_AR";
			default:
				return string.Empty;
			}
		}
		catch
		{
			return string.Empty;
		}
	}

	public void LoadData()
	{
		m_TextDict = new Dictionary<string, string>();
		TextAsset textAsset = LoadTextResource(m_FilePathPrefix + GetLanguageCode());
		if (textAsset != null)
		{
			StringReader stringReader = new StringReader(textAsset.text);
			bool flag = HasUTF8BOM(textAsset.bytes);
			int num = 0;
			string text = stringReader.ReadLine();
			while (text != null)
			{
				int num2 = text.IndexOf('\t');
				if (num2 != -1)
				{
					string text2 = text.Substring(0, num2);
					string text3 = text.Substring(num2 + 1);
					if (flag && num == 0)
					{
						text2 = RemoveUTF8BOM(text2);
					}
					m_TextDict[text2] = text3.Replace("\\n", "\n");
				}
				text = stringReader.ReadLine();
				num++;
			}
		}
		m_TermsOfUseTextDict = new Dictionary<string, string>();
		textAsset = LoadTextResource(m_TermsOfUseFilePathPrefix + GetLanguageCode());
		if (textAsset != null)
		{
			StringReader stringReader2 = new StringReader(textAsset.text);
			for (string text4 = stringReader2.ReadLine(); text4 != null; text4 = stringReader2.ReadLine())
			{
				int num3 = text4.IndexOf('\t');
				if (num3 != -1)
				{
					string key = text4.Substring(0, num3);
					string text5 = text4.Substring(num3 + 1);
					m_TermsOfUseTextDict[key] = text5.Replace("\\n", "\n");
				}
			}
		}
		m_cInstance = this;
	}

	public bool HasUTF8BOM(byte[] aBytes)
	{
		int num = kUTF8ByteOrederMarks.Length;
		if (aBytes.Length < num)
		{
			return false;
		}
		for (int i = 0; i < num; i++)
		{
			if (aBytes[i] != kUTF8ByteOrederMarks[i])
			{
				return false;
			}
		}
		return true;
	}

	private TextAsset LoadTextResource(string aResourcePath)
	{
		TextAsset textAsset = Resources.Load(aResourcePath, typeof(TextAsset)) as TextAsset;
		if (textAsset == null)
		{
			textAsset = Resources.Load(GUIUtil.NormalizeResourcePath(aResourcePath), typeof(TextAsset)) as TextAsset;
		}
		return textAsset;
	}

	public string RemoveUTF8BOM(string aString)
	{
		char[] array = aString.ToCharArray();
		StringBuilder stringBuilder = new StringBuilder(string.Empty);
		char[] array2 = array;
		foreach (char c in array2)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(new char[1] { c });
			if (bytes.Length > 2)
			{
				stringBuilder.Append("?");
			}
			else
			{
				stringBuilder.Append(c);
			}
		}
		string text = stringBuilder.ToString().Trim();
		return text.TrimStart('?');
	}

	public string GetTOUString(string aTextId)
	{
		if (m_TermsOfUseTextDict.ContainsKey(aTextId))
		{
			return m_TermsOfUseTextDict[aTextId];
		}
		return aTextId;
	}

	public string GetString(string aTextId)
	{
		if (m_TextDict.ContainsKey(aTextId))
		{
			return m_TextDict[aTextId];
		}
		return aTextId;
	}

	public string GetString(string aTextId, object aObject)
	{
		string text = GetString(aTextId);
		if (text != aTextId && aObject != null)
		{
			text = string.Format(text, aObject);
		}
		return text;
	}

	public string GetString(string aTextId, object aObject1, object aObject2)
	{
		string text = GetString(aTextId);
		if (text != aTextId && aObject1 != null && aObject2 != null)
		{
			text = string.Format(text, aObject1, aObject2);
		}
		return text;
	}
}
