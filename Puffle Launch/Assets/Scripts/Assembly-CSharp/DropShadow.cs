using UnityEngine;

public class DropShadow : MonoBehaviour
{
	public enum DropShadowOffset
	{
		eAuto = 0,
		eCustom = 1
	}

	public enum DropShadowColor
	{
		eAuto = -2,
		eCustom = -1,
		eBlack = 0,
		eWhite = 1,
		eBrown = 2
	}

	public static Color[] sm_DropShadowColorList = new Color[3]
	{
		Color.black,
		Color.white,
		new Color(53f / 85f, 0.5019608f, 0.44313726f, 1f)
	};

	public static Vector3 m_DefaultDropOffset = new Vector3(0.03f, -0.06f, 0.06f);

	public DropShadowOffset m_DropOffset;

	public Vector3 m_CustomOffset;

	public DropShadowColor m_DropColor = DropShadowColor.eAuto;

	public Color m_CustomColor;

	private SpriteText m_SourceSpriteText;

	private SpriteText m_CopySpriteText;

	private MeshRenderer m_SourceSpriteMeshRenderer;

	private MeshRenderer m_CopySpriteMeshRenderer;

	private bool m_IsShadowCreated;

	public void Awake()
	{
		m_SourceSpriteText = GetComponent<SpriteText>();
		Utilities.AssertMsg(m_SourceSpriteText != null, "Drop Shadow might not work because no source sprite text is found!");
		m_SourceSpriteMeshRenderer = m_SourceSpriteText.gameObject.GetComponent<MeshRenderer>();
		Utilities.AssertMsg(m_SourceSpriteMeshRenderer != null, "Source sprite text doesn't have a mesh renderer!");
	}

	public void Update()
	{
		if (m_IsShadowCreated)
		{
			if (m_SourceSpriteMeshRenderer.enabled != m_CopySpriteMeshRenderer.enabled)
			{
				HideDropShadowText(!m_SourceSpriteMeshRenderer.enabled);
			}
			else if (m_SourceSpriteText.Text != m_CopySpriteText.Text)
			{
				UpdateDropShadowText();
			}
		}
	}

	public void CreateShadow()
	{
		if (!m_IsShadowCreated && !(m_SourceSpriteText == null))
		{
			Vector3 dropShadowOffset = GetDropShadowOffset();
			GameObject gameObject = Object.Instantiate(Resources.Load("Prefabs/EZGUI/DropShadow"), dropShadowOffset, base.transform.rotation) as GameObject;
			gameObject.transform.parent = base.transform;
			m_CopySpriteText = gameObject.GetComponent<SpriteText>();
			m_CopySpriteText.Copy(m_SourceSpriteText);
			m_CopySpriteText.SetFont(m_SourceSpriteText.font, base.GetComponent<Renderer>().material);
			m_CopySpriteText.SetCharacterSize(m_SourceSpriteText.characterSize);
			m_CopySpriteText.CharacterSpacing = m_SourceSpriteText.CharacterSpacing;
			m_CopySpriteText.maxWidth = m_SourceSpriteText.maxWidth;
			m_CopySpriteText.Text = RemoveColorTags(m_SourceSpriteText.Text);
			Color dropShadowColor = GetDropShadowColor(m_SourceSpriteText.color, m_DropColor);
			m_CopySpriteText.SetColor(dropShadowColor);
			m_CopySpriteMeshRenderer = gameObject.GetComponent<MeshRenderer>();
			Utilities.AssertMsg(m_CopySpriteMeshRenderer != null, "Copy sprite text doesn't have a mesh renderer!");
			m_CopySpriteMeshRenderer.enabled = m_SourceSpriteMeshRenderer.enabled;
			m_IsShadowCreated = true;
		}
	}

	private Vector3 GetDropShadowOffset()
	{
		Vector3 position = base.transform.position;
		switch (m_DropOffset)
		{
		case DropShadowOffset.eAuto:
			position.x += m_DefaultDropOffset.x;
			position.y += m_DefaultDropOffset.y;
			position.z += m_DefaultDropOffset.z;
			break;
		case DropShadowOffset.eCustom:
			position.x += m_CustomOffset.x;
			position.y += m_CustomOffset.y;
			position.z += m_CustomOffset.z;
			break;
		}
		return position;
	}

	private Color GetDropShadowColor(Color aSpriteTextColor, DropShadowColor aDropColor)
	{
		Color result = sm_DropShadowColorList[0];
		switch (m_DropColor)
		{
		case DropShadowColor.eAuto:
			result = ((!aSpriteTextColor.Equals(GUIConstants.kDarkBrownColor)) ? ((!aSpriteTextColor.Equals(GUIConstants.kLightBrownColor)) ? ((!aSpriteTextColor.Equals(GUIConstants.kDarkerBrownColor)) ? ((!aSpriteTextColor.Equals(GUIConstants.kBlackColor)) ? sm_DropShadowColorList[0] : sm_DropShadowColorList[1]) : sm_DropShadowColorList[2]) : sm_DropShadowColorList[1]) : sm_DropShadowColorList[1]);
			break;
		case DropShadowColor.eCustom:
			result = m_CustomColor;
			break;
		default:
			if (m_DropColor >= DropShadowColor.eBlack && (int)m_DropColor < sm_DropShadowColorList.Length)
			{
				result = sm_DropShadowColorList[(int)m_DropColor];
			}
			break;
		}
		return result;
	}

	private string RemoveColorTags(string text)
	{
		string text2 = string.Empty;
		bool flag = false;
		char[] array = text.ToCharArray();
		for (int i = 0; i < text.Length; i++)
		{
			if (array[i] == '[')
			{
				flag = true;
			}
			if (array[i] == ']')
			{
				flag = false;
				i++;
			}
			if (!flag)
			{
				text2 += array[i];
			}
		}
		return text2;
	}

	public void UpdateDropShadowText()
	{
		if (m_CopySpriteText != null)
		{
			m_CopySpriteText.Text = m_SourceSpriteText.Text;
		}
	}

	public void UpdateDropShadowSize()
	{
		if (m_CopySpriteText != null)
		{
			m_CopySpriteText.SetCharacterSize(m_SourceSpriteText.characterSize);
		}
	}

	public void HideDropShadowText(bool aHide)
	{
		if (m_CopySpriteText != null)
		{
			m_CopySpriteText.Hide(aHide);
		}
	}
}
