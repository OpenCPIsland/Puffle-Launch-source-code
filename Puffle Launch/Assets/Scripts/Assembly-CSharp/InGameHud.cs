using System;
using UnityEngine;

public class InGameHud : BaseGUI
{
	public enum Button
	{
		ePause = 0,
		eSlowMo = 1,
		eCompleteLevel = 2,
		eButton_COUNT = 3
	}

	private int mi_loadFrames;

	public bool mb_isInitialized;

	public CustomButton3D mo_pauseButton;

	public CustomButton3D mo_slowMoButton;

	public TextMesh mo_timer;

	public TextMesh mo_timerShadow;

	private Vector3 m_WorkingVector = default(Vector3);

	private bool mSlowMoButtonEnable = true;

	public Vector2 mv2_slowmotionButtonPositionRatio = new Vector2(0.0175f, 0.875f);

	public Vector2 mv2_slowmotionButtonSizeRatio = new Vector2(0.084375f, 0.1046875f);

	public Vector2 mv2_slowmotionButtonCenterPixelPosition;

	private bool mb_isVisible = true;

	public InGameHud(GameObject aRefObj)
		: base(aRefObj)
	{
		mv2_slowmotionButtonCenterPixelPosition = new Vector2((float)(((double)mv2_slowmotionButtonPositionRatio.x + 0.5 * (double)mv2_slowmotionButtonSizeRatio.x) * (double)GUIConstants.kReferenceScreenWidth), (float)(((double)mv2_slowmotionButtonPositionRatio.y + 0.5 * (double)mv2_slowmotionButtonSizeRatio.y) * (double)Screen.height));
		mb_isInitialized = false;
		mo_pauseButton = null;
		mo_slowMoButton = null;
		mo_timer = null;
		mo_timerShadow = null;
		mi_loadFrames = 10;
	}

	public void OnPause(object sender, EventArgs e)
	{
		GameFlowManager.Instance.AudioManager.PlayUISFx(GameFlowManager.Instance.MenuClick24);
		GameFlowManager.Instance.GUIManager.ShowPauseMenu(true);
	}

	public void OnSlowMo(object sender, EventArgs e)
	{
		if (mSlowMoButtonEnable)
		{
			if (mo_slowMoButton.mb_toggleState)
			{
				GameManager.Instance.ActivatePlayerSlowMo();
			}
			else
			{
				GameManager.Instance.StopPlayerSlowMo();
			}
		}
	}

	protected override void CreateLayouts()
	{
	}

	private bool Init()
	{
		if (mi_loadFrames > 0)
		{
			mi_loadFrames--;
			return false;
		}
		if (!mb_isInitialized)
		{
			Camera mainCamera = Camera.main;
			float num = mainCamera.orthographicSize * mainCamera.aspect;
			if (mo_pauseButton == null && (bool)GameObject.Find("PauseButton"))
			{
				mo_pauseButton = GameObject.Find("PauseButton").GetComponent<CustomButton3D>();
				if (mo_pauseButton != null)
				{
					Bounds bounds = mo_pauseButton.GetComponent<MeshFilter>().GetComponent<Renderer>().bounds;
					m_WorkingVector = mo_pauseButton.transform.position;
					m_WorkingVector.x = 1.5f * bounds.extents.x - num;
					mo_pauseButton.transform.position = m_WorkingVector;
					mo_pauseButton.mf_detectionZoneScale = 5f;
					mo_pauseButton.InitButtonBounds();
					mo_pauseButton.customOnSelect += OnPause;
					mo_pauseButton.gameObject.SetActiveRecursively(false);
				}
			}
			if (mo_slowMoButton == null && (bool)GameObject.Find("SlowMoButton"))
			{
				mo_slowMoButton = GameObject.Find("SlowMoButton").GetComponent<CustomButton3D>();
				if (mo_slowMoButton != null)
				{
					Bounds bounds2 = mo_slowMoButton.GetComponent<MeshFilter>().GetComponent<Renderer>().bounds;
					m_WorkingVector = mo_slowMoButton.transform.position;
					m_WorkingVector.x = 1.5f * bounds2.extents.x - num;
					mo_slowMoButton.transform.position = m_WorkingVector;
					mo_slowMoButton.mf_detectionZoneScale = 5f;
					mo_slowMoButton.InitButtonBounds();
					mo_slowMoButton.customOnSelect += OnSlowMo;
					mo_slowMoButton.gameObject.SetActiveRecursively(false);
				}
			}
			if (mo_timer == null || mo_timerShadow == null)
			{
				if (mo_timer == null && (bool)GameObject.Find("Timer"))
				{
					mo_timer = GameObject.Find("Timer").GetComponent<TextMesh>();
				}
				if (mo_timerShadow == null && (bool)GameObject.Find("TimerShadow"))
				{
					mo_timerShadow = GameObject.Find("TimerShadow").GetComponent<TextMesh>();
				}
				if (mo_timer != null && mo_timerShadow != null)
				{
					UpdateTimeDisplay();
					mo_timer.gameObject.SetActiveRecursively(false);
					mo_timerShadow.gameObject.SetActiveRecursively(false);
				}
			}
			if (mo_pauseButton != null && mo_slowMoButton != null && mo_timer != null && mo_timerShadow != null)
			{
				mb_isInitialized = true;
			}
		}
		return mb_isInitialized;
	}

