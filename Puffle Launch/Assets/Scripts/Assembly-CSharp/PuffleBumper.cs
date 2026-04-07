using System;
using UnityEngine;

public class PuffleBumper : MonoBehaviour
{
	public AudioClip impactSound;

	public float bounceStrength = 1f;

	public Animation hitAnimation;

	private Transform mTransform;

	public void Start()
	{
		mTransform = base.transform;
	}

	public void OnTriggerEnter(Collider aOther)
	{
		if (!(aOther.tag == "Player"))
		{
			return;
		}
		Puffle component = aOther.GetComponent<Puffle>();
		Vector3 vector = component.transform.position - mTransform.position;
		float num = Mathf.Round(Mathf.Atan2(vector.y, vector.x) * 57.29578f);
		Vector3 vector2 = new Vector3(Mathf.Cos(num * ((float)Math.PI / 180f)), Mathf.Sin(num * ((float)Math.PI / 180f)), 0f);
		vector2 *= bounceStrength * ScaleItem.Instance.LevelScale;
		component.Velocity = vector2 * 0.8f;
		if (impactSound != null)
		{
			AudioManager.Instance.PlayObstacleSound(impactSound);
		}
		if (hitAnimation != null)
		{
			if (vector.x < 0f)
			{
				hitAnimation.Play("ObstacleHitLeft");
			}
			else
			{
				hitAnimation.Play("ObstacleHitRight");
			}
		}
	}
}
