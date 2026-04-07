using System;
using System.Collections.Generic;
using UnityEngine;

public class TallyMenu : BaseGUI
{
	public enum Button
	{
		eMenu = 0,
		eNextLevel = 1,
		eReplayLevel = 2,
		eLogin = 3,
		eButton_COUNT = 4
	}

	public enum Textures
	{
		eBigPuffleO = 4
	}

	private enum State
	{
		eState_SlideInAnim = 0,
		eState_SlideOutAnim = 1,
		eState_CountingRingsAndCoins = 2,
		eState_TransferCoinsFX = 3,
		eState_HandlingTransferError = 4,
		eState_WaitingForNextState = 5,
		eState_WaitingForSlideInAnimDone = 6,
		eState_WaitingForLogin = 7,
		eState_WaitingForTransfer = 8,
		eState_Idle = 9,
		eState_None = 10
	}

	private enum LabelIndex
	{
		eLevelCoinCount = 0
	}

	private const float kCoinTransferDurationPer100Coins = 1f;

	private const float k3DObjectIpadXScale = 0.8621f;

	private const float k3DObjectIpadYScale = 0.84006f;

	private const float k3DObjectIpadXOffset = 1.123847f;

	private const float k3DObjectIpadYOffset = 0.4452588f;

	private const float k3DObjectIpadLockedTimeTrialYOffset = 0.45f;

	private const float mTimePerTexture = 0.125f;

	private List<Texture> mProgressRingTextures;

	private List<Texture> mFinishedRingTextures;

	private float mTextureTimer;

	private float mRingTimer;

	private float mCoinTimer;

	private int mMaxIndex;

	private int mCurrentIndex;

	private int mFinishedIndex;

	private State mCurrentState = State.eState_Idle;

	private State mNextState = State.eState_None;

	private float mDeltaTime;

	private float mLastFrameTimestamp;

	private GameObject mCoinTransferObject;

	private CoinSpawner mCoinSpawner;

	private Transform mBlueButtonContainer;

	private Transform mBlueButton;

	private TextMesh mProfileCoinCount;

	private TextMesh mProfileCoinCountDropShadow;

	private ProgressBar mProgressBar;

	private Transform mProgressArrows;

	private GameObject mNewBestPuffleOTextObject;

	private AnimatedText mNewBestPuffleOText;

	private TextMesh mNewBestPuffleOTextMesh;

	private TextMesh mNewBestPuffleOTextMeshShadow;

	private GameObject mNewBestTimeTextObject;

	private AnimatedText mNewBestTimeText;

	private TextMesh mNewBestTimeTextMesh;

	private TextMesh mNewBestTimeTextMeshShadow;

	private GameObject mPopupBgObject;

	private Transform mTotalPuffleOBgTransform;

	private Transform mTotalTimeBgTransform;

	private float mWaitTime;

	private bool mCountingRingsCompleted;

	private bool mTimeTrialUnlocked;

	private bool mRotateArrows;

	private ProgressBar ProgressBar
	{
		get
		{
			if (mProgressBar == null)
			{
				mProgressBar = GameObject.Find("Main Camera").transform.Find("ProgressBar").GetComponent<ProgressBar>();
			}
			return mProgressBar;
		}
	}

	public TallyMenu(GameObject aRefObj)
		: base(aRefObj)
	{
		GameObject.Find("ProgressBar").GetComponent<ProgressBar>().progressText.Show = false;
		mLastFrameTimestamp = Time.realtimeSinceStartup;
	}

