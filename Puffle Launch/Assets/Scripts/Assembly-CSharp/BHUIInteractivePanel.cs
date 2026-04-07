using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("EZ GUI/Panels/Interactive Panel")]
public class BHUIInteractivePanel : UIInteractivePanel, IUIPanelExtension
{
	public delegate void ActivatePanelHandler();

	public delegate void DeactivatePanelHandler();

	public bool m_DismissOnStart;

	public List<UIPanelBase> m_DisableButtonOnPanelsWhenActive = new List<UIPanelBase>();

	public BHUIPanelManager panelManager;

	public List<BHUIButton> buttonList;

	public List<string> buttonName;

	public List<BHUITexture> textureList;

	public List<string> textureName;

	public List<BHUILabel> labelList;

	public List<string> labelName;

	public List<BHUITextField> textFieldList;

	public List<string> textFieldName;

	protected UIControlHandler<BHUIButton> m_ButtonHandler;

	protected UIControlHandler<BHUITexture> m_TextureHandler;

	protected UIControlHandler<BHUILabel> m_LabelHandler;

	protected UIControlHandler<BHUITextField> m_TextFieldHandler;

	public BHUIPanelManager PanelManager
	{
		get
		{
			return panelManager;
		}
		set
		{
			panelManager = value;
		}
	}

	public List<BHUIButton> ButtonList
	{
		get
		{
			return buttonList;
		}
		set
		{
			buttonList = value;
		}
	}

	public List<string> ButtonName
	{
		get
		{
			return buttonName;
		}
		set
		{
			buttonName = value;
		}
	}

	public List<BHUITexture> TextureList
	{
		get
		{
			return textureList;
		}
		set
		{
			textureList = value;
		}
	}

	public List<string> TextureName
	{
		get
		{
			return textureName;
		}
		set
		{
			textureName = value;
		}
	}

	public List<BHUILabel> LabelList
	{
		get
		{
			return labelList;
		}
		set
		{
			labelList = value;
		}
	}

	public List<string> LabelName
	{
		get
		{
			return labelName;
		}
		set
		{
			labelName = value;
		}
	}

	public List<BHUITextField> TextFieldList
	{
		get
		{
			return textFieldList;
		}
		set
		{
			textFieldList = value;
		}
	}

	public List<string> TextFieldName
	{
		get
		{
			return textFieldName;
		}
		set
		{
			textFieldName = value;
		}
	}

	public UIControlHandler<BHUIButton> ButtonHandler
	{
		get
		{
			return m_ButtonHandler;
		}
	}

	public UIControlHandler<BHUITexture> TextureHandler
	{
		get
		{
			return m_TextureHandler;
		}
	}

	public UIControlHandler<BHUILabel> LabelHandler
	{
		get
		{
			return m_LabelHandler;
		}
	}

	public UIControlHandler<BHUITextField> TextFieldHandler
	{
		get
		{
			return m_TextFieldHandler;
		}
	}

	public event ActivatePanelHandler activatePanel;

	public event DeactivatePanelHandler deactivatePanel;

	public virtual void Awake()
	{
		Utilities.AssertMsg(panelManager != null, string.Concat("Panel: ", base.gameObject, " doesn't have a valid panel manager!"));
		if (panelManager != null)
		{
			panelManager.AddChild(base.gameObject);
		}
		m_ButtonHandler = new UIControlHandler<BHUIButton>(buttonList, buttonName);
		m_TextureHandler = new UIControlHandler<BHUITexture>(textureList, textureName);
		m_LabelHandler = new UIControlHandler<BHUILabel>(labelList, labelName);
		m_TextFieldHandler = new UIControlHandler<BHUITextField>(textFieldList, textFieldName);
		if (m_DismissOnStart)
		{
			StartCoroutine(DismissAfterControlsReady());
		}
	}

	private IEnumerator DismissAfterControlsReady()
	{
		if (deactivateAllOnDismiss && m_LabelHandler.m_ControlList.Count > 0)
		{
			foreach (BHUILabel label in m_LabelHandler.m_ControlList)
			{
				while (!label.IsReady)
				{
					yield return null;
				}
			}
		}
		Dismiss();
	}

	public virtual void Activate(bool aActivate)
	{
		if (Utilities.AssertMsg(panelManager != null, "Invalid panel manager, make sure you run 'Setup Panel' in edit mode!"))
		{
			for (int i = 0; i < m_DisableButtonOnPanelsWhenActive.Count; i++)
			{
				IUIPanelExtension iUIPanelExtension = m_DisableButtonOnPanelsWhenActive[i] as IUIPanelExtension;
				if (Utilities.AssertMsg(iUIPanelExtension != null, "Invalid or unknown type panel: " + m_DisableButtonOnPanelsWhenActive[i]))
				{
					iUIPanelExtension.ButtonHandler.EnableAll(!aActivate);
				}
			}
		}
		if (aActivate)
		{
			BringIn();
			if (this.activatePanel != null)
			{
				this.activatePanel();
				this.activatePanel = null;
			}
		}
		else
		{
			Dismiss();
			if (this.deactivatePanel != null)
			{
				this.deactivatePanel();
				this.deactivatePanel = null;
			}
		}
	}
}
