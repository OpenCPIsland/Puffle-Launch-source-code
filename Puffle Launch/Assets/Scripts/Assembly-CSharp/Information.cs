public class Information : BaseMonoScreen
{
	private enum Button
	{
		eBack = 0,
		eButton_COUNT = 1
	}

	protected override void CreateMainScreenLayouts()
	{
		base.MainScreen.TextureData = new GUIDefines.TextureData[1]
		{
			new GUIDefines.TextureData
			{
				pos = new GUIDefines.RectInfo
				{
					widthRatio = 1f,
					heightRatio = 0.120313f
				},
				icon = new GUIDefines.TextureInfo
				{
					name = "Create_Account_NavBar"
				}
			}
		};
		base.MainScreen.ButtonData = new GUIDefines.ButtonData[1]
		{
			new GUIDefines.ButtonData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.013542f,
					topRatio = 0.01719f,
					widthRatio = 0.15f,
					heightRatio = 0.082813f
				},
				style = new GUIDefines.StyleInfo
				{
					styleName = "CloseButton"
				},
				content = new GUIDefines.ContentInfo
				{
					textId = "TXT_EndGameScreen"
				}
			}
		};
	}

	private void Awake()
	{
		Init(base.gameObject);
	}

	private void OnGUI()
	{
		if (base.MainScreen.CanDraw())
		{
			base.MainScreen.Draw();
			BlockControl(false);
		}
	}

	protected override void OnMainScreenButtonSelect()
	{
		if (base.MainScreen.SelectedButton == 0)
		{
			base.MainScreen.StopGUI();
			GameFlowManager.Instance.LoadScene("!Loader_MainMenu", false);
		}
	}

	protected override void OnBack()
	{
		base.MainScreen.BlockControl(true);
		GameFlowManager.Instance.LoadScene("!Loader_MainMenu", false);
	}
}
