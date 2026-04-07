using UnityEngine;

public class TutorialPanel : MonoBehaviour
{
	private const float kFocusMoveSpeed = 0.1f;

	private const float kFocusZoomSpeed = 0.1f;

	private const float kFocusSize = 10f;

	public int triggerIndex;

	public bool keepContainerInView = true;

	private Transform mTransform;

	private Puffle mPlayer;

	private PuffleContainer mPuffleContainer;

	private bool mTutorialShown;

	private float mPanelDepth;

	private float mCameraMoveSpeed;

	private float mCameraZoomSpeed;

	private Vector3 mCameraTarget;

	private float mCameraSize;

	private bool mAutoLaunch;

	private bool m_DoInit = true;

	private void Start()
	{
		m_DoInit = true;
	}

	private void Update()
	{
		if (GameManager.Instance.CurrentWorld == GameManager.World.eWorld_BonusWorld || !LevelLoader.Instance.isLoadingFinished)
		{
			return;
		}
		if (m_DoInit)
		{
			m_DoInit = false;
			if (ProfileManager.Instance.CurrentProfile.m_LevelData[(int)GameManager.smCurrentLevel].LevelComplete)
			{
				base.enabled = false;
			}
			mTransform = base.transform;
			mPlayer = Puffle.Instance;
			PuffleContainer[] array = (PuffleContainer[])Object.FindObjectsOfType(typeof(PuffleContainer));
			mPuffleContainer = array[triggerIndex];
			mTutorialShown = false;
			mPanelDepth = mTransform.position.z;
			Cannon component = mPuffleContainer.GetComponent<Cannon>();
			if ((bool)component)
			{
				mAutoLaunch = component.autoLaunch;
			}
			if (keepContainerInView)
			{
				Camera.main.GetComponentInChildren<VisualEffects>().TutorialObject = mPuffleContainer.transform;
			}
			else
			{
				Camera.main.GetComponentInChildren<VisualEffects>().TutorialObject = null;
			}
		}
		else if (mTutorialShown)
		{
			bool flag = false;
			if (GameFlowManager.Instance.GUIManager.CurrentScene != GUIManager.Scene.ePauseMenu && GameFlowManager.Instance.InputController.TouchCount > 0)
			{
				flag = true;
			}
			if (flag)
			{
				Puffle.Instance.DisableInput = false;
				CameraFollow component2 = Camera.main.GetComponent<CameraFollow>();
				component2.moveSpeed = mCameraMoveSpeed;
				component2.zoomSpeed = mCameraZoomSpeed;
				component2.TargetPosition = mCameraTarget;
				component2.TargetSize = mCameraSize;
				component2.ZoomOverride = false;
				component2.Target = Puffle.Instance.transform;
				Camera.main.GetComponentInChildren<VisualEffects>().ShowTutorialFX(false);
				Vector3 position = mTransform.position;
				position.z = mPanelDepth;
				mTransform.position = position;
				if (mAutoLaunch)
				{
					mPuffleContainer.GetComponent<Cannon>().autoLaunch = true;
				}
				base.enabled = false;
			}
		}
		else if ((mPlayer.State == Puffle.PuffleState.eInCannon || mPlayer.State == Puffle.PuffleState.eInSlingshot) && mPuffleContainer.IsPuffleInside())
		{
			Puffle.Instance.DisableInput = true;
			mTutorialShown = true;
			Vector3 position2 = mTransform.position;
			position2.z = Camera.main.transform.position.z + 4f;
			mTransform.position = position2;
			Camera.main.GetComponentInChildren<VisualEffects>().ShowTutorialFX(true);
			CameraFollow component3 = Camera.main.GetComponent<CameraFollow>();
			mCameraMoveSpeed = component3.moveSpeed;
			mCameraZoomSpeed = component3.zoomSpeed;
			component3.moveSpeed = 0.1f;
			component3.zoomSpeed = 0.1f;
			mCameraTarget = component3.TargetPosition;
			mCameraSize = component3.TargetSize;
			if (keepContainerInView)
			{
				Vector3 vector = (mTransform.position + mPuffleContainer.transform.position) / 2f;
				component3.TargetPosition = new Vector3(vector.x, vector.y, mCameraTarget.z);
			}
			else
			{
				component3.TargetPosition = new Vector3(mTransform.position.x, mTransform.position.y, mCameraTarget.z);
			}
			if (GameManager.smCurrentLevel != GameManager.Level.eLevel_1 && GameManager.smCurrentLevel != GameManager.Level.eLevel_5 && GameManager.smCurrentLevel != GameManager.Level.eLevel_6 && GameManager.smCurrentLevel != GameManager.Level.eLevel_14)
			{
				component3.Target = null;
			}
			component3.TargetSize = 10f;
			component3.ZoomOverride = true;
			if (mAutoLaunch)
			{
				mPuffleContainer.GetComponent<Cannon>().autoLaunch = false;
			}
		}
	}
}
