using UnityEngine;

public static class GUIConstants
{
	public const string kLegalLoadingScene = "LegalAndLoadingScreen";

	public const string kGameScene = "Gameplay";

	public const string kMainMenuScene = "!Loader_MainMenu";

	public const string kLevelSelectScene = "LevelSelect";

	public const string kLevelSelectScene_Lite = "LevelSelect_Lite";

	public const string kInfoScene = "Information";

	public const string kLoginScene = "Login";

	public const string kCreateAccountScene = "CreateAccount";

	public const string kEndCinematicScene = "EndCinematic";

	public const string kAboutCP = "AboutCP";

	public const string kCredits = "Credits";

	public const string kCreditsNew = "CreditsNew";

	public const string kLightGrayButtonStyle = "LightGrayButton";

	public const string kCheatButtonStyle = "CheatButton";

	public const string kTextFieldStyle = "TextField";

	public const string kSmallButtonStyle = "SmallButton";

	public const string kSlowmoButtonStyle = "SlowmoButton";

	public const string kTurboButtonStyle = "TurboButton";

	public const string kCPTextLargeStyle = "CPTextLarge";

	public const string kCPTextMediumStyle = "CPTextMedium";

	public const string kErrorPopupWindowStyle = "ErrorPopupWindow";

	public const string kInGameTextMiniStyle = "InGameTextMini";

	public const string kInGameTextSmallStyle = "InGameTextSmall";

	public const string kInGameTextMediumStyle = "InGameTextMedium";

	public const string kInGameTextLargeStyle = "InGameTextLarge";

	public const string kTallyScreenCounterStyle = "TallyScreenCounter";

	public const string kLowResImageNameSuffix = "_lowres";

	public const string kIpadImageNameSuffix = "_iPad";

	public const string kCommonPath = "GUI/Common/";

	public const string kSemiTransparentImageName = "GUI/Common/semi_transparent";

	public const string kTopBarImageName = "Create_Account_NavBar";

	public const string kBackButton = "Create_Account_BackBtn";

	public const string kBackButtonPressed = "Create_Account_BackBtn_pressed";

	public const string kLevelSelectPath = "GUI/LevelSelect/";

	public const string kLevelPadlock = "GUI/LevelSelect/level_lock";

	public const string kLevelPuffleONew = "GUI/LevelSelect/puffle-o_new";

	public const string kLevelPuffleOBronze = "GUI/LevelSelect/puffle-o_orange";

	public const string kLevelPuffleOSilver = "GUI/LevelSelect/puffle-o_silver";

	public const string kLevelPuffleOGold = "GUI/LevelSelect/puffle-o_gold";

	public const string kLevelPuffleOTurbo = "GUI/LevelSelect/puffle-o_fire";

	public const string kLevelClock = "GUI/LevelSelect/clock";

	public const string kTallyProgressRing = "GUI/TallyMenu/ProgressRing/progress_bar_";

	public const string kTallyFinishedRing = "GUI/TallyMenu/ProgressRing/Finished/finished";

	public const string kTurboButton = "GUI/LevelSelect/turbo";

	public const string kCoinTransferPath = "GUI/CoinTransfer/Textures/";

	public const string kLoginBGImageName = "GUI/CreditsNew/Credits_BG_BlueGradient";

	public const string kLoginButtonImageName = "GUI/Common/button";

	public const string kLoginButtonPressedImageName = "GUI/Common/button_pressed";

	public const string kLoginSeparatorImageName = "GUI/CoinTransfer/Textures/Login_seperator";

	public const string kCPLogoImageName = "GUI/CreditsNew/CP_Logo_HiRes";

	public const string kCPTextFieldImageName = "GUI/CreateAccountNew/Create_Account_FormBox";

	public const string kCPTextFieldHighlightImageName = "GUI/CreateAccountNew/Create_Account_FormBox_Focus";

	public const string kCPTextFieldOrangeImageName = "GUI/CreateAccountNew/Create_Account_FormBox_Error";

	public const string kFontPath = "Font/";

	public const string kLowResFontPrefix = "LowRes";

	public const float kOriginalScreenWidth = 960f;

	public const float kOriginalScreenHeight = 640f;

	public const float kLowResScreenWidth = 480f;

	public const float kLowResScreenHeight = 320f;

	public const float kIpadScreenWidth = 1024f;

	public const float kIpadScreenHeight = 768f;

	public const float kAspectRatioIPhone = 1.5f;

	public const float kAspectRatioIPad = 1.3333334f;

	public const float kIpadXOffset = 0f;

	public const float kIpadYOffset = 0f;

	public const float kIpadWidthScale = 0.9375f;

	public const float kIpadHeightScale = 5f / 6f;

	public const float kBackButtonDetectZoneScale = 1.5f;

	public const float kMotionSyncFactor = 30f;

	public const float kOutOfScreenOffset = 30f;

	public const float kOutOfScreenOffsetIpad = 45f;

	public const float kMoveTextFieldOffset = 170f;

	public const float kTextFieldCursorBlinkPeriod = 1.5f;

	public const float kMinMargin = 10f;

	public const int kInvalidIndex = -1;

	public const int kMaxTextLength = 20;

	public const int kMaxUsernameLength = 12;

	public const int kMaxEmailLength = 40;

	public const float kTextureRotateSpeed = 4f;

	public const char kPasswordMask = '*';

	public const float kPassworkNukeDuration = 2f;

	public const string kBuyAppLink = "market://details?id=com.disney.PuffleLaunch";

	public static string[] kFontNames = new string[2] { "BURBANKSMALL-BOLD", "CPWONDERBOY" };

	public static string[] kFontNamesJA = new string[2] { "epmarugo", "epmarugo" };

	public static int[] kFontSizes = new int[4] { 20, 26, 35, 55 };

	public static int[] kLowResFontSizes = new int[4] { 10, 13, 17, 27 };

	public static float kReferenceScreenWidth = 0f;

	public static float kReferenceScreenHeight = 0f;

	public static Color kBlackColor = Color.black;

	public static Color kWhiteColor = Color.white;

	public static Color kRedColor = Color.red;

	public static Color kOrangeColor = new Color(1f, 0.73725f, 0.22745f, 1f);

	public static Color kLightBrownColor = new Color(0.45490196f, 0.36862746f, 0.3254902f, 1f);

	public static Color kDarkBrownColor = new Color(0.3019608f, 0.23921569f, 11f / 51f, 1f);

	public static Color kDarkerBrownColor = new Color(0.11372549f, 0.09019608f, 4f / 51f, 1f);

	public static Color kFaintBrownColor = new Color(0.6549f, 0.56863f, 0.52549f, 1f);

	public static Color kBlueColor = new Color(0.12159f, 0.46667f, 0.81569f, 1f);

	public static Color kGreyColor = new Color(0.72941f, 0.72941f, 0.72941f, 1f);

	public static Color kLightGreyColor = new Color(0.985f, 0.985f, 0.985f, 1f);

	public static Color kDarkGreyColor = new Color(0.2039f, 0.2039f, 0.2039f, 1f);

	public static Color kLessDarkGreyColor = new Color(0.4f, 0.4f, 0.4f, 1f);

	public static Color kTOULinkColorColor = new Color(0.5f, 0.75f, 0.9f);

	public static Color kLevelSelectBlueTextColor = new Color(0.17647f, 0.26667f, 0.46275f, 1f);

	public static Color kLevelSelectOrangeTextColor = new Color(0.4549f, 0.22353f, 0.08235f, 1f);

	public static Color kLevelSelectNewTextColor = new Color(0.97647f, 0.95686f, 0.41961f, 1f);
}
