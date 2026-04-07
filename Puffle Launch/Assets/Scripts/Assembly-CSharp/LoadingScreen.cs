using UnityEngine;

public class LoadingScreen : BaseGUI
{
	private const float m_LoadingProgressBarWidth = 0.41979f;

	private bool m_UseLoadingBar;

	public LoadingScreen(GameObject aRefObj)
		: base(aRefObj)
	{
	}

	protected override void CreateLayouts()
	{
		base.TextureData = new GUIDefines.TextureData[4]
		{
			new GUIDefines.TextureData
			{
				pos = new GUIDefines.RectInfo
				{
					widthRatio = 1f,
					heightRatio = 1f
				},
				icon = new GUIDefines.TextureInfo
				{
					name = "GUI/LoadingScreen/BlackScreen"
				},
				bgInfo = new GUIDefines.BackgroundInfo()
			},
			new GUIDefines.TextureData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.55417f,
					topRatio = 0.90844f,
					widthRatio = 0.41979f,
					heightRatio = 0.04306f,
					IPad = new GUIDefines.RectIPadInfo
					{
						leftOffset = 20f,
						topOffset = -6f,
						widthScale = -10f,
						heightScale = 5f
					}
				},
				icon = new GUIDefines.TextureInfo
				{
					name = "GUI/LoadingScreen/AnimatedLoadingScreen/bar_frame_bg"
				},
				bgInfo = new GUIDefines.BackgroundInfo(),
				invisible = true
			},
			new GUIDefines.TextureData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.55417f,
					topRatio = 0.90844f,
					heightRatio = 0.04306f,
					IPad = new GUIDefines.RectIPadInfo
					{
						leftOffset = 20f,
						topOffset = -6f,
						widthScale = -10f,
						heightScale = 5f
					}
				},
				icon = new GUIDefines.TextureInfo
				{
					name = "GUI/LoadingScreen/AnimatedLoadingScreen/bar_filler_slice"
				},
				bgInfo = new GUIDefines.BackgroundInfo(),
				invisible = true
			},
			new GUIDefines.TextureData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.53646f,
					topRatio = 0.87813f,
					widthRatio = 73f / 160f,
					heightRatio = 0.10469f,
					IPad = new GUIDefines.RectIPadInfo
					{
						leftOffset = 20f,
						widthScale = -10f
					}
				},
				icon = new GUIDefines.TextureInfo
				{
					name = "GUI/LoadingScreen/AnimatedLoadingScreen/bar_frame"
				},
				bgInfo = new GUIDefines.BackgroundInfo(),
				invisible = true
			}
		};
	}

	private void SetPosition(Vector2 av2_loadingAnimPosition)
	{
	}

	protected override void OnButtonSelect()
	{
	}

	public void Update()
	{
		if (m_UseLoadingBar && LevelLoader.Instance != null && base.TextureData != null)
		{
			base.TextureData[2].pos.widthRatio = LevelLoader.Instance.loadingProgress * 0.41979f;
			base.TextureData[2].Init();
		}
	}

	public override void Draw()
	{
		base.Draw();
	}

	public void Start()
	{
	}

	public void Stop()
	{
	}

	public void StartLoadingBar()
	{
		m_UseLoadingBar = true;
		GameFlowManager.Instance.GUIManager.LoadingScreen.TextureData[1].invisible = false;
		GameFlowManager.Instance.GUIManager.LoadingScreen.TextureData[2].invisible = false;
		GameFlowManager.Instance.GUIManager.LoadingScreen.TextureData[3].invisible = false;
		base.TextureData[2].pos.widthRatio = 0f;
		base.TextureData[2].Init();
	}

	public void StopLoadingBar()
	{
		m_UseLoadingBar = false;
		base.TextureData[2].pos.widthRatio = 0f;
		base.TextureData[2].Init();
		GameFlowManager.Instance.GUIManager.LoadingScreen.TextureData[1].invisible = true;
		GameFlowManager.Instance.GUIManager.LoadingScreen.TextureData[2].invisible = true;
		GameFlowManager.Instance.GUIManager.LoadingScreen.TextureData[3].invisible = true;
	}
}
