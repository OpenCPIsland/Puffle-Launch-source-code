using UnityEngine;

public class HitFX : MonoBehaviour
{
	private int mNumEmitters;

	private int mDestroyedEmitters;

	private void Start()
	{
		if (ResolutionManager.Instance.AssetResolution == ResolutionManager.eAssetResolution.eLowres)
		{
			base.transform.localScale *= 0.5f;
		}
		mNumEmitters = GetComponentsInChildren<HitDebris>(true).Length;
		mDestroyedEmitters = 0;
		GetComponent<SpriteManager>().animationend += OnAnimationEnd;
	}

	private void Update()
	{
		if (mDestroyedEmitters == mNumEmitters)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public void OnAnimationEnd(object sender, AnimationChangedEventArgs args)
	{
		GetComponent<MeshRenderer>().enabled = false;
		GetComponent<SpriteManager>().enabled = false;
	}

	public void OnEmitterEnd()
	{
		mDestroyedEmitters++;
	}
}
