using System;
using System.Text;
using UnityEngine;

public static class GUIUtil
{
	private static GUIDefines.TextureInfo sm_SemiTransparentLayer = new GUIDefines.TextureInfo
	{
		name = "GUI/Common/semi_transparent"
	};

	private static Rect sm_Rect = default(Rect);

	private static GUIContent sm_Content = new GUIContent();

	private static Vector2 sm_Vector2 = default(Vector2);

	private static GUILayoutOption[] sm_LayoutOptions = new GUILayoutOption[2];

	private static Color sm_BackupGUIColor;

	public static void ApplyBgColor(bool ab_Apply, Color ao_Color, bool ab_BgOnly)
	{
		if (ab_Apply)
		{
			if (ab_BgOnly)
			{
				sm_BackupGUIColor = GUI.backgroundColor;
				GUI.backgroundColor = ao_Color;
			}
			else
			{
				sm_BackupGUIColor = GUI.color;
				GUI.color = ao_Color;
			}
		}
	}

	public static void RestoreBgColor(bool ab_Restore, bool ab_BgOnly)
	{
		if (ab_Restore)
		{
			if (ab_BgOnly)
			{
				GUI.backgroundColor = sm_BackupGUIColor;
			}
			else
			{
				GUI.color = sm_BackupGUIColor;
			}
		}
	}

	public static Rect ConvertRatioToPixel(float aLeftRatio, float aTopRatio, float aWidthRatio, float aHeightRatio, bool aKeepSizeRatioOnIPad, bool aKeepWidthRatioOnIpad)
	{
		sm_Rect.xMin = aLeftRatio * GUIConstants.kReferenceScreenWidth;
		sm_Rect.yMin = aTopRatio * GUIConstants.kReferenceScreenHeight;
		sm_Rect.width = aWidthRatio * GUIConstants.kReferenceScreenWidth;
		sm_Rect.height = aHeightRatio * GUIConstants.kReferenceScreenHeight;
		if (Utilities.ReferenceAspectRatio != 1.5f)
		{
			if (!aKeepSizeRatioOnIPad && !aKeepWidthRatioOnIpad && aWidthRatio < 1f)
			{
				sm_Rect.xMin = sm_Rect.xMin;
				sm_Rect.xMax = sm_Rect.xMax;
				sm_Rect.width *= 0.9375f;
			}
			if (!aKeepSizeRatioOnIPad && aHeightRatio < 1f)
			{
				sm_Rect.yMin = sm_Rect.yMin;
				sm_Rect.yMax = sm_Rect.yMax;
				sm_Rect.height *= 5f / 6f;
			}
		}
		return sm_Rect;
	}

	public static Vector2 ConvertRatioToPixel(float aXSpaceRatio, float aYSpaceRatio)
	{
		Rect rect = ConvertRatioToPixel(aXSpaceRatio, aYSpaceRatio, 0f, 0f, true, true);
		sm_Vector2.x = rect.xMin;
		sm_Vector2.y = rect.yMin;
		return sm_Vector2;
	}

	public static float CalculateIpadLeftOffset(float aTargetRatio, float aRefRatio)
	{
		return (aTargetRatio - aRefRatio) * -64f;
	}

	public static float CalculateIpadTopOffset(float aTargetRatio, float aRefRatio)
	{
		return (aTargetRatio - aRefRatio) * -128f;
	}

	public static Rect ApplyIPadAdjustment(Rect aRectInPixel, float aLeftRatio, float aTopRatio, GUIDefines.RectIPadInfo aIPad)
	{
		sm_Rect = aRectInPixel;
		float num = aIPad.leftOffset;
		float num2 = aIPad.topOffset;
		if (aIPad.useLeftRefRatio)
		{
			num += CalculateIpadLeftOffset(aLeftRatio, aIPad.leftRefRatio);
		}
		num += aIPad.leftRefOffset;
		if (num != 0f)
		{
			sm_Rect.xMin = aLeftRatio * GUIConstants.kReferenceScreenWidth + num;
		}
		if (aIPad.useTopRefRatio)
		{
			num2 += CalculateIpadTopOffset(aTopRatio, aIPad.topRefRatio);
		}
		num2 += aIPad.topRefOffset;
		if (num2 != 0f)
		{
			sm_Rect.yMin = aTopRatio * GUIConstants.kReferenceScreenHeight + num2;
		}
		sm_Rect.xMax += num + aIPad.widthScale;
		sm_Rect.yMax += num2 + aIPad.heightScale;
		return sm_Rect;
	}

