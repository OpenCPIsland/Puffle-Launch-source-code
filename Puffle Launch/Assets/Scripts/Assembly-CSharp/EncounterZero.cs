using UnityEngine;

public class EncounterZero : MonoBehaviour
{
	private ElasticMovement mElasticMovement;

	private CrabbyAnimController mCrabbyAnimController;

	private BossController mBossController;

	private bool mIsLeaving;

	private Vector3 mStartPosition;

	private int mMoveTimeout;

	public void Start()
	{
		mElasticMovement = GetComponent<ElasticMovement>();
		mCrabbyAnimController = GetComponentInChildren<CrabbyAnimController>();
		mBossController = GetComponent<BossController>();
		mIsLeaving = false;
		mStartPosition = base.transform.position;
		mMoveTimeout = 0;
	}

	public void FixedUpdate()
	{
		if (!mBossController.IsAlive)
		{
			return;
		}
		if (mIsLeaving)
		{
			mElasticMovement.Velocity += new Vector3(-0.2f, 0.2f, 0f) * ScaleItem.Instance.LevelScale;
			if (!mCrabbyAnimController.IsAnimPlaying)
			{
				mCrabbyAnimController.Play(CrabbyAnimController.CrabbyAnim.eLeaving);
			}
		}
		else if (mMoveTimeout > 0)
		{
			mMoveTimeout--;
		}
		else
		{
			mMoveTimeout = 40;
			if (mElasticMovement.TargetPosition.x > mStartPosition.x)
			{
				mElasticMovement.TargetPosition = new Vector3(mStartPosition.x - 100f * ScaleItem.Instance.LevelScale, mStartPosition.y, 0f);
			}
			else
			{
				mElasticMovement.TargetPosition = new Vector3(mStartPosition.x + 100f * ScaleItem.Instance.LevelScale, mStartPosition.y, 0f);
			}
		}
	}

	public void OnTriggerEnter(Collider aOther)
	{
		if (aOther.tag == "Player")
		{
			mIsLeaving = true;
			mElasticMovement.elasticMultiplier = 0f;
		}
	}

	public void OnGiantPuffleOCollect()
	{
		mCrabbyAnimController.Play(CrabbyAnimController.CrabbyAnim.eLose);
	}
}
