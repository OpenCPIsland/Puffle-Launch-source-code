using UnityEngine;

public class BackgroundResChange : MonoBehaviour
{
	public Camera m_MyCamera;

	public bool m_RotateAxis;

	public bool m_AdjustAspectRatioOnly;

	public bool m_AdjustWidthToScreen = true;

	public bool m_AdjustHeightToScreen = true;

	public bool m_MoveToTopOfScreen;

	public bool m_MoveToBottomOfScreen;

	public bool m_MoveToLeftOfScreen;

	public bool m_MoveToRightOfScreen;

	public MeshRenderer m_AlternateMeshRenderer;

	public string m_LocalizationENSuffix = "_EN";

	public string m_LocalizationFRSuffix = "_FR";

	public string m_LocalizationPTSuffix = "_PT";

	public string m_LocalizationESSuffix = "_ES";

	public string m_LocalizationDESuffix = "_DE";

	public string m_LocalizationJASuffix = "_JA";

	public string m_BasePath = string.Empty;

	public string m_TextureName = string.Empty;

	public bool m_IsLocalized;

	private bool m_GotWidthRatio;

	private bool m_GotHeightRatio;

	private Transform m_MyTransform;

	private MeshRenderer m_MyMeshRenderer;

	private Bounds m_MyBackgroundBounds;

	private Vector3 m_BottomLeft;

	private Vector3 m_TopRight;

	private float m_WidthRatio = 1f;

	private float m_HeightRatio = 1f;

	private Vector3 m_WorkingVector = default(Vector3);

	public float WidthRatio
	{
		get
		{
			if (!m_GotWidthRatio)
			{
				m_GotWidthRatio = true;
				m_WidthRatio = m_MyCamera.orthographicSize * m_MyCamera.aspect / GetComponent<MeshFilter>().GetComponent<Renderer>().bounds.extents.x;
			}
			return m_WidthRatio;
		}
	}

	public float HeightRatio
	{
		get
		{
			if (!m_GotHeightRatio)
			{
				m_GotHeightRatio = true;
				m_HeightRatio = m_MyCamera.orthographicSize / GetComponent<MeshFilter>().GetComponent<Renderer>().bounds.extents.y;
			}
			return m_HeightRatio;
		}
	}

	private void Awake()
	{
		m_GotWidthRatio = false;
		m_GotHeightRatio = false;
		m_MyTransform = base.transform;
		m_MyMeshRenderer = GetComponent<MeshRenderer>();
		if (m_AlternateMeshRenderer != null)
		{
			m_MyBackgroundBounds = m_AlternateMeshRenderer.bounds;
		}
		else
		{
			m_MyBackgroundBounds = GetComponent<MeshFilter>().GetComponent<Renderer>().bounds;
		}
		if (m_MyCamera == null)
		{
			m_MyCamera = Camera.main;
		}
		m_BottomLeft = m_MyCamera.ScreenToWorldPoint(new Vector3(0f, 0f, m_MyTransform.position.z));
		m_TopRight = m_MyCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, m_MyTransform.position.z));
	}

	private void Start()
	{
		Vector3 localScale = m_MyTransform.localScale;
		if (!m_GotWidthRatio)
		{
			m_GotWidthRatio = true;
			m_WidthRatio = m_MyCamera.orthographicSize * m_MyCamera.aspect / m_MyBackgroundBounds.extents.x;
		}
		if (!m_GotHeightRatio)
		{
			m_GotHeightRatio = true;
			m_HeightRatio = m_MyCamera.orthographicSize / m_MyBackgroundBounds.extents.y;
		}
		if (m_AdjustAspectRatioOnly)
		{
			if (m_RotateAxis)
			{
				localScale.z = localScale.x * (m_MyBackgroundBounds.extents.x * m_MyCamera.aspect);
			}
			else
			{
				localScale.x = localScale.z * (m_MyBackgroundBounds.extents.z * m_MyCamera.aspect);
			}
		}
		else
		{
			if (m_AdjustWidthToScreen)
			{
				if (m_RotateAxis)
				{
					localScale.z *= m_WidthRatio;
				}
				else
				{
					localScale.x *= m_WidthRatio;
				}
			}
			if (m_AdjustHeightToScreen)
			{
				if (m_RotateAxis)
				{
					localScale.x *= m_HeightRatio;
				}
				else
				{
					localScale.z *= m_HeightRatio;
				}
			}
		}
		m_MyTransform.localScale = localScale;
		m_WorkingVector = m_MyTransform.position;
		if (m_MoveToTopOfScreen)
		{
			m_WorkingVector.y += m_TopRight.y - m_MyBackgroundBounds.min.y;
		}
		else if (m_MoveToBottomOfScreen)
		{
			m_WorkingVector.y += m_BottomLeft.y - m_MyBackgroundBounds.max.y;
		}
		if (m_MoveToLeftOfScreen)
		{
			m_WorkingVector.x += m_BottomLeft.x - m_MyBackgroundBounds.min.x;
		}
		else if (m_MoveToRightOfScreen)
		{
			m_WorkingVector.x += m_TopRight.x - m_MyBackgroundBounds.max.x;
		}
		m_MyTransform.position = m_WorkingVector;
		LoadResolutionDependentTexture();
	}

	private void LoadResolutionDependentTexture()
	{
		if (!(m_MyMeshRenderer != null) || !(m_BasePath != string.Empty) || !(m_TextureName != string.Empty))
		{
			return;
		}
		string text = m_BasePath + m_TextureName;
		if (m_IsLocalized)
		{
			switch (LocalizationManager.GetLanguageCode())
			{
			case "fr":
				text += m_LocalizationFRSuffix;
				break;
			case "pt":
				text += m_LocalizationPTSuffix;
				break;
			case "es":
				text += m_LocalizationESSuffix;
				break;
			case "de":
				text += m_LocalizationDESuffix;
				break;
			case "ja":
				text += m_LocalizationJASuffix;
				break;
			default:
				text += m_LocalizationENSuffix;
				break;
			}
		}
		string text2 = text;
		if ((float)Screen.width <= 480f && (float)Screen.height <= 320f)
		{
			text2 += "_lowres";
		}
		else if ((float)Screen.width == 1024f && (float)Screen.height == 768f)
		{
			text2 += "_iPad";
		}
		m_MyMeshRenderer.material.mainTexture = GUIUtil.LoadTexture2D(text2);
		if (m_MyMeshRenderer.material.mainTexture == null)
		{
			m_MyMeshRenderer.material.mainTexture = GUIUtil.LoadTexture2D(text);
		}
	}
}
