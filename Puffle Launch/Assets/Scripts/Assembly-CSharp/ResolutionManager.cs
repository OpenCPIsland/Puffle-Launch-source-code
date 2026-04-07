using UnityEngine;

public class ResolutionManager
{
	public enum eAssetResolution
	{
		eLowres = 0,
		eOriginal = 1,
		eIPad = 2
	}

	public enum eLayoutSize
	{
		eLowres = 0,
		eOriginal = 1,
		eIPad = 2
	}

	private DeviceOrientation m_PrevOrientation = DeviceOrientation.LandscapeLeft;

	private eAssetResolution me_assetResolution;

	private eLayoutSize me_layoutSize;

	private bool mb_resolutionInfoSet;

	private float m_AspectRatio;

	private static ResolutionManager m_cInstance;

	public static ResolutionManager Instance
	{
		get
		{
			if (m_cInstance == null)
			{
				m_cInstance = new ResolutionManager();
				m_cInstance.Initialize();
			}
			return m_cInstance;
		}
	}

	public bool ResolutionInfoSet
	{
		get
		{
			return mb_resolutionInfoSet;
		}
	}

	public eAssetResolution AssetResolution
	{
		get
		{
			return me_assetResolution;
		}
	}

	public eLayoutSize LayoutSize
	{
		get
		{
			return me_layoutSize;
		}
	}

	public void Initialize()
	{
		mb_resolutionInfoSet = false;
		SetResolutionInfo();
	}

	public void CheckDeviceOrientation()
	{
		DeviceOrientation deviceOrientation = Input.deviceOrientation;
		if (deviceOrientation != m_PrevOrientation && (deviceOrientation == DeviceOrientation.LandscapeLeft || deviceOrientation == DeviceOrientation.LandscapeRight))
		{
			m_PrevOrientation = deviceOrientation;
			Screen.orientation = ScreenOrientation.LandscapeLeft;
		}
	}

	public float GetAspectRatio()
	{
		if (m_AspectRatio == 0f)
		{
			m_AspectRatio = GetMaxValueOfScreenSize() / GetMinValueOfScreenSize();
		}
		return m_AspectRatio;
	}

	public float GetMaxValueOfScreenSize()
	{
		return Mathf.Max(Screen.width, Screen.height);
	}

	public float GetMinValueOfScreenSize()
	{
		return Mathf.Min(Screen.width, Screen.height);
	}

	private void SetResolutionInfo()
	{
		me_assetResolution = eAssetResolution.eOriginal;
		me_layoutSize = eLayoutSize.eOriginal;
		float num = 500f;
		float num2 = 850f;
		if ((float)Screen.width <= num)
		{
			me_assetResolution = eAssetResolution.eLowres;
			me_layoutSize = eLayoutSize.eLowres;
			GUIConstants.kReferenceScreenWidth = 480f;
			GUIConstants.kReferenceScreenHeight = 320f;
		}
		else
		{
			me_assetResolution = eAssetResolution.eOriginal;
			me_layoutSize = eLayoutSize.eOriginal;
			GUIConstants.kReferenceScreenWidth = 960f;
			GUIConstants.kReferenceScreenHeight = 640f;
			if ((float)Screen.width >= num2)
			{
				float num3 = (float)Screen.width / (float)Screen.height;
				float num4 = Mathf.Abs(num3 - 1.3333334f);
				float num5 = Mathf.Abs(num3 - 1.5f);
				if (num4 < num5)
				{
					me_layoutSize = eLayoutSize.eIPad;
					me_assetResolution = eAssetResolution.eIPad;
					GUIConstants.kReferenceScreenWidth = 1024f;
					GUIConstants.kReferenceScreenHeight = 768f;
				}
			}
		}
		mb_resolutionInfoSet = true;
	}
}
