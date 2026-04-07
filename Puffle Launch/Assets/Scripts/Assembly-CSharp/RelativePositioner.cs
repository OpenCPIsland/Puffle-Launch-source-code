using UnityEngine;

public class RelativePositioner : MonoBehaviour
{
	public Vector3 rootOffset;

	public Transform[] children;

	public Vector3[] childrenOffsets;

	public void Start()
	{
		if (children.Length != childrenOffsets.Length)
		{
			Debug.LogWarning("Children array size mismatch");
		}
		for (int i = 0; i < children.Length; i++)
		{
			float z = children[i].localPosition.z;
			Vector3 vector = (childrenOffsets[i] + rootOffset) * ScaleItem.Instance.LevelScale + Vector3.forward * z;
			children[i].localPosition = vector / base.transform.localScale.x;
		}
	}
}
