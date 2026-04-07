using UnityEngine;

public class GoToLastFrame : MonoBehaviour
{
	private SpriteManager mSpriteManager;

	private void Start()
	{
		mSpriteManager = GetComponent<SpriteManager>();
		mSpriteManager.Play("Fire");
		mSpriteManager.GoToLastFrame();
	}
}
