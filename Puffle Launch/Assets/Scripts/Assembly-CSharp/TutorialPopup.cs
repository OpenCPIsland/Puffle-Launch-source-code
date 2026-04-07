using UnityEngine;

public class TutorialPopup : MonoBehaviour
{
	private enum Tutorial
	{
		eGravityTutorial = 0,
		ePuffleControlTutorial = 1,
		eGiantPuffleOTutorial = 2,
		eGreenCannonTutorial = 3,
		eRedCannonTutorial = 4,
		ePurpleCannonTutorial = 5,
		eSlingshotTutorial = 6,
		eTutorial_COUNT = 7
	}

	private const string kNextButtonName = "Next";

	private const string kBackButtonName = "Back";

	private const string kCloseButtonName = "Close";

	public string nextButtonPath;

	public string[] nextButtonTexture;

	public string backButtonPath;

	public string[] backButtonTexture;

	public string closeButtonPath;

	public string[] closeButtonTexture;

	public Transform[] mButtonList;

	private Transform mCurrentSelection;

	private bool mExitedButton;

	private InputController mInputController;

	private static TutorialPopup m_singleton;

	private bool m_isActive;

	private GameObject mo_TurorialPrefab;

	private Tutorial m_CurrentTutorial;

	private GameObject mo_RightThumb;

	private GameObject mo_LeftThumb;

	private GameObject mo_TouchIndicator;

	private GameObject mo_Border;

	private Camera m_Camera;

	private GameObject mo_Puffle;

	private GameObject mo_PurpleCannon;

	public static TutorialPopup Instance
	{
		get
		{
			return m_singleton;
		}
	}

	public bool TutorialActive
	{
		get
		{
			return m_isActive;
		}
	}

	private void Start()
	{
		Camera.main.GetComponent<CameraFollow>().ZoomEnabled = false;
		mInputController = GameFlowManager.Instance.InputController;
		if (GameManager.smCurrentLevel == GameManager.Level.eLevel_3)
		{
			SelectTutorial(1);
		}
		else if (GameManager.smCurrentLevel == GameManager.Level.eLevel_6)
		{
			SelectTutorial(5);
		}
		ScaleItem.Instance.ScaleLevelItem(mo_Puffle.transform, 1f, 1f, true);
		ScaleItem.Instance.ScaleLevelItem(mo_PurpleCannon.transform, 1f, 1f, false);
		ScaleItem.Instance.ScaleLevelItem(mo_TurorialPrefab.transform.Find("Cloud1").transform, 1f, 1f, false);
		ScaleItem.Instance.ScaleLevelItem(mo_TurorialPrefab.transform.Find("Cloud2").transform, 1f, 1f, false);
	}

	private void Awake()
	{
		m_singleton = this;
		m_isActive = true;
		mo_TurorialPrefab = base.gameObject;
		mo_RightThumb = mo_TurorialPrefab.transform.Find("RightThumb").gameObject;
		mo_LeftThumb = mo_TurorialPrefab.transform.Find("LeftThumb").gameObject;
		mo_TouchIndicator = mo_TurorialPrefab.transform.Find("TouchScaler").gameObject;
		mo_Border = mo_TurorialPrefab.transform.Find("Border").gameObject;
		m_Camera = mo_TurorialPrefab.transform.Find("Camera").GetComponent<Camera>();
		mo_Puffle = mo_TurorialPrefab.transform.Find("Puffle").gameObject;
		mo_PurpleCannon = mo_TurorialPrefab.transform.Find("ControllableCannon").gameObject;
		float num = 1.5f;
		float num2 = 0.7348f;
		float num3 = 0.13f;
		float num4 = (float)Screen.width / (float)Screen.height;
		float num5 = num / num4 * num2;
		float left = (num2 - num5) * 0.5f + num3;
		m_Camera.rect = new Rect(left, 0.12f, num5, 0.79f);
	}

