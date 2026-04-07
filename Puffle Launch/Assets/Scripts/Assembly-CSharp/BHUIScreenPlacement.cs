using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[ExecuteInEditMode]
[AddComponentMenu("EZ GUI/Utility/EZ Screen Placement")]
public class BHUIScreenPlacement : MonoBehaviour, IUseCamera
{
	public enum HORIZONTAL_ALIGN
	{
		NONE = 0,
		SCREEN_LEFT = 1,
		SCREEN_RIGHT = 2,
		SCREEN_CENTER = 3,
		OBJECT = 4
	}

	public enum VERTICAL_ALIGN
	{
		NONE = 0,
		SCREEN_TOP = 1,
		SCREEN_BOTTOM = 2,
		SCREEN_CENTER = 3,
		OBJECT = 4
	}

	[Serializable]
	public class RelativeTo
	{
		public HORIZONTAL_ALIGN horizontal = HORIZONTAL_ALIGN.SCREEN_LEFT;

		public VERTICAL_ALIGN vertical = VERTICAL_ALIGN.SCREEN_TOP;

		protected BHUIScreenPlacement script;

		public BHUIScreenPlacement Script
		{
			get
			{
				return script;
			}
			set
			{
				Script = value;
			}
		}

		public RelativeTo(BHUIScreenPlacement sp, RelativeTo rt)
		{
			script = sp;
			Copy(rt);
		}

		public RelativeTo(BHUIScreenPlacement sp)
		{
			script = sp;
		}

		public bool Equals(RelativeTo rt)
		{
			if (rt == null)
			{
				return false;
			}
			return horizontal == rt.horizontal && vertical == rt.vertical;
		}

		public void Copy(RelativeTo rt)
		{
			if (rt != null)
			{
				horizontal = rt.horizontal;
				vertical = rt.vertical;
			}
		}
	}

	public Camera renderCamera;

	public Vector3 screenPos = Vector3.forward;

	public Vector3 screenPosSmall;

	public Vector3 screenPosMedium;

	public Vector3 screenPosLarge;

	public bool ignoreZ;

	public RelativeTo relativeTo;

	public Transform relativeObject;

	public bool alwaysRecursive = true;

	public bool allowTransformDrag;

	protected Vector2 screenSize;

	protected bool justEnabled = true;

	protected BHUIScreenPlacementMirror mirror = new BHUIScreenPlacementMirror();

	protected bool m_awake;

	protected bool m_started;

	public Camera RenderCamera
	{
		get
		{
			return renderCamera;
		}
		set
		{
			SetCamera(value);
		}
	}

	public Vector3 ScreenCoord
	{
		get
		{
			return renderCamera.WorldToScreenPoint(base.transform.position);
		}
	}

	public virtual void Awake()
	{
		if (!m_awake)
		{
			m_awake = true;
			SetupScreenPlacement();
			IUseCamera useCamera = (IUseCamera)GetComponent("IUseCamera");
			if (useCamera != null)
			{
				renderCamera = useCamera.RenderCamera;
			}
			if (renderCamera == null)
			{
				renderCamera = Camera.main;
			}
			if (this.relativeTo == null)
			{
				this.relativeTo = new RelativeTo(this);
			}
			else if (this.relativeTo.Script != this)
			{
				RelativeTo relativeTo = new RelativeTo(this, this.relativeTo);
				this.relativeTo = relativeTo;
			}
		}
	}

	protected virtual void SetupScreenPlacement()
	{
		if (relativeTo != null && SizeCategory.Instance != null)
		{
			Vector3 vector = default(Vector3);
			switch (SizeCategory.Instance.CurCategoryId)
			{
			case SizeCategory.CategoryId.eSmall:
				vector = screenPosSmall;
				break;
			default:
				vector = screenPosMedium;
				break;
			case SizeCategory.CategoryId.eLarge:
				vector = screenPosLarge;
				break;
			}
			if (relativeTo.horizontal != HORIZONTAL_ALIGN.NONE)
			{
				screenPos.x = vector.x;
			}
			if (relativeTo.vertical != VERTICAL_ALIGN.NONE)
			{
				screenPos.y = vector.y;
			}
			screenPos.z = vector.z;
		}
	}