	public static void ApplyIPadAdjustment(ref Vector2 aWorkVector, float aXRatio, float aYRatio, GUIDefines.Vector2IPadInfo aIPad)
	{
		if (aIPad.xOffset != 0f)
		{
			aWorkVector.x = aXRatio * GUIConstants.kReferenceScreenWidth + aIPad.xOffset;
		}
		if (aIPad.yOffset != 0f)
		{
			aWorkVector.y = aYRatio * GUIConstants.kReferenceScreenHeight + aIPad.yOffset;
		}
	}

	public static Rect ConvertToRelativePos(Vector3 aReferencePos, GUIDefines.RectInfo aAbsolutePos)
	{
		sm_Rect = aAbsolutePos.inPixel;
		if (!aAbsolutePos.detatchFromRefObject)
		{
			sm_Rect.xMin = aAbsolutePos.inPixel.xMin + aReferencePos.x * 30f;
			sm_Rect.yMin = aAbsolutePos.inPixel.yMin - aReferencePos.y * 30f;
			sm_Rect.width = aAbsolutePos.inPixel.width;
			sm_Rect.height = aAbsolutePos.inPixel.height;
		}
		return sm_Rect;
	}

	public static Rect FullScreenRect()
	{
		sm_Rect.xMin = 0f;
		sm_Rect.yMin = 0f;
		sm_Rect.width = GUIConstants.kReferenceScreenWidth;
		sm_Rect.height = GUIConstants.kReferenceScreenHeight;
		return sm_Rect;
	}

	public static Rect ApplyStyleBgOriginalSize(GUIDefines.RectInfo aPos, GUIDefines.StyleInfo aStyle)
	{
		sm_Rect = aPos.inPixel;
		GUIStyle guiStyle = GetGuiStyle(aStyle);
		if (guiStyle != null)
		{
			bool aKeepSizeRatioOnIPad = aPos.IPad != null && aPos.IPad.keepSizeRatio;
			bool aKeepWidthRatioOnIPad = aPos.IPad != null && aPos.IPad.keepWidthRatio;
			if (aPos.useOriginalWidth && Utilities.AssertMsg(guiStyle.normal.background != null, "ApplyStyleBgOriginalSize: Invalid normal.background Texture2D in GUI Style"))
			{
				sm_Rect.width = GetDisplayImageWidth(guiStyle.normal.background.width, GetImageRelativeScreenWidth(guiStyle.normal.background.name), aKeepSizeRatioOnIPad, aKeepWidthRatioOnIPad);
			}
			if (aPos.useOriginalHeight && Utilities.AssertMsg(guiStyle.normal.background != null, "ApplyStyleBgOriginalSize: Invalid normal.background Texture2D in GUI Style"))
			{
				sm_Rect.height = GetDisplayImageHeight(guiStyle.normal.background.height, GetImageRelativeScreenHeight(guiStyle.normal.background.name), aKeepSizeRatioOnIPad);
			}
		}
		return sm_Rect;
	}

	public static Rect ApplyTextureOriginalSize(GUIDefines.RectInfo aPos, GUIDefines.TextureInfo aTexture)
	{
		sm_Rect = aPos.inPixel;
		if (aTexture != null && aTexture.image != null)
		{
			bool aKeepSizeRatioOnIPad = aPos.IPad != null && aPos.IPad.keepSizeRatio;
			bool aKeepWidthRatioOnIPad = aPos.IPad != null && aPos.IPad.keepWidthRatio;
			if (aPos.useOriginalWidth)
			{
				sm_Rect.width = GetDisplayImageWidth(aTexture.image.width, GetImageRelativeScreenWidth(aTexture.image.name), aKeepSizeRatioOnIPad, aKeepWidthRatioOnIPad);
			}
			if (aPos.useOriginalHeight)
			{
				sm_Rect.height = GetDisplayImageHeight(aTexture.image.height, GetImageRelativeScreenHeight(aTexture.image.name), aKeepSizeRatioOnIPad);
			}
		}
		return sm_Rect;
	}

	public static Vector2 ApplyTextureOriginalSize(GUIDefines.Vector2Info aSize, GUIDefines.TextureInfo aTexture)
	{
		sm_Vector2 = aSize.inPixel;
		if (aTexture.image != null)
		{
			if (aSize.useOriginalWidth)
			{
				sm_Vector2.x = GetDisplayImageWidth(aTexture.image.width, GetImageRelativeScreenWidth(aTexture.image.name), false, false);
			}
			if (aSize.useOriginalHeight)
			{
				sm_Vector2.y = GetDisplayImageHeight(aTexture.image.height, GetImageRelativeScreenHeight(aTexture.image.name), false);
			}
		}
		return sm_Vector2;
	}