	public void Update()
	{
		if (Init())
		{
			bool flag = StartOfGameDelay.Instance == null && GameFlowManager.Instance.GUIManager.CurrentScene != GUIManager.Scene.eTallyMenu && mb_isVisible;
			mo_pauseButton.gameObject.SetActiveRecursively(flag);
			mo_slowMoButton.gameObject.SetActiveRecursively(GameManager.HasCompletedTurboMode(GameManager.Instance.CurrentWorld) && flag);
			mo_timer.gameObject.SetActiveRecursively(GameManager.HasCollectedAllRings(GameManager.Instance.CurrentWorld) && flag);
			mo_timerShadow.gameObject.SetActiveRecursively(GameManager.HasCollectedAllRings(GameManager.Instance.CurrentWorld) && flag);
			UpdateTimeDisplay();
		}
	}

	public void SetVisible(bool ab_isVisible)
	{
		mb_isVisible = ab_isVisible;
		bool activeRecursively = StartOfGameDelay.Instance == null && GameFlowManager.Instance.GUIManager.CurrentScene != GUIManager.Scene.eTallyMenu && mb_isVisible;
		mo_pauseButton.gameObject.SetActiveRecursively(activeRecursively);
		mo_slowMoButton.gameObject.SetActiveRecursively(activeRecursively);
		mo_timer.gameObject.SetActiveRecursively(activeRecursively);
		mo_timerShadow.gameObject.SetActiveRecursively(activeRecursively);
	}

	protected override void OnButtonSelect()
	{
		Button selectedButton = (Button)base.SelectedButton;
		if (selectedButton == Button.eCompleteLevel && GameObject.FindGameObjectWithTag("Player") != null && GameObject.FindGameObjectWithTag("Finish") != null)
		{
			GameObject.FindGameObjectWithTag("Player").transform.position = GameObject.FindGameObjectWithTag("Finish").transform.position;
		}
		ResetButton();
	}

	public void SetSlowmoButtonState(bool aActive)
	{
		mo_slowMoButton.mb_toggleState = aActive;
	}

	public void SetSlowmoButtonVisible(bool aVisible)
	{
		mo_slowMoButton.gameObject.SetActiveRecursively(aVisible);
	}

	public void SetSlowMoButtonEnable(bool aEnable)
	{
		mSlowMoButtonEnable = aEnable;
		mo_slowMoButton.DisableTouch(!aEnable);
	}

	private void UpdateTimeDisplay()
	{
		if (mo_timer != null && mo_timer.gameObject.activeSelf)
		{
			mo_timer.text = GameManager.GetTimeFormatedString(GameManager.smCurrentTimeCount);
		}
		if (mo_timerShadow != null && mo_timerShadow.gameObject.activeSelf)
		{
			mo_timerShadow.text = GameManager.GetTimeFormatedString(GameManager.smCurrentTimeCount);
		}
	}
}
