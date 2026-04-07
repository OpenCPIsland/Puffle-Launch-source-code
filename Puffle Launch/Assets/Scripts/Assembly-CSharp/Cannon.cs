using UnityEngine;

[RequireComponent(typeof(PuffleContainer))]
public class Cannon : MonoBehaviour
{
	public enum ControlType
	{
		eButtonPointTouch = 0,
		eButtonRotate = 1,
		eTouchRelease = 2,
		eControlType_COUNT = 3
	}

	public bool autoLaunch;

	public AudioClip mLaunchCannonSound;

	private Transform mTransform;

	private PuffleContainer mThisContainer;

	private TweeningController mTweening;

	public void Start()
	{
		mTransform = base.transform;
		mTweening = GetComponentInChildren<TweeningController>();
	}

	public void Awake()
	{
		mThisContainer = GetComponent<PuffleContainer>();
	}

	public void FixedUpdate()
	{
		if (autoLaunch && mThisContainer.IsPuffleInside())
		{
			LaunchPuffle();
		}
	}

	public virtual void LaunchPuffle()
	{
		mThisContainer.GetContainedPuffle().Launch(mTransform.right, 50f * ScaleItem.Instance.LevelScale);
		mThisContainer.ReleasePuffle();
		mTweening.Play(true);
		AudioManager.Instance.PlayCannonSound(mLaunchCannonSound);
	}

	public bool IsPuffleInside()
	{
		return mThisContainer.IsPuffleInside();
	}

	public virtual void OnCannonEnter()
	{
		mTweening.Reset(true);
	}
}
