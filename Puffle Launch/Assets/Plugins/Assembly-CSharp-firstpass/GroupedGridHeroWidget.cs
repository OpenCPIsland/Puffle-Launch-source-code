using System;
using System.Collections.Generic;
using UnityEngine;

public class GroupedGridHeroWidget : HeroWidget
{
	public class EmtpyGridProperty : AbstractEmptyProperty
	{
		protected override string JAVA_CLASS_NAME
		{
			get
			{
				return "com/amazon/device/home/GroupedGridHeroWidget$EmptyGridProperty";
			}
		}

		protected override string JAVA_HELPER_CLASS_NAME
		{
			get
			{
				return "com.amazon.device.home.GridEntryHelper";
			}
		}
	}

	public class Group : AbstractGroup
	{
		protected override string JAVA_CLASS_NAME
		{
			get
			{
				return "com/amazon/device/home/GroupedGridHeroWidget$Group";
			}
		}

		public void SetGridEntries(List<GridEntry> gridEntries)
		{
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.LinkedList");
			foreach (GridEntry gridEntry in gridEntries)
			{
				androidJavaObject.Call<bool>("add", new object[1] { gridEntry.ToAndroidJavaObject() });
			}
			javaObj.Call("setGridEntries", androidJavaObject);
		}
	}

	public class GridEntry : AbstractEntry
	{
		protected override string JAVA_CLASS_NAME
		{
			get
			{
				return "com/amazon/device/home/GroupedGridHeroWidget$GridEntry";
			}
		}

		protected override string JAVA_HELPER_CLASS_NAME
		{
			get
			{
				return "com.amazon.device.home.GridEntryHelper";
			}
		}

		public GridEntry SetHandleClickEvent(bool handleClick, string data)
		{
			javaHelperObj.Call("setHandleClickEvent", handleClick, data);
			return this;
		}

		public GridEntry SetPlayInfo(bool playButton, string playDuration)
		{
			javaHelperObj.Call("setPlayInfo", playButton, playDuration);
			return this;
		}

		public GridEntry SetThumbnail(string resourceName)
		{
			javaHelperObj.Call("setThumbnailResource", resourceName);
			return this;
		}

		public GridEntry SetThumbnail(Uri uri)
		{
			javaHelperObj.Call("setThumbnailUri", uri.ToString());
			return this;
		}
	}

	public static readonly int MAX_GRID_SIZE = 50;

	public static readonly int MAX_STRING_LENGTH = 256;

	protected override string JAVA_CLASS_NAME
	{
		get
		{
			return "com.amazon.device.home.GroupedGridHeroWidget";
		}
	}

	public void SetEmptyGridProperty(EmtpyGridProperty emptyGridProperty)
	{
		javaObj.Call("setEmptyGridProperty", emptyGridProperty.ToAndroidJavaObject());
	}

	public void SetGroups(List<Group> groups)
	{
		AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.LinkedList");
		foreach (Group group in groups)
		{
			androidJavaObject.Call<bool>("add", new object[1] { group.ToAndroidJavaObject() });
		}
		javaObj.Call("setGroups", androidJavaObject);
	}
}
