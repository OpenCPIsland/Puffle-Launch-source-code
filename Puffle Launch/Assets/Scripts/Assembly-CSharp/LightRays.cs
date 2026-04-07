using System;
using UnityEngine;

public class LightRays : MonoBehaviour
{
	public float rotationSpeed;

	public Vector3 scale;

	public Material material;

	public int rayCount = 12;

	public Vector3 offset;

	private Transform mTransform;

	private void Start()
	{
		mTransform = base.transform;
		Vector3[] array = new Vector3[3];
		Vector3[] array2 = new Vector3[3];
		Vector2[] array3 = new Vector2[3];
		int[] array4 = new int[3];
		Vector3 vector = Camera.main.WorldToScreenPoint(mTransform.position);
		float num = vector.magnitude * 1.5f;
		Vector3 vector2 = Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, -1f)) - Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));
		float num2 = (float)Math.PI / (float)(rayCount * 2);
		array[0] = Camera.main.ScreenToWorldPoint(vector);
		array[1] = Camera.main.ScreenToWorldPoint(vector + new Vector3(num * Mathf.Cos(0f - num2), num * Mathf.Sin(0f - num2), 0f));
		array[2] = Camera.main.ScreenToWorldPoint(vector + new Vector3(num * Mathf.Cos(num2), num * Mathf.Sin(num2), 0f));
		array2[0] = vector2;
		array2[1] = vector2;
		array2[2] = vector2;
		array3[0] = Vector2.zero;
		array3[1] = Vector2.zero;
		array3[2] = Vector2.zero;
		array4 = new int[3] { 0, 1, 2 };
		Mesh mesh = new Mesh();
		mesh.vertices = array;
		mesh.normals = array2;
		mesh.uv = array3;
		mesh.triangles = array4;
		GameObject gameObject = new GameObject();
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		gameObject.AddComponent<MeshRenderer>().sharedMaterial = material;
		meshFilter.sharedMesh = mesh;
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = Vector3.zero;
		for (int i = 1; i < rayCount; i++)
		{
			num2 = (float)i * 360f / (float)rayCount;
			GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject);
			gameObject2.transform.parent = base.transform;
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject2.transform.localEulerAngles = new Vector3(0f, 0f, num2);
		}
		mTransform.parent.position = offset;
	}

	private void Update()
	{
		mTransform.parent.localScale = Vector3.one;
		mTransform.localEulerAngles = new Vector3(0f, 0f, Time.time * rotationSpeed);
		mTransform.parent.localScale = scale;
	}
}
