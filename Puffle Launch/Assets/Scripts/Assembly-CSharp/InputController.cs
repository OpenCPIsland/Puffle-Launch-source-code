using UnityEngine;

public class InputController : MonoBehaviour
{
    public enum SwipeAxis
    {
        eUp_Down = 0,
        eLeft_Right = 1,
        eNone = 2,
        eSlide_COUNT = 3
    }

    public enum TiltAxis
    {
        eTiltUp = 0,
        eTiltLeft = 1,
        eTiltDown = 2,
        eTiltRight = 3,
        eTilt_COUNT = 4
    }

    public enum ZoomAxis
    {
        eZoomIn = 0,
        eZoomOut = 1,
        eNone = 2,
        eZoom_COUNT = 3
    }

    private enum Slide
    {
        eUp = 0,
        eLeft = 1,
        eDown = 2,
        eRight = 3,
        eSlide_Count = 4
    }

    private enum Orientation
    {
        ePortrait = 0,
        eLandscapeLeft = 1,
        ePortraitUpsideDown = 2,
        eLandscapeRight = 3,
        eOrientation_COUNT = 4
    }

    private const float mSwipeDetectTime = 0.03f;
    private const float mReturnSwipeDetectTime = 0.45f;
    private const float mNoMoveHoldDetectionTime = 0.3f;
    private const float mSlideDetectionDistance = 60f;
    private const float mMoveDetectionDistance = 30f;

    public float mTiltDeadzone = 0.05f;

    private float mHoldDetectTime = 0.1f;
    private float mLongHoldDetectTime = 0.3f;
    private float mTapDetectTime = 0.03f;
    private float mDoubleTapDetectTime = 0.3f;

    private float mTapTimer;
    private float mDoubleTapTimer;
    private float mSwipeTimer;
    private float mReturnSwipeTimer;
    private float mNoMoveHoldTimer;

    private Vector3 mTouchPosition1 = Vector3.zero;
    private Vector3 mPreviousTouchPosition1 = default(Vector3);
    private Vector3 mMoveStartPos1 = Vector3.zero;
    private bool m_Finger1Moved;
    private bool mb_skipNextUpdate;

    private Vector3 mTouchPosition2 = Vector3.zero;
    private Vector3 mPreviousTouchPosition2 = default(Vector3);
    private Vector3 mMoveStartPos2 = Vector3.zero;
    private bool m_Finger2Moved;

    private Vector3 mTapPosition = Vector3.zero;
    private Vector3 mReleasePosition = Vector3.zero;
    private Vector3 mStartTouchPos = default(Vector3);

    private Vector2 mSlideDirection = default(Vector2);
    private Vector2[] mVectTable = new Vector2[4];

    private bool mTouch;
    private bool mTouchDown;
    private bool mTouchWasDown;

    private int mTouchCount;
    private int mPreviousTouchCount;

    private int mTapCount;

    private bool mHold;
    private bool mLongHold;
    private bool mSwipe;
    private bool mNoReturnSwipe;
    private bool mReturnSwipe;
    private bool mRelease;
    private bool mSingleTap;
    private bool mDoubleTap;
    private bool mDetectingFirstTap;

    private Slide mSwipeDirection = Slide.eSlide_Count;
    private Slide mPreviousSwipeDirection = Slide.eSlide_Count;

    private SwipeAxis mSwipeAxis = SwipeAxis.eNone;

    private bool mIsFirstZoom = true;
    private bool mZoom;
    private ZoomAxis mZoomDirection = ZoomAxis.eNone;
    private float mZoomDistance;

    private Vector2 mAccelerometerDirection = default(Vector2);
    private float mAccelerometerDeadZone = 0.075f;

    private bool mShake;
    private Vector3 mShakeDirection = default(Vector3);

    private bool mTilt;
    private TiltAxis mTiltDirection = TiltAxis.eTilt_COUNT;
    private float mTiltAngle;

    private Orientation mDeviceOrientation = Orientation.eOrientation_COUNT;