	protected override void CreateLayouts()
	{
		mTimeTrialUnlocked = GameManager.HasCollectedAllRings(GameManager.Instance.CurrentWorld);
		float num = 0f;
		if (!mTimeTrialUnlocked)
		{
			num = ((ResolutionManager.Instance.LayoutSize != ResolutionManager.eLayoutSize.eIPad) ? 0.04f : 0.06f);
		}
		base.TextureData = new GUIDefines.TextureData[6]
		{
			new GUIDefines.TextureData
			{
				icon = new GUIDefines.TextureInfo
				{
					name = "GUI/TallyMenu/end-level_popup"
				},
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.1671875f,
					topRatio = 0.004062501f,
					widthRatio = 0.6604167f,
					heightRatio = 0.925f,
					IPad = new GUIDefines.RectIPadInfo
					{
						leftOffset = 23f,
						topOffset = 52f
					}
				}
			},
			new GUIDefines.TextureData
			{
				icon = new GUIDefines.TextureInfo
				{
					name = "GUI/TallyMenu/total_puffle-o_bg"
				},
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.1635416f,
					topRatio = 0.339125f + num,
					widthRatio = 0.67083f,
					heightRatio = 39f / 160f,
					IPad = new GUIDefines.RectIPadInfo
					{
						leftOffset = 23f,
						topOffset = 5f
					}
				}
			},
			new GUIDefines.TextureData
			{
				icon = new GUIDefines.TextureInfo
				{
					name = "GUI/TallyMenu/total_time_bg"
				},
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.1635416f,
					topRatio = 0.4625625f,
					widthRatio = 0.67083f,
					heightRatio = 0.228125f,
					IPad = new GUIDefines.RectIPadInfo
					{
						leftOffset = 23f,
						topOffset = -10f
					}
				},
				invisible = !mTimeTrialUnlocked
			},
			new GUIDefines.TextureData
			{
				icon = new GUIDefines.TextureInfo
				{
					name = "GUI/TallyMenu/mini-puffle-o"
				},
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.478125f,
					topRatio = 0.4063125f + num,
					widthRatio = 0.05f,
					heightRatio = 0.1078125f,
					IPad = new GUIDefines.RectIPadInfo
					{
						leftOffset = -4f,
						topOffset = -4f
					}
				}
			},
			new GUIDefines.TextureData
			{
				icon = new GUIDefines.TextureInfo
				{
					name = "GUI/TallyMenu/ProgressRing/progress_bar_0"
				},
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.4354166f,
					topRatio = 0.111f,
					widthRatio = 0.1333334f,
					heightRatio = 0.246875f,
					IPad = new GUIDefines.RectIPadInfo
					{
						leftOffset = 3f,
						topOffset = 27f
					}
				}
			},
			new GUIDefines.TextureData
			{
				icon = new GUIDefines.TextureInfo
				{
					name = "GUI/TallyMenu/clock"
				},
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.4791667f,
					topRatio = 0.536f,
					widthRatio = 0.04791667f,
					heightRatio = 0.071875f,
					IPad = new GUIDefines.RectIPadInfo
					{
						leftOffset = -4f,
						topOffset = -17f
					}
				},
				invisible = !mTimeTrialUnlocked
			}
		};
		string text = string.Format("0/{0}", ProgressBar.TotalPuffleOs);
		string timeFormatedString = GameManager.GetTimeFormatedString(GameManager.smCurrentTimeCount);
		base.LabelData = new GUIDefines.LabelData[4]
		{
			new GUIDefines.LabelData
			{
				content = new GUIDefines.ContentInfo
				{
					text = text
				},
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.2604167f,
					topRatio = 0.4344375f + num,
					widthRatio = 13f / 64f,
					heightRatio = 7f / 128f,
					IPad = new GUIDefines.RectIPadInfo
					{
						leftOffset = 8f,
						topOffset = -5f
					}
				},
				style = new GUIDefines.StyleInfo
				{
					styleName = "TallyScreenCounter"
				}
			},
			new GUIDefines.LabelData
			{
				content = new GUIDefines.ContentInfo
				{
					text = timeFormatedString
				},
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 53f / 160f,
					topRatio = 0.54225f,
					widthRatio = 21f / 160f,
					heightRatio = 7f / 128f,
					IPad = new GUIDefines.RectIPadInfo
					{
						leftOffset = 10f,
						topOffset = -16f
					}
				},
				style = new GUIDefines.StyleInfo
				{
					styleName = "TallyScreenCounter"
				},
				invisible = !mTimeTrialUnlocked
			},
			new GUIDefines.LabelData
			{
				content = new GUIDefines.ContentInfo
				{
					textId = "TXT_Total"
				},
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.5354167f,
					topRatio = 0.4344375f + num,
					widthRatio = 21f / 160f,
					heightRatio = 7f / 128f,
					IPad = new GUIDefines.RectIPadInfo
					{
						leftOffset = 8f,
						topOffset = -5f
					}
				},
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customFontSize = GUIDefines.FontSize.eMedium,
					customFontType = GUIDefines.FontType.eInGame,
					useCustomTextAlignment = true,
					customTextAlignment = TextAnchor.MiddleLeft
				}
			},
			new GUIDefines.LabelData
			{
				content = new GUIDefines.ContentInfo
				{
					textId = "TXT_Total"
				},
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.5354167f,
					topRatio = 0.54225f,
					widthRatio = 21f / 160f,
					heightRatio = 7f / 128f,
					IPad = new GUIDefines.RectIPadInfo
					{
						leftOffset = 8f,
						topOffset = -16f
					}
				},
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customFontSize = GUIDefines.FontSize.eMedium,
					customFontType = GUIDefines.FontType.eInGame,
					useCustomTextAlignment = true,
					customTextAlignment = TextAnchor.MiddleLeft
				},
				invisible = !mTimeTrialUnlocked
			}
		};
		base.ButtonData = new GUIDefines.ButtonData[3]
		{
			new GUIDefines.ButtonData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.2270833f,
					topRatio = 0.645375f,
					widthRatio = 0.128125f,
					heightRatio = 0.1703126f,
					IPad = new GUIDefines.RectIPadInfo
					{
						leftOffset = 18f,
						topOffset = -31f
					}
				},
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customNormal = new GUIDefines.Texture2DInfo
					{
						name = "GUI/TallyMenu/end-level_menu_button"
					},
					customActive = new GUIDefines.Texture2DInfo
					{
						name = "GUI/TallyMenu/end-level_menu_button_pressed"
					}
				}
			},
			new GUIDefines.ButtonData
			{
				buttonId = 2,
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.3687499f,
					topRatio = 0.645375f,
					widthRatio = 0.128125f,
					heightRatio = 0.1703126f,
					IPad = new GUIDefines.RectIPadInfo
					{
						leftOffset = 8f,
						topOffset = -31f
					}
				},
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customNormal = new GUIDefines.Texture2DInfo
					{
						name = "GUI/TallyMenu/end-level_replay_button"
					},
					customActive = new GUIDefines.Texture2DInfo
					{
						name = "GUI/TallyMenu/end-level_replay_button_pressed"
					}
				}
			},
			new GUIDefines.ButtonData
			{
				buttonId = 1,
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.5072917f,
					topRatio = 0.645375f,
					widthRatio = 0.2625f,
					heightRatio = 27f / 160f,
					IPad = new GUIDefines.RectIPadInfo
					{
						topOffset = -31f
					}
				},
				content = new GUIDefines.ContentInfo
				{
					textId = "TXT_NEXT"
				},
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customNormal = new GUIDefines.Texture2DInfo
					{
						name = "GUI/TallyMenu/end-level_popup_nextlevel-button"
					},
					customActive = new GUIDefines.Texture2DInfo
					{
						name = "GUI/TallyMenu/end-level_popup_nextlevel-button_pressed"
					},
					customFontSize = GUIDefines.FontSize.eMedium,
					customFontType = GUIDefines.FontType.eInGame,
					useCustomTextAlignment = true,
					customTextAlignment = TextAnchor.MiddleLeft,
					customPadding = new GUIDefines.Vector2Info
					{
						xRatio = 0.03645833f
					}
				}
			}
		};
	}

	public override void Draw()
	{
		if (!CanDraw())
		{
			return;
		}
		if (GameFlowManager.Instance.m_DoWindowBack && !GameFlowManager.Instance.GUIManager.IsRateMyAppPopupShowing)
		{
			base.SelectedButton = 0;
			OnButtonSelect();
			GameFlowManager.Instance.m_DoWindowBack = false;
		}
		mDeltaTime = Time.realtimeSinceStartup - mLastFrameTimestamp;
		mLastFrameTimestamp = Time.realtimeSinceStartup;
		UpdateProgressTexture();
		if (mRotateArrows)
		{
			mProgressArrows.Rotate(Vector3.forward, -130f * Time.deltaTime, Space.World);
		}
		if (mCurrentState == State.eState_WaitingForNextState)
		{
			mWaitTime -= mDeltaTime;
			if (mWaitTime < 0f)
			{
				mCurrentState = mNextState;
				mNextState = State.eState_None;
			}
		}
		else
		{
			UpdateRingValue();
		}
		base.Draw();
		BlockControl(false);
	}

	protected override void OnButtonSelect()
	{
		GameFlowManager.Instance.AudioManager.PlayUISFx(GameFlowManager.Instance.MenuClick24);
		GameFlowManager.Instance.GUIManager.ShowTallyMenu(false);
		Camera.main.GetComponent<Light>().enabled = false;
		GameManager.smCurrentLevelRingCount = 0;
		switch ((Button)base.SelectedButton)
		{
		case Button.eMenu:
			UnityEngine.Object.DestroyImmediate(GameObject.Find("TallyMenuCoinTransfer"));
			if (LevelSelect.SelectedLevel - 1 == 23)
			{
				GameFlowManager.Instance.LoadScene("EndCinematic", false);
			}
			else
			{
				GameFlowManager.Instance.LoadScene("LevelSelect", false);
			}
			break;
		case Button.eNextLevel:
			UnityEngine.Object.DestroyImmediate(GameObject.Find("TallyMenuCoinTransfer"));
			if (LevelSelect.SelectedLevel - 1 == 23)
			{
				LevelSelect.SelectedLevel++;
				GameFlowManager.Instance.LoadScene("EndCinematic", false);
				break;
			}
			if (LevelSelect.SelectedLevel - 1 == 59)
			{
				GameFlowManager.Instance.LoadScene("LevelSelect", false);
				break;
			}
			LevelSelect.SelectedLevel++;
			if (LevelSelect.SelectedLevel - 1 <= 11)
			{
				GameManager.Instance.CurrentWorld = GameManager.World.eWorld_BlueSky;
			}
			else if (LevelSelect.SelectedLevel - 1 <= 23)
			{
				GameManager.Instance.CurrentWorld = GameManager.World.eWorld_SodaSunset;
			}
			else if (LevelSelect.SelectedLevel - 1 <= 59)
			{
				GameManager.Instance.CurrentWorld = GameManager.World.eWorld_BonusWorld;
			}
			GameManager.Instance.StartLevel((GameManager.Level)(LevelSelect.SelectedLevel - 1));
			GameFlowManager.Instance.LoadScene("Gameplay", true);
			break;
		case Button.eReplayLevel:
			UnityEngine.Object.DestroyImmediate(GameObject.Find("TallyMenuCoinTransfer"));
			GameManager.Instance.StartLevel((GameManager.Level)(LevelSelect.SelectedLevel - 1));
			GameFlowManager.Instance.LoadScene("Gameplay", true);
			break;
		}
		ResetButton();
	}

	private void UpdateProgressTexture()
	{
		if (mCurrentIndex < mProgressRingTextures.Count)
		{
			mTextureTimer += mDeltaTime;
			if (mTextureTimer > 0.125f)
			{
				mTextureTimer = 0f;
				mCurrentIndex++;
				if (mProgressRingTextures.Count == mCurrentIndex)
				{
					if (mFinishedRingTextures.Count > 0)
					{
						base.TextureData[4].icon.image = mFinishedRingTextures[0];
						mFinishedIndex = 0;
					}
				}
				else
				{
					base.TextureData[4].icon.image = mProgressRingTextures[mCurrentIndex];
				}
			}
		}
		if (mFinishedRingTextures.Count > 0)
		{
			mTextureTimer += mDeltaTime;
			if (mTextureTimer > 0.25f)
			{
				mFinishedIndex = (mFinishedIndex + 1) % 10;
				base.TextureData[4].icon.image = mFinishedRingTextures[mFinishedIndex];
			}
		}
	}

	public void SetCoinTransfer3DObject(GameObject aCoinTransferObject)
	{
		mCoinTransferObject = aCoinTransferObject;
		mBlueButtonContainer = mCoinTransferObject.transform.Find("BlueButtonContainer");
		mBlueButton = mBlueButtonContainer.transform.Find("BlueButton");
		mCoinSpawner = mBlueButton.GetComponent<CoinSpawner>();
		mProgressArrows = mBlueButtonContainer.transform.Find("Arrows");
		Camera.main.GetComponent<Light>().enabled = true;
		mProfileCoinCount = mBlueButton.Find("CoinText").GetComponent<TextMesh>();
		mProfileCoinCountDropShadow = mBlueButton.Find("CoinText").Find("CoinTextDropShadow").GetComponent<TextMesh>();
		mBlueButton.Find("TransferText").GetComponent<Renderer>().enabled = false;
		mBlueButton.Find("TransferText").Find("TransferTextDropShadow").GetComponent<Renderer>().enabled = false;
		mBlueButton.GetComponent<Button3DPressStateController>().Enabled = false;
		mBlueButton.GetComponent<Button3DPressStateController>().onReleased += TallyMenuBlueButton_onReleased;
		mBlueButton.Find("ErrorButton").GetComponent<Button3DPressStateController>().onReleased += TallyMenuErrorButton_onReleased;
		mBlueButton.Find("ErrorButton").gameObject.active = false;
		mBlueButton.Find("TransferText").GetComponent<TextMesh>().text = LocalizationManager.Instance.GetString("TXT_TapToTransfer");
		mBlueButton.Find("TransferText").Find("TransferTextDropShadow").GetComponent<TextMesh>()
			.text = LocalizationManager.Instance.GetString("TXT_TapToTransfer");
		mBlueButton.Find("CoinsTransferredText").GetComponent<TextMesh>().text = LocalizationManager.Instance.GetString("TXT_CoinsTransferred");
		mBlueButton.Find("CoinsTransferredText").Find("CoinsTransferredTextDropShadow").GetComponent<TextMesh>()
			.text = LocalizationManager.Instance.GetString("TXT_CoinsTransferred");
		InitGUITextures();
		if (ResolutionManager.Instance.LayoutSize == ResolutionManager.eLayoutSize.eIPad)
		{
			Vector3 localScale = mBlueButton.localScale;
			localScale.x *= 0.8621f;
			mBlueButton.localScale = localScale;
			Vector3 position = mBlueButtonContainer.Find("Penguin Icon").transform.position;
			position.x -= 1.873847f;
			mBlueButtonContainer.Find("Penguin Icon").transform.position = position;
			position = mProgressArrows.position;
			position.x -= 1.873847f;
			mProgressArrows.position = position;
		}
		mCurrentState = State.eState_SlideInAnim;
	}

	private void InitGUITextures()
	{
		float num = (float)GameManager.smCurrentLevelRingCount / (float)ProgressBar.TotalPuffleOs;
		mMaxIndex = Mathf.FloorToInt(num * 12f) + 1;
		mProgressRingTextures = new List<Texture>(mMaxIndex);
		for (int i = 0; i < mMaxIndex; i++)
		{
			GUIDefines.TextureInfo textureInfo = new GUIDefines.TextureInfo();
			textureInfo.name = "GUI/TallyMenu/ProgressRing/progress_bar_" + i;
			GUIDefines.TextureInfo textureInfo2 = textureInfo;
			textureInfo2.Init();
			mProgressRingTextures.Add(textureInfo2.image);
		}
		mFinishedRingTextures = new List<Texture>();
		if ((double)num == 1.0)
		{
			for (int j = 1; j < 11; j++)
			{
				GUIDefines.TextureInfo textureInfo = new GUIDefines.TextureInfo();
				textureInfo.name = "GUI/TallyMenu/ProgressRing/Finished/finished" + j;
				GUIDefines.TextureInfo textureInfo3 = textureInfo;
				textureInfo3.Init();
				mFinishedRingTextures.Add(textureInfo3.image);
			}
		}
	}

	private void TallyMenuBlueButton_onReleased(object sender, EventArgs e)
	{
		if (!NetManager.Instance.IsPlayerLoggedIn() && !GameFlowManager.Instance.GUIManager.IsUpsellPopupShowing)
		{
			GameFlowManager.Instance.GUIManager.RegisterLoginBackTraceScene();
			GameFlowManager.Instance.GUIManager.ShowLoginPopup(true);
			mBlueButton.GetComponent<Button3DPressStateController>().Enabled = false;
		}
	}

	private void TallyMenuErrorButton_onReleased(object sender, EventArgs e)
	{
		NetManager.Instance.ShowError(NetManager.Instance.GetLastErrorMsg(NetManager.Request.eCoinTransfer), false);
	}

	private void UpdateRingValue()
	{
		switch (mCurrentState)
		{
		case State.eState_SlideInAnim:
			PlaySlideInAnim();
			break;
		case State.eState_SlideOutAnim:
			PlaySlideOutAnim();
			break;
		case State.eState_WaitingForSlideInAnimDone:
			WaitForSlideInAnimDone();
			break;
		case State.eState_CountingRingsAndCoins:
			CountingRings();
			CountingCoins();
			ShowRateThisApp();
			break;
		case State.eState_TransferCoinsFX:
			TransferCoinsEffects();
			break;
		case State.eState_HandlingTransferError:
			HandleTransferError();
			break;
		case State.eState_WaitingForLogin:
			WaitForLogin();
			break;
		case State.eState_WaitingForTransfer:
			WaitForTransfer();
			break;
		case State.eState_WaitingForNextState:
			break;
		}
	}

	private void PlaySlideInAnim()
	{
		mCurrentState = State.eState_WaitingForSlideInAnimDone;
	}

	private void PlaySlideOutAnim()
	{
		mRotateArrows = false;
		if (!NetManager.Instance.HasCoinTransferError())
		{
			mBlueButtonContainer.GetComponent<Animation>()["TallyMenuButtonSlideIn"].speed = -1f;
			mBlueButtonContainer.GetComponent<Animation>()["TallyMenuButtonSlideIn"].time = mBlueButtonContainer.GetComponent<Animation>()["TallyMenuButtonSlideIn"].length;
			mBlueButtonContainer.GetComponent<Animation>().Play("TallyMenuButtonSlideIn");
		}
		mCurrentState = State.eState_Idle;
	}

	private void WaitForSlideInAnimDone()
	{
		if (!mBlueButtonContainer.GetComponent<Animation>().isPlaying)
		{
			int oldTotalCoins = GetOldTotalCoins();
			mProfileCoinCount.text = oldTotalCoins.ToString();
			mProfileCoinCountDropShadow.text = oldTotalCoins.ToString();
			mProfileCoinCount.GetComponent<Renderer>().enabled = true;
			mProfileCoinCountDropShadow.GetComponent<Renderer>().enabled = true;
			mCountingRingsCompleted = false;
			mCurrentState = State.eState_WaitingForNextState;
			mNextState = State.eState_CountingRingsAndCoins;
		}
	}

	private void CountingRings()
	{
		mRingTimer += mDeltaTime;
		float a = mRingTimer / (0.125f * (float)mMaxIndex);
		a = Mathf.Min(a, 1f);
		a *= (float)GameManager.smCurrentLevelRingCount;
		base.LabelData[0].content.text = string.Format("{0}/{1}", Mathf.FloorToInt(a).ToString(), ProgressBar.TotalPuffleOs);
		if (a == (float)GameManager.smCurrentLevelRingCount)
		{
			mCountingRingsCompleted = true;
			mRingTimer = 0f;
		}
	}

	private void CountingCoins()
	{
		mCoinTimer += mDeltaTime;
		float a = 1f - mCoinTimer / (0.125f * (float)mMaxIndex);
		a = Mathf.Max(a, 0f);
		a *= (float)GameManager.smCurrentLevelRingCount;
		int oldTotalCoins = GetOldTotalCoins();
		a = mCoinTimer / (0.125f * (float)mMaxIndex);
		a = Mathf.Min(a, 1f);
		a *= (float)GameManager.smCurrentLevelRingCount;
		a += (float)oldTotalCoins;
		mProfileCoinCount.text = string.Format("{0}", Mathf.FloorToInt(a).ToString());
		mProfileCoinCountDropShadow.text = string.Format("{0}", Mathf.FloorToInt(a).ToString());
		if (!mCountingRingsCompleted || a != (float)(GameManager.smCurrentLevelRingCount + oldTotalCoins))
		{
			return;
		}
		mCurrentState = State.eState_WaitingForNextState;
		mWaitTime = 0.5f;
		mCoinTimer = 0f;
		if (NetManager.Instance.IsPlayerLoggedIn())
		{
			if (NetManager.Instance.IsAnyRequestInProgess())
			{
				mRotateArrows = true;
				mCurrentState = State.eState_WaitingForTransfer;
				mNextState = State.eState_TransferCoinsFX;
			}
			else if (NetManager.Instance.HasCoinTransferError() && !NetManager.Instance.HasReachedCoinTransferLimitError())
			{
				ShowErrorButton();
			}
			else
			{
				mNextState = State.eState_TransferCoinsFX;
			}
		}
		else
		{
			mBlueButton.GetComponent<Button3DPressStateController>().Enabled = true;
			mBlueButton.Find("TransferText").GetComponent<Renderer>().enabled = true;
			mBlueButton.Find("TransferText").Find("TransferTextDropShadow").GetComponent<Renderer>().enabled = true;
			mNextState = State.eState_WaitingForLogin;
		}
	}

	private void TransferCoinsEffects()
	{
		if (!mCoinSpawner.enabled)
		{
			mCoinSpawner.enabled = true;
			if (GameManager.smCurrentLevelRingCount + ProfileManager.Instance.CurrentProfile.TotalCoins != 0 && (!NetManager.Instance.HasCoinTransferError() || NetManager.Instance.HasReachedCoinTransferLimitError()))
			{
				mRotateArrows = true;
				if (!NetManager.Instance.HasReachedCoinTransferLimitError())
				{
					mCoinSpawner.SpawnRing();
				}
			}
		}
		mCoinTimer += mDeltaTime;
		int num = Mathf.Max(GameManager.Instance.CoinsBeforeTransfer - ProfileManager.Instance.CurrentProfile.TotalCoins, 0);
		float num2 = (float)num / 100f * 1f;
		float a = mCoinTimer / num2;
		a = Mathf.Min(a, 1f);
		float num3 = (float)GameManager.Instance.CoinsBeforeTransfer - (float)num * a;
		mProfileCoinCount.text = string.Format("{0}", Mathf.FloorToInt(num3).ToString());
		mProfileCoinCountDropShadow.text = string.Format("{0}", Mathf.FloorToInt(num3).ToString());
		if (num3 == (float)ProfileManager.Instance.CurrentProfile.TotalCoins)
		{
			mNextState = State.eState_SlideOutAnim;
			mCurrentState = State.eState_WaitingForNextState;
			mWaitTime = 2f;
			mCoinSpawner.enabled = false;
			if (!NetManager.Instance.HasReachedCoinTransferLimitError())
			{
				mProfileCoinCount.GetComponent<Renderer>().enabled = false;
				mProfileCoinCountDropShadow.GetComponent<Renderer>().enabled = false;
			}
			if (!NetManager.Instance.HasCoinTransferError())
			{
				mBlueButton.Find("CoinsTransferredText").GetComponent<Renderer>().enabled = true;
				mBlueButton.Find("CoinsTransferredText").Find("CoinsTransferredTextDropShadow").GetComponent<Renderer>().enabled = true;
				mBlueButton.Find("CoinsTransferredText").GetComponent<Animation>().Play();
			}
			else if (NetManager.Instance.HasReachedCoinTransferLimitError())
			{
				ShowErrorButton();
			}
		}
	}

	private void HandleTransferError()
	{
	}

	private void WaitForLogin()
	{
		if (NetManager.Instance.IsPlayerLoggedIn())
		{
			NetManager.Instance.TransferCoins(ProfileManager.Instance.CurrentProfile.TotalCoins, TransferCallback, true);
			mBlueButton.Find("TransferText").GetComponent<Renderer>().enabled = !NetManager.Instance.IsPlayerLoggedIn();
			mBlueButton.Find("TransferText").Find("TransferTextDropShadow").GetComponent<Renderer>().enabled = !NetManager.Instance.IsPlayerLoggedIn();
			mCurrentState = State.eState_Idle;
			mNextState = State.eState_TransferCoinsFX;
		}
		else
		{
			mBlueButton.GetComponent<Button3DPressStateController>().Enabled = true;
		}
	}

	private void WaitForTransfer()
	{
		if (!NetManager.Instance.IsAnyRequestInProgess() && NetManager.Instance.HasCoinTransferCompleted())
		{
			if (NetManager.Instance.HasCoinTransferError())
			{
				ShowErrorButton();
			}
			else
			{
				mCurrentState = mNextState;
			}
		}
	}

	private void TransferCallback(bool aSuccess)
	{
		mProfileCoinCount.text = ProfileManager.Instance.CurrentProfile.TotalCoins.ToString();
		if (aSuccess)
		{
			mCurrentState = State.eState_TransferCoinsFX;
		}
		else
		{
			ShowErrorButton();
		}
	}

	private void ShowErrorButton()
	{
		mRotateArrows = false;
		mCurrentState = State.eState_HandlingTransferError;
		mBlueButton.Find("ErrorButton").gameObject.active = true;
		mBlueButton.Find("ErrorButton").GetComponent<Animation>().Play();
		mBlueButton.Find("ErrorButton").GetComponent<Renderer>().enabled = true;
		mBlueButton.Find("ErrorButton").GetComponent<Button3DPressStateController>().Enabled = true;
		mBlueButton.Find("ErrorButton").GetComponent<ErrorButtonController>().ErrorHappened = true;
	}

	private int GetOldTotalCoins()
	{
		int num = Mathf.Max(GameManager.Instance.CoinsBeforeTransfer, ProfileManager.Instance.CurrentProfile.TotalCoins);
		return Mathf.Max(num - GameManager.smCurrentLevelRingCount, 0);
	}

	public void SetBackground3DObject(GameObject aBackgroundObj)
	{
		mPopupBgObject = aBackgroundObj;
		mTotalPuffleOBgTransform = mPopupBgObject.transform.Find("TotalPuffleOBg");
		mTotalTimeBgTransform = mPopupBgObject.transform.Find("TotalTimeBg");
		if (!mTimeTrialUnlocked)
		{
			Vector3 localPosition = mTotalPuffleOBgTransform.localPosition;
			localPosition.z += 0.45f;
			mTotalPuffleOBgTransform.localPosition = localPosition;
			mTotalTimeBgTransform.GetComponent<MeshRenderer>().enabled = false;
		}
		if (ResolutionManager.Instance.LayoutSize == ResolutionManager.eLayoutSize.eIPad)
		{
			Vector3 localScale = mPopupBgObject.transform.localScale;
			localScale.x *= 0.8621f;
			localScale.z *= 0.84006f;
			mPopupBgObject.transform.localScale = localScale;
			Vector3 position = mPopupBgObject.transform.position;
			position.y += 0.4452588f;
			mPopupBgObject.transform.position = position;
		}
	}

	public void SetNewBestPuffleOText3DObject(GameObject aNewBestPuffleOTextObj)
	{
		mNewBestPuffleOTextObject = aNewBestPuffleOTextObj;
		mNewBestPuffleOText = mNewBestPuffleOTextObject.GetComponent<AnimatedText>();
		mNewBestPuffleOTextMesh = mNewBestPuffleOTextObject.GetComponent<TextMesh>();
		if (mNewBestPuffleOText.textShadow != null)
		{
			mNewBestPuffleOTextMeshShadow = mNewBestPuffleOText.textShadow.GetComponent<TextMesh>();
		}
		mNewBestPuffleOTextMesh.text = LocalizationManager.Instance.GetString("TXT_NewRecord");
		if (mNewBestPuffleOTextMeshShadow != null)
		{
			mNewBestPuffleOTextMeshShadow.text = mNewBestPuffleOTextMesh.text;
		}
		if (!mTimeTrialUnlocked)
		{
			Vector3 position = mNewBestPuffleOTextObject.transform.position;
			position.y -= 1.2662555f;
			mNewBestPuffleOTextObject.transform.position = position;
		}
		if (ResolutionManager.Instance.LayoutSize == ResolutionManager.eLayoutSize.eIPad)
		{
			mNewBestPuffleOText.transform.localScale = IpadTextScale(mNewBestPuffleOText.transform.localScale);
			mNewBestPuffleOText.transform.position = IpadTextOffset(mNewBestPuffleOText.transform.position);
		}
	}

	public void SetNewBestTimeText3DObject(GameObject aNewBestTimeTextObj)
	{
		mNewBestTimeTextObject = aNewBestTimeTextObj;
		mNewBestTimeText = mNewBestTimeTextObject.GetComponent<AnimatedText>();
		mNewBestTimeTextMesh = mNewBestTimeTextObject.GetComponent<TextMesh>();
		if (mNewBestTimeText.textShadow != null)
		{
			mNewBestTimeTextMeshShadow = mNewBestTimeText.textShadow.GetComponent<TextMesh>();
		}
		mNewBestTimeTextMesh.text = LocalizationManager.Instance.GetString("TXT_NewRecord");
		if (mNewBestTimeTextMeshShadow != null)
		{
			mNewBestTimeTextMeshShadow.text = mNewBestTimeTextMesh.text;
		}
		Vector3 position = mNewBestTimeTextObject.transform.position;
		position.y += mTotalTimeBgTransform.position.y - mTotalPuffleOBgTransform.position.y;
		mNewBestTimeTextObject.transform.position = position;
		if (ResolutionManager.Instance.LayoutSize == ResolutionManager.eLayoutSize.eIPad)
		{
			mNewBestTimeTextObject.transform.localScale = IpadTextScale(mNewBestTimeTextObject.transform.localScale);
			mNewBestTimeTextObject.transform.position = IpadTextOffset(mNewBestTimeTextObject.transform.position);
		}
	}

	private Vector3 IpadTextScale(Vector3 aScale)
	{
		Vector3 result = aScale;
		result.x *= 0.8621f;
		result.y *= 0.84006f;
		return result;
	}

	private Vector3 IpadTextOffset(Vector3 aPos)
	{
		Vector3 result = aPos;
		result.x += 1.123847f;
		result.y += 0.4452588f;
		return result;
	}

	private void ShowRateThisApp()
	{
		if (GameManager.smCurrentLevel == GameManager.Level.eLevel_5 && PlayerPrefs.GetInt("RateMyApp") != 10 && PlayerPrefs.GetInt("RateMyAppAtLevel5") > 0)
		{
			PlayerPrefs.SetInt("RateMyAppAtLevel5", 0);
			PlayerPrefs.SetInt("RateMyApp", 0);
			GameFlowManager.Instance.GUIManager.ShowRateMyAppPopup(true);
		}
	}
}
