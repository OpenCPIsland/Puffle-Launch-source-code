using System;
using UnityEngine;

[RequireComponent(typeof(ElasticMovement))]
public class Asteroid : MonoBehaviour
{
	private Transform mTransform;

	private SpriteManager mSpriteManager;

	private ElasticMovement mElasticMovement;

	public void Start()
	{
		mTransform = base.transform;
		mElasticMovement = GetComponent<ElasticMovement>();
		mSpriteManager = GetComponent<SpriteManager>();
		mSpriteManager.Seek(UnityEngine.Random.Range(0, mSpriteManager.GetCurrAnimTotalFrames()));
	}

	public void OnTriggerEnter(Collider aOther)
	{
		if (aOther.tag == "Player")
		{
			TimeManager.Instance.StopSlowmo();
			Puffle component = aOther.GetComponent<Puffle>();
			Vector3 vector = component.transform.position - mTransform.position;
			float num = Mathf.Round(Mathf.Atan2(vector.y, vector.x) * 57.29578f);
			Vector3 vector2 = new Vector3(Mathf.Cos(num * ((float)Math.PI / 180f)), Mathf.Sin(num * ((float)Math.PI / 180f)), 0f);
			vector2 *= 50f * ScaleItem.Instance.LevelScale;
			component.Velocity = vector2 * 0.8f;
			mElasticMovement.Velocity = vector2 * -0.25f;
		}
	}
}
