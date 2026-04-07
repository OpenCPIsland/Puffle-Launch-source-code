using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownloadableAnimationManager : MonoBehaviour
{
	public string BaseUrl;

	public bool debug;

	private List<StreamingRequest> m_Requests = new List<StreamingRequest>();

	public void Attach(SpriteManager sprite)
	{
		sprite.animationstart += QueueAnimation;
	}

	public void AttachExtra(SpriteManager sprite)
	{
	}

	private void Start()
	{
		StartCoroutine(ProcessRequests());
	}

	public void QueueAnimation(object sender, AnimationChangedEventArgs e)
	{
		if (!e.anim.loaded)
		{
			QueueRequests(sender as SpriteManager, e.anim);
		}
	}

	public void UnloadAll()
	{
		StopAllCoroutines();
	}

	private void AddRequest(SpriteAnimation aAnim, SpriteManager aSprite)
	{
		SpriteManager extraSprite = aSprite.extraSprite;
		if (extraSprite != null)
		{
			SpriteAnimation spriteAnimation = extraSprite.GetAnimation(aAnim.name);
			if (spriteAnimation != null)
			{
				spriteAnimation.extra.HookExtra(aSprite, aAnim);
			}
		}
	}

	private StreamingRequest FirstRequest()
	{
		if (m_Requests.Count == 0)
		{
			return null;
		}
		return m_Requests[0];
	}

	private void QueueRequests(SpriteManager aSprite, SpriteAnimation aAnim)
	{
		AddRequest(aAnim, aSprite);
	}

	public void PrefetchAnimation(SpriteManager aSprite, SpriteAnimation aAnim)
	{
		QueueRequests(aSprite, aAnim);
	}

	public IEnumerator ProcessRequests()
	{
		while (true)
		{
			if (m_Requests.Count == 0)
			{
				yield return null;
				continue;
			}
			StreamingRequest req = m_Requests[0];
			IEnumerator e = req.process();
			if (e != null)
			{
				while (e.MoveNext())
				{
					yield return e.Current;
				}
			}
			m_Requests.RemoveAt(0);
		}
	}
}
