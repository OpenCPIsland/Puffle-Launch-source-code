using UnityEngine;

[RequireComponent(typeof(Cannon))]
public class CannonRotator : MonoBehaviour
{
	public float rotationTime = 10f;

	public float rotationStep = 90f;

	public int stepAmount = 1;

	public bool pingPong = true;

	public bool alwaysActive;

	public bool doPause = true;

	private Transform mTransform;

	private Cannon mCannon;

	private float mStartAngle;

	private float mTime;

	private bool mInPause;

	private int mCurrentStep;

	private bool mForward;

	public void Start()
	{
		mTransform = base.transform;
		mCannon = GetComponent<Cannon>();
		mStartAngle = mTransform.eulerAngles.z;
		mTime = 0f;
		mInPause = doPause;
		mCurrentStep = 0;
		mForward = true;
	}

	public void FixedUpdate()
	{
		if (!alwaysActive && !mCannon.IsPuffleInside())
		{
			return;
		}
		mTime += TimeManager.Instance.DeltaTime;
		if (mTime > rotationTime)
		{
			mTime -= rotationTime;
			if (!mInPause)
			{
				if (mForward)
				{
					mCurrentStep++;
					if (mCurrentStep >= stepAmount)
					{
						if (pingPong)
						{
							mForward = false;
						}
						else
						{
							mCurrentStep = 0;
						}
					}
				}
				else
				{
					mCurrentStep--;
					if (mCurrentStep <= 0)
					{
						if (pingPong)
						{
							mForward = true;
						}
						else
						{
							mCurrentStep = stepAmount - 1;
						}
					}
				}
			}
			if (doPause)
			{
				mInPause = !mInPause;
			}
		}
		int num = mCurrentStep + (mForward ? 1 : (-1));
		float z = mStartAngle + Mathf.LerpAngle(rotationStep * (float)mCurrentStep, rotationStep * (float)num, (!mInPause) ? (mTime / rotationTime) : 0f);
		mTransform.eulerAngles = new Vector3(0f, 0f, z);
	}
}
