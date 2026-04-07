using System.Collections.Generic;
using UnityEngine;

public static class GUIStyleContainer
{
	public struct TableData
	{
		public GUIStyle guiStyle;

		public CreateGUIStyleFunc createFunc;

		public TableData(CreateGUIStyleFunc aFunc)
		{
			guiStyle = null;
			createFunc = aFunc;
		}
	}

	public delegate GUIStyle CreateGUIStyleFunc();

	public static GUIStyle CustomGUIStyle = new GUIStyle();

	public static Dictionary<string, TableData> GUIStyleTable;

	public static void Init()
	{
		SetupCustomGUIStyle();
		GUIStyleTable = new Dictionary<string, TableData>();
		GUIStyleTable["LightGrayButton"] = new TableData(CreateLightGrayButtonGUIStyle);
		GUIStyleTable["CheatButton"] = new TableData(CreateCheatButtonGUIStyle);
		GUIStyleTable["SmallButton"] = new TableData(CreateSmallButtonGUIStyle);
		GUIStyleTable["SlowmoButton"] = new TableData(CreateSlowmoButtonGUIStyle);
		GUIStyleTable["TurboButton"] = new TableData(CreateTurboButtonGUIStyle);
		GUIStyleTable["ErrorPopupWindow"] = new TableData(CreateErrorPopupWindowGUIStyle);
		GUIStyleTable["InGameTextMini"] = new TableData(CreateMiniLabelGUIStyle);
		GUIStyleTable["InGameTextSmall"] = new TableData(CreateSmallLabelGUIStyle);
		GUIStyleTable["InGameTextMedium"] = new TableData(CreateMediumLabelGUIStyle);
		GUIStyleTable["InGameTextLarge"] = new TableData(CreateLargeLabelGUIStyle);
		GUIStyleTable["TallyScreenCounter"] = new TableData(CreateTallyScreenCounterGUIStyle);
	}

