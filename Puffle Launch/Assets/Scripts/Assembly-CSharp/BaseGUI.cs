using UnityEngine;

public abstract class BaseGUI
{
	public enum GUIPriority
	{
		eLow = 0,
		eNormal = 1,
		eHigh = 2,
		eGUIPriority_COUNT = 3
	}

	private GUIDefines.ButtonData[] m_ButtonData;

	private GUIDefines.TextureData[] m_TextureData;

	private GUIDefines.LabelData[] m_LabelData;

	private GUIDefines.TextFieldData[] m_TextFieldData;

	private GameObject m_RefObj;

	private Transform m_LocalTransform;

	private int m_SelectedButton = -1;

	private bool m_StopDraw;

	private bool m_BlockControl;

	private GUIPriority m_Priority = GUIPriority.eNormal;

	private int m_LastFocusedTextField = -1;

	private bool m_WasKeyboardVisible;

	public GUIDefines.ButtonData[] ButtonData
	{
		get
		{
			return m_ButtonData;
		}
		set
		{
			m_ButtonData = value;
		}
	}

	public GUIDefines.TextureData[] TextureData
	{
		get
		{
			return m_TextureData;
		}
		set
		{
			m_TextureData = value;
		}
	}

	public GUIDefines.LabelData[] LabelData
	{
		get
		{
			return m_LabelData;
		}
		set
		{
			m_LabelData = value;
		}
	}

	public GUIDefines.TextFieldData[] TextFieldData
	{
		get
		{
			return m_TextFieldData;
		}
		set
		{
			m_TextFieldData = value;
		}
	}

	public Transform LocalTransform
	{
		get
		{
			return m_LocalTransform;
		}
	}

	public int SelectedButton
	{
		get
		{
			return m_SelectedButton;
		}
		set
		{
			m_SelectedButton = value;
		}
	}

	public GUIDefines.ButtonData SelectedButtonData
	{
		get
		{
			for (int i = 0; i < m_ButtonData.Length; i++)
			{
				if (m_SelectedButton == m_ButtonData[i].buttonId)
				{
					return m_ButtonData[i];
				}
			}
			Utilities.AssertMsg(false, "Tried to get selected button's data, but there is none! You can use HasSelectedButton() to avoid this assert");
			return new GUIDefines.ButtonData();
		}
	}

	public bool StopDraw
	{
		get
		{
			return m_StopDraw;
		}
		set
		{
			m_StopDraw = value;
		}
	}

	public GUIPriority Priority
	{
		get
		{
			return m_Priority;
		}
		set
		{
			m_Priority = value;
		}
	}

	public BaseGUI(GameObject aRefObj)
	{
		Init(aRefObj);
	}

	public BaseGUI()
	{
	}

	public bool HasSelectedButton()
	{
		return m_SelectedButton != -1;
	}

	protected abstract void CreateLayouts();

	protected abstract void OnButtonSelect();

	public virtual void Init(GameObject aRefObj)
	{
		InitReference(aRefObj);
		CreateLayouts();
		InitLayouts();
	}

	protected virtual void InitLayouts()
	{
		if (m_ButtonData != null)
		{
			for (int i = 0; i < m_ButtonData.Length; i++)
			{
				m_ButtonData[i].Init();
			}
		}
		if (m_TextureData != null)
		{
			for (int j = 0; j < m_TextureData.Length; j++)
			{
				m_TextureData[j].Init();
			}
		}
		if (m_LabelData != null)
		{
			for (int k = 0; k < m_LabelData.Length; k++)
			{
				m_LabelData[k].Init();
			}
		}
		if (m_TextFieldData != null)
		{
			for (int l = 0; l < m_TextFieldData.Length; l++)
			{
				m_TextFieldData[l].Init();
			}
		}
	}

	protected virtual void InitReference(GameObject aRefObj)
	{
		if (Utilities.Assert(aRefObj != null))
		{
			m_RefObj = aRefObj;
			m_LocalTransform = m_RefObj.transform;
		}
	}

	public virtual void Draw()
	{
		GUI.matrix = GameFlowManager.Instance.GUIManager.m_NewResMatrix;
		if (m_TextureData != null)
		{
			GUICompoundControls.Textures(m_LocalTransform.position, m_TextureData);
		}
		if (m_TextFieldData != null)
		{
			GUICompoundControls.TextFields(m_LocalTransform.position, m_TextFieldData, IsControlBlocked());
			MoveScreenUpToShowHiddenTextField();
		}
		if (m_ButtonData != null)
		{
			int num = GUICompoundControls.Buttons(m_LocalTransform.position, m_ButtonData);
			if (num >= 0)
			{
				OnButtonSelect(num);
			}
		}
		if (m_LabelData != null)
		{
			GUICompoundControls.Labels(m_LocalTransform.position, m_LabelData);
		}
	}

