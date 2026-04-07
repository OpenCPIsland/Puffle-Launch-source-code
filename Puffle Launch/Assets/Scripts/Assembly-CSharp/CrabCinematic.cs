using UnityEngine;

public class CrabCinematic : MonoBehaviour
{
	private const int kFrameRate = 12;

	public Texture[] animFrames;

	private float mCurrentFrame;

	public void Start()
	{
		mCurrentFrame = 0f;
	}

	public void Update()
	{
		int num = (int)mCurrentFrame;
		mCurrentFrame += Time.deltaTime * 12f;
		int num2 = (int)mCurrentFrame % animFrames.Length;
		if (num2 != num)
		{
			base.GetComponent<Renderer>().material.mainTexture = animFrames[num2];
		}
	}
}
