using UnityEngine;

public class PauseMenu : BaseGUI
{
	public enum Button
	{
		eQuit = 0,
		eRestart = 1,
		eMute = 2,
		eUnmute = 3,
		eResume = 4,
		eTurboSpeedPlus = 5,
		eTurboSpeedMinus = 6,
		eButton_COUNT = 7
	}

	public PauseMenu(GameObject aRefObj)
		: base(aRefObj)
	{
	}

	protected override void CreateLayouts()
	{
		if (ResolutionManager.Instance.LayoutSize == ResolutionManager.eLayoutSize.eLowres)
		{
			base.ButtonData = new GUIDefines.ButtonData[5]
			{
				new GUIDefines.ButtonData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.146875f,
						topRatio = 0.3578125f,
						widthRatio = 0.1708333f,
						heightRatio = 0.2921875f,
						IPad = new GUIDefines.RectIPadInfo
						{
							leftOffset = 16f,
							topOffset = -27f
						}
					},
					content = new GUIDefines.ContentInfo
					{
						textId = "TXT_PauseMenu"
					},
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customNormal = new GUIDefines.Texture2DInfo
						{
							name = "GUI/PauseMenu/pause_quit"
						},
						customActive = new GUIDefines.Texture2DInfo
						{
							name = "GUI/PauseMenu/pause_quit_pressed"
						},
						customFontSize = GUIDefines.FontSize.eSmall,
						customPadding = new GUIDefines.Vector2Info
						{
							xRatio = 0.01354167f,
							yRatio = 0.171625f,
							IPad = new GUIDefines.Vector2IPadInfo
							{
								yOffset = -18f
							}
						}
					},
					isAutoResizeOff = true
				},
				new GUIDefines.ButtonData
				{
					buttonId = 1,
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.31666672f,
						topRatio = 0.3578125f,
						widthRatio = 0.1645833f,
						heightRatio = 0.2921875f,
						IPad = new GUIDefines.RectIPadInfo
						{
							leftOffset = 5f,
							topOffset = -27f
						}
					},
					content = new GUIDefines.ContentInfo
					{
						textId = "TXT_PauseMenu4"
					},
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customNormal = new GUIDefines.Texture2DInfo
						{
							name = "GUI/PauseMenu/pause_restart"
						},
						customActive = new GUIDefines.Texture2DInfo
						{
							name = "GUI/PauseMenu/pause_restart_pressed"
						},
						customFontSize = GUIDefines.FontSize.eSmall,
						customPadding = new GUIDefines.Vector2Info
						{
							yRatio = 0.171625f,
							IPad = new GUIDefines.Vector2IPadInfo
							{
								yOffset = -18f
							}
						}
					},
					isAutoResizeOff = true
				},
				new GUIDefines.ButtonData
				{
					buttonId = 2,
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.4822913f,
						topRatio = 0.3578125f,
						widthRatio = 0.1625f,
						heightRatio = 0.2921875f,
						IPad = new GUIDefines.RectIPadInfo
						{
							leftOffset = -6f,
							topOffset = -27f
						}
					},
					content = new GUIDefines.ContentInfo
					{
						textId = "TXT_PauseMenu2"
					},
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customNormal = new GUIDefines.Texture2DInfo
						{
							name = "GUI/PauseMenu/pause_unmute"
						},
						customActive = new GUIDefines.Texture2DInfo
						{
							name = "GUI/PauseMenu/pause_unmute_pressed"
						},
						customFontSize = GUIDefines.FontSize.eSmall,
						customPadding = new GUIDefines.Vector2Info
						{
							yRatio = 0.171625f,
							IPad = new GUIDefines.Vector2IPadInfo
							{
								yOffset = -18f
							}
						}
					},
					invisible = AudioManager.Instance.Muted,
					isAutoResizeOff = true
				},
				new GUIDefines.ButtonData
				{
					buttonId = 3,
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.4822913f,
						topRatio = 0.3578125f,
						widthRatio = 0.1625f,
						heightRatio = 0.2921875f,
						IPad = new GUIDefines.RectIPadInfo
						{
							leftOffset = -6f,
							topOffset = -27f
						}
					},
					content = new GUIDefines.ContentInfo
					{
						textId = "TXT_PauseMenu2"
					},
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customNormal = new GUIDefines.Texture2DInfo
						{
							name = "GUI/PauseMenu/pause_mute"
						},
						customActive = new GUIDefines.Texture2DInfo
						{
							name = "GUI/PauseMenu/pause_mute_pressed"
						},
						customFontSize = GUIDefines.FontSize.eSmall,
						customPadding = new GUIDefines.Vector2Info
						{
							yRatio = 0.171625f,
							IPad = new GUIDefines.Vector2IPadInfo
							{
								yOffset = -18f
							}
						}
					},
					invisible = !AudioManager.Instance.Muted,
					isAutoResizeOff = true
				},
				new GUIDefines.ButtonData
				{
					buttonId = 4,
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 103f / 160f,
						topRatio = 0.3578125f,
						widthRatio = 0.190625f,
						heightRatio = 0.2921875f,
						IPad = new GUIDefines.RectIPadInfo
						{
							leftOffset = -16f,
							topOffset = -27f
						}
					},
					content = new GUIDefines.ContentInfo
					{
						textId = "TXT_Play"
					},
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customNormal = new GUIDefines.Texture2DInfo
						{
							name = "GUI/PauseMenu/pause_resume"
						},
						customActive = new GUIDefines.Texture2DInfo
						{
							name = "GUI/PauseMenu/pause_resume_pressed"
						},
						customFontSize = GUIDefines.FontSize.eSmall,
						customPadding = new GUIDefines.Vector2Info
						{
							xRatio = -0.01770833f,
							yRatio = 0.171625f,
							IPad = new GUIDefines.Vector2IPadInfo
							{
								yOffset = -18f
							}
						}
					},
					isAutoResizeOff = true
				}
			};
		}
		else
		{
			base.ButtonData = new GUIDefines.ButtonData[5]
			{
				new GUIDefines.ButtonData
				{
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.146875f,
						topRatio = 0.3578125f,
						widthRatio = 0.1708333f,
						heightRatio = 0.2921875f,
						IPad = new GUIDefines.RectIPadInfo
						{
							leftOffset = 16f,
							topOffset = -27f
						}
					},
					content = new GUIDefines.ContentInfo
					{
						textId = "TXT_PauseMenu"
					},
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customNormal = new GUIDefines.Texture2DInfo
						{
							name = "GUI/PauseMenu/pause_quit"
						},
						customActive = new GUIDefines.Texture2DInfo
						{
							name = "GUI/PauseMenu/pause_quit_pressed"
						},
						customFontSize = GUIDefines.FontSize.eSmall,
						customPadding = new GUIDefines.Vector2Info
						{
							xRatio = 0.01354167f,
							yRatio = 0.171625f,
							IPad = new GUIDefines.Vector2IPadInfo
							{
								yOffset = -18f
							}
						}
					},
					isAutoResizeOff = true
				},
				new GUIDefines.ButtonData
				{
					buttonId = 1,
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.3177084f,
						topRatio = 0.3578125f,
						widthRatio = 0.1645833f,
						heightRatio = 0.2921875f,
						IPad = new GUIDefines.RectIPadInfo
						{
							leftOffset = 5f,
							topOffset = -27f
						}
					},
					content = new GUIDefines.ContentInfo
					{
						textId = "TXT_PauseMenu4"
					},
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customNormal = new GUIDefines.Texture2DInfo
						{
							name = "GUI/PauseMenu/pause_restart"
						},
						customActive = new GUIDefines.Texture2DInfo
						{
							name = "GUI/PauseMenu/pause_restart_pressed"
						},
						customFontSize = GUIDefines.FontSize.eSmall,
						customPadding = new GUIDefines.Vector2Info
						{
							yRatio = 0.171625f,
							IPad = new GUIDefines.Vector2IPadInfo
							{
								yOffset = -18f
							}
						}
					},
					isAutoResizeOff = true
				},
				new GUIDefines.ButtonData
				{
					buttonId = 2,
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.4822913f,
						topRatio = 0.3578125f,
						widthRatio = 0.1625f,
						heightRatio = 0.2921875f,
						IPad = new GUIDefines.RectIPadInfo
						{
							leftOffset = -6f,
							topOffset = -27f
						}
					},
					content = new GUIDefines.ContentInfo
					{
						textId = "TXT_PauseMenu2"
					},
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customNormal = new GUIDefines.Texture2DInfo
						{
							name = "GUI/PauseMenu/pause_unmute"
						},
						customActive = new GUIDefines.Texture2DInfo
						{
							name = "GUI/PauseMenu/pause_unmute_pressed"
						},
						customFontSize = GUIDefines.FontSize.eSmall,
						customPadding = new GUIDefines.Vector2Info
						{
							yRatio = 0.171625f,
							IPad = new GUIDefines.Vector2IPadInfo
							{
								yOffset = -18f
							}
						}
					},
					invisible = AudioManager.Instance.Muted,
					isAutoResizeOff = true
				},
				new GUIDefines.ButtonData
				{
					buttonId = 3,
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.4822913f,
						topRatio = 0.3578125f,
						widthRatio = 0.1625f,
						heightRatio = 0.2921875f,
						IPad = new GUIDefines.RectIPadInfo
						{
							leftOffset = -6f,
							topOffset = -27f
						}
					},
					content = new GUIDefines.ContentInfo
					{
						textId = "TXT_PauseMenu2"
					},
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customNormal = new GUIDefines.Texture2DInfo
						{
							name = "GUI/PauseMenu/pause_mute"
						},
						customActive = new GUIDefines.Texture2DInfo
						{
							name = "GUI/PauseMenu/pause_mute_pressed"
						},
						customFontSize = GUIDefines.FontSize.eSmall,
						customPadding = new GUIDefines.Vector2Info
						{
							yRatio = 0.171625f,
							IPad = new GUIDefines.Vector2IPadInfo
							{
								yOffset = -18f
							}
						}
					},
					invisible = !AudioManager.Instance.Muted,
					isAutoResizeOff = true
				},
				new GUIDefines.ButtonData
				{
					buttonId = 4,
					pos = new GUIDefines.RectInfo
					{
						leftRatio = 0.6447917f,
						topRatio = 0.3578125f,
						widthRatio = 0.190625f,
						heightRatio = 0.2921875f,
						IPad = new GUIDefines.RectIPadInfo
						{
							leftOffset = -16f,
							topOffset = -27f
						}
					},
					content = new GUIDefines.ContentInfo
					{
						textId = "TXT_Play"
					},
					style = new GUIDefines.StyleInfo
					{
						useCustomStyle = true,
						customNormal = new GUIDefines.Texture2DInfo
						{
							name = "GUI/PauseMenu/pause_resume"
						},
						customActive = new GUIDefines.Texture2DInfo
						{
							name = "GUI/PauseMenu/pause_resume_pressed"
						},
						customFontSize = GUIDefines.FontSize.eSmall,
						customPadding = new GUIDefines.Vector2Info
						{
							xRatio = -0.01770833f,
							yRatio = 0.171625f,
							IPad = new GUIDefines.Vector2IPadInfo
							{
								yOffset = -18f
							}
						}
					},
					isAutoResizeOff = true
				}
			};
		}
		string textId = "TXT_BlueSkyNoCaps";
		if (GameManager.Instance.CurrentWorld == GameManager.World.eWorld_SodaSunset)
		{
			textId = "TXT_SodaSunsetNoCaps";
		}
		else if (GameManager.Instance.CurrentWorld == GameManager.World.eWorld_BonusWorld)
		{
			textId = "TXT_Bonus";
		}
		base.LabelData = new GUIDefines.LabelData[2]
		{
			new GUIDefines.LabelData
			{
				content = new GUIDefines.ContentInfo
				{
					textId = textId
				},
				pos = new GUIDefines.RectInfo
				{
					topRatio = 0.021875f,
					widthRatio = 1f,
					heightRatio = 0.1f
				},
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customFontSize = GUIDefines.FontSize.eMedium
				}
			},
			new GUIDefines.LabelData
			{
				content = new GUIDefines.ContentInfo
				{
					text = LocalizationManager.Instance.GetString("TXT_Level", (int)(GameManager.smCurrentLevel + 1))
				},
				pos = new GUIDefines.RectInfo
				{
					topRatio = 0.0984375f,
					widthRatio = 1f,
					heightRatio = 0.1f
				},
				style = new GUIDefines.StyleInfo
				{
					useCustomStyle = true,
					customFontSize = GUIDefines.FontSize.eMedium
				}
			}
		};
	}

	public override void Draw()
	{
		if (CanDraw())
		{
			GUIUtil.DrawSemiTransparentLayer();
			base.Draw();
			BlockControl(false);
		}
	}

	protected override void OnButtonSelect()
	{
		GameFlowManager.Instance.AudioManager.PlayUISFx(GameFlowManager.Instance.MenuClick24);
		switch ((Button)base.SelectedButton)
		{
		case Button.eQuit:
			GameManager.Instance.QuitLevel();
			GameFlowManager.Instance.GUIManager.ShowPauseMenu(false);
			GameFlowManager.Instance.LoadScene("LevelSelect", false);
			break;
		case Button.eRestart:
			StartOfGameDelay.Instance.RestartLevel();
			GameManager.Instance.QuitLevel();
			GameFlowManager.Instance.GUIManager.ShowPauseMenu(false);
			GameManager.Instance.StartLevel((GameManager.Level)(LevelSelect.SelectedLevel - 1));
			GameFlowManager.Instance.LoadSceneImmediate("Gameplay", true);
			break;
		case Button.eMute:
			AudioManager.Instance.Mute();
			base.ButtonData[2].invisible = true;
			base.ButtonData[3].invisible = false;
			break;
		case Button.eUnmute:
			AudioManager.Instance.Unmute();
			base.ButtonData[2].invisible = false;
			base.ButtonData[3].invisible = true;
			break;
		case Button.eResume:
			GameFlowManager.Instance.InputController.Reset();
			GameFlowManager.Instance.GUIManager.ShowPauseMenu(false);
			break;
		}
		ResetButton();
	}
}
