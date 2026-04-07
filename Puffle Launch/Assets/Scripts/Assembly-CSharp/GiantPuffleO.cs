using UnityEngine;

public class GiantPuffleO : MonoBehaviour
{
	public AudioClip mReachedSound;

	private SpriteManager mSpriteManager;

	public void Start()
	{
		mSpriteManager = GetComponent<SpriteManager>();
		mSpriteManager.Play("GiantPuffleO");
		mSpriteManager.animationend += FrameChangedEventHandler;
	}

	public void OnTriggerEnter(Collider other)
	{
		if (!(other.tag == "Player"))
		{
			return;
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("Boss");
		if ((bool)gameObject)
		{
			EncounterZero component = gameObject.GetComponent<EncounterZero>();
			if ((bool)component)
			{
				component.OnGiantPuffleOCollect();
			}
		}
		other.transform.parent = base.transform;
		other.gameObject.SetActiveRecursively(false);
		AudioManager.Instance.PlayObstacleSound(mReachedSound);
		if (LevelSelect.SelectedLevel - 1 == 23)
		{
			mSpriteManager.Play("GiantPuffleOBox");
		}
		else
		{
			mSpriteManager.Play("GiantPuffleOReach");
		}
	}

	public void FrameChangedEventHandler(object sender, AnimationChangedEventArgs e)
	{
		if (GameManager.Instance.EnableTiming && (e.anim.name == "GiantPuffleOReach" || e.anim.name == "GiantPuffleOBox"))
		{
			GameManager.Instance.EnableTiming = false;
			Puffle.Instance.transform.parent = null;
			GameManager.Instance.ShowEndLevelScreens();
		}
	}
}
