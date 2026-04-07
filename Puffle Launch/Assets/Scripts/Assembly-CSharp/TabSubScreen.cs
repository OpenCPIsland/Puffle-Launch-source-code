using UnityEngine;

public class TabSubScreen : BaseMonoScreen
{
	public MeshRenderer tabBackground;

	public string[] worldBackgroundsIPad;

	public string[] worldBackgroundsNormal;

	public string[] worldBackgroundsLowres;

	private void Awake()
	{
		UpdateTab((int)GameManager.Instance.CurrentWorld);
	}

	public void UpdateTab(int aSelectedTab)
	{
		iPadTextureEN = worldBackgroundsIPad[aSelectedTab];
		normalTextureEN = worldBackgroundsNormal[aSelectedTab];
		lowresTextureEN = worldBackgroundsLowres[aSelectedTab];
		iPadTextureES = worldBackgroundsIPad[aSelectedTab];
		normalTextureES = worldBackgroundsNormal[aSelectedTab];
		lowresTextureES = worldBackgroundsLowres[aSelectedTab];
		iPadTextureFR = worldBackgroundsIPad[aSelectedTab];
		normalTextureFR = worldBackgroundsNormal[aSelectedTab];
		lowresTextureFR = worldBackgroundsLowres[aSelectedTab];
		iPadTexturePT = worldBackgroundsIPad[aSelectedTab];
		normalTexturePT = worldBackgroundsNormal[aSelectedTab];
		lowresTexturePT = worldBackgroundsLowres[aSelectedTab];
		iPadTextureDE = worldBackgroundsIPad[aSelectedTab];
		normalTextureDE = worldBackgroundsNormal[aSelectedTab];
		lowresTextureDE = worldBackgroundsLowres[aSelectedTab];
		iPadTextureJA = worldBackgroundsIPad[aSelectedTab];
		normalTextureJA = worldBackgroundsNormal[aSelectedTab];
		lowresTextureJA = worldBackgroundsLowres[aSelectedTab];
		Init(base.gameObject);
	}

	protected override void CreateMainScreenLayouts()
	{
	}

	protected override void OnMainScreenButtonSelect()
	{
	}

	protected override void OnBack()
	{
		base.MainScreen.StopGUI();
		AssetLoader.Instance.ScrollList.SetActiveRecursively(false);
		GameFlowManager.Instance.LoadScene("!Loader_MainMenu", false);
	}
}
