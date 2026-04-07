using UnityEngine;

public class RedBalloon : Balloon
{
	private const int mkLoopCount = 10;

	private SpriteManager mSpriteManager;

	private int mLoop;

	public override void Start()
	{
		base.Start();
		mSpriteManager = GetComponent<SpriteManager>();
		mSpriteManager.animationend += OnAnimationEnd;
	}

	protected override void ReboundPlayer(Puffle aPuffle, Vector3 aPush)
	{
		Vector3 velocity = new Vector3(aPush.x * -0.5f, aPush.y * -1.5f, 0f);
		aPuffle.Velocity = velocity;
		aPuffle.AngularVelocity = (Mathf.Abs(aPush.x) + Mathf.Abs(aPush.y)) / ScaleItem.Instance.LevelScale;
		mLoop = 10;
		mSpriteManager.animations[0].framerate = 24;
		mSpriteManager.Seek(1);
	}

	public void OnAnimationEnd(object sender, AnimationChangedEventArgs e)
	{
		if (--mLoop > 0)
		{
			mSpriteManager.Seek(1);
			return;
		}
		mSpriteManager.Seek(0);
		mSpriteManager.animations[0].framerate = 0;
	}
}
