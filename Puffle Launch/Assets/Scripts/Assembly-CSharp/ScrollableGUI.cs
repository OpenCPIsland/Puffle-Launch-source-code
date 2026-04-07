using UnityEngine;

public abstract class ScrollableGUI : BaseGUI
{
	public enum ScrollDirection
	{
		eVertical = 0,
		eHorizontal = 1,
		eNone = 2
	}

	private enum ScrollBarState
	{
		eInactive = 0,
		eActive = 1,
		eFadeDelay = 2,
		eFading = 3
	}

	public delegate void ScrollableGUICallback(int aSelectedButton);

	private const int ci_maxNumSmoothingValues = 5;

	private const float cf_scrollTolerance = 10f;

	private const float cf_scrollStopTime = 1f;

	protected Rect mo_scrollAreaRect;

	protected Rect mo_scrollAreaInnerRect;

	protected ScrollableGUICallback m_Callback;

	public ScrollDirection me_scrollDirection;

	private ScrollBarState me_scrollBarState;

	private float mf_scrollBarTimer;

	public float mf_scrollBarFadeDelayTime = 0.5f;

	public float mf_scrollBarDissapearDelay = 0.5f;

	public float mf_scrollBarFadeSpeed = 3f;

	private float mf_scrollBarAlpha = 1f;

	private bool mb_isTouchDown;

	public float mf_scrollPosition;

	private Rect mo_scrollAreaDetectZone;

	private int mi_numSmoothingValues;

	private float[] m_scrollSpeeds = new float[5];

	private float mf_scrollSpeed;

	private int mi_touchPosition;

	private int mi_startTouchPosition;

	private int mi_previousTouchPosition;

	public bool mb_Scrolling;

	public bool mb_disableButtons;

	public bool mb_renableButtons;

	public bool mb_ScrollAreaSelected;

	private float mf_scrollStopTimer;

	private Vector2 mv2_touchPosition = new Vector2(0f, 0f);

	protected float mf_scrollPercentage;

	public float ScrollPercentage
	{
		get
		{
			return mf_scrollPercentage;
		}
		set
		{
			mf_scrollPercentage = value;
		}
	}

	public ScrollableGUI(GameObject aRefObj)
		: base(aRefObj)
	{
		mf_scrollPercentage = 0f;
	}

	public ScrollableGUI()
	{
	}

	protected void InitScrollArea(GUIDefines.RectInfo ao_scrollableArea, ScrollDirection ae_ScrollDirection, float af_scrollableDistance)
	{
		ao_scrollableArea.Init();
		mo_scrollAreaRect = ao_scrollableArea.inPixel;
		me_scrollDirection = ae_ScrollDirection;
		if (me_scrollDirection == ScrollDirection.eVertical)
		{
			mo_scrollAreaInnerRect = new Rect(0f, 0f, mo_scrollAreaRect.width, af_scrollableDistance * mo_scrollAreaRect.height);
		}
		else if (me_scrollDirection == ScrollDirection.eHorizontal)
		{
			mo_scrollAreaInnerRect = new Rect(0f, 0f, af_scrollableDistance * mo_scrollAreaRect.width, mo_scrollAreaRect.height);
		}
		else
		{
			Utilities.AssertMsg(false, "Unsupported Scrolling type!\n");
		}
		mo_scrollAreaDetectZone = new Rect(mo_scrollAreaRect.xMin, (float)Screen.height - (mo_scrollAreaRect.yMin + mo_scrollAreaRect.height), mo_scrollAreaRect.width, mo_scrollAreaRect.height);
	}

