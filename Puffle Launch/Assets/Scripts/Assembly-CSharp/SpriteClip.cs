using System;
using UnityEngine;

[Serializable]
public class SpriteClip
{
	public string[] stringTiles;

	public string[] urls;

	public Vector2 stride;

	public Vector2 offset;

	public int cols;

	public int rows;

	public int total;

	public bool useHighRes;

	private int m_CurrentFrame;

	private bool[] m_Ignore;

	private Texture2D[] tiles;

	public static bool FORCE_SCALE;

	public bool loaded
	{
		get
		{
			if ((tiles == null || tiles.Length == 0) && stringTiles.Length > 0)
			{
				tiles = new Texture2D[stringTiles.Length];
				for (int i = 0; i < stringTiles.Length; i++)
				{
					if (ResolutionManager.Instance.AssetResolution == ResolutionManager.eAssetResolution.eLowres && !useHighRes)
					{
						tiles[i] = Resources.Load(string.Format("{0}_lowres", stringTiles[i]), typeof(Texture2D)) as Texture2D;
						Utilities.AssertMsg(tiles[i] != null, string.Format("Low-res sprite sheet not found: {0}", stringTiles[i]));
					}
					else
					{
						tiles[i] = Resources.Load(stringTiles[i], typeof(Texture2D)) as Texture2D;
						Utilities.AssertMsg(tiles[i] != null, string.Format("Sprite sheet not found: {0}", stringTiles[i]));
					}
				}
			}
			for (int j = 0; j < stringTiles.Length; j++)
			{
				Texture2D texture2D = tiles[j];
				if ((m_Ignore == null || j >= m_Ignore.Length || !m_Ignore[j]) && texture2D == null)
				{
					return false;
				}
			}
			return true;
		}
	}

	public void Reset()
	{
		m_CurrentFrame = 0;
	}

	public void SetIgnore(bool[] aIgnore)
	{
		m_Ignore = aIgnore;
	}

	public void Apply(SpriteManager aManager, Material[] aMaterials)
	{
		int i = 0;
		if (tiles == null)
		{
			tiles = new Texture2D[stringTiles.Length];
		}
		for (int j = 0; j < stringTiles.Length; j++)
		{
			if (tiles[j] == null)
			{
				if (ResolutionManager.Instance.AssetResolution == ResolutionManager.eAssetResolution.eLowres && !useHighRes)
				{
					tiles[j] = Resources.Load(string.Format("{0}_lowres", stringTiles[j]), typeof(Texture2D)) as Texture2D;
					Utilities.AssertMsg(tiles[j] != null, string.Format("Low-res sprite sheet not found: {0}", stringTiles[j]));
				}
				else
				{
					tiles[j] = Resources.Load(stringTiles[j], typeof(Texture2D)) as Texture2D;
					Utilities.AssertMsg(tiles[j] != null, string.Format("Sprite sheet not found: {0}", stringTiles[j]));
				}
			}
			if (tiles[j] == null || (m_Ignore != null && j < m_Ignore.Length && m_Ignore[j]) || !(aMaterials[i] != null))
			{
				continue;
			}
			Material material = aMaterials[i];
			if (FORCE_SCALE || material.mainTexture != tiles[j])
			{
				material.mainTexture = tiles[j];
				material.mainTextureScale = new Vector2((stride.x - 1f) / (float)tiles[j].width, (stride.y - 1f) / (float)tiles[j].height);
				if (ResolutionManager.Instance.AssetResolution == ResolutionManager.eAssetResolution.eLowres && !useHighRes)
				{
					material.mainTextureScale *= 0.5f;
				}
			}
			material.mainTextureOffset = new Vector2(stride.x / (float)tiles[j].width * (float)(m_CurrentFrame % cols), stride.y / (float)tiles[j].height * (float)(m_CurrentFrame / cols));
			if (ResolutionManager.Instance.AssetResolution == ResolutionManager.eAssetResolution.eLowres && !useHighRes)
			{
				material.mainTextureOffset *= 0.5f;
			}
			i++;
		}
		for (; i < aMaterials.Length; i++)
		{
			Material material2 = aMaterials[i];
			material2.mainTexture = null;
		}
		aManager.ClipApplied(this);
	}

	public int Update(int aDeltaFrame)
	{
		m_CurrentFrame += aDeltaFrame;
		if (m_CurrentFrame >= total)
		{
			return m_CurrentFrame - (total - 1);
		}
		return 0;
	}

	public void Unload()
	{
		stringTiles = null;
		tiles = null;
	}
}
