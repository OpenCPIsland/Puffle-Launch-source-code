using System.Collections.Generic;

public static class NetError
{
	public enum ClientError
	{
		eNone = 0,
		eFailToConnectToServer = 1,
		eServerTimeOut = 2,
		eServerError = 3,
		eMissingResultKey = 4,
		ePasswordEmptyConfirm = 5,
		ePasswordMismatch = 6,
		eUnknown = 7,
		eClientError_COUNT = 8
	}

	public enum ServerError
	{
		eNone = 0,
		ePlayerNotFound = -32070,
		ePlayerBanned = -32071,
		ePlayerBannedForever = -32072,
		ePlayerActive = -32074,
		eInvalidToken = -32076,
		eDataAccessError = -32101,
		eUnauthorizedAccess = -32102,
		eAccountNotFound = -32270,
		eAuthorizationFailed = -32271,
		eUserNameTaken = -32276,
		eUserNameEmpty = -32277,
		eUserNameTooShort = -32278,
		eUserNameTooLong = -32279,
		eUserNameTooManyNumbers = -32280,
		eUserNameTooManySpaces = -32281,
		eUserNameTooFewChars = -32282,
		eUserNameWrongFormat = -32283,
		eUserNameBannedWord = -32284,
		eUserNameNotAllowed = -32285,
		ePasswordEmpty = -32286,
		ePasswordTooShort = -32289,
		ePasswordTooLong = -32290,
		ePasswordMatchesUserName = -32291,
		ePasswordEasyToGuess = -32292,
		ePasswordIsAFirstName = -32293,
		eEmailEmpty = -32294,
		eEmailWrongFormat = -32295,
		eEmailWrongBannedDomain = -32296,
		eEmailTooManyAccounts = -32297,
		eEmailBadISP = -32299,
		eWrongPlayerColor = -32298,
		eReachedDailyLimit = -32401,
		eInvalidMissingParam = -32602,
		eInternalSystemError = -32603,
		eReceiptNotFound = -32301,
		eInvalidGuestFlow = -32302,
		eReceiptMismatch = -32303,
		eReceiptRedeemedByAnother = -32305
	}

	private static Dictionary<int, string> m_cErrorCodeDict;

	public static void CreateErrorCodeDictionary()
	{
		Utilities.AssertMsg(m_cErrorCodeDict == null, "Error Code Dictionary already created!");
		m_cErrorCodeDict = new Dictionary<int, string>();
		m_cErrorCodeDict[1] = "TXT_FailToConnect";
		m_cErrorCodeDict[2] = "TXT_NetworkError";
		m_cErrorCodeDict[3] = "TXT_NetworkError";
		m_cErrorCodeDict[4] = "TXT_ServerError";
		m_cErrorCodeDict[5] = "TXT_PasswordEmptyConfirm";
		m_cErrorCodeDict[6] = "TXT_PasswordMismatch";
		m_cErrorCodeDict[7] = "TXT_FailToConnect";
		m_cErrorCodeDict[-32070] = "TXT_ServerError";
		m_cErrorCodeDict[-32071] = "TXT_ServerError";
		m_cErrorCodeDict[-32072] = "TXT_ServerError";
		m_cErrorCodeDict[-32074] = "TXT_PlayerActive";
		m_cErrorCodeDict[-32076] = "TXT_ServerError";
		m_cErrorCodeDict[-32101] = "TXT_Error";
		m_cErrorCodeDict[-32102] = "TXT_ServerError";
		m_cErrorCodeDict[-32270] = "TXT_AccountNotFound";
		m_cErrorCodeDict[-32271] = "TXT_AuthorizationFailed";
		m_cErrorCodeDict[-32276] = "TXT_UserNameTaken";
		m_cErrorCodeDict[-32277] = "TXT_UserNameEmpty";
		m_cErrorCodeDict[-32278] = "TXT_UserNameTooShort";
		m_cErrorCodeDict[-32279] = "TXT_UserNameTooLong";
		m_cErrorCodeDict[-32280] = "TXT_UserNameTooManyNumbers";
		m_cErrorCodeDict[-32281] = "TXT_UserNameTooManySpaces";
		m_cErrorCodeDict[-32282] = "TXT_UserNameTooFewChars";
		m_cErrorCodeDict[-32283] = "TXT_UserNameWrongFormat";
		m_cErrorCodeDict[-32284] = "TXT_UserNameBannedWord";
		m_cErrorCodeDict[-32285] = "TXT_UserNameNotAllowed";
		m_cErrorCodeDict[-32286] = "TXT_PasswordEmpty";
		m_cErrorCodeDict[-32289] = "TXT_PasswordTooShort";
		m_cErrorCodeDict[-32290] = "TXT_PasswordTooLong";
		m_cErrorCodeDict[-32291] = "TXT_PasswordMatchesUserName";
		m_cErrorCodeDict[-32292] = "TXT_PasswordEasyToGuess";
		m_cErrorCodeDict[-32293] = "TXT_PasswordlsAfirstName";
		m_cErrorCodeDict[-32294] = "TXT_EmailEmpty";
		m_cErrorCodeDict[-32295] = "TXT_EmailWrongFormat";
		m_cErrorCodeDict[-32296] = "TXT_EmailWrongBannedDomain";
		m_cErrorCodeDict[-32297] = "TXT_EmailTooManyAccounts";
		m_cErrorCodeDict[-32299] = "TXT_EmailBadISP";
		m_cErrorCodeDict[-32298] = "TXT_ServerError";
		m_cErrorCodeDict[-32401] = "TXT_ReachedDailyLimit";
		m_cErrorCodeDict[-32602] = "TXT_ServerError";
		m_cErrorCodeDict[-32603] = "TXT_Error";
		m_cErrorCodeDict[-32301] = "TXT_ReceiptNotFound";
		m_cErrorCodeDict[-32302] = "TXT_InvalidGuestFlow";
		m_cErrorCodeDict[-32303] = "TXT_ReceiptMismatch";
		m_cErrorCodeDict[-32305] = "TXT_ReceiptRedeemedByAnother";
	}

	public static string GetErrorMsg(int aErrorCode)
	{
		string value;
		if (m_cErrorCodeDict.TryGetValue(aErrorCode, out value))
		{
			return LocalizationManager.Instance.GetString(value);
		}
		return "Error: " + aErrorCode;
	}

	public static string GetErrorMsgTextId(int aErrorCode)
	{
		string value;
		if (m_cErrorCodeDict.TryGetValue(aErrorCode, out value))
		{
			return value;
		}
		return "Error: " + aErrorCode;
	}

	public static bool IsUserNameRelatedError(int aErrorCode)
	{
		return (aErrorCode <= -32276 && aErrorCode >= -32285) || aErrorCode == -32270;
	}

	public static bool IsPasswordRelatedError(int aErrorCode)
	{
		return (aErrorCode <= -32286 && aErrorCode >= -32293) || aErrorCode == -32271;
	}

	public static bool IsPasswordMismatchError(int aErrorCode)
	{
		return aErrorCode == 6 || aErrorCode == 5;
	}

	public static bool IsEmailRelatedError(int aErrorCode)
	{
		return (aErrorCode <= -32294 && aErrorCode >= -32297) || aErrorCode == -32299;
	}
}
