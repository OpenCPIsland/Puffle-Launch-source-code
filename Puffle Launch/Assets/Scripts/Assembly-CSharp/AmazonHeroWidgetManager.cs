using System.Collections.Generic;
using UnityEngine;

public class AmazonHeroWidgetManager : MonoBehaviour
{
	private static AmazonHeroWidgetManager _instance;

	private static bool CanUseHomeWidgets()
	{
		return Application.platform == RuntimePlatform.Android && !Application.isEditor;
	}

	public static void Init()
	{
		if (_instance == null)
		{
			GameObject gameObject = new GameObject("AmazonHeroWidgetManager");
			_instance = gameObject.AddComponent<AmazonHeroWidgetManager>();
			Object.DontDestroyOnLoad(gameObject);
		}
	}

	private void Start()
	{
		UpdateWidgets();
	}

	private void OnDestroy()
	{
		UpdateWidgets();
	}

	private void OnLevelWasLoaded(int l)
	{
		UpdateWidgets();
	}

	private void OnApplicationPause(bool p)
	{
		UpdateWidgets();
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.Escape))
		{
			UpdateWidgets();
		}
	}

	public static void UpdateWidgets()
	{
		if (!CanUseHomeWidgets())
		{
			return;
		}
		if (HomeManager.IsAvailable() && ProfileManager.Instance != null && ProfileManager.Instance.CurrentProfile != null)
		{
			Debug.Log("HomeManager is available - updating Hero Widget");
			HomeManager instance = HomeManager.GetInstance();
			if (instance == null)
			{
				return;
			}
			instance.UpdateNumericBadge(0);
			GroupedListHeroWidget groupedListHeroWidget = new GroupedListHeroWidget();
			GroupedListHeroWidget.Group obj = new GroupedListHeroWidget.Group();
			GroupedListHeroWidget.ListEntry listEntry = new GroupedListHeroWidget.ListEntry();
			int num = 0;
			while (ProfileManager.Instance.CurrentProfile.m_LevelData[num++].LevelComplete)
			{
			}
			List<GroupedListHeroWidget.ListEntry> list = new List<GroupedListHeroWidget.ListEntry>();
			listEntry.SetPrimaryText("You are on level " + num);
			listEntry.SetPrimaryIcon("puffle");
			listEntry.SetHandleClickEvent(false, null);
			list.Add(listEntry);
			listEntry = new GroupedListHeroWidget.ListEntry();
			listEntry.SetPrimaryText("You have " + ProfileManager.Instance.CurrentProfile.TotalCoins + " coins");
			listEntry.SetPrimaryIcon("coins");
			listEntry.SetHandleClickEvent(false, null);
			list.Add(listEntry);
			obj.SetListEntries(list);
			groupedListHeroWidget.AddGroup(0, obj);
			instance.UpdateWidget(groupedListHeroWidget);
		}
		else if (!HomeManager.IsAvailable())
		{
			Debug.Log("HomeManager is not available.");
		}
	}
}
