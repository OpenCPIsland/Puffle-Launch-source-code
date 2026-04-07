using System.Threading;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
	public bool showFPS = true;

	public float FPSCap;

	public float updateInterval = 0.5f;

	private float accum;

	private int frames;

	private float timeleft;

	private Color fpsColor = default(Color);

	private string fpsText = string.Empty;

	private Vector2 fpsPos;

	private Vector2 fpsShadowPosOffset;

	private Rect fpsRect;

	private Rect fpsShadowRect;

	private GUIStyle fpsStyle;

	private GUIStyle fpsShadowStyle;

	private int cachedScreenWidth;

	private int cachedScreenHeight;

	private static bool isCreated;

	private void Awake()
	{
		if (isCreated)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		Object.DontDestroyOnLoad(this);
		isCreated = true;
	}

	private void Start()
	{
		timeleft = updateInterval;
		RefreshLayout();
	}

	private void Update()
	{
		if (!showFPS)
		{
			return;
		}
		if (FPSCap > 0f)
		{
			Thread.Sleep((int)(1000f / FPSCap));
		}
		timeleft -= Time.deltaTime;
		accum += Time.timeScale / Time.deltaTime;
		frames++;
		if ((double)timeleft <= 0.0)
		{
			float num = accum / (float)frames;
			fpsText = string.Format("{0:F2} FPS", num);
			if (num > 25f)
			{
				fpsColor = Color.green;
			}
			else if (num > 10f)
			{
				fpsColor = Color.yellow;
			}
			else
			{
				fpsColor = Color.red;
			}
			timeleft = updateInterval;
			accum = 0f;
			frames = 0;
		}
	}

	private void OnGUI()
	{
		if (!showFPS || string.IsNullOrEmpty(fpsText))
		{
			return;
		}
		RefreshLayout();
		EnsureStyles();
		fpsStyle.normal.textColor = fpsColor;
		GUI.Label(fpsShadowRect, fpsText, fpsShadowStyle);
		GUI.Label(fpsRect, fpsText, fpsStyle);
	}

	private void RefreshLayout()
	{
		if (cachedScreenWidth == Screen.width && cachedScreenHeight == Screen.height)
		{
			return;
		}
		cachedScreenWidth = Screen.width;
		cachedScreenHeight = Screen.height;
		fpsPos = new Vector2(0.05f * (float)Screen.width, 0.05f * (float)Screen.height);
		fpsShadowPosOffset = new Vector2(0.0025f * (float)Screen.width, -0.0025f * (float)Screen.height);
		float num = Mathf.Max(18f, (float)Screen.height * 0.03f);
		fpsRect = new Rect(fpsPos.x, fpsPos.y, (float)Screen.width * 0.3f, num * 1.5f);
		fpsShadowRect = new Rect(fpsPos.x + fpsShadowPosOffset.x, fpsPos.y + fpsShadowPosOffset.y, fpsRect.width, fpsRect.height);
		if (fpsStyle != null)
		{
			fpsStyle.fontSize = Mathf.RoundToInt(num);
		}
		if (fpsShadowStyle != null)
		{
			fpsShadowStyle.fontSize = Mathf.RoundToInt(num);
		}
	}

	private void EnsureStyles()
	{
		if (fpsStyle == null)
		{
			fpsStyle = new GUIStyle(GUI.skin.label);
			fpsStyle.alignment = TextAnchor.UpperLeft;
			fpsStyle.fontSize = Mathf.RoundToInt(Mathf.Max(18f, (float)Screen.height * 0.03f));
		}
		if (fpsShadowStyle == null)
		{
			fpsShadowStyle = new GUIStyle(fpsStyle);
			fpsShadowStyle.normal.textColor = Color.black;
		}
	}
}
