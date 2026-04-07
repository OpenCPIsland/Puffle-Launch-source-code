using System;
using UnityEngine;

public class Fan : MonoBehaviour
{
	private Transform mTransform;

	public void Start()
	{
		mTransform = base.transform;
	}

	public void OnTriggerEnter(Collider aOther)
	{
		if (aOther.tag == "Player")
		{
			PushPlayer(aOther.GetComponent<Puffle>());
		}
	}

	public void OnTriggerStay(Collider aOther)
	{
		if (aOther.tag == "Player")
		{
			PushPlayer(aOther.GetComponent<Puffle>());
		}
	}

	private void PushPlayer(Puffle aPlayer)
	{
		if (aPlayer.State == Puffle.PuffleState.eFlying)
		{
			float num = mTransform.eulerAngles.z + 90f;
			Vector3 vector = new Vector3(Mathf.Cos(num * ((float)Math.PI / 180f)), Mathf.Sin(num * ((float)Math.PI / 180f)), 0f);
			vector *= 1.8f * ScaleItem.Instance.LevelScale;
			Vector3 velocity = aPlayer.Velocity;
			velocity.x += vector.x;
			if (velocity.y < 0f)
			{
				velocity.y += vector.y - velocity.y / 3f;
			}
			else
			{
				velocity.y += vector.y;
			}
			aPlayer.Velocity = velocity;
			aPlayer.AngularVelocity += vector.y / ScaleItem.Instance.LevelScale;
		}
	}
}
