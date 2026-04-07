using UnityEngine;

[RequireComponent(typeof(Cannon))]
public class CannonMover : ObstacleMover
{
	public bool alwaysActive;

	private Cannon mCannon;

	private SphereCollider mCollider;

	public override void Start()
	{
		base.Start();
		mCannon = GetComponent<Cannon>();
		mCollider = (SphereCollider)base.GetComponent<Collider>();
	}

	protected override void UpdateTransform()
	{
		Vector3 position = mTransform.position;
		if (alwaysActive || mCannon.IsPuffleInside())
		{
			base.UpdateTransform();
		}
		else if (mTransform.position != mStartPosition)
		{
			if (Vector3.Dot(mVelocity, movementOffset) > 0f)
			{
				mVelocity = -movementOffset.normalized * velocity * ScaleItem.Instance.LevelScale;
			}
			base.UpdateTransform();
		}
		Vector3 vector = mTransform.position - position;
		mCollider.center = mTransform.rotation * vector;
	}
}
