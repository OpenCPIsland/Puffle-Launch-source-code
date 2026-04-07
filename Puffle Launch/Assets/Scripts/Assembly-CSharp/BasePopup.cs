using UnityEngine;

public abstract class BasePopup : BaseGUI
{
	public delegate void PopupCallback(int aSelectedButton);

	public const int kPopupBackgroundId = 9;

	public const int kPopupWindowId = 10;

	public const int kCreateAccountPopupWindowId = 11;

	public const int kLoginPopupWindowId = 12;

	protected GUIDefines.WindowData m_WindowData;

	protected GUIDefines.WindowData m_WindowBackground = new GUIDefines.WindowData
	{
		pos = new GUIDefines.RectInfo
		{
			widthRatio = 1f,
			heightRatio = 1f
		},
		id = 9
	};

	protected bool m_IsShowing;

	protected PopupCallback m_Callback;

	protected bool m_IsPopupInitialized;

	public GUIDefines.WindowData WindowData
	{
		get
		{
			return m_WindowData;
		}
		set
		{
			m_WindowData = value;
		}
	}

	public bool IsShowing
	{
		get
		{
			return m_IsShowing;
		}
	}

	public BasePopup(GameObject aRefObj)
		: base(aRefObj)
	{
	}

	protected void InitPopup()
	{
		if (!m_IsPopupInitialized)
		{
			base.InitLayouts();
			m_WindowData.Init();
			if (m_WindowBackground != null)
			{
				m_WindowBackground.Init();
			}
			m_IsPopupInitialized = true;
		}
	}

	public virtual void ClosePopup()
	{
	}

	protected override void InitLayouts()
	{
	}

	public override void Draw()
	{
		if (CanDraw())
		{
			InitPopup();
			GUICompoundControls.Window(base.LocalTransform.position, m_WindowData, WindowContent);
		}
	}

	public override bool CanDraw()
	{
		return base.CanDraw() && m_IsShowing;
	}

	protected override void OnButtonSelect()
	{
		Show(false);
		if (m_Callback != null)
		{
			m_Callback(base.SelectedButton);
		}
	}

	protected void OnAutoSelect(int aSelection)
	{
		base.SelectedButton = aSelection;
		OnButtonSelect();
	}

	public virtual void Show(bool aShow)
	{
		if (aShow)
		{
			ResetButton();
		}
		m_IsShowing = aShow;
	}

	public void RegisterCallback(PopupCallback aCallback)
	{
		m_Callback = aCallback;
	}

	protected virtual void WindowContent(int aWindowId)
	{
		if (m_WindowBackground != null && aWindowId == m_WindowBackground.id)
		{
			GUI.BringWindowToFront(aWindowId);
			GUIUtil.DrawSemiTransparentLayer();
		}
		else if (aWindowId == m_WindowData.id)
		{
			if (m_WindowBackground != null)
			{
				GUI.BringWindowToFront(aWindowId);
			}
			DrawWindowContent(aWindowId);
		}
	}

	protected virtual void DrawWindowContent(int aWindowId)
	{
		base.Draw();
	}
}
