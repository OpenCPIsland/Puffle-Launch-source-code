using System;
using UnityEngine;

public class TweeningController : MonoBehaviour
{
	public bool PlayOnStart = true;

	public bool Looping = true;

	public bool keyframeOffsetInPixels;

	public bool keyframeOffsetInWorldSpace = true;

	public Keyframe[] keyframes;

	public bool EaseInEaseOut;

	public bool AffectX = true;

	public bool AffectY = true;

	public bool AffectZ = true;

	public bool AffectOrientation = true;

	private Transform mTransform;

	private Vector3 mPreviousOffset;

	private Vector3 mOriginalScale;

	private Vector3 mOriginalPosition;

	private int mCurrentKeyframe;

	private float mFrame;

	private bool mForward;

	private bool mPlay;

	private Vector3 mOffsetVector = default(Vector3);

	private Vector3 mWorkingVector = default(Vector3);

	public Keyframe[] Keyframes
	{
		get
		{
			return keyframes;
		}
		set
		{
			if (keyframeOffsetInPixels)
			{
				for (int i = 0; i < value.Length; i++)
				{
					value[i].offset.x = value[i].offset.x * ScaleItem.Instance.LevelScale;
					value[i].offset.y = value[i].offset.y * ScaleItem.Instance.LevelScale;
					value[i].offset.z = value[i].offset.z * ScaleItem.Instance.LevelScale;
				}
			}
			keyframes = value;
		}
	}

	public event TweeningEndEventHandler ForwardTweenEnd;

	public event TweeningEndEventHandler ReverseTweenEnd;

	public void Start()
	{
		mPlay = PlayOnStart;
		mForward = true;
		mTransform = base.transform;
		mFrame = 0f;
		mOriginalScale = mTransform.localScale;
		mOriginalPosition = mTransform.localPosition;
		if (keyframeOffsetInPixels)
		{
			for (int i = 0; i < keyframes.Length; i++)
			{
				keyframes[i].offset.x = keyframes[i].offset.x * ScaleItem.Instance.LevelScale;
				keyframes[i].offset.y = keyframes[i].offset.y * ScaleItem.Instance.LevelScale;
				keyframes[i].offset.z = keyframes[i].offset.z * ScaleItem.Instance.LevelScale;
			}
		}
		if (keyframes.Length > 0)
		{
			if (AffectOrientation)
			{
				mTransform.eulerAngles = new Vector3(0f, 0f, keyframes[0].angle);
			}
			mTransform.localScale = Vector3.Scale(mOriginalScale, keyframes[0].scale);
		}
		if (keyframes.Length < 2)
		{
			mTransform.position += keyframes[0].offset;
			base.enabled = false;
		}
	}

