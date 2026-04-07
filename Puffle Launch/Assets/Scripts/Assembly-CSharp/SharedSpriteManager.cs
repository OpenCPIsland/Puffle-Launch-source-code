using System.Collections.Generic;
using UnityEngine;

public class SharedSpriteManager : MonoBehaviour
{
	private class SharedSpriteRef
	{
		public SpriteManager sprite;

		public int refCount;
	}

	public string spriteName;

	public SpriteManager sharedSpritePrefab;

	private static Dictionary<string, SharedSpriteRef> smSharedSpriteManagers = new Dictionary<string, SharedSpriteRef>();

	private Transform m_MyTransform;

	private Vector3 m_MyPos;

	public SpriteManager SharedInstance
	{
		get
		{
			return smSharedSpriteManagers[spriteName].sprite;
		}
	}

	public void Awake()
	{
		m_MyTransform = base.transform;
		SharedSpriteRef sharedSpriteRef = null;
		if (smSharedSpriteManagers.ContainsKey(spriteName))
		{
			sharedSpriteRef = smSharedSpriteManagers[spriteName];
		}
		if (sharedSpriteRef == null)
		{
			SpriteManager spriteManager = (SpriteManager)Object.Instantiate(sharedSpritePrefab);
			MeshRenderer component = spriteManager.GetComponent<MeshRenderer>();
			for (int i = 0; i < component.materials.Length; i++)
			{
				component.materials[i] = new Material(component.materials[i]);
				component.materials[i].mainTexture = null;
			}
			sharedSpriteRef = new SharedSpriteRef();
			sharedSpriteRef.sprite = spriteManager;
		}
		sharedSpriteRef.refCount++;
		smSharedSpriteManagers[spriteName] = sharedSpriteRef;
		sharedSpriteRef.sprite.clipchanged += OnClipChanged;
		GetComponent<MeshFilter>().sharedMesh = sharedSpriteRef.sprite.GetComponent<MeshFilter>().sharedMesh;
		GetComponent<MeshRenderer>().sharedMaterials = sharedSpriteRef.sprite.GetComponent<MeshRenderer>().materials;
	}

	private void Update()
	{
		if (StartOfGameDelay.Instance != null)
		{
			m_MyPos = m_MyTransform.position;
			m_MyTransform.position = m_MyPos;
		}
	}

	public void OnClipChanged(object sender, ClipChangedEventArgs e)
	{
		GetComponent<MeshFilter>().sharedMesh = ((SpriteManager)sender).GetComponent<MeshFilter>().sharedMesh;
	}

	public void OnDestroy()
	{
		SharedSpriteRef sharedSpriteRef = smSharedSpriteManagers[spriteName];
		if (--sharedSpriteRef.refCount == 0)
		{
			if ((bool)sharedSpriteRef.sprite)
			{
				Object.Destroy(sharedSpriteRef.sprite.gameObject);
			}
			smSharedSpriteManagers.Remove(spriteName);
		}
	}
}
