using UnityEngine;

public class NetManager : MonoBehaviour
{
	public enum Request
	{
		eLogin = 0,
		eCreateAccount = 1,
		eCoinTransfer = 2,
		eRequest_COUNT = 3
	}

	public enum PopupType
	{
		eGeneric = 0,
		eCount = 1,
		eCreateAccount = 2,
		eNone = 3
	}

	public const float kNeverSync = -1f;

	public const float kRetryNow = -1f;

	private static NetManager m_cInstance;

	private string m_OnlineUsername;

	private int m_LastCoinTransferCount;

	private LoginRequest m_LoginRequest;

	private CreateAccountRequest m_CreateAccountRequest;

	private CoinTransferRequest m_CoinTransferRequest;

	private float m_CoinTransferTimeStart;

	private BaseNetRequest m_CurrentRequest;

	private PopupType m_currentPopupType = PopupType.eNone;

	private MessagePopup[] m_NetPopup;

	private ActivityIndicatorPopup m_ActivityIndicatorPopup;

	public static NetManager Instance
	{
		get
		{
			return m_cInstance;
		}
	}

	public bool IsNetPopupShowing
	{
		get
		{
			return (m_ActivityIndicatorPopup != null && m_ActivityIndicatorPopup.IsShowing) || (GetCurrentPopup() != null && GetCurrentPopup().IsShowing);
		}
	}

	private void Awake()
	{
		m_cInstance = this;
		m_NetPopup = new MessagePopup[1];
	}

	private void Start()
	{
		NetError.CreateErrorCodeDictionary();
		m_CoinTransferRequest = new CoinTransferRequest();
	}

	private void Update()
	{
		if (m_CurrentRequest != null && m_CurrentRequest.InProgress)
		{
			m_CurrentRequest.Update();
		}
	}

	public void Draw()
	{
		if (GetCurrentPopup() != null)
		{
			GetCurrentPopup().Draw();
		}
		else
		{
			SetCurrentPopupType(PopupType.eGeneric);
		}
		if (m_ActivityIndicatorPopup == null)
		{
			if (ResolutionManager.Instance.ResolutionInfoSet)
			{
				m_ActivityIndicatorPopup = new ActivityIndicatorPopup(base.gameObject);
			}
		}
		else
		{
			m_ActivityIndicatorPopup.Draw();
		}
	}

	public void ShowProgressing(bool aShow, bool aSilent)
	{
		if (aSilent)
		{
			if (!aShow)
			{
			}
		}
		else if (m_ActivityIndicatorPopup != null)
		{
			m_ActivityIndicatorPopup.Show(aShow);
		}
	}

	public void ShowError(string aErrorMsg, bool aSilent)
	{
		if (m_ActivityIndicatorPopup != null)
		{
			m_ActivityIndicatorPopup.Show(false);
		}
		if (!aSilent && GetCurrentPopup() != null)
		{
			GetCurrentPopup().ShowText(aErrorMsg);
		}
	}

	public void ShowErrorTextId(string aTextId, bool aSilent)
	{
		if (m_ActivityIndicatorPopup != null)
		{
			m_ActivityIndicatorPopup.Show(false);
		}
		if (!aSilent && GetCurrentPopup() != null)
		{
			GetCurrentPopup().ShowTextId(aTextId);
		}
	}

	public void HideError()
	{
		if (GetCurrentPopup() != null)
		{
			GetCurrentPopup().Show(false);
		}
	}

	private MessagePopup GetCurrentPopup()
	{
		if (m_currentPopupType < PopupType.eCount)
		{
			return m_NetPopup[(int)m_currentPopupType];
		}
		return null;
	}

	public void SetCurrentPopupType(PopupType ae_popupType)
	{
		if (m_currentPopupType != ae_popupType)
		{
			if (GetCurrentPopup() != null)
			{
				m_NetPopup[(int)m_currentPopupType] = null;
			}
			m_currentPopupType = ae_popupType;
			if (ae_popupType == PopupType.eGeneric)
			{
				m_NetPopup[(int)m_currentPopupType] = new MessagePopup(base.gameObject);
			}
		}
	}

	public string GetAuthToken()
	{
		return ProfileManager.Instance.CurrentProfile.AuthToken;
	}

	public void UpdateAuthToken(string aAuthToken)
	{
		if (aAuthToken != null)
		{
			ProfileManager.Instance.CurrentProfile.AuthToken = aAuthToken;
			ProfileManager.Instance.SaveCurrentProfile();
		}
	}

	public void ResetAuthToken()
	{
		UpdateAuthToken(string.Empty);
	}

	public bool IsPlayerLoggedIn()
	{
		return ProfileManager.Instance.CurrentProfile.HasAuthToken();
	}

	public void Login(string aUserName, string aPassword, BaseNetRequest.RequestCompleteCB aCallback)
	{
		if (m_LoginRequest == null)
		{
			m_LoginRequest = new LoginRequest();
		}
		m_LoginRequest.FeedbackMode = BaseNetRequest.Feedback.eProgressingOnly;
		m_LoginRequest.RegisterRequestCompleteCB(aCallback);
		BaseNetRequest.Message aMessage = m_LoginRequest.BuildRequestMessage(aUserName, aPassword);
		m_OnlineUsername = aUserName;
		StartCoroutine(m_LoginRequest.SendRequest(aMessage));
		m_CurrentRequest = m_LoginRequest;
	}

