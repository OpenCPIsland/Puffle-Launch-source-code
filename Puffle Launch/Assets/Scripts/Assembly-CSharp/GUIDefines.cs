using UnityEngine;

public static class GUIDefines
{
	public enum FontSize
	{
		eMini = 0,
		eSmall = 1,
		eMedium = 2,
		eLarge = 3,
		eFontSize_COUNT = 4
	}

	public enum FontType
	{
		eOnDemand = -1,
		eCPMenus = 0,
		eInGame = 1,
		eFontType_COUNT = 2
	}

	public enum ScreenResolution
	{
		eOriginal = 0,
		eLow = 1,
		eIPad = 2,
		eScreenResolution_COUNT = 3
	}

	public enum RotateDirection
	{
		eNone = 0,
		eClockwise = 1,
		eCounterClockwise = 2,
		eRotateDirection_COUNT = 3
	}

	public enum AutoResizeAllignment
	{
		eAuto = 0,
		eCenter = 1,
		eLeft = 2,
		eRight = 3,
		eAutoResizeAllignment_COUNT = 4
	}

	public class BackgroundInfo
	{
		public bool useBgColor;

		public Color bgColor;
	}

	public class RectIPadInfo
	{
		public bool keepSizeRatio;

		public bool keepWidthRatio;

		public float leftOffset;

		public float topOffset;

		public float widthScale;

		public float heightScale;

		public bool useLeftRefRatio;

		public float leftRefRatio;

		public bool useTopRefRatio;

		public float topRefRatio;

		public float leftRefOffset;

		public float topRefOffset;

		public bool enableDebug;

		public RectIPadInfo()
		{
			Init();
		}

		public RectIPadInfo(RectIPadInfo aIpad)
		{
			keepSizeRatio = aIpad.keepSizeRatio;
			keepWidthRatio = aIpad.keepWidthRatio;
			leftOffset = aIpad.leftOffset;
			topOffset = aIpad.topOffset;
			widthScale = aIpad.widthScale;
			heightScale = aIpad.heightScale;
			useLeftRefRatio = aIpad.useLeftRefRatio;
			leftRefRatio = aIpad.leftRefRatio;
			useTopRefRatio = aIpad.useTopRefRatio;
			topRefRatio = aIpad.topRefRatio;
			leftRefOffset = aIpad.leftRefOffset;
			topRefOffset = aIpad.topRefOffset;
			enableDebug = aIpad.enableDebug;
			Init();
		}

		public void Init()
		{
		}
	}

	public class RectInfo
	{
		public float leftRatio;

		public float topRatio;

		public float widthRatio;

		public float heightRatio;

		public bool useOriginalWidth;

		public bool useOriginalHeight;

		public RectIPadInfo IPad;

		public Rect inPixel;

		public bool detatchFromRefObject;

		public bool useAnchor;

		public RectInfo anchor;

		public ContentInfo anchorContent;

		public StyleInfo anchorStyle;

		public bool enableDebug;

		public RectInfo()
		{
			Init();
		}

		public RectInfo(RectInfo aInfo)
		{
			leftRatio = aInfo.leftRatio;
			topRatio = aInfo.topRatio;
			widthRatio = aInfo.widthRatio;
			heightRatio = aInfo.heightRatio;
			useOriginalWidth = aInfo.useOriginalWidth;
			useOriginalHeight = aInfo.useOriginalHeight;
			if (aInfo.IPad != null)
			{
				IPad = new RectIPadInfo(aInfo.IPad);
			}
			enableDebug = aInfo.enableDebug;
			Init();
		}

