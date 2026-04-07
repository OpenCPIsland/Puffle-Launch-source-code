using System;

public class ClipChangedEventArgs : EventArgs
{
	public SpriteClip previous;

	public SpriteClip current;

	public ClipChangedEventArgs(SpriteClip previous, SpriteClip current)
	{
		this.previous = previous;
		this.current = current;
	}
}
