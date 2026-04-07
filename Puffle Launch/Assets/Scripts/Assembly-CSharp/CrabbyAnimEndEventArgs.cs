using System;

public class CrabbyAnimEndEventArgs : EventArgs
{
	public CrabbyAnimController.CrabbyAnim anim;

	public CrabbyAnimEndEventArgs(CrabbyAnimController.CrabbyAnim aAnim)
	{
		anim = aAnim;
	}
}
