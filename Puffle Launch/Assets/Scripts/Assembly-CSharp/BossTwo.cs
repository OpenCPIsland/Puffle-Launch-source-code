using System;
using UnityEngine;

[RequireComponent(typeof(BossController))]
[RequireComponent(typeof(PathFollower))]
[RequireComponent(typeof(ElasticMovement))]
public class BossTwo : MonoBehaviour
{
	private Transform mTransform;

	private ElasticMovement mElasticMovement;

	private BossController mBossController;

	private PathFollower mPathFollower;

	private Transform mArm;

	private Transform mPlayer;

	private bool mIsChasing;

	private float mChaseTimer;

	private bool mIsAttacking;

	private int mAttackStartNode;

	private bool mResetOnLoop;

	private float mArmAngle;

	private float mArmAngularVelocity;

	private uint mFrameCount;

	private Vector3[] mPathBackup;

	private bool mInCutscene;

	private bool mRestoreSlowMo;

	public bool IsAttacking
	{
		get
		{
			return mIsAttacking;
		}
	}

	public void Start()
	{
		mTransform = base.transform;
		mElasticMovement = GetComponent<ElasticMovement>();
		mBossController = GetComponent<BossController>();
		mBossController.onRecovery += RecoveryEventHandler;
		mPathFollower = GetComponent<PathFollower>();
		mArm = base.transform.Find("Magnet");
		mPlayer = Puffle.Instance.transform;
		Puffle.Instance.puffleDeath += PlayerRespawnHandler;
		mIsChasing = false;
		mIsAttacking = true;
		mResetOnLoop = false;
		mArmAngle = 0f;
		mArmAngularVelocity = 0f;
		mFrameCount = 0u;
		mInCutscene = true;
		GameManager.Instance.DuringCutscene = true;
		Puffle.Instance.DisableInput = true;
		mPathBackup = new Vector3[mPathFollower.pathNodes.Length];
		mPathFollower.pathNodes.CopyTo(mPathBackup, 0);
		mPathFollower.pathNodes = new Vector3[3]
		{
			new Vector3(653f, 1540f),
			new Vector3(-665f, 1603f),
			new Vector3(-675.05f, 1325f)
		};
		mPathFollower.CurrentNode = 0;
		AudioManager.Instance.PlayMusic(AudioManager.MusicTrack.eMusic_Boss);
	}

	public void FixedUpdate()
	{
		mFrameCount++;
		if (mFrameCount == 72)
		{
			mRestoreSlowMo = TimeManager.Instance.SlowmoOverride;
			GameManager.Instance.StartCutscene(true);
		}
		else if (mFrameCount == 96)
		{
			AudioManager.Instance.PlayMusic(AudioManager.MusicTrack.eMusic_Boss);
		}
		else if (mFrameCount == 240)
		{
			Puffle.Instance.DisableInput = false;
			GameManager.Instance.EndCutscene();
			if (mRestoreSlowMo)
			{
				GameManager.Instance.ActivatePlayerSlowMo();
				GameFlowManager.Instance.GUIManager.HudManager.InGameHud.SetSlowmoButtonState(mRestoreSlowMo);
			}
		}
		else if (mFrameCount == 336)
		{
			mPathFollower.pathNodes = new Vector3[mPathBackup.Length];
			mPathBackup.CopyTo(mPathFollower.pathNodes, 0);
			mPathFollower.CurrentNode = 0;
			mElasticMovement.TargetPosition = mPathFollower.pathNodes[0] * ScaleItem.Instance.LevelScale;
			mIsAttacking = false;
			mInCutscene = false;
			GameManager.Instance.DuringCutscene = false;
		}
		if (mBossController.IsAlive)
		{
			float sqrMagnitude = (mTransform.position - mPlayer.position).sqrMagnitude;
			if (!mInCutscene)
			{
				AdjustSpeed(sqrMagnitude);
				if (mIsChasing)
				{
					mChaseTimer -= TimeManager.Instance.DeltaTime;
					if (mChaseTimer <= 0f)
					{
						mIsChasing = false;
					}
					ChasePlayer();
				}
			}
			if (mIsAttacking)
			{
				if (mResetOnLoop)
				{
					if (mPathFollower.CurrentNode == mAttackStartNode)
					{
						mIsAttacking = false;
						mResetOnLoop = false;
					}
				}
				else if (mPathFollower.CurrentNode != mAttackStartNode)
				{
					mResetOnLoop = true;
				}
			}
		}
		SwingArm();
	}

	public void RecoveryEventHandler(object sender, EventArgs e)
	{
		mIsChasing = true;
		mChaseTimer = 120f;
		mIsAttacking = true;
		mAttackStartNode = mPathFollower.CurrentNode;
	}

	public void PlayerRespawnHandler(object sender, EventArgs e)
	{
		mIsAttacking = true;
		mAttackStartNode = mPathFollower.CurrentNode;
	}

	private void AdjustSpeed(float aDistance)
	{
		if (aDistance > Mathf.Pow(1500f * ScaleItem.Instance.LevelScale, 2f))
		{
			mElasticMovement.elasticMultiplier = 0.02f;
		}
		else if (aDistance > Mathf.Pow(1000f * ScaleItem.Instance.LevelScale, 2f))
		{
			mElasticMovement.elasticMultiplier = 0.01f;
		}
		else if (aDistance > Mathf.Pow(600f * ScaleItem.Instance.LevelScale, 2f))
		{
			mElasticMovement.elasticMultiplier = 0.006f;
		}
		else
		{
			mElasticMovement.elasticMultiplier = 0.003f;
		}
	}

	private void ChasePlayer()
	{
		int num = mPathFollower.CurrentNode + 1;
		if (num == mPathFollower.pathNodes.Length)
		{
			num = 0;
		}
		float sqrMagnitude = (mPlayer.position - mPathFollower.pathNodes[num]).sqrMagnitude;
		int num2 = mPathFollower.CurrentNode - 1;
		if (num2 == -1)
		{
			num2 = mPathFollower.pathNodes.Length - 1;
		}
		float sqrMagnitude2 = (mPlayer.position - mPathFollower.pathNodes[num2]).sqrMagnitude;
		mPathFollower.reversed = sqrMagnitude2 > sqrMagnitude;
	}

	private void SwingArm()
	{
		float num = ((!mInCutscene) ? TimeManager.Instance.DeltaTime : 1f);
		float num2 = (0f - mElasticMovement.Velocity.x) / ScaleItem.Instance.LevelScale;
		if (mIsAttacking)
		{
			mArmAngularVelocity += 4f * Mathf.Sign(num2) * num;
		}
		mArmAngularVelocity += num2 * 0.1f * num;
		if (mArmAngle != 0f && !mIsAttacking)
		{
			mArmAngularVelocity -= mArmAngle * 0.1f * num;
		}
		mArmAngularVelocity *= 0.9f * num;
		mArmAngle += mArmAngularVelocity * num;
		if (mArmAngle > 360f)
		{
			mArmAngle -= 360f;
			if (!AudioManager.Instance.Muted)
			{
				mArm.GetComponent<AudioSource>().Play();
			}
		}
		else if (mArmAngle < -360f)
		{
			mArmAngle += 360f;
			if (!AudioManager.Instance.Muted)
			{
				mArm.GetComponent<AudioSource>().Play();
			}
		}
		mArm.localEulerAngles = new Vector3(0f, 0f, mArmAngle);
	}
}
