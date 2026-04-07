using System;
using UnityEngine;

[Serializable]
public class SpriteAnimation
{
	public string name;

	public string stream;

	public int framerate;

	public bool looping;

	public bool preload;

	public SpriteClip[] clips;

	public SpriteExtra extra;

	private int m_CurrentClip;

	private int m_CurrentFrame;

	private float m_CurrentTime;

	private Texture2D[] m_CachedClips;

	public bool loaded
	{
		get
		{
			return current.loaded;
		}
	}

	public int frame
	{
		get
		{
			return m_CurrentFrame;
		}
	}

	protected SpriteClip current
	{
		get
		{
			if (m_CurrentClip == -1 || clips == null || m_CurrentClip >= clips.Length)
			{
				return null;
			}
			return clips[m_CurrentClip];
		}
	}

	public void SetIgnore(bool[] aIgnore)
	{
		SpriteClip[] array = clips;
		foreach (SpriteClip spriteClip in array)
		{
			spriteClip.SetIgnore(aIgnore);
		}
	}

	public void Reset()
	{
		m_CurrentClip = 0;
		m_CurrentFrame = 0;
		m_CurrentTime = 0f;
		current.Reset();
	}

	public void Seek(int aFrame)
	{
		Reset();
		m_CurrentFrame = aFrame;
		m_CurrentTime = (float)aFrame / (float)framerate;
		while (aFrame >= current.total && m_CurrentClip != clips.Length)
		{
			aFrame -= current.total;
			m_CurrentClip++;
		}
		if (m_CurrentClip != clips.Length)
		{
			current.Reset();
			current.Update(aFrame);
		}
		else
		{
			Reset();
		}
	}

	public void GoToLastFrame()
	{
		Seek(GetTotalNumFrame() - 1);
	}

	public int GetTotalNumFrame()
	{
		int num = 0;
		for (int i = 0; i < clips.Length; i++)
		{
			num += clips[i].total;
		}
		return num;
	}

	public void Apply(SpriteManager aManager, Material[] aMaterials)
	{
		if (current != null)
		{
			current.Apply(aManager, aMaterials);
		}
	}

	public float Update(SpriteManager aManager, float aDeltaTime)
	{
		if (framerate == 0)
		{
			return 0f;
		}
		m_CurrentTime += aDeltaTime;
		int num = (int)(m_CurrentTime * (float)framerate + 0.5f) - m_CurrentFrame;
		m_CurrentFrame += num;
		while (num != 0)
		{
			num = current.Update(num);
			if (num <= 0)
			{
				continue;
			}
			m_CurrentClip++;
			if (m_CurrentClip == clips.Length)
			{
				Reset();
				if (!looping)
				{
					return (float)num / (float)framerate;
				}
			}
			num--;
			current.Reset();
		}
		aManager.FrameChanged(name, m_CurrentFrame);
		return 0f;
	}

	public void Preload()
	{
		int num = 0;
		SpriteClip[] array = clips;
		foreach (SpriteClip spriteClip in array)
		{
			num += spriteClip.stringTiles.Length;
		}
		m_CachedClips = new Texture2D[num];
		int num2 = 0;
		SpriteClip[] array2 = clips;
		foreach (SpriteClip spriteClip2 in array2)
		{
			string[] stringTiles = spriteClip2.stringTiles;
			foreach (string text in stringTiles)
			{
				if (ResolutionManager.Instance.AssetResolution == ResolutionManager.eAssetResolution.eLowres && !spriteClip2.useHighRes)
				{
					m_CachedClips[num2] = Resources.Load(string.Format("{0}_lowres", text), typeof(Texture2D)) as Texture2D;
					Utilities.AssertMsg(m_CachedClips[num2] != null, string.Format("Low-res sprite sheet not found: {0}", text));
				}
				else
				{
					m_CachedClips[num2] = Resources.Load(text, typeof(Texture2D)) as Texture2D;
					Utilities.AssertMsg(m_CachedClips[num2] != null, string.Format("Sprite sheet not found: {0}", text));
				}
				num2++;
			}
		}
	}
}
