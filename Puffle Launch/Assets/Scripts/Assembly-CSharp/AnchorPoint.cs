using UnityEngine;

public class AnchorPoint : MonoBehaviour
{
	public Transform anchorObject;

	public TextAnchor anchorPoint;

	private void Start()
	{
		Bounds bounds = new Bounds(anchorObject.position, Vector3.zero);
		Bounds bounds2 = new Bounds(base.transform.position, Vector3.zero);
		if ((bool)anchorObject.GetComponent<Renderer>())
		{
			bounds = anchorObject.GetComponent<Renderer>().bounds;
		}
		if ((bool)base.GetComponent<Renderer>())
		{
			bounds2 = base.GetComponent<Renderer>().bounds;
		}
		Vector3 localPosition = new Vector3(0f, 0f, base.transform.localPosition.z);
		switch (anchorPoint)
		{
		case TextAnchor.UpperLeft:
			localPosition.x = bounds.min.x - bounds2.min.x;
			localPosition.y = bounds.max.y - bounds2.max.y;
			break;
		case TextAnchor.UpperCenter:
			localPosition.x = bounds.center.x - bounds2.center.x;
			localPosition.y = bounds.max.y - bounds2.max.y;
			break;
		case TextAnchor.UpperRight:
			localPosition.x = bounds.max.x - bounds2.max.x;
			localPosition.y = bounds.max.y - bounds2.max.y;
			break;
		case TextAnchor.MiddleLeft:
			localPosition.x = bounds.min.x - bounds2.min.x;
			localPosition.y = bounds.center.y - bounds2.center.y;
			break;
		case TextAnchor.MiddleCenter:
			localPosition.x = bounds.center.x - bounds2.center.x;
			localPosition.y = bounds.center.y - bounds2.center.y;
			break;
		case TextAnchor.MiddleRight:
			localPosition.x = bounds.max.x - bounds2.max.x;
			localPosition.y = bounds.center.y - bounds2.center.y;
			break;
		case TextAnchor.LowerLeft:
			localPosition.x = bounds.min.x - bounds2.min.x;
			localPosition.y = bounds.min.y - bounds2.min.y;
			break;
		case TextAnchor.LowerCenter:
			localPosition.x = bounds.center.x - bounds2.center.x;
			localPosition.y = bounds.min.y - bounds2.min.y;
			break;
		case TextAnchor.LowerRight:
			localPosition.x = bounds.max.x - bounds2.max.x;
			localPosition.y = bounds.min.y - bounds2.min.y;
			break;
		}
		base.transform.localPosition = localPosition;
	}
}
