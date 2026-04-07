using UnityEngine;

public class PrivacyPolicyList : ScrollableGUI
{
	private GUIDefines.RectInfo mo_scrollAreaRectInfo;

	private float m_ScrollableDistance;

	private float m_PPTextPadding1;

	private GUIDefines.TextureData[] mto_scrollBarTexture;

	private GUIDefines.Vector2Info mv2o_scrollBarStartPos;

	private GUIDefines.Vector2Info mv2o_scrollBarEndPos;

	private int m_UnsentCodeCount;

	private int m_SentCodeCount;

	public PrivacyPolicyList(GameObject aRefObj, GUIDefines.RectInfo ao_scrollableArea, ScrollDirection ae_ScrollDirection)
		: base(aRefObj)
	{
		InitScrollArea(ao_scrollableArea, ae_ScrollDirection, m_ScrollableDistance);
	}

	protected override void CreateLayouts()
	{
		switch (LocalizationManager.GetLanguageCode())
		{
		case "fr":
			switch (ResolutionManager.Instance.LayoutSize)
			{
			case ResolutionManager.eLayoutSize.eLowres:
				m_ScrollableDistance = 38.55f;
				break;
			case ResolutionManager.eLayoutSize.eOriginal:
				m_ScrollableDistance = 40.1f;
				break;
			case ResolutionManager.eLayoutSize.eIPad:
				m_ScrollableDistance = 37.8f;
				break;
			}
			break;
		case "es":
			switch (ResolutionManager.Instance.LayoutSize)
			{
			case ResolutionManager.eLayoutSize.eLowres:
				m_ScrollableDistance = 38.55f;
				break;
			case ResolutionManager.eLayoutSize.eOriginal:
				m_ScrollableDistance = 40f;
				break;
			case ResolutionManager.eLayoutSize.eIPad:
				m_ScrollableDistance = 37.8f;
				break;
			}
			break;
		case "pt":
			switch (ResolutionManager.Instance.LayoutSize)
			{
			case ResolutionManager.eLayoutSize.eLowres:
				m_ScrollableDistance = 36.2f;
				break;
			case ResolutionManager.eLayoutSize.eOriginal:
				m_ScrollableDistance = 37.5f;
				break;
			case ResolutionManager.eLayoutSize.eIPad:
				m_ScrollableDistance = 35.6f;
				break;
			}
			break;
		case "de":
			switch (ResolutionManager.Instance.LayoutSize)
			{
			case ResolutionManager.eLayoutSize.eLowres:
				m_ScrollableDistance = 43.2f;
				break;
			case ResolutionManager.eLayoutSize.eOriginal:
				m_ScrollableDistance = 44.8f;
				break;
			case ResolutionManager.eLayoutSize.eIPad:
				m_ScrollableDistance = 42.2f;
				break;
			}
			break;
		default:
			switch (ResolutionManager.Instance.LayoutSize)
			{
			case ResolutionManager.eLayoutSize.eLowres:
				m_ScrollableDistance = 31.15f;
				break;
			case ResolutionManager.eLayoutSize.eOriginal:
				m_ScrollableDistance = 32.5f;
				break;
			case ResolutionManager.eLayoutSize.eIPad:
				m_ScrollableDistance = 30.5f;
				break;
			}
			break;
		}
		base.LabelData = new GUIDefines.LabelData[2]
		{
			new GUIDefines.LabelData
			{
				pos = new GUIDefines.RectInfo
				{
					widthRatio = 0.4742233f,
					heightRatio = 7f / 160f,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true
					}
				},
				content = new GUIDefines.ContentInfo
				{
					text = LocalizationManager.Instance.GetTOUString("TXT_PP_1")
				},
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customFontSize = GUIDefines.FontSize.eSmall,
					customNormalTextColor = GUIConstants.kWhiteColor,
					useCustomTextAlignment = true,
					customWordWrap = true
				}
			},
			new GUIDefines.LabelData
			{
				pos = new GUIDefines.RectInfo
				{
					widthRatio = 0.4742233f,
					heightRatio = 7f / 160f,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true
					}
				},
				content = new GUIDefines.ContentInfo
				{
					text = LocalizationManager.Instance.GetTOUString("TXT_PP_2")
				},
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customFontSize = GUIDefines.FontSize.eSmall,
					customNormalTextColor = GUIConstants.kWhiteColor,
					useCustomTextAlignment = true,
					customWordWrap = true
				}
			}
		};
		if (ResolutionManager.Instance.LayoutSize == ResolutionManager.eLayoutSize.eIPad)
		{
			mv2o_scrollBarStartPos = new GUIDefines.Vector2Info
			{
				xRatio = 0.73828125f,
				yRatio = 43f / 128f
			};
			mv2o_scrollBarStartPos.Init();
			mv2o_scrollBarEndPos = new GUIDefines.Vector2Info
			{
				xRatio = 0.73828125f,
				yRatio = 0.6791667f
			};
			mv2o_scrollBarEndPos.Init();
			mto_scrollBarTexture = new GUIDefines.TextureData[1]
			{
				new GUIDefines.TextureData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.5f,
						topRatio = 0.5f,
						widthRatio = 0.01367188f,
						heightRatio = 0.11979167f,
						IPad = new GUIDefines.RectIPadInfo
						{
							keepSizeRatio = true
						}
					},
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/CreateAccountNew/TermsOfUsePopup/scrollbar"
					}
				}
			};
		}
		else
		{
			mv2o_scrollBarStartPos = new GUIDefines.Vector2Info
			{
				xRatio = 0.75f,
				yRatio = 19f / 64f
			};
			mv2o_scrollBarStartPos.Init();
			mv2o_scrollBarEndPos = new GUIDefines.Vector2Info
			{
				xRatio = 0.75f,
				yRatio = 0.709375f
			};
			mv2o_scrollBarEndPos.Init();
			mto_scrollBarTexture = new GUIDefines.TextureData[1]
			{
				new GUIDefines.TextureData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.5f,
						topRatio = 0.5f,
						widthRatio = 0.01458333f,
						heightRatio = 23f / 160f,
						IPad = new GUIDefines.RectIPadInfo
						{
							keepSizeRatio = true
						}
					},
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/CreateAccountNew/TermsOfUsePopup/scrollbar"
					}
				}
			};
		}
		for (int i = 0; i < mto_scrollBarTexture.Length; i++)
		{
			mto_scrollBarTexture[i].Init();
		}
	}

	public override void Init(GameObject aRefObj)
	{
		base.Init(aRefObj);
		for (int i = 1; i < base.LabelData.Length; i++)
		{
			GUIContent content = GUIUtil.CreateGuiContent(base.LabelData[i - 1].content);
			GUIStyle guiStyle = GUIUtil.GetGuiStyle(base.LabelData[i - 1].style);
			Vector2 inPixel = new Vector2(0f, guiStyle.CalcHeight(content, base.LabelData[i - 1].pos.inPixel.width * 0.95f));
			base.LabelData[i].style.customPadding = new GUIDefines.Vector2Info
			{
				inPixel = inPixel
			};
		}
	}

	public override void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			mf_scrollPosition = 0f;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			mf_scrollPosition = m_PPTextPadding1 * GUIConstants.kReferenceScreenHeight - 50f;
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			mf_scrollPosition = (m_PPTextPadding1 + m_PPTextPadding1) * GUIConstants.kReferenceScreenHeight - 50f;
		}
		base.Update();
	}

	public override void DrawScrollListContent()
	{
		GUILayout.BeginArea(mo_scrollAreaRect);
		GUIDefines.LabelData[] labelData = base.LabelData;
		foreach (GUIDefines.LabelData labelData2 in labelData)
		{
			GUILayout.BeginArea(mo_scrollAreaInnerRect, labelData2.content.text, GUIUtil.GetGuiStyle(labelData2.style));
			GUILayout.EndArea();
		}
		GUILayout.EndArea();
	}

	public override void DrawBorders()
	{
	}

	public override void DrawScrollBar()
	{
		Vector2 vector = mv2o_scrollBarStartPos.inPixel + base.ScrollPercentage * (mv2o_scrollBarEndPos.inPixel - mv2o_scrollBarStartPos.inPixel);
		mto_scrollBarTexture[0].pos.inPixel.x = vector.x;
		mto_scrollBarTexture[0].pos.inPixel.y = vector.y;
		GUICompoundControls.Textures(base.LocalTransform.position, mto_scrollBarTexture);
	}
}
