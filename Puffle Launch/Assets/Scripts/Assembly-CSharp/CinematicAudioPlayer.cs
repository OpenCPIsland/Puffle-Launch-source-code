using UnityEngine;

public class CinematicAudioPlayer : MonoBehaviour
{
	private void Start()
	{
		GameFlowManager.Instance.AudioManager.PlayMusic(AudioManager.MusicTrack.eMusic_Cinematic);
	}
}