	private void DestroyTutorial()
	{
		m_isActive = false;
		Camera.main.GetComponent<CameraFollow>().ZoomEnabled = true;
		Object.DestroyImmediate(mo_TurorialPrefab);
	}

	private void CloseAllTutorial()
	{
		mo_Puffle.SetActiveRecursively(false);
		mo_PurpleCannon.SetActiveRecursively(false);
	}

	public void SelectTutorial(int aTutorialNum)
	{
		m_CurrentTutorial = (Tutorial)aTutorialNum;
		CloseAllTutorial();
		mo_Puffle.SetActiveRecursively(true);
		mo_TurorialPrefab.GetComponent<Animation>().Stop();
		mo_TurorialPrefab.GetComponent<Animation>().Play("Tutorial" + aTutorialNum);
		switch (aTutorialNum)
		{
		case 0:
			ResetPuffle();
			ResetThumbs();
			break;
		case 1:
			ResetPuffle();
			break;
		case 2:
			ResetPuffle();
			ResetThumbs();
			break;
		case 3:
			ResetThumbs();
			break;
		case 4:
			ResetThumbs();
			break;
		case 5:
			mo_PurpleCannon.SetActiveRecursively(true);
			SetPuffleTrailEmission(false);
			break;
		case 6:
			break;
		}
	}

	private void LoadNewTexture(Transform button, int index)
	{
		Dynamic3DBackground component = button.GetComponent<Dynamic3DBackground>();
		switch (button.name)
		{
		case "Next":
			component.LoadNewTexture(nextButtonPath, nextButtonTexture[index]);
			break;
		case "Back":
			component.LoadNewTexture(backButtonPath, backButtonTexture[index]);
			break;
		case "Close":
			component.LoadNewTexture(closeButtonPath, closeButtonTexture[index]);
			break;
		}
	}

	private void ResetPuffle()
	{
		SetPuffleSprite(0);
		mo_Puffle.transform.parent = mo_TurorialPrefab.transform;
		mo_Puffle.transform.eulerAngles = new Vector3(0f, 0f, 0f);
		SetPuffleTrailEmission(false);
	}

	private void ResetThumbs()
	{
		mo_LeftThumb.transform.localPosition = new Vector3(14f, 7f, 2f);
		mo_RightThumb.transform.localPosition = new Vector3(-14f, 7f, 2f);
	}

	private void Update()
	{
		if (GameFlowManager.Instance.m_DoWindowBack && !GameFlowManager.Instance.GUIManager.IsPauseMenu)
		{
			DestroyTutorial();
			GameFlowManager.Instance.m_DoWindowBack = false;
		}
		else if (mInputController.TouchCount > 0)
		{
			if (mInputController.TouchDown)
			{
				mCurrentSelection = null;
				Transform[] array = mButtonList;
				foreach (Transform transform in array)
				{
					Bounds bounds = transform.GetComponent<Renderer>().bounds;
					Vector3 center = bounds.center;
					center.z = 0f;
					bounds.center = center;
					bounds.Expand(3f);
					Vector3 point = m_Camera.ScreenToWorldPoint(mInputController.TouchPosition1);
					point.z = 0f;
					if (bounds.Contains(point))
					{
						mCurrentSelection = transform;
						break;
					}
				}
				if (mCurrentSelection != null)
				{
					LoadNewTexture(mCurrentSelection, 1);
				}
			}
			else
			{
				if (!(mCurrentSelection != null))
				{
					return;
				}
				Bounds bounds2 = mCurrentSelection.GetComponent<Renderer>().bounds;
				Vector3 center2 = bounds2.center;
				center2.z = 0f;
				bounds2.center = center2;
				bounds2.Expand(3f);
				Vector3 point2 = m_Camera.ScreenToWorldPoint(mInputController.TouchPosition1);
				point2.z = 0f;
				if (bounds2.Contains(point2))
				{
					if (mExitedButton)
					{
						LoadNewTexture(mCurrentSelection, 1);
						mExitedButton = false;
					}
				}
				else if (!mExitedButton)
				{
					LoadNewTexture(mCurrentSelection, 0);
					mExitedButton = true;
				}
			}
		}
		else
		{
			if (!(mCurrentSelection != null))
			{
				return;
			}
			LoadNewTexture(mCurrentSelection, 0);
			if (mInputController.Release && !mExitedButton)
			{
				switch (mCurrentSelection.name)
				{
				case "Next":
				{
					int num = (int)(m_CurrentTutorial + 1) % 7;
					break;
				}
				case "Back":
				{
					int value = (int)(m_CurrentTutorial - 1) % 7;
					value = Mathf.Clamp(value, 0, 7);
					break;
				}
				case "Close":
					DestroyTutorial();
					break;
				}
				mCurrentSelection = null;
			}
			mExitedButton = false;
		}
	}

