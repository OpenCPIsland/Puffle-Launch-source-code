using UnityEngine;

public class ProgressText : MonoBehaviour
{
	private enum TextState
	{
		eFadingIn = 0,
		eFadingOut = 1,
		eDisplayed = 2,
		eHidden = 3,
		TextState_COUNT = 4
	}

	public float fadeTime;

	public float displayTime;

	public float minScale;

	public float maxScale;

	public MeshRenderer textShadow;

	public bool enableFireworks;

	private float mStateStart;

	private float mScaleStart;

	private float mFireworkTimer;

	private MeshRenderer mTextRenderer;

	private TextState mState = TextState.eHidden;

	private Vector3 mOriginalScale;

	private Bounds mBounds;

	private float mLastFrameTimeStamp;

	public bool Show
	{
		set
		{
			if (value && mState == TextState.eHidden)
			{
				mState = TextState.eFadingIn;
				mTextRenderer.enabled = true;
				if (textShadow != null)
				{
					textShadow.enabled = true;
				}
				mStateStart = (mScaleStart = Time.realtimeSinceStartup);
			}
			else
			{
				mState = TextState.eHidden;
				mTextRenderer.enabled = false;
				if (textShadow != null)
				{
					textShadow.enabled = false;
				}
			}
		}
	}

	private void Start()
	{
		mOriginalScale = base.transform.localScale;
		mState = TextState.eHidden;
		mTextRenderer = GetComponent<MeshRenderer>();
		mTextRenderer.material.color = new Color(0.85f, 0.85f, 0.85f);
		mTextRenderer.enabled = false;
		mBounds = mTextRenderer.bounds;
		if (textShadow != null)
		{
			textShadow.enabled = false;
		}
		base.transform.localScale = mOriginalScale * minScale;
		mLastFrameTimeStamp = Time.realtimeSinceStartup;
	}

	private void Update()
	{
		if (mState == TextState.eFadingIn)
		{
			float num = Time.realtimeSinceStartup - mStateStart;
			Color color = mTextRenderer.material.color;
			float t = num / fadeTime;
			color.a = Mathf.Lerp(0f, 1f, t);
			mTextRenderer.material.color = color;
			if (textShadow != null)
			{
				color = textShadow.material.color;
				color.a = Mathf.Lerp(0f, 1f, t);
				textShadow.material.color = color;
			}
			if (num > fadeTime)
			{
				mStateStart = Time.realtimeSinceStartup;
				mState = TextState.eDisplayed;
			}
		}
		else if (mState == TextState.eFadingOut)
		{
			float num2 = Time.realtimeSinceStartup - mStateStart;
			Color color2 = mTextRenderer.material.color;
			float t2 = num2 / fadeTime;
			color2.a = Mathf.Lerp(1f, 0f, t2);
			mTextRenderer.material.color = color2;
			if (textShadow != null)
			{
				color2 = textShadow.material.color;
				color2.a = Mathf.Lerp(1f, 0f, t2);
				textShadow.material.color = color2;
			}
			if (num2 > fadeTime)
			{
				mStateStart = Time.realtimeSinceStartup;
				mState = TextState.eHidden;
			}
		}
		else if (mState == TextState.eDisplayed)
		{
			float num3 = Time.realtimeSinceStartup - mStateStart;
			if (num3 > displayTime)
			{
				mStateStart = Time.realtimeSinceStartup;
				mState = TextState.eFadingOut;
			}
		}
		else if (mState == TextState.eHidden)
		{
			mTextRenderer.enabled = false;
			if (textShadow != null)
			{
				textShadow.enabled = false;
			}
		}
		if (mState == TextState.eHidden)
		{
			return;
		}
		float num4 = Time.realtimeSinceStartup - mScaleStart;
		float t3 = num4 / (2f * fadeTime + displayTime);
		base.transform.localScale = mOriginalScale * Mathf.Lerp(minScale, maxScale, t3);
		if (enableFireworks)
		{
			if (mFireworkTimer < 0f)
			{
				float num5 = mBounds.center.x + Random.Range(0f - mBounds.extents.x / 2f, mBounds.extents.x / 2f);
				float num6 = mBounds.center.y + Random.Range(0f - mBounds.extents.y / 2f, mBounds.extents.y / 2f);
				float z = -9f;
				GameObject gameObject = Object.Instantiate(Resources.Load("Prefabs/FX/Sparkle", typeof(Object))) as GameObject;
				Vector3 position = Camera.main.transform.position;
				position.x += num5;
				position.y += num6;
				position.z = z;
				gameObject.transform.position = position;
				mFireworkTimer = Random.Range(0.1f, 0.25f);
			}
			else
			{
				mFireworkTimer -= Time.realtimeSinceStartup - mLastFrameTimeStamp;
				mLastFrameTimeStamp = Time.realtimeSinceStartup;
			}
		}
	}
}
