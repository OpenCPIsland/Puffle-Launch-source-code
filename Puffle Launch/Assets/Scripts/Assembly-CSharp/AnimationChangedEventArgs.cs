using System;

public class AnimationChangedEventArgs : EventArgs
{
	public SpriteAnimation anim;

	public AnimationChangedEventArgs(SpriteAnimation a)
	{
		anim = a;
	}
}
