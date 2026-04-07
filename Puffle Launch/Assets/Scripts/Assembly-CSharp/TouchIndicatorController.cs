using UnityEngine;

public class TouchIndicatorController : MonoBehaviour
{
	private Camera mCamera;

	private MeshRenderer mRenderer;

	private Vector3 mTouchPosition;

	private Vector3 mFXPosition;

	private Transform mTransform;

	private Vector3 mBaseScale;

	private float mBaseOrthographicSize;

	private void Awake()
	{
		mRenderer = GetComponent<MeshRenderer>();
		mCamera = Camera.main;
	}

	private void Start()
	{
		mTransform = base.transform;
		mBaseScale = mTransform.localScale;
		mBaseOrthographicSize = mCamera.orthographicSize;
	}

	private void Update()
	{
		if (GameManager.Instance.IsPause() || GameManager.Instance.DuringCutscene || GameFlowManager.Instance.GUIManager.CurrentScene != GUIManager.Scene.eInGameHud)
		{
			mRenderer.enabled = false;
			return;
		}
		bool flag = Input.touchCount > 0;
		mRenderer.enabled = flag;
		if (flag)
		{
			mTouchPosition = Input.touches[0].position;
			mFXPosition = mCamera.ScreenToWorldPoint(mTouchPosition);
			mFXPosition.z = -1f;
			base.transform.position = mFXPosition;
		}
	}

	public void LateUpdate()
	{
		float num = mCamera.orthographicSize / mBaseOrthographicSize;
		mTransform.localScale = mBaseScale * num;
	}
}
