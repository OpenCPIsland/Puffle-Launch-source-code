using System.Collections;
using UnityEngine;

public class CustomGUI3DItem : MonoBehaviour
{
	public Transform iPadTransform;

	public bool repositionToRelativeObject;

	public GameObject relativeObject;

	private void Start()
	{
		InitPosition();
	}

	public virtual void InitPosition()
	{
		if (repositionToRelativeObject && relativeObject != null && relativeObject.GetComponent<Renderer>() != null)
		{
			StartCoroutine(WaitToRepositionToRelativeObject());
		}
		else if (ResolutionManager.Instance.AssetResolution == ResolutionManager.eAssetResolution.eIPad)
		{
			if (iPadTransform != null)
			{
				base.transform.localPosition = iPadTransform.localPosition;
				return;
			}
			Vector3 localPosition = base.gameObject.transform.localPosition;
			localPosition.x *= 8f / 9f;
			base.transform.localPosition = localPosition;
		}
	}

	private IEnumerator WaitToRepositionToRelativeObject()
	{
		while (relativeObject.GetComponent<Renderer>().bounds.size.y == 0f)
		{
			yield return null;
		}
		RepositionToRelativeObject();
	}

	private void RepositionToRelativeObject()
	{
		Vector3 position = base.gameObject.transform.position;
		position.x = relativeObject.transform.position.x;
		position.y = relativeObject.transform.position.y - relativeObject.GetComponent<Renderer>().bounds.size.y / 2f - 0.5f;
		base.transform.position = position;
	}
}
