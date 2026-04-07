using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CinematicManager : MonoBehaviour
{
    public enum MovieId
    {
        eIntro = 0,
        eAboutCP = 1,
        eMovieId_COUNT = 2
    }

    public enum MovieType
    {
        eUnknown = -1,
        eNetwork = 0,
        eLocal = 1,
        eMovieType_COUNT = 2
    }

    public enum MovieState
    {
        eIdle = 0,
        eReadyToPlay = 1,
        ePlaySucceeded = 2,
        ePlayFailed = 3,
        eMovieState_COUNT = 4
    }

    private class CinematicData
    {
        public string movieURL = string.Empty;
        public MovieType movieType = MovieType.eUnknown;
        public event PlayFailedHandler playFailed;

        public CinematicData(string aMovieURL)
        {
            if (!string.IsNullOrEmpty(aMovieURL))
            {
                movieURL = aMovieURL;
                movieType = IsNetworkBaseURL(movieURL) ? MovieType.eNetwork : MovieType.eLocal;
            }
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(movieURL) && movieType != MovieType.eUnknown;
        }

        private bool IsNetworkBaseURL(string aMovieURL)
        {
            return aMovieURL.ToLower().Contains("http://") || aMovieURL.ToLower().Contains("https://") || aMovieURL.ToLower().Contains("file://");
        }

        public void OnPlayFailed()
        {
            playFailed?.Invoke();
        }

        public bool HasPlayFailedHandler()
        {
            return playFailed != null;
        }
    }

    public delegate void PlayFailedHandler();
    public delegate void PlayCompletedHandler(bool aSuccess);

    private const string kIntroMovieURL = "Trailer_480x320.m4v";

    private static CinematicManager m_cInstance;
    private Dictionary<MovieId, CinematicData> m_CinematicList = new Dictionary<MovieId, CinematicData>();
    private CinematicData m_CurrentMovieData;
    private MovieState m_MovieState;

    private bool m_ShowFullscreenBgWhenPlaying = true;
    private GameObject m_FullscreenBgObj;
    private MeshRenderer m_FullscreenBgMesh;

    private VideoPlayer m_VideoPlayer;

    public static CinematicManager Instance
    {
        get
        {
            if (m_cInstance == null)
            {
                GameObject go = Instantiate(Resources.Load("Prefabs/Managers/CinematicManager", typeof(GameObject))) as GameObject;
                if (go == null) Debug.LogError("Fail to instantiate CinematicManager from prefab!");
            }
            return m_cInstance;
        }
    }

    public bool ShowFullscreenBgWhenPlaying
    {
        get => m_ShowFullscreenBgWhenPlaying;
        set => m_ShowFullscreenBgWhenPlaying = value;
    }

    public event PlayCompletedHandler playCompleted;

    private void Awake()
    {
        m_cInstance = this;
        CreateCinematicList();

        m_FullscreenBgObj = Instantiate(Resources.Load("Prefabs/GUI/FullscreenBG", typeof(GameObject))) as GameObject;
        if (m_FullscreenBgObj == null) Debug.LogError("Fail to instantiate FullscreenBG from prefab!");
        m_FullscreenBgMesh = m_FullscreenBgObj.GetComponent<MeshRenderer>();
        if (m_FullscreenBgMesh == null) Debug.LogError("Fail to get MeshRenderer component from FullscreenBG object!");
        m_FullscreenBgMesh.enabled = false;

        m_VideoPlayer = gameObject.AddComponent<VideoPlayer>();
        m_VideoPlayer.playOnAwake = false;
        m_VideoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
        m_VideoPlayer.targetCameraAlpha = 1f;
        m_VideoPlayer.loopPointReached += OnVideoFinished;
        m_VideoPlayer.errorReceived += OnVideoError;
    }

    private void Update()
    {
        switch (m_MovieState)
        {
            case MovieState.eReadyToPlay:
                PlayMovie(m_CurrentMovieData);
                break;
            case MovieState.ePlaySucceeded:
                ChangeMovieState(MovieState.eIdle);
                break;
            case MovieState.ePlayFailed:
                m_CurrentMovieData?.OnPlayFailed();
                ChangeMovieState(MovieState.eIdle);
                break;
        }
    }

    private void OnDestroy()
    {
        m_cInstance = null;
    }

    private void CreateCinematicList()
    {
        m_CinematicList[MovieId.eIntro] = new CinematicData(kIntroMovieURL);

        string aboutCP = "http://wpc.176f.edgecastcdn.net/80176F/external01.tapulous.com/content/CPVideos/CPVideo_en_Android.mp4";

        m_CinematicList[MovieId.eAboutCP] = new CinematicData(aboutCP);
        m_CinematicList[MovieId.eAboutCP].playFailed += OnAboutCPMoviePlayFailed;
    }

    private CinematicData GetCinematicData(MovieId aMovieId)
    {
        if (!m_CinematicList.TryGetValue(aMovieId, out var value))
        {
            Debug.LogError($"Movie: '{aMovieId}' not found in cinematic list!");
            return null;
        }
        return value;
    }

    public void Play(MovieId aMovieId)
    {
        m_CurrentMovieData = GetCinematicData(aMovieId);
        ChangeMovieState(MovieState.eReadyToPlay);
    }

    public void Play(string aMovieURL)
    {
        m_CurrentMovieData = new CinematicData(aMovieURL);
        ChangeMovieState(MovieState.eReadyToPlay);
    }

    private void PlayMovie(CinematicData aCineData)
    {
        if (aCineData == null || !aCineData.IsValid()) return;

        // Mute audio
        AudioListener.volume = 0f;

        m_VideoPlayer.url = aCineData.movieURL;
        m_VideoPlayer.Play();

        ChangeMovieState(MovieState.ePlaySucceeded);
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        ChangeMovieState(MovieState.ePlaySucceeded);
    }

    private void OnVideoError(VideoPlayer vp, string message)
    {
        Debug.LogError("VideoPlayer error: " + message);
        ChangeMovieState(MovieState.ePlayFailed);
    }

    private void ChangeMovieState(MovieState aNewState)
    {
        if (aNewState >= MovieState.eMovieState_COUNT)
        {
            Debug.LogError("Unknown movie state: " + aNewState);
            return;
        }

        switch (aNewState)
        {
            case MovieState.eIdle:
                m_CurrentMovieData = null;
                playCompleted?.Invoke(m_MovieState == MovieState.ePlaySucceeded);
                playCompleted = null;
                ShowFullscreenBg(false);
                AudioListener.volume = 1f;
                break;
            case MovieState.eReadyToPlay:
                if (m_ShowFullscreenBgWhenPlaying) ShowFullscreenBg(true);
                break;
        }

        m_MovieState = aNewState;
    }

    private void ShowFullscreenBg(bool aShow)
    {
        if (m_FullscreenBgMesh != null)
            m_FullscreenBgMesh.enabled = aShow;
    }

    private void OnAboutCPMoviePlayFailed()
    {
        Debug.LogWarning("About CP movie failed to play.");
    }
}