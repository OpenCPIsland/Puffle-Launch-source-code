using UnityEngine;

public class ActivityIndicatorPopup : BasePopup
{
	public ActivityIndicatorPopup(GameObject aRefObj)
		: base(aRefObj)
	{
	}

	protected override void CreateLayouts()
	{
		base.WindowData = new GUIDefines.WindowData
		{
			pos = new GUIDefines.RectInfo
			{
				widthRatio = 1f,
				heightRatio = 1f
			},
			id = 10
		};
		base.TextureData = new GUIDefines.TextureData[2]
		{
			new GUIDefines.TextureData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.4307292f,
					topRatio = 0.3992187f,
					widthRatio = 0.1385417f,
					heightRatio = 0.2046875f
				},
				icon = new GUIDefines.TextureInfo
				{
					name = "GUI/Common/activity_indicator_bg"
				}
			},
			new GUIDefines.TextureData
			{
				pos = new GUIDefines.RectInfo
				{
					leftRatio = 0.4739584f,
					topRatio = 0.465625f,
					widthRatio = 9f / 160f,
					heightRatio = 0.084375f,
					IPad = new GUIDefines.RectIPadInfo
					{
						leftOffset = -5f,
						topOffset = -10f
					}
				},
				icon = new GUIDefines.TextureInfo
				{
					name = "GUI/Common/activity_indicator_arrow"
				},
				rotate = GUIDefines.RotateDirection.eCounterClockwise,
				pivotPointOffset = new GUIDefines.Vector2Info
				{
					xRatio = -0.001041667f,
					yRatio = -0.003125f,
					IPad = new GUIDefines.Vector2IPadInfo
					{
						yOffset = 1f
					}
				}
			}
		};
	}
}
