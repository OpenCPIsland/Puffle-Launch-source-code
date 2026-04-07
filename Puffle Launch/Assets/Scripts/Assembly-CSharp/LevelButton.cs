using UnityEngine;

public class LevelButton : MonoBehaviour
{
	public float m_MoveSpeed = 2f;

	public float m_RotateSpeed = 1f;

	public float m_ScaleChangeSpeed = 10f;

	public TextMesh m_LevelNumber;

	public TextMesh m_LevelNumberShadow;

	public TextMesh m_TimeTrial;

	public TextMesh m_TimeTrialShadow;

	public Material m_LockedMaterial;

	public Material m_UnlockedMaterial;

	public Material m_CompleteMaterial;

	private Vector3 m_WorkingVector = default(Vector3);

	private Vector3 m_TargetScale;

	public Vector3 m_TargetPosition;

	private float m_TargetRotation;

	private Transform m_Transform;

	private float m_SnapDistance = 0.02f;

	private float m_SnapRotation = 5f;

	private float m_SnapScale = 0.01f;

	private void Awake()
	{
		m_Transform = base.transform;
		m_TargetScale = new Vector3(1f, 1f, 1f);
		m_TargetPosition = new Vector3(0f, -20f, 0f);
	}

	private void Update()
	{
		if (m_Transform.localScale != m_TargetScale)
		{
			ChangeScale();
		}
		if (m_Transform.rotation.y != m_TargetRotation)
		{
			ChangeRotation();
		}
		if (m_Transform.position != m_TargetPosition)
		{
			ChangePosition();
		}
	}

	public static LevelButton Instantiate(int aLevel, int aWorld, int aLevelsPerWorld)
	{
		LevelButton levelButton = null;
		GameObject gameObject = Object.Instantiate(Resources.Load("GUI/LevelSelect/Prefabs/LevelButton", typeof(Object))) as GameObject;
		if (gameObject != null)
		{
			levelButton = gameObject.GetComponent<LevelButton>();
			if (levelButton != null)
			{
				levelButton.SetLevel(aLevel, aWorld, aLevelsPerWorld);
			}
		}
		return levelButton;
	}

	public void SetLevel(int aLevel, int aWorld, int aLevelsPerWorld)
	{
		m_LevelNumber.text = (aLevel + aWorld * aLevelsPerWorld + 1).ToString();
		m_LevelNumberShadow.text = (aLevel + aWorld * aLevelsPerWorld + 1).ToString();
		m_TimeTrial.text = string.Empty;
		m_TimeTrialShadow.text = string.Empty;
		int num = aLevel + aWorld * aLevelsPerWorld;
		if (GameManager.HasCompletedTurboMode(GameManager.Instance.CurrentWorld) || GameManager.Instance.HasAchievedTimeTrialFire(GameManager.Instance.CurrentWorld))
		{
			SetButtonToTurboMode(num);
		}
		else if (GameManager.HasCollectedAllRings(GameManager.Instance.CurrentWorld))
		{
			SetButtonToTimeTrialMode(num);
		}
		else
		{
			SetButtonToRegularMode(num);
		}
	}

	private void ChangePosition()
	{
		m_WorkingVector = Vector3.Lerp(m_Transform.position, m_TargetPosition, Time.deltaTime * m_MoveSpeed);
		if ((m_TargetPosition - m_WorkingVector).magnitude < m_SnapDistance)
		{
			m_WorkingVector = m_TargetPosition;
		}
		m_Transform.position = m_WorkingVector;
	}

	private void ChangeRotation()
	{
		m_WorkingVector = m_Transform.eulerAngles;
		m_WorkingVector.y = Mathf.Lerp(m_Transform.eulerAngles.y, m_TargetRotation, Time.deltaTime * m_RotateSpeed);
		if (Mathf.Abs(m_TargetRotation - m_WorkingVector.y) < m_SnapRotation)
		{
			m_WorkingVector.y = m_TargetRotation;
		}
		m_Transform.eulerAngles = m_WorkingVector;
	}

	private void ChangeScale()
	{
		m_WorkingVector = Vector3.Lerp(m_Transform.localScale, m_TargetScale, Time.deltaTime * m_ScaleChangeSpeed);
		if ((m_TargetScale - m_WorkingVector).magnitude < m_SnapScale)
		{
			m_WorkingVector = m_TargetScale;
		}
		m_Transform.localScale = m_WorkingVector;
	}

	public void SetTargetPosition(Vector3 aPosition)
	{
		m_TargetPosition = aPosition;
	}

	public void SetTargetScale(Vector3 aScale)
	{
		m_TargetScale.x = aScale.x;
		m_TargetScale.y = m_Transform.localScale.z;
		m_TargetScale.z = aScale.y;
	}

	public void SetTargetRotation(float aYRotation)
	{
		m_TargetRotation = aYRotation;
	}

	public void SetInstantPosition(Vector3 aPosition)
	{
		SetTargetPosition(aPosition);
		m_Transform.position = aPosition;
	}

	public void SetInstantScale(Vector3 aScale)
	{
		SetTargetScale(aScale);
		m_Transform.localScale = m_TargetScale;
	}

	private void SetButtonToTurboMode(int aGlobalLevel)
	{
		base.GetComponent<Renderer>().material = m_CompleteMaterial;
		if (ProfileManager.Instance.CurrentProfile.m_LevelData[aGlobalLevel].BestTimeCount != float.MaxValue)
		{
			string timeFormatedString = GameManager.GetTimeFormatedString(ProfileManager.Instance.CurrentProfile.m_LevelData[aGlobalLevel].BestTimeCount);
			m_TimeTrial.text = timeFormatedString;
			m_TimeTrialShadow.text = timeFormatedString;
		}
	}

	private void SetButtonToTimeTrialMode(int aGlobalLevel)
	{
		base.GetComponent<Renderer>().material = m_CompleteMaterial;
		if (ProfileManager.Instance.CurrentProfile.m_LevelData[aGlobalLevel].BestTimeCount != float.MaxValue)
		{
			string timeFormatedString = GameManager.GetTimeFormatedString(ProfileManager.Instance.CurrentProfile.m_LevelData[aGlobalLevel].BestTimeCount);
			m_TimeTrial.text = timeFormatedString;
			m_TimeTrialShadow.text = timeFormatedString;
		}
	}

	private void SetButtonToRegularMode(int aGlobalLevel)
	{
		if (ProfileManager.Instance.CurrentProfile.m_LevelData[aGlobalLevel].LevelComplete)
		{
			base.GetComponent<Renderer>().material = m_CompleteMaterial;
		}
		else if (ProfileManager.Instance.CurrentProfile.m_LevelData[aGlobalLevel].LevelUnlocked)
		{
			base.GetComponent<Renderer>().material = m_UnlockedMaterial;
		}
		else
		{
			base.GetComponent<Renderer>().material = m_LockedMaterial;
		}
	}
}
