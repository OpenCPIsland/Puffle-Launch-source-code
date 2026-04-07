using UnityEngine;

public class PageIndicatorManager : MonoBehaviour
{
	protected SizeCategory.CategoryId m_AssetSizeCategoryId = SizeCategory.CategoryId.eUnknown;

	protected LocalizationManager.Language m_AssetLanguage;

	private UIScrollList m_scrollList;

	private BHUITexture[] m_pageIndicators;

	private int m_currentPage;

	private int m_prevPage;

	private bool isInitialize;

	private void Start()
	{
		if (GameObject.Find("ScrollList(Clone)") != null)
		{
			m_scrollList = GameObject.Find("ScrollList(Clone)").GetComponent<UIScrollList>();
		}
		GameObject gameObject = null;
		int num = Mathf.CeilToInt(2.9166667f);
		float num2 = (float)(num - 1) * -0.5f;
		for (int i = 0; i < num; i++)
		{
			gameObject = Object.Instantiate(Resources.Load("Prefabs/PageIndicator")) as GameObject;
			gameObject.transform.parent = base.transform;
			gameObject.transform.localPosition = new Vector3(num2 + (float)i * 1f, 0f, 0f);
		}
		m_pageIndicators = base.gameObject.GetComponentsInChildren<BHUITexture>();
	}

	private void Update()
	{
		if (!(m_scrollList == null) && m_scrollList.Count != 0)
		{
			if (!isInitialize)
			{
				isInitialize = true;
				int index = Mathf.Clamp(LevelSelect.Instance.mPrevItemSelected, 0, m_scrollList.Count - 1);
				m_scrollList.ScrollToItem(index, 0.001f);
				m_currentPage = Mathf.FloorToInt(m_scrollList.ScrollPosition * (float)m_scrollList.Count);
				UpdatePageIndicator();
			}
			m_currentPage = Mathf.Clamp(Mathf.FloorToInt(m_scrollList.ScrollPosition * (float)m_scrollList.Count), 0, m_scrollList.Count - 1);
			if (m_currentPage != m_prevPage)
			{
				m_prevPage = m_currentPage;
				UpdatePageIndicator();
			}
		}
	}

	private void UpdatePageIndicator()
	{
		string empty = string.Empty;
		for (int i = 0; i < m_pageIndicators.Length; i++)
		{
			ChangeMaterial(aMaterialName: (i != m_currentPage) ? "PageDot" : "PageDotActive", aGameObject: m_pageIndicators[i].gameObject);
		}
	}

	private void ChangeMaterial(GameObject aGameObject, string aMaterialName)
	{
		aGameObject.GetComponent<MeshRenderer>().material = Resources.Load("EZGUI/LevelSelect/" + aMaterialName, typeof(Material)) as Material;
		ResourceLoader.Instance.SetMaterialTexture(aGameObject, "EZGUI/LevelSelect/", false, out m_AssetSizeCategoryId, out m_AssetLanguage);
	}
}
