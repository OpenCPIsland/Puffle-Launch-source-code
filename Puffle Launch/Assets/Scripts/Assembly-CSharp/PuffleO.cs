using UnityEngine;

public class PuffleO : MonoBehaviour
{
	public AudioClip mPuffleOCollectSound;

	private Transform mTransform;

	private ProgressBar mProgressBar;

	private Transform mMagnet;

	private Vector3 mMagnetOffset;

	private float mMagnetRadius;

	private Vector3 mStartPosition;

	private Vector3 mVelocity;

	private bool mSleeping;

	public void Start()
	{
		mTransform = base.transform;
		mProgressBar = GameObject.Find("ProgressBar").GetComponent<ProgressBar>();
		mMagnet = null;
		mStartPosition = base.transform.position;
		mSleeping = true;
	}

	public void FixedUpdate()
	{
		if (!mSleeping)
		{
			if ((bool)mMagnet)
			{
				Vector3 vector = mMagnet.position + mMagnetOffset - mTransform.position;
				float magnitude = vector.magnitude;
				mVelocity += 0.01f * (mMagnetRadius - magnitude) * vector / magnitude;
			}
			mVelocity *= 0.9f * TimeManager.Instance.DeltaTime;
			mTransform.position += mVelocity;
			if (mMagnet == null && mVelocity.sqrMagnitude < 0.0001f)
			{
				mVelocity = Vector3.zero;
				mSleeping = true;
			}
		}
	}

	public void OnTriggerEnter(Collider aOther)
	{
		if (aOther.tag == "Player")
		{
			Puffle component = aOther.GetComponent<Puffle>();
			if (component.State == Puffle.PuffleState.eFlying)
			{
				OnCollect();
				mProgressBar.CollectPuffleO();
				GameManager.smCurrentLevelRingCount++;
			}
		}
		else if (aOther.tag == "Magnet")
		{
			if ((bool)mMagnet)
			{
				mMagnet.root.GetComponent<BossController>().OnPuffleOCollect();
				OnCollect();
				return;
			}
			mMagnet = aOther.transform;
			mMagnetOffset = ((SphereCollider)aOther).center;
			mMagnetRadius = ((SphereCollider)aOther).radius;
			mSleeping = false;
		}
	}

	public void OnTriggerExit(Collider aOther)
	{
		if (aOther.tag == "Magnet")
		{
			mMagnet = null;
		}
	}

	public void EffectEndEventHandler(object sender, AnimationChangedEventArgs e)
	{
		Object.Destroy(((SpriteManager)sender).gameObject);
		GameObject gameObject = Object.Instantiate(Resources.Load("Prefabs/PuffleOBreadcrumb", typeof(Object)), mStartPosition, default(Quaternion)) as GameObject;
		gameObject.transform.localScale *= ScaleItem.Instance.BillboardScale;
		if (GameManager.Instance.CurrentWorld == GameManager.World.eWorld_BonusWorld)
		{
			string path = "Textures/PuffleOBreadcrumb/PuffleOBreadcrumbBonusLevel_texture_01";
			if (ResolutionManager.Instance.AssetResolution == ResolutionManager.eAssetResolution.eLowres)
			{
				path = "Textures/PuffleOBreadcrumb/PuffleOBreadcrumbBonusLevel_texture_01";
			}
			Texture mainTexture = Resources.Load(path, typeof(Texture)) as Texture;
			gameObject.GetComponent<MeshRenderer>().materials[0].mainTexture = mainTexture;
		}
		Object.Destroy(base.gameObject);
	}

	private void OnCollect()
	{
		AudioManager.Instance.PlayPuffleOSound(mPuffleOCollectSound);
		mMagnet = null;
		mVelocity = Vector3.zero;
		Object.Destroy(base.GetComponent<Collider>());
		SpriteManager spriteManager = (SpriteManager)Object.Instantiate(Resources.Load("Prefabs/PuffleOEffect", typeof(SpriteManager)) as SpriteManager, mTransform.position, default(Quaternion));
		spriteManager.animationend += EffectEndEventHandler;
		spriteManager.transform.localScale *= ScaleItem.Instance.BillboardScale;
		base.GetComponent<Renderer>().enabled = false;
	}
}
