using UnityEngine;

public class UIControlExtension : MonoBehaviour
{
	public string texturePath;

	public string m_TextId = string.Empty;

	public string m_Text = string.Empty;

	protected SizeCategory.CategoryId m_AssetSizeCategoryId = SizeCategory.CategoryId.eUnknown;

	protected LocalizationManager.Language m_AssetLanguage;

	public SizeCategory.CategoryId AssetSizeCategoryId
	{
		get
		{
			return m_AssetSizeCategoryId;
		}
	}

	public LocalizationManager.Language AssetLanguage
	{
		get
		{
			return m_AssetLanguage;
		}
	}

	public virtual void Awake()
	{
	}

	public virtual void OnDestroy()
	{
	}

	public virtual void SetMaterialTexture()
	{
		if (texturePath != null && texturePath.Length > 0)
		{
			ResourceLoader.Instance.SetMaterialTexture(base.gameObject, texturePath, false, out m_AssetSizeCategoryId, out m_AssetLanguage);
		}
	}

	public virtual void SetMaterialLocalizedTexture(bool aLocalized)
	{
		if (texturePath != null && texturePath.Length > 0)
		{
			ResourceLoader.Instance.SetMaterialTexture(base.gameObject, texturePath, aLocalized, out m_AssetSizeCategoryId, out m_AssetLanguage);
		}
	}

	public virtual string GetLocalizeText()
	{
		if (m_TextId != null && m_TextId.Length > 0)
		{
			if (GameFlowManager.Instance != null && LocalizationManager.Instance != null)
			{
				m_Text = LocalizationManager.Instance.GetString(m_TextId);
			}
			else
			{
				m_Text = m_TextId;
			}
		}
		return m_Text;
	}
}
