using UnityEngine;

public class Checkpoint : MonoBehaviour
{
	private bool mActiveCheckpoint;

	private MeshRenderer mFlagRenderer;

	private SpriteManager mFlagSpriteManager;

	private Transform mTransform;

	public void Start()
	{
		mTransform = base.transform;
		Transform transform = mTransform.Find("Flag");
		mFlagRenderer = transform.GetComponent<MeshRenderer>();
		mFlagSpriteManager = transform.GetComponent<SpriteManager>();
		mFlagRenderer.enabled = false;
		switch (ResolutionManager.Instance.AssetResolution)
		{
		case ResolutionManager.eAssetResolution.eLowres:
			transform.localPosition = new Vector3(-10f, 5.8f, 0f);
			break;
		case ResolutionManager.eAssetResolution.eIPad:
			break;
		default:
			transform.localPosition = new Vector3(-5f, 2.9f, 0f);
			break;
		}
	}

	public void Update()
	{
		if (mActiveCheckpoint && !Puffle.Instance.spawnPoint.Equals(mTransform.position))
		{
			mActiveCheckpoint = false;
			mFlagRenderer.enabled = false;
			mFlagSpriteManager.Seek(0);
		}
	}

	public void OnTriggerEnter(Collider aOther)
	{
		if (!mActiveCheckpoint && aOther.tag == "Player")
		{
			mActiveCheckpoint = true;
			mFlagRenderer.enabled = true;
			mFlagSpriteManager.Play("Flag");
			Puffle.Instance.spawnPoint = base.transform.position;
		}
	}
}
