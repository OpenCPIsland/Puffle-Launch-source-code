using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public enum MusicTrack
	{
		eMusic_Gameplay = 0,
		eMusic_Boss = 1,
		eMusic_Win = 2,
		eMusic_Cinematic = 3,
		eMusic_Menu = 4,
		eMusic_COUNT = 5
	}

	public AudioSource cannonAudioSource;

	public AudioSource puffleOAudioSource;

	public AudioSource obstacleAudioSource;

	public AudioSource musicAudioSource;

	public AudioSource mUISFx;

	public AudioClip pianoHit;

	public AudioClip cactusHit;

	public AudioClip pinkBalloonBump;

	public AudioClip gameplayMusic;

	public AudioClip bossMusic;

	public AudioClip winMusic;

	public AudioClip cinematicMusic;

	public AudioClip menuMusic;

	public float musicVolume;

	private AudioClip mMusicClip;

	private static AudioManager mSingleton;

	private int mMuteRequestCount;

	private int mMuteBackup;

	public float MusicVolume
	{
		get
		{
			return musicVolume;
		}
		set
		{
			musicVolume = value;
			if (musicAudioSource != null)
			{
				musicAudioSource.volume = GetNormalizedMusicVolume();
			}
		}
	}

	public bool Muted
	{
		get
		{
			return mMuteRequestCount > 0;
		}
	}

	public AudioClip CurrentMusic
	{
		get
		{
			return mMusicClip;
		}
	}

	public static AudioManager Instance
	{
		get
		{
			return mSingleton;
		}
	}

	private void Awake()
	{
		mSingleton = this;
		ConfigureAudioSources();
		if (PlayerPrefs.GetInt("AudioIsMute", 0) == 1)
		{
			Mute();
		}
	}

	public void Start()
	{
		mSingleton = this;
		ConfigureAudioSources();
	}

	public void PlayCannonSound(AudioClip aSound)
	{
		cannonAudioSource.PlayOneShot(aSound);
	}

	public void PlayPuffleOSound(AudioClip aSound)
	{
		puffleOAudioSource.PlayOneShot(aSound);
	}

	public void PlayObstacleSound(AudioClip aSound)
	{
		obstacleAudioSource.loop = false;
		obstacleAudioSource.clip = aSound;
		obstacleAudioSource.Play();
	}

	public bool IsObstacleSoundPlaying()
	{
		return obstacleAudioSource.isPlaying;
	}

	public void PlayMusic(MusicTrack aMusic)
	{
		AudioClip aMusic2 = null;
		switch (aMusic)
		{
		case MusicTrack.eMusic_Gameplay:
			aMusic2 = gameplayMusic;
			break;
		case MusicTrack.eMusic_Boss:
			aMusic2 = bossMusic;
			break;
		case MusicTrack.eMusic_Win:
			aMusic2 = winMusic;
			break;
		case MusicTrack.eMusic_Cinematic:
			aMusic2 = cinematicMusic;
			break;
		case MusicTrack.eMusic_Menu:
			aMusic2 = menuMusic;
			break;
		}
		PlayMusic(aMusic2);
	}

	public void PlayMusic(AudioClip aMusic)
	{
		if (mMusicClip != aMusic)
		{
			mMusicClip = aMusic;
			musicAudioSource.clip = mMusicClip;
			musicAudioSource.volume = GetNormalizedMusicVolume();
			musicAudioSource.Play();
		}
	}

	public void Mute()
	{
		mMuteRequestCount++;
		mMuteRequestCount = Mathf.Max(mMuteRequestCount, 0);
		if (mMuteRequestCount > 0)
		{
			PlayerPrefs.SetInt("AudioIsMute", 1);
			SetMuteEnabled(true);
		}
	}

	public void Unmute()
	{
		mMuteRequestCount--;
		if (mMuteRequestCount <= 0)
		{
			PlayerPrefs.SetInt("AudioIsMute", 0);
			SetMuteEnabled(false);
			mMuteRequestCount = 0;
		}
	}

	public void ForceMute()
	{
		mMuteBackup = PlayerPrefs.GetInt("AudioIsMute");
		SetMuteEnabled(true);
	}

	public void ResetMute()
	{
		SetMuteEnabled(mMuteBackup == 1);
	}

	public void PlayUISFx(AudioClip aAudioClip)
	{
		if (aAudioClip != null && IsSoundEnabled())
		{
			mUISFx.clip = aAudioClip;
			mUISFx.time = 0f;
			mUISFx.Play();
		}
	}

	public void SetMuteEnabled(bool ab_soundEnabled)
	{
		cannonAudioSource.mute = ab_soundEnabled;
		puffleOAudioSource.mute = ab_soundEnabled;
		obstacleAudioSource.mute = ab_soundEnabled;
		musicAudioSource.mute = ab_soundEnabled;
		mUISFx.mute = ab_soundEnabled;
	}

	private void Update()
	{
		SetMuteEnabled(!IsSoundEnabled());
	}

	public bool IsSoundEnabled()
	{
		return mMuteRequestCount == 0;
	}

	private void ConfigureAudioSources()
	{
		ConfigureAudioSource(cannonAudioSource, false, 1f);
		ConfigureAudioSource(puffleOAudioSource, false, 1f);
		ConfigureAudioSource(obstacleAudioSource, false, 1f);
		ConfigureAudioSource(mUISFx, false, 1f);
		ConfigureAudioSource(musicAudioSource, true, GetNormalizedMusicVolume());
	}

	private void ConfigureAudioSource(AudioSource aSource, bool aLoop, float aVolume)
	{
		if (aSource == null)
		{
			return;
		}
		aSource.playOnAwake = false;
		aSource.loop = aLoop;
		aSource.volume = Mathf.Clamp01(aVolume);
		aSource.spatialBlend = 0f;
		aSource.dopplerLevel = 0f;
		aSource.rolloffMode = AudioRolloffMode.Linear;
		aSource.minDistance = 1f;
		aSource.maxDistance = 500f;
	}

	private float GetNormalizedMusicVolume()
	{
		if (musicVolume > 1f)
		{
			return Mathf.Clamp01(musicVolume / 10f);
		}
		return Mathf.Clamp01(musicVolume);
	}
}