    public Vector3 StartTouchPos => mStartTouchPos;
    public Vector3 TouchPosition1 => mTouchPosition1;
    public bool HasFinger1Moved => m_Finger1Moved;
    public bool HasFinger2Moved => m_Finger2Moved;
    public Vector3 TouchPosition2 => mTouchPosition2;
    public int TouchCount => mTouchCount;
    public int PreviousTouchCount => mPreviousTouchCount;
    public bool TouchDown => mTouchDown;
    public Vector3 TapPosition => mTapPosition;
    public int FirstFingerId => (Input.touchCount <= 0) ? (-1) : Input.touches[0].fingerId;
    public int SecondFingerId => (Input.touchCount <= 1) ? (-1) : Input.touches[1].fingerId;
    public Vector3 ReleasePosition => mReleasePosition;
    public bool SingleTap => mSingleTap;
    public bool DoubleTap => mDoubleTap;
    public bool DetectingFirstTap => mDetectingFirstTap;
    public bool Held => mHold;
    public bool LongHold => mLongHold;
    public bool Release => mRelease;
    public bool Swipe => mSwipe;
    public Vector2 SlideDirection => mSlideDirection;
    public bool ReturnSwipe => mReturnSwipe;
    public SwipeAxis ReturnSwipeAxis => mSwipeAxis;
    public Vector2 AccelerometerDirection => mAccelerometerDirection;
    public float AccelerometerDeadZone => mAccelerometerDeadZone;
    public bool Zoom => mZoom;
    public ZoomAxis ZoomDirection => mZoomDirection;
    public float ZoomDistance => mZoomDistance;
    public bool Tilt => mTilt;
    public TiltAxis TiltDirection => mTiltDirection;
    public float TiltAngle => mTiltAngle;
    public bool Shake => mShake;
    public Vector3 ShakeDirection => mShakeDirection;

    private void Start()
    {
        mAccelerometerDirection.x = (mAccelerometerDirection.y = 0f);
    }

    public void Reset()
    {
        mTouchPosition1 = Vector3.zero;
        mTouch = false;
        mTouchDown = false;
        mTouchCount = 0;
        mHold = false;
        mSingleTap = false;
        mDoubleTap = false;
        mSwipe = false;
        mRelease = false;
        mTapTimer = 0f;
        m_Finger1Moved = false;
        mMoveStartPos1 = Vector3.zero;
        mTouchWasDown = false;
        mPreviousTouchCount = 0;
        mb_skipNextUpdate = true;
    }

    private void Update()
    {
        mTouchPosition1 = Vector3.zero;
        mTouch = false;
        mTouchDown = false;
        mTouchCount = 0;
        mHold = false;
        mLongHold = false;
        mSingleTap = false;
        mDoubleTap = false;
        mSwipe = false;
        mRelease = false;

        if (GameManager.Instance.IsPause())
        {
            Reset();
            return;
        }

        if (mb_skipNextUpdate)
        {
            mb_skipNextUpdate = false;
            return;
        }

        if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer ||
            Application.platform == RuntimePlatform.LinuxPlayer ||
            Application.platform == RuntimePlatform.OSXPlayer ||
            Application.platform == RuntimePlatform.WebGLPlayer)
        {
            if (Input.GetMouseButton(0))
            {
                mTouchCount = 1;
                mTouch = true;
                mTouchPosition1 = Input.mousePosition;
                if (!mTouchWasDown)
                    mTouchDown = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                mRelease = true;
                mReleasePosition = Input.mousePosition;
            }

            Vector3 mouseDelta = Input.mousePosition - mPreviousTouchPosition1;
            if (mouseDelta.magnitude >= mMoveDetectionDistance)
            {
                m_Finger1Moved = true;
            }

            Vector2 keyDir = Vector2.zero;
            if (Input.GetKey(KeyCode.UpArrow)) keyDir.y += 1;
            if (Input.GetKey(KeyCode.DownArrow)) keyDir.y -= 1;
            if (Input.GetKey(KeyCode.LeftArrow)) keyDir.x -= 1;
            if (Input.GetKey(KeyCode.RightArrow)) keyDir.x += 1;

            if (Input.GetKey(KeyCode.Space))
            {
                mTouch = true;
                mTouchDown = !mTouchWasDown;
                mTouchCount = 1;
                mTouchPosition1 += new Vector3(0f, 0f, 0f);
            }

            if (keyDir != Vector2.zero)
            {
                mTouch = true;
                mTouchCount = 1;
                mTouchPosition1 += new Vector3(keyDir.x, keyDir.y, 0f) * 5f;
                m_Finger1Moved = true;
            }
        }
        else
        {
            mZoom = false;
            mTouchPosition2 = Vector3.zero;
            mTilt = false;
            mTiltDirection = TiltAxis.eTilt_COUNT;
            mShake = false;
            mTouchCount = Input.touchCount;

            if (mTouchCount > 0)
            {
                mTouch = true;
                Vector2 position = Input.touches[0].position;
                mTouchPosition1.x = position.x;
                mTouchPosition1.y = position.y;
                mTouchPosition1.z = 0f;

                if (mTouchCount > 1)
                {
                    position = Input.touches[1].position;
                    mTouchPosition2.x = position.x;
                    mTouchPosition2.y = position.y;
                    mTouchPosition2.z = 0f;
                }

                if (!mTouchWasDown)
                    mTouchDown = true;
            }

            mAccelerometerDirection.x = Input.acceleration.x;
            mAccelerometerDirection.y = Input.acceleration.y;
            mAccelerometerDirection = mAccelerometerDirection.normalized;

            switch (Input.deviceOrientation)
            {
                case DeviceOrientation.LandscapeLeft:
                    mDeviceOrientation = Orientation.eLandscapeLeft;
                    break;
                case DeviceOrientation.LandscapeRight:
                    mDeviceOrientation = Orientation.eLandscapeRight;
                    break;
            }
        }

