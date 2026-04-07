using System;
using UnityEngine;

public class Button3DPressStateController : MonoBehaviour
{
	private enum TouchState
	{
		eTouchState_Touching = 0,
		eTouchState_Released = 1,
		eTouchState_COUNT = 2
	}

	private enum TextureState
	{
		eTextureState_Default = 0,
		eTextureState_Pressed = 1
	}

	public string defaultState;

	public string pressState;

	public Transform buttonTransform;

	public MeshRenderer buttonRenderer;

	public string renderCameraName;

	private InputController mInputController;

	private TextureState mTextureState;

	private TouchState mState;

	private bool mExitedButton;

	private bool mEnabled = true;

	private Camera mRenderCamera;

	public bool Enabled
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

	public event OnPressedHandler onPressed;

	public event OnReleasedHandler onReleased;

	private void Start()
	{
		mTextureState = TextureState.eTextureState_Default;
		mState = TouchState.eTouchState_Released;
		mInputController = GameFlowManager.Instance.InputController;
		mRenderCamera = Camera.main;
		Camera[] allCameras = Camera.allCameras;
		for (int i = 0; i < allCameras.Length; i++)
		{
			if (allCameras[i].name == renderCameraName)
			{
				mRenderCamera = allCameras[i];
			}
		}
	}

	private void Update()
	{
		if (!Enabled)
		{
			if (mTextureState != TextureState.eTextureState_Default)
			{
				LoadDefaultStateTexture();
			}
			return;
		}
		if (mInputController.TouchCount > 0)
		{
			if (mState == TouchState.eTouchState_Released)
			{
				Bounds bounds = buttonTransform.GetComponent<Renderer>().bounds;
				Vector3 center = bounds.center;
				center.z = 0f;
				bounds.center = center;
				Vector3 point = mRenderCamera.ScreenToWorldPoint(mInputController.TouchPosition1);
				point.z = 0f;
				if (bounds.Contains(point))
				{
					if (this.onPressed != null)
					{
						this.onPressed(this, EventArgs.Empty);
					}
					LoadPressStateTexture();
					mState = TouchState.eTouchState_Touching;
				}
				return;
			}
			Bounds bounds2 = buttonTransform.GetComponent<Renderer>().bounds;
			Vector3 center2 = bounds2.center;
			center2.z = 0f;
			bounds2.center = center2;
			Vector3 point2 = mRenderCamera.ScreenToWorldPoint(mInputController.TouchPosition1);
			point2.z = 0f;
			if (bounds2.Contains(point2))
			{
				if (mExitedButton)
				{
					LoadPressStateTexture();
					mExitedButton = false;
				}
			}
			else if (!mExitedButton)
			{
				LoadDefaultStateTexture();
				mExitedButton = true;
			}
			return;
		}
		if (mState == TouchState.eTouchState_Touching)
		{
			if (this.onReleased != null)
			{
				this.onReleased(this, EventArgs.Empty);
			}
			LoadDefaultStateTexture();
			mState = TouchState.eTouchState_Released;
		}
		mExitedButton = false;
	}

	private void LoadPressStateTexture()
	{
		mTextureState = TextureState.eTextureState_Pressed;
		buttonRenderer.material.mainTexture = GUIUtil.LoadTexture2D(pressState);
	}

	private void LoadDefaultStateTexture()
	{
		mTextureState = TextureState.eTextureState_Default;
		buttonRenderer.material.mainTexture = GUIUtil.LoadTexture2D(defaultState);
	}

	public void RegisterCallback()
	{
	}
}
