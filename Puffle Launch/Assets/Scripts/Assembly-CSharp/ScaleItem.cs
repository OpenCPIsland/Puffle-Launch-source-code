using UnityEngine;

public class ScaleItem
{
	private const float mkNativeHeight = 480f;

	private const float mkDPIFactor = 0.72f;

	private float mLevelScale = 1f;

	private float mPlayerRadius;

	private static ScaleItem m_cInstance;

	public static ScaleItem Instance
	{
		get
		{
			if (m_cInstance == null)
			{
				m_cInstance = new ScaleItem();
				m_cInstance.Initialize();
			}
			return m_cInstance;
		}
	}

	public float BillboardScale
	{
		get
		{
			return (float)Screen.height / 480f * 0.72f;
		}
	}

	public float LevelScale
	{
		get
		{
			return mLevelScale;
		}
	}

	public float PlayerRadius
	{
		get
		{
			return mPlayerRadius;
		}
		set
		{
			mPlayerRadius = value;
		}
	}

	private void Initialize()
	{
		float num = (float)Screen.height / 480f;
		mLevelScale = Mathf.Abs((Camera.main.ScreenToWorldPoint(new Vector2(1f, 0f)) - Camera.main.ScreenToWorldPoint(new Vector2(0f, 0f))).x * num);
	}

	public void ScaleLevelItem(Transform aItem, float aXScale, float aYScale, bool aIsPlayer)
	{
		float billboardScale = BillboardScale;
		aItem.localScale *= billboardScale;
		Vector3 localScale = aItem.localScale;
		localScale.x *= aXScale;
		localScale.y *= aYScale;
		aItem.localScale = localScale;
		Transform[] componentsInChildren = aItem.GetComponentsInChildren<Transform>();
		Transform[] array = componentsInChildren;
		foreach (Transform transform in array)
		{
			ScaleCollider(transform.GetComponent<Collider>(), aIsPlayer);
		}
	}

	private void ScaleCollider(Collider aCollider, bool aIsPlayer)
	{
		if (aCollider is SphereCollider)
		{
			ScaleCollider((SphereCollider)aCollider, aIsPlayer);
		}
		else if (aCollider is BoxCollider)
		{
			ScaleCollider((BoxCollider)aCollider, aIsPlayer);
		}
	}

	private void ScaleCollider(SphereCollider aCollider, bool aIsPlayer)
	{
		float billboardScale = BillboardScale;
		if (!aIsPlayer)
		{
			aCollider.radius -= mPlayerRadius;
		}
		aCollider.radius *= mLevelScale / billboardScale;
		aCollider.center *= mLevelScale / billboardScale;
	}

	private void ScaleCollider(BoxCollider aCollider, bool aIsPlayer)
	{
		float billboardScale = BillboardScale;
		if (!aIsPlayer)
		{
			Vector3 vector = new Vector3(mPlayerRadius, mPlayerRadius, mPlayerRadius);
			aCollider.size -= vector * 2f;
		}
		aCollider.size *= mLevelScale / billboardScale;
		aCollider.center *= mLevelScale / billboardScale;
	}
}
