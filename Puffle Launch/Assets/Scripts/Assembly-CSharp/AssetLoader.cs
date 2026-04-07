using UnityEngine;

public class AssetLoader : MonoBehaviour
{
	public Transform gpo;

	public Transform puffle;

	public GameObject EmptyList;

	public GameObject bonusScrollList;

	private static AssetLoader m_cInstance;

	private Transform m_PuffleTemplate;

	private Transform m_GiantPuffleOTemplate;

	private GameObject m_ScrollList;

	private GameObject m_EmptyScrollList;

	public static AssetLoader Instance
	{
		get
		{
			return m_cInstance;
		}
	}

	public Transform PuffleTemplate
	{
		get
		{
			return m_PuffleTemplate;
		}
		set
		{
			m_PuffleTemplate = value;
		}
	}

	public Transform GiantPuffleOTemplate
	{
		get
		{
			return m_GiantPuffleOTemplate;
		}
		set
		{
			m_GiantPuffleOTemplate = value;
		}
	}

	public GameObject EmptyScrollList
	{
		get
		{
			return m_EmptyScrollList;
		}
		set
		{
			m_EmptyScrollList = value;
		}
	}

	public GameObject ScrollList
	{
		get
		{
			return m_ScrollList;
		}
		set
		{
			m_ScrollList = value;
		}
	}

	private void Start()
	{
		m_cInstance = this;
		Object.DontDestroyOnLoad(this);
		LoadAssets();
	}

	private void LoadAssets()
	{
		LoadScrollLists();
		GiantPuffleOTemplate = (Transform)Object.Instantiate(gpo, new Vector3(-100f, 0f, 0f), default(Quaternion));
		GiantPuffleOTemplate.GetComponent<Renderer>().enabled = false;
		GiantPuffleOTemplate.gameObject.active = false;
		Object.DontDestroyOnLoad(GiantPuffleOTemplate);
		PuffleTemplate = (Transform)Object.Instantiate(puffle, new Vector3(-100f, 0f, 0f), default(Quaternion));
		PuffleTemplate.GetComponent<Renderer>().enabled = false;
		PuffleTemplate.gameObject.active = false;
		PuffleTemplate.GetComponent<Rigidbody>().Sleep();
		Object.DontDestroyOnLoad(PuffleTemplate);
	}

	private void LoadScrollLists()
	{
		Vector3 position = new Vector3(0f, -2.2f, 0f);
		EmptyScrollList = Object.Instantiate(EmptyList, position, default(Quaternion)) as GameObject;
		EmptyScrollList.SetActiveRecursively(false);
		Object.DontDestroyOnLoad(EmptyScrollList);
		ScrollList = Object.Instantiate(bonusScrollList, position, default(Quaternion)) as GameObject;
		ScrollList.SetActiveRecursively(false);
		Object.DontDestroyOnLoad(ScrollList);
		EmptyScrollList.SetActiveRecursively(true);
	}
}
