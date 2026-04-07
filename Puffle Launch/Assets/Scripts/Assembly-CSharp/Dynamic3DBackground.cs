using UnityEngine;

public class Dynamic3DBackground : MonoBehaviour
{
	public string msz_path;

	public string msz_name;

	public MeshRenderer mo_meshRenderer;

	public bool mb_applyIpadScaling;

	public bool mb_applyIpadPositionScaling;

	public bool mb_useIpadPath = true;

	public Vector3 mv3_iPadPositionOffset = Vector3.zero;

	public Vector3 mv3_iPadScaleMultiplier = new Vector3(1f, 1f, 1f);

	public bool mb_forceToEnglish;

	public string msz_localisationSuffix_english = string.Empty;

	public string msz_localisationSuffix_french = string.Empty;

	public string msz_localisationSuffix_portuguese = string.Empty;

	public string msz_localisationSuffix_spanish = string.Empty;

	public string msz_localisationSuffix_german = string.Empty;

	public string msz_localisationSuffix_japanese = string.Empty;

	public Vector3 scaleFactorEnglish = Vector3.one;

	public Vector3 scaleFactorFrench = Vector3.one;

	public Vector3 scaleFactorPortuguese = Vector3.one;

	public Vector3 scaleFactorSpanish = Vector3.one;

	public Vector3 scaleFactorGerman = Vector3.one;

	public Vector3 scaleFactorJapanese = Vector3.one;

	private void Awake()
	{
		LoadDeviceDependentTexture(true);
	}

	private void LoadDeviceDependentTexture(bool aShouldScale)
	{
		if (mo_meshRenderer != null)
		{
			if (msz_name != string.Empty)
			{
				string empty = string.Empty;
				if (LocalizationManager.IsFrench)
				{
					empty = msz_localisationSuffix_french;
				}
				else if (LocalizationManager.IsPortuguese)
				{
					empty = msz_localisationSuffix_portuguese;
				}
				else if (LocalizationManager.IsSpanish)
				{
					empty = msz_localisationSuffix_spanish;
				}
				else if (LocalizationManager.IsEnglish)
				{
					empty = msz_localisationSuffix_english;
				}
				else if (LocalizationManager.IsGerman)
				{
					empty = msz_localisationSuffix_german;
				}
				else if (LocalizationManager.IsJapanese)
				{
					empty = msz_localisationSuffix_japanese;
				}
				if (mb_forceToEnglish)
				{
					empty = msz_localisationSuffix_english;
				}
				string text = ((ResolutionManager.Instance.AssetResolution != ResolutionManager.eAssetResolution.eLowres) ? string.Empty : "_lowres");
				string text2 = string.Format("{0}{1}{2}{3}{4}", msz_path, (!mb_useIpadPath || ResolutionManager.Instance.AssetResolution != ResolutionManager.eAssetResolution.eIPad) ? string.Empty : "IPad/", msz_name, empty, text);
				mo_meshRenderer.material.mainTexture = GUIUtil.LoadTexture2D(text2);
				if (mo_meshRenderer.material.mainTexture == null)
				{
					Debug.Log(string.Format("Did not find {0}. Defaulting to {1}\n", text2, msz_path + msz_name + empty + text));
					mo_meshRenderer.material.mainTexture = GUIUtil.LoadTexture2D(msz_path + msz_name + empty + text);
				}
			}
			Utilities.AssertMsg(mo_meshRenderer.material.mainTexture != null, "Dynamic3DBackground not loaded!");
		}
		if (aShouldScale)
		{
			if (ResolutionManager.Instance.AssetResolution == ResolutionManager.eAssetResolution.eIPad && mb_applyIpadScaling)
			{
				Vector3 localScale = base.gameObject.transform.localScale;
				localScale.x *= 8f / 9f;
				base.transform.localScale = localScale;
			}
			if (ResolutionManager.Instance.AssetResolution == ResolutionManager.eAssetResolution.eIPad && mb_applyIpadPositionScaling)
			{
				Vector3 localPosition = base.gameObject.transform.localPosition;
				localPosition.x *= 8f / 9f;
				base.transform.localPosition = localPosition;
			}
			if (ResolutionManager.Instance.AssetResolution == ResolutionManager.eAssetResolution.eIPad)
			{
				Vector3 localPosition2 = base.gameObject.transform.localPosition;
				localPosition2 += mv3_iPadPositionOffset;
				base.transform.localPosition = localPosition2;
			}
			if (ResolutionManager.Instance.AssetResolution == ResolutionManager.eAssetResolution.eIPad)
			{
				Vector3 localScale2 = base.gameObject.transform.localScale;
				localScale2.x *= mv3_iPadScaleMultiplier.x;
				localScale2.y *= mv3_iPadScaleMultiplier.y;
				localScale2.z *= mv3_iPadScaleMultiplier.z;
				base.transform.localScale = localScale2;
			}
			if (LocalizationManager.IsFrench)
			{
				Vector3 localScale3 = base.transform.localScale;
				localScale3.Scale(scaleFactorFrench);
				base.transform.localScale = localScale3;
			}
			else if (LocalizationManager.IsPortuguese)
			{
				Vector3 localScale4 = base.transform.localScale;
				localScale4.Scale(scaleFactorPortuguese);
				base.transform.localScale = localScale4;
			}
			else if (LocalizationManager.IsSpanish)
			{
				Vector3 localScale5 = base.transform.localScale;
				localScale5.Scale(scaleFactorSpanish);
				base.transform.localScale = localScale5;
			}
			else if (LocalizationManager.IsEnglish)
			{
				Vector3 localScale6 = base.transform.localScale;
				localScale6.Scale(scaleFactorEnglish);
				base.transform.localScale = localScale6;
			}
			else if (LocalizationManager.IsGerman)
			{
				Vector3 localScale7 = base.transform.localScale;
				localScale7.Scale(scaleFactorGerman);
				base.transform.localScale = localScale7;
			}
			else if (LocalizationManager.IsJapanese)
			{
				Vector3 localScale8 = base.transform.localScale;
				localScale8.Scale(scaleFactorJapanese);
				base.transform.localScale = localScale8;
			}
		}
	}

	public void LoadNewTexture(string aNewPath, string aNewName)
	{
		msz_path = aNewPath;
		msz_name = aNewName;
		LoadDeviceDependentTexture(false);
	}
}