	public static float GetDisplayImageWidth(float aActualImageWidth, float aRelativeScreenWidth, bool aKeepSizeRatioOnIPad, bool aKeepWidthRatioOnIPad)
	{
		float num = aActualImageWidth;
		if (Utilities.ReferenceAspectRatio == 1.5f)
		{
			if (GUIConstants.kReferenceScreenWidth != aRelativeScreenWidth)
			{
				num = aActualImageWidth / aRelativeScreenWidth * GUIConstants.kReferenceScreenWidth;
			}
		}
		else
		{
			num = aActualImageWidth / aRelativeScreenWidth * GUIConstants.kReferenceScreenWidth;
			if (!aKeepSizeRatioOnIPad && !aKeepWidthRatioOnIPad)
			{
				num *= 0.9375f;
			}
		}
		Utilities.AssertMsg(num >= 0f, "Invalid display width: " + num);
		return num;
	}

	public static float GetDisplayImageHeight(float aActualImageHeight, float aRelativeScreenHeight, bool aKeepSizeRatioOnIPad)
	{
		float num = aActualImageHeight;
		if (Utilities.ReferenceAspectRatio == 1.5f)
		{
			if (GUIConstants.kReferenceScreenHeight != aRelativeScreenHeight)
			{
				num = aActualImageHeight / aRelativeScreenHeight * GUIConstants.kReferenceScreenHeight;
			}
		}
		else
		{
			num = aActualImageHeight / aRelativeScreenHeight * GUIConstants.kReferenceScreenHeight;
			if (!aKeepSizeRatioOnIPad)
			{
				num *= 5f / 6f;
			}
		}
		Utilities.AssertMsg(num >= 0f, "Invalid display height: " + num);
		return num;
	}

	public static float GetImageRelativeScreenWidth(string aImageName)
	{
		if (aImageName.Contains("_lowres"))
		{
			return 480f;
		}
		return 960f;
	}

	public static float GetImageRelativeScreenHeight(string aImageName)
	{
		if (aImageName.Contains("_lowres"))
		{
			return 320f;
		}
		return 640f;
	}

	public static GUIStyle GetGuiStyle(GUIDefines.StyleInfo aStyle)
	{
		GUIStyle gUIStyle;
		if (aStyle == null || !aStyle.useCustomStyle)
		{
			gUIStyle = ((aStyle == null || aStyle.styleName == null || aStyle.styleName.Length <= 0) ? null : GUIStyleContainer.GetStyle(aStyle.styleName));
		}
		else
		{
			gUIStyle = CustomizeStyle(aStyle);
			Utilities.AssertMsg(gUIStyle != null, "Fail to customize GUI style!");
		}
		return gUIStyle;
	}

