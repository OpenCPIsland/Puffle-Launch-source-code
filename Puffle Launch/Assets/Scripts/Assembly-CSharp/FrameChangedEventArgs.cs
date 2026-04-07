using System;

public class FrameChangedEventArgs : EventArgs
{
	public string name;

	public int frame;

	public FrameChangedEventArgs(string n, int f)
	{
		name = n;
		frame = f;
	}
}
