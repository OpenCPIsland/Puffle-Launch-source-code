using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : BaseMonoScreen
{
	private enum Button
	{
		eBack = 0,
		eWorld1Tab = 1,
		eWorld2Tab = 2,
		eWorldBonusTab = 3,
		eTimeTrial = 4,
		eTurbo = 5,
		eSlowMotion = 6,
		eAppStore = 7,
		eLevelButton_START = 8,
		eButton_COUNT = 9
	}

	private const int kNumLevelsPerWorld = 12;

	private const float kButtonWidthRatio = 0.183f;

	private const float kButtonHeightRatio = 0.275f;

	private const float kButtonOriginX = 0.00821f;

	private const float kButtonStrideX = 0.16042f;

	private const float kPadlockXOffset = 0.063374996f;

	private const float kPadlockYOffset = 0.08f;

	private const float kNewPuffleOXOffset = 0.06442f;

	private const float kNewPuffleOYOffset = 0.065f;

	private const float kNewTextYOffset = 0.0015625f;

	private const float kCompletedPuffleOXOffset = 0.0738f;

	private const float kCompletedPuffleOYOffset = 0.065f;

	private const float kClockXOffset = 0.055f;

	private const float kClockYOffset = 0.1725f;

	private const float kPuffleCounterYOffset = -0.0075f;

	private const float kTimeXOffset = -0.0025f;

	private const float kTimeYOffset = -0.005f;

	private const float kLevelNumXOffset = 0.005f;

	private const float kProgressBarMaxWidth = 0.3385084f;

	private const float kProgressBarPerSectionWidth = 0.11283613f;

	private const float kProgressBarClockSemiWidth = 0.02476384f;

	private const float kProgressBarScale = 0.5610654f;

	private const float kProgressBarCapWidth = 0.01354167f;

	private const int kNumWorlds = 3;

	public TabSubScreen worldTab;

	public int mPrevItemSelected;

	private static LevelSelect m_cInstance;

	private bool isInitialize;

	private int frameCount;

	private float m_StartTime;

	private LevelSelectPopup mo_levelSelectPopup;

	private TimeTrialPopup m_TimeTrialPopup;

	private static int smSelectedLevel = 1;

	private int mPreviousTab;

	private GameObject PageIndicator;

	private float kButtonOriginY = 0.36056f;

	private float kButtonStrideY = 0.24f;

	private LevelButtonController[] mButtonList;

	private string[] mWorldColorSuffixes = new string[3] { "_blue", "_red", "_yellow" };

	private List<Texture> mLevelLockedTextures;

	public static LevelSelect Instance
	{
		get
		{
			return m_cInstance;
		}
	}

	public static int SelectedLevel
	{
		get
		{
			return smSelectedLevel;
		}
		set
		{
			smSelectedLevel = value;
		}
	}

	protected override void CreateMainScreenLayouts()
	{
		ArrayList aTextureData = new ArrayList();
		ArrayList aButtonData = new ArrayList();
		ArrayList aLabelData = new ArrayList();
		CreateCommonLayoutItems(ref aTextureData, ref aButtonData, ref aLabelData);
		base.MainScreen.TextureData = (GUIDefines.TextureData[])aTextureData.ToArray(typeof(GUIDefines.TextureData));
		base.MainScreen.ButtonData = (GUIDefines.ButtonData[])aButtonData.ToArray(typeof(GUIDefines.ButtonData));
		base.MainScreen.LabelData = (GUIDefines.LabelData[])aLabelData.ToArray(typeof(GUIDefines.LabelData));
	}

	private void CreateCommonLayoutItems(ref ArrayList aTextureData, ref ArrayList aButtonData, ref ArrayList aLabelData)
	{
		switch (GameManager.Instance.CurrentWorld)
		{
		case GameManager.World.eWorld_BlueSky:
			aTextureData.Add(new GUIDefines.TextureData
			{
				pos = new GUIDefines.RectInfo
				{
					topRatio = 0.18f,
					widthRatio = 1f,
					heightRatio = 0.19531f,
					IPad = new GUIDefines.RectIPadInfo
					{
						topOffset = 61f
					}
				},
				icon = new GUIDefines.TextureInfo
				{
					name = "GUI/LevelSelect/world_tab_blue-front"
				}
			});
			break;
		case GameManager.World.eWorld_SodaSunset:
			aTextureData.Add(new GUIDefines.TextureData
			{
				pos = new GUIDefines.RectInfo
				{
					topRatio = 0.18f,
					widthRatio = 1f,
					heightRatio = 0.19531f,
					IPad = new GUIDefines.RectIPadInfo
					{
						topOffset = 61f
					}
				},
				icon = new GUIDefines.TextureInfo
				{
					name = "GUI/LevelSelect/world_tab_red-front"
				}
			});
			break;
		case GameManager.World.eWorld_BonusWorld:
			aTextureData.Add(new GUIDefines.TextureData
			{
				pos = new GUIDefines.RectInfo
				{
					topRatio = 0.18f,
					widthRatio = 1f,
					heightRatio = 0.19531f,
					IPad = new GUIDefines.RectIPadInfo
					{
						topOffset = 61f
					}
				},
				icon = new GUIDefines.TextureInfo
				{
					name = "GUI/LevelSelect/world_tab_yellow-front"
				}
			});
			break;
		}
		if (GameManager.Instance.CurrentWorld != GameManager.World.eWorld_BonusWorld)
		{
			aTextureData.Add(new GUIDefines.TextureData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.4808334f,
					topRatio = 0.8686875f,
					widthRatio = 0.2239583f,
					heightRatio = 0.1046875f,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true,
						leftOffset = 6f,
						topOffset = -2f,
						widthScale = -10f,
						heightScale = -14f
					}
				},
				icon = new GUIDefines.TextureInfo
				{
					name = "GUI/LevelSelect/LevelSelect_Turbo_SloMo_Bar"
				}
			});
			aTextureData.Add(new GUIDefines.TextureData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.7475f,
					topRatio = 0.8686875f,
					widthRatio = 0.2239583f,
					heightRatio = 0.1046875f,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true,
						leftOffset = -7f,
						topOffset = -2f,
						widthScale = -10f,
						heightScale = -14f
					}
				},
				icon = new GUIDefines.TextureInfo
				{
					name = "GUI/LevelSelect/LevelSelect_Turbo_SloMo_Bar"
				}
			});
			aTextureData.Add(new GUIDefines.TextureData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 7f / 160f,
					topRatio = 0.9015625f,
					widthRatio = 0.3833f,
					heightRatio = 0.043625f,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true,
						leftOffset = -2f,
						topOffset = -10f
					}
				},
				icon = new GUIDefines.TextureInfo
				{
					name = "GUI/LevelSelect/time-trial_bg"
				}
			});
			GameManager.LevelTimes aCompletedLevelTime;
			float aCompletedPercentage;
			GameManager.RetrieveTimeTrialCompletion(GameManager.Instance.CurrentWorld, out aCompletedLevelTime, out aCompletedPercentage);
			float num = aCompletedPercentage * 0.11283613f * 0.5610654f + 0.02476384f;
			switch (aCompletedLevelTime)
			{
			case GameManager.LevelTimes.eTime_Silver:
				num += 0.11283613f;
				break;
			case GameManager.LevelTimes.eTime_Gold:
				num += 0.22567226f;
				break;
			case GameManager.LevelTimes.eTime_Fire:
				num = 0.3385084f;
				break;
			}
			if (num > 0f)
			{
				num -= 0.01354167f;
				aTextureData.Add(new GUIDefines.TextureData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.06770834f,
						topRatio = 0.9f,
						widthRatio = num,
						heightRatio = 0.043625f,
						IPad = new GUIDefines.RectIPadInfo
						{
							keepWidthRatio = true,
							topOffset = -7f
						}
					},
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/LevelSelect/LevelSelect_TimeTrial_ProgressBar"
					}
				});
				aTextureData.Add(new GUIDefines.TextureData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.06770834f + num,
						topRatio = 0.896875f,
						widthRatio = 0.01354167f,
						heightRatio = 0.05143747f,
						IPad = new GUIDefines.RectIPadInfo
						{
							topOffset = -7f
						}
					},
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/LevelSelect/time-trial_filler_cap"
					}
				});
			}
			string text = ((!GameManager.HasCollectedAllRings(GameManager.Instance.CurrentWorld)) ? "GUI/LevelSelect/LevelSelect_TimeTrial_ProgressBar4_Box_Locked" : "GUI/LevelSelect/LevelSelect_TimeTrial_ProgressBar4_Box");
			aTextureData.Add(new GUIDefines.TextureData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 1f / 32f,
					topRatio = 0.8625f,
					widthRatio = 0.4114583f,
					heightRatio = 15f / 128f,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true,
						topOffset = -1f,
						widthScale = -9f,
						heightScale = -17f
					}
				},
				icon = new GUIDefines.TextureInfo
				{
					name = text
				}
			});
			text = ((!GameManager.HasCollectedAllRings(GameManager.Instance.CurrentWorld)) ? "GUI/LevelSelect/LevelSelect_Locked" : "GUI/LevelSelect/LevelSelect_TimeTrial_BlueClock");
			aTextureData.Add(new GUIDefines.TextureData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.04270833f,
					topRatio = 113f / 128f,
					widthRatio = 0.05312503f,
					heightRatio = 0.0765625f,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true,
						topOffset = -3f,
						widthScale = -6f,
						heightScale = -12f
					}
				},
				icon = new GUIDefines.TextureInfo
				{
					name = text
				}
			});
			text = ((!GameManager.Instance.HasAchievedTimeTrialSilver(GameManager.Instance.CurrentWorld)) ? "GUI/LevelSelect/LevelSelect_Locked" : "GUI/LevelSelect/LevelSelect_TimeTrial_SilverClock");
			aTextureData.Add(new GUIDefines.TextureData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.1541667f,
					topRatio = 113f / 128f,
					widthRatio = 0.05312503f,
					heightRatio = 0.0765625f,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true,
						topOffset = -3f,
						widthScale = -6f,
						heightScale = -12f
					}
				},
				icon = new GUIDefines.TextureInfo
				{
					name = text
				}
			});
			text = ((!GameManager.Instance.HasAchievedTimeTrialGold(GameManager.Instance.CurrentWorld)) ? "GUI/LevelSelect/LevelSelect_Locked" : "GUI/LevelSelect/LevelSelect_TimeTrial_GoldClock");
			aTextureData.Add(new GUIDefines.TextureData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.2677084f,
					topRatio = 113f / 128f,
					widthRatio = 0.05312503f,
					heightRatio = 0.0765625f,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true,
						leftOffset = -3f,
						topOffset = -3f,
						widthScale = -6f,
						heightScale = -12f
					}
				},
				icon = new GUIDefines.TextureInfo
				{
					name = text
				}
			});
			text = ((!GameManager.Instance.HasAchievedTimeTrialFire(GameManager.Instance.CurrentWorld)) ? "GUI/LevelSelect/LevelSelect_Locked" : "GUI/LevelSelect/LevelSelect_TimeTrial_Turbo");
			if (GameManager.Instance.HasAchievedTimeTrialFire(GameManager.Instance.CurrentWorld))
			{
				aTextureData.Add(new GUIDefines.TextureData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.378125f,
						topRatio = 0.840625f,
						widthRatio = 0.05312503f,
						heightRatio = 0.1140625f,
						IPad = new GUIDefines.RectIPadInfo
						{
							keepSizeRatio = true,
							leftOffset = -7f,
							widthScale = -3f,
							heightScale = -12f
						}
					},
					icon = new GUIDefines.TextureInfo
					{
						name = text
					}
				});
			}
			else
			{
				aTextureData.Add(new GUIDefines.TextureData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.378125f,
						topRatio = 113f / 128f,
						widthRatio = 0.05312503f,
						heightRatio = 0.0765625f,
						IPad = new GUIDefines.RectIPadInfo
						{
							keepSizeRatio = true,
							leftOffset = -5f,
							topOffset = -4f,
							widthScale = -6f,
							heightScale = -12f
						}
					},
					icon = new GUIDefines.TextureInfo
					{
						name = text
					}
				});
			}
		}
		aButtonData.Add(new GUIDefines.ButtonData
		{
			pos = new GUIDefines.RectInfo
			{
				leftRatio = 0.01979f,
				topRatio = 0.025f,
				widthRatio = 0.09895834f,
				heightRatio = 0.0765625f
			},
			detectZoneScale = 1.5f,
			style = new GUIDefines.StyleInfo
			{
				styleName = "LightGrayButton"
			}
		});
		aButtonData.Add(new GUIDefines.ButtonData
		{
			buttonId = 1,
			pos = new GUIDefines.RectInfo
			{
				leftRatio = 0.017708f,
				topRatio = 0.16575f,
				widthRatio = 0.333333f,
				heightRatio = 0.14f,
				IPad = new GUIDefines.RectIPadInfo
				{
					leftOffset = 7f,
					topOffset = 63f
				}
			},
			content = new GUIDefines.ContentInfo
			{
				textId = "TXT_Area1"
			},
			style = new GUIDefines.StyleInfo
			{
				styleName = "InGameTextSmall"
			}
		});
		aButtonData.Add(new GUIDefines.ButtonData
		{
			buttonId = 2,
			pos = new GUIDefines.RectInfo
			{
				leftRatio = 0.333333f,
				topRatio = 0.16575f,
				widthRatio = 0.333333f,
				heightRatio = 0.14f,
				IPad = new GUIDefines.RectIPadInfo
				{
					leftOffset = 10f,
					topOffset = 63f
				}
			},
			content = new GUIDefines.ContentInfo
			{
				textId = "TXT_Area2"
			},
			style = new GUIDefines.StyleInfo
			{
				styleName = "InGameTextSmall"
			}
		});
		aButtonData.Add(new GUIDefines.ButtonData
		{
			buttonId = 3,
			pos = new GUIDefines.RectInfo
			{
				leftRatio = 0.648958f,
				topRatio = 0.16575f,
				widthRatio = 0.333333f,
				heightRatio = 0.14f,
				IPad = new GUIDefines.RectIPadInfo
				{
					leftOffset = 13f,
					topOffset = 63f
				}
			},
			content = new GUIDefines.ContentInfo
			{
				textId = "TXT_Bonus"
			},
			style = new GUIDefines.StyleInfo
			{
				styleName = "InGameTextSmall"
			}
		});
		if (GameManager.Instance.CurrentWorld != GameManager.World.eWorld_BonusWorld)
		{
			aButtonData.Add(new GUIDefines.ButtonData
			{
				buttonId = 4,
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 1f / 32f,
					topRatio = 0.8625f,
					widthRatio = 0.4114583f,
					heightRatio = 15f / 128f,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true,
						leftOffset = -4f,
						topOffset = -10f
					}
				}
			});
			aButtonData.Add(new GUIDefines.ButtonData
			{
				buttonId = 5,
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.4770834f,
					topRatio = 0.8716251f,
					widthRatio = 0.2333333f,
					heightRatio = 0.1f,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true,
						topOffset = -9f
					}
				}
			});
			if (GameManager.Instance.HasAchievedTimeTrialFire(GameManager.Instance.CurrentWorld))
			{
				aTextureData.Add(new GUIDefines.TextureData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.4854166f,
						topRatio = 0.846625f,
						widthRatio = 0.06041666f,
						heightRatio = 0.1234375f,
						IPad = new GUIDefines.RectIPadInfo
						{
							keepSizeRatio = true,
							leftOffset = 5f,
							topOffset = -1f,
							widthScale = -3f,
							heightScale = -14f
						}
					},
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/LevelSelect/LevelSelect_Turbo_Icon"
					}
				});
			}
			else
			{
				aTextureData.Add(new GUIDefines.TextureData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.4885416f,
						topRatio = 0.8809999f,
						widthRatio = 0.05416667f,
						heightRatio = 13f / 160f,
						IPad = new GUIDefines.RectIPadInfo
						{
							keepSizeRatio = true,
							leftOffset = 6f,
							topOffset = -4f,
							widthScale = -3f,
							heightScale = -10f
						}
					},
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/LevelSelect/LevelSelect_Locked"
					}
				});
			}
			aButtonData.Add(new GUIDefines.ButtonData
			{
				buttonId = 6,
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 89f / 120f,
					topRatio = 0.8716251f,
					widthRatio = 0.2333333f,
					heightRatio = 0.1f,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true,
						leftOffset = -11f,
						topOffset = -10f
					}
				}
			});
			if (GameManager.HasCompletedTurboMode(GameManager.Instance.CurrentWorld))
			{
				aTextureData.Add(new GUIDefines.TextureData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 145f / 192f,
						topRatio = 0.8809999f,
						widthRatio = 0.05624994f,
						heightRatio = 0.0796875f,
						IPad = new GUIDefines.RectIPadInfo
						{
							keepSizeRatio = true,
							leftOffset = -8f,
							topOffset = -5f,
							widthScale = -3f,
							heightScale = -8f
						}
					},
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/LevelSelect/LevelSelect_SloMo_Icon"
					}
				});
			}
			else
			{
				aTextureData.Add(new GUIDefines.TextureData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 145f / 192f,
						topRatio = 0.8809999f,
						widthRatio = 0.05416667f,
						heightRatio = 13f / 160f,
						IPad = new GUIDefines.RectIPadInfo
						{
							keepSizeRatio = true,
							leftOffset = -7f,
							topOffset = -5f,
							widthScale = -4f,
							heightScale = -9f
						}
					},
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/LevelSelect/LevelSelect_Locked"
					}
				});
			}
		}
		int levelCompletion = GameManager.GetLevelCompletion(GameManager.Instance.CurrentWorld);
		string text2 = levelCompletion + " " + LocalizationManager.Instance.GetString("TXT_Of") + " 12";
		float leftRatio = 0.184375f;
		float leftOffset = 0f;
		if (GameManager.Instance.CurrentWorld == GameManager.World.eWorld_SodaSunset)
		{
			leftRatio = 0.5f;
			leftOffset = 0f;
		}
		if (GameManager.Instance.CurrentWorld == GameManager.World.eWorld_BonusWorld)
		{
			text2 = levelCompletion + " " + LocalizationManager.Instance.GetString("TXT_Of") + " " + 36;
			leftRatio = 49f / 60f;
			leftOffset = 0f;
		}
		aLabelData.Add(new GUIDefines.LabelData
		{
			pos = new GUIDefines.RectInfo
			{
				leftRatio = leftRatio,
				topRatio = 0.2953125f,
				IPad = new GUIDefines.RectIPadInfo
				{
					leftOffset = leftOffset,
					topOffset = 44f
				}
			},
			content = new GUIDefines.ContentInfo
			{
				text = text2
			},
			style = new GUIDefines.StyleInfo
			{
				useCustomStyle = true,
				customFontSize = GUIDefines.FontSize.eSmall,
				customFontType = GUIDefines.FontType.eInGame,
				customTextAlignment = TextAnchor.MiddleCenter,
				customNormal = new GUIDefines.Texture2DInfo
				{
					name = "GUI/Common/semi_transparent"
				},
				customNormalTextColor = ((GameManager.Instance.CurrentWorld != GameManager.World.eWorld_BlueSky) ? GUIConstants.kBlackColor : GUIConstants.kLevelSelectBlueTextColor)
			}
		});
		if (GameManager.Instance.CurrentWorld != GameManager.World.eWorld_BonusWorld)
		{
			aLabelData.Add(new GUIDefines.LabelData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.5727917f,
					topRatio = 0.8949375f,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true,
						topOffset = -4f
					}
				},
				content = new GUIDefines.ContentInfo
				{
					textId = "TXT_TurboAllCaps"
				},
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customFontSize = GUIDefines.FontSize.eSmall,
					customFontType = GUIDefines.FontType.eInGame,
					useCustomTextAlignment = true
				}
			});
			aLabelData.Add(new GUIDefines.LabelData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.8881251f,
					topRatio = 0.8949375f,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true,
						leftOffset = -15f,
						topOffset = -3f
					}
				},
				content = new GUIDefines.ContentInfo
				{
					textId = "TXT_SlowMoAllCaps"
				},
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customFontSize = GUIDefines.FontSize.eSmall,
					customFontType = GUIDefines.FontType.eInGame,
					useCustomTextAlignment = true,
					customTextAlignment = TextAnchor.UpperCenter
				}
			});
		}
	}

	private void Start()
	{
		AudioManager.Instance.PlayMusic(AudioManager.MusicTrack.eMusic_Menu);
	}

	private void CreateLevelButtons(ref ArrayList aTextureData, ref ArrayList aButtonData, ref ArrayList aLabelData)
	{
		if (ResolutionManager.Instance.LayoutSize == ResolutionManager.eLayoutSize.eIPad)
		{
			kButtonOriginY = 0.38556f;
			kButtonStrideY = 0.205f;
		}
		for (int i = 0; i < 12; i++)
		{
			int currentWorld = (int)GameManager.Instance.CurrentWorld;
			int num = i + currentWorld * 12;
			Vector2 vector = new Vector2(0.00821f + 0.16042f * (float)(i % 6), kButtonOriginY + kButtonStrideY * (float)(i * 2 / 12));
			if (ProfileManager.Instance.CurrentProfile.m_LevelData[num].LevelComplete)
			{
				string text = "GUI/LevelSelect/puffle-o_fire";
				if (!ProfileManager.Instance.CurrentProfile.m_LevelData[num].TurboLevelComplete)
				{
					float num2 = (float)ProfileManager.Instance.CurrentProfile.m_LevelData[num].BestRingCount / (float)GameManager.smMaxRingInLevel[num];
					text = ((num2 >= 1f) ? "GUI/LevelSelect/puffle-o_gold" : ((!(num2 >= 0.5f)) ? "GUI/LevelSelect/puffle-o_orange" : "GUI/LevelSelect/puffle-o_silver"));
				}
				aTextureData.Add(new GUIDefines.TextureData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = vector.x + 0.0738f,
						topRatio = vector.y + 0.065f,
						widthRatio = 0.0354167f,
						heightRatio = 11f / 160f,
						IPad = new GUIDefines.RectIPadInfo
						{
							leftOffset = -4f,
							topOffset = -4f
						}
					},
					icon = new GUIDefines.TextureInfo
					{
						name = text
					}
				});
				string text2 = string.Format("{0}/{1}", ProfileManager.Instance.CurrentProfile.m_LevelData[num].BestRingCount, GameManager.smMaxRingInLevel[num]);
				aLabelData.Add(new GUIDefines.LabelData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = vector.x,
						topRatio = vector.y + -0.0075f,
						IPad = new GUIDefines.RectIPadInfo
						{
							leftOffset = -6f,
							topOffset = -9f
						}
					},
					content = new GUIDefines.ContentInfo
					{
						text = text2
					},
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customFontType = GUIDefines.FontType.eInGame,
						customPadding = new GUIDefines.Vector2Info
						{
							xRatio = 0.18229f,
							yRatio = 0.31f,
							IPad = new GUIDefines.Vector2IPadInfo
							{
								yOffset = -5f
							}
						}
					}
				});
				if (GameManager.HasCollectedAllRings(GameManager.Instance.CurrentWorld) && ProfileManager.Instance.CurrentProfile.m_LevelData[num].BestTimeCount != float.MaxValue)
				{
					aTextureData.Add(new GUIDefines.TextureData
					{
						pos = new GUIDefines.RectInfo
						{
							leftRatio = vector.x + 0.055f,
							topRatio = vector.y + 0.1725f,
							widthRatio = 0.02083332f,
							heightRatio = 0.034375f,
							IPad = new GUIDefines.RectIPadInfo
							{
								leftOffset = -4f,
								topOffset = -14f
							}
						},
						icon = new GUIDefines.TextureInfo
						{
							name = "GUI/LevelSelect/clock"
						}
					});
					string timeFormatedString = GameManager.GetTimeFormatedString(ProfileManager.Instance.CurrentProfile.m_LevelData[num].BestTimeCount);
					aLabelData.Add(new GUIDefines.LabelData
					{
						pos = new GUIDefines.RectInfo
						{
							leftRatio = vector.x + -0.0025f,
							topRatio = vector.y + -0.005f,
							IPad = new GUIDefines.RectIPadInfo
							{
								leftOffset = -7f,
								topOffset = -14f
							}
						},
						content = new GUIDefines.ContentInfo
						{
							text = timeFormatedString
						},
						style = new GUIDefines.StyleInfo
						{
							useCustomStyle = true,
							customFontType = GUIDefines.FontType.eInGame,
							customPadding = new GUIDefines.Vector2Info
							{
								xRatio = 0.21f,
								yRatio = 0.39f,
								IPad = new GUIDefines.Vector2IPadInfo
								{
									yOffset = -5f
								}
							},
							customNormalTextColor = ((GameManager.Instance.CurrentWorld != GameManager.World.eWorld_BlueSky) ? GUIConstants.kLevelSelectOrangeTextColor : GUIConstants.kLevelSelectBlueTextColor)
						}
					});
				}
			}
			else if (ProfileManager.Instance.CurrentProfile.m_LevelData[num].LevelUnlocked)
			{
				aTextureData.Add(new GUIDefines.TextureData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = vector.x + 0.06442f,
						topRatio = vector.y + 0.065f,
						widthRatio = 0.054167f,
						heightRatio = 13f / 128f,
						IPad = new GUIDefines.RectIPadInfo
						{
							leftOffset = -4f,
							topOffset = -5f
						}
					},
					icon = new GUIDefines.TextureInfo
					{
						name = "GUI/LevelSelect/puffle-o_new"
					}
				});
				aLabelData.Add(new GUIDefines.LabelData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = vector.x,
						topRatio = vector.y + 0.0015625f,
						IPad = new GUIDefines.RectIPadInfo
						{
							leftOffset = -7f,
							topOffset = 3f
						}
					},
					content = new GUIDefines.ContentInfo
					{
						textId = "TXT_New"
					},
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customFontSize = GUIDefines.FontSize.eSmall,
						customFontType = GUIDefines.FontType.eInGame,
						customPadding = new GUIDefines.Vector2Info
						{
							xRatio = 0.18229f,
							yRatio = 0.38f,
							IPad = new GUIDefines.Vector2IPadInfo
							{
								yOffset = -38f
							}
						},
						customNormalTextColor = GUIConstants.kLevelSelectNewTextColor
					}
				});
			}
			else
			{
				aTextureData.Add(new GUIDefines.TextureData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = vector.x + 0.063374996f,
						topRatio = vector.y + 0.08f,
						widthRatio = 9f / 160f,
						heightRatio = 0.1109375f,
						IPad = new GUIDefines.RectIPadInfo
						{
							leftOffset = -4f,
							topOffset = -2f
						}
					},
					icon = new GUIDefines.TextureInfo
					{
						image = mLevelLockedTextures[currentWorld]
					}
				});
			}
			aButtonData.Add(new GUIDefines.ButtonData
			{
				buttonId = 8 + i,
				pos = new GUIDefines.RectInfo
				{
					leftRatio = vector.x + 0.005f,
					topRatio = vector.y,
					widthRatio = 0.183f,
					heightRatio = 0.275f,
					IPad = new GUIDefines.RectIPadInfo
					{
						keepSizeRatio = true,
						leftOffset = -7f
					}
				},
				content = new GUIDefines.ContentInfo
				{
					text = (num + 1).ToString()
				},
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customFontType = GUIDefines.FontType.eInGame,
					customPadding = new GUIDefines.Vector2Info
					{
						xRatio = 0.04167f,
						yRatio = 0.06f,
						IPad = new GUIDefines.Vector2IPadInfo
						{
							yOffset = -6f
						}
					},
					customNormalTextColor = ((GameManager.Instance.CurrentWorld != GameManager.World.eWorld_BlueSky) ? GUIConstants.kLevelSelectOrangeTextColor : GUIConstants.kLevelSelectBlueTextColor),
					useCustomTextAlignment = true
				}
			});
		}
	}

	private void Awake()
	{
		m_cInstance = this;
		m_StartTime = Time.realtimeSinceStartup;
		PageIndicator = GameObject.Find("PageIndicator");
		mPreviousTab = (int)GameManager.Instance.CurrentWorld;
		AssetLoader.Instance.EmptyScrollList.SetActiveRecursively(false);
		AssetLoader.Instance.ScrollList.SetActiveRecursively(true);
		mButtonList = AssetLoader.Instance.ScrollList.GetComponentsInChildren<LevelButtonController>();
		ChangeScrollList();
		InitGUITextures();
		Init(base.gameObject);
		mo_levelSelectPopup = new LevelSelectPopup(base.gameObject);
		m_TimeTrialPopup = new TimeTrialPopup(base.gameObject);
	}

	private void InitGUITextures()
	{
		mLevelLockedTextures = new List<Texture>(3);
		for (int i = 0; i < 3; i++)
		{
			GUIDefines.TextureInfo textureInfo = new GUIDefines.TextureInfo();
			textureInfo.name = "GUI/LevelSelect/level_lock" + mWorldColorSuffixes[i];
			GUIDefines.TextureInfo textureInfo2 = textureInfo;
			textureInfo2.Init();
			mLevelLockedTextures.Add(textureInfo2.image);
		}
	}

	private void OnGUI()
	{
		if (base.MainScreen.CanDraw())
		{
			base.MainScreen.Draw();
			mo_levelSelectPopup.Draw();
			m_TimeTrialPopup.Draw();
			BlockControl(mo_levelSelectPopup.IsShowing || m_TimeTrialPopup.IsShowing);
		}
	}

	protected override void Init(GameObject aRefObj)
	{
		base.Init(aRefObj);
		m_MainScreen.BlockControl(true);
	}

	protected override void OnMainScreenButtonSelect()
	{
		if (Time.realtimeSinceStartup - m_StartTime <= 0.3f)
		{
			return;
		}
		switch ((Button)base.MainScreen.SelectedButton)
		{
		case Button.eBack:
			OnBack();
			break;
		case Button.eWorld1Tab:
			GameManager.Instance.CurrentWorld = GameManager.World.eWorld_BlueSky;
			ChangeScrollList();
			UpdateTabStrip();
			break;
		case Button.eWorld2Tab:
			GameManager.Instance.CurrentWorld = GameManager.World.eWorld_SodaSunset;
			ChangeScrollList();
			UpdateTabStrip();
			break;
		case Button.eWorldBonusTab:
			GameManager.Instance.CurrentWorld = GameManager.World.eWorld_BonusWorld;
			ChangeScrollList();
			UpdateTabStrip();
			break;
		case Button.eTimeTrial:
			GameObject.Find("LevelSelect").GetComponent<LevelSelectManager>().RequestChangeButtonsState(false);
			if (GameManager.HasCollectedAllRings(GameManager.Instance.CurrentWorld))
			{
				m_TimeTrialPopup.Show(true);
				break;
			}
			mo_levelSelectPopup.SetPageID(LevelSelectPopup.PageID.TimeTrialLocked);
			mo_levelSelectPopup.Show(true);
			break;
		case Button.eTurbo:
			GameObject.Find("LevelSelect").GetComponent<LevelSelectManager>().RequestChangeButtonsState(false);
			if (GameManager.Instance.HasAchievedTimeTrialFire(GameManager.Instance.CurrentWorld))
			{
				mo_levelSelectPopup.SetPageID(LevelSelectPopup.PageID.TurboModeInstructions);
				mo_levelSelectPopup.Show(true);
			}
			else
			{
				mo_levelSelectPopup.SetPageID(LevelSelectPopup.PageID.TurboModeLocked);
				mo_levelSelectPopup.Show(true);
			}
			break;
		case Button.eSlowMotion:
			GameObject.Find("LevelSelect").GetComponent<LevelSelectManager>().RequestChangeButtonsState(false);
			if (GameManager.HasCompletedTurboMode(GameManager.Instance.CurrentWorld))
			{
				mo_levelSelectPopup.SetPageID(LevelSelectPopup.PageID.SlowMotionInstructions);
				mo_levelSelectPopup.Show(true);
			}
			else
			{
				mo_levelSelectPopup.SetPageID(LevelSelectPopup.PageID.SlowMotionLocked);
				mo_levelSelectPopup.Show(true);
			}
			break;
		case Button.eAppStore:
			Application.OpenURL("market://details?id=com.disney.PuffleLaunch");
			break;
		}
	}

	protected override void OnBack()
	{
		base.MainScreen.StopGUI();
		AssetLoader.Instance.ScrollList.SetActiveRecursively(false);
		GameFlowManager.Instance.LoadScene("!Loader_MainMenu", false);
	}

	private void UpdateTabStrip()
	{
		int currentWorld = (int)GameManager.Instance.CurrentWorld;
		if (currentWorld != mPreviousTab)
		{
			Init(base.gameObject);
			worldTab.UpdateTab(currentWorld);
		}
		mPreviousTab = currentWorld;
	}

	private void StartSelectedLevel()
	{
		GameManager.Instance.StartLevel((GameManager.Level)(smSelectedLevel - 1));
		GameFlowManager.Instance.LoadScene("Gameplay", true);
	}

	private bool IsSelectedLevel(GameManager.Level aLevel)
	{
		return smSelectedLevel - 1 == (int)aLevel;
	}

	private void MoviePlayCompleted(bool aSuccess)
	{
		StartSelectedLevel();
	}

	private void ChangeScrollList()
	{
		UIScrollList component = AssetLoader.Instance.ScrollList.GetComponent<UIScrollList>();
		if (mPreviousTab == 2)
		{
			mPrevItemSelected = Mathf.FloorToInt(component.ScrollPosition * (float)component.Count);
		}
		if (GameManager.Instance.CurrentWorld == GameManager.World.eWorld_BonusWorld)
		{
			component.touchScroll = true;
			component.ScrollToItem(mPrevItemSelected, 0f);
			PageIndicator.SetActiveRecursively(true);
		}
		else
		{
			component.touchScroll = false;
			component.ScrollToItem(0, 0f);
			PageIndicator.SetActiveRecursively(false);
		}
		LevelButtonController[] array = mButtonList;
		foreach (LevelButtonController levelButtonController in array)
		{
			levelButtonController.ChangeList();
		}
	}
}
