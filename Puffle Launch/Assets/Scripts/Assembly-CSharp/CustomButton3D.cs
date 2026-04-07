using System;
using UnityEngine;

public class CustomButton3D : CustomGUI3DItem
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

	public bool mb_togglable;

	public bool mb_toggleState;

	public float mf_detectionZoneScale = 1f;

	private InputController mInputController;

	private TextureState mTextureState;

	private TouchState mState;

	private bool mEnabled = true;

	private bool mDisableTouch;

	private Camera mRenderCamera;

	private Bounds mo_buttonBounds;

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

	public event CustomOnSelectHandler customOnSelect;

	private void Start()
	{
		InitPosition();
		InitButtonBounds();
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
		}
		else if (mInputController.TouchCount > 0)
		{
			if (mState != TouchState.eTouchState_Released || !ContainsTouch() || !mInputController.TouchDown || mDisableTouch)
			{
				return;
			}
			if (mb_togglable)
			{
				mb_toggleState = !mb_toggleState;
				if (this.customOnSelect != null)
				{
					this.customOnSelect(this, EventArgs.Empty);
					if (mb_toggleState)
					{
						LoadPressStateTexture();
					}
					else
					{
						LoadDefaultStateTexture();
					}
				}
				mState = TouchState.eTouchState_Touching;
			}
			else
			{
				if (this.customOnSelect != null)
				{
					this.customOnSelect(this, EventArgs.Empty);
				}
				LoadPressStateTexture();
				mState = TouchState.eTouchState_Touching;
			}
		}
		else if (mState == TouchState.eTouchState_Touching)
		{
			if (!mb_togglable)
			{
				LoadDefaultStateTexture();
			}
			mState = TouchState.eTouchState_Released;
		}
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

	public void InitButtonBounds()
	{
		Bounds bounds = buttonTransform.GetComponent<Renderer>().bounds;
		Vector3 center = bounds.center;
		center.z = 0f;
		bounds.center = center;
		mo_buttonBounds = new Bounds(center, new Vector3(buttonTransform.GetComponent<Renderer>().bounds.size.x * mf_detectionZoneScale, buttonTransform.GetComponent<Renderer>().bounds.size.y * mf_detectionZoneScale, buttonTransform.GetComponent<Renderer>().bounds.size.z * mf_detectionZoneScale));
	}

	public void RegisterCallback()
	{
	}

	public bool ContainsTouch()
	{
		Vector3 point = mRenderCamera.ScreenToWorldPoint(mInputController.TouchPosition1);
		point.z = 0f;
		return mo_buttonBounds.Contains(point);
	}

	public bool ContainsTouchRelease()
	{
		Vector3 point = mRenderCamera.ScreenToWorldPoint(mInputController.ReleasePosition);
		point.z = 0f;
		return mo_buttonBounds.Contains(point) && mInputController.Release;
	}

	public void DisableTouch(bool aDisable)
	{
		mDisableTouch = aDisable;
	}
}
