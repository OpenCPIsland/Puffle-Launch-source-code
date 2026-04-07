using UnityEngine;

public class AnimatedText : MonoBehaviour
{
	private enum TextState
	{
		eFadingIn = 0,
		eDisplayed = 1,
		eHidden = 2,
		TextState_COUNT = 3
	}

	public Color textColor;

	public float fadeTime;

	public float scaleTime;

	public float minScale;

	public float maxScale;

	public MeshRenderer textShadow;

	public bool enableFireworks;

	public float fireworkDisplayTime;

	public float fireworkScale;

	private float mStateStart;

	private float mScaleStart;

	private float mFireworkTimer;

	private MeshRenderer mTextRenderer;

	private TextState mState = TextState.eHidden;

	private Vector3 mOriginalScale;

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
		mTextRenderer.material.color = textColor;
		mTextRenderer.enabled = false;
		if (textShadow != null)
		{
			textShadow.enabled = false;
		}
		base.transform.localScale = mOriginalScale * minScale;
		mLastFrameTimeStamp = Time.realtimeSinceStartup;
	}

	private void Update()
	{
		switch (mState)
		{
		case TextState.eFadingIn:
			FadeIn();
			break;
		}
		if (mState != TextState.eHidden)
		{
			ScaleUpText();
			float num = Time.realtimeSinceStartup - mStateStart;
			if (enableFireworks && num <= fireworkDisplayTime)
			{
				FireworkEffect();
			}
		}
	}

	private void FadeIn()
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

	private void ScaleUpText()
	{
		float num = Time.realtimeSinceStartup - mScaleStart;
		float t = num / scaleTime;
		base.transform.localScale = mOriginalScale * Mathf.Lerp(minScale, maxScale, t);
	}

	private void FireworkEffect()
	{
		if (mFireworkTimer < 0f)
		{
			GameObject gameObject = Object.Instantiate(Resources.Load("Prefabs/FX/AnimatedSparkle", typeof(Object))) as GameObject;
			gameObject.layer = base.gameObject.layer;
			Vector3 localScale = gameObject.transform.localScale;
			localScale.x *= fireworkScale;
			localScale.y *= fireworkScale;
			gameObject.transform.localScale = localScale;
			Vector3 position = base.transform.position;
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
