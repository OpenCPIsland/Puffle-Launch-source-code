using UnityEngine;

public class TimeManager : MonoBehaviour
{
	private const float mkOriginalFramerate = 24f;

	private const float mkSlowmoScale = 0.2f;

	private const float mkSlowmoDuration = 2.5f;

	public const float mkTurboScaleMin = 1f;

	public const float mkTurboScaleMax = 3f;

	public const float mkTurboScaleInc = 0.025f;

	private static TimeManager mInstance;

	private float mTimeScale = 1f;

	private float mSlowmoTimer;

	private bool mPaused;

	private float mOriginalTimeScale;

	private bool mSlowmoOverride;

	private float mkTurboScale = 1.45f;

	public static TimeManager Instance
	{
		get
		{
			return mInstance;
		}
	}

	public float DeltaTime
	{
		get
		{
			if (mPaused)
			{
				return 0f;
			}
			return Time.deltaTime * 24f * mTimeScale;
		}
	}

	public float TimeScale
	{
		get
		{
			return mTimeScale;
		}
	}

	public float TimeScaleRatio
	{
		get
		{
			if (mSlowmoTimer > 0f)
			{
				return 0f;
			}
			return (mTimeScale - 0.2f) / 0.8f;
		}
	}

	public bool SlowmoOverride
	{
		get
		{
			return mSlowmoOverride;
		}
		set
		{
			mSlowmoOverride = value;
		}
	}

	public float GetTurboScale()
	{
		return mkTurboScale;
	}

	public void AdjustTurboScale(float af_adjustmentAmount)
	{
		mkTurboScale += af_adjustmentAmount;
		if (mkTurboScale < 1f)
		{
			mkTurboScale = 1f;
		}
		if (mkTurboScale > 3f)
		{
			mkTurboScale = 3f;
		}
		Time.timeScale = mkTurboScale;
		mOriginalTimeScale = Time.timeScale;
	}

	public void Awake()
	{
		mInstance = this;
		Time.fixedDeltaTime = 1f / 24f;
	}

	public void Start()
	{
		mOriginalTimeScale = Time.timeScale;
	}

	public void FixedUpdate()
	{
		if (!mSlowmoOverride)
		{
			if (mSlowmoTimer > 0f)
			{
				mSlowmoTimer -= Time.deltaTime;
			}
			else if (mTimeScale < 1f)
			{
				mTimeScale = Mathf.Min(1f, mTimeScale + 0.015f * Time.deltaTime * 24f);
			}
		}
	}

	public void ActivateSlowmo()
	{
		mSlowmoTimer = 2.5f;
		mTimeScale = 0.2f;
		Camera.main.GetComponentInChildren<VisualEffects>().ShowSlowMoFX(true);
	}

	public void StopSlowmo()
	{
		if (!mSlowmoOverride)
		{
			mTimeScale = 1f;
			VisualEffects componentInChildren = Camera.main.GetComponentInChildren<VisualEffects>();
			if (componentInChildren != null)
			{
				componentInChildren.ShowSlowMoFX(false);
			}
		}
	}

	public bool IsSlowMo()
	{
		return mTimeScale == 0.2f;
	}

	public void ActivateTurbo()
	{
		Time.timeScale = mkTurboScale;
		mOriginalTimeScale = Time.timeScale;
	}

	public void StopTurbo()
	{
		Time.timeScale = 1f;
		mOriginalTimeScale = Time.timeScale;
	}

	public void Pause(bool aPaused)
	{
		mPaused = aPaused;
		if (mPaused)
		{
			Time.timeScale = 0f;
		}
		else
		{
			Time.timeScale = mOriginalTimeScale;
		}
	}
}
