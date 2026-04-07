using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
	public Vector3 movementOffset;

	public float velocity;

	public float friction;

	public float waitFrames;

	public bool accelerate;

	protected Vector3 mVelocity;

	protected Vector3 mAcceleration;

	protected Transform mTransform;

	protected Vector3 mStartPosition;

	protected float mWaitTime;

	public virtual void Start()
	{
		mTransform = base.transform;
		mStartPosition = mTransform.position;
		mWaitTime = 0f;
		Vector3 vector = movementOffset.normalized * velocity * ScaleItem.Instance.LevelScale;
		if (accelerate)
		{
			mAcceleration = vector;
		}
		else
		{
			mVelocity = vector;
		}
	}

	public void FixedUpdate()
	{
		UpdateTransform();
	}

	protected virtual void UpdateTransform()
	{
		if (mWaitTime == 0f)
		{
			Vector3 vector = mVelocity;
			if (accelerate)
			{
				vector = mAcceleration;
			}
			Vector3 vector2 = mStartPosition;
			if (Vector3.Dot(vector, movementOffset) > 0f)
			{
				vector2 += movementOffset * ScaleItem.Instance.LevelScale;
			}
			mVelocity += mAcceleration * TimeManager.Instance.DeltaTime;
			Vector3 vector3 = mTransform.position + mVelocity * TimeManager.Instance.DeltaTime;
			Vector3 lhs = vector2 - vector3;
			if (Vector3.Dot(lhs, vector) < -0.001f)
			{
				mWaitTime = waitFrames;
				if (!accelerate)
				{
					mVelocity = Vector3.zero;
					mTransform.position = vector2;
				}
				else
				{
					mAcceleration = -mAcceleration;
				}
			}
		}
		else
		{
			mWaitTime = Mathf.Max(mWaitTime - TimeManager.Instance.DeltaTime, 0f);
		}
		mVelocity *= 1f - friction * TimeManager.Instance.DeltaTime;
		mTransform.position += mVelocity * TimeManager.Instance.DeltaTime;
		if (!accelerate && mVelocity == Vector3.zero && mWaitTime == 0f)
		{
			mVelocity = -movementOffset.normalized * velocity * ScaleItem.Instance.LevelScale;
			if (mTransform.position == mStartPosition)
			{
				mVelocity *= -1f;
			}
		}
	}
}
