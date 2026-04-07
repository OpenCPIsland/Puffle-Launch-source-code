using UnityEngine;

public class AboutCPGUI
{
	public delegate void PageChangeCallback();

	private const float kScrollSpeedMinimum = 10.5f;

	private const float kScrollSpeedMaximum = 200f;

	private const float kMinimum3DDistanceForScroll = 2.25f;

	private const int kNumOfPageControls = 5;

	private const float kReferenceScreenHeight = 640f;

	private bool mb_isInitialized;

	private Vector3 m_CenterPoint = new Vector3(0f, 0f, 0f);

	private Vector3 m_OutOfLeftScreenPoint = new Vector3(-30f, 0f, 0f);

	private Vector3 m_OutOfRightScreenPoint = new Vector3(30f, 0f, 0f);

	private GUIDefines.PageControlData[] m_PageControl;

	private int m_CurrentPage;

	private int m_DestinationPage = 1;

	private Texture2D[] m_Textures;

	public Vector3 m_StartPageAnchorPoint = new Vector3(0f, 0f, 0f);

	public Vector3 m_DestinationPageAnchorPoint = new Vector3(0f, 0f, 0f);

	public Vector3 m_TargetPoint;

	private PageChangeCallback m_Callback;

	public bool IsInitialized
	{
		get
		{
			return mb_isInitialized;
		}
	}

	private Transform CurrentPageTransform
	{
		get
		{
			return m_PageControl[m_CurrentPage].refTransform;
		}
	}

	private Transform DestinationPageTransform
	{
		get
		{
			return m_PageControl[m_DestinationPage].refTransform;
		}
	}

	private int FirstItemInCurrentPage
	{
		get
		{
			return m_PageControl[m_CurrentPage].firstInPage;
		}
		set
		{
			m_PageControl[m_CurrentPage].firstInPage = value;
		}
	}

	private int FirstItemInDestinationPage
	{
		get
		{
			return m_PageControl[m_DestinationPage].firstInPage;
		}
		set
		{
			m_PageControl[m_DestinationPage].firstInPage = value;
		}
	}

	public int CurrentPage
	{
		get
		{
			return m_PageControl[m_CurrentPage].PageNumber;
		}
		set
		{
			m_PageControl[m_CurrentPage].PageNumber = value;
		}
	}

	public int DestinationPage
	{
		get
		{
			return m_PageControl[m_DestinationPage].PageNumber;
		}
		set
		{
			m_PageControl[m_DestinationPage].PageNumber = value;
		}
	}

	public int TotalPage
	{
		get
		{
			return 5;
		}
	}

	public bool ScrollDone
	{
		get
		{
			return DestinationPage == CurrentPage;
		}
	}

	public AboutCPGUI(GameObject[] aRefObj)
	{
		mb_isInitialized = false;
		m_PageControl = new GUIDefines.PageControlData[aRefObj.Length];
		for (int i = 0; i < aRefObj.Length; i++)
		{
			m_PageControl[i] = new GUIDefines.PageControlData();
			m_PageControl[i].refObj = aRefObj[i];
			m_PageControl[i].refTransform = aRefObj[i].transform;
		}
		CurrentPage = GameFlowManager.Instance.GUIManager.AboutCPCurrentPage;
		DestinationPage = 1;
	}

	public void InitCPGUI(Texture2D[] aTextures)
	{
		m_Textures = aTextures;
		CurrentPageTransform.GetComponent<Renderer>().material.mainTexture = m_Textures[CurrentPage];
		DestinationPageTransform.GetComponent<Renderer>().material.mainTexture = m_Textures[DestinationPage];
		mb_isInitialized = true;
	}

	public void Start()
	{
		CurrentPageTransform.localPosition = m_CenterPoint;
		DestinationPageTransform.localPosition = m_OutOfRightScreenPoint;
	}

	public void Draw()
	{
	}

	public void RegisterCallback(PageChangeCallback aCallback)
	{
		m_Callback = aCallback;
	}

