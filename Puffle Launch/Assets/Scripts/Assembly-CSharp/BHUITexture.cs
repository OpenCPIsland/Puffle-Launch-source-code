using UnityEngine;

[AddComponentMenu("EZ GUI/Controls/Texture")]
[RequireComponent(typeof(UIControlExtension))]
public class BHUITexture : SimpleSprite, IUIControlExtension
{
	public enum RotateDirection
	{
		eNone = -1,
		eClockwise = 0,
		eCounterClockwise = 1
	}

	public Vector2[] lowerLeftPixelSmall = new Vector2[6];

	public Vector2[] pixelDimensionsSmall = new Vector2[6];

	public Vector2[] lowerLeftPixelMedium = new Vector2[6];

	public Vector2[] pixelDimensionsMedium = new Vector2[6];

	public Vector2[] lowerLeftPixelLarge = new Vector2[6];

	public Vector2[] pixelDimensionsLarge = new Vector2[6];

	public Texture2D defaultAtlasTexture;

	public string defaultAtlasTexturePath;

	public bool m_Localized;

	public bool m_FullScreenTile;

	public bool m_HorizontalMirror;

	public bool m_VericalMirror;

	public RotateDirection m_RotateDirection = RotateDirection.eNone;

	protected float m_CurrentRotateAngle;

	protected UIControlExtension m_ControlExt;

	private bool m_IsReady;

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
		m_ControlExt.SetMaterialLocalizedTexture(m_Localized);
		SetupSimpleSprite();
		base.Awake();
	}

	public override void Start()
	{
		base.Start();
		m_IsReady = true;
	}

	public virtual void Update()
	{
		if (m_RotateDirection != RotateDirection.eNone)
		{
			RotateTexture();
		}
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
	}

	protected virtual void RotateTexture()
	{
	}

	public void Enable(bool aEnable)
	{
	}

	public void Show(bool aShow)
	{
		base.gameObject.SetActiveRecursively(aShow);
		Hide(!aShow);
	}

	protected virtual void SetupSimpleSprite()
	{
		int assetLanguage = (int)m_ControlExt.AssetLanguage;
		if (assetLanguage < 0 || assetLanguage >= lowerLeftPixelSmall.Length || assetLanguage >= pixelDimensionsSmall.Length)
		{
			Utilities.AssertMsg(false, "Fail to set simple sprite uv due to invalid asset language: " + m_ControlExt.AssetLanguage);
			return;
		}
		Vector2 vector;
		Vector2 vector2;
		switch (m_ControlExt.AssetSizeCategoryId)
		{
		case SizeCategory.CategoryId.eSmall:
			vector = lowerLeftPixelSmall[assetLanguage];
			vector2 = pixelDimensionsSmall[assetLanguage];
			break;
		default:
			vector = lowerLeftPixelMedium[assetLanguage];
			vector2 = pixelDimensionsMedium[assetLanguage];
			break;
		case SizeCategory.CategoryId.eLarge:
			vector = lowerLeftPixelLarge[assetLanguage];
			vector2 = pixelDimensionsLarge[assetLanguage];
			break;
		}
		if (m_FullScreenTile)
		{
			vector2.x = Screen.width;
			vector2.y = Screen.height;
		}
		if (m_HorizontalMirror)
		{
			vector.x += vector2.x;
			vector2.x *= -1f;
		}
		if (m_VericalMirror)
		{
			vector.y -= vector2.y;
			vector2.y *= -1f;
		}
		SetLowerLeftPixel(vector);
		SetPixelDimensions(vector2);
	}
}
