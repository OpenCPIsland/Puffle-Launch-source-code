using UnityEngine;

public class StartOfGameDelay : MonoBehaviour
{
	private enum State
	{
		ePaused = 0,
		eUnPaused = 1,
		eStateCount = 2
	}

	public float m_InitialDelay;

	public float m_CountdownDelay;

	public int m_CountdownCount;

	public ProgressText m_CountdownText;

	private static StartOfGameDelay m_cInstance;

	private float mStartTime;

	private bool mCountdownStarted;

	private int mCountdownValue;

	private bool m_DoInit = true;

	private State mState;

	private float mTimeWhenPause;

	private float mTimePaused;

	public static StartOfGameDelay Instance
	{
		get
		{
			return m_cInstance;
		}
	}

	private void Awake()
	{
		m_cInstance = this;
	}

	private void Start()
	{
		mState = State.eUnPaused;
		m_DoInit = true;
		GameManager.Instance.Pause(true);
		mCountdownStarted = false;
		mCountdownValue = 0;
		GameObject.Find("TouchIndicator").GetComponent<Renderer>().material.mainTexture = Resources.Load("Textures/TouchIndicator/tap-thing", typeof(Texture2D)) as Texture2D;
	}

	public void OnApplicationPause(bool aPause)
	{
		if (aPause)
		{
			mState = State.ePaused;
			mTimeWhenPause = Time.realtimeSinceStartup;
		}
	}

	public void RestartLevel()
	{
		mStartTime = Time.realtimeSinceStartup;
		mTimePaused = 0f;
	}

	private void Update()
	{
		GameManager.Instance.Pause(true);
		if (!LevelLoader.Instance.isLoadingFinished)
		{
			return;
		}
		if (mState == State.ePaused)
		{
			if (GameFlowManager.Instance.GUIManager.CurrentScene == GUIManager.Scene.ePauseMenu)
			{
				return;
			}
			mTimePaused += Time.realtimeSinceStartup - mTimeWhenPause;
			mState = State.eUnPaused;
		}
		if (m_DoInit)
		{
			m_DoInit = false;
			Resources.UnloadUnusedAssets();
			mStartTime = Time.realtimeSinceStartup;
			GameFlowManager.Instance.GUIManager.LoadingScreen.TextureData[0].bgInfo.bgColor = new Color(1f, 1f, 1f, 1f);
			if (GameManager.smCurrentLevel == GameManager.Level.eLevel_3 || GameManager.smCurrentLevel == GameManager.Level.eLevel_6)
			{
				GameObject gameObject = Object.Instantiate(Resources.Load("Prefabs/Tutorial", typeof(Object))) as GameObject;
				gameObject.transform.parent = Camera.main.transform;
				gameObject.transform.localPosition = new Vector3(0f, 0f, -100f);
				gameObject.transform.eulerAngles = new Vector3(90f, 180f, 0f);
			}
			return;
		}
		float num = Time.realtimeSinceStartup - mStartTime - mTimePaused;
		float a = 1f - num / m_InitialDelay;
		GameFlowManager.Instance.GUIManager.LoadingScreen.TextureData[0].bgInfo.useBgColor = true;
		GameFlowManager.Instance.GUIManager.LoadingScreen.TextureData[0].bgInfo.bgColor.a = a;
		if (mCountdownStarted)
		{
			if (!(num > m_InitialDelay + m_CountdownDelay * (float)(mCountdownValue + 1)))
			{
				return;
			}
			mCountdownValue++;
			if (m_CountdownCount - mCountdownValue <= 0)
			{
				if (GameFlowManager.Instance.GUIManager.CurrentScene != GUIManager.Scene.ePauseMenu)
				{
					GameManager.Instance.Pause(false);
					Puffle.Instance.StopMovement = false;
				}
				GameManager.Instance.EnableTiming = true;
				GameFlowManager.Instance.GUIManager.LoadingScreen.TextureData[0].bgInfo.bgColor.a = 1f;
				Object.Destroy(m_CountdownText.gameObject);
				Object.Destroy(base.gameObject);
			}
			else
			{
				m_CountdownText.Show = false;
				string text = (m_CountdownCount - mCountdownValue).ToString();
				m_CountdownText.GetComponent<TextMesh>().text = text;
				if (m_CountdownText.textShadow != null)
				{
					m_CountdownText.textShadow.GetComponent<TextMesh>().text = text;
				}
				m_CountdownText.Show = true;
			}
		}
		else
		{
			if (!(num > m_InitialDelay))
			{
				return;
			}
			if ((GameManager.smCurrentLevel == GameManager.Level.eLevel_3 || GameManager.smCurrentLevel == GameManager.Level.eLevel_6) && TutorialPopup.Instance != null)
			{
				GameManager.Instance.Pause(false);
				Puffle.Instance.StopMovement = true;
				return;
			}
			mStartTime = Time.realtimeSinceStartup - m_InitialDelay - mTimePaused;
			GameFlowManager.Instance.GUIManager.HideLoadingScreen(false);
			GameFlowManager.Instance.GUIManager.LoadingScreen.TextureData[0].bgInfo.useBgColor = false;
			mCountdownStarted = true;
			m_CountdownText.GetComponent<TextMesh>().text = m_CountdownCount.ToString();
			if (m_CountdownText.textShadow != null)
			{
				m_CountdownText.textShadow.GetComponent<TextMesh>().text = m_CountdownCount.ToString();
			}
			m_CountdownText.Show = true;
		}
	}
}