	public void UpdateScroll()
	{
		Vector3 vector = m_TargetPoint - CurrentPageTransform.localPosition;
		Vector3 vector2 = CurrentPageTransform.localPosition - m_StartPageAnchorPoint;
		float num = Mathf.Abs(vector.x);
		if (vector2.x == 0f && !ScrollDone)
		{
			DestinationPage = CurrentPage;
			CurrentPageTransform.localPosition = (m_StartPageAnchorPoint = m_CenterPoint);
		}
		else if (vector2.x <= -30f)
		{
			ChangeToNextPage();
		}
		else if (vector2.x >= 30f)
		{
			ChangeToPreviousPage();
		}
		if (vector.x < 0f)
		{
			if (CurrentPage < TotalPage - 1 && vector2.x <= 0f)
			{
				ShowNextPage();
			}
			else if (vector2.x <= 0f)
			{
				m_TargetPoint = CurrentPageTransform.localPosition;
				vector = Vector3.zero;
				num = 0f;
			}
		}
		else if (vector.x > 0f)
		{
			if (CurrentPage > 0 && vector2.x >= 0f)
			{
				ShowPreviousPage();
			}
			else if (vector2.x >= 0f)
			{
				m_TargetPoint = CurrentPageTransform.localPosition;
				vector = Vector3.zero;
				num = 0f;
			}
		}
		if (Mathf.Abs(num) > 0f)
		{
			float a = Time.deltaTime * (10.5f + 189.5f * (num / 30f));
			Vector3 vector3 = vector;
			vector3.Normalize();
			vector3 *= Mathf.Min(a, num);
			CurrentPageTransform.localPosition += vector3;
			DestinationPageTransform.localPosition += vector3;
		}
	}

	public void StartManualScroll()
	{
		m_TargetPoint = CurrentPageTransform.localPosition;
	}

	public void ManualScroll(Vector2 av2_scrollMovement)
	{
		Vector3 vector = Camera.main.ScreenToWorldPoint(av2_scrollMovement) - Camera.main.ScreenToWorldPoint(Vector2.zero);
		vector.y = 0f;
		m_TargetPoint += vector;
	}

	public void RecenterScroll()
	{
		Vector3 vector = m_StartPageAnchorPoint - m_TargetPoint;
		if (vector.x == 0f)
		{
			return;
		}
		Vector3 zero = Vector3.zero;
		zero = ((!(vector.x > 0f)) ? m_OutOfRightScreenPoint : m_OutOfLeftScreenPoint);
		if (Mathf.Abs(vector.x) > 2.25f && DestinationPage != CurrentPage)
		{
			if ((!(vector.x < 0f) || CurrentPage != 0) && (!(vector.x > 0f) || CurrentPage != TotalPage - 1))
			{
				m_TargetPoint = zero;
			}
			else
			{
				m_TargetPoint = m_StartPageAnchorPoint;
			}
		}
		else
		{
			m_TargetPoint = m_StartPageAnchorPoint;
		}
	}

	private void ShowNextPage()
	{
		if (DestinationPage <= CurrentPage)
		{
			DestinationPage = CurrentPage + 1;
			CurrentPageTransform.localPosition = (m_StartPageAnchorPoint = m_CenterPoint);
			DestinationPageTransform.localPosition = (m_DestinationPageAnchorPoint = m_OutOfRightScreenPoint);
			DestinationPageTransform.GetComponent<Renderer>().material.mainTexture = m_Textures[DestinationPage];
		}
	}

	private void ChangeToNextPage()
	{
		CurrentPage = DestinationPage;
		CurrentPageTransform.localPosition = (m_StartPageAnchorPoint = m_CenterPoint);
		CurrentPageTransform.GetComponent<Renderer>().material.mainTexture = m_Textures[CurrentPage];
		DestinationPageTransform.localPosition = (m_DestinationPageAnchorPoint = m_OutOfRightScreenPoint);
		if (m_Callback != null)
		{
			m_Callback();
		}
	}

	private void ShowPreviousPage()
	{
		if (DestinationPage >= CurrentPage)
		{
			DestinationPage = CurrentPage - 1;
			CurrentPageTransform.localPosition = (m_StartPageAnchorPoint = m_CenterPoint);
			DestinationPageTransform.localPosition = (m_DestinationPageAnchorPoint = m_OutOfLeftScreenPoint);
			DestinationPageTransform.GetComponent<Renderer>().material.mainTexture = m_Textures[DestinationPage];
		}
	}

	private void ChangeToPreviousPage()
	{
		CurrentPage = DestinationPage;
		CurrentPageTransform.localPosition = (m_StartPageAnchorPoint = m_CenterPoint);
		CurrentPageTransform.GetComponent<Renderer>().material.mainTexture = m_Textures[CurrentPage];
		DestinationPageTransform.localPosition = (m_DestinationPageAnchorPoint = m_OutOfLeftScreenPoint);
		if (m_Callback != null)
		{
			m_Callback();
		}
	}
}
