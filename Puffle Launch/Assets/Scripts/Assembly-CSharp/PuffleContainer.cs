using UnityEngine;

public class PuffleContainer : MonoBehaviour
{
	private Puffle mContainedPuffle;

	public void Start()
	{
		mContainedPuffle = null;
	}

	public void Update()
	{
	}

	public void OnPuffleEnter(Puffle aOther)
	{
		mContainedPuffle = aOther;
	}

	public void ReleasePuffle()
	{
		mContainedPuffle = null;
	}

	public Puffle GetContainedPuffle()
	{
		return mContainedPuffle;
	}

	public bool IsPuffleInside()
	{
		return mContainedPuffle != null;
	}
}
