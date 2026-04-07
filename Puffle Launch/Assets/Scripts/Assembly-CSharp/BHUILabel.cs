using UnityEngine;

[RequireComponent(typeof(DropShadow))]
[AddComponentMenu("EZ GUI/Controls/Label")]
[RequireComponent(typeof(UIControlExtension))]
[RequireComponent(typeof(AutoAdjustSpriteText))]
public class BHUILabel : SpriteText, IUIControlExtension
{
	public float m_MaxWidthSmall;

	public float m_MaxWidthMedium;

	public float m_MaxWidthLarge;

	protected UIControlExtension m_ControlExt;

	protected AutoAdjustSpriteText m_AutoAdjust;

	protected DropShadow m_DropShadow;

	protected bool m_IsReady;

	public UIControlExtension ControlExt
	{
		get
		{
			return m_ControlExt;
		}
	}

	public bool IsReady
	{
		get
		{
			return m_IsReady;
		}
	}

	protected override void Awake()
	{
		m_ControlExt = base.gameObject.GetComponent<UIControlExtension>();
		Utilities.AssertMsgCritical(m_ControlExt != null, "Fail to get UIControlExtension component!");
		m_AutoAdjust = base.gameObject.GetComponent<AutoAdjustSpriteText>();
		m_DropShadow = base.gameObject.GetComponent<DropShadow>();
		base.Awake();
		SetupSpriteText();
	}

	public override void Start()
	{
		base.Start();
		string localizeText = m_ControlExt.GetLocalizeText();
		if (!string.IsNullOrEmpty(localizeText))
		{
			base.Text = localizeText;
		}
		if ((bool)m_AutoAdjust)
		{
			m_AutoAdjust.AutoAdjust();
		}
		if ((bool)m_DropShadow)
		{
			m_DropShadow.CreateShadow();
		}
		m_IsReady = true;
	}

	public void Enable(bool aEnable)
	{
	}

	public void Show(bool aShow)
	{
		base.gameObject.SetActiveRecursively(aShow);
		Hide(!aShow);
	}

	public void UpdateDropShadow()
	{
		if (m_DropShadow != null)
		{
			m_DropShadow.UpdateDropShadowText();
			m_DropShadow.UpdateDropShadowSize();
		}
	}

	public void HideDropShadow(bool aHide)
	{
		if (m_DropShadow != null)
		{
			m_DropShadow.HideDropShadowText(aHide);
		}
	}

	protected virtual void SetupSpriteText()
	{
		if (SizeCategory.Instance == null)
		{
			return;
		}
		switch (SizeCategory.Instance.CurCategoryId)
		{
		case SizeCategory.CategoryId.eSmall:
			if (m_MaxWidthSmall > 0f)
			{
				maxWidth = m_MaxWidthSmall;
			}
			break;
		case SizeCategory.CategoryId.eMedium:
			if (m_MaxWidthMedium > 0f)
			{
				maxWidth = m_MaxWidthMedium;
			}
			break;
		case SizeCategory.CategoryId.eLarge:
			if (m_MaxWidthLarge > 0f)
			{
				maxWidth = m_MaxWidthLarge;
			}
			break;
		}
	}
}
