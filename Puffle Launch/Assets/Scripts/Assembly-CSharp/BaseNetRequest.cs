using System.Collections;
using System.Collections.Generic;
using Procurios.Public;
using UnityEngine;

public abstract class BaseNetRequest
{
	public enum Separator
	{
		eNone = 0,
		ePrefix = 1,
		eSuffix = 2,
		eSeparator_COUNT = 3
	}

	public enum Feedback
	{
		eVerbose = 0,
		eProgressingOnly = 1,
		eErrorOnly = 2,
		eSilent = 3,
		eFeedbackMode_COUNT = 4
	}

	public class Message
	{
		private string m_BaseURL;

		private WWWForm m_Parameters;

		public Dictionary<string, string> m_ParamterDict = new Dictionary<string, string>();

		public Message(string aQueryString)
		{
			m_BaseURL = NetConstants.kHost + aQueryString;
			m_Parameters = new WWWForm();
			m_ParamterDict.Clear();
		}

		public void AddParameter(string aKey, string aValue)
		{
			string value = ((aValue == null) ? string.Empty : aValue);
			m_Parameters.AddField(aKey, value);
			m_ParamterDict.Add(aKey, aValue);
		}

		public WWW CreateConnection()
		{
			Dictionary<string, string> headers = m_Parameters.headers;
			headers["Content-Type"] = headers["Content-Type"] + "; charset=UTF-8";
			byte[] data = m_Parameters.data;
			return new WWW(m_BaseURL, data, headers);
		}
	}

	public delegate void RequestCompleteCB(bool aSuccess);

	protected Message m_RequestMssage;

	protected List<string> m_RequiredResultKeys = new List<string>();

	protected List<RequestCompleteCB> m_RequestCompleteCB;

	protected bool m_InProgress;

	protected Feedback m_FeedbackMode;

	protected WWW m_WWW;

	protected float m_RequestStartTime;

	protected bool m_IsRequestTimeOut;

	protected bool m_IsRequestCancelled;

	protected Hashtable m_LastResult;

	protected int m_LastErrorCode;

	protected string m_LastErrorMsg = string.Empty;

	public bool InProgress
	{
		get
		{
			return m_InProgress;
		}
	}

	public Feedback FeedbackMode
	{
		get
		{
			return m_FeedbackMode;
		}
		set
		{
			m_FeedbackMode = value;
		}
	}

	public bool SilentProgressingFeedback
	{
		get
		{
			return m_FeedbackMode == Feedback.eSilent || m_FeedbackMode == Feedback.eErrorOnly;
		}
	}

	public bool SilentErrorFeedback
	{
		get
		{
			return m_FeedbackMode == Feedback.eSilent || m_FeedbackMode == Feedback.eProgressingOnly;
		}
	}

	public Hashtable LastResult
	{
		get
		{
			return m_LastResult;
		}
	}

	public int LastErrorCode
	{
		get
		{
			return m_LastErrorCode;
		}
	}

	public string LastErrorMsg
	{
		get
		{
			return m_LastErrorMsg;
		}
	}

	public BaseNetRequest()
	{
		m_RequestCompleteCB = new List<RequestCompleteCB>();
		Init();
		CreateRequiredResultKeyList();
	}

	protected virtual void Init()
	{
	}

	protected abstract void CreateRequiredResultKeyList();

	protected abstract void OnFail(Hashtable aResult);

	protected abstract void OnSuccess(Hashtable aResult);

	public virtual void Update()
	{
		if (m_InProgress && !m_IsRequestTimeOut && Time.realtimeSinceStartup - m_RequestStartTime >= 30f)
		{
			m_IsRequestTimeOut = true;
			CancelRequest();
		}
	}

	public virtual IEnumerator SendRequest(Message aMessage)
	{
		m_LastResult = null;
		m_LastErrorCode = 0;
		m_LastErrorMsg = string.Empty;
		m_RequestMssage = aMessage;
		NetManager.Instance.ShowProgressing(true, SilentProgressingFeedback);
		m_InProgress = true;
		m_RequestStartTime = Time.realtimeSinceStartup;
		m_IsRequestTimeOut = false;
		m_IsRequestCancelled = false;
		m_WWW = aMessage.CreateConnection();
		yield return m_WWW;
		bool handleServerResult = m_WWW != null && m_WWW.isDone && !m_IsRequestCancelled;
		RequestDone(handleServerResult);
	}

	public virtual void CancelRequest()
	{
		if (!m_IsRequestCancelled && m_WWW != null && !m_WWW.isDone)
		{
			m_IsRequestCancelled = true;
			m_WWW.Dispose();
			bool isRequestTimeOut = m_IsRequestTimeOut;
			RequestDone(isRequestTimeOut);
		}
	}

	protected virtual void RequestDone(bool aHandleServerResult)
	{
		if (aHandleServerResult)
		{
			HandleServerResult(m_WWW);
		}
		m_InProgress = false;
		m_WWW = null;
	}

	protected void HandleServerResult(WWW aWww)
	{
		bool flag = false;
		Hashtable aResult = (m_LastResult = GetServerResult(aWww));
		if (HandleError(aResult))
		{
			flag = false;
			OnFail(aResult);
		}
		else
		{
			NetManager.Instance.ShowProgressing(false, SilentProgressingFeedback);
			flag = true;
			OnSuccess(aResult);
		}
		ExecuteRequestCompleteCBs(flag);
	}

