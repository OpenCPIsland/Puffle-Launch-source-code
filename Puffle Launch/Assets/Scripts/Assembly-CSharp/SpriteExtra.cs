using System;
using UnityEngine;

[Serializable]
public class SpriteExtra
{
	public bool background;

	public int[] frames;

	public SpriteManager sprite;

	private SpriteManager m_ParentSprite;

	[NonSerialized]
	private SpriteAnimation m_Anim;

	private int m_LastFrame;

	public SpriteExtra(SpriteManager aSprite)
	{
		sprite = aSprite;
	}

	public void HookExtra(SpriteManager aSprite, SpriteAnimation aAnim)
	{
		m_Anim = aAnim;
		m_ParentSprite = aSprite;
		m_ParentSprite.framechanged += FrameChanged;
		m_ParentSprite.animationend += AnimationEnd;
		sprite.transform.parent = aSprite.transform;
		sprite.transform.localScale = new Vector3(1f, 1f, 1f);
		if (background)
		{
			sprite.transform.localPosition = new Vector3(0f, 0f, 0.2f);
		}
		else
		{
			sprite.transform.localPosition = new Vector3(0f, 0f, -0.2f);
		}
		m_LastFrame = 0;
	}

	public void AnimationEnd(object sender, AnimationChangedEventArgs e)
	{
		if (e.anim == m_Anim)
		{
			m_ParentSprite.framechanged -= FrameChanged;
			m_ParentSprite.animationend -= AnimationEnd;
			sprite.ClipApplied(null);
		}
	}

	public void FrameChanged(object sender, FrameChangedEventArgs e)
	{
		if (e.frame == m_LastFrame || !(e.name == m_Anim.name))
		{
			return;
		}
		for (int i = 0; i < frames.Length; i++)
		{
			if (m_LastFrame < frames[i] && e.frame >= frames[i])
			{
				if (i == 0)
				{
					sprite.Play(m_Anim.name);
					sprite.Pause(true);
				}
				sprite.Seek(i);
				break;
			}
		}
		m_LastFrame = e.frame;
	}
}
