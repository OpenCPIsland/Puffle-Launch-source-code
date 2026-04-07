using UnityEngine;

public class ColorPickerZone : BaseGUI
{
	public delegate void ColorPickerZoneCallback(int aSelectedButton);

	protected ColorPickerZoneCallback m_Callback;

	private GUIDefines.RectInfo mo_area;

	private string msz_colorBallResource;

	private string msz_colorMaskResource;

	private string msz_colorBgResource;

	private string msz_colorBgHighlightedResource;

	private int mi_elementsPerRow;

	private int mi_elementsPerColumn;

	private GUIDefines.ButtonData[] mto_colorButtons;

	private GUIDefines.TextureData[] mto_colorMasks;

	private GUIDefines.TextureData[] mto_colorBalls;

	private GUIDefines.TextureData[] mto_colorBg;

	private GUIDefines.TextureData[] mto_colorBgHighlighted;

	private GUIDefines.TextureData[] mto_colorBgHighlightedCurrent;

	private Utilities.PenguinColors[] mto_selectableColors = new Utilities.PenguinColors[14]
	{
		Utilities.PenguinColors.eBlue,
		Utilities.PenguinColors.eGreen,
		Utilities.PenguinColors.ePink,
		Utilities.PenguinColors.eBlack,
		Utilities.PenguinColors.eRed,
		Utilities.PenguinColors.eOrange,
		Utilities.PenguinColors.eYellowMustard,
		Utilities.PenguinColors.eDarkPurple,
		Utilities.PenguinColors.eBrown,
		Utilities.PenguinColors.ePeach,
		Utilities.PenguinColors.eDarkGreen,
		Utilities.PenguinColors.eLightBlue,
		Utilities.PenguinColors.eLimeGreen,
		Utilities.PenguinColors.eAqua
	};

	private Utilities.PenguinColors m_SelectedColor;

	public Utilities.PenguinColors SelectedColor
	{
		get
		{
			return m_SelectedColor;
		}
	}

