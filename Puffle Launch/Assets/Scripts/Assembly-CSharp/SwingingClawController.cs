using UnityEngine;

public class SwingingClawController : MonoBehaviour
{
	public AudioClip retractClawSound;

	public AudioClip extendClawSound;

	private int mFrameCount;

	private int mFrameCountDelay;

	private AudioSource mAudioSource;

	private SpriteManager mSpriteManager;

	private void Start()
	{
		mAudioSource = GetComponent<AudioSource>();
		mSpriteManager = GetComponent<SpriteManager>();
		if (ResolutionManager.Instance.AssetResolution == ResolutionManager.eAssetResolution.eLowres)
		{
			base.transform.localPosition *= 2f;
		}
	}

	private void Update()
	{
		mAudioSource.mute = AudioManager.Instance.Muted;
	}

	private void FixedUpdate()
	{
		if (mFrameCountDelay == 0)
		{
			if (mSpriteManager.current.frame == 8)
			{
				mSpriteManager.Pause(true);
				mFrameCountDelay = 18;
			}
			else if (mSpriteManager.current.frame == 9)
			{
				mAudioSource.PlayOneShot(retractClawSound);
			}
			else if (mSpriteManager.current.frame == 18)
			{
				mSpriteManager.Pause(true);
				mFrameCountDelay = 18;
			}
			else if (mSpriteManager.current.frame == 19)
			{
				mAudioSource.PlayOneShot(extendClawSound);
			}
		}
		else
		{
			mFrameCountDelay--;
			if (mFrameCountDelay == 0)
			{
				mSpriteManager.Pause(false);
			}
		}
	}
}
