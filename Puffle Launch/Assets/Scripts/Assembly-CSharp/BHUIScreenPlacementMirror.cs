using UnityEngine;

public class BHUIScreenPlacementMirror
{
	public Vector3 worldPos;

	public Vector3 screenPos;

	public BHUIScreenPlacement.RelativeTo relativeTo;

	public Transform relativeObject;

	public Camera renderCamera;

	public Vector2 screenSize;

	public BHUIScreenPlacementMirror()
	{
		relativeTo = new BHUIScreenPlacement.RelativeTo(null);
	}

	public virtual void Mirror(BHUIScreenPlacement sp)
	{
		worldPos = sp.transform.position;
		screenPos = sp.screenPos;
		relativeTo.Copy(sp.relativeTo);
		relativeObject = sp.relativeObject;
		renderCamera = sp.renderCamera;
		screenSize = new Vector2(sp.renderCamera.pixelWidth, sp.renderCamera.pixelHeight);
	}

	public virtual bool Validate(BHUIScreenPlacement sp)
	{
		if (sp.relativeTo.horizontal != BHUIScreenPlacement.HORIZONTAL_ALIGN.OBJECT && sp.relativeTo.vertical != BHUIScreenPlacement.VERTICAL_ALIGN.OBJECT)
		{
			sp.relativeObject = null;
		}
		if (sp.relativeObject != null && !BHUIScreenPlacement.TestDepenency(sp))
		{
			Debug.LogError("ERROR: The Relative Object you recently assigned on \"" + sp.name + "\" which points to \"" + sp.relativeObject.name + "\" would create a circular dependency.  Please check your placement dependencies to resolve this.");
			sp.relativeObject = null;
		}
		return true;
	}

	public virtual bool DidChange(BHUIScreenPlacement sp)
	{
		if (worldPos != sp.transform.position)
		{
			if (sp.allowTransformDrag)
			{
				sp.WorldToScreenPos(sp.transform.position);
			}
			else
			{
				sp.PositionOnScreen();
			}
			return true;
		}
		if (screenPos != sp.screenPos)
		{
			return true;
		}
		if (renderCamera != null && (screenSize.x != sp.renderCamera.pixelWidth || screenSize.y != sp.renderCamera.pixelHeight))
		{
			return true;
		}
		if (!relativeTo.Equals(sp.relativeTo))
		{
			return true;
		}
		if (renderCamera != sp.renderCamera)
		{
			return true;
		}
		if (relativeObject != sp.relativeObject)
		{
			return true;
		}
		return false;
	}
}
