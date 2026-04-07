using UnityEngine;

public class AutoAdjustSpriteText : MonoBehaviour
{
	public enum SpriteTextSize
	{
		eUseSizeGivenInSpriteText = -1,
		eMini = 0,
		eSmall = 1,
		eMedium = 2,
		eLarge = 3
	}

	public struct SizeData
	{
		public float characterSize;

		public float lineSpacing;
	}

	public enum SpriteTextColor
	{
		eUseColorGivenInSpriteText = -1,
		eWhite = 0,
		eBlack = 1,
		eOrange = 2,
		eLightBrown = 3,
		eDarkBrown = 4,
		ePaintBrown = 5,
		eBlue = 6,
		eGrey = 7,
		eGreyBrown = 8,
		eYellow = 9
	}

	public static SizeData[] sm_SpriteSizeData = new SizeData[4]
	{
		new SizeData
		{
			characterSize = 0.5f,
			lineSpacing = 0.5f
		},
		new SizeData
		{
			characterSize = 0.8f,
			lineSpacing = 0.6f
		},
		new SizeData
		{
			characterSize = 1f,
			lineSpacing = 0.8f
		},
		new SizeData
		{
			characterSize = 1.791534f,
			lineSpacing = 1.1f
		}
	};

	public static Color[] sm_SpriteTextColorList = new Color[10]
	{
		Color.white,
		Color.black,
		new Color(0.4549f, 0.22353f, 0.08235f, 1f),
		new Color(0.45490196f, 0.36862746f, 0.3254902f, 1f),
		new Color(0.3019608f, 0.23921569f, 11f / 51f, 1f),
		new Color(0.6549f, 0.56863f, 0.52549f, 1f),
		new Color(0.17647f, 0.26667f, 0.46275f, 1f),
		new Color(0.72941f, 0.72941f, 0.72941f, 1f),
		new Color(39f / 64f, 71f / 128f, 0.52734375f, 1f),
		new Color(0.97647f, 0.95686f, 0.41961f, 1f)
	};

	public SpriteTextSize m_SpriteTextSize = SpriteTextSize.eMedium;

	public SpriteTextColor m_SpriteTextColor;

	private SpriteText m_SpriteText;

	public void Awake()
	{
		m_SpriteText = GetComponent<SpriteText>();
		Utilities.AssertMsg(m_SpriteText != null, "No sprite text is found in " + base.gameObject);
	}

	public void AutoAdjust()
	{
		if (!(m_SpriteText == null))
		{
			AutoSize(ref m_SpriteText);
			SetColor(ref m_SpriteText);
		}
	}

	private void AutoSize(ref SpriteText aSpriteText)
	{
		if (aSpriteText == null)
		{
			return;
		}
		SpriteTextSize spriteTextSize = m_SpriteTextSize;
		if (spriteTextSize != SpriteTextSize.eUseSizeGivenInSpriteText)
		{
			int spriteTextSize2 = (int)m_SpriteTextSize;
			if (spriteTextSize2 >= 0 && spriteTextSize2 < sm_SpriteSizeData.Length)
			{
				aSpriteText.SetCharacterSize(sm_SpriteSizeData[spriteTextSize2].characterSize);
				aSpriteText.SetLineSpacing(sm_SpriteSizeData[spriteTextSize2].lineSpacing);
			}
		}
	}

	private void SetColor(ref SpriteText aSpriteText)
	{
		if (!(aSpriteText == null))
		{
			SpriteTextColor spriteTextColor = m_SpriteTextColor;
			if (spriteTextColor != SpriteTextColor.eUseColorGivenInSpriteText)
			{
				aSpriteText.SetColor(GetColor(m_SpriteTextColor));
			}
		}
	}

	public static Color GetColor(SpriteTextColor aColor)
	{
		if (aColor >= SpriteTextColor.eWhite && (int)aColor < sm_SpriteTextColorList.Length)
		{
			return sm_SpriteTextColorList[(int)aColor];
		}
		Utilities.AssertMsg(false, "Color not found for: " + aColor);
		return Color.clear;
	}
}
