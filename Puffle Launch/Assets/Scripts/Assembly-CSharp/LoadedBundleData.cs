using UnityEngine;

public class LoadedBundleData
{
	public string name;

	public AssetBundle bundle;

	private int usercount;

	public bool valid
	{
		get
		{
			return bundle != null;
		}
	}

	public LoadedBundleData(string aName, AssetBundle aBundle)
	{
		name = aName;
		bundle = aBundle;
		usercount = 0;
	}

	public void Acquire()
	{
		usercount++;
	}

	public void Release()
	{
		usercount--;
		if (usercount == 0)
		{
			bundle.Unload(true);
			bundle = null;
		}
	}

	public void Destroy()
	{
		if (valid)
		{
			usercount = 0;
			bundle.Unload(true);
		}
	}
}