        if (!mTouch && mTouchWasDown)
        {
            mRelease = true;
            mReleasePosition = mPreviousTouchPosition1;
        }

        TapGesture();
        MoveGesture();
        SwipeGesture();
        ReturnSwipeGesture();
        ZoomGesture();
        TiltGesture();
        ShakeGesture();

        mTouchWasDown = mTouch;
        mPreviousTouchPosition1 = mTouchPosition1;
        if (!Application.isEditor &&
            Application.platform != RuntimePlatform.WindowsPlayer &&
            Application.platform != RuntimePlatform.LinuxPlayer &&
            Application.platform != RuntimePlatform.OSXPlayer &&
            Application.platform != RuntimePlatform.WebGLPlayer)
        {
            mPreviousTouchPosition2 = mTouchPosition2;
        }

        mPreviousSwipeDirection = mSwipeDirection;
        mPreviousTouchCount = mTouchCount;
    }

    private void TapGesture()
    {
        float deltaTime = Time.deltaTime;
        mTapDetectTime = 0.03f;
        if (deltaTime > mTapDetectTime)
        {
            mTapDetectTime = deltaTime;
        }
        if (mTapTimer >= mHoldDetectTime)
        {
            mHold = true;
        }
        if (mTapTimer >= mLongHoldDetectTime)
        {
            mLongHold = true;
        }
        if (mDetectingFirstTap && mTapTimer >= mTapDetectTime)
        {
            mDetectingFirstTap = false;
        }
        if (mTouch)
        {
            if (mTouchDown)
            {
                mTapPosition = mTouchPosition1;
                mTapTimer = 0f;
                if (mTapCount == 0)
                {
                    mDoubleTapTimer = 0f;
                    mDetectingFirstTap = true;
                }
            }
            else
            {
                mTapTimer += Time.deltaTime;
                mDoubleTapTimer += Time.deltaTime;
            }
            return;
        }
        mDoubleTapTimer += Time.deltaTime;
        if (mTapTimer >= mTapDetectTime && mTouchWasDown && !mHold)
        {
            mTapCount++;
        }
        if (mDoubleTapTimer >= mDoubleTapDetectTime && !mHold)
        {
            if (mTapCount == 1)
            {
                mSingleTap = true;
                mTapCount = 0;
                mDoubleTapTimer = 0f;
            }
            else if (mTapCount == 2)
            {
                mDoubleTap = true;
                mTapCount = 0;
                mDoubleTapTimer = 0f;
            }
            else
            {
                mTapCount = 0;
                mDoubleTapTimer = 0f;
            }
        }
    }

    private void MoveGesture()
    {
        if (mTouch)
        {
            if (mTouchDown)
            {
                mMoveStartPos1 = mTouchPosition1;
                if (mTouchCount > 1)
                {
                    mMoveStartPos2 = mTouchPosition2;
                }
            }
            else
            {
                m_Finger1Moved = (mTouchPosition1 - mMoveStartPos1).magnitude >= 30f;
                if (mTouchCount > 1)
                {
                    m_Finger2Moved = (mTouchPosition2 - mMoveStartPos2).magnitude >= 30f;
                }
                else
                {
                    m_Finger2Moved = false;
                }
            }
        }
        else
        {
            m_Finger1Moved = false;
            mMoveStartPos1 = Vector3.zero;
            m_Finger2Moved = false;
            mMoveStartPos2 = Vector3.zero;
        }
    }

    private void SwipeGesture()
    {
        if (mTouch)
        {
            if (mTouchDown)
            {
                mSwipeTimer = 0f;
                mStartTouchPos = mTouchPosition1;
                mNoReturnSwipe = true;
            }
            else
            {
                mSwipeTimer += Time.deltaTime;
            }
        }
        else if (mSwipeTimer >= 0.03f && mTouchWasDown)
        {
            Vector2 vector = mPreviousTouchPosition1 - mStartTouchPos;
            float magnitude = vector.magnitude;
            if (magnitude >= 60f && mNoReturnSwipe)
            {
                mSwipe = true;
                mSlideDirection = vector.normalized;
            }
        }
    }

    private void ReturnSwipeGesture()
    {
        if (mTouch)
        {
            if (mTouchDown)
            {
                return;
            }
            mReturnSwipeTimer += Time.deltaTime;
            Vector2 vector = mTouchPosition1 - mPreviousTouchPosition1;
            float magnitude = vector.magnitude;
            vector /= magnitude;
            float num = 360f;
            float num2 = 0f;
            if (magnitude == 0f)
            {
                mNoMoveHoldTimer += Time.deltaTime;
                return;
            }
            mNoMoveHoldTimer = 0f;
            if (mNoMoveHoldTimer <= 0.3f)
            {
                for (int i = 0; i < 4; i++)
                {
                    num2 = Vector2.Angle(vector, mVectTable[i]);
                    if (num2 < num)
                    {
                        num = num2;
                        mSwipeDirection = (Slide)i;
                    }
                }
                if (mSwipeDirection == Slide.eRight)
                {
                    if (mPreviousSwipeDirection == Slide.eLeft && mReturnSwipeTimer >= 0.45f)
                    {
                        mSwipeAxis = SwipeAxis.eLeft_Right;
                        mReturnSwipe = true;
                        mNoReturnSwipe = false;
                    }
                }
                else if (mSwipeDirection == Slide.eLeft)
                {
                    if (mPreviousSwipeDirection == Slide.eRight && mReturnSwipeTimer >= 0.45f)
                    {
                        mSwipeAxis = SwipeAxis.eLeft_Right;
                        mReturnSwipe = true;
                        mNoReturnSwipe = false;
                    }
                }
                else if (mSwipeDirection == Slide.eUp)
                {
                    if (mPreviousSwipeDirection == Slide.eDown && mReturnSwipeTimer >= 0.45f)
                    {
                        mSwipeAxis = SwipeAxis.eUp_Down;
                        mReturnSwipe = true;
                        mNoReturnSwipe = false;
                    }
                }
                else if (mSwipeDirection == Slide.eDown && mPreviousSwipeDirection == Slide.eUp && mReturnSwipeTimer >= 0.45f)
                {
                    mSwipeAxis = SwipeAxis.eUp_Down;
                    mReturnSwipe = true;
                    mNoReturnSwipe = false;
                }
            }
            else
            {
                mReturnSwipe = false;
            }
        }
        else
        {
            mReturnSwipeTimer = 0f;
            mReturnSwipe = false;
        }
    }

    private void ZoomGesture()
    {
        if (Input.touchCount > 1)
        {
            Vector2 vector = mPreviousTouchPosition1 - mPreviousTouchPosition2;
            float magnitude = ((Vector2)(mTouchPosition1 - mTouchPosition2)).magnitude;
            float magnitude2 = vector.magnitude;
            if (mIsFirstZoom)
            {
                mIsFirstZoom = false;
                mZoomDistance = 0f;
            }
            else
            {
                mZoomDistance = magnitude - magnitude2;
            }
            if (magnitude > magnitude2)
            {
                mZoom = true;
                mZoomDirection = ZoomAxis.eZoomIn;
            }
            else if (magnitude < magnitude2)
            {
                mZoom = true;
                mZoomDirection = ZoomAxis.eZoomOut;
            }
            else
            {
                mIsFirstZoom = true;
                mZoom = false;
                mZoomDirection = ZoomAxis.eNone;
            }
        }
        else
        {
            mIsFirstZoom = true;
        }
    }

    private void TiltGesture()
    {
        if (Mathf.Abs(mAccelerometerDirection.y) >= mTiltDeadzone)
        {
            mTilt = true;
            mTiltDirection = ((!(mAccelerometerDirection.y > 0f)) ? ((mDeviceOrientation != Orientation.eLandscapeLeft) ? TiltAxis.eTiltLeft : TiltAxis.eTiltRight) : ((mDeviceOrientation != Orientation.eLandscapeLeft) ? TiltAxis.eTiltRight : TiltAxis.eTiltLeft));
            mTiltAngle = mAccelerometerDirection.y;
        }
        if (Mathf.Abs(mAccelerometerDirection.x) >= mTiltDeadzone)
        {
            mTilt = true;
            mTiltDirection = ((!(mAccelerometerDirection.x > 0f)) ? ((mDeviceOrientation != Orientation.eLandscapeLeft) ? TiltAxis.eTiltDown : TiltAxis.eTiltUp) : ((mDeviceOrientation != Orientation.eLandscapeLeft) ? TiltAxis.eTiltUp : TiltAxis.eTiltDown));
            mTiltAngle = mAccelerometerDirection.x;
        }
    }

    private void ShakeGesture()
    {
        Vector3 acceleration = Input.acceleration;
        if (acceleration.sqrMagnitude > 3f)
        {
            mShake = true;
            mShakeDirection = acceleration.normalized;
        }
        else
        {
            mShake = false;
        }
    }
}