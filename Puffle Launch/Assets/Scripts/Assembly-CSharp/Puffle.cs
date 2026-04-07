using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Puffle : MonoBehaviour
{
    public enum ControlType
    {
        eTouchScreen = 0,
        eTilting = 1,
        eControlType_COUNT = 2
    }

    public enum PuffleState
    {
        eFlying = 0,
        eInCannon = 1,
        eInSlingshot = 2,
        eLaunching = 3,
        eRespawning = 4,
        ePuffleState_COUNT = 5
    }

    public const int kPlayerSpriteIndex = 1;

    public const int kPuffleCount = 10;

    private const float kBreakSpeed = 1f;

    private const float kOppositeHorizontalVelocityScalingRange = 1f;

    private const float kInitialBoostSpeed = 0.75f;

    private const float kInitialBoostHorizontalVelocityScalingRange = 0.25f;

    private const float kTurboModeSensitivityMultiplier = 0.85f;

    private const float mkShootControlTimeout = 0.1f;

    private const float mkTapMovementInterval = 0.2f;

    public float tiltTransitionSize = 1f;

    public Vector3 spawnPoint;

    public int respawnCount;

    public float groundPosition;

    public float ceilingPosition;

    public AudioClip waterFallSound;

    public AudioClip cloudFallSound;

    public ParticleSystem trail;

    public static ControlType smControlType;

    private static Puffle mInstance;

    private Transform mTransform;

    private Transform mTrailTransform;

    private SpriteManager mSpriteManager;

    private Vector3 mVelocity;

    private float mAngularVelocity;

    private bool mInvertGravity;

    private bool mStopMovement;

    private float mInitialTrailTime;

    private Collider mCurrentContainer;

    private Cannon mCurrentCannon;

    private InputController mInputController;

    private float mControlTimeout;

    private PuffleState mState;

    private float mTrailDelay;

    private Splash mSplashObject;

    private float mMovementChangeTimer;

    private float mLastMovement;

    private Vector3 mPrevPosition;

    private bool mDisableInput;

    public static Puffle Instance
    {
        get
        {
            return mInstance;
        }
    }

    public Vector3 Velocity
    {
        get
        {
            return mVelocity;
        }
        set
        {
            mVelocity = value;
        }
    }

    public Splash Splash
    {
        get
        {
            return mSplashObject;
        }
        set
        {
            mSplashObject = value;
        }
    }

    public float AngularVelocity
    {
        get
        {
            return mAngularVelocity;
        }
        set
        {
            mAngularVelocity = value;
        }
    }

    public bool InvertGravity
    {
        get
        {
            return mInvertGravity;
        }
        set
        {
            mInvertGravity = value;
        }
    }

    public bool StopMovement
    {
        get
        {
            return mStopMovement;
        }
        set
        {
            mStopMovement = value;
        }
    }

    public bool DisableInput
    {
        get
        {
            return mDisableInput;
        }
        set
        {
            mDisableInput = value;
        }
    }

    public PuffleState State
    {
        get
        {
            return mState;
        }
    }

    public event PuffleDeathEventHandler puffleDeath;

    public void Awake()
    {
        mInstance = this;
        if (GameFlowManager.Instance != null)
        {
            mInputController = GameFlowManager.Instance.InputController;
        }
    }

    public void Start()
    {
        mInvertGravity = false;
        mTransform = base.transform;
        EnsureTrail();
        mCurrentContainer = null;
        if (GameFlowManager.Instance != null)
        {
            mInputController = GameFlowManager.Instance.InputController;
        }
        smControlType = ControlType.eTouchScreen;
        mControlTimeout = 0f;
        mTrailDelay = 0f;
        mState = PuffleState.eFlying;
        mSpriteManager = GetComponent<SpriteManager>();
        mSpriteManager.Seek(1);
    }

    public void Update()
    {
        if (mInputController == null && GameFlowManager.Instance != null)
        {
            mInputController = GameFlowManager.Instance.InputController;
        }

        if (GameManager.Instance.IsPause())
        {
            return;
        }

        if (!mDisableInput && mState == PuffleState.eInCannon && GetLaunchPuffle())
        {
            mState = PuffleState.eLaunching;
        }

        mControlTimeout = Mathf.Max(mControlTimeout - Time.deltaTime, 0f);

        float num = mTrailDelay;
        if (num > 0f)
        {
            mTrailDelay -= Time.deltaTime;
            if (mTrailDelay <= 0f)
            {
                mTrailDelay = 0f;
                SetTrailEmission(true);
            }
        }
    }

    public void FixedUpdate()
    {
        if (GameManager.Instance.IsPause())
        {
            return;
        }
        float deltaTime = TimeManager.Instance.DeltaTime;
        if (LevelLoader.Instance == null)
        {
            return;
        }
        float levelScale = ScaleItem.Instance.LevelScale;
        bool flag = false;
        if (mState == PuffleState.eFlying)
        {
            if (mTransform.position.y < groundPosition || mTransform.position.y > ceilingPosition)
            {
                if (GameManager.Instance.CurrentWorld == GameManager.World.eWorld_BlueSky)
                {
                    AudioManager.Instance.PlayObstacleSound(waterFallSound);
                }
                else
                {
                    AudioManager.Instance.PlayObstacleSound(cloudFallSound);
                }
                mSplashObject.transform.position = mTransform.position;
                mSplashObject.Reset();
                if (mTransform.position.y > ceilingPosition)
                {
                    Vector3 localScale = mSplashObject.transform.localScale;
                    localScale.y *= -1f;
                    mSplashObject.transform.localScale = localScale;
                }
                mSplashObject.Puffle = this;
                mState = PuffleState.eRespawning;
                GetComponent<MeshRenderer>().enabled = false;
                TimeManager.Instance.StopSlowmo();
                return;
            }
            if (Mathf.Abs(mVelocity.x) < 5f * levelScale && Mathf.Abs(mVelocity.y) < 5f * levelScale)
            {
                mTrailDelay = 0f;
                SetTrailEmission(false);
            }
            mVelocity += new Vector3(0f, (float)(mInvertGravity ? 1 : (-1)) * deltaTime * levelScale, 0f);
            mVelocity *= 1f - 0.02f * deltaTime;
            mAngularVelocity *= 1f - 0.02f * deltaTime;
            float num = 0f;
            if (!mDisableInput)
            {
                num = GetPuffleMovement();
            }
            if (num != 0f)
            {
                if (num > 0f)
                {
                    if (mVelocity.x < 0f)
                    {
                        float value = 1f * Mathf.Abs(mVelocity.x / 1f);
                        num *= 0.75f + Mathf.Clamp(value, 0f, 1f);
                    }
                    else
                    {
                        float value2 = 0.75f * Mathf.Clamp(1f - Mathf.Abs(mVelocity.x / 0.25f), 0f, 1f);
                        num *= 1f + Mathf.Clamp(value2, 0f, 0.75f);
                    }
                }
                else if (mVelocity.x > 0f)
                {
                    float value3 = 1f * Mathf.Abs(mVelocity.x / 1f);
                    num *= 0.75f + Mathf.Clamp(value3, 0f, 1f);
                }
                else
                {
                    float value4 = 0.75f * Mathf.Clamp(1f - Mathf.Abs(mVelocity.x / 0.25f), 0f, 1f);
                    num *= 1f + Mathf.Clamp(value4, 0f, 0.75f);
                }
                if (GameManager.Instance.EnableTurboMode)
                {
                    num *= 0.85f;
                }
            }
            if (smControlType == ControlType.eTouchScreen && num != 0f && num != mLastMovement)
            {
                if (mMovementChangeTimer > 0f)
                {
                    mVelocity += new Vector3(num * 0.9f * deltaTime * levelScale / Mathf.Pow(TimeManager.Instance.TimeScale, 2f), 0f, 0f);
                }
                mMovementChangeTimer = 0.2f;
            }
            mLastMovement = num;
            mVelocity += new Vector3(num * 0.8f * deltaTime * levelScale / Mathf.Pow(TimeManager.Instance.TimeScale, 2f), 0f, 0f);
            if (num != 0f)
            {
                mAngularVelocity += 0.4f * deltaTime;
            }
            mMovementChangeTimer = Mathf.Max(mMovementChangeTimer - Time.deltaTime, 0f);
            flag = true;
        }
        else if (mState == PuffleState.eLaunching)
        {
            mCurrentCannon.LaunchPuffle();
            flag = true;
        }
        if (mStopMovement)
        {
            flag = false;
            mVelocity = Vector3.zero;
        }
        if (flag)
        {
            mPrevPosition = mTransform.position;
            mTransform.position += mVelocity * deltaTime;
            mTransform.eulerAngles += new Vector3(0f, 0f, mAngularVelocity * deltaTime);
            SetTrailWorldVelocity(-mVelocity * 2f * deltaTime);
            if (mTrailTransform != null)
            {
                mTrailTransform.position = mTransform.position - mVelocity * deltaTime * 3f + Vector3.forward * 0.01f;
            }
        }
    }

    public void OnTriggerEnter(Collider aOther)
    {
        if (mState != PuffleState.eFlying || !(aOther != mCurrentContainer))
        {
            return;
        }
        PuffleContainer component = aOther.GetComponent<PuffleContainer>();
        if ((bool)component)
        {
            mCurrentContainer = aOther;
            component.OnPuffleEnter(this);
            mVelocity = Vector3.zero;
            mAngularVelocity = 0f;
            mTransform.parent = aOther.transform;
            mTransform.localPosition = new Vector3(-0.08f, 0.13f, 0f);
            mTransform.localRotation = Quaternion.identity;
            mTrailDelay = 0f;
            SetTrailEmission(false);
            TimeManager.Instance.StopSlowmo();
            mSpriteManager.Seek(11);
            mCurrentCannon = mCurrentContainer.GetComponent<Cannon>();
            if ((bool)mCurrentCannon)
            {
                mCurrentCannon.OnCannonEnter();
                mState = PuffleState.eInCannon;
            }
            else
            {
                mState = PuffleState.eInSlingshot;
            }
        }
    }

    public void Launch(Vector3 aDirection, float aForce)
    {
        mTransform.localPosition = Vector3.zero;
        if (mCurrentCannon != null && !mCurrentCannon.autoLaunch)
        {
            mControlTimeout = 0.1f;
        }
        mState = PuffleState.eFlying;
        mTransform.parent = null;
        mCurrentContainer = null;
        Vector3 vector = aDirection * aForce;
        mVelocity = vector * 0.8f;
        mTransform.position += vector;
        mAngularVelocity = (Mathf.Abs(vector.x) + Mathf.Abs(vector.y)) / ScaleItem.Instance.LevelScale;
        mSpriteManager.Seek(1);
        SetTrailWorldVelocity(-mVelocity * 2f);
        if (mTrailTransform != null)
        {
            mTrailTransform.position = mTransform.position - mVelocity * 3f + Vector3.forward * 0.01f;
        }
        mTrailDelay = 0.1f;
    }

    public void Respawn()
    {
        if (this.puffleDeath != null)
        {
            this.puffleDeath(this, EventArgs.Empty);
        }
        mTransform.position = spawnPoint;
        mPrevPosition = mTransform.position;
        mVelocity = Vector3.zero;
        mAngularVelocity = 0f;
        mCurrentContainer = null;
        mCurrentCannon = null;
        mState = PuffleState.eFlying;
        mMovementChangeTimer = 0f;
        mLastMovement = 0f;
        mTrailDelay = 0f;
        SetTrailEmission(false);
        mInvertGravity = false;
        GetComponent<MeshRenderer>().enabled = true;
        respawnCount++;
    }

    private float GetPuffleMovement()
    {
        float value = 0f;
        if (mControlTimeout <= 0f && mInputController != null)
        {
            if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer)
            {
                value = Input.GetAxisRaw("Horizontal");
            }
            else
            {
                switch (smControlType)
                {
                    case ControlType.eTilting:
                        if (mInputController.Tilt)
                        {
                            float value2 = 0f;
                            if (mInputController.TiltDirection == InputController.TiltAxis.eTiltLeft)
                            {
                                value2 = (0f - mInputController.TiltAngle) / tiltTransitionSize;
                            }
                            else if (mInputController.TiltDirection == InputController.TiltAxis.eTiltRight)
                            {
                                value2 = mInputController.TiltAngle / tiltTransitionSize;
                            }
                            value2 = Mathf.Clamp(value2, -1f, 1f);
                            value = Mathf.Sign(value2) * (0f - Mathf.Cos(value2 * (float)Math.PI * 0.5f) + 1f);
                        }
                        break;
                    case ControlType.eTouchScreen:
                        if (mInputController.TouchCount > 0 && !mInputController.Zoom)
                        {
                            bool flag2 = false;
                            Vector2 vector = new Vector2(mInputController.TouchPosition1.x, (float)Screen.height - mInputController.TouchPosition1.y);
                            if (GameManager.HasCompletedTurboMode(GameManager.Instance.CurrentWorld) && (vector - GameFlowManager.Instance.GUIManager.HudManager.InGameHud.mv2_slowmotionButtonCenterPixelPosition).magnitude < GameFlowManager.Instance.GUIManager.HudManager.InGameHud.mv2_slowmotionButtonSizeRatio.x * (float)Screen.width)
                            {
                                flag2 = true;
                            }
                            if (!flag2)
                            {
                                value = ((!(mInputController.TouchPosition1.x > (float)Screen.width * 0.5f)) ? (-1f) : 1f);
                            }
                        }
                        break;
                }
            }
        }
        return Mathf.Clamp(value, -1f, 1f);
    }

    private bool GetLaunchPuffle()
    {
        if (!GameFlowManager.Instance.GUIManager.HudManager.InGameHud.mb_isInitialized)
        {
            return false;
        }
        if (GameFlowManager.Instance.GUIManager.HudManager.InGameHud.mo_pauseButton.ContainsTouch())
        {
            return false;
        }
        if (GameManager.HasCompletedTurboMode(GameManager.Instance.CurrentWorld) &&
            GameFlowManager.Instance.GUIManager.HudManager.InGameHud.mo_slowMoButton.ContainsTouch())
        {
            return false;
        }
        if (GUIUtility.hotControl != 0 || (!Application.isEditor && (mInputController == null || mInputController.Zoom)))
        {
            return false;
        }

        bool result = false;

        if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer)
        {
            if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                result = true;
            }
        }
        else
        {
            switch (smControlType)
            {
                case ControlType.eTouchScreen:
                case ControlType.eTilting:
                    if (mInputController != null && mInputController.TouchDown && mCurrentContainer.tag != "ControllableCannon")
                    {
                        result = true;
                    }
                    break;
            }
        }

        return result;
    }

    public Vector3 GetContactPoint(Collider aSender)
    {
        int num = 256;
        num = ~num;
        Vector3 direction = mTransform.position - mPrevPosition;
        bool isTrigger = aSender.isTrigger;
        aSender.isTrigger = false;
        float radius = GetComponent<SphereCollider>().radius;
        RaycastHit hitInfo;
        Physics.SphereCast(mPrevPosition - direction.normalized * radius, radius, direction, out hitInfo, direction.magnitude + radius, num);
        aSender.isTrigger = isTrigger;
        return hitInfo.point;
    }

    public static void SetControlType()
    {
        smControlType++;
        smControlType = (ControlType)((int)smControlType % 2);
    }

    private void EnsureTrail()
    {
        if (trail == null)
        {
            trail = GetComponentInChildren<ParticleSystem>(true);
        }
        if (trail != null)
        {
            mTrailTransform = trail.transform;
            ParticleSystem.MainModule main = trail.main;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
        }
    }

    private void SetTrailEmission(bool enabled)
    {
        EnsureTrail();
        if (trail == null)
        {
            return;
        }
        ParticleSystem.EmissionModule emission = trail.emission;
        emission.enabled = enabled;
        if (enabled)
        {
            trail.Play();
        }
        else
        {
            trail.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    private void SetTrailWorldVelocity(Vector3 velocity)
    {
        EnsureTrail();
        if (trail == null)
        {
            return;
        }
        ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = trail.velocityOverLifetime;
        velocityOverLifetime.enabled = true;
        velocityOverLifetime.space = ParticleSystemSimulationSpace.World;
        velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(velocity.x);
        velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(velocity.y);
        velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(velocity.z);
    }
}