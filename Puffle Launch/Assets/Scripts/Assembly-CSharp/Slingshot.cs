using UnityEngine;

[RequireComponent(typeof(PuffleContainer))]
[RequireComponent(typeof(ElasticMovement))]
public class Slingshot : MonoBehaviour
{
	private const float kMinMovementX = 0.4f;

	private const float kMinMovementY = 0.2f;

	private const float kMinTimeToReplayStretchSound = 1f;

	public float launchForce = 1f;

	public float touchRadius = 1f;

	public float dragRatio = 1f;

	public Transform mLeftBalloonTransform;

	public Transform mRightBalloonTransform;

	public AudioClip ReleaseSound;

	public AudioClip StretchSound;

	private Transform mTransform;

	private PuffleContainer mThisContainer;

	private InputController mInputController;

	private Vector3 mInitialPosition;

	private float mElasticMultiplierDefault;

	private Vector3 mTouchDownPosition;

	private bool mInputActive;

	private ElasticMovement mElasticMovement;

	public void Start()
	{
		mTransform = base.transform;
		mThisContainer = GetComponent<PuffleContainer>();
		mInputController = GameFlowManager.Instance.InputController;
		mInputActive = false;
		mInitialPosition = mTransform.position;
		mElasticMovement = GetComponent<ElasticMovement>();
		float num = 960f / (float)Screen.width;
		Vector3 localPosition = mLeftBalloonTransform.localPosition;
		localPosition.x *= num;
		mLeftBalloonTransform.localPosition = localPosition;
		localPosition = mRightBalloonTransform.localPosition;
		localPosition.x *= num;
		mRightBalloonTransform.localPosition = localPosition;
	}

	public void OnTriggerEnter(Collider aOther)
	{
		if (aOther.tag == "Player")
		{
			Puffle component = aOther.GetComponent<Puffle>();
			if (component.State == Puffle.PuffleState.eFlying)
			{
				Vector3 normalized = component.Velocity.normalized;
				normalized *= 20f * ScaleItem.Instance.LevelScale;
				mElasticMovement.Velocity = normalized * 1.5f;
			}
		}
	}

	public void Update()
	{
		HandleControls();
	}

	private void HandleControls()
	{
		if (!mThisContainer.IsPuffleInside())
		{
			return;
		}
		if (mInputActive)
		{
			if (mInputController.Release)
			{
				mInputActive = false;
				Vector3 vector = mInitialPosition - mElasticMovement.TargetPosition;
				if (vector.y > 0.5f)
				{
					Vector3 aDirection = new Vector3(vector.x / 2.6f, vector.y, 0f);
					mThisContainer.GetContainedPuffle().Launch(aDirection, launchForce);
					mThisContainer.ReleasePuffle();
					AudioManager.Instance.PlayObstacleSound(ReleaseSound);
				}
				mElasticMovement.TargetPosition = mInitialPosition;
				return;
			}
			Vector3 vector2 = Camera.main.ScreenToWorldPoint(mInputController.TouchPosition1);
			vector2.z = mTransform.position.z;
			Vector3 vector3 = (vector2 - mTouchDownPosition) * dragRatio;
			if (vector3.x > 98f * ScaleItem.Instance.LevelScale)
			{
				vector3.x = 98f * ScaleItem.Instance.LevelScale;
			}
			else if (vector3.x < -98f * ScaleItem.Instance.LevelScale)
			{
				vector3.x = -98f * ScaleItem.Instance.LevelScale;
			}
			if (vector3.y < -166f * ScaleItem.Instance.LevelScale)
			{
				vector3.y = -166f * ScaleItem.Instance.LevelScale;
			}
			vector3.y = Mathf.Min(vector3.y, 0f);
			mElasticMovement.TargetPosition = mInitialPosition + vector3;
		}
		else if (mInputController.TouchDown)
		{
			Vector3 vector4 = Camera.main.ScreenToWorldPoint(mInputController.TouchPosition1);
			vector4.z = mTransform.position.z;
			if ((vector4 - mTransform.position).sqrMagnitude <= Mathf.Pow(touchRadius, 2f))
			{
				mInputActive = true;
				mTouchDownPosition = vector4;
			}
		}
	}
}