		public void Init()
		{
			bool aKeepSizeRatioOnIPad = false;
			bool aKeepWidthRatioOnIpad = false;
			if (IPad != null)
			{
				IPad.Init();
				aKeepSizeRatioOnIPad = IPad.keepSizeRatio;
				aKeepWidthRatioOnIpad = IPad.keepWidthRatio;
			}
			if (useAnchor)
			{
				anchor.Init();
				inPixel = GUIUtil.ConvertRatioToPixel(leftRatio, topRatio, widthRatio, heightRatio, aKeepSizeRatioOnIPad, aKeepWidthRatioOnIpad);
				inPixel.x += anchor.inPixel.x;
				inPixel.y += anchor.inPixel.y;
				float x = GUIUtil.GetGuiStyle(anchorStyle).CalcSize(GUIUtil.CreateGuiContent(anchorContent)).x;
				inPixel.x += x;
				float num = 0f;
				if (anchorStyle.customTextAlignment == TextAnchor.LowerCenter || anchorStyle.customTextAlignment == TextAnchor.MiddleCenter || anchorStyle.customTextAlignment == TextAnchor.UpperCenter)
				{
					num = (anchor.inPixel.xMax - anchor.inPixel.xMin - x) * 0.5f;
				}
				else if (anchorStyle.customTextAlignment == TextAnchor.LowerRight || anchorStyle.customTextAlignment == TextAnchor.MiddleRight || anchorStyle.customTextAlignment == TextAnchor.UpperRight)
				{
					num = anchor.inPixel.xMax - anchor.inPixel.xMin - x;
				}
				inPixel.x += num;
			}
			else
			{
				inPixel = GUIUtil.ConvertRatioToPixel(leftRatio, topRatio, widthRatio, heightRatio, aKeepSizeRatioOnIPad, aKeepWidthRatioOnIpad);
			}
			if (ResolutionManager.Instance.LayoutSize == ResolutionManager.eLayoutSize.eIPad)
			{
				if (IPad == null)
				{
					IPad = new RectIPadInfo();
				}
				inPixel = GUIUtil.ApplyIPadAdjustment(inPixel, leftRatio, topRatio, IPad);
			}
		}
	}

	public class Vector2IPadInfo
	{
		public float xOffset;

		public float yOffset;

		public bool enableDebug;

		public void Init()
		{
		}
	}

	public class Vector2Info
	{
		public float xRatio;

		public float yRatio;

		public bool useOriginalWidth;

		public bool useOriginalHeight;

		public Vector2IPadInfo IPad;

		public Vector2 inPixel;

		public bool setPixelsDirectly;

		public bool enableDebug;

		public void Init()
		{
			if (setPixelsDirectly)
			{
				return;
			}
			if (IPad != null)
			{
				IPad.Init();
			}
			inPixel = GUIUtil.ConvertRatioToPixel(xRatio, yRatio);
			if (ResolutionManager.Instance.LayoutSize == ResolutionManager.eLayoutSize.eIPad)
			{
				if (IPad == null)
				{
					IPad = new Vector2IPadInfo();
				}
				GUIUtil.ApplyIPadAdjustment(ref inPixel, xRatio, yRatio, IPad);
			}
		}
	}

	public class ButtonElementInfo
	{
		public int buttonId;

		public ContentInfo content;

		public StyleInfo style;

		public void Init()
		{
			if (content != null)
			{
				content.Init();
			}
			if (style != null)
			{
				style.Init();
			}
		}
	}

	public class GenerateElementInfo
	{
		public bool enable;

		public int elementCount;

		public string iconNamePrefix;

		public int iconIndexStartAt;

		public void Init()
		{
		}
	}

	public class MultiPageInfo
	{
		public int elementPerRow;

		public int elementPerCol;

		public int totalPage;

		public void Init()
		{
		}
	}

	public class Texture2DInfo
	{
		public string name;

		public Texture2D image;

		public bool isLocalized;

		public void Init()
		{
			string text = string.Empty;
			if (name == null || name.Length <= 0)
			{
				return;
			}
			string text2 = name;
			switch (ResolutionManager.Instance.AssetResolution)
			{
			case ResolutionManager.eAssetResolution.eLowres:
				text2 = name + "_lowres";
				break;
			case ResolutionManager.eAssetResolution.eIPad:
				text2 = name + "_iPad";
				break;
			}
			if (isLocalized)
			{
				switch (LocalizationManager.GetLanguageCode())
				{
				case "fr":
					text = "_fr";
					break;
				case "es":
					text = "_es";
					break;
				case "pt":
					text = "_pt";
					break;
				default:
					text = "_en";
					break;
				}
			}
			text2 += text;
			image = GUIUtil.LoadTexture2D(text2);
			if (image == null && ResolutionManager.Instance.AssetResolution == ResolutionManager.eAssetResolution.eLowres)
			{
				Debug.Log("Fail to load alternate Texture2D: " + text2 + ". Loading original Texture2D: " + name);
			}
			if (!(image != null))
			{
				if (isLocalized)
				{
					image = GUIUtil.LoadTexture2D(name + text);
				}
				else
				{
					image = GUIUtil.LoadTexture2D(name);
				}
				Utilities.AssertMsg(image != null, "Fail to load Texture2D: " + name);
			}
		}
	}

	public class TextureInfo
	{
		public string name;

		public Texture image;

		public void Init()
		{
			if (name != null && name.Length > 0)
			{
				string text = name;
				switch (ResolutionManager.Instance.AssetResolution)
				{
				case ResolutionManager.eAssetResolution.eLowres:
					text = name + "_lowres";
					break;
				case ResolutionManager.eAssetResolution.eIPad:
					text = name + "_iPad";
					break;
				}
				image = GUIUtil.LoadTexture(text);
				if (image == null && ResolutionManager.Instance.AssetResolution == ResolutionManager.eAssetResolution.eLowres)
				{
					Debug.Log("Fail to load alternate Texture2D: " + text + ". Loading original Texture2D: " + name);
				}
				if (!(image != null))
				{
					image = GUIUtil.LoadTexture(name);
					Utilities.AssertMsg(image != null, "Fail to load Texture: " + name);
				}
			}
		}
	}

	public class StyleInfo
	{
		public string styleName;

		public GUIStyle defaultStyle;

		public bool useCustomStyle;

		public Texture2DInfo customNormal;

		public Texture2DInfo customActive;

		public FontSize customFontSize;

		public FontType customFontType;

		public string customOnDemandFontName;

		public Vector2Info customPadding;

		public Vector2Info customPadding2;

		public Color customNormalTextColor;

		public Color customActiveTextColor;

		public Color customFocusedTextColor;

		public bool useCustomTextAlignment;

		public TextAnchor customTextAlignment;

		public bool customWordWrap;

		public ImagePosition customImagePosition;

		public bool useCustomDropShadowColor;

		public Color customDropShadowColor;

		public bool useCustomDropShadowOffset;

		public Vector2 customDropShadowOffset;

		public void Init()
		{
			if (customNormal != null)
			{
				customNormal.Init();
			}
			if (customActive != null)
			{
				customActive.Init();
			}
			if (customPadding != null)
			{
				customPadding.Init();
			}
			if (customPadding2 != null)
			{
				customPadding2.Init();
			}
			if (customNormalTextColor.a == 0f)
			{
				customNormalTextColor = GUIConstants.kWhiteColor;
			}
			if (customActiveTextColor.a == 0f)
			{
				customActiveTextColor = GUIConstants.kWhiteColor;
				customActiveTextColor.a = 0.5f;
			}
			if (customFocusedTextColor.a == 0f)
			{
				customFocusedTextColor = GUIConstants.kWhiteColor;
			}
			if (!useCustomTextAlignment)
			{
				customTextAlignment = TextAnchor.MiddleCenter;
			}
		}
	}

	public class ContentInfo
	{
		public string textId;

		public string text;

		public string prefixTextId;

		public string prefixText;

		public string suffixTextId;

		public string suffixText;

		public TextureInfo icon;

		public void Init()
		{
			if (icon != null)
			{
				icon.Init();
			}
		}
	}

	public class AutoResizeData
	{
		public int groupId;

		public int index;

		public Rect pos;
	}

	public class ButtonData
	{
		public int buttonId;

		public RectInfo pos;

		public float detectZoneScale;

		public ContentInfo content;

		public StyleInfo style;

		public bool invisible;

		public bool isTogglable;

		public bool toggleState;

		public bool isControlBlocked;

		public bool isAutoResizeOff;

		public bool useAutoResizeGroup;

		public int autoResizeGroupId;

		public AutoResizeAllignment autoResizeAllignment;

		public void Init()
		{
			pos.Init();
			if (content != null)
			{
				content.Init();
			}
			if (style != null)
			{
				style.Init();
			}
			pos.inPixel = GUIUtil.ApplyStyleBgOriginalSize(pos, style);
		}
	}

	public class GroupButtonData
	{
		public RectInfo area;

		public Vector2Info size;

		public Vector2Info space;

		public StyleInfo style;

		public MultiPageInfo multiPage;

		public ButtonElementInfo[] elements;

		public GenerateElementInfo autoGenerate;

		public void Init()
		{
			if (autoGenerate != null && autoGenerate.enable)
			{
				GenerateElements();
			}
			if (area.IPad == null)
			{
				area.IPad = new RectIPadInfo();
			}
			area.IPad.keepSizeRatio = true;
			area.Init();
			size.Init();
			space.Init();
			if (style != null)
			{
				style.Init();
			}
			if (multiPage != null)
			{
				multiPage.Init();
			}
			for (int i = 0; i < elements.Length; i++)
			{
				elements[i].Init();
			}
			if (elements.Length > 0)
			{
				if (elements[0].content != null)
				{
					size.inPixel = GUIUtil.ApplyTextureOriginalSize(size, elements[0].content.icon);
				}
				multiPage.totalPage = Mathf.CeilToInt((float)elements.Length / (float)(multiPage.elementPerCol * multiPage.elementPerRow));
			}
		}

