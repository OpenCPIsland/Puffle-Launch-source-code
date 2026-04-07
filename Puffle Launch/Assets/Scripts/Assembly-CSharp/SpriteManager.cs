using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class SpriteManager : MonoBehaviour
{
	public int defaultAnimation;

	public bool defaultOnStart = true;

	public DownloadableAnimationManager manager;

	public SpriteAnimation[] animations;

	public bool sharedMaterial;

	public bool zoomInvariant;

	private SpriteManager m_ExtraSprite;

	private int m_CurrentAnim = -1;

	private SpriteMeshGenerator m_MeshGen;

	private MeshRenderer m_MeshRenderer;

	private SpriteClip m_CurrentClip;

	private bool m_Paused;

	private Transform m_MyTransform;

	private Vector3 m_MyPos;

	public SpriteManager extraSprite
	{
		get
		{
			return m_ExtraSprite;
		}
		set
		{
			m_ExtraSprite = value;
		}
	}

	public SpriteAnimation current
	{
		get
		{
			if (m_CurrentAnim == -1 || m_CurrentAnim >= animations.Length)
			{
				return null;
			}
			return animations[m_CurrentAnim];
		}
	}

	public event FrameChangedEventHandler framechanged;

	public event AnimationChangedEventHandler animationstart;

	public event AnimationChangedEventHandler animationend;

	public event AnimationChangedEventHandler animationfetched;

	public event ClipChangedEventHandler clipchanged;

	private void Awake()
	{
		m_MyTransform = base.transform;
		SpriteAnimation[] array = animations;
		foreach (SpriteAnimation spriteAnimation in array)
		{
			SpriteClip[] clips = spriteAnimation.clips;
			foreach (SpriteClip spriteClip in clips)
			{
				if (spriteClip.stringTiles.Length == 0)
				{
					Debug.LogWarning("missing path, please fix: " + base.gameObject.name);
				}
				string[] stringTiles = spriteClip.stringTiles;
				foreach (string text in stringTiles)
				{
					if (text == string.Empty || text == null)
					{
						Debug.LogWarning("empty asset in path, please fix: " + base.gameObject.name);
					}
					if (text.Contains(".png"))
					{
						Debug.LogWarning("asset contains .png in path, please remove: " + text + ", " + base.gameObject.name);
					}
				}
			}
		}
		SpriteAnimation[] array2 = animations;
		foreach (SpriteAnimation spriteAnimation2 in array2)
		{
			if (spriteAnimation2.preload)
			{
				spriteAnimation2.Preload();
			}
		}
		m_MeshGen = new SpriteMeshGenerator(GetComponent<MeshFilter>());
		this.clipchanged = (ClipChangedEventHandler)Delegate.Combine(this.clipchanged, new ClipChangedEventHandler(m_MeshGen.Generate));
		if (manager != null)
		{
			manager.Attach(this);
			if (m_ExtraSprite != null)
			{
				manager.AttachExtra(m_ExtraSprite);
			}
		}
		if (defaultOnStart)
		{
			m_CurrentAnim = defaultAnimation;
			if (current != null)
			{
				PlayInternal();
			}
		}
		MeshRenderer component = GetComponent<MeshRenderer>();
		if (!sharedMaterial)
		{
			for (int m = 0; m < component.materials.Length; m++)
			{
				component.materials[m] = new Material(component.materials[m]);
				component.materials[m].mainTexture = null;
			}
		}
		m_MeshRenderer = component;
	}

	private void Update()
	{
		if (StartOfGameDelay.Instance != null)
		{
			m_MyPos = m_MyTransform.position;
			m_MyTransform.position = m_MyPos;
		}
		if (current == null || !current.loaded || m_Paused || (!base.gameObject.GetComponent<Renderer>().isVisible && !sharedMaterial && StartOfGameDelay.Instance == null))
		{
			return;
		}
		float num = current.Update(this, Time.deltaTime);
		if (num > 0f)
		{
			AnimationEnd(current);
			m_CurrentAnim = defaultAnimation;
			if (current == null)
			{
				return;
			}
			PlayInternal();
		}
		if (current.loaded)
		{
			if (sharedMaterial)
			{
				current.Apply(this, m_MeshRenderer.sharedMaterials);
			}
			else
			{
				current.Apply(this, m_MeshRenderer.materials);
			}
		}
	}

	public void ClipApplied(SpriteClip aNewClip)
	{
		if (aNewClip != m_CurrentClip)
		{
			if (this.clipchanged != null)
			{
				this.clipchanged(this, new ClipChangedEventArgs(m_CurrentClip, aNewClip));
			}
			m_CurrentClip = aNewClip;
		}
	}

	public void FrameChanged(string aName, int aFrame)
	{
		if (this.framechanged != null)
		{
			this.framechanged(this, new FrameChangedEventArgs(aName, aFrame));
		}
	}

	public void AnimationEnd(SpriteAnimation aAnimation)
	{
		if (this.animationend != null)
		{
			this.animationend(this, new AnimationChangedEventArgs(aAnimation));
		}
	}

	public void AnimationStart(SpriteAnimation aAnimation)
	{
		if (this.animationstart != null)
		{
			this.animationstart(this, new AnimationChangedEventArgs(aAnimation));
		}
	}

	public void AnimationFetched(SpriteAnimation aAnimation)
	{
		if (this.animationfetched != null)
		{
			this.animationfetched(this, new AnimationChangedEventArgs(aAnimation));
		}
	}

	public void Reset()
	{
		m_CurrentAnim = defaultAnimation;
	}

	public SpriteAnimation GetExtraAnimation(string aName)
	{
		if (extraSprite != null)
		{
			return extraSprite.GetAnimation(aName);
		}
		return null;
	}

	public SpriteAnimation GetAnimation(string aName)
	{
		for (int i = 0; i < animations.Length; i++)
		{
			if (aName == animations[i].name)
			{
				return animations[i];
			}
		}
		return null;
	}

	public void GoToLastFrame()
	{
		if (current != null)
		{
			current.GoToLastFrame();
			MeshRenderer component = GetComponent<MeshRenderer>();
			if (sharedMaterial)
			{
				current.Apply(this, component.sharedMaterials);
			}
			else
			{
				current.Apply(this, component.materials);
			}
		}
	}

	public int GetCurrAnimTotalFrames()
	{
		return current.GetTotalNumFrame();
	}

	public bool Contains(string aName)
	{
		return GetAnimation(aName) != null;
	}

	private void PlayInternal()
	{
		current.Reset();
		m_Paused = false;
		AnimationStart(current);
	}

	public void Pause(bool aPause)
	{
		m_Paused = aPause;
	}

	public bool Play(string aName)
	{
		for (int i = 0; i < animations.Length; i++)
		{
			if (aName == animations[i].name)
			{
				if (current != null)
				{
					AnimationEnd(current);
				}
				m_CurrentAnim = i;
				PlayInternal();
				return true;
			}
		}
		return false;
	}

	public bool Play(int index)
	{
		if (index < animations.Length)
		{
			if (current != null)
			{
				AnimationEnd(current);
			}
			m_CurrentAnim = index;
			PlayInternal();
			return true;
		}
		return false;
	}

	public bool Prefetch(string aName)
	{
		for (int i = 0; i < animations.Length; i++)
		{
			if (aName == animations[i].name && manager != null)
			{
				manager.PrefetchAnimation(this, animations[i]);
				return true;
			}
		}
		return false;
	}

	public string AnimationPlaying()
	{
		return current.name;
	}

	public int CurrentAnimation()
	{
		return m_CurrentAnim;
	}

	public void Seek(int aFrame)
	{
		if (current == null)
		{
			return;
		}
		current.Seek(aFrame);
		if (current.loaded)
		{
			MeshRenderer component = GetComponent<MeshRenderer>();
			if (sharedMaterial)
			{
				current.Apply(this, component.sharedMaterials);
			}
			else
			{
				current.Apply(this, component.materials);
			}
		}
	}

	public void SetIgnore(bool[] aIgnore)
	{
		SpriteAnimation[] array = animations;
		foreach (SpriteAnimation spriteAnimation in array)
		{
			spriteAnimation.SetIgnore(aIgnore);
		}
	}

	public void MergeInto(SpriteManager other)
	{
		if (!(other == null))
		{
			MergeInto(other.animations);
		}
	}

	public void MergeInto(SpriteAnimation[] other)
	{
		if (other == null)
		{
			return;
		}
		SpriteAnimation[] array = animations;
		animations = new SpriteAnimation[((array != null) ? array.Length : 0) + ((other != null) ? other.Length : 0)];
		int num = 0;
		if (array != null)
		{
			SpriteAnimation[] array2 = array;
			foreach (SpriteAnimation spriteAnimation in array2)
			{
				animations[num++] = spriteAnimation;
			}
		}
		if (other == null)
		{
			return;
		}
		foreach (SpriteAnimation spriteAnimation2 in other)
		{
			if (spriteAnimation2.extra != null)
			{
				spriteAnimation2.extra.sprite = this;
			}
			animations[num++] = spriteAnimation2;
		}
	}

	public void MergeInto(SpriteAnimation other)
	{
		if (other != null)
		{
			MergeInto(new SpriteAnimation[1] { other });
		}
	}
}