	public static GUIStyle CustomizeStyle(GUIDefines.StyleInfo aStyle)
	{
		GUIStyle customGUIStyle = new GUIStyle(GUIStyleContainer.CustomGUIStyle);
		if (aStyle.customNormal != null)
		{
			customGUIStyle.normal.background = aStyle.customNormal.image;
		}
		else
		{
			customGUIStyle.normal.background = null;
		}
		if (aStyle.customActive == null || aStyle.customActive.image == null)
		{
			customGUIStyle.active.background = customGUIStyle.normal.background;
			customGUIStyle.onActive.background = customGUIStyle.normal.background;
		}
		else
		{
			customGUIStyle.active.background = aStyle.customActive.image;
			customGUIStyle.onNormal.background = aStyle.customActive.image;
			customGUIStyle.onActive.background = aStyle.customNormal.image;
		}
		customGUIStyle.hover.background = customGUIStyle.normal.background;
		customGUIStyle.focused.background = customGUIStyle.active.background;
		customGUIStyle.onFocused.background = customGUIStyle.onActive.background;
		customGUIStyle.onHover.background = customGUIStyle.onNormal.background;
		if (aStyle.customFontType == GUIDefines.FontType.eOnDemand)
		{
			customGUIStyle.font = GameFlowManager.Instance.GUIManager.GetOnDemandFont(aStyle.customOnDemandFontName);
		}
		else
		{
			customGUIStyle.font = GameFlowManager.Instance.GUIManager.GetFont(aStyle.customFontSize, aStyle.customFontType);
		}
		Vector2 vector = ((aStyle.customPadding == null) ? Vector2.zero : GetSpace(aStyle.customPadding));
		customGUIStyle.padding.left = (int)vector.x;
		customGUIStyle.padding.top = (int)vector.y;
		Vector2 vector2 = ((aStyle.customPadding2 == null) ? Vector2.zero : GetSpace(aStyle.customPadding2));
		customGUIStyle.padding.right = (int)vector2.x;
		customGUIStyle.padding.bottom = (int)vector2.y;
		customGUIStyle.normal.textColor = aStyle.customNormalTextColor;
		customGUIStyle.active.textColor = aStyle.customActiveTextColor;
		customGUIStyle.focused.textColor = aStyle.customFocusedTextColor;
		customGUIStyle.hover.textColor = aStyle.customNormalTextColor;
		customGUIStyle.onNormal.textColor = aStyle.customActiveTextColor;
		customGUIStyle.onHover.textColor = aStyle.customActiveTextColor;
		customGUIStyle.onActive.textColor = aStyle.customNormalTextColor;
		customGUIStyle.onFocused.textColor = aStyle.customFocusedTextColor;
		customGUIStyle.alignment = aStyle.customTextAlignment;
		customGUIStyle.wordWrap = aStyle.customWordWrap;
		customGUIStyle.imagePosition = aStyle.customImagePosition;
		return customGUIStyle;
	}

	public static GUIStyle CreateDropShadowTextStyle(GUIStyle aGuiStyle)
	{
		Utilities.Assert(aGuiStyle != null);
		GUIStyle gUIStyle = new GUIStyle(aGuiStyle);
		Color textColor = DetermineDropShadowColor(aGuiStyle);
		SetTextColorForAllStates(gUIStyle, textColor);
		int dropShadowOffsetX = GameFlowManager.Instance.GUIManager.DropShadowOffsetX;
		int dropShadowOffsetY = GameFlowManager.Instance.GUIManager.DropShadowOffsetY;
		gUIStyle.contentOffset = new Vector2(dropShadowOffsetX, dropShadowOffsetY);
		return gUIStyle;
	}

	public static GUIStyle CreateDropShadowTextStyleForLabel(GUIStyle aGuiStyle)
	{
		Utilities.Assert(aGuiStyle != null);
		GUIStyle gUIStyle = new GUIStyle(aGuiStyle);
		Color textColor = DetermineDropShadowColor(aGuiStyle);
		SetTextColorForAllStates(gUIStyle, textColor);
		return gUIStyle;
	}

	public static GUIStyle CreateCustomDropShadowTextStyleForLabel(GUIStyle aGuiStyle, Color aColor)
	{
		Utilities.Assert(aGuiStyle != null);
		GUIStyle gUIStyle = new GUIStyle(aGuiStyle);
		SetTextColorForAllStates(gUIStyle, aColor);
		return gUIStyle;
	}

	private static Color DetermineDropShadowColor(GUIStyle aGuiStyle)
	{
		if (aGuiStyle.normal.textColor.Equals(GUIConstants.kWhiteColor) || aGuiStyle.normal.textColor.Equals(GUIConstants.kLevelSelectNewTextColor) || aGuiStyle.normal.textColor.Equals(GUIConstants.kLightGreyColor))
		{
			return GameFlowManager.Instance.GUIManager.DarkBrownDropShadowColor;
		}
		return GameFlowManager.Instance.GUIManager.WhiteDropShadowColor;
	}

	public static GUIStyle CreateFrontTextStyle(GUIStyle aGuiStyle)
	{
		Utilities.Assert(aGuiStyle != null);
		GUIStyle gUIStyle = new GUIStyle(aGuiStyle);
		SetBackgroundForAllStates(gUIStyle, null);
		return gUIStyle;
	}

	public static GUIStyle CreateFrontTextStyleWithNoDropShadow(GUIStyle aGuiStyle)
	{
		Utilities.Assert(aGuiStyle != null);
		return new GUIStyle(aGuiStyle);
	}

