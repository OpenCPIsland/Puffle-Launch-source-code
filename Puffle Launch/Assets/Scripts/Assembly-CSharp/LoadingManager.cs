using UnityEngine;

public class LoadingManager : MonoBehaviour
{
	private void Start()
	{
		GameFlowManager.Instance.LoadScene("!Loader_MainMenu", false);
	}

	private void Update()
	{
	}
}
