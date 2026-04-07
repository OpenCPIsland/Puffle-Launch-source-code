using UnityEngine;

public class SpriteMeshGenerator
{
	private MeshFilter m_MeshFilter;

	private Vector3[] m_Vertices;

	private Vector3[] m_Normals;

	private Vector2[] m_TextureCoordinates;

	private int[] m_Triangles;

	private bool m_AdjustZoom;

	private float m_OrthographicSize;

	public SpriteMeshGenerator(MeshFilter mf)
	{
		m_MeshFilter = mf;
		m_Vertices = new Vector3[4];
		m_Normals = new Vector3[4];
		m_TextureCoordinates = new Vector2[4];
		m_Triangles = new int[6] { 0, 1, 2, 2, 3, 0 };
		m_TextureCoordinates[0].x = 0f;
		m_TextureCoordinates[0].y = 0f;
		m_TextureCoordinates[1].x = 0f;
		m_TextureCoordinates[1].y = 1f;
		m_TextureCoordinates[2].x = 1f;
		m_TextureCoordinates[2].y = 1f;
		m_TextureCoordinates[3].x = 1f;
		m_TextureCoordinates[3].y = 0f;
		Vector3 vector = Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, -1f)) - Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));
		m_Normals[0] = vector;
		m_Normals[1] = vector;
		m_Normals[2] = vector;
		m_Normals[3] = vector;
		CameraFollow component = Camera.main.GetComponent<CameraFollow>();
		if ((bool)component)
		{
			m_AdjustZoom = true;
			m_OrthographicSize = component.OriginalOrthographicSize;
			if (m_OrthographicSize == 0f)
			{
				m_OrthographicSize = Camera.main.orthographicSize;
			}
		}
	}

	public void Generate(Vector2 aOffset, Vector2 aSize, bool aShared)
	{
		Vector3 vector = Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));
		m_Vertices[0] = Camera.main.ScreenToWorldPoint(new Vector3(aOffset.x, aOffset.y, 0f)) - vector;
		m_Vertices[1] = Camera.main.ScreenToWorldPoint(new Vector3(aOffset.x, aOffset.y + aSize.y, 0f)) - vector;
		m_Vertices[2] = Camera.main.ScreenToWorldPoint(new Vector3(aOffset.x + aSize.x, aOffset.y + aSize.y, 0f)) - vector;
		m_Vertices[3] = Camera.main.ScreenToWorldPoint(new Vector3(aOffset.x + aSize.x, aOffset.y, 0f)) - vector;
		if (m_AdjustZoom)
		{
			float num = m_OrthographicSize / Camera.main.orthographicSize;
			for (int i = 0; i < m_Vertices.Length; i++)
			{
				m_Vertices[i] *= num;
			}
		}
		Mesh mesh = new Mesh();
		if (aShared)
		{
			m_MeshFilter.sharedMesh = mesh;
		}
		else
		{
			m_MeshFilter.mesh = mesh;
		}
		mesh.vertices = m_Vertices;
		mesh.normals = m_Normals;
		mesh.uv = m_TextureCoordinates;
		mesh.triangles = m_Triangles;
	}

	public void Generate(object sender, ClipChangedEventArgs e)
	{
		if (e.current == null)
		{
			m_MeshFilter.mesh = null;
			return;
		}
		Vector2 offset = e.current.offset;
		Vector2 aSize = new Vector2(e.current.stride.x - 1f, e.current.stride.y - 1f);
		Generate(offset, aSize, ((SpriteManager)sender).sharedMaterial);
	}
}
