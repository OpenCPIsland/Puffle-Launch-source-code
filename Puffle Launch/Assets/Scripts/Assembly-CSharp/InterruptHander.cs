using UnityEngine;

public class InterruptHander : MonoBehaviour
{
	private void OnApplicationPause(bool aPause)
	{
		if (!aPause && !GameFlowManager.Instance.GUIManager.IsPauseMenu && GameFlowManager.Instance.GUIManager != null && GameFlowManager.Instance.GUIManager.CurrentScene == GUIManager.Scene.eInGameHud)
		{
			GameFlowManager.Instance.GUIManager.ShowPauseMenu(true);
		}
	}
}
