using UnityEngine;

public class ScrollListManager : MonoBehaviour
{
	public GameObject ListItem;

	private static ScrollListManager m_instance;

	private UIScrollList mUIScrollList;

	private int mCurrentListLevelCount;

	private int mCurrentListID;

	public static ScrollListManager Instance
	{
		get
		{
			return m_instance;
		}
	}

	public int ListID
	{
		get
		{
			return mCurrentListID;
		}
	}

	public int ListLevelCount
	{
		get
		{
			return mCurrentListLevelCount;
		}
	}

	private void Awake()
	{
		m_instance = this;
		mUIScrollList = GetComponent<UIScrollList>();
		int num = Mathf.CeilToInt(2.9166667f);
		int num2 = 36;
		for (int i = 0; i < num; i++)
		{
			mCurrentListID = i;
			if (num2 >= 12)
			{
				mCurrentListLevelCount = 12;
				num2 -= 12;
			}
			else
			{
				mCurrentListLevelCount = num2;
			}
			GameObject gameObject = Object.Instantiate(ListItem) as GameObject;
			gameObject.transform.parent = base.transform;
			mUIScrollList.sceneItems[i] = gameObject;
		}
	}
}
