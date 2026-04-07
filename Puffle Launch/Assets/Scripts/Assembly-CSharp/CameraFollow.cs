using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
	public Rect deadZone;

	public float minimumSize;

	public float maximumSize;

	public float defaultSize;

	public float moveSpeed;

	public float zoomSpeed;

	public float zoomScaleFactor = 0.05f;

	private Transform mTransform;

	private Transform mTarget;

	private Camera mCamera;

	private float mOriginalOrthographicSize;

	private Vector3 mTargetPosition;

	private float mTargetSize;

	private bool mZoomOverride;

	private bool mEnabled = true;

	public float OriginalOrthographicSize
	{
		get
		{
			return mOriginalOrthographicSize;
		}
	}

	public Vector3 TargetPosition
	{
		get
		{
			return mTargetPosition;
		}
		set
		{
			mTargetPosition = value;
		}
	}

	public float TargetSize
	{
		get
		{
			return mTargetSize;
		}
		set
		{
			mTargetSize = value;
		}
	}

	public bool ZoomEnabled
	{
		get
		{
			return mEnabled;
		}
		set
		{
			mEnabled = value;
		}
	}

	public Transform Target
	{
		get
		{
			return mTarget;
		}
		set
		{
			mTarget = value;
		}
	}

	public bool ZoomOverride
	{
		get
		{
			return mZoomOverride;
		}
		set
		{
			mZoomOverride = value;
		}
	}

	public void Awake()
	{
		mTransform = base.transform;
		mCamera = GetComponent<Camera>();
		mOriginalOrthographicSize = mCamera.orthographicSize;
	}

	public void Start()
	{
		mTargetPosition = mTransform.position;
		mCamera.orthographicSize = (mTargetSize = defaultSize);
		mZoomOverride = false;
	}

	public void Update()
	{
		if (!mZoomOverride)
		{
			HandlePinchZoom();
		}
	}

	public void LateUpdate()
	{
	}

	public void FixedUpdate()
	{
		UpdateZoom(TimeManager.Instance.DeltaTime);
		if (mTarget != null)
		{
			Vector3 vector = mTarget.position - mTargetPosition;
			float num = Mathf.Abs(vector.x) - deadZone.width * 0.5f;
			float num2 = Mathf.Abs(vector.y) - deadZone.height * 0.5f;
			if (num > 0f)
			{
				mTargetPosition.x += ((!(vector.x > 0f)) ? (0f - num) : num);
			}
			if (num2 > 0f)
			{
				mTargetPosition.y += ((!(vector.y > 0f)) ? (0f - num2) : num2);
			}
			UpdateTransform(TimeManager.Instance.DeltaTime);
		}
	}

	public void UpdateTransform(float aDeltaTime)
	{
		Vector3 vector = mTargetPosition - mTransform.position;
		float magnitude = (vector * moveSpeed * aDeltaTime).magnitude;
		float magnitude2 = vector.magnitude;
		float num = Mathf.Min(magnitude2, magnitude);
		mTransform.position += vector.normalized * num;
	}

	private void UpdateZoom(float aDeltaTime)
	{
		float num = mTargetSize - mCamera.orthographicSize;
		mCamera.orthographicSize += num * zoomSpeed * aDeltaTime;
	}

	private void HandlePinchZoom()
	{
		if (ZoomEnabled && GameFlowManager.Instance.InputController.Zoom)
		{
			Debug.Log("zoom distance: " + GameFlowManager.Instance.InputController.ZoomDistance);
			mTargetSize = mCamera.orthographicSize - GameFlowManager.Instance.InputController.ZoomDistance * zoomScaleFactor;
			mTargetSize = Mathf.Clamp(mTargetSize, minimumSize, maximumSize);
			Debug.Log("ortho size: " + mCamera.orthographicSize);
		}
	}
}
