using UnityEngine;

public class LevelButtonPosition : MonoBehaviour
{
	public GameManager.World world;

	private GameObject m_LevelButton;

	private float posX = float.NaN;

	private void Awake()
	{
		m_LevelButton = Resources.Load("Prefabs/LevelButton") as GameObject;
		for (int i = 0; i < ScrollListManager.Instance.ListLevelCount; i++)
		{
			GameObject gameObject = Object.Instantiate(m_LevelButton) as GameObject;
			gameObject.transform.parent = base.transform;
		}
		float num = -2.2f;
		float num2 = 4.8f;
		float num3 = 1.5f;
		float num4 = (float)Screen.width / (float)Screen.height;
		float num5 = num2 * (num4 / num3);
		float num6 = num5 * 2.5f;
		int num7 = 0;
		Vector3 zero = Vector3.zero;
		float x = (float)Screen.width / 5f;
		Vector3 vector = Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));
		Vector3 vector2 = Camera.main.ScreenToWorldPoint(new Vector3(x, 0f, 0f));
		float magnitude = (vector - vector2).magnitude;
		foreach (Transform item in base.transform)
		{
			zero.x = 0f - num6 + num5 * (float)(num7 % 6);
			if (num7 < 6)
			{
				zero.y = num5 * 0.5f;
			}
			else
			{
				zero.y = (0f - num5) * 0.5f;
			}
			item.localPosition = zero;
			item.GetComponent<BHUIButton>().width = magnitude;
			item.GetComponent<BHUIButton>().height = magnitude;
			num7++;
			item.GetComponent<LevelButtonController>().buttonID = num7 + 12 * ScrollListManager.Instance.ListID;
		}
	}

	private void FixedUpdate()
	{
		if (float.IsNaN(posX))
		{
			posX = base.transform.position.x;
		}
		if (float.IsNaN(base.transform.position.x) || float.IsNaN(base.transform.position.y) || float.IsNaN(base.transform.position.z))
		{
			base.transform.position = new Vector3(posX, -2.2f, 0f);
		}
	}
}
