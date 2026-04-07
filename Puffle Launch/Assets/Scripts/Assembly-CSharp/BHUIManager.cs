using UnityEngine;

[AddComponentMenu("EZ GUI/Management/UI Manager")]
public class BHUIManager : UIManager
{
	public override void Awake()
	{
		pointerType = POINTER_TYPE.TOUCHPAD;
		base.Awake();
	}
}
