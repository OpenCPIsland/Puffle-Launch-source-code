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

    public float rotationSpeed = 5f;

    private Cannon mCannon;

    private Transform mCannonTransform;

    private InputController mInputController;

    private Vector3 tempVector = default(Vector3);

    private Camera mCamera;

    private CannonState mCannonState;

    private Vector3 lastPointerPosition;

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
            return;
        }

        if (!mCannon.IsPuffleInside())
            return;

        if (mCannonState == CannonState.eEmpty)
        {
            if (mInputController.TouchCount > 0)
                return;
            mCannonState = CannonState.eIdle;
        }
        else if (mInputController.PreviousTouchCount < 2 && mInputController.Release)
        {
            m_RotateFingerId = -1;
            bool flag = GameManager.HasCompletedTurboMode(GameManager.Instance.CurrentWorld) &&
                        GameFlowManager.Instance.GUIManager.HudManager.InGameHud.mo_slowMoButton.ContainsTouchRelease();
            mCannonState = ((mCannonState != CannonState.eIdle || flag) ? CannonState.eIdle : CannonState.eLaunch);
        }
        else if (mInputController.TouchCount == 1 && (!GameManager.HasCompletedTurboMode(GameManager.Instance.CurrentWorld) ||
                 !GameFlowManager.Instance.GUIManager.HudManager.InGameHud.mo_slowMoButton.ContainsTouchRelease()) &&
                 !mInputController.DetectingFirstTap &&
                 (mInputController.HasFinger1Moved || mInputController.LongHold) && m_RotateFingerId == -1)
        {
            m_RotateFingerId = mInputController.FirstFingerId;
            mCannonState = CannonState.eRotate;
        }
        else if (Input.mousePresent)
        {
            mCannonState = CannonState.eRotate;
            m_RotateFingerId = -1;
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

    private void RotateCannon()
    {
        Puffle.ControlType smControlType = Puffle.smControlType;
        if (smControlType != Puffle.ControlType.eTouchScreen && smControlType != Puffle.ControlType.eTilting)
            return;

        Vector3 targetPosition;
        bool isMouse = false;

        if (mInputController.TouchCount > 0)
        {
            if (m_RotateFingerId != mInputController.FirstFingerId)
                return;
            targetPosition = mCamera.ScreenToWorldPoint(mInputController.TouchPosition1);
        }
        else
        {
            targetPosition = mCamera.ScreenToWorldPoint(Input.mousePosition);
            isMouse = true;
        }

        targetPosition.z = mCannonTransform.position.z;
        Vector3 direction = targetPosition - mCannonTransform.position;

        if (direction.magnitude < m_MinDistanceFromCannonForRotation)
        {
            if ((isMouse && !Input.GetMouseButton(0)) || (!isMouse && mInputController.TouchCount == 0))
                mCannonState = CannonState.eIdle;
            return;
        }

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Vector3 currentEuler = mCannonTransform.eulerAngles;
        float angle = Mathf.LerpAngle(currentEuler.z, targetAngle, rotationSpeed * Time.deltaTime);
        mCannonTransform.eulerAngles = new Vector3(currentEuler.x, currentEuler.y, angle);

        lastPointerPosition = targetPosition;
    }
}