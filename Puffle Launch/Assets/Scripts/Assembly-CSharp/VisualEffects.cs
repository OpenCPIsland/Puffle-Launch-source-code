using UnityEngine;

public class VisualEffects : MonoBehaviour
{
	public Transform slowMoFX;

	public Transform tutorialOverlay;

	public Material tutorialMaterial;

	private bool mForceSlowMoFX;

	private float mBaseOrthographicSize;

	private Transform mTutorialObject;

	private Vector3 mScreenRatioAdjustment;

	private Vector3 mScreenSizeInverse;

	private Color mWhite = new Color(1f, 1f, 1f, 0.5f);

	private Color mClearWhite = new Color(1f, 1f, 1f, 0f);

	public Transform TutorialObject
	{
		get
		{
			return mTutorialObject;
		}
		set
		{
			mTutorialObject = value;
			if (mTutorialObject == null)
			{
				tutorialOverlay.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(1f, 1f);
			}
		}
	}

	public void Start()
	{
		mBaseOrthographicSize = Camera.main.GetComponent<CameraFollow>().OriginalOrthographicSize;
		SpriteMeshGenerator spriteMeshGenerator = new SpriteMeshGenerator(slowMoFX.GetComponent<MeshFilter>());
		spriteMeshGenerator.Generate(new Vector2(-Screen.width / 2, -Screen.height / 2), new Vector2(Screen.width, Screen.height), true);
		tutorialOverlay.GetComponent<MeshFilter>().sharedMesh = slowMoFX.GetComponent<MeshFilter>().sharedMesh;
		tutorialOverlay.transform.localPosition += new Vector3(0f, 0f, 5f);
		tutorialMaterial = tutorialOverlay.GetComponent<Renderer>().material;
		mScreenRatioAdjustment = new Vector3((float)Screen.width / (float)Screen.height, 1f);
		mScreenSizeInverse = new Vector3(1f / (float)Screen.width, 1f / (float)Screen.height);
	}

	public void Update()
	{
		if (slowMoFX.gameObject.active && !mForceSlowMoFX)
		{
			float timeScaleRatio = TimeManager.Instance.TimeScaleRatio;
			if (TimeManager.Instance.TimeScaleRatio == 1f)
			{
				slowMoFX.gameObject.active = false;
			}
			else
			{
				slowMoFX.GetComponent<Renderer>().material.SetColor("_TintColor", Color.Lerp(mWhite, mClearWhite, timeScaleRatio));
			}
		}
	}

	public void LateUpdate()
	{
		float num = Camera.main.orthographicSize / mBaseOrthographicSize;
		slowMoFX.localScale = Vector3.one * num;
		tutorialOverlay.localScale = Vector3.one * num;
		if ((bool)mTutorialObject)
		{
			Vector3 vector = mScreenRatioAdjustment;
			vector *= 0.2f * Mathf.Sin(Time.time * 6f) + 2f;
			tutorialMaterial.mainTextureScale = vector;
			Vector3 vector2 = Camera.main.WorldToScreenPoint(mTutorialObject.position);
			vector.Scale(mScreenSizeInverse);
			vector2.Scale(vector);
			tutorialMaterial.mainTextureOffset = -vector2 + Vector3.one * 0.5f;
		}
	}

	public void ShowSlowMoFX(bool aShow)
	{
		if (aShow)
		{
			slowMoFX.GetComponent<Renderer>().material.SetColor("_TintColor", mWhite);
		}
		slowMoFX.gameObject.active = aShow;
		mForceSlowMoFX = false;
	}

	public void ForceSlowMoFX()
	{
		ShowSlowMoFX(true);
		mForceSlowMoFX = true;
	}

	public void ShowTutorialFX(bool aShow)
	{
		tutorialOverlay.gameObject.active = aShow;
	}
}