	public void TransferCoins(int aNumCoins, BaseNetRequest.RequestCompleteCB aCallback, bool aSilentMode)
	{
		if (aSilentMode)
		{
			m_CoinTransferRequest.FeedbackMode = BaseNetRequest.Feedback.eSilent;
		}
		else
		{
			m_CoinTransferRequest.FeedbackMode = BaseNetRequest.Feedback.eVerbose;
		}
		m_CoinTransferRequest.RegisterRequestCompleteCB(aCallback);
		m_CoinTransferRequest.RegisterRequestCompleteCB(OnTransferCoinsComplete);
		BaseNetRequest.Message aMessage = m_CoinTransferRequest.BuildRequestMessage(GetAuthToken(), aNumCoins);
		m_LastCoinTransferCount = aNumCoins;
		StartCoroutine(m_CoinTransferRequest.SendRequest(aMessage));
		m_CurrentRequest = m_CoinTransferRequest;
		m_CoinTransferTimeStart = Time.realtimeSinceStartup;
	}

	public void OnTransferCoinsComplete(bool aSuccess)
	{
		if (aSuccess)
		{
			int aValue = -1;
			for (int i = 0; i < 60 && ProfileManager.Instance.CurrentProfile.m_LevelData[i].LevelComplete; i++)
			{
				aValue = i;
			}
			BizIntel.ContextualEvent contextualEvent = new BizIntel.ContextualEvent("coin-transfer");
			contextualEvent.AddContextItem("player-id", ProfileManager.Instance.CurrentProfile.ProfileName);
			contextualEvent.AddContextItem("coin-count", m_LastCoinTransferCount);
			contextualEvent.AddContextItem("elapsed-time-msec", (int)((Time.realtimeSinceStartup - m_CoinTransferTimeStart) * 1000f));
			contextualEvent.AddContextItem("highest-level", aValue);
			contextualEvent.AddContextItem("most-recent-level", ProfileManager.Instance.CurrentProfile.LastLevelPlayed);
			contextualEvent.Log();
		}
	}

	public void OnAccountCreationComplete(bool aSuccess)
	{
		if (aSuccess)
		{
			BizIntel.ContextualEvent contextualEvent = new BizIntel.ContextualEvent("create-account");
			contextualEvent.AddContextItem("player-id", m_OnlineUsername);
			contextualEvent.AddContextItem("profile-id", ProfileManager.Instance.CurrentProfile.ProfileName);
			contextualEvent.Log();
		}
	}

	public bool IsAnyRequestInProgess()
	{
		return (m_LoginRequest != null && m_LoginRequest.InProgress) || (m_CoinTransferRequest != null && m_CoinTransferRequest.InProgress);
	}

	public void CreateCPAccount(string aUserName, string aPassword, string aPasswordConfirm, string aEmail, int aColor, BaseNetRequest.RequestCompleteCB aCallback)
	{
		if (m_CreateAccountRequest == null)
		{
			m_CreateAccountRequest = new CreateAccountRequest();
		}
		if (aPassword == aPasswordConfirm)
		{
			SetCurrentPopupType(PopupType.eCreateAccount);
			m_OnlineUsername = aUserName;
			m_CreateAccountRequest.FeedbackMode = BaseNetRequest.Feedback.eProgressingOnly;
			m_CreateAccountRequest.RegisterRequestCompleteCB(aCallback);
			m_CreateAccountRequest.RegisterRequestCompleteCB(OnAccountCreationComplete);
			BaseNetRequest.Message aMessage = m_CreateAccountRequest.BuildRequestMessage(aUserName, aPassword, aEmail, aColor);
			StartCoroutine(m_CreateAccountRequest.SendRequest(aMessage));
			m_CurrentRequest = m_CreateAccountRequest;
		}
		else
		{
			m_CreateAccountRequest.SetNetError((aPasswordConfirm != null && aPasswordConfirm.Length != 0) ? 6 : 5);
			if (aCallback != null)
			{
				aCallback(false);
			}
		}
	}

	public bool HasCoinTransferError()
	{
		return m_CoinTransferRequest != null && m_CoinTransferRequest.LastErrorCode != 0;
	}

	public bool HasReachedCoinTransferLimitError()
	{
		return m_CoinTransferRequest != null && m_CoinTransferRequest.LastErrorCode == -32401;
	}

	public bool HasCoinTransferCompleted()
	{
		return m_CoinTransferRequest.LastResult != null;
	}

	public int GetLastErrorCode(Request aRequest)
	{
		switch (aRequest)
		{
		case Request.eLogin:
			if (m_LoginRequest != null)
			{
				return m_LoginRequest.LastErrorCode;
			}
			break;
		case Request.eCreateAccount:
			if (m_CreateAccountRequest != null)
			{
				return m_CreateAccountRequest.LastErrorCode;
			}
			break;
		case Request.eCoinTransfer:
			if (m_CoinTransferRequest != null)
			{
				return m_CoinTransferRequest.LastErrorCode;
			}
			break;
		}
		return 0;
	}

	public string GetLastErrorMsg(Request aRequest)
	{
		switch (aRequest)
		{
		case Request.eLogin:
			if (m_LoginRequest != null)
			{
				return m_LoginRequest.LastErrorMsg;
			}
			break;
		case Request.eCreateAccount:
			if (m_CreateAccountRequest != null)
			{
				return m_CreateAccountRequest.LastErrorMsg;
			}
			break;
		case Request.eCoinTransfer:
			if (m_CoinTransferRequest != null)
			{
				return m_CoinTransferRequest.LastErrorMsg;
			}
			break;
		}
		return string.Empty;
	}
}