	public virtual void Update()
	{
		bool flag = false;
		if (Input.GetMouseButton(0))
		{
			mb_isTouchDown = true;
			mv2_touchPosition = Input.mousePosition;
			if (Input.GetMouseButtonDown(0))
			{
				flag = true;
			}
		}
		else
		{
			mb_isTouchDown = false;
		}
		if (me_scrollDirection == ScrollDirection.eVertical)
		{
			mi_touchPosition = (int)mv2_touchPosition.y;
		}
		else if (me_scrollDirection == ScrollDirection.eHorizontal)
		{
			mi_touchPosition = (int)mv2_touchPosition.x;
		}
		if (!mb_isTouchDown && !mb_Scrolling)
		{
			mb_renableButtons = true;
		}
		float num = 0f;
		if (mb_isTouchDown)
		{
			if (flag)
			{
				mi_numSmoothingValues = 0;
				mf_scrollSpeed = 0f;
				mi_startTouchPosition = (mi_previousTouchPosition = mi_touchPosition);
				if (mo_scrollAreaDetectZone.Contains(mv2_touchPosition))
				{
					mb_ScrollAreaSelected = true;
				}
				else
				{
					mb_ScrollAreaSelected = false;
				}
			}
			if (mb_ScrollAreaSelected)
			{
				if ((float)Mathf.Abs(mi_touchPosition - mi_startTouchPosition) > 10f)
				{
					mb_Scrolling = true;
					mb_disableButtons = true;
					mf_scrollStopTimer = 1f;
				}
				if (mb_Scrolling)
				{
					num = mi_touchPosition - mi_previousTouchPosition;
					float num2 = (mf_scrollSpeed = num / Time.deltaTime);
					mi_numSmoothingValues++;
					mi_numSmoothingValues = ((mi_numSmoothingValues <= 5) ? mi_numSmoothingValues : 5);
					for (int num3 = mi_numSmoothingValues - 1; num3 > 0; num3--)
					{
						m_scrollSpeeds[num3] = m_scrollSpeeds[num3 - 1];
						mf_scrollSpeed += m_scrollSpeeds[num3];
					}
					m_scrollSpeeds[0] = num2;
					mf_scrollSpeed /= mi_numSmoothingValues;
				}
				mi_previousTouchPosition = mi_touchPosition;
			}
		}
		else
		{
			if (mb_Scrolling)
			{
				mf_scrollStopTimer -= Time.deltaTime;
				if (mf_scrollStopTimer <= 0f)
				{
					mf_scrollStopTimer = 0f;
					mb_Scrolling = false;
				}
				num += mf_scrollStopTimer * mf_scrollSpeed * Time.deltaTime;
			}
			mb_ScrollAreaSelected = false;
		}
		if (mf_scrollSpeed == 0f)
		{
			mb_Scrolling = false;
		}
		mf_scrollPosition += num;
		if (mf_scrollPosition < 0f)
		{
			mf_scrollPosition = 0f;
		}
		else if (mf_scrollPosition > mo_scrollAreaInnerRect.height - mo_scrollAreaRect.height)
		{
			mf_scrollPosition = ((!(mo_scrollAreaInnerRect.height - mo_scrollAreaRect.height > 0f)) ? 0f : (mo_scrollAreaInnerRect.height - mo_scrollAreaRect.height));
		}
		if (me_scrollDirection == ScrollDirection.eVertical)
		{
			mo_scrollAreaInnerRect.y = 0f - mf_scrollPosition;
		}
		else if (me_scrollDirection == ScrollDirection.eHorizontal)
		{
			mo_scrollAreaInnerRect.y = 0f - mf_scrollPosition;
		}
		mf_scrollPercentage = mf_scrollPosition / (mo_scrollAreaInnerRect.height - mo_scrollAreaRect.height);
		switch (me_scrollBarState)
		{
		case ScrollBarState.eInactive:
			if (mb_Scrolling)
			{
				me_scrollBarState = ScrollBarState.eActive;
			}
			break;
		case ScrollBarState.eActive:
			mf_scrollBarAlpha = 1f;
			if (!mb_Scrolling && !mb_ScrollAreaSelected)
			{
				me_scrollBarState = ScrollBarState.eFadeDelay;
				mf_scrollBarTimer = mf_scrollBarFadeDelayTime;
			}
			break;
		case ScrollBarState.eFadeDelay:
			mf_scrollBarAlpha = 1f;
			mf_scrollBarTimer -= Time.deltaTime;
			if (mf_scrollBarTimer <= 0f)
			{
				me_scrollBarState = ScrollBarState.eFading;
			}
			break;
		case ScrollBarState.eFading:
			mf_scrollBarAlpha -= mf_scrollBarFadeSpeed * Time.deltaTime;
			if (mf_scrollBarAlpha <= 0f)
			{
				mf_scrollBarAlpha = 0f;
				me_scrollBarState = ScrollBarState.eInactive;
			}
			break;
		}
	}

	public void RegisterCallback(ScrollableGUICallback aCallback)
	{
		m_Callback = aCallback;
	}

	protected override void OnButtonSelect()
	{
	}

	protected override void OnButtonSelect(int aSelectedButton)
	{
		if (!mb_disableButtons && m_Callback != null)
		{
			m_Callback(aSelectedButton);
		}
	}

	public override void Draw()
	{
		if (CanDraw())
		{
			DrawScrollListContent();
			if (me_scrollBarState != ScrollBarState.eInactive)
			{
				Color color = GUI.color;
				if (mf_scrollBarAlpha < 1f)
				{
					Color color2 = GUI.color;
					color2 = Color.white;
					color2.a = mf_scrollBarAlpha;
					GUI.color = color2;
				}
				DrawScrollBar();
				if (mf_scrollBarAlpha < 1f)
				{
					GUI.color = color;
				}
			}
			DrawBorders();
		}
		if (mb_renableButtons)
		{
			mb_renableButtons = false;
			mb_disableButtons = false;
		}
	}

	public virtual void DrawScrollListContent()
	{
		GUILayout.BeginArea(mo_scrollAreaRect);
		GUILayout.BeginArea(mo_scrollAreaInnerRect);
		base.Draw();
		GUILayout.EndArea();
		GUILayout.EndArea();
	}

	public virtual void DrawBorders()
	{
	}

	public virtual void DrawScrollBar()
	{
	}

	public void ResetScrollPosition()
	{
		mf_scrollPosition = 0f;
	}
}