	public static void CleanUp()
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, TableData> item in GUIStyleTable)
		{
			if (item.Value.guiStyle != null)
			{
				list.Add(item.Key);
			}
		}
		foreach (string item2 in list)
		{
			TableData value = GUIStyleTable[item2];
			value.guiStyle = null;
			GUIStyleTable[item2] = value;
		}
		CustomGUIStyle.normal.background = null;
		CustomGUIStyle.hover.background = null;
		CustomGUIStyle.active.background = null;
		CustomGUIStyle.focused.background = null;
	}

	public static GUIStyle GetStyle(string aStyleName)
	{
		TableData value;
		if (GUIStyleTable.TryGetValue(aStyleName, out value))
		{
			if (value.guiStyle == null && Utilities.AssertMsg(value.createFunc != null, "Create function is not set for GUI style: " + aStyleName))
			{
				value.guiStyle = value.createFunc();
				GUIStyleTable[aStyleName] = value;
			}
			return value.guiStyle;
		}
		Utilities.AssertMsg(false, "GUI Style: " + aStyleName + " not found!");
		return null;
	}

	public static void SetupCustomGUIStyle()
	{
		CustomGUIStyle.clipping = TextClipping.Overflow;
	}

	public static GUIStyle CreateLightGrayButtonGUIStyle()
	{
		return CreateStandardButtonGUIStyle("GUI/LevelSelect/button_back", "GUI/LevelSelect/button_back_pressed", GameFlowManager.Instance.GUIManager.GetFont(GUIDefines.FontSize.eSmall, GUIDefines.FontType.eCPMenus), GUIConstants.kBlackColor);
	}

	public static GUIStyle CreateCheatButtonGUIStyle()
	{
		return CreateStandardButtonGUIStyle("GUI/LevelSelect/cheat_button", "GUI/LevelSelect/cheat_button", GameFlowManager.Instance.GUIManager.GetFont(GUIDefines.FontSize.eSmall, GUIDefines.FontType.eInGame), GUIConstants.kBlackColor);
	}

	public static GUIStyle CreateSmallButtonGUIStyle()
	{
		return CreateStandardButtonGUIStyle("GUI/Common/button", "GUI/Common/button_pressed", GameFlowManager.Instance.GUIManager.GetFont(GUIDefines.FontSize.eMedium, GUIDefines.FontType.eCPMenus), GUIConstants.kWhiteColor);
	}

	public static GUIStyle CreateSlowmoButtonGUIStyle()
	{
		return CreateStandardToggleGUIStyle("GUI/InGame/SlowMoButton_Inactive", "GUI/InGame/SlowMoButton_Inactive", "GUI/InGame/SlowMoButton_Active", "GUI/InGame/SlowMoButton_Active", GameFlowManager.Instance.GUIManager.GetFont(GUIDefines.FontSize.eSmall, GUIDefines.FontType.eInGame));
	}

	public static GUIStyle CreateTurboButtonGUIStyle()
	{
		return CreateStandardToggleGUIStyle("GUI/LevelSelect/Popups/TurboMode_TurboOnBox", "GUI/LevelSelect/Popups/TurboMode_TurboOnBox", "GUI/LevelSelect/Popups/TurboMode_TurboOnBox_Checked", "GUI/LevelSelect/Popups/TurboMode_TurboOnBox_Checked", GameFlowManager.Instance.GUIManager.GetFont(GUIDefines.FontSize.eMedium, GUIDefines.FontType.eCPMenus));
	}

	public static GUIStyle CreateMiniLabelGUIStyle()
	{
		return CreateStandardLabelGUIStyle(GameFlowManager.Instance.GUIManager.GetFont(GUIDefines.FontSize.eMini, GUIDefines.FontType.eInGame), TextAnchor.MiddleCenter, Color.white);
	}

	public static GUIStyle CreateSmallLabelGUIStyle()
	{
		return CreateStandardLabelGUIStyle(GameFlowManager.Instance.GUIManager.GetFont(GUIDefines.FontSize.eSmall, GUIDefines.FontType.eInGame), TextAnchor.MiddleCenter, Color.white);
	}

	public static GUIStyle CreateMediumLabelGUIStyle()
	{
		return CreateStandardLabelGUIStyle(GameFlowManager.Instance.GUIManager.GetFont(GUIDefines.FontSize.eMedium, GUIDefines.FontType.eInGame), TextAnchor.MiddleCenter, Color.white);
	}

	public static GUIStyle CreateLargeLabelGUIStyle()
	{
		return CreateStandardLabelGUIStyle(GameFlowManager.Instance.GUIManager.GetFont(GUIDefines.FontSize.eLarge, GUIDefines.FontType.eInGame), TextAnchor.MiddleCenter, Color.white);
	}

	public static GUIStyle CreateTallyScreenCounterGUIStyle()
	{
		return CreateStandardLabelGUIStyle(GameFlowManager.Instance.GUIManager.GetFont(GUIDefines.FontSize.eMedium, GUIDefines.FontType.eInGame), TextAnchor.MiddleRight, Color.white);
	}

	public static GUIStyle CreateStandardButtonGUIStyle(string aNormalTextureName, string aActiveTextureName, Font aFont, Color aTextColor)
	{
		GUIDefines.Texture2DInfo texture2DInfo = new GUIDefines.Texture2DInfo();
		texture2DInfo.name = aNormalTextureName;
		GUIDefines.Texture2DInfo texture2DInfo2 = texture2DInfo;
		texture2DInfo2.Init();
		texture2DInfo = new GUIDefines.Texture2DInfo();
		texture2DInfo.name = aActiveTextureName;
		GUIDefines.Texture2DInfo texture2DInfo3 = texture2DInfo;
		texture2DInfo3.Init();
		GUIStyle gUIStyle = new GUIStyle();
		gUIStyle.normal.background = texture2DInfo2.image;
		gUIStyle.normal.textColor = aTextColor;
		gUIStyle.hover.background = texture2DInfo2.image;
		gUIStyle.hover.textColor = aTextColor;
		gUIStyle.active.background = texture2DInfo3.image;
		gUIStyle.active.textColor = aTextColor;
		gUIStyle.focused.background = texture2DInfo3.image;
		gUIStyle.focused.textColor = aTextColor;
		gUIStyle.onNormal.background = texture2DInfo2.image;
		gUIStyle.onNormal.textColor = aTextColor;
		gUIStyle.onHover.background = texture2DInfo3.image;
		gUIStyle.onHover.textColor = aTextColor;
		gUIStyle.onActive.background = texture2DInfo3.image;
		gUIStyle.onActive.textColor = aTextColor;
		gUIStyle.onFocused.background = texture2DInfo3.image;
		gUIStyle.onFocused.textColor = aTextColor;
		gUIStyle.clipping = TextClipping.Overflow;
		gUIStyle.font = aFont;
		gUIStyle.alignment = TextAnchor.MiddleCenter;
		return gUIStyle;
	}

	public static GUIStyle CreateStandardToggleGUIStyle(string aNormalTextureName, string aActiveTextureName, string aOnNormalTextureName, string aOnActiveTextureName, Font aFont)
	{
		GUIDefines.Texture2DInfo texture2DInfo = new GUIDefines.Texture2DInfo();
		texture2DInfo.name = aNormalTextureName;
		GUIDefines.Texture2DInfo texture2DInfo2 = texture2DInfo;
		texture2DInfo2.Init();
		texture2DInfo = new GUIDefines.Texture2DInfo();
		texture2DInfo.name = aActiveTextureName;
		GUIDefines.Texture2DInfo texture2DInfo3 = texture2DInfo;
		texture2DInfo3.Init();
		texture2DInfo = new GUIDefines.Texture2DInfo();
		texture2DInfo.name = aOnNormalTextureName;
		GUIDefines.Texture2DInfo texture2DInfo4 = texture2DInfo;
		texture2DInfo4.Init();
		texture2DInfo = new GUIDefines.Texture2DInfo();
		texture2DInfo.name = aOnActiveTextureName;
		GUIDefines.Texture2DInfo texture2DInfo5 = texture2DInfo;
		texture2DInfo5.Init();
		GUIStyle gUIStyle = new GUIStyle();
		gUIStyle.normal.background = texture2DInfo2.image;
		gUIStyle.normal.textColor = GUIConstants.kWhiteColor;
		gUIStyle.hover.background = texture2DInfo2.image;
		gUIStyle.hover.textColor = GUIConstants.kWhiteColor;
		gUIStyle.active.background = texture2DInfo3.image;
		gUIStyle.active.textColor = GUIConstants.kWhiteColor;
		gUIStyle.focused.background = texture2DInfo3.image;
		gUIStyle.focused.textColor = GUIConstants.kWhiteColor;
		gUIStyle.onNormal.background = texture2DInfo4.image;
		gUIStyle.onNormal.textColor = GUIConstants.kBlackColor;
		gUIStyle.onHover.background = texture2DInfo4.image;
		gUIStyle.onHover.textColor = GUIConstants.kBlackColor;
		gUIStyle.onActive.background = texture2DInfo5.image;
		gUIStyle.onActive.textColor = GUIConstants.kBlackColor;
		gUIStyle.onFocused.background = texture2DInfo5.image;
		gUIStyle.onFocused.textColor = GUIConstants.kBlackColor;
		gUIStyle.clipping = TextClipping.Overflow;
		gUIStyle.font = aFont;
		gUIStyle.alignment = TextAnchor.MiddleCenter;
		return gUIStyle;
	}

	public static GUIStyle CreateStandardLabelGUIStyle(Font aFont, TextAnchor aAlignment, Color aTextColor)
	{
		GUIStyle gUIStyle = new GUIStyle();
		gUIStyle.font = aFont;
		gUIStyle.alignment = aAlignment;
		gUIStyle.normal.textColor = aTextColor;
		gUIStyle.hover.textColor = aTextColor;
		gUIStyle.active.textColor = aTextColor;
		gUIStyle.focused.textColor = aTextColor;
		gUIStyle.onNormal.textColor = aTextColor;
		gUIStyle.onHover.textColor = aTextColor;
		gUIStyle.onActive.textColor = aTextColor;
		gUIStyle.onFocused.textColor = aTextColor;
		return gUIStyle;
	}

	public static GUIStyle CreateErrorPopupWindowGUIStyle()
	{
		return CreateStandardWindowGUIStyle("GUI/Common/error_popup");
	}

	public static GUIStyle CreateStandardWindowGUIStyle(string aBackgroundTextureName)
	{
		GUIDefines.Texture2DInfo texture2DInfo = new GUIDefines.Texture2DInfo();
		texture2DInfo.name = aBackgroundTextureName;
		GUIDefines.Texture2DInfo texture2DInfo2 = texture2DInfo;
		texture2DInfo2.Init();
		GUIStyle gUIStyle = new GUIStyle();
		gUIStyle.normal.background = texture2DInfo2.image;
		gUIStyle.hover.background = texture2DInfo2.image;
		gUIStyle.active.background = texture2DInfo2.image;
		gUIStyle.focused.background = texture2DInfo2.image;
		gUIStyle.onNormal.background = texture2DInfo2.image;
		gUIStyle.onHover.background = texture2DInfo2.image;
		gUIStyle.onActive.background = texture2DInfo2.image;
		gUIStyle.onFocused.background = texture2DInfo2.image;
		return gUIStyle;
	}
}
