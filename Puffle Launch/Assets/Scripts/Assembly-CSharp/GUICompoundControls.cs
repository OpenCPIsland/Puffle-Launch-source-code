using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class GUICompoundControls
{
	private static TouchScreenKeyboard m_cKeyboard = null;

	private static TextInfo m_TextInfo = CultureInfo.InvariantCulture.TextInfo;

	private static List<GUIDefines.AutoResizeData> m_AutoResizeData = null;

	private static void InitAutoResizeData()
	{
		if (m_AutoResizeData == null)
		{
			m_AutoResizeData = new List<GUIDefines.AutoResizeData>();
		}
		m_AutoResizeData.Clear();
	}

	public static int Buttons(Vector3 aReferencePos, GUIDefines.ButtonData[] aButtonData)
	{
		int result = -1;
		if (GameFlowManager.Instance.GUIManager.EnableAutoResize)
		{
			InitAutoResizeData();
			for (int i = 0; i < aButtonData.Length; i++)
			{
				if (aButtonData[i] == null || aButtonData[i].isAutoResizeOff)
				{
					Utilities.AssertMsg(aButtonData[i] != null, "Button data () is null!");
					continue;
				}
				Rect aPosition = GUIUtil.ConvertToRelativePos(aReferencePos, aButtonData[i].pos);
				GUIContent aContent = GUIUtil.CreateGuiContent(aButtonData[i].content);
				GUIStyle guiStyle = GUIUtil.GetGuiStyle(aButtonData[i].style);
				if (GUIUtil.AutoResizeAccordingToContent(guiStyle, aContent, aButtonData[i].autoResizeAllignment, ref aPosition))
				{
					GUIDefines.AutoResizeData autoResizeData = new GUIDefines.AutoResizeData();
					autoResizeData.groupId = aButtonData[i].autoResizeGroupId;
					autoResizeData.index = i;
					autoResizeData.pos = new Rect(aPosition);
					m_AutoResizeData.Add(autoResizeData);
				}
			}
		}
		for (int j = 0; j < aButtonData.Length; j++)
		{
			if (aButtonData[j] == null || aButtonData[j].invisible)
			{
				continue;
			}
			bool isControlBlocked = aButtonData[j].isControlBlocked;
			Rect rect = GUIUtil.ConvertToRelativePos(aReferencePos, aButtonData[j].pos);
			GUIContent gUIContent = GUIUtil.CreateGuiContent(aButtonData[j].content);
			GUIStyle gUIStyle = GUIUtil.GetGuiStyle(aButtonData[j].style);
			if (gUIStyle == null)
			{
				gUIStyle = GUI.skin.button;
			}
			GUIStyle style = GUIUtil.CreateDropShadowTextStyle(gUIStyle);
			GUIStyle style2 = GUIUtil.CreateFrontTextStyle(gUIStyle);
			if (GameFlowManager.Instance.GUIManager.EnableAutoResize)
			{
				foreach (GUIDefines.AutoResizeData autoResizeDatum in m_AutoResizeData)
				{
					if (j == autoResizeDatum.index || (aButtonData[j].useAutoResizeGroup && aButtonData[j].autoResizeGroupId == autoResizeDatum.groupId))
					{
						rect.x = autoResizeDatum.pos.x;
						rect.width = autoResizeDatum.pos.width;
					}
				}
			}
			Rect position = rect;
			if (aButtonData[j].detectZoneScale > 0f)
			{
				position.width *= aButtonData[j].detectZoneScale;
				position.height *= aButtonData[j].detectZoneScale;
				position.xMin -= position.width - rect.width;
				position.yMin -= position.height - rect.height;
			}
			bool flag = false;
			bool flag2 = true;
			flag2 = Input.touchCount <= 1;
			flag2 = flag2 && !isControlBlocked;
			if (aButtonData[j].isTogglable)
			{
				if (flag2)
				{
					bool flag3 = GUI.Toggle(rect, aButtonData[j].toggleState, gUIContent, style);
					if (flag3 != aButtonData[j].toggleState)
					{
						aButtonData[j].toggleState = flag3;
						flag = true;
					}
				}
				else
				{
					GUI.Label(rect, gUIContent, style);
				}
			}
			else if (flag2)
			{
				flag = GUI.Button(rect, gUIContent, style);
			}
			else
			{
				GUI.Label(rect, gUIContent, style);
			}
			gUIContent.image = null;
			GUI.Label(rect, gUIContent, style2);
			if (aButtonData[j].detectZoneScale > 0f && flag2)
			{
				flag = flag || GUI.Button(position, GUIContent.none, GUIStyle.none);
			}
			if (flag)
			{
				result = aButtonData[j].buttonId;
			}
		}
		return result;
	}

	public static int MultiPageGroupButtons(Vector3 aReferencePos, GUIDefines.GroupButtonData aGroupButtonData, int aStartAtElement)
	{
		if (aStartAtElement >= aGroupButtonData.elements.Length)
		{
			return -1;
		}
		int result = -1;
		int num = aGroupButtonData.multiPage.elementPerRow * aGroupButtonData.multiPage.elementPerCol;
		int num2 = 0;
		GUILayoutOption[] options = GUIUtil.CreateGuiLayoutOptions(aGroupButtonData.size);
		Vector2 space = GUIUtil.GetSpace(aGroupButtonData.space);
		Rect screenRect = GUIUtil.ConvertToRelativePos(aReferencePos, aGroupButtonData.area);
		GUILayout.BeginArea(screenRect);
		GUILayout.BeginVertical();
		bool flag = false;
		for (int i = 0; i < aGroupButtonData.multiPage.elementPerCol; i++)
		{
			if (flag)
			{
				break;
			}
			GUILayout.BeginHorizontal();
			for (int j = 0; j < aGroupButtonData.multiPage.elementPerRow; j++)
			{
				int num3 = i * aGroupButtonData.multiPage.elementPerRow + j + aStartAtElement;
				if (num2 >= num || num3 >= aGroupButtonData.elements.Length)
				{
					flag = true;
					break;
				}
				GUIStyle guiStyle = GUIUtil.GetGuiStyle(aGroupButtonData.elements[num3].style);
				GUIContent content = GUIUtil.CreateGuiContent(aGroupButtonData.elements[num3].content);
				bool flag2 = false;
				if ((guiStyle != null) ? GUILayout.Button(content, guiStyle, options) : GUILayout.Button(content, options))
				{
					result = aGroupButtonData.elements[num3].buttonId;
				}
				GUILayout.Space(space.x);
				num2++;
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(space.y);
		}
		GUILayout.EndVertical();
		GUILayout.EndArea();
		return result;
	}

	public static int HorizontalGroupButtons(Vector3 aReferencePos, GUIDefines.GroupButtonData aGroupButtonData)
	{
		int result = -1;
		GUIStyle guiStyle = GUIUtil.GetGuiStyle(aGroupButtonData.style);
		Vector2 space = GUIUtil.GetSpace(aGroupButtonData.space);
		Rect screenRect = GUIUtil.ConvertToRelativePos(aReferencePos, aGroupButtonData.area);
		GUILayout.BeginArea(screenRect);
		for (int i = 0; i < aGroupButtonData.elements.Length; i++)
		{
			GUILayout.BeginHorizontal();
			GUIContent content = GUIUtil.CreateGuiContent(aGroupButtonData.elements[i].content);
			bool flag = false;
			if ((guiStyle != null) ? GUILayout.Button(content, guiStyle) : GUILayout.Button(content))
			{
				result = aGroupButtonData.elements[i].buttonId;
			}
			GUILayout.Space(space.x);
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea();
		return result;
	}

	public static int VertialGroupButtons(Vector3 aReferencePos, GUIDefines.GroupButtonData aGroupButtonData)
	{
		int result = -1;
		GUIStyle guiStyle = GUIUtil.GetGuiStyle(aGroupButtonData.style);
		Vector2 space = GUIUtil.GetSpace(aGroupButtonData.space);
		Rect screenRect = GUIUtil.ConvertToRelativePos(aReferencePos, aGroupButtonData.area);
		GUILayout.BeginArea(screenRect);
		for (int i = 0; i < aGroupButtonData.elements.Length; i++)
		{
			GUILayout.BeginVertical();
			GUIContent content = GUIUtil.CreateGuiContent(aGroupButtonData.elements[i].content);
			bool flag = false;
			if ((guiStyle != null) ? GUILayout.Button(content, guiStyle) : GUILayout.Button(content))
			{
				result = aGroupButtonData.elements[i].buttonId;
			}
			GUILayout.Space(space.y);
			GUILayout.EndVertical();
		}
		GUILayout.EndArea();
		return result;
	}

	public static void Textures(Vector3 aReferencePos, GUIDefines.TextureData[] aTextureData)
	{
		for (int i = 0; i < aTextureData.Length; i++)
		{
			if (aTextureData[i].invisible || aTextureData[i].icon.image == null)
			{
				continue;
			}
			if (aTextureData[i].bgInfo != null)
			{
				GUIUtil.ApplyBgColor(aTextureData[i].bgInfo.useBgColor, aTextureData[i].bgInfo.bgColor, false);
			}
			Matrix4x4 matrix = GUI.matrix;
			Rect position = GUIUtil.ConvertToRelativePos(aReferencePos, aTextureData[i].pos);
			if (aTextureData[i].rotate != GUIDefines.RotateDirection.eNone)
			{
				Vector2 pivotPoint = new Vector2(position.xMin + position.width / 2f, position.yMin + position.height / 2f);
				pivotPoint.x *= (float)Screen.width / GUIConstants.kReferenceScreenWidth;
				pivotPoint.y *= (float)Screen.height / GUIConstants.kReferenceScreenHeight;
				if (aTextureData[i].pivotPointOffset != null)
				{
					Vector2 space = GUIUtil.GetSpace(aTextureData[i].pivotPointOffset);
					pivotPoint.x += space.x;
					pivotPoint.y += space.y;
				}
				GUIUtility.RotateAroundPivot(aTextureData[i].rotateAngle, pivotPoint);
			}
			if (aTextureData[i].tiled)
			{
				float width = aTextureData[i].tileSize.inPixel.width;
				float height = aTextureData[i].tileSize.inPixel.height;
				Rect position2 = new Rect(0f, 0f, width, height);
				for (float num = 0f; num < position.width; num += width)
				{
					for (float num2 = 0f; num2 < position.height; num2 += height)
					{
						position2.x = position.xMin + num;
						position2.y = position.yMin + num2;
						GUI.DrawTexture(position2, aTextureData[i].icon.image);
					}
				}
			}
			else
			{
				GUI.DrawTexture(position, aTextureData[i].icon.image);
			}
			if (aTextureData[i].rotate != GUIDefines.RotateDirection.eNone)
			{
				switch (aTextureData[i].rotate)
				{
				case GUIDefines.RotateDirection.eClockwise:
					aTextureData[i].rotateAngle += 4f;
					break;
				case GUIDefines.RotateDirection.eCounterClockwise:
					aTextureData[i].rotateAngle -= 4f;
					break;
				}
				GUI.matrix = matrix;
			}
			if (aTextureData[i].bgInfo != null)
			{
				GUIUtil.RestoreBgColor(aTextureData[i].bgInfo.useBgColor, false);
			}
		}
	}

	public static void FullScreenTexture(GUIDefines.TextureInfo aTextureInfo)
	{
		Rect position = GUIUtil.FullScreenRect();
		GUI.DrawTexture(position, aTextureInfo.image);
	}

	public static void Labels(Vector3 aReferencePos, GUIDefines.LabelData[] aLabelData)
	{
		for (int i = 0; i < aLabelData.Length; i++)
		{
			if (aLabelData[i].invisible)
			{
				continue;
			}
			if (aLabelData[i].bgInfo != null)
			{
				GUIUtil.ApplyBgColor(aLabelData[i].bgInfo.useBgColor, aLabelData[i].bgInfo.bgColor, true);
			}
			Rect position = GUIUtil.ConvertToRelativePos(aReferencePos, aLabelData[i].pos);
			Rect position2 = default(Rect);
			GUIContent gUIContent = GUIUtil.CreateGuiContent(aLabelData[i].content);
			GUIStyle gUIStyle = GUIUtil.GetGuiStyle(aLabelData[i].style);
			if (gUIStyle == null)
			{
				gUIStyle = GUI.skin.label;
			}
			GUIStyle gUIStyle2 = null;
			gUIStyle2 = ((aLabelData[i].style == null || !aLabelData[i].style.useCustomDropShadowColor) ? GUIUtil.CreateDropShadowTextStyleForLabel(gUIStyle) : GUIUtil.CreateCustomDropShadowTextStyleForLabel(gUIStyle, aLabelData[i].style.customDropShadowColor));
			GUIStyle gUIStyle3 = null;
			gUIStyle3 = ((!aLabelData[i].disableDropShadow) ? GUIUtil.CreateFrontTextStyle(gUIStyle) : GUIUtil.CreateFrontTextStyleWithNoDropShadow(gUIStyle));
			if (aLabelData[i].disableDropShadow)
			{
				GUI.Label(position, gUIContent, gUIStyle3);
			}
			else
			{
				float num = 0f;
				float num2 = 0f;
				if (aLabelData[i].style != null && aLabelData[i].style.useCustomDropShadowOffset)
				{
					num = aLabelData[i].style.customDropShadowOffset.x;
					num2 = aLabelData[i].style.customDropShadowOffset.y;
					if (ResolutionManager.Instance.LayoutSize == ResolutionManager.eLayoutSize.eLowres)
					{
						num *= 0.5f;
						num2 *= 0.5f;
					}
				}
				else
				{
					num = GameFlowManager.Instance.GUIManager.DropShadowOffsetX;
					num2 = GameFlowManager.Instance.GUIManager.DropShadowOffsetY;
				}
				position2.xMin = position.xMin + num;
				position2.yMin = position.yMin + num2;
				position2.xMax = position.xMax + num;
				position2.yMax = position.yMax + num2;
				GUI.Label(position2, gUIContent, gUIStyle2);
				gUIContent.image = null;
				GUI.Label(position, gUIContent, gUIStyle3);
			}
			if (aLabelData[i].bgInfo != null)
			{
				GUIUtil.RestoreBgColor(aLabelData[i].bgInfo.useBgColor, true);
			}
		}
	}

	public static string[] TextFields(Vector3 aReferencePos, GUIDefines.TextFieldData[] aTextFieldData, bool aIsControlBlocked)
	{
		string[] array = new string[aTextFieldData.Length];
		for (int i = 0; i < aTextFieldData.Length; i++)
		{
			array[i] = string.Empty;
			Rect position = GUIUtil.ConvertToRelativePos(aReferencePos, aTextFieldData[i].pos);
			GUIUtil.SetControlName(aTextFieldData[i].controlName);
			bool flag = aTextFieldData[i].isFocused;
			bool flag2 = false;
			bool flag3 = false;
			GUIStyle guiStyle = GUIUtil.GetGuiStyle(aTextFieldData[i].style);
			if (guiStyle != null)
			{
				GUI.skin.textField.normal.background = guiStyle.normal.background;
				GUI.skin.textField.hover.background = guiStyle.normal.background;
				GUI.skin.textField.active.background = guiStyle.active.background;
				GUI.skin.textField.normal.textColor = GUIConstants.kGreyColor;
				GUI.skin.textField.active.textColor = guiStyle.active.textColor;
				GUI.skin.textField.focused.textColor = guiStyle.focused.textColor;
				GUI.skin.settings.cursorColor = Color.clear;
				GUI.skin.settings.selectionColor = GUIConstants.kGreyColor;
				if (flag)
				{
					GUI.skin.textField.normal.background = GUI.skin.textField.focused.background;
					GUI.skin.textField.hover.background = GUI.skin.textField.focused.background;
				}
			}
			GUIStyle gUIStyle = new GUIStyle(GUI.skin.textField);
			gUIStyle.normal.background = null;
			GUIStyle gUIStyle2 = new GUIStyle(gUIStyle);
			gUIStyle2.normal.textColor = guiStyle.focused.textColor;
			string text = string.Empty;
			string text2 = string.Empty;
			if (aTextFieldData[i].editedText != null && aTextFieldData[i].editedText.Length > 0)
			{
				text2 = aTextFieldData[i].editedText;
			}
			else if (aTextFieldData[i].defaultTextId != null && aTextFieldData[i].defaultTextId.Length > 0 && (!flag || !TouchScreenKeyboard.visible))
			{
				text = LocalizationManager.Instance.GetString(aTextFieldData[i].defaultTextId);
			}
			if (aTextFieldData[i].isReadOnly)
			{
				array[i] = text2;
				GUI.Label(position, text2, GUI.skin.textField);
			}
			else
			{
				GUI.skin.textField.normal.textColor = Color.clear;
				GUI.skin.textField.hover.textColor = Color.clear;
				GUI.skin.textField.active.textColor = Color.clear;
				GUI.skin.textField.focused.textColor = Color.clear;
				if (true)
				{
					flag2 = GUI.Button(position, string.Empty, GUI.skin.textField);
				}
				else
				{
					GUI.Label(position, string.Empty, GUI.skin.textField);
				}
				if (flag2 && !aIsControlBlocked)
				{
					if (m_cKeyboard != null)
					{
						m_cKeyboard.active = false;
						m_cKeyboard = null;
						float realtimeSinceStartup = Time.realtimeSinceStartup;
						while (Time.realtimeSinceStartup - realtimeSinceStartup < 0.2f)
						{
						}
					}
					m_cKeyboard = TouchScreenKeyboard.Open(text2, aTextFieldData[i].keyboardType, false, false, true);
					Utilities.AssertMsg(m_cKeyboard != null, "Fail to create keyboard!");
					if (!flag)
					{
						flag = true;
						aTextFieldData[i].isFocused = true;
						for (int j = 0; j < aTextFieldData.Length; j++)
						{
							if (i != j)
							{
								aTextFieldData[j].isFocused = false;
							}
						}
					}
				}
				if (TouchScreenKeyboard.visible && m_cKeyboard != null && m_cKeyboard.active && flag)
				{
					array[i] = m_cKeyboard.text;
					if (array[i].Length > aTextFieldData[i].maxLength)
					{
						array[i] = array[i].Substring(0, aTextFieldData[i].maxLength);
						m_cKeyboard.text = array[i];
					}
					if (aTextFieldData[i].titleCase)
					{
						string text3 = array[i];
						array[i] = m_TextInfo.ToTitleCase(array[i]);
						if (text3 != array[i])
						{
							m_cKeyboard.text = array[i];
						}
					}
					flag3 = true;
				}
			}
			if (flag3)
			{
				if (aTextFieldData[i].isPassword)
				{
					if (array[i].Length > 0)
					{
						string text4 = GUIUtil.MaskPassword(ref aTextFieldData[i], array[i]);
						if (text4.Length > 0)
						{
							string stringToDisplay = GUIUtil.GetStringToDisplay(text4, gUIStyle2, position.width, true);
							GUI.Label(position, stringToDisplay, gUIStyle2);
							aTextFieldData[i].maskedPassword = text4.Remove(text4.Length - 1, 1) + '*';
						}
					}
					else
					{
						aTextFieldData[i].maskedPassword = string.Empty;
						string stringToDisplay2 = GUIUtil.GetStringToDisplay(aTextFieldData[i].maskedPassword, gUIStyle2, position.width, true);
						GUI.Label(position, stringToDisplay2, gUIStyle2);
					}
				}
				else
				{
					string stringToDisplay3 = GUIUtil.GetStringToDisplay(array[i], gUIStyle2, position.width, true);
					GUI.Label(position, stringToDisplay3, gUIStyle2);
				}
				aTextFieldData[i].editedText = array[i];
			}
			else if (text2.Length == 0 && text.Length > 0)
			{
				GUI.Label(position, text, gUIStyle);
			}
			else if (aTextFieldData[i].isPassword)
			{
				GUI.Label(position, aTextFieldData[i].maskedPassword, gUIStyle2);
				aTextFieldData[i].timeOfNukedPassword = 0f;
			}
			else
			{
				string stringToDisplay4 = GUIUtil.GetStringToDisplay(text2, gUIStyle2, position.width, false);
				GUI.Label(position, stringToDisplay4, gUIStyle2);
			}
		}
		if (TouchScreenKeyboard.visible)
		{
			bool flag4 = GUI.Button(GUIUtil.FullScreenRect(), string.Empty);
			if (m_cKeyboard != null && flag4)
			{
				m_cKeyboard.active = false;
				m_cKeyboard = null;
			}
		}
		return array;
	}

	public static int HorizontalRadioButtons(Vector3 aReferencePos, GUIDefines.RadioButtonData aRadioButtonData)
	{
		int num = -1;
		bool[] array = new bool[aRadioButtonData.isOn.Length];
		GUIStyle guiStyle = GUIUtil.GetGuiStyle(aRadioButtonData.style);
		Vector2 space = GUIUtil.GetSpace(aRadioButtonData.space);
		Rect screenRect = GUIUtil.ConvertToRelativePos(aReferencePos, aRadioButtonData.area);
		GUILayout.BeginArea(screenRect);
		GUILayout.BeginHorizontal();
		for (int i = 0; i < aRadioButtonData.isOn.Length; i++)
		{
			bool flag = false;
			flag = ((guiStyle != null) ? GUILayout.Toggle(aRadioButtonData.isOn[i], string.Empty, guiStyle) : GUILayout.Toggle(aRadioButtonData.isOn[i], string.Empty));
			array[i] = flag;
			GUILayout.Space(space.x);
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		for (int j = 0; j < aRadioButtonData.isOn.Length; j++)
		{
			if (aRadioButtonData.isOn[j] != array[j] && array[j])
			{
				num = j;
				aRadioButtonData.isOn[j] = true;
			}
		}
		if (num != -1)
		{
			for (int k = 0; k < aRadioButtonData.isOn.Length; k++)
			{
				if (k != num)
				{
					aRadioButtonData.isOn[k] = false;
				}
			}
		}
		return num;
	}

	public static void HorizontalUnClickableRadioButtons(Vector3 aReferencePos, GUIDefines.UnClickableRadioButtonData aUnClickableRadioButtonData, int aCurrentOn)
	{
		Vector2 space = GUIUtil.GetSpace(aUnClickableRadioButtonData.space);
		Vector2 space2 = GUIUtil.GetSpace(aUnClickableRadioButtonData.onPadding);
		Vector2 space3 = GUIUtil.GetSpace(aUnClickableRadioButtonData.offPadding);
		Rect screenRect = GUIUtil.ConvertToRelativePos(aReferencePos, aUnClickableRadioButtonData.area);
		GUILayout.BeginArea(screenRect);
		GUILayout.BeginHorizontal();
		for (int i = 0; i < aUnClickableRadioButtonData.count; i++)
		{
			GUILayout.BeginVertical();
			if (i == aCurrentOn)
			{
				GUILayout.Space(space2.y);
				GUILayout.Label(aUnClickableRadioButtonData.on.image);
			}
			else
			{
				GUILayout.Space(space3.y);
				GUILayout.Label(aUnClickableRadioButtonData.off.image);
			}
			GUILayout.EndVertical();
			GUILayout.Space(space.x);
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	public static void Window(Vector3 aReferencePos, GUIDefines.WindowData aWindowData, GUI.WindowFunction aFunction)
	{
		Rect clientRect = GUIUtil.ConvertToRelativePos(aReferencePos, aWindowData.pos);
		GUIStyle guiStyle = GUIUtil.GetGuiStyle(aWindowData.style);
		if (guiStyle == null)
		{
			GUI.Window(aWindowData.id, clientRect, aFunction, string.Empty);
		}
		else
		{
			GUI.Window(aWindowData.id, clientRect, aFunction, string.Empty, guiStyle);
		}
	}

	public static void Windows(Vector3 aReferencePos, GUIDefines.WindowData[] aWindowData, GUI.WindowFunction aFunction)
	{
		for (int i = 0; i < aWindowData.Length; i++)
		{
			Window(aReferencePos, aWindowData[i], aFunction);
		}
	}
}
