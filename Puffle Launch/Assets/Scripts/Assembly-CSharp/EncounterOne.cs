using UnityEngine;

public class EncounterOne : MonoBehaviour
{
	public Transform fakeGiantPuffleO;

	public AudioClip extendClawSound;

	public AudioClip retractClawSound;

	private bool mInCutscene;

	private bool mCutscenePlayed;

	private uint mFrameCount;

	private TweeningController mTweeningController;

	private TweeningController mClawTweeningController;

	private SpriteManager mClawSpriteManager;

	private TweeningController mGiantPuffleOTC;

	private PuffleContainer mPuffleContainer;

	private CrabbyAnimController mCrabbyAnimController;

	private Puffle mPlayer;

	private Transform mGiantPuffleO;

	private Transform mClaw;

	private bool mRestoreSlowMo;

	private AudioSource mAudioSource;

	private void Start()
	{
		mCutscenePlayed = false;
		mFrameCount = 0u;
		mTweeningController = GetComponent<TweeningController>();
		mAudioSource = GetComponent<AudioSource>();
		mPlayer = Puffle.Instance;
		mPlayer.GetComponent<Puffle>().DisableInput = true;
		mInCutscene = false;
		GameManager.Instance.DuringCutscene = false;
		Cannon[] array = (Cannon[])Object.FindObjectsOfType(typeof(Cannon));
		mPuffleContainer = array[22].GetComponent<PuffleContainer>();
		base.transform.Find("Ship").GetComponent<Renderer>().enabled = false;
		base.transform.Find("Ship").Find("Crabby").GetComponent<Renderer>().enabled = false;
		mClaw = base.transform.Find("Ship").Find("Claw");
		mClaw.GetComponent<Renderer>().enabled = false;
		mClawTweeningController = mClaw.GetComponent<TweeningController>();
		mClawSpriteManager = mClaw.GetComponent<SpriteManager>();
		mCrabbyAnimController = base.transform.Find("Ship").Find("Crabby").GetComponent<CrabbyAnimController>();
		mGiantPuffleO = (Transform)Object.Instantiate(fakeGiantPuffleO);
		mGiantPuffleO.position = base.transform.position;
		mGiantPuffleOTC = mGiantPuffleO.GetComponent<TweeningController>();
		mGiantPuffleO.localScale *= ScaleItem.Instance.BillboardScale;
		AudioManager.Instance.PlayMusic(AudioManager.MusicTrack.eMusic_Boss);
		base.transform.position += new Vector3(0.51038f, 0.10208f, -0.14f);
	}

	private void Update()
	{
		mAudioSource.mute = AudioManager.Instance.Muted;
	}

	private void FixedUpdate()
	{
		if (mPlayer.State == Puffle.PuffleState.eInCannon && mPuffleContainer.IsPuffleInside() && !mCutscenePlayed)
		{
			mCutscenePlayed = true;
			mInCutscene = true;
			mTweeningController.Play(true);
			mClawTweeningController.Play(true);
		}
		else
		{
			if (!mInCutscene)
			{
				return;
			}
			mFrameCount++;
			if (mFrameCount == 1)
			{
				GameManager.Instance.DuringCutscene = true;
				GameFlowManager.Instance.GUIManager.HudManager.InGameHud.SetSlowMoButtonEnable(false);
				base.transform.Find("Ship").GetComponent<Renderer>().enabled = true;
				base.transform.Find("Ship").Find("Crabby").GetComponent<Renderer>().enabled = true;
				mClaw.GetComponent<Renderer>().enabled = true;
				mRestoreSlowMo = TimeManager.Instance.SlowmoOverride;
				GameManager.Instance.StartCutscene(false);
				Camera.main.GetComponentInChildren<VisualEffects>().ForceSlowMoFX();
				mAudioSource.PlayOneShot(extendClawSound);
			}
			else if (mFrameCount == 60)
			{
				mCrabbyAnimController.Play(CrabbyAnimController.CrabbyAnim.eLaugh);
			}
			else if (mFrameCount == 70)
			{
				mCrabbyAnimController.Play(CrabbyAnimController.CrabbyAnim.eLaugh);
			}
			else if (mFrameCount == 80)
			{
				mCrabbyAnimController.Play(CrabbyAnimController.CrabbyAnim.eLaugh);
			}
			else if (mFrameCount == 101)
			{
				mGiantPuffleO.transform.parent = mClaw.transform;
				mClawSpriteManager.Seek(1);
			}
			else if (mFrameCount == 120)
			{
				mAudioSource.PlayOneShot(retractClawSound);
			}
			else if (mFrameCount == 140)
			{
				mCrabbyAnimController.Play(CrabbyAnimController.CrabbyAnim.eLaugh);
				mClaw.GetComponent<Renderer>().enabled = false;
				mGiantPuffleOTC.Play(true);
			}
			else if (mFrameCount == 147)
			{
				Object.Destroy(mClaw.gameObject);
				Object.Destroy(mGiantPuffleO.gameObject);
			}
			else if (mFrameCount == 150)
			{
				mCrabbyAnimController.Play(CrabbyAnimController.CrabbyAnim.eLaugh);
			}
			else if (mFrameCount == 160)
			{
				mCrabbyAnimController.Play(CrabbyAnimController.CrabbyAnim.eLaugh);
			}
			else if (mFrameCount == 170)
			{
				mCrabbyAnimController.Play(CrabbyAnimController.CrabbyAnim.eLaugh);
			}
			else if (mFrameCount == 180)
			{
				mCrabbyAnimController.Play(CrabbyAnimController.CrabbyAnim.eLaugh);
			}
			else if (mFrameCount == 190)
			{
				mCrabbyAnimController.Play(CrabbyAnimController.CrabbyAnim.eLaugh);
			}
			else if (mFrameCount == 200)
			{
				mCrabbyAnimController.Play(CrabbyAnimController.CrabbyAnim.eLaugh);
			}
			else if (mFrameCount == 213)
			{
				GameFlowManager.Instance.GUIManager.HudManager.InGameHud.SetSlowMoButtonEnable(true);
				GameManager.Instance.EndCutscene();
				if (mRestoreSlowMo)
				{
					GameManager.Instance.ActivatePlayerSlowMo();
					GameFlowManager.Instance.GUIManager.HudManager.InGameHud.SetSlowmoButtonState(mRestoreSlowMo);
				}
				else
				{
					Camera.main.GetComponentInChildren<VisualEffects>().ShowSlowMoFX(false);
				}
				mPlayer.GetComponent<Puffle>().DisableInput = false;
				Object.Destroy(base.gameObject);
				GameManager.Instance.DuringCutscene = false;
			}
		}
	}
}
