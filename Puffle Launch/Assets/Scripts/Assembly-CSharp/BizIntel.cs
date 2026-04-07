using System.Collections.Generic;
using UnityEngine;

public static class BizIntel
{
	public class ContextualEvent
	{
		private class KeyValue
		{
			public string m_Key;

			public string m_Value;

			public KeyValue(string aKey, string aValue)
			{
				m_Key = aKey;
				m_Value = aValue;
			}
		}

		private string m_Scope;

		private List<KeyValue> m_Context;

		public ContextualEvent(string aScope)
		{
			m_Scope = aScope;
			m_Context = new List<KeyValue>();
		}

		public void AddContextItem(string aKey, string aValue)
		{
			if (aValue == null)
			{
				aValue = " ";
			}
			m_Context.Add(new KeyValue(aKey, aValue));
		}

		public void AddContextItem(string aKey, int aValue)
		{
			m_Context.Add(new KeyValue(aKey, string.Empty + aValue));
		}

		public void AddContextItem(string aKey, bool aValue)
		{
			m_Context.Add(new KeyValue(aKey, string.Empty + aValue));
		}

		public void Log()
		{
			if (Application.isEditor || m_appMeasurement == null)
			{
				return;
			}
			m_appMeasurement.Call("clearVars");
			int num = 1;
			foreach (KeyValue item in m_Context)
			{
				string fieldName = string.Format("prop{0}", num);
				string val = string.Format("{0}={1}", item.m_Key, item.m_Value);
				m_appMeasurement.Set(fieldName, val);
				num++;
				if (num > 50)
				{
					break;
				}
			}
			string val2 = string.Format("Puffle Launch Android - {0}", m_Scope);
			m_appMeasurement.Set("pageName", val2);
			m_appMeasurement.Call<string>("track", new object[0]);
		}
	}

	private static AndroidJavaObject m_appMeasurement;

	private static bool IsAndroidRuntime()
	{
		return Application.platform == RuntimePlatform.Android && !Application.isEditor;
	}

	private static void LogSimpleEvent(string eventDescription)
	{
		if (!Application.isEditor)
		{
		}
	}

	public static void StartBizIntel()
	{
		if (!IsAndroidRuntime())
		{
			return;
		}
		try
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			if (androidJavaObject == null)
			{
				return;
			}
			AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getApplication", new object[0]);
			if (androidJavaObject2 != null)
			{
				m_appMeasurement = new AndroidJavaObject("com.omniture.AppMeasurement", androidJavaObject2);
				if (m_appMeasurement != null)
				{
					m_appMeasurement.Set("account", "wdgwdolcppuffleandroid");
					m_appMeasurement.Set("pageName", string.Empty);
					m_appMeasurement.Set("pageURL", string.Empty);
					m_appMeasurement.Set("currencyCode", "USD");
					m_appMeasurement.Set("trackingServer", "mdisney.112.2o7.net");
				}
			}
		}
		catch
		{
			m_appMeasurement = null;
		}
	}

	public static void StopBizIntel()
	{
		if (IsAndroidRuntime() && m_appMeasurement != null)
		{
			m_appMeasurement.Dispose();
			m_appMeasurement = null;
		}
	}
}