	private void SetPuffleSprite(int index)
	{
		if (index == 0)
		{
			mo_Puffle.GetComponent<SpriteManager>().Seek(1);
		}
		else
		{
			mo_Puffle.GetComponent<SpriteManager>().Seek(11);
		}
	}

	private void LaunchPuffleAnim()
	{
		switch (m_CurrentTutorial)
		{
		case Tutorial.ePurpleCannonTutorial:
			mo_PurpleCannon.GetComponentInChildren<TweeningController>().Play(true);
			break;
		}
		mo_Puffle.transform.parent = mo_TurorialPrefab.transform;
		SetPuffleSprite(0);
	}

	private void SetPuffleParent()
	{
		switch (m_CurrentTutorial)
		{
		case Tutorial.eGravityTutorial:
			break;
		case Tutorial.ePuffleControlTutorial:
			break;
		case Tutorial.eGiantPuffleOTutorial:
			break;
		case Tutorial.eGreenCannonTutorial:
			break;
		case Tutorial.eRedCannonTutorial:
			break;
		case Tutorial.ePurpleCannonTutorial:
			mo_Puffle.transform.parent = mo_PurpleCannon.transform;
			mo_Puffle.transform.localRotation = Quaternion.identity;
			break;
		case Tutorial.eSlingshotTutorial:
			break;
		}
	}

	private void ShowPressAnim(int index)
	{
		Transform transform = mo_TouchIndicator.transform;
		if (index == 0)
		{
			transform.parent = mo_LeftThumb.transform;
			mo_LeftThumb.transform.localScale = Vector3.one * 1.85f;
		}
		else
		{
			transform.parent = mo_RightThumb.transform;
			Vector3 localScale = Vector3.one * 1.85f;
			localScale.x *= -1f;
			mo_RightThumb.transform.localScale = localScale;
		}
		transform.localPosition = new Vector3(1f, -1f, -1f);
		transform.localEulerAngles = Vector3.zero;
		transform.localScale = Vector3.one * 0.5f;
		mo_TouchIndicator.GetComponentInChildren<MeshRenderer>().enabled = true;
	}

	private void RemovePressAnim()
	{
		mo_LeftThumb.transform.localScale = Vector3.one * 2f;
		Vector3 localScale = Vector3.one * 2f;
		localScale.x *= -1f;
		mo_RightThumb.transform.localScale = localScale;
		mo_TouchIndicator.GetComponentInChildren<MeshRenderer>().enabled = false;
	}

	private void SetPuffleTrailEmission(bool enabled)
	{
		ParticleSystem componentInChildren = mo_Puffle.GetComponentInChildren<ParticleSystem>(true);
		if (componentInChildren == null)
		{
			return;
		}
		ParticleSystem.EmissionModule emission = componentInChildren.emission;
		emission.enabled = enabled;
		if (enabled)
		{
			componentInChildren.Play();
		}
		else
		{
			componentInChildren.Stop(true, ParticleSystemStopBehavior.StopEmitting);
		}
	}
}
