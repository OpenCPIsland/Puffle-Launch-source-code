using UnityEngine;

[RequireComponent(typeof(SpriteManager))]
public class CrabbyAnimController : MonoBehaviour
{
	public enum CrabbyAnim
	{
		eLeaving = 0,
		eLaugh = 1,
		eFreefall = 2,
		eHit = 3,
		eLose = 4,
		eIdle = 5
	}

	public bool reachingIdle;

	private SpriteManager mSpriteManager;

	private bool mIsAnimPlaying;

	private CrabbyAnim mAnimPlaying;

	private string[] mIdleAnimsStill;

	private string[] mIdleAnimsReaching;

	private string[] mAnimNames;

	public bool IsAnimPlaying
	{
		get
		{
			return mIsAnimPlaying;
		}
	}

	public CrabbyAnim CurrentAnim
	{
		get
		{
			return mAnimPlaying;
		}
	}

	public event CrabbyAnimEndEventHandler animationEnd;

	public void Start()
	{
		mSpriteManager = GetComponent<SpriteManager>();
		mSpriteManager.animationend += OnAnimationEnd;
		InitIdles();
		InitAnims();
	}

	public void FixedUpdate()
	{
		if (!mIsAnimPlaying && Random.Range(0, 10) == 0)
		{
			if (reachingIdle)
			{
				mSpriteManager.Play(mIdleAnimsReaching[Random.Range(0, mIdleAnimsReaching.Length)]);
			}
			else
			{
				mSpriteManager.Play(mIdleAnimsStill[Random.Range(0, mIdleAnimsStill.Length)]);
			}
			mIsAnimPlaying = true;
			mAnimPlaying = CrabbyAnim.eIdle;
		}
	}

	public void Play(CrabbyAnim aAnim)
	{
		mSpriteManager.Play(mAnimNames[(int)aAnim]);
		mAnimPlaying = aAnim;
		mIsAnimPlaying = true;
	}

	public void OnAnimationEnd(object sender, AnimationChangedEventArgs e)
	{
		if (this.animationEnd != null)
		{
			this.animationEnd(this, new CrabbyAnimEndEventArgs(mAnimPlaying));
		}
		mAnimPlaying = CrabbyAnim.eIdle;
		mIsAnimPlaying = false;
	}

	private void InitIdles()
	{
		if (reachingIdle)
		{
			mSpriteManager.defaultAnimation = 2;
		}
		else
		{
			mSpriteManager.defaultAnimation = 5;
		}
		mIdleAnimsStill = new string[2] { "Still_ArmL", "Still_ArmR" };
		mIdleAnimsReaching = new string[3] { "Reaching_ArmL", "Reaching_ArmR", "Blink" };
	}

	private void InitAnims()
	{
		mIsAnimPlaying = false;
		mAnimNames = new string[5] { "Leaving", "Laugh", "Freefall", "Hit", "Lose" };
	}
}
