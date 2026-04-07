using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
	public Transform spawnPoint;

	private float mSpawnTimer;

	private float mMinTimeToSpawn = 0.25f;

	private float mMaxTimeToSpawn = 1f;

	private float mChanceToSpawn = 0.25f;

	private float mDeltaTime;

	private float mLastFrameTimestamp;

	private void Start()
	{
		mLastFrameTimestamp = Time.realtimeSinceStartup;
	}

	private void FixedUpdate()
	{
		mDeltaTime = Time.realtimeSinceStartup - mLastFrameTimestamp;
		mLastFrameTimestamp = Time.realtimeSinceStartup;
		mSpawnTimer += mDeltaTime;
		if (mSpawnTimer > mMinTimeToSpawn && Random.Range(0f, 1f) < mChanceToSpawn)
		{
			SpawnRing();
		}
		if (mSpawnTimer > mMaxTimeToSpawn)
		{
			SpawnRing();
		}
	}

	public void SpawnRing()
	{
		GameObject gameObject = Object.Instantiate(Resources.Load("Prefabs/GUI/SpinningCoin", typeof(GameObject))) as GameObject;
		gameObject.transform.parent = spawnPoint;
		if (ResolutionManager.Instance.LayoutSize == ResolutionManager.eLayoutSize.eLowres)
		{
			gameObject.transform.localScale *= 0.5f;
		}
		mSpawnTimer = 0f;
	}
}