		private void GenerateElements()
		{
			if (autoGenerate.elementCount <= 0)
			{
				return;
			}
			elements = new ButtonElementInfo[autoGenerate.elementCount];
			for (int i = 0; i < autoGenerate.elementCount; i++)
			{
				elements[i] = new ButtonElementInfo();
				elements[i].buttonId = i + autoGenerate.iconIndexStartAt;
				if (autoGenerate.iconNamePrefix != null && autoGenerate.iconNamePrefix.Length > 0)
				{
					int num = i;
					elements[i].content = new ContentInfo
					{
						icon = new TextureInfo
						{
							name = autoGenerate.iconNamePrefix + num
						}
					};
				}
			}
		}
	}

	public class TextureData
	{
		public RectInfo pos;

		public TextureInfo icon;

		public BackgroundInfo bgInfo;

		public bool invisible;

		public RotateDirection rotate;

		public Vector2Info pivotPointOffset;

		public float rotateAngle;

		public bool tiled;

		public RectInfo tileSize;

		public void Init()
		{
			pos.Init();
			if (tiled)
			{
				tileSize.Init();
			}
			if (icon != null)
			{
				icon.Init();
			}
			if (pivotPointOffset != null)
			{
				pivotPointOffset.Init();
			}
			pos.inPixel = GUIUtil.ApplyTextureOriginalSize(pos, icon);
		}
	}

	public class LabelData
	{
		public RectInfo pos;

		public ContentInfo content;

		public StyleInfo style;

		public BackgroundInfo bgInfo;

		public bool invisible;

		public bool disableDropShadow;

		public void Init()
		{
			pos.Init();
			if (content != null)
			{
				content.Init();
			}
			if (style != null)
			{
				style.Init();
			}
			pos.inPixel = GUIUtil.ApplyStyleBgOriginalSize(pos, style);
		}
	}

	public class TextFieldData
	{
		public string controlName;

		public RectInfo pos;

		public bool isPassword;

		public bool isReadOnly;

		public int maxLength;

		public StyleInfo style;

		public string defaultTextId;

		public TouchScreenKeyboardType keyboardType;

		public bool titleCase;

		public string editedText;

		public float timeOfNukedPassword;

		public string maskedPassword;

		public bool isFocused;

		public void Init()
		{
			pos.Init();
			if (style != null)
			{
				style.Init();
			}
			pos.inPixel = GUIUtil.ApplyStyleBgOriginalSize(pos, style);
			editedText = string.Empty;
			maskedPassword = string.Empty;
			isFocused = false;
		}
	}

	public class RadioButtonData
	{
		public RectInfo area;

		public Vector2Info space;

		public int count;

		public int defaultOn;

		public StyleInfo style;

		public bool[] isOn;

		public void Init()
		{
			if (area.IPad == null)
			{
				area.IPad = new RectIPadInfo();
			}
			area.IPad.keepSizeRatio = true;
			area.Init();
			space.Init();
			if (style != null)
			{
				style.Init();
			}
			isOn = new bool[count];
			for (int i = 0; i < isOn.Length; i++)
			{
				if (i == defaultOn)
				{
					isOn[i] = true;
				}
				else
				{
					isOn[i] = false;
				}
			}
		}
	}

	public class UnClickableRadioButtonData
	{
		public RectInfo area;

		public Vector2Info space;

		public TextureInfo on;

		public Vector2Info onPadding;

		public TextureInfo off;

		public Vector2Info offPadding;

		public int count;

		public void Init()
		{
			if (area.IPad == null)
			{
				area.IPad = new RectIPadInfo();
			}
			area.IPad.keepSizeRatio = true;
			area.Init();
			space.Init();
			on.Init();
			if (onPadding != null)
			{
				onPadding.Init();
			}
			off.Init();
			if (offPadding != null)
			{
				offPadding.Init();
			}
		}
	}

	public class WindowData
	{
		public RectInfo pos;

		public int id;

		public StyleInfo style;

		public bool respectIpadSizeRatio;

		public void Init()
		{
			if (!respectIpadSizeRatio)
			{
				if (pos.IPad == null)
				{
					pos.IPad = new RectIPadInfo();
				}
				pos.IPad.keepSizeRatio = true;
			}
			pos.Init();
			if (style != null)
			{
				style.Init();
			}
			pos.inPixel = GUIUtil.ApplyStyleBgOriginalSize(pos, style);
		}
	}

	public class PageControlData
	{
		public GameObject refObj;

		public Transform refTransform;

		public int firstInPage;

		public int PageNumber;
	}
}
