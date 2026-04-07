using UnityEngine;

public class HitDebris : MonoBehaviour
{
	public void OnDestroy()
	{
		base.transform.parent.GetComponent<HitFX>().OnEmitterEnd();
	}
}
