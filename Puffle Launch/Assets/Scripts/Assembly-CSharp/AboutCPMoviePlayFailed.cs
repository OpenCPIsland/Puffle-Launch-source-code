using UnityEngine;

public class AboutCPMoviePlayFailed : MonoBehaviour
{
	private bool m_IsCompleted;

	public bool IsCompleted
	{
		get
		{
			return m_IsCompleted;
		}
	}

	private void Update()
	{
		m_IsCompleted = Input.touchCount > 0;
	}
}