	public void Start()
	{
		if (!m_started)
		{
			m_started = true;
			if (renderCamera != null)
			{
				screenSize.x = renderCamera.pixelWidth;
				screenSize.y = renderCamera.pixelHeight;
			}
			PositionOnScreenRecursively();
		}
	}

	public void PositionOnScreenRecursively()
	{
		if (!m_started)
		{
			Start();
		}
		if (relativeObject != null)
		{
			BHUIScreenPlacement bHUIScreenPlacement = relativeObject.GetComponent(typeof(BHUIScreenPlacement)) as BHUIScreenPlacement;
			if (bHUIScreenPlacement != null)
			{
				bHUIScreenPlacement.PositionOnScreenRecursively();
			}
		}
		PositionOnScreen();
	}

	public Vector3 ScreenPosToLocalPos(Vector3 screenPos)
	{
		return base.transform.InverseTransformPoint(ScreenPosToWorldPos(screenPos));
	}

	public Vector3 ScreenPosToParentPos(Vector3 screenPos)
	{
		return ScreenPosToLocalPos(screenPos) + base.transform.localPosition;
	}

	public Vector3 ScreenPosToWorldPos(Vector3 screenPos)
	{
		if (!m_started)
		{
			Start();
		}
		if (renderCamera == null)
		{
			Debug.LogError("Render camera not yet assigned to BHUIScreenPlacement component of \"" + base.name + "\" when attempting to call PositionOnScreen()");
			return base.transform.position;
		}
		Vector3 vector = renderCamera.WorldToScreenPoint(base.transform.position);
		Vector3 position = screenPos;
		switch (relativeTo.horizontal)
		{
		case HORIZONTAL_ALIGN.SCREEN_RIGHT:
			position.x = screenSize.x + position.x;
			break;
		case HORIZONTAL_ALIGN.SCREEN_CENTER:
			position.x = screenSize.x * 0.5f + position.x;
			break;
		case HORIZONTAL_ALIGN.OBJECT:
			if (relativeObject != null)
			{
				position.x = renderCamera.WorldToScreenPoint(relativeObject.position).x + position.x;
			}
			else
			{
				position.x = vector.x;
			}
			break;
		case HORIZONTAL_ALIGN.NONE:
			position.x = vector.x;
			break;
		}
		switch (relativeTo.vertical)
		{
		case VERTICAL_ALIGN.SCREEN_TOP:
			position.y = screenSize.y + position.y;
			break;
		case VERTICAL_ALIGN.SCREEN_CENTER:
			position.y = screenSize.y * 0.5f + position.y;
			break;
		case VERTICAL_ALIGN.OBJECT:
			if (relativeObject != null)
			{
				position.y = renderCamera.WorldToScreenPoint(relativeObject.position).y + position.y;
			}
			else
			{
				position.y = vector.y;
			}
			break;
		case VERTICAL_ALIGN.NONE:
			position.y = vector.y;
			break;
		}
		return renderCamera.ScreenToWorldPoint(position);
	}

	public void PositionOnScreen()
	{
		if (m_awake)
		{
			if (ignoreZ)
			{
				Plane plane = new Plane(renderCamera.transform.forward, renderCamera.transform.position);
				screenPos.z = plane.GetDistanceToPoint(base.transform.position);
			}
			if (ignoreZ)
			{
				Vector3 position = ScreenPosToWorldPos(screenPos);
				position.z = base.transform.position.z;
				base.transform.position = position;
			}
			else
			{
				base.transform.position = ScreenPosToWorldPos(screenPos);
			}
			SendMessage("OnReposition", SendMessageOptions.DontRequireReceiver);
		}
	}

	public void PositionOnScreen(int x, int y, float depth)
	{
		PositionOnScreen(new Vector3(x, y, depth));
	}

