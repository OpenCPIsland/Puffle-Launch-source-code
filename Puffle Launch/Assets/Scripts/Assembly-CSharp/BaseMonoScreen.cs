using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMonoScreen : MonoBehaviour
{
	public enum TopBarButton
	{
		eBack = 0,
		eCustomButton_Start = 1,
		eTopBarButton_COUNT = 2
	}

	public delegate void TopBarButtonCallback();

	public string iPadTextureEN;

	public string normalTextureEN;

	public string lowresTextureEN;

	public string iPadTextureES;

	public string normalTextureES;

	public string lowresTextureES;

	public string iPadTextureFR;

	public string normalTextureFR;

	public string lowresTextureFR;

	public string iPadTexturePT;

	public string normalTexturePT;

	public string lowresTexturePT;

	public string iPadTextureDE;

	public string normalTextureDE;

	public string lowresTextureDE;

	public string iPadTextureJA;

	public string normalTextureJA;

	public string lowresTextureJA;

	protected string mActiveTexture;

	protected Vector3 mAspectScale;

	private bool mAspectScaleApplied;

	protected BaseScreen m_MainScreen;

	public string msz_back = "TXT_Back";

	public string msz_title = string.Empty;

	private GUIDefines.TextureData[] mto_topBar;

	private GUIDefines.LabelData[] mto_title;

	private GUIDefines.ButtonData[] mto_topBarButtons;

	private TopBarButtonCallback[] m_TopBarCustomCallbacks;

	public BaseScreen MainScreen
	{
		get
		{
			return m_MainScreen;
		}
	}

	protected abstract void CreateMainScreenLayouts();

	protected abstract void OnMainScreenButtonSelect();

	protected abstract void OnBack();

	protected virtual void Init(GameObject aRefObj)
	{
		switch (LocalizationManager.GetLanguageCode())
		{
		default:
			SetTextures(iPadTextureEN, normalTextureEN, lowresTextureEN);
			break;
		case "pt":
			SetTextures(iPadTexturePT, normalTexturePT, lowresTexturePT);
			break;
		case "fr":
			SetTextures(iPadTextureFR, normalTextureFR, lowresTextureFR);
			break;
		case "es":
			SetTextures(iPadTextureES, normalTextureES, lowresTextureES);
			break;
		case "de":
			SetTextures(iPadTextureDE, normalTextureDE, lowresTextureDE);
			break;
		case "ja":
			SetTextures(iPadTextureJA, normalTextureJA, lowresTextureJA);
			break;
		}
		MeshRenderer componentInChildren = GetComponentInChildren<MeshRenderer>();
		if (componentInChildren != null)
		{
			if (!mAspectScaleApplied)
			{
				componentInChildren.transform.position = Vector3.Scale(componentInChildren.transform.position, mAspectScale);
				componentInChildren.transform.localScale = Vector3.Scale(componentInChildren.transform.localScale, mAspectScale);
				mAspectScaleApplied = true;
			}
			for (int i = 0; i < componentInChildren.materials.Length; i++)
			{
				if (mActiveTexture != string.Empty && mActiveTexture != null)
				{
					componentInChildren.materials[i].mainTexture = GUIUtil.LoadTexture(mActiveTexture);
				}
			}
		}
		m_MainScreen = new BaseScreen();
		CreateMainScreenLayouts();
		m_MainScreen.Init(aRefObj);
	}

	protected virtual void BlockControl(bool aBlockControl)
	{
		MainScreen.BlockControl(aBlockControl);
		if (MainScreen.ButtonData != null)
		{
			for (int i = 0; i < MainScreen.ButtonData.Length; i++)
			{
				MainScreen.ButtonData[i].isControlBlocked = aBlockControl;
			}
		}
	}

	protected virtual void HandleButtonSelect()
	{
		if (MainScreen.IsAnyButtonSelected())
		{
			OnMainScreenButtonSelect();
			MainScreen.ResetButton();
		}
		else
		{
			if (MainScreen.IsControlBlocked())
			{
				return;
			}
			if (GameFlowManager.Instance.m_DoWindowBack)
			{
				if (!TouchScreenKeyboard.visible)
				{
					OnBack();
				}
				GameFlowManager.Instance.m_DoWindowBack = false;
			}
			else if (Input.GetKeyUp("menu"))
			{
				OnSettingsButton();
			}
		}
	}

	protected virtual void OnSettingsButton()
	{
	}

	public virtual void Update()
	{
		HandleButtonSelect();
	}

	private void OnDestroy()
	{
		MainScreen.StopGUI();
	}

	public void SetTopBarData(string asz_back, string asz_title)
	{
		SetTopBarData(asz_back, asz_title, null, null);
	}

	public void SetTopBarData(string asz_back, string asz_title, GUIDefines.ButtonData[] aCustomButtons, TopBarButtonCallback[] aCustomCallbacks)
	{
		msz_back = asz_back;
		msz_title = asz_title;
		if (ResolutionManager.Instance.AssetResolution == ResolutionManager.eAssetResolution.eIPad)
		{
			mto_topBar = new GUIDefines.TextureData[1]
			{
				new GUIDefines.TextureData
				{
					pos = new GUIDefines.RectInfo
					{
						widthRatio = 1f,
						heightRatio = 0.10677083f,
						IPad = new GUIDefines.RectIPadInfo
						{
							keepSizeRatio = true
						}
					},
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/Common/iPad/Create_Account_NavBar"
					}
				}
			};
		}
		else
		{
			mto_topBar = new GUIDefines.TextureData[1]
			{
				new GUIDefines.TextureData
				{
					pos = new GUIDefines.RectInfo
					{
						widthRatio = 1f,
						heightRatio = 0.125f
					},
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/Common/Create_Account_NavBar"
					}
				}
			};
		}
		mto_title = new GUIDefines.LabelData[1]
		{
			new GUIDefines.LabelData
			{
				pos = new GUIDefines.RectInfo
				{
					topRatio = 7f / 160f,
					widthRatio = 1f,
					heightRatio = 7f / 160f,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true,
						topOffset = -10f
					}
				},
				content = new GUIDefines.ContentInfo
				{
					textId = msz_title
				}
			}
		};
		GUIDefines.ButtonData buttonData = null;
		if (asz_back != string.Empty)
		{
			if (ResolutionManager.Instance.AssetResolution == ResolutionManager.eAssetResolution.eIPad)
			{
				GUIDefines.ButtonData buttonData2 = new GUIDefines.ButtonData();
				buttonData2.pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.02133333f,
					topRatio = 0.02589583f,
					widthRatio = 0.10839844f,
					heightRatio = 0.06640625f,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true,
						topOffset = -4f
					}
				};
				buttonData2.detectZoneScale = 1.5f;
				buttonData2.content = new GUIDefines.ContentInfo();
				buttonData2.style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customNormal = new GUIDefines.Texture2DInfo
					{
						name = "GUI/Common/iPad/Create_Account_BackBtn_iPad"
					},
					customActive = new GUIDefines.Texture2DInfo
					{
						name = "GUI/Common/iPad/Create_Account_BackBtn_pressed_iPad"
					}
				};
				buttonData = buttonData2;
			}
			else
			{
				GUIDefines.ButtonData buttonData2 = new GUIDefines.ButtonData();
				buttonData2.pos = new GUIDefines.RectInfo
				{
					leftRatio = 3f / 160f,
					topRatio = 0.021875f,
					widthRatio = 7f / 64f,
					heightRatio = 0.0796875f
				};
				buttonData2.detectZoneScale = 1.5f;
				buttonData2.content = new GUIDefines.ContentInfo();
				buttonData2.style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customNormal = new GUIDefines.Texture2DInfo
					{
						name = "GUI/Common/Create_Account_BackBtn"
					},
					customActive = new GUIDefines.Texture2DInfo
					{
						name = "GUI/Common/Create_Account_BackBtn_pressed"
					}
				};
				buttonData = buttonData2;
			}
		}
		List<GUIDefines.ButtonData> list = new List<GUIDefines.ButtonData>();
		if (buttonData != null)
		{
			list.Add(buttonData);
		}
		if (aCustomButtons != null && aCustomButtons.Length > 0)
		{
			Utilities.AssertMsg(aCustomButtons.Length == aCustomCallbacks.Length, "Custom top bar buttons and callbacks size mismatch");
			for (int i = 0; i < aCustomButtons.Length; i++)
			{
				aCustomButtons[i].buttonId = 1 + i;
			}
			list.AddRange(aCustomButtons);
			m_TopBarCustomCallbacks = aCustomCallbacks;
		}
		if (list.Count > 0)
		{
			mto_topBarButtons = list.ToArray();
		}
		if (mto_topBarButtons != null)
		{
			for (int j = 0; j < mto_topBarButtons.Length; j++)
			{
				mto_topBarButtons[j].Init();
			}
		}
		if (mto_topBar != null)
		{
			for (int k = 0; k < mto_topBar.Length; k++)
			{
				mto_topBar[k].Init();
			}
		}
		if (mto_title != null)
		{
			for (int l = 0; l < mto_title.Length; l++)
			{
				mto_title[l].Init();
			}
		}
	}

	public float GetTopBarHeightPixels()
	{
		return mto_topBar[0].pos.inPixel.height;
	}

	public float GetTopBarHeightRatio()
	{
		return mto_topBar[0].pos.heightRatio;
	}

	public void DrawTopBar()
	{
		GUICompoundControls.Textures(MainScreen.LocalTransform.position, mto_topBar);
		int num = 0;
		if (mto_topBarButtons != null)
		{
			num = GUICompoundControls.Buttons(MainScreen.LocalTransform.position, mto_topBarButtons);
		}
		if (!m_MainScreen.IsControlBlocked())
		{
			switch (num)
			{
			case 0:
				OnBack();
				break;
			case 1:
				m_TopBarCustomCallbacks[num - 1]();
				break;
			}
		}
		GUICompoundControls.Labels(MainScreen.LocalTransform.position, mto_title);
	}

	protected void SetTextures(string aIPadTexture, string aNormalTexture, string aLowresTexture)
	{
		switch (ResolutionManager.Instance.AssetResolution)
		{
		case ResolutionManager.eAssetResolution.eLowres:
			if (aLowresTexture == null)
			{
				aLowresTexture = lowresTextureEN;
			}
			mActiveTexture = aLowresTexture;
			mAspectScale = Vector3.one;
			break;
		case ResolutionManager.eAssetResolution.eOriginal:
			if (aNormalTexture == null)
			{
				aNormalTexture = normalTextureEN;
			}
			mActiveTexture = aNormalTexture;
			mAspectScale = Vector3.one;
			break;
		case ResolutionManager.eAssetResolution.eIPad:
			if (aIPadTexture == null)
			{
				aIPadTexture = iPadTextureEN;
			}
			mActiveTexture = aIPadTexture;
			mAspectScale = Vector3.one;
			break;
		}
	}
}
