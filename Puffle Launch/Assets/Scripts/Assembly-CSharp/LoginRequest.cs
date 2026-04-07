using System.Collections;

public class LoginRequest : BaseNetRequest
{
	protected override void CreateRequiredResultKeyList()
	{
		m_RequiredResultKeys.Add("authToken");
		m_RequiredResultKeys.Add("color");
	}

	public Message BuildRequestMessage(string aUserName, string aPassword)
	{
		Message message = new Message("/mobileas/api/json/account/login");
		message.AddParameter("appVersion", "pl-1.0");
		message.AddParameter("user", aUserName);
		message.AddParameter("pass", aPassword);
		return message;
	}

	protected override void OnFail(Hashtable aResult)
	{
	}

	protected override void OnSuccess(Hashtable aResult)
	{
		ProfileManager.Instance.CurrentProfile.AuthToken = aResult["authToken"] as string;
	}
}