	public ColorPickerZone(GameObject aRefObj, GUIDefines.RectInfo ao_area, Vector2 av2_itemSizeRatio, string asz_colorBallResource, string asz_colorMaskResource, string asz_colorBgResource, string asz_colorBgHighlightedResource, int ai_elementsPerRow, int ai_elementsPerColumn)
		: base(aRefObj)
	{
		mto_colorBgHighlightedCurrent = new GUIDefines.TextureData[1];
		mto_colorBgHighlightedCurrent[0] = null;
		mo_area = ao_area;
		mo_area.Init();
		msz_colorBallResource = asz_colorBallResource;
		msz_colorMaskResource = asz_colorMaskResource;
		msz_colorBgResource = asz_colorBgResource;
		msz_colorBgHighlightedResource = asz_colorBgHighlightedResource;
		mi_elementsPerRow = ai_elementsPerRow;
		mi_elementsPerColumn = ai_elementsPerColumn;
		int num = mto_selectableColors.Length;
		mto_colorButtons = new GUIDefines.ButtonData[num];
		mto_colorMasks = new GUIDefines.TextureData[num];
		mto_colorBalls = new GUIDefines.TextureData[num];
		mto_colorBg = new GUIDefines.TextureData[num];
		mto_colorBgHighlighted = new GUIDefines.TextureData[num];
		GUIDefines.TextureInfo textureInfo = new GUIDefines.TextureInfo
		{
			name = msz_colorBgResource
		};
		textureInfo.Init();
		GUIDefines.TextureInfo textureInfo2 = new GUIDefines.TextureInfo
		{
			name = msz_colorBgHighlightedResource
		};
		textureInfo2.Init();
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < mto_selectableColors.Length; i++)
		{
			Color bgColor = Utilities.m_cPenguinColors[(int)mto_selectableColors[i]];
			Vector2 vector = new Vector2(mo_area.leftRatio + (float)num3 * (mo_area.widthRatio / (float)mi_elementsPerRow), mo_area.topRatio + (float)num2 * (mo_area.heightRatio / (float)mi_elementsPerColumn));
			mto_colorBg[i] = new GUIDefines.TextureData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = vector.x,
					topRatio = vector.y,
					widthRatio = av2_itemSizeRatio.x,
					heightRatio = av2_itemSizeRatio.y,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true
					}
				},
				icon = textureInfo
			};
			mto_colorBg[i].Init();
			mto_colorBgHighlighted[i] = new GUIDefines.TextureData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = vector.x,
					topRatio = vector.y,
					widthRatio = av2_itemSizeRatio.x,
					heightRatio = av2_itemSizeRatio.y,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true
					}
				},
				icon = textureInfo2
			};
			mto_colorBgHighlighted[i].Init();
			mto_colorMasks[i] = new GUIDefines.TextureData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = vector.x,
					topRatio = vector.y,
					widthRatio = av2_itemSizeRatio.x,
					heightRatio = av2_itemSizeRatio.y,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true
					}
				},
				icon = new GUIDefines.TextureInfo
				{
					name = msz_colorMaskResource
				},
				bgInfo = new GUIDefines.BackgroundInfo
				{
					useBgColor = true,
					bgColor = bgColor
				}
			};
			mto_colorMasks[i].Init();
			mto_colorBalls[i] = new GUIDefines.TextureData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = vector.x,
					topRatio = vector.y,
					widthRatio = av2_itemSizeRatio.x,
					heightRatio = av2_itemSizeRatio.y,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true
					}
				},
				icon = new GUIDefines.TextureInfo
				{
					name = msz_colorBallResource
				}
			};
			mto_colorBalls[i].Init();
			mto_colorButtons[i] = new GUIDefines.ButtonData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = vector.x,
					topRatio = vector.y,
					widthRatio = av2_itemSizeRatio.x,
					heightRatio = av2_itemSizeRatio.y,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true
					}
				},
				buttonId = i
			};
			mto_colorButtons[i].Init();
			num3++;
			if (num3 >= mi_elementsPerRow)
			{
				num2++;
				num3 = 0;
			}
		}
		OnColorSelect(0);
	}

	public ColorPickerZone()
	{
	}

	protected override void CreateLayouts()
	{
	}

	public virtual void Update()
	{
	}

	public void RegisterCallback(ColorPickerZoneCallback aCallback)
	{
		m_Callback = aCallback;
	}

	protected override void OnButtonSelect()
	{
		if (m_Callback != null)
		{
			m_Callback(base.SelectedButton);
		}
	}

	protected override void OnButtonSelect(int aSelectedButton)
	{
		if (m_Callback != null)
		{
			m_Callback(aSelectedButton);
		}
	}

	public override void Draw()
	{
		if (CanDraw())
		{
			base.Draw();
			int aSelectedButton = GUICompoundControls.Buttons(base.LocalTransform.position, mto_colorButtons);
			GUICompoundControls.Textures(base.LocalTransform.position, mto_colorBg);
			GUICompoundControls.Textures(base.LocalTransform.position, mto_colorBalls);
			GUICompoundControls.Textures(base.LocalTransform.position, mto_colorMasks);
			if (mto_colorBgHighlightedCurrent[0] != null)
			{
				GUICompoundControls.Textures(base.LocalTransform.position, mto_colorBgHighlightedCurrent);
			}
			OnColorSelect(aSelectedButton);
		}
	}

	private void OnColorSelect(int aSelectedButton)
	{
		if (IsValidButton(aSelectedButton))
		{
			m_SelectedColor = mto_selectableColors[aSelectedButton];
			mto_colorBgHighlightedCurrent[0] = mto_colorBgHighlighted[aSelectedButton];
			base.SelectedButton = aSelectedButton;
			OnButtonSelect();
		}
	}
}
