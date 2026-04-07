using UnityEngine;

[RequireComponent(typeof(SpriteManager))]
public class Splash : MonoBehaviour
{
	private SpriteManager mSpriteManager;

	private MeshRenderer mRenderer;

	private Puffle mPuffle;

	private bool mAnimEndReached;

	private int mRespawnTimer = 6;

	public Puffle Puffle
	{
		get
		{
			return mPuffle;
		}
		set
		{
			mPuffle = value;
		}
	}

	public void Start()
	{
		mSpriteManager = GetComponent<SpriteManager>();
		mSpriteManager.animationend += OnAnimationEnd;
		mRenderer = GetComponent<MeshRenderer>();
		base.transform.localScale *= ScaleItem.Instance.BillboardScale;
	}

	public void FixedUpdate()
	{
		if (mAnimEndReached && --mRespawnTimer == 0)
		{
			mRenderer.enabled = false;
			mSpriteManager.enabled = false;
			mAnimEndReached = false;
			mRespawnTimer = 6;
			base.gameObject.active = false;
			mPuffle.Respawn();
		}
	}

	public void OnAnimationEnd(object sender, AnimationChangedEventArgs e)
	{
		mRenderer.enabled = false;
		mAnimEndReached = true;
	}

	public void Reset()
	{
		base.gameObject.active = true;
		if (!mRenderer)
		{
			mRenderer = GetComponent<MeshRenderer>();
		}
		mRenderer.enabled = true;
		if (!mSpriteManager)
		{
			mSpriteManager = GetComponent<SpriteManager>();
		}
		mSpriteManager.enabled = true;
		mSpriteManager.Reset();
		mSpriteManager.Play(0);
	}
}
