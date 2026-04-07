using UnityEngine;

public class ElasticMovement : MonoBehaviour
{
	public float elasticMultiplier = 0.01f;

	public float friction = 0.1f;

	public bool restoreElasticity;

	private Transform mTransform;

	private Vector3 mVelocity;

	private Vector3 mTargetPosition;

	private float mElasticMultiplierDefault;

	private bool mTargetOverride;

	private bool mSleeping;

	public Vector3 Velocity
	{
		get
		{
			return mVelocity;
		}
		set
		{
			mVelocity = value;
			mSleeping = false;
		}
	}

	public Vector3 TargetPosition
	{
		get
		{
			return mTargetPosition;
		}
		set
		{
			mTargetPosition = value;
			mTargetOverride = true;
			mSleeping = false;
		}
	}

	public void Awake()
	{
		mTargetOverride = false;
	}

	public void Start()
	{
		mTransform = base.transform;
		if (!mTargetOverride)
		{
			mTargetPosition = mTransform.position;
		}
		mElasticMultiplierDefault = elasticMultiplier;
		mSleeping = false;
	}

	public void FixedUpdate()
	{
		if (!mSleeping)
		{
			UpdateTransform(TimeManager.Instance.DeltaTime);
		}
		if (!restoreElasticity)
		{
			return;
		}
		if (elasticMultiplier < mElasticMultiplierDefault)
		{
			float num = 100f - elasticMultiplier / mElasticMultiplierDefault * 100f;
			if (num > 10f)
			{
				elasticMultiplier *= 1.05f;
			}
		}
		else if (elasticMultiplier > mElasticMultiplierDefault)
		{
			elasticMultiplier = mElasticMultiplierDefault;
		}
	}

	public void UpdateTransform(float aDeltaTime)
	{
		if (LevelLoader.Instance != null)
		{
			float levelScale = ScaleItem.Instance.LevelScale;
			Vector3 vector = mTargetPosition - mTransform.position;
			if (Mathf.Abs(vector.x) <= levelScale)
			{
				vector.x = 0f;
			}
			if (Mathf.Abs(vector.y) <= levelScale)
			{
				vector.y = 0f;
			}
			mVelocity += vector * elasticMultiplier * aDeltaTime;
			mVelocity *= 1f - friction * aDeltaTime;
			mTransform.position += mVelocity * aDeltaTime;
			if (mVelocity.sqrMagnitude < 0.0001f && vector == Vector3.zero)
			{
				mVelocity = Vector3.zero;
				mSleeping = true;
			}
		}
	}
}
