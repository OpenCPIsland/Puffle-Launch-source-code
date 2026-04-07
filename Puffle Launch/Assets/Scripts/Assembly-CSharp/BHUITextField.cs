using UnityEngine;

[RequireComponent(typeof(UIControlExtension))]
[AddComponentMenu("EZ GUI/Controls/Text Field")]
public class BHUITextField : UITextField, IUIControlExtension
{
	public AutoAdjustSpriteText.SpriteTextColor m_UnfocusTextColor = AutoAdjustSpriteText.SpriteTextColor.eGrey;

	public AutoAdjustSpriteText.SpriteTextColor m_FocusTextColor = AutoAdjustSpriteText.SpriteTextColor.eGrey;

	protected UIControlExtension m_ControlExt;

	protected override void Awake()
	{
		m_ControlExt = base.gameObject.GetComponent<UIControlExtension>();
		Utilities.AssertMsgCritical(m_ControlExt != null, "Fail to get UIControlExtension component!");
		m_ControlExt.SetMaterialTexture();
		UpdateSpriteFrameInfo();
		base.Awake();
	}

	public override void Start()
	{
		base.Start();
		Text = m_ControlExt.GetLocalizeText();
		spriteText.SetColor(AutoAdjustSpriteText.GetColor(m_UnfocusTextColor));
		AddFocusDelegate(OnTextFieldFocus);
		AddCommitDelegate(OnTextFieldCommit);
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		RemoveFocusDelegate(OnTextFieldFocus);
		RemoveCommitDelegate(OnTextFieldCommit);
	}

	protected virtual void OnTextFieldFocus(UITextField field)
	{
		spriteText.SetColor(AutoAdjustSpriteText.GetColor(m_FocusTextColor));
	}

	protected virtual void OnTextFieldCommit(IKeyFocusable control)
	{
		spriteText.SetColor(AutoAdjustSpriteText.GetColor(m_UnfocusTextColor));
	}

	public void Enable(bool aEnable)
	{
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
