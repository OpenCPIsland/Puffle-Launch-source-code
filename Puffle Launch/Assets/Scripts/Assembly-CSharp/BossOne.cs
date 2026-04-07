using UnityEngine;

public class BossOne : MonoBehaviour
{
	private ElasticMovement mElasticMovement;

	private BossController mBossController;

	private Vector3 mStartPosition;

	private int mMoveTimeout;

	public void Start()
	{
		mElasticMovement = GetComponent<ElasticMovement>();
		mBossController = GetComponent<BossController>();
		mStartPosition = base.transform.position;
		mMoveTimeout = 0;
	}

	public void FixedUpdate()
	{
		if (!mBossController.IsAlive)
		{
			return;
		}
		if (mMoveTimeout > 0)
		{
			mMoveTimeout--;
			return;
		}
		mMoveTimeout = 40;
		if (mElasticMovement.TargetPosition.x > mStartPosition.x)
		{
			mElasticMovement.TargetPosition = new Vector3(mStartPosition.x - 500f * ScaleItem.Instance.LevelScale, mStartPosition.y, 0f);
		}
		else
		{
			mElasticMovement.TargetPosition = new Vector3(mStartPosition.x + 500f * ScaleItem.Instance.LevelScale, mStartPosition.y, 0f);
		}
	}
}
