using System;
using UnityEngine;

[RequireComponent(typeof(ElasticMovement))]
public class Balloon : MonoBehaviour
{
	public AudioClip reboundSound;

	public float pushForce = 50f;

	private Transform mTransform;

	private Vector3 mVelocity;

	private ElasticMovement mElasticMovement;

	public virtual void Start()
	{
		mTransform = base.transform;
		mElasticMovement = GetComponent<ElasticMovement>();
	}

	public void OnTriggerEnter(Collider aCollider)
	{
		if (aCollider.tag == "Player")
		{
			AudioManager.Instance.PlayObstacleSound(reboundSound);
			Puffle component = aCollider.GetComponent<Puffle>();
			Vector3 vector = mTransform.position - component.transform.position;
			float num = Mathf.Round(Mathf.Atan2(vector.y, vector.x) * 57.29578f);
			Vector3 vector2 = new Vector3(Mathf.Cos(num * ((float)Math.PI / 180f)), Mathf.Sin(num * ((float)Math.PI / 180f)), 0f);
			vector2 *= pushForce * ScaleItem.Instance.LevelScale;
			mElasticMovement.Velocity += vector2;
			ReboundPlayer(component, vector2);
			GetComponent<ElasticMovement>().UpdateTransform(1f);
		}
	}

	protected virtual void ReboundPlayer(Puffle aPuffle, Vector3 aPush)
	{
		if (aPuffle.Velocity.y <= 0f)
		{
			Vector3 velocity = new Vector3(aPuffle.Velocity.x * 0.5f, (Mathf.Abs(aPush.y) + Mathf.Abs(aPush.x)) * 0.5f, 0f);
			aPuffle.Velocity = velocity;
			aPuffle.AngularVelocity = (Mathf.Abs(aPush.x) + Mathf.Abs(aPush.y)) / ScaleItem.Instance.LevelScale;
		}
	}
}
