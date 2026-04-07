using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class FallZone : MonoBehaviour
{
	private Transform mTransform;

	private Transform mMainCamera;

	private float mHorizontalOffset;

	private float mBaseOrthographicSize;

	private Vector3 mBaseScale;

	public void Start()
	{
		mTransform = base.transform;
		SpriteMeshGenerator spriteMeshGenerator = new SpriteMeshGenerator(GetComponent<MeshFilter>());
		spriteMeshGenerator.Generate(new Vector2(0f, -1024f), new Vector2(Screen.width, 1024f), false);
		Vector3 localScale = mTransform.localScale;
		localScale.y *= ScaleItem.Instance.BillboardScale;
		mTransform.localScale = localScale;
		mBaseScale = localScale;
		Vector3 position = mTransform.position;
		position.y += 40f * Mathf.Sign(mTransform.localScale.y) * ScaleItem.Instance.LevelScale;
		position.z = 0.01f;
		mTransform.position = position;
		Camera main = Camera.main;
		mMainCamera = main.transform;
		mBaseOrthographicSize = main.GetComponent<CameraFollow>().OriginalOrthographicSize;
		mHorizontalOffset = base.GetComponent<Renderer>().bounds.size.x / 2f;
	}

	public void LateUpdate()
	{
		float num = Camera.main.orthographicSize / mBaseOrthographicSize;
		Vector3 position = mTransform.position;
		position.x = mMainCamera.position.x - mHorizontalOffset * num;
		mTransform.position = position;
		mTransform.localScale = mBaseScale * num;
	}
}
