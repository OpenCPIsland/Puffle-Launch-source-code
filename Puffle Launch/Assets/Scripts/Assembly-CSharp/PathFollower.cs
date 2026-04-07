using UnityEngine;

[RequireComponent(typeof(ElasticMovement))]
public class PathFollower : MonoBehaviour
{
	public Vector3[] pathNodes;

	public bool loop;

	public bool reversed;

	private Transform mTransform;

	private ElasticMovement mElasticMovement;

	private int mCurrentNode;

	public int CurrentNode
	{
		get
		{
			return mCurrentNode;
		}
		set
		{
			mCurrentNode = value;
		}
	}

	public void Start()
	{
		mTransform = base.transform;
		mElasticMovement = GetComponent<ElasticMovement>();
		mCurrentNode = (reversed ? (pathNodes.Length - 1) : 0);
		if (pathNodes.Length == 0)
		{
			base.enabled = false;
		}
		else
		{
			mElasticMovement.TargetPosition = pathNodes[mCurrentNode] * ScaleItem.Instance.LevelScale;
		}
	}

	public void FixedUpdate()
	{
		if (!((mTransform.position - pathNodes[mCurrentNode] * ScaleItem.Instance.LevelScale).sqrMagnitude < Mathf.Pow(100f * ScaleItem.Instance.LevelScale, 2f)))
		{
			return;
		}
		if (!reversed)
		{
			if (++mCurrentNode == pathNodes.Length)
			{
				if (!loop)
				{
					base.enabled = false;
					return;
				}
				mCurrentNode = 0;
			}
		}
		else if (--mCurrentNode == -1)
		{
			if (!loop)
			{
				base.enabled = false;
				return;
			}
			mCurrentNode = pathNodes.Length - 1;
		}
		mElasticMovement.TargetPosition = pathNodes[mCurrentNode] * ScaleItem.Instance.LevelScale;
	}
}
