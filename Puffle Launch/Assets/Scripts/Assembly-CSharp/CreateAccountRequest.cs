using System.Collections;

public class CreateAccountRequest : BaseNetRequest
{
	public enum ServerLanguage
	{
		eEnglish = 1,
		ePortuguese = 2,
		eFrench = 4,
		eSpanish = 8
	}

	protected override void CreateRequiredResultKeyList()
	{
		m_RequiredResultKeys.Add("authToken");
	}

	public Message BuildRequestMessage(string aUserName, string aPassword, string aEmail, int aColor)
	{
		int num = (int)ConvertToServerLanguageCode(LocalizationManager.GetLanguageCode());
		Message message = new Message("/mobileas/api/json/account/create_account");
		message.AddParameter("appVersion", "pl-1.0");
		message.AddParameter("user", aUserName);
		message.AddParameter("pass", aPassword);
		message.AddParameter("email", aEmail);
		message.AddParameter("color", aColor.ToString());
		message.AddParameter("lang", num.ToString());
		return message;
	}

	private ServerLanguage ConvertToServerLanguageCode(string aLanguage)
	{
		switch (aLanguage)
		{
		default:
			return ServerLanguage.eEnglish;
		case "fr":
			return ServerLanguage.eFrench;
		case "es":
			return ServerLanguage.eSpanish;
		case "pt":
			return ServerLanguage.ePortuguese;
		}
	}

	protected override void OnFail(Hashtable aResult)
	{
	}

	protected override void OnSuccess(Hashtable aResult)
	{
		NetManager.Instance.UpdateAuthToken(aResult["authToken"] as string);
	}
}
