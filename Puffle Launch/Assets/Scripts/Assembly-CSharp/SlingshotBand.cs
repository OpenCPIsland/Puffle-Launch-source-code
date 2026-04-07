using UnityEngine;

public class SlingshotBand : MonoBehaviour
{
	public Transform firstEndpoint;

	public Transform secondEndpoint;

	private Transform mTransform;

	private Vector3 mInitialAngles;

	private float mInitialScale;

	private float mBaseLength;

	public void Start()
	{
		mTransform = base.transform;
		mInitialAngles = mTransform.localEulerAngles;
		mInitialScale = mTransform.localScale.x;
		if (ResolutionManager.Instance.LayoutSize == ResolutionManager.eLayoutSize.eIPad)
		{
			mInitialScale *= 1.25f;
		}
		else if (ResolutionManager.Instance.LayoutSize != ResolutionManager.eLayoutSize.eLowres)
		{
		}
		mBaseLength = (secondEndpoint.position - firstEndpoint.position).magnitude;
	}

	public void Update()
	{
		Vector3 vector = secondEndpoint.position - firstEndpoint.position;
		vector.z = 0f;
		Vector3 localScale = mTransform.localScale;
		localScale.x = mInitialScale * vector.magnitude / mBaseLength;
		mTransform.localScale = localScale;
		float num = Vector3.Angle(Vector3.right, vector);
		if (Vector3.Cross(Vector3.right, vector).z < 0f)
		{
			num *= -1f;
		}
		mTransform.localEulerAngles = mInitialAngles;
		mTransform.RotateAround(Vector3.zero, Vector3.forward, num);
		mTransform.position = firstEndpoint.position + vector * 0.5f;
		mTransform.position += new Vector3(0f, 0f, 0.1f);
	}
}
