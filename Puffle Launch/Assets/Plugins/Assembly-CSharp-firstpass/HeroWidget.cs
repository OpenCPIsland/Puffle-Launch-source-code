using AmazonCommon;
using UnityEngine;

public abstract class HeroWidget : AndroidJavaObjectWrapper
{
	public abstract class AbstractEmptyProperty : AndroidJavaObjectWrapper
	{
		public static readonly int CLICK_EVENT_MAX_DATA_LENGTH = 2048;

		protected AndroidJavaClass javaHelperClass;

		protected abstract string JAVA_HELPER_CLASS_NAME { get; }

		public AbstractEmptyProperty()
		{
			try
			{
				javaHelperClass = new AndroidJavaClass(JAVA_HELPER_CLASS_NAME);
			}
			catch
			{
				Debug.LogError("Could not obtain java " + JAVA_HELPER_CLASS_NAME + " class.");
			}
		}

		public void SetHandleClickEvent(bool handleClick, string data)
		{
			javaHelperClass.CallStatic("setHandleClickEventForEmptyProperty", javaObj, handleClick, data);
		}

		public void SetLabel(string label)
		{
			javaObj.Call("setLabel", label);
		}
	}

	public abstract class AbstractGroup : AndroidJavaObjectWrapper
	{
		public void SetGroupName(string groupName)
		{
			javaObj.Call("setGroupName", groupName);
		}
	}

	public abstract class AbstractEntry : AndroidJavaObjectWrapper
	{
		public static readonly int CLICK_LISTENER_MAX_DATA_LENGTH = 2048;

		protected AndroidJavaObject javaHelperObj;

		protected abstract string JAVA_HELPER_CLASS_NAME { get; }

		public AbstractEntry()
		{
			try
			{
				javaObj = new AndroidJavaObject(JAVA_CLASS_NAME, HomeManager.context);
			}
			catch
			{
				Debug.LogError("Could not obtain java " + JAVA_CLASS_NAME + " class.");
			}
			try
			{
				javaHelperObj = new AndroidJavaObject(JAVA_HELPER_CLASS_NAME, javaObj, HomeManager.context);
			}
			catch
			{
				Debug.LogError("Could not obtain java " + JAVA_HELPER_CLASS_NAME + " class.");
			}
		}
	}

	private static readonly AndroidJavaClass sClickBroadcastReceiverClass = new AndroidJavaClass("com.amazon.device.home.WidgetBroadcastReceiver");

	public static string readLastClickData()
	{
		return sClickBroadcastReceiverClass.CallStatic<string>("readLastClickData", new object[0]);
	}
}
