using UnityEngine;

public class CoinTransferProgressBar : MonoBehaviour
{
	private Transform mLeftEnd;

	private Transform mRightEnd;

	private Transform[] mChunks;

	private void Start()
	{
		mLeftEnd = base.transform.Find("LeftEnd");
		mRightEnd = base.transform.Find("RightEnd");
		ShowSection(mLeftEnd, false);
		ShowSection(mRightEnd, false);
		mChunks = new Transform[7];
		for (int i = 1; i < 8; i++)
		{
			mChunks[i - 1] = base.transform.Find("Chunk" + i);
			ShowSection(mChunks[i - 1], false);
		}
	}

	private void FixedUpdate()
	{
	}

	private void SetProgress(float aProgress)
	{
		ShowSection(mLeftEnd, false);
		ShowSection(mRightEnd, false);
		for (int i = 1; i < 8; i++)
		{
			ShowSection(mChunks[i - 1], false);
		}
		if (aProgress > 10f)
		{
			ShowSection(mLeftEnd, true);
		}
		if (aProgress > 20f)
		{
			ShowSection(mChunks[0], true);
		}
		if (aProgress > 30f)
		{
			ShowSection(mChunks[1], true);
		}
		if (aProgress > 40f)
		{
			ShowSection(mChunks[2], true);
		}
		if (aProgress > 50f)
		{
			ShowSection(mChunks[3], true);
		}
		if (aProgress > 60f)
		{
			ShowSection(mChunks[4], true);
		}
		if (aProgress > 70f)
		{
			ShowSection(mChunks[5], true);
		}
		if (aProgress > 80f)
		{
			ShowSection(mChunks[6], true);
		}
		if (aProgress > 90f)
		{
			ShowSection(mRightEnd, true);
		}
	}

	private void ShowSection(Transform aSection, bool aShow)
	{
		aSection.transform.position = new Vector3(aSection.transform.position.x, aSection.transform.position.y, (!aShow) ? 1f : (-1f));
	}
}
