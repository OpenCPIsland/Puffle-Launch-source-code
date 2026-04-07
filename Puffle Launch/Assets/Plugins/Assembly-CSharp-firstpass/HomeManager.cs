using UnityEngine;

public class HomeManager
{
	public static AndroidJavaObject context;

	private static readonly string FEATURE_HOME = "com.amazon.software.home";

	private static HomeManager homeManager;

	private static readonly string JAVA_CLASS_NAME = "com.amazon.device.home.HomeManager";

	private AndroidJavaObject javaObj;

	private static bool CanUseHomeManager()
	{
		return Application.platform == RuntimePlatform.Android && !Application.isEditor;
	}

	private static bool EnsureContext()
	{
		if (!CanUseHomeManager())
		{
			return false;
		}
		if (context != null)
		{
			return true;
		}
		try
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			context = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
		}
		catch
		{
			context = null;
		}
		return context != null;
	}

	private HomeManager()
	{
		if (!EnsureContext())
		{
			return;
		}
		try
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass(JAVA_CLASS_NAME);
			javaObj = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[1] { context });
		}
		catch
		{
			Debug.LogError("Could not obtain java " + JAVA_CLASS_NAME + " class.");
		}
	}

	public static HomeManager GetInstance()
	{
		if (homeManager == null)
		{
			homeManager = new HomeManager();
		}
		if (homeManager != null && homeManager.javaObj == null)
		{
			return null;
		}
		return homeManager;
	}

	public static bool IsAvailable()
	{
		if (!EnsureContext())
		{
			return false;
		}
		try
		{
			AndroidJavaObject androidJavaObject = context.Call<AndroidJavaObject>("getPackageManager", new object[0]);
			return androidJavaObject.Call<bool>("hasSystemFeature", new object[1] { FEATURE_HOME });
		}
		catch
		{
			return false;
		}
	}

	public void UpdateNumericBadge(int numericBadgeValue)
	{
		if (javaObj != null)
		{
			javaObj.Call("updateNumericBadge", numericBadgeValue);
		}
	}

	public void RemoveWidget()
	{
		if (javaObj != null)
		{
			javaObj.Call("removeWidget");
		}
	}

	public void UpdateWidget(HeroWidget widget)
	{
		if (javaObj != null && widget != null)
		{
			javaObj.Call("updateWidget", widget.ToAndroidJavaObject());
		}
	}
}
