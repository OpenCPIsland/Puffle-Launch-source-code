using UnityEngine;

public class ProgressBar : MonoBehaviour
{
	public GameObject mShine;

	public Camera mCamera;

	public ProgressText progressText;

	private Transform mTransform;

	private SpriteManager mBarSpriteManager;

	private SpriteManager mShineSpriteManager;

	private TextMesh mTextMesh;

	private TextMesh mTextMeshShadow;

	private int mCollectedPuffleOs;

	private int mTotalPuffleOs;

	private float mPercentageCollected;

	private int mCurrentFrame;

	private bool[] mTextShown;

	private bool initialized;

	private float mBaseOrthographicSize;

	private Vector3 mBaseScale;

	private Vector3 mScreenPosition = default(Vector3);

	private Vector3 mTempMeshSize;

	private Vector3 mTempLocalPosition;

	public int TotalPuffleOs
	{
		get
		{
			return mTotalPuffleOs;
		}
		set
		{
			mTotalPuffleOs = value;
		}
	}

	public void Start()
	{
		mTransform = base.transform;
		mBarSpriteManager = GetComponent<SpriteManager>();
		mShineSpriteManager = mShine.GetComponent<SpriteManager>();
		mTotalPuffleOs = LevelLoader.Instance.NumPuffleOs;
		mBarSpriteManager.clipchanged += TilesChangedEventHandler;
		mBaseOrthographicSize = mCamera.orthographicSize;
		mBaseScale = mTransform.localScale;
		mTextShown = new bool[3];
		bool[] array = mTextShown;
		bool flag;
		mTextShown[1] = (flag = (mTextShown[2] = false));
		array[0] = flag;
		mTextMesh = progressText.GetComponent<TextMesh>();
		if (progressText.textShadow != null)
		{
			mTextMeshShadow = progressText.textShadow.GetComponent<TextMesh>();
		}
		if (ResolutionManager.Instance.LayoutSize == ResolutionManager.eLayoutSize.eLowres)
		{
			mBaseScale *= 0.5f;
			Transform transform = mTransform.Find("Timer").transform;
			transform.localScale *= 2f;
			transform.localPosition *= 2f;
		}
	}

	public void Update()
	{
		if (mTotalPuffleOs <= 0)
		{
			return;
		}
		if (mCurrentFrame >= 6 && mCurrentFrame <= 11 && mShineSpriteManager.CurrentAnimation() != 0)
		{
			mShineSpriteManager.Play("Shine");
		}
		if (mPercentageCollected >= (float)(mCurrentFrame + 1) * 8.333333f)
		{
			mCurrentFrame++;
			DoTextAnimation();
			if (mCurrentFrame <= 11)
			{
				mBarSpriteManager.Seek(mCurrentFrame);
				return;
			}
			mShineSpriteManager.Play("EmptyAnim");
			mBarSpriteManager.Play("Finished");
		}
	}

	public void LateUpdate()
	{
		float num = mCamera.orthographicSize / mBaseOrthographicSize;
		mTransform.localScale = mBaseScale * num;
		mScreenPosition.x = Screen.width;
		mScreenPosition.y = Screen.height;
		mScreenPosition.z = 2f;
		mScreenPosition = mCamera.ScreenToWorldPoint(mScreenPosition);
		mTempMeshSize = base.GetComponent<Renderer>().bounds.size;
		mScreenPosition.x -= mTempMeshSize.x * 0.6f;
		mScreenPosition.y -= mTempMeshSize.y * 0.65f;
		mTransform.position = mScreenPosition;
	}

	public void CollectPuffleO()
	{
		mCollectedPuffleOs++;
		mPercentageCollected = (float)mCollectedPuffleOs / (float)mTotalPuffleOs * 100f;
	}

	public void TilesChangedEventHandler(object sender, ClipChangedEventArgs e)
	{
		if (!initialized)
		{
			initialized = true;
			ScaleItem.Instance.ScaleLevelItem(mTransform, 1f, 1f, false);
			mTempMeshSize = base.GetComponent<Renderer>().bounds.size;
			mTempLocalPosition = mTransform.localPosition;
			mTempLocalPosition.x -= mTempMeshSize.x * 0.6f;
			mTempLocalPosition.y -= mTempMeshSize.y * 0.65f;
			mTransform.localPosition = mTempLocalPosition;
		}
	}

	private void DoTextAnimation()
	{
		if (mPercentageCollected == 100f)
		{
			mTextMesh.text = LocalizationManager.Instance.GetString("TXT_Good4");
			if (mTextMeshShadow != null)
			{
				mTextMeshShadow.text = mTextMesh.text;
			}
			progressText.Show = true;
		}
		else if (mPercentageCollected >= 75f && !mTextShown[2])
		{
			mTextShown[2] = true;
			mTextMesh.text = LocalizationManager.Instance.GetString("TXT_Good3");
			if (mTextMeshShadow != null)
			{
				mTextMeshShadow.text = mTextMesh.text;
			}
			progressText.Show = true;
		}
		else if (mPercentageCollected >= 50f && !mTextShown[1])
		{
			mTextShown[1] = true;
			mTextMesh.text = LocalizationManager.Instance.GetString("TXT_Good2");
			if (mTextMeshShadow != null)
			{
				mTextMeshShadow.text = mTextMesh.text;
			}
			progressText.Show = true;
		}
		else if (mPercentageCollected >= 25f && !mTextShown[0])
		{
			mTextShown[0] = true;
			mTextMesh.text = LocalizationManager.Instance.GetString("TXT_Good1");
			if (mTextMeshShadow != null)
			{
				mTextMeshShadow.text = mTextMesh.text;
			}
			progressText.Show = true;
		}
	}
}