	public static GUIContent CreateGuiContent(GUIDefines.ContentInfo aContent)
	{
		sm_Content.text = string.Empty;
		sm_Content.image = null;
		if (aContent == null)
		{
			return sm_Content;
		}
		if (aContent.text != null && aContent.text.Length > 0)
		{
			sm_Content.text = aContent.text;
		}
		else if (aContent.textId != null && aContent.textId.Length > 0)
		{
			sm_Content.text = LocalizationManager.Instance.GetString(aContent.textId);
		}
		if (aContent.prefixText != null && aContent.prefixText.Length > 0)
		{
			sm_Content.text = aContent.prefixText + " " + sm_Content.text;
		}
		else if (aContent.prefixTextId != null && aContent.prefixTextId.Length > 0)
		{
			sm_Content.text = LocalizationManager.Instance.GetString(aContent.prefixTextId) + " " + sm_Content.text;
		}
		if (aContent.suffixText != null && aContent.suffixText.Length > 0)
		{
			sm_Content.text = sm_Content.text + " " + aContent.suffixText;
		}
		else if (aContent.suffixTextId != null && aContent.suffixTextId.Length > 0)
		{
			sm_Content.text = sm_Content.text + " " + LocalizationManager.Instance.GetString(aContent.suffixTextId);
		}
		if (aContent.icon != null)
		{
			sm_Content.image = aContent.icon.image;
		}
		return sm_Content;
	}

	public static GUILayoutOption[] CreateGuiLayoutOptions(GUIDefines.Vector2Info aSize)
	{
		sm_Vector2 = aSize.inPixel;
		sm_LayoutOptions[0] = GUILayout.Width(sm_Vector2.x);
		sm_LayoutOptions[1] = GUILayout.Height(sm_Vector2.y);
		return sm_LayoutOptions;
	}

	public static Vector2 GetSpace(GUIDefines.Vector2Info aSpace)
	{
		if (aSpace == null)
		{
			return Vector2.zero;
		}
		sm_Vector2 = aSpace.inPixel;
		return sm_Vector2;
	}

	public static void SetControlName(string aControlName)
	{
		if (aControlName != null && aControlName.Length > 0)
		{
			GUI.SetNextControlName(aControlName);
		}
	}

	public static bool IsCurrentFocusControl(string aControlName)
	{
		if (aControlName != null && aControlName.Length > 0)
		{
			return GUI.GetNameOfFocusedControl() == aControlName;
		}
		return false;
	}

	public static int PrevPage(GUIDefines.GroupButtonData aGroupButtonData, int aFirstInPage)
	{
		int num = aFirstInPage - aGroupButtonData.multiPage.elementPerRow * aGroupButtonData.multiPage.elementPerCol;
		if (num < 0)
		{
			num = aFirstInPage;
		}
		return num;
	}

	public static int NextPage(GUIDefines.GroupButtonData aGroupButtonData, int aFirstInPage)
	{
		int num = aFirstInPage + aGroupButtonData.multiPage.elementPerRow * aGroupButtonData.multiPage.elementPerCol;
		if (num >= aGroupButtonData.elements.Length)
		{
			num = aFirstInPage;
		}
		return num;
	}

	public static string GetStringToDisplay(string aString, GUIStyle aStyle, float aWidth, bool aShowCursor)
	{
		string text = string.Format("{0}|", aString);
		int num = 0;
		for (float num2 = aStyle.CalcSize(new GUIContent(text)).x - aWidth; num2 > 0f; num2 = aStyle.CalcSize(new GUIContent(text.Substring(num, text.Length - num))).x - aWidth)
		{
			num++;
		}
		string empty = string.Empty;
		empty = ((!aShowCursor || !(Time.time - (float)(int)(Time.time / 1.5f) * 1.5f > 0.75f)) ? aString : text);
		return empty.Substring(num, empty.Length - num);
	}

