using UnityEngine;

public class ResourceLoader
{
	public static string[] kLocalizedSuffixs = new string[6]
	{
		string.Empty,
		"_fr",
		"_es",
		"_pt",
		"_de",
		"_ja"
	};

	private static ResourceLoader m_cInstance = null;

	public static ResourceLoader Instance
	{
		get
		{
			if (m_cInstance == null)
			{
				m_cInstance = new ResourceLoader();
			}
			return m_cInstance;
		}
		set
		{
			if (Utilities.AssertMsg(value == null, "Cannot assign anything else but NULL to singleton instance!"))
			{
				m_cInstance = null;
			}
		}
	}

	private ResourceLoader()
	{
	}

	public void SetMaterialTexture(GameObject aGameObj, string aTexturePath, bool aLocalized, out SizeCategory.CategoryId loadedAssetSizeCategoryId, out LocalizationManager.Language loadedAssetLanguage)
	{
		loadedAssetSizeCategoryId = SizeCategory.CategoryId.eUnknown;
		loadedAssetLanguage = LocalizationManager.Language.eEnglish;
		if (SizeCategory.Instance == null)
		{
			return;
		}
		if (aTexturePath == null || aTexturePath.Length <= 0)
		{
			Utilities.AssertMsg(false, "Invalid texturePath: " + aTexturePath);
			return;
		}
		MeshRenderer renderer = GetRenderer(aGameObj);
		if (renderer == null || renderer.sharedMaterial == null)
		{
			return;
		}
		SizeCategory.CategoryId categoryId = SizeCategory.Instance.CurCategoryId;
		Texture2D texture2D = null;
		while (texture2D == null)
		{
			string text = aTexturePath + SizeCategory.Instance.GetCategory(categoryId) + "/" + GetResourceSafeMaterialName(renderer.sharedMaterial.name);
			texture2D = GUIUtil.LoadTexture2D(text);
			if (texture2D == null)
			{
				SizeCategory.CategoryId alternateCategoryId = SizeCategory.Instance.GetAlternateCategoryId(categoryId);
				if (alternateCategoryId == categoryId)
				{
					Utilities.AssertMsg(false, "No valid texture is found! Fail to set material texture of game object: " + aGameObj);
					break;
				}
				categoryId = alternateCategoryId;
				continue;
			}
			if (aLocalized)
			{
				string languageCode = LocalizationManager.GetLanguageCode();
				string path = text + languageCode;
				Texture2D texture2D2 = GUIUtil.LoadTexture2D(path);
				if (texture2D2 != null)
				{
					texture2D = texture2D2;
					loadedAssetLanguage = GetLanguageByCode(languageCode);
				}
			}
			loadedAssetSizeCategoryId = categoryId;
			renderer.sharedMaterial.mainTexture = texture2D;
		}
	}

	public void ResetMaterialTexture(GameObject aGameObj)
	{
		MeshRenderer renderer = GetRenderer(aGameObj);
		if (!(renderer == null) && !(renderer.sharedMaterial == null))
		{
			renderer.sharedMaterial.mainTexture = null;
		}
	}

	public MeshRenderer GetRenderer(GameObject aGameObj)
	{
		if (aGameObj == null)
		{
			Utilities.AssertMsg(false, "Fail to get renderer due to invalid given game object!");
			return null;
		}
		MeshRenderer component = aGameObj.GetComponent<MeshRenderer>();
		Utilities.AssertMsg(component != null, "No MeshRenderer found in " + aGameObj);
		Utilities.AssertMsg(component.sharedMaterial != null, "No valid shared material in renderer of " + aGameObj);
		return component;
	}

	public static string GetLocalizedSuffixByLanguage(LocalizationManager.Language lang)
	{
		if (lang < LocalizationManager.Language.eEnglish || (int)lang >= kLocalizedSuffixs.Length)
		{
			Utilities.AssertMsg(false, "Invalid language: " + lang);
			return string.Empty;
		}
		return kLocalizedSuffixs[(int)lang];
	}

	private static string GetResourceSafeMaterialName(string aMaterialName)
	{
		if (string.IsNullOrEmpty(aMaterialName))
		{
			return string.Empty;
		}
		int num = aMaterialName.IndexOf(" (");
		if (num >= 0)
		{
			return aMaterialName.Substring(0, num);
		}
		return aMaterialName;
	}

	private static LocalizationManager.Language GetLanguageByCode(string aLanguageCode)
	{
		switch (aLanguageCode)
		{
		case "fr":
			return LocalizationManager.Language.eFrench;
		case "es":
			return LocalizationManager.Language.eSpanish;
		case "pt":
			return LocalizationManager.Language.ePortuguese;
		case "de":
			return LocalizationManager.Language.eGerman;
		case "ja":
			return LocalizationManager.Language.eJanpanies;
		default:
			return LocalizationManager.Language.eEnglish;
		}
	}
}
