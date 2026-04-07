using UnityEngine;

[RequireComponent(typeof(UIControlExtension))]
[AddComponentMenu("EZ GUI/Controls/Button")]
public class BHUIButton : UIButton, IUIControlExtension
{
	public bool m_PlayDefaultSFX = true;

	protected UIControlExtension m_ControlExt;

	protected override void Awake()
	{
		base.Awake();
		m_ControlExt = base.gameObject.GetComponent<UIControlExtension>();
		Utilities.AssertMsgCritical(m_ControlExt != null, "Fail to get UIControlExtension component!");
		m_ControlExt.SetMaterialTexture();
		UpdateSpriteFrameInfo();
		Text = string.Empty;
	}

	public override void Start()
	{
		base.Start();
		AddValueChangedDelegate(OnButtonPressed);
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		RemoveValueChangedDelegate(OnButtonPressed);
	}

	protected virtual void OnButtonPressed(IUIObject obj)
	{
		if (m_PlayDefaultSFX)
		{
			GameFlowManager.Instance.AudioManager.PlayUISFx(GameFlowManager.Instance.MenuClick24);
		}
	}

	public void Enable(bool aEnable)
	{
		SetControlState((!aEnable) ? CONTROL_STATE.DISABLED : CONTROL_STATE.NORMAL);
	}

	public void Show(bool aShow)
	{
		base.gameObject.SetActiveRecursively(aShow);
		Hide(!aShow);
	}

	protected virtual void UpdateSpriteFrameInfo()
	{
		switch (m_ControlExt.AssetSizeCategoryId)
		{
		case SizeCategory.CategoryId.eSmall:
		{
			for (int k = 0; k < states.Length; k++)
			{
				for (int l = 0; l < states[k].spriteFrames.Length; l++)
				{
					states[k].spriteFrames[l].CopyFromSmall();
				}
			}
			break;
		}
		case SizeCategory.CategoryId.eMedium:
			break;
		case SizeCategory.CategoryId.eLarge:
		case SizeCategory.CategoryId.eXLarge:
		{
			for (int i = 0; i < states.Length; i++)
			{
				for (int j = 0; j < states[i].spriteFrames.Length; j++)
				{
					states[i].spriteFrames[j].CopyFromLarge();
				}
			}
			break;
		}
		}
	}
}
