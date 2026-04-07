using UnityEngine;

public class Sparkle : MonoBehaviour
{
	private void Start()
	{
		GetComponent<SpriteManager>().animationend += FrameChangedEventHandler;
	}

	public void FrameChangedEventHandler(object sender, AnimationChangedEventArgs e)
	{
		if (e.anim.name == "PuffleOEffect")
		{
			Object.Destroy(base.gameObject);
		}
	}
}