	protected virtual void MoveScreenUpToShowHiddenTextField()
	{
		if (m_TextFieldData == null)
		{
			return;
		}
		for (int i = 0; i < m_TextFieldData.Length; i++)
		{
			if (TouchScreenKeyboard.visible)
			{
				if (m_TextFieldData[i].isFocused && m_LastFocusedTextField != i)
				{
					m_LastFocusedTextField = i;
					bool flag = false;
					if (m_TextFieldData[i].pos.inPixel.yMax >= TouchScreenKeyboard.area.yMin)
					{
						float num = 0f;
						if (ResolutionManager.Instance.LayoutSize == ResolutionManager.eLayoutSize.eOriginal)
						{
							num = 170f;
						}
						float num2 = 0f;
						float num3 = 30f;
						num3 *= 640f / GUIConstants.kReferenceScreenHeight;
						num2 = TouchScreenKeyboard.area.yMin;
						float num4 = (m_TextFieldData[i].pos.inPixel.yMax - num2 - num) / num3;
						if (num4 > 0f)
						{
							m_LocalTransform.position = new Vector3(0f, num4, 0f);
							flag = true;
						}
					}
					if (!flag)
					{
						m_LocalTransform.position = Vector3.zero;
					}
				}
			}
			else if (m_WasKeyboardVisible)
			{
				m_LastFocusedTextField = -1;
				m_LocalTransform.position = Vector3.zero;
			}
			m_WasKeyboardVisible = TouchScreenKeyboard.visible;
		}
	}

	public virtual bool CanDraw()
	{
		GUI.skin = GameFlowManager.Instance.GUIManager.Skin;
		Utilities.Assert(m_LocalTransform != null);
		return GUI.skin != null && m_LocalTransform != null && !m_StopDraw;
	}

	protected virtual void OnButtonSelect(int aSelectedButton)
	{
		if (!IsControlBlocked() && IsValidButton(aSelectedButton))
		{
			m_SelectedButton = aSelectedButton;
			OnButtonSelect();
		}
	}

	public virtual void ResetButton()
	{
		m_SelectedButton = -1;
	}

	public virtual bool IsAnyButtonSelected()
	{
		return IsValidButton(m_SelectedButton);
	}

	public virtual bool IsValidButton(int aButtonId)
	{
		return aButtonId != -1;
	}

	public virtual void BlockControl(bool aBlockControl)
	{
		m_BlockControl = aBlockControl;
		if (ButtonData != null)
		{
			for (int i = 0; i < ButtonData.Length; i++)
			{
				ButtonData[i].isControlBlocked = aBlockControl;
			}
		}
	}

	public virtual bool IsControlBlocked()
	{
		if (m_Priority < GUIPriority.eHigh)
		{
			return m_BlockControl || NetManager.Instance.IsNetPopupShowing || GameFlowManager.Instance.GUIManager.CurrentScene == GUIManager.Scene.eLoadingScreen;
		}
		return m_BlockControl;
	}

	public virtual void StopGUI()
	{
		m_StopDraw = true;
		m_BlockControl = true;
		GUIStyleContainer.CleanUp();
	}

	public virtual int GetButtonIndex(int aButtonId)
	{
		if (m_ButtonData != null)
		{
			for (int i = 0; i < m_ButtonData.Length; i++)
			{
				if (m_ButtonData[i].buttonId == aButtonId)
				{
					return i;
				}
			}
		}
		Utilities.Assert(false);
		return -1;
	}

	public void SetLabelTextId(int aLabelIndex, string aTextId)
	{
		if (LabelData.Length > aLabelIndex)
		{
			LabelData[aLabelIndex].content.text = string.Empty;
			LabelData[aLabelIndex].content.textId = aTextId;
		}
	}

	public void SetLabelText(int aLabelIndex, string aText)
	{
		if (LabelData.Length > aLabelIndex)
		{
			LabelData[aLabelIndex].content.text = aText;
			LabelData[aLabelIndex].content.textId = string.Empty;
		}
	}

	public void SetButtonTextId(int aButtonId, string aTextId)
	{
		int buttonIndex = GetButtonIndex(aButtonId);
		if (buttonIndex != -1)
		{
			ButtonData[buttonIndex].content.text = string.Empty;
			ButtonData[buttonIndex].content.textId = aTextId;
		}
	}

	public void SetButtonText(int aButtonId, string aText)
	{
		int buttonIndex = GetButtonIndex(aButtonId);
		if (buttonIndex != -1)
		{
			ButtonData[buttonIndex].content.text = aText;
			ButtonData[buttonIndex].content.textId = string.Empty;
		}
	}

	public void SetLabelInvisible(int aLabelIndex, bool aInvisible)
	{
		if (LabelData.Length > aLabelIndex)
		{
			LabelData[aLabelIndex].invisible = aInvisible;
		}
	}

	public void SetTextureInvisible(int aTextureIndex, bool aInvisible)
	{
		if (TextureData.Length > aTextureIndex)
		{
			TextureData[aTextureIndex].invisible = aInvisible;
		}
	}

	public void SetButtonInvisible(int aButtonId, bool aInvisible)
	{
		int buttonIndex = GetButtonIndex(aButtonId);
		if (buttonIndex != -1)
		{
			ButtonData[buttonIndex].invisible = aInvisible;
		}
	}
}
