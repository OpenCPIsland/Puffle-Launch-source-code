using UnityEngine;

public class SizeCategory
{
	public enum CategoryId
	{
		eUnknown = -1,
		eSmall = 0,
		eMedium = 1,
		eLarge = 2,
		eXLarge = 3
	}

	private class ResolutionCategory
	{
		public float width;

		public float height;

		public string name = string.Empty;

		public float AspectRatio
		{
			get
			{
				return width / height;
			}
		}

		public ResolutionCategory(float resW, float resH, string fName)
		{
			width = resW;
			height = resH;
			name = fName;
		}
	}

	public const string kSmall = "small";

	public const string kMedium = "medium";

	public const string kLarge = "large";

	public const string kXlarge = "xlarge";

	private static SizeCategory m_cInstance;

	private ResolutionCategory[] m_Resolutions = new ResolutionCategory[5]
	{
		new ResolutionCategory(480f, 320f, "small"),
		new ResolutionCategory(800f, 480f, "medium"),
		new ResolutionCategory(854f, 480f, "medium"),
		new ResolutionCategory(960f, 540f, "large"),
		new ResolutionCategory(1280f, 800f, "xlarge")
	};

	private string m_CurrentCategory = string.Empty;

	private CategoryId m_CurrentCategoryId = CategoryId.eMedium;

	public static SizeCategory Instance
	{
		get
		{
			if (m_cInstance == null)
			{
				m_cInstance = new SizeCategory();
			}
			return m_cInstance;
		}
	}

	public string Category
	{
		get
		{
			return m_CurrentCategory;
		}
	}

	public CategoryId CurCategoryId
	{
		get
		{
			return m_CurrentCategoryId;
		}
	}

	public SizeCategory()
	{
		if (Utilities.AssertMsg(m_Resolutions.Length > 0, "You did not provide any Resolution Category!"))
		{
			m_CurrentCategory = GetCategory();
			m_CurrentCategoryId = GetCategoryId(m_CurrentCategory);
		}
	}

	private string GetCategory()
	{
		if (m_Resolutions.Length == 1)
		{
			return m_Resolutions[0].name;
		}
		float num = Screen.width;
		float num2 = Screen.height;
		ResolutionCategory[] resolutions = m_Resolutions;
		foreach (ResolutionCategory resolutionCategory in resolutions)
		{
			if (resolutionCategory != null && ((num == resolutionCategory.width && num2 == resolutionCategory.height) || (num2 == resolutionCategory.width && num == resolutionCategory.height)))
			{
				return resolutionCategory.name;
			}
		}
		string result = string.Empty;
		float num3 = float.PositiveInfinity;
		float num4 = float.PositiveInfinity;
		ResolutionCategory[] resolutions2 = m_Resolutions;
		foreach (ResolutionCategory resolutionCategory2 in resolutions2)
		{
			if (resolutionCategory2 != null)
			{
				float num5 = Mathf.Abs(resolutionCategory2.width - num);
				float num6 = Mathf.Abs(resolutionCategory2.height - num2);
				if (num5 < num3 || num6 < num4)
				{
					num3 = num5;
					num4 = num6;
					result = resolutionCategory2.name;
				}
			}
		}
		return result;
	}

	private CategoryId GetCategoryId(string category)
	{
		switch (category)
		{
		case "small":
			return CategoryId.eSmall;
		default:
			return CategoryId.eMedium;
		case "large":
			return CategoryId.eLarge;
		case "xlarge":
			return CategoryId.eXLarge;
		}
	}

	public CategoryId GetAlternateCategoryId(CategoryId categoryId)
	{
		switch (categoryId)
		{
		case CategoryId.eSmall:
			return CategoryId.eMedium;
		default:
			return CategoryId.eMedium;
		case CategoryId.eLarge:
			return CategoryId.eMedium;
		case CategoryId.eXLarge:
			return CategoryId.eLarge;
		}
	}

	public string GetCategory(CategoryId categoryId)
	{
		switch (categoryId)
		{
		case CategoryId.eSmall:
			return "small";
		default:
			return "medium";
		case CategoryId.eLarge:
			return "large";
		case CategoryId.eXLarge:
			return "xlarge";
		}
	}
}
