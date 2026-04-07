using UnityEngine;

[RequireComponent(typeof(Cannon))]
public class CannonController : MonoBehaviour
{
	public enum CannonState
	{
		eEmpty = 0,
		eIdle = 1,
		eRotate = 2,
		eLaunch = 3,
		eCannonState_COUNT = 4
	}

	private const float m_MinDistanceFromCannonForRotation = 4f;

	public const float touchDectionBoundMultiplier = 1.8f;

	private int m_RotateFingerId = -1;

	public float rotationSpeed = 1f;

	private Cannon mCannon;

	private Transform mCannonTransform;

	private InputController mInputController;

	private Vector3 tempVector = default(Vector3);

	private Camera mCamera;

	private CannonState mCannonState;

	public void Start()
	{
		mCannon = GetComponent<Cannon>();
		mCannonTransform = mCannon.transform;
		mInputController = GameFlowManager.Instance.InputController;
		mCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
		mCannonState = CannonState.eEmpty;
	}

	public void Update()
	{
		if (GameManager.Instance.IsPause())
		{
			mCannonState = CannonState.eIdle;
		}
		else
		{
			if (!mCannon.IsPuffleInside())
			{
				return;
			}
			if (mCannonState == CannonState.eEmpty)
			{
				if (mInputController.TouchCount > 0)
				{
					return;
				}
				mCannonState = CannonState.eIdle;
			}
			else if (mInputController.PreviousTouchCount < 2 && mInputController.Release)
			{
				m_RotateFingerId = -1;
				bool flag = GameManager.HasCompletedTurboMode(GameManager.Instance.CurrentWorld) && GameFlowManager.Instance.GUIManager.HudManager.InGameHud.mo_slowMoButton.ContainsTouchRelease();
				mCannonState = ((mCannonState != CannonState.eIdle || flag) ? CannonState.eIdle : CannonState.eLaunch);
			}
			else if (mInputController.TouchCount == 1 && (!GameManager.HasCompletedTurboMode(GameManager.Instance.CurrentWorld) || !GameFlowManager.Instance.GUIManager.HudManager.InGameHud.mo_slowMoButton.ContainsTouchRelease()) && !mInputController.DetectingFirstTap && (mInputController.HasFinger1Moved || mInputController.LongHold) && m_RotateFingerId == -1)
			{
				m_RotateFingerId = mInputController.FirstFingerId;
				mCannonState = CannonState.eRotate;
			}
			switch (mCannonState)
			{
			case CannonState.eRotate:
				RotateCannon();
				break;
			case CannonState.eLaunch:
				mCannon.LaunchPuffle();
				mCannonState = CannonState.eEmpty;
				break;
			}
		}
	}

	private void RotateCannon()
	{
		Puffle.ControlType smControlType = Puffle.smControlType;
		if (smControlType != Puffle.ControlType.eTouchScreen && smControlType != Puffle.ControlType.eTilting)
		{
			return;
		}
		if (mInputController.TouchCount > 0)
		{
			if (m_RotateFingerId != mInputController.FirstFingerId)
			{
				return;
			}
			tempVector = mCamera.ScreenToWorldPoint(mInputController.TouchPosition1);
			tempVector.z = mCannonTransform.position.z;
			Vector3 vector = mCannonTransform.position - tempVector;
			if (!(vector.magnitude < 4f))
			{
				float num = Vector3.Angle(mCannonTransform.right, vector);
				Vector3 vector2 = Vector3.Cross(mCannonTransform.right, vector);
				tempVector = mCannonTransform.eulerAngles;
				if (vector2.z > 0f)
				{
					tempVector.z += num;
				}
				else
				{
					tempVector.z -= num;
				}
				mCannonTransform.eulerAngles = tempVector;
			}
		}
		else
		{
			m_RotateFingerId = -1;
			mCannonState = CannonState.eIdle;
		}
	}
}
