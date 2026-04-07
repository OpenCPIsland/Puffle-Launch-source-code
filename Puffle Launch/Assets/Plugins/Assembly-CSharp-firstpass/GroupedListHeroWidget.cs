using System;
using System.Collections.Generic;
using UnityEngine;

public class GroupedListHeroWidget : HeroWidget
{
	[Flags]
	public enum VisualStyle
	{
		DEFAULT = 0,
		PEEKABLE = 1,
		SHOPPING = 2,
		SIMPLE = 3
	}

	private enum PropertyType
	{
		PRIMARY = 0,
		SECONDARY = 1,
		TERTIARY = 2,
		QUATERNARY = 3
	}

	public class EmptyListProperty : AbstractEmptyProperty
	{
		protected override string JAVA_CLASS_NAME
		{
			get
			{
				return "com/amazon/device/home/GroupedListHeroWidget$EmptyListProperty";
			}
		}

		protected override string JAVA_HELPER_CLASS_NAME
		{
			get
			{
				return "com.amazon.device.home.ListEntryHelper";
			}
		}
	}

	public class Group : AbstractGroup
	{
		protected override string JAVA_CLASS_NAME
		{
			get
			{
				return "com/amazon/device/home/GroupedListHeroWidget$Group";
			}
		}

		public void SetListEntries(List<ListEntry> listEntries)
		{
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.LinkedList");
			foreach (ListEntry listEntry in listEntries)
			{
				androidJavaObject.Call<bool>("add", new object[1] { listEntry.ToAndroidJavaObject() });
			}
			javaObj.Call("setListEntries", androidJavaObject);
		}
	}

	public class ListEntry : AbstractEntry
	{
		public static readonly float MAX_STAR_RATING = 5f;

		public static readonly float MIN_STAR_RATING;

		protected override string JAVA_CLASS_NAME
		{
			get
			{
				return "com/amazon/device/home/GroupedListHeroWidget$ListEntry";
			}
		}

		protected override string JAVA_HELPER_CLASS_NAME
		{
			get
			{
				return "com.amazon.device.home.ListEntryHelper";
			}
		}

		public ListEntry SetHandleClickEvent(bool handleClick, string data)
		{
			javaHelperObj.Call("setHandleClickEvent", handleClick, data);
			return this;
		}

		public ListEntry SetHighlight(bool highlight)
		{
			javaObj.Call<AndroidJavaObject>("setHighlight", new object[1] { highlight });
			return this;
		}

		public ListEntry SetReviewCount(int reviewCount)
		{
			javaHelperObj.Call("setReviewCount", reviewCount);
			return this;
		}

		public ListEntry SetStarRating(float starRating)
		{
			javaHelperObj.Call("setStarRating", starRating);
			return this;
		}

		public ListEntry SetVisualStyle(VisualStyle visualStyle)
		{
			javaHelperObj.Call("setVisualStyle", visualStyle.ToString());
			return this;
		}

		public ListEntry SetPrimaryIcon(string resourceName)
		{
			javaHelperObj.Call("setIconResource", resourceName, 0);
			return this;
		}

		public ListEntry SetPrimaryIcon(Uri uri)
		{
			javaHelperObj.Call("setIconUri", uri.ToString(), 0);
			return this;
		}

		public ListEntry SetPrimaryText(string primaryText)
		{
			javaHelperObj.Call("setText", primaryText, 0);
			return this;
		}

		public ListEntry SetSecondaryIcon(string resourceName)
		{
			javaHelperObj.Call("setIconResource", resourceName, 1);
			return this;
		}

		public ListEntry SetSecondaryIcon(Uri uri)
		{
			javaHelperObj.Call("setIconUri", uri.ToString(), 1);
			return this;
		}

		public ListEntry SetSecondaryText(string secondaryText)
		{
			javaHelperObj.Call("setText", secondaryText, 1);
			return this;
		}

		public ListEntry SetTertiaryContentCanPeek(bool canPeek)
		{
			javaHelperObj.Call("setContentCanPeek", canPeek, 2);
			return this;
		}

		public ListEntry SetTertiaryIcon(string resourceName)
		{
			javaHelperObj.Call("setIconResource", resourceName, 2);
			return this;
		}

		public ListEntry SetTertiaryIcon(Uri uri)
		{
			javaHelperObj.Call("setIconUri", uri.ToString(), 2);
			return this;
		}

		public ListEntry SetTertiaryText(string tertiaryText)
		{
			javaHelperObj.Call("setText", tertiaryText, 2);
			return this;
		}

		public ListEntry SetQuaternaryContentCanPeek(bool canPeek)
		{
			javaHelperObj.Call("setContentCanPeek", canPeek, 3);
			return this;
		}

		public ListEntry SetQuaternaryIcon(string resourceName)
		{
			javaHelperObj.Call("setIconResource", resourceName, 3);
			return this;
		}

		public ListEntry SetQuaternaryIcon(Uri uri)
		{
			javaHelperObj.Call("setIconUri", uri.ToString(), 3);
			return this;
		}

		public ListEntry SetQuaternaryText(string quaternaryText)
		{
			javaHelperObj.Call("setText", quaternaryText, 3);
			return this;
		}
	}

	public static readonly int MAX_LIST_SIZE = 50;

	public static readonly int MAX_STRING_LENGTH = 256;

	protected override string JAVA_CLASS_NAME
	{
		get
		{
			return "com.amazon.device.home.GroupedListHeroWidget";
		}
	}

	public void AddGroup(int groupIndex, Group group)
	{
		javaObj.Call("addGroup", groupIndex, group.ToAndroidJavaObject());
	}

	public void AddListEntry(int groupIndex, int listIndex, ListEntry listEntry)
	{
		javaObj.Call("addListEntry", groupIndex, listIndex, listEntry.ToAndroidJavaObject());
	}

	public void RemoveGroup(int groupIndex)
	{
		javaObj.Call("removeGroup", groupIndex);
	}

	public void RemoveListEntry(int groupIndex, int listIndex)
	{
		javaObj.Call("removeListEntry", groupIndex, listIndex);
	}

	public void UpdateListEntry(int groupIndex, int listIndex, ListEntry listEntry)
	{
		javaObj.Call("updateListEntry", groupIndex, listIndex, listEntry.ToAndroidJavaObject());
	}

	public void SetEmptyListProperty(EmptyListProperty emptyListProperty)
	{
		javaObj.Call("setEmptyListProperty", emptyListProperty.ToAndroidJavaObject());
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