	public static string MaskPassword(ref GUIDefines.TextFieldData aTextFieldData, string aPassword)
	{
		if (aTextFieldData.editedText == null || aPassword.Length > aTextFieldData.editedText.Length)
		{
			aTextFieldData.timeOfNukedPassword = Time.realtimeSinceStartup;
		}
		else if (aPassword.Length < aTextFieldData.editedText.Length)
		{
			aTextFieldData.timeOfNukedPassword = 0f;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append('*', aPassword.Length - 1);
		if (Time.realtimeSinceStartup - aTextFieldData.timeOfNukedPassword < 2f)
		{
			stringBuilder.Append(aPassword.Substring(aPassword.Length - 1, 1));
		}
		else
		{
			stringBuilder.Append('*');
		}
		return stringBuilder.ToString();
	}

	public static bool AutoResizeAccordingToContent(GUIStyle aStyle, GUIContent aContent, GUIDefines.AutoResizeAllignment aResizeAllignment, ref Rect aPosition)
	{
		if (aStyle == null || aContent == null)
		{
			return false;
		}
		float x = aStyle.CalcSize(aContent).x;
		if (aPosition.width < x + 10f)
		{
			float num = x - aPosition.width + 10f;
			float num2 = aPosition.x / GUIConstants.kReferenceScreenWidth;
			float num3 = (aPosition.x + aPosition.width) / GUIConstants.kReferenceScreenWidth;
			switch ((GUIDefines.AutoResizeAllignment)((aResizeAllignment != GUIDefines.AutoResizeAllignment.eAuto) ? ((int)aResizeAllignment) : ((num2 <= 0.1f) ? 2 : ((!(num3 >= 0.9f)) ? 1 : 3))))
			{
			case GUIDefines.AutoResizeAllignment.eRight:
				aPosition.x -= num;
				break;
			default:
				aPosition.x -= num / 2f;
				break;
			case GUIDefines.AutoResizeAllignment.eLeft:
				break;
			}
			aPosition.width += num;
			return true;
		}
		return false;
	}

	public static void DrawSemiTransparentLayer()
	{
		if (sm_SemiTransparentLayer.image == null)
		{
			sm_SemiTransparentLayer.Init();
		}
		GUICompoundControls.FullScreenTexture(sm_SemiTransparentLayer);
	}

	public static void CleanUp()
	{
		sm_SemiTransparentLayer.image = null;
	}

	public static float FindHorizontalPositionToAlign(float af_widthRatio, int ai_numDivisions, int ai_divisionIndex)
	{
		float num = (1f / (float)ai_numDivisions - af_widthRatio) / 2f;
		float num2 = 1f / (float)ai_numDivisions * (float)ai_divisionIndex;
		return num + num2;
	}

	public static string NormalizeResourcePath(string aResourcePath)
	{
		if (string.IsNullOrEmpty(aResourcePath))
		{
			return aResourcePath;
		}
		string[] array = aResourcePath.Split('/');
		for (int i = 0; i < array.Length - 1; i++)
		{
			if (!string.IsNullOrEmpty(array[i]))
			{
				array[i] = array[i].ToLowerInvariant();
			}
		}
		return string.Join("/", array);
	}

	public static Texture2D LoadTexture2D(string aTextureName)
	{
		GUITextureStatistics.MarkTextureInUse(aTextureName);
		Texture2D texture2D = Resources.Load(aTextureName, typeof(Texture2D)) as Texture2D;
		if (texture2D == null)
		{
			string text = NormalizeResourcePath(aTextureName);
			if (!string.Equals(text, aTextureName, StringComparison.Ordinal))
			{
				texture2D = Resources.Load(text, typeof(Texture2D)) as Texture2D;
			}
		}
		return texture2D;
	}

	public static Texture LoadTexture(string aTextureName)
	{
		GUITextureStatistics.MarkTextureInUse(aTextureName);
		Texture texture = Resources.Load(aTextureName, typeof(Texture)) as Texture;
		if (texture == null)
		{
			string text = NormalizeResourcePath(aTextureName);
			if (!string.Equals(text, aTextureName, StringComparison.Ordinal))
			{
				texture = Resources.Load(text, typeof(Texture)) as Texture;
			}
		}
		return texture;
	}

	private static void SetBackgroundForAllStates(GUIStyle aGuiStyle, Texture2D aBackground)
	{
		aGuiStyle.normal.background = aBackground;
		aGuiStyle.hover.background = aBackground;
		aGuiStyle.active.background = aBackground;
		aGuiStyle.focused.background = aBackground;
		aGuiStyle.onNormal.background = aBackground;
		aGuiStyle.onHover.background = aBackground;
		aGuiStyle.onActive.background = aBackground;
		aGuiStyle.onFocused.background = aBackground;
	}

	private static void SetTextColorForAllStates(GUIStyle aGuiStyle, Color aTextColor)
	{
		aGuiStyle.normal.textColor = aTextColor;
		aGuiStyle.hover.textColor = aTextColor;
		aGuiStyle.active.textColor = aTextColor;
		aGuiStyle.focused.textColor = aTextColor;
		aGuiStyle.onNormal.textColor = aTextColor;
		aGuiStyle.onHover.textColor = aTextColor;
		aGuiStyle.onActive.textColor = aTextColor;
		aGuiStyle.onFocused.textColor = aTextColor;
	}
}