	public void PositionOnScreen(Vector3 pos)
	{
		screenPos = pos;
		PositionOnScreen();
	}

	public void SetCamera()
	{
		SetCamera(renderCamera);
	}

	public void SetCamera(Camera c)
	{
		if (!(c == null))
		{
			renderCamera = c;
			screenSize.x = renderCamera.pixelWidth;
			screenSize.y = renderCamera.pixelHeight;
			if (alwaysRecursive || (Application.isEditor && !Application.isPlaying))
			{
				PositionOnScreenRecursively();
			}
			else
			{
				PositionOnScreen();
			}
		}
	}

	public void WorldToScreenPos(Vector3 worldPos)
	{
		if (renderCamera == null)
		{
			return;
		}
		Vector3 vector = renderCamera.WorldToScreenPoint(worldPos);
		switch (relativeTo.horizontal)
		{
		case HORIZONTAL_ALIGN.SCREEN_CENTER:
			screenPos.x = vector.x - renderCamera.pixelWidth / 2f;
			break;
		case HORIZONTAL_ALIGN.SCREEN_LEFT:
			screenPos.x = vector.x;
			break;
		case HORIZONTAL_ALIGN.SCREEN_RIGHT:
			screenPos.x = vector.x - renderCamera.pixelWidth;
			break;
		case HORIZONTAL_ALIGN.OBJECT:
			if (relativeObject != null)
			{
				Vector3 vector2 = renderCamera.WorldToScreenPoint(relativeObject.transform.position);
				screenPos.x = vector.x - vector2.x;
			}
			break;
		}
		switch (relativeTo.vertical)
		{
		case VERTICAL_ALIGN.SCREEN_CENTER:
			screenPos.y = vector.y - renderCamera.pixelHeight / 2f;
			break;
		case VERTICAL_ALIGN.SCREEN_TOP:
			screenPos.y = vector.y - renderCamera.pixelHeight;
			break;
		case VERTICAL_ALIGN.SCREEN_BOTTOM:
			screenPos.y = vector.y;
			break;
		case VERTICAL_ALIGN.OBJECT:
			if (relativeObject != null)
			{
				Vector3 vector3 = renderCamera.WorldToScreenPoint(relativeObject.transform.position);
				screenPos.y = vector.y - vector3.y;
			}
			break;
		}
		screenPos.z = vector.z;
		if (alwaysRecursive)
		{
			PositionOnScreenRecursively();
		}
		else
		{
			PositionOnScreen();
		}
	}

	public static bool TestDepenency(BHUIScreenPlacement sp)
	{
		if (sp.relativeObject == null)
		{
			return true;
		}
		List<BHUIScreenPlacement> list = new List<BHUIScreenPlacement>();
		list.Add(sp);
		BHUIScreenPlacement bHUIScreenPlacement = sp.relativeObject.GetComponent(typeof(BHUIScreenPlacement)) as BHUIScreenPlacement;
		while (bHUIScreenPlacement != null)
		{
			if (list.Contains(bHUIScreenPlacement))
			{
				return false;
			}
			list.Add(bHUIScreenPlacement);
			if (bHUIScreenPlacement.relativeObject == null)
			{
				return true;
			}
			bHUIScreenPlacement = bHUIScreenPlacement.relativeObject.GetComponent(typeof(BHUIScreenPlacement)) as BHUIScreenPlacement;
		}
		return true;
	}

	public virtual void DoMirror()
	{
		if (!Application.isPlaying)
		{
			if (mirror == null)
			{
				mirror = new BHUIScreenPlacementMirror();
				mirror.Mirror(this);
			}
			mirror.Validate(this);
			if (mirror.DidChange(this))
			{
				SetCamera(renderCamera);
				mirror.Mirror(this);
			}
		}
	}

	public virtual void OnDrawGizmosSelected()
	{
		DoMirror();
	}

	public virtual void OnDrawGizmos()
	{
		DoMirror();
	}
}
