using System;
using System.Collections;

public class CoinTransferRequest : BaseNetRequest
{
	protected override void Init()
	{
	}

	protected override void CreateRequiredResultKeyList()
	{
	}

	public Message BuildRequestMessage(string aAuthToken, int aNumCoins)
	{
		Message message = new Message("/mobileds/api/json/player/uploadCoin");
		message.AddParameter("appVersion", "pl-1.0");
		message.AddParameter("authToken", aAuthToken);
		message.AddParameter("coin", aNumCoins.ToString());
		return message;
	}

	protected override void OnFail(Hashtable aResult)
	{
		if (aResult.ContainsKey("coin"))
		{
			UpdateProfileCoins(Convert.ToInt32(aResult["coin"]));
		}
	}

	protected override void OnSuccess(Hashtable aResult)
	{
		UpdateProfileCoins(0);
	}

	private void UpdateProfileCoins(int aRemainingCoins)
	{
		GameManager.Instance.CoinsBeforeTransfer = ProfileManager.Instance.CurrentProfile.TotalCoins;
		ProfileManager.Instance.CurrentProfile.TotalCoins = aRemainingCoins;
		ProfileManager.Instance.SaveCurrentProfile();
	}
}
