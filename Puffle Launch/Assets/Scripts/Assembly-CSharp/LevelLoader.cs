using System.Collections;
using System.IO;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
	private const float chunkPercentage = 0.2f;

	public string levelName;

	public Puffle puffle;

	public Transform levelEnd;

	public Transform[] backgroundElements;

	public Transform[] obstacleVariants;

	public Cannon[] cannonVariants;

	public PuffleO puffleO;

	public Camera gameplayCamera;

	public FallZone[] fallZones;

	public Splash[] splashVariants;

	public float loadingProgress;

	public bool isLoadingFinished;

	private static LevelLoader mSingleton;

	private int mNumPuffleOs;

	private float mCeilingHeight;

	private StringReader mReader;

	private float progressIncrement;

	private int yieldCount;

	private int elementChunkToLoad = 10;

	public static LevelLoader Instance
	{
		get
		{
			return mSingleton;
		}
	}

	public int NumPuffleOs
	{
		get
		{
			return mNumPuffleOs;
		}
	}

	public void Awake()
	{
		loadingProgress = 0f;
		isLoadingFinished = false;
		StartCoroutine(AwakeInternal());
	}

	public IEnumerator AwakeInternal()
	{
		mSingleton = this;
		TextAsset levelFile = (TextAsset)Resources.Load(string.Format("LevelData/Level{0}", LevelSelect.SelectedLevel), typeof(TextAsset));
		mReader = new StringReader(levelFile.text);
		string line = mReader.ReadLine();
		string[] header = line.Split(',');
		float totalElements = float.Parse(header[6]);
		progressIncrement = 1f / totalElements;
		elementChunkToLoad = (int)(0.2f * totalElements);
		int worldNumber = int.Parse(header[0]) - 1;
		Color[] bgColors = new Color[4]
		{
			new Color(0.015686275f, 23f / 51f, 64f / 85f),
			new Color(0.96862745f, 46f / 85f, 0.28627452f),
			new Color(37f / 85f, 0.09019608f, 0.6156863f),
			new Color(0.99607843f, 66f / 85f, 0.23529412f)
		};
		gameplayCamera.backgroundColor = bgColors[worldNumber];
		float groundPosition = (0f - float.Parse(header[1])) * ScaleItem.Instance.LevelScale;
		Object.Instantiate(fallZones[worldNumber], new Vector3(0f, groundPosition, 0f), default(Quaternion));
		loadingProgress += progressIncrement;
		yield return null;
		Transform puffleTransform = (Transform)Object.Instantiate(AssetLoader.Instance.PuffleTemplate, new Vector3(float.Parse(header[2]) * ScaleItem.Instance.LevelScale, (0f - float.Parse(header[3])) * ScaleItem.Instance.LevelScale, 0f), default(Quaternion));
		Puffle puffleInstance = puffleTransform.GetComponent<Puffle>();
		puffleInstance.tag = "Player";
		puffleInstance.GetComponent<Renderer>().enabled = true;
		puffleInstance.gameObject.SetActive(true);
		puffleInstance.GetComponent<Rigidbody>().WakeUp();
		loadingProgress += progressIncrement;
		yield return null;
		Camera.main.GetComponent<CameraFollow>().Target = puffleInstance.transform;
		Vector3 targetPos = puffleInstance.transform.position;
		targetPos.z = -10f;
		Camera.main.transform.position = targetPos;
		Camera.main.GetComponent<CameraFollow>().FixedUpdate();
		Camera.main.GetComponent<CameraFollow>().UpdateTransform(100f);
		puffleInstance.spawnPoint = puffleInstance.transform.position;
		puffleInstance.groundPosition = groundPosition;
		puffleInstance.Splash = (Splash)Object.Instantiate(splashVariants[worldNumber], puffleInstance.transform.position, default(Quaternion));
		puffleInstance.Splash.gameObject.SetActive(false);
		puffleInstance.Splash.GetComponent<MeshRenderer>().enabled = false;
		ScaleItem.Instance.PlayerRadius = puffleInstance.GetComponent<SphereCollider>().radius;
		ScaleItem.Instance.ScaleLevelItem(puffleInstance.transform, 1f, 1f, true);
		loadingProgress += progressIncrement;
		yield return null;
		mCeilingHeight = puffleInstance.transform.position.y;
		Transform giantPuffleO = (Transform)Object.Instantiate(AssetLoader.Instance.GiantPuffleOTemplate, new Vector3(float.Parse(header[4]) * ScaleItem.Instance.LevelScale, (0f - float.Parse(header[5])) * ScaleItem.Instance.LevelScale, 0f), levelEnd.rotation);
		giantPuffleO.GetComponent<Renderer>().enabled = true;
		giantPuffleO.gameObject.SetActive(true);
		giantPuffleO.tag = "Finish";
		ScaleItem.Instance.ScaleLevelItem(giantPuffleO, 1f, 1f, false);
		while (true)
		{
			line = mReader.ReadLine();
			if (line == null)
			{
				break;
			}
			if (line.Length <= 0)
			{
				continue;
			}
			if (line.Equals("[Background]"))
			{
				yieldCount = 0;
				while (ParseBackground(ref mReader, 0.3f))
				{
					loadingProgress += progressIncrement;
					yieldCount++;
					if (yieldCount == elementChunkToLoad)
					{
						yieldCount = 0;
						yield return null;
					}
				}
				if (yieldCount > 0)
				{
					yield return null;
				}
			}
			else if (line.Equals("[Cannons]"))
			{
				yieldCount = 0;
				while (ParseCannons(ref mReader))
				{
					loadingProgress += progressIncrement;
					yieldCount++;
					if (yieldCount == elementChunkToLoad)
					{
						yieldCount = 0;
						yield return null;
					}
				}
				if (yieldCount > 0)
				{
					yield return null;
				}
			}
			else if (line.Equals("[Obstacles]"))
			{
				yieldCount = 0;
				while (ParseObstacles(ref mReader))
				{
					loadingProgress += progressIncrement;
					yieldCount++;
					if (yieldCount == elementChunkToLoad)
					{
						yieldCount = 0;
						yield return null;
					}
				}
				if (yieldCount > 0)
				{
					yield return null;
				}
			}
			else
			{
				if (!line.Equals("[PuffleOs]"))
				{
					continue;
				}
				yieldCount = 0;
				while (ParsePuffleOs(ref mReader))
				{
					loadingProgress += progressIncrement;
					yieldCount++;
					if (yieldCount == elementChunkToLoad)
					{
						yieldCount = 0;
						yield return null;
					}
				}
				if (yieldCount > 0)
				{
					yield return null;
				}
				GameObject.Find("Main Camera").transform.Find("ProgressBar").GetComponent<ProgressBar>().TotalPuffleOs = mNumPuffleOs;
			}
		}
		mReader.Close();
		if (worldNumber == 2)
		{
			mCeilingHeight += 500f * ScaleItem.Instance.LevelScale;
			puffleInstance.ceilingPosition = mCeilingHeight;
			FallZone ceiling = (FallZone)Object.Instantiate(fallZones[worldNumber], new Vector3(0f, mCeilingHeight, 0f), default(Quaternion));
			Vector3 ceilingScale = ceiling.transform.localScale;
			ceilingScale.y *= -1f;
			ceiling.transform.localScale = ceilingScale;
		}
		else
		{
			puffleInstance.ceilingPosition = float.PositiveInfinity;
		}
		loadingProgress = 1f;
		GameFlowManager.Instance.GUIManager.LoadingScreen.StopLoadingBar();
		isLoadingFinished = true;
		yield return null;
		loadingProgress = 0f;
	}

	private IEnumerator LoadAsset(string assetURL)
	{
		yield return null;
	}

	private bool ParseBackground(ref StringReader aReader, float aZOffset)
	{
		string text = aReader.ReadLine();
		if (text == null || text.Length == 0)
		{
			return false;
		}
		string[] array = text.Split(',');
		int num = int.Parse(array[0]) - 1;
		if (num < backgroundElements.Length)
		{
			Transform transform = backgroundElements[num];
			if ((bool)transform)
			{
				Vector3 position = new Vector3(float.Parse(array[1]) * ScaleItem.Instance.LevelScale, (0f - float.Parse(array[2])) * ScaleItem.Instance.LevelScale, aZOffset);
				Transform aItem = (Transform)Object.Instantiate(transform, position, transform.transform.rotation);
				ScaleItem.Instance.ScaleLevelItem(aItem, float.Parse(array[3]), float.Parse(array[4]), false);
				aZOffset += 0.1f;
			}
			else
			{
				Debug.LogWarning(string.Format("Background element not set: {0}", num));
			}
		}
		else
		{
			Debug.LogWarning(string.Format("Background index out of range: {0}", num));
		}
		return true;
	}

	private bool ParseCannons(ref StringReader aReader)
	{
		string text = mReader.ReadLine();
		if (text == null || text.Length == 0)
		{
			return false;
		}
		string[] array = text.Split(',');
		int num = int.Parse(array[0]) - 1;
		if (num < cannonVariants.Length)
		{
			Cannon cannon = cannonVariants[num];
			if ((bool)cannon)
			{
				Cannon cannon2 = (Cannon)Object.Instantiate(cannon);
				cannon2.gameObject.SetActive(true);
				foreach (Transform item in cannon2.transform)
				{
					item.gameObject.SetActive(true);
				}
				cannon2.transform.position = new Vector3(float.Parse(array[1]) * ScaleItem.Instance.LevelScale, (0f - float.Parse(array[2])) * ScaleItem.Instance.LevelScale, 0f);
				cannon2.transform.eulerAngles = new Vector3(0f, 0f, 0f - float.Parse(array[3]));
				ScaleItem.Instance.ScaleLevelItem(cannon2.transform, float.Parse(array[4]), float.Parse(array[5]), false);
				if (cannon2.transform.position.y > mCeilingHeight)
				{
					mCeilingHeight = cannon2.transform.position.y;
				}
			}
			else
			{
				Debug.LogWarning(string.Format("Cannon variant not set: {0}", num));
			}
		}
		else
		{
			Debug.LogWarning(string.Format("Cannon index out of range: {0}", num));
		}
		return true;
	}

	private bool ParseObstacles(ref StringReader aReader)
	{
		string text = mReader.ReadLine();
		if (text == null || text.Length == 0)
		{
			return false;
		}
		string[] array = text.Split(',');
		int num = int.Parse(array[0]) - 1;
		if (num < obstacleVariants.Length)
		{
			Transform transform = obstacleVariants[num];
			if ((bool)transform)
			{
				Transform transform2 = (Transform)Object.Instantiate(transform);
				transform2.gameObject.SetActive(true);
				transform2.transform.position = new Vector3(float.Parse(array[1]) * ScaleItem.Instance.LevelScale, (0f - float.Parse(array[2])) * ScaleItem.Instance.LevelScale, 0f);
				transform2.transform.eulerAngles = new Vector3(0f, 0f, 0f - float.Parse(array[3]));
				ScaleItem.Instance.ScaleLevelItem(transform2, float.Parse(array[4]), float.Parse(array[5]), false);
				if (transform2.position.y > mCeilingHeight)
				{
					mCeilingHeight = transform2.position.y;
				}
			}
			else
			{
				Debug.LogWarning(string.Format("Obstacle variant not set: {0}", num));
			}
		}
		else
		{
			Debug.LogWarning(string.Format("Obstacle index out of range: {0}", num));
		}
		return true;
	}

	private bool ParsePuffleOs(ref StringReader aReader)
	{
		string text = mReader.ReadLine();
		if (text == null || text.Length == 0)
		{
			return false;
		}
		string[] array = text.Split(',');
		PuffleO puffleO = (PuffleO)Object.Instantiate(this.puffleO);
		puffleO.transform.position = new Vector3(float.Parse(array[1]) * ScaleItem.Instance.LevelScale, (0f - float.Parse(array[2])) * ScaleItem.Instance.LevelScale, 0f);
		ScaleItem.Instance.ScaleLevelItem(puffleO.transform, float.Parse(array[3]), float.Parse(array[4]), false);
		mNumPuffleOs++;
		if (puffleO.transform.position.y > mCeilingHeight)
		{
			mCeilingHeight = puffleO.transform.position.y;
		}
		return true;
	}
}