	public void FixedUpdate()
	{
		if (!mPlay)
		{
			return;
		}
		float t;
		if (mForward)
		{
			mFrame += TimeManager.Instance.DeltaTime;
			if (mFrame > (float)keyframes[mCurrentKeyframe + 1].frame)
			{
				mCurrentKeyframe++;
				if (mCurrentKeyframe == keyframes.Length - 1)
				{
					if (Looping)
					{
						mFrame = 0f;
						mCurrentKeyframe = 0;
					}
					else
					{
						mPlay = false;
						if (this.ForwardTweenEnd != null)
						{
							this.ForwardTweenEnd(this, EventArgs.Empty);
						}
						mCurrentKeyframe--;
						mFrame = keyframes[mCurrentKeyframe + 1].frame;
					}
				}
			}
			t = (mFrame - (float)keyframes[mCurrentKeyframe].frame) / (float)(keyframes[mCurrentKeyframe + 1].frame - keyframes[mCurrentKeyframe].frame);
			if (EaseInEaseOut)
			{
				int num = keyframes[mCurrentKeyframe + 1].frame - keyframes[mCurrentKeyframe].frame;
				EaseInEaseOut3D(ref mOffsetVector, num - (keyframes[mCurrentKeyframe + 1].frame - (int)mFrame), keyframes[mCurrentKeyframe].offset, keyframes[mCurrentKeyframe + 1].offset - keyframes[mCurrentKeyframe].offset, num);
			}
			else
			{
				mOffsetVector = Vector3.Lerp(keyframes[mCurrentKeyframe].offset, keyframes[mCurrentKeyframe + 1].offset, t);
			}
			float angle = Mathf.Lerp(keyframes[mCurrentKeyframe].angle, keyframes[mCurrentKeyframe + 1].angle, t);
			ApplyFiltering(ref mOffsetVector, ref angle);
			mTransform.position -= mPreviousOffset;
			mOffsetVector = Quaternion.Euler(0f, 0f, angle) * mOffsetVector;
			mTransform.position += mOffsetVector;
			mPreviousOffset = mOffsetVector;
			mWorkingVector.x = 0f;
			mWorkingVector.y = 0f;
			mWorkingVector.z = angle;
			mTransform.eulerAngles = mWorkingVector;
			mTransform.localScale = Vector3.Scale(mOriginalScale, Vector3.Lerp(keyframes[mCurrentKeyframe].scale, keyframes[mCurrentKeyframe + 1].scale, t));
			return;
		}
		mFrame -= TimeManager.Instance.DeltaTime;
		if (mFrame < (float)keyframes[mCurrentKeyframe - 1].frame)
		{
			mCurrentKeyframe--;
			if (mCurrentKeyframe == 0)
			{
				if (Looping)
				{
					Reset(false);
					mPlay = true;
				}
				else
				{
					mPlay = false;
					if (this.ReverseTweenEnd != null)
					{
						this.ReverseTweenEnd(this, EventArgs.Empty);
					}
					mCurrentKeyframe++;
					mFrame = keyframes[0].frame;
				}
			}
		}
		t = ((float)keyframes[mCurrentKeyframe].frame - mFrame) / (float)(keyframes[mCurrentKeyframe].frame - keyframes[mCurrentKeyframe - 1].frame);
		if (EaseInEaseOut)
		{
			int aDuration = keyframes[mCurrentKeyframe].frame - keyframes[mCurrentKeyframe - 1].frame;
			EaseInEaseOut3D(ref mOffsetVector, keyframes[mCurrentKeyframe].frame - (int)mFrame, keyframes[mCurrentKeyframe - 1].offset, keyframes[mCurrentKeyframe].offset - keyframes[mCurrentKeyframe - 1].offset, aDuration);
		}
		else
		{
			mOffsetVector = Vector3.Lerp(keyframes[mCurrentKeyframe].offset, keyframes[mCurrentKeyframe - 1].offset, t);
		}
		float angle2 = Mathf.Lerp(keyframes[mCurrentKeyframe].angle, keyframes[mCurrentKeyframe - 1].angle, t);
		ApplyFiltering(ref mOffsetVector, ref angle2);
		mTransform.position -= mPreviousOffset;
		mOffsetVector = Quaternion.Euler(0f, 0f, angle2) * mOffsetVector;
		mTransform.position += mOffsetVector;
		mPreviousOffset = mOffsetVector;
		mWorkingVector.x = 0f;
		mWorkingVector.y = 0f;
		mWorkingVector.z = angle2;
		mTransform.eulerAngles = mWorkingVector;
		mTransform.localScale = Vector3.Scale(mOriginalScale, Vector3.Lerp(keyframes[mCurrentKeyframe].scale, keyframes[mCurrentKeyframe - 1].scale, t));
	}

	public void Play(bool aForward)
	{
		Reset(false);
		mPlay = true;
		if (aForward)
		{
			mForward = true;
		}
		else
		{
			mForward = false;
		}
	}

	public void Reset(bool aResetPosAndScale)
	{
		if (aResetPosAndScale)
		{
			base.transform.localPosition = mOriginalPosition;
			base.transform.localScale = mOriginalScale;
		}
		mPlay = false;
		if (mForward)
		{
			mFrame = 0f;
			mCurrentKeyframe = 0;
		}
		else
		{
			mFrame = keyframes[keyframes.Length - 1].frame;
			mCurrentKeyframe = keyframes.Length - 1;
		}
	}

	private void ApplyFiltering(ref Vector3 offset, ref float angle)
	{
		if (!AffectX)
		{
			offset.x = 0f;
		}
		if (!AffectY)
		{
			offset.y = 0f;
		}
		if (!AffectZ)
		{
			offset.z = 0f;
		}
		if (!AffectOrientation)
		{
			angle = mTransform.eulerAngles.z;
		}
	}

	private void EaseInEaseOut3D(ref Vector3 outVector, int aTime, Vector3 aBegin, Vector3 aChange, int aDuration)
	{
		outVector.x = EaseInEaseOut1D(aTime, aBegin.x, aChange.x, aDuration);
		outVector.y = EaseInEaseOut1D(aTime, aBegin.y, aChange.y, aDuration);
		outVector.z = EaseInEaseOut1D(aTime, aBegin.z, aChange.z, aDuration);
	}

	private float EaseInEaseOut1D(float aTime, float aBegin, float aChange, float aDuration)
	{
		if ((aTime /= 0.5f * aDuration) < 1f)
		{
			return 0.5f * aChange * aTime * aTime + aBegin;
		}
		return (0f - 0.5f * aChange) * ((aTime -= 1f) * (aTime - 2f) - 1f) + aBegin;
	}
}
