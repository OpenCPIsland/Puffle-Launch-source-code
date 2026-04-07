using UnityEngine;

public class EndCinematic : MonoBehaviour
{
	private void Update()
	{
		if (GameFlowManager.Instance.m_DoWindowBack)
		{
			GameFlowManager.Instance.m_DoWindowBack = false;
			OnCinematicEnd();
		}
	}

	public void OnCinematicEnd()
	{
		if (LevelSelect.SelectedLevel - 1 == 23)
		{
			GameManager.Instance.CurrentWorld = GameManager.World.eWorld_SodaSunset;
			GameFlowManager.Instance.LoadScene("LevelSelect", false);
		}
		else if (LevelSelect.SelectedLevel - 1 == 60)
		{
			GameFlowManager.Instance.LoadScene("!Loader_MainMenu", false);
		}
		else
		{
			GameManager.Instance.CurrentWorld = GameManager.World.eWorld_BonusWorld;
			GameManager.Instance.StartLevel((GameManager.Level)(LevelSelect.SelectedLevel - 1));
			GameFlowManager.Instance.LoadScene("Gameplay", true);
		}
	}
}
