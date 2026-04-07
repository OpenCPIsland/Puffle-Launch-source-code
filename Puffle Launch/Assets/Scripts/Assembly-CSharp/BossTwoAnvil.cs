using System;
using System.Collections;
using UnityEngine;

public class BossTwoAnvil : MonoBehaviour
{
	private Transform mTransform;

	private BossTwo mParentBoss;

	private Vector3 mLastPosition;

	public AudioClip playerHitSFX;

	public Transform relativeObject;

	public void Start()
	{
		mTransform = base.transform;
		mParentBoss = mTransform.root.GetComponent<BossTwo>();
		if (relativeObject != null && relativeObject.GetComponent<Renderer>() != null)
		{
			StartCoroutine(PositionAnvilToRelativeObject());
		}
		else if (SizeCategory.Instance.Category == "small")
		{
			mTransform.localPosition = new Vector3(-0.6f, -21.28f, 0.01f);
		}
		else if (SizeCategory.Instance.Category == "large")
		{
			mTransform.localPosition = new Vector3(-0.6f, -8.87f, 0.01f);
		}
	}

	private IEnumerator PositionAnvilToRelativeObject()
	{
		while (relativeObject.GetComponent<Renderer>().bounds.size == Vector3.zero)
		{
			yield return null;
		}
		Vector3 position = mTransform.position;
		position.y = relativeObject.transform.position.y - relativeObject.GetComponent<Renderer>().bounds.size.y / 2f - base.GetComponent<Renderer>().bounds.size.y;
		mTransform.position = position;
	}

	public void FixedUpdate()
	{
		mLastPosition = mTransform.position;
	}

	public void OnTriggerEnter(Collider aOther)
	{
		if (aOther.tag == "Player")
		{
			OnAnvilHit(aOther.GetComponent<Puffle>(), mParentBoss.IsAttacking);
			return;
		}
		ElasticMovement component = aOther.GetComponent<ElasticMovement>();
		if ((bool)component)
		{
			OnAnvilHit(component, mParentBoss.IsAttacking);
		}
	}

	private void OnAnvilHit(Puffle aPlayer, bool aIsAttacking)
	{
		Vector3 pushVector = GetPushVector(aPlayer.transform.position, 25f);
		if (aIsAttacking)
		{
			pushVector -= GetPushVector(mLastPosition, 50f);
		}
		aPlayer.Velocity = pushVector;
		if ((bool)playerHitSFX)
		{
			AudioManager.Instance.PlayObstacleSound(playerHitSFX);
		}
	}

	private void OnAnvilHit(ElasticMovement aObstacle, bool aIsAttacking)
	{
		Vector3 pushVector = GetPushVector(aObstacle.transform.position, 50f);
		if (aIsAttacking)
		{
			pushVector -= GetPushVector(mLastPosition, 100f);
		}
		aObstacle.Velocity = pushVector;
		aObstacle.elasticMultiplier = 0.001f;
	}

	private Vector3 GetPushVector(Vector3 aTarget, float aForce)
	{
		Vector3 vector = aTarget - mTransform.position;
		float num = Mathf.Round(Mathf.Atan2(vector.y, vector.x) * 57.29578f);
		Vector3 vector2 = new Vector3(Mathf.Cos(num * ((float)Math.PI / 180f)), Mathf.Sin(num * ((float)Math.PI / 180f)), 0f);
		return vector2 * aForce * ScaleItem.Instance.LevelScale;
	}
}
