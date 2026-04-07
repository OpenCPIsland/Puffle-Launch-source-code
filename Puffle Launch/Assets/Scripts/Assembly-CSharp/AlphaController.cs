using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class AlphaController : MonoBehaviour
{
	public float alphaValue;

	private void Update()
	{
		Color color = base.GetComponent<Renderer>().material.color;
		color.a = alphaValue;
		base.GetComponent<Renderer>().material.color = color;
	}

	public void DeleteSelf()
	{
		Object.DestroyImmediate(base.gameObject);
	}
}
