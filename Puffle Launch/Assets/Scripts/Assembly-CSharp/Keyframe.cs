using System;
using UnityEngine;

[Serializable]
public class Keyframe
{
	public int frame;

	public Vector3 offset;

	public float angle;

	public Vector3 scale = Vector3.one;

	public Keyframe(int aFrame, Vector3 aOffset, float aAngle, Vector3 aScale)
	{
		frame = aFrame;
		offset = aOffset;
		angle = aAngle;
		scale = aScale;
	}
}
