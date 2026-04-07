public static class NetConstants
{
	public const string kAppVersion = "pl-1.0";

	public const string kParamStarter = "?";

	public const string kParamSeparator = "&";

	public const string kParamValueAssigner = "=";

	public const string kParamValueAppender = ",";

	public const string kRequestLogin = "/mobileas/api/json/account/login";

	public const string kRequestCreateAccount = "/mobileas/api/json/account/create_account";

	public const string kRequestUpload = "/mobileds/api/json/player/upload";

	public const string kRequestDownload = "/mobileds/api/json/player/download";

	public const string kRequestProductIds = "/mobilecs/api/json/player/getProductIdentifiers";

	public const string kRequestGuestPurchase = "/mobilecs/api/json/player/guest/purchase";

	public const string kRequestMMOPurchase = "/mobilecs/api/json/player/mmo/purchase";

	public const string kRequestCoinTransfer = "/mobileds/api/json/player/uploadCoin";

	public const string kKeyClientError = "clientError";

	public const string kKeySuccess = "success";

	public const string kKeyErrorCode = "errorCode";

	public const string kKeyAppVersion = "appVersion";

	public const string kKeyAuthToken = "authToken";

	public const string kKeyUser = "user";

	public const string kKeyPassword = "pass";

	public const string kKeyEmail = "email";

	public const string kKeyColor = "color";

	public const string kLanguage = "lang";

	public const string kKeyUserSuggestion = "userSuggestion";

	public const string kKeyScore = "score";

	public const string kKeyWinSensei = "winSensei";

	public const string kKeyRank = "rank";

	public const string kKeyWin = "win";

	public const string kKeyLoss = "loss";

	public const string kKeyQuit = "quit";

	public const string kKeyItemIds = "itemIds";

	public const string kKeyCardIds = "cardIds";

	public const string kKeyStampIds = "stampIds";

	public const string kKeyCoin = "coin";

	public const string kKeyUDID = "deviceId";

	public const string kKeyProfileId = "profileId";

	public const string kKeyReceiptData = "receiptData";

	public const string kKeyProductIds = "productIdentifiers";

	public const string kKeyReceiptId = "receiptId";

	public const string kKeyGameCardNumber = "gamecardNumber";

	public const string kKeyCardDuration = "cardDuration";

	public const string kKeyCardUnit = "cardUnit";

	public const int kSyncFrequency = 5;

	public const int kMaxTimeSinceLastSuccessSync = 999;

	public const float kMaxTimeWaitingForServer = 30f;

	public const float kRetryFrequency = 120f;

	public const int kRankValueOffset = -1;

	public static string kHost = "https://ms.clubpenguin.com:8443";
}
