using UnityEngine;

[RequireComponent(typeof(SpriteManager))]
public class PoofyCloud : MonoBehaviour
{
	public AudioClip impactSound;

	private SpriteManager mSpriteManager;

	public void Start()
	{
		mSpriteManager = GetComponent<SpriteManager>();
		mSpriteManager.animationend += OnAnimationEnd;
		Vector3 vector = Vector3.forward * 0.1f;
		base.transform.position -= vector;
		GetComponent<SphereCollider>().center += vector;
	}

	public void OnTriggerEnter(Collider aOther)
	{
		if (aOther.tag == "Player")
		{
			Object.Destroy(GetComponent<SharedSpriteManager>());
			mSpriteManager.sharedMaterial = false;
			mSpriteManager.enabled = true;
			if (impactSound != null)
			{
				AudioManager.Instance.PlayObstacleSound(impactSound);
			}
		}
	}

	public void OnAnimationEnd(object sender, AnimationChangedEventArgs e)
	{
		Object.Destroy(base.gameObject);
	}
}