	protected virtual Hashtable GetServerResult(WWW aWww)
	{
		Hashtable hashtable;
		if (m_IsRequestTimeOut)
		{
			hashtable = new Hashtable();
			hashtable["clientError"] = NetError.ClientError.eServerTimeOut;
		}
		else if (aWww == null)
		{
			hashtable = new Hashtable();
			hashtable["clientError"] = NetError.ClientError.eUnknown;
		}
		else if (aWww.error != null)
		{
			hashtable = new Hashtable();
			hashtable["clientError"] = NetError.ClientError.eFailToConnectToServer;
		}
		else
		{
			hashtable = JSON.JsonDecode(aWww.text) as Hashtable;
			if (hashtable == null)
			{
				hashtable = new Hashtable();
				hashtable["clientError"] = NetError.ClientError.eServerError;
			}
		}
		return hashtable;
	}

	protected virtual bool HandleError(Hashtable aResult)
	{
		if (aResult.Contains("clientError"))
		{
			ShowNetError((int)aResult["clientError"]);
			return true;
		}
		if (!aResult.Contains("success"))
		{
			ShowNetError(4, "success");
			return true;
		}
		if (!(bool)aResult["success"])
		{
			if (!aResult.Contains("errorCode"))
			{
				ShowNetError(4, "errorCode");
				return true;
			}
			int serverErrorCode = GetServerErrorCode(aResult);
			if (!HandleSpecialServerError(serverErrorCode, aResult))
			{
				ShowNetError(serverErrorCode);
			}
			return true;
		}
		foreach (string requiredResultKey in m_RequiredResultKeys)
		{
			if (!aResult.Contains(requiredResultKey))
			{
				ShowNetError(4, requiredResultKey);
				return true;
			}
		}
		return false;
	}

	private bool HandleSpecialServerError(int aErrorCode, Hashtable aResult)
	{
		bool result = false;
		string empty = string.Empty;
		switch ((NetError.ServerError)aErrorCode)
		{
		case NetError.ServerError.eUnauthorizedAccess:
		case NetError.ServerError.eInvalidToken:
			NetManager.Instance.ResetAuthToken();
			break;
		case NetError.ServerError.eUserNameTaken:
			if (aResult.Contains("userSuggestion"))
			{
				empty = LocalizationManager.Instance.GetString(NetError.GetErrorMsgTextId(aErrorCode), aResult["userSuggestion"]);
				ShowNetError(aErrorCode, empty, string.Empty);
			}
			else
			{
				empty = LocalizationManager.Instance.GetString("TXT_UserNameTaken1");
				ShowNetError(aErrorCode, empty, string.Empty);
			}
			result = true;
			break;
		case NetError.ServerError.eEmailBadISP:
		{
			string value = string.Empty;
			if (m_RequestMssage != null && m_RequestMssage.m_ParamterDict.TryGetValue("email", out value))
			{
				string empty2 = string.Empty;
				if (value != null)
				{
					int num = value.IndexOf('@');
					empty2 = value.Substring(num + 1);
					empty = LocalizationManager.Instance.GetString(NetError.GetErrorMsgTextId(aErrorCode), empty2);
					ShowNetError(aErrorCode, empty, string.Empty);
					result = true;
				}
			}
			break;
		}
		}
		return result;
	}

	public int GetErrorCode(Hashtable aResult)
	{
		if (aResult != null && aResult.Contains("clientError"))
		{
			NetError.ClientError clientError = (NetError.ClientError)(int)aResult["clientError"];
			if (clientError != NetError.ClientError.eNone)
			{
				return (int)clientError;
			}
		}
		return GetServerErrorCode(aResult);
	}

	public int GetServerErrorCode(Hashtable aResult)
	{
		if (aResult != null && aResult.Contains("errorCode"))
		{
			return (int)(double)aResult["errorCode"];
		}
		return 0;
	}

	private void ShowNetError(int aErrorCode)
	{
		ShowNetError(aErrorCode, NetError.GetErrorMsg(aErrorCode), string.Empty);
	}

	private void ShowNetError(int aErrorCode, string aExtraErrorInfo)
	{
		ShowNetError(aErrorCode, NetError.GetErrorMsg(aErrorCode), aExtraErrorInfo);
	}

	private void ShowNetError(int aErrorCode, string aErrorMsg, string aExtraErrorInfo)
	{
		SetNetError(aErrorCode, aErrorMsg + aExtraErrorInfo);
		NetManager.Instance.ShowError(m_LastErrorMsg, SilentErrorFeedback);
	}

	public void SetNetError(int aErrorCode)
	{
		SetNetError(aErrorCode, NetError.GetErrorMsg(aErrorCode));
	}

	public void SetNetError(int aErrorCode, string aErrorMsg)
	{
		m_LastErrorCode = aErrorCode;
		m_LastErrorMsg = aErrorMsg;
	}

	public virtual void RegisterRequestCompleteCB(RequestCompleteCB aCallback)
	{
		m_RequestCompleteCB.Add(aCallback);
	}

	public virtual void UnRegisterAllRequestCompleteCBs()
	{
		m_RequestCompleteCB.Clear();
	}

	public virtual void ExecuteRequestCompleteCBs(bool aSuccess)
	{
		foreach (RequestCompleteCB item in m_RequestCompleteCB)
		{
			item(aSuccess);
		}
		UnRegisterAllRequestCompleteCBs();
	}
}
