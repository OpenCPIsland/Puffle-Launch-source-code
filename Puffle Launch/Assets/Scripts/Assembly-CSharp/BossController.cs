using System;
using UnityEngine;

public class BossController : MonoBehaviour
{
	private const int mkCollisionTimeoutFrames = 15;

	public bool spawnGiantPuffleO;

	public GameObject hitFX;

	public AudioClip impactSound;

	public AudioClip explosionSound;

	private Transform mTransform;

	private ElasticMovement mElasticMovement;

	private CrabbyAnimController mCrabbyAnimController;

	private TweeningController mTweeningController;

	private SpriteManager mSpriteManager;

	private AudioSource mAudioSource;

	private Vector3 mStartPosition;

	private int mHealth;

	private bool mIsAlive;

	private bool mIsCollidable;

	private float mHitCollisionTimeout;

	public bool IsAlive
	{
		get
		{
			return mIsAlive;
		}
	}

	public bool IsCollidable
	{
		get
		{
			return mIsCollidable;
		}
	}

	public event HitEventHandler onHit;

	public event RecoveryEventHandler onRecovery;

	public void Start()
	{
		mTransform = base.transform;
		mElasticMovement = GetComponent<ElasticMovement>();
		mCrabbyAnimController = GetComponentInChildren<CrabbyAnimController>();
		mCrabbyAnimController.animationEnd += CrabbyAnimEndEventHandler;
		mTweeningController = GetComponent<TweeningController>();
		mSpriteManager = GetComponent<SpriteManager>();
		mAudioSource = GetComponent<AudioSource>();
		mAudioSource.mute = AudioManager.Instance.Muted;
		mStartPosition = mTransform.position;
		mHealth = 8;
		mIsAlive = true;
		mIsCollidable = true;
	}

	public void Update()
	{
		mAudioSource.mute = AudioManager.Instance.Muted;
	}

	public void FixedUpdate()
	{
		if (!mIsAlive)
		{
			if (mCrabbyAnimController.CurrentAnim != CrabbyAnimController.CrabbyAnim.eFreefall)
			{
				mCrabbyAnimController.Play(CrabbyAnimController.CrabbyAnim.eFreefall);
				if (spawnGiantPuffleO)
				{
					SpawnGiantPuffleO();
				}
			}
			mElasticMovement.Velocity -= Vector3.up * 0.4f * ScaleItem.Instance.LevelScale;
			if (UnityEngine.Random.Range(0, 15) == 0)
			{
				Vector3 vector = new Vector3(UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(-3f, 3f), -1f);
				UnityEngine.Object.Instantiate(hitFX, mTransform.position + vector, default(Quaternion));
				if (!AudioManager.Instance.Muted)
				{
					mAudioSource.Play();
				}
			}
		}
		else if (mHitCollisionTimeout > 0f)
		{
			mHitCollisionTimeout = Mathf.Max(mHitCollisionTimeout - TimeManager.Instance.DeltaTime, 0f);
			if (mHitCollisionTimeout == 0f)
			{
				mIsCollidable = true;
			}
		}
	}

	public void OnTriggerEnter(Collider aOther)
	{
		if (!mIsCollidable || !(aOther.tag == "Player"))
		{
			return;
		}
		Puffle component = aOther.GetComponent<Puffle>();
		if (component.State == Puffle.PuffleState.eFlying)
		{
			Vector3 vector = mTransform.position - component.transform.position;
			float num = Mathf.Round(Mathf.Atan2(vector.y, vector.x) * 57.29578f);
			Vector3 vector2 = new Vector3(Mathf.Cos(num * ((float)Math.PI / 180f)), Mathf.Sin(num * ((float)Math.PI / 180f)), 0f);
			vector2 *= 20f * ScaleItem.Instance.LevelScale;
			mElasticMovement.Velocity = vector2 * 1.5f;
			ReboundPlayer(component, vector2);
			TakeDamage();
			TimeManager.Instance.StopSlowmo();
			if (this.onHit != null)
			{
				this.onHit(this, EventArgs.Empty);
			}
			if (impactSound != null)
			{
				AudioManager.Instance.PlayObstacleSound(impactSound);
			}
		}
	}

	public void OnPuffleOCollect()
	{
		if (!mCrabbyAnimController.IsAnimPlaying)
		{
			mCrabbyAnimController.Play(CrabbyAnimController.CrabbyAnim.eLaugh);
		}
	}

	public void CrabbyAnimEndEventHandler(object sender, CrabbyAnimEndEventArgs e)
	{
		if (e.anim != CrabbyAnimController.CrabbyAnim.eHit)
		{
			return;
		}
		if (mIsAlive)
		{
			if ((bool)mTweeningController)
			{
				mTweeningController.enabled = true;
			}
			if (this.onRecovery != null)
			{
				this.onRecovery(this, EventArgs.Empty);
			}
		}
		if (mSpriteManager.current.name == "Ship2")
		{
			mSpriteManager.current.framerate = 0;
			mSpriteManager.Seek(0);
		}
	}

	private void ReboundPlayer(Puffle aPuffle, Vector3 aPush)
	{
		Vector3 velocity = new Vector3(aPush.x * -0.2f, 0f - aPush.y, 0f);
		if (aPuffle.Velocity.y <= 0f)
		{
			velocity.y = (Mathf.Abs(aPush.y) + Mathf.Abs(aPush.x)) * 0.5f;
		}
		aPuffle.Velocity = velocity;
		aPuffle.AngularVelocity = (Mathf.Abs(aPush.x) + Mathf.Abs(aPush.y)) / ScaleItem.Instance.LevelScale;
	}

	private void TakeDamage()
	{
		if (mHealth <= 0)
		{
			return;
		}
		UnityEngine.Object.Instantiate(hitFX, mTransform.position - Vector3.forward, default(Quaternion));
		if ((bool)mTweeningController)
		{
			mTweeningController.enabled = false;
		}
		mTransform.eulerAngles = Vector3.zero;
		if (--mHealth == 0)
		{
			mIsAlive = false;
			mIsCollidable = false;
			mElasticMovement.elasticMultiplier = 0f;
			mAudioSource.Stop();
			mAudioSource.clip = explosionSound;
			mAudioSource.loop = false;
			Collider[] componentsInChildren = GetComponentsInChildren<Collider>();
			Collider[] array = componentsInChildren;
			foreach (Collider obj in array)
			{
				UnityEngine.Object.Destroy(obj);
			}
			AudioManager.Instance.PlayMusic(AudioManager.MusicTrack.eMusic_Win);
		}
		mCrabbyAnimController.Play(CrabbyAnimController.CrabbyAnim.eHit);
		if (mSpriteManager.current.name == "Ship2")
		{
			mSpriteManager.current.framerate = 24;
			mSpriteManager.Seek(1);
		}
		else if (!(mSpriteManager.current.name == "Ship3"))
		{
		}
	}

	private void SpawnGiantPuffleO()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Finish");
		if ((bool)gameObject)
		{
			gameObject.transform.position = mTransform.position;
			gameObject.GetComponent<ElasticMovement>().TargetPosition = mStartPosition;
		}
	}
}
