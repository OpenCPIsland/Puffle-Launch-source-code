using System;
using UnityEngine;

[Serializable]
public class CSpriteFrame
{
	public Rect uvs;

	public Rect uvsSmall;

	public Rect uvsLarge;

	public Vector2 scaleFactor = new Vector2(0.5f, 0.5f);

	public Vector2 scaleFactorSmall = new Vector2(0.5f, 0.5f);

	public Vector2 scaleFactorLarge = new Vector2(0.5f, 0.5f);

	public Vector2 topLeftOffset = new Vector2(-1f, 1f);

	public Vector2 topLeftOffsetSmall = new Vector2(-1f, 1f);

	public Vector2 topLeftOffsetLarge = new Vector2(-1f, 1f);

	public Vector2 bottomRightOffset = new Vector2(1f, -1f);

	public Vector2 bottomRightOffsetSmall = new Vector2(1f, -1f);

	public Vector2 bottomRightOffsetLarge = new Vector2(1f, -1f);

	public CSpriteFrame()
	{
	}

	public CSpriteFrame(CSpriteFrame f)
	{
		Copy(f);
	}

	public CSpriteFrame(SPRITE_FRAME f)
	{
		Copy(f);
	}

	public void Copy(SPRITE_FRAME f)
	{
		uvs = f.uvs;
		scaleFactor = f.scaleFactor;
		topLeftOffset = f.topLeftOffset;
		bottomRightOffset = f.bottomRightOffset;
	}

	public void Copy(CSpriteFrame f)
	{
		uvs = f.uvs;
		scaleFactor = f.scaleFactor;
		topLeftOffset = f.topLeftOffset;
		bottomRightOffset = f.bottomRightOffset;
	}

	public void CopyToSmall(SPRITE_FRAME f)
	{
		uvsSmall = f.uvs;
		scaleFactorSmall = f.scaleFactor;
		topLeftOffsetSmall = f.topLeftOffset;
		bottomRightOffsetSmall = f.bottomRightOffset;
	}

	public void CopyFromSmall()
	{
		uvs = uvsSmall;
		scaleFactor = scaleFactorSmall;
		topLeftOffset = topLeftOffsetSmall;
		bottomRightOffset = bottomRightOffsetSmall;
	}

	public void CopyToLarge(SPRITE_FRAME f)
	{
		uvsLarge = f.uvs;
		scaleFactorLarge = f.scaleFactor;
		topLeftOffsetLarge = f.topLeftOffset;
		bottomRightOffsetLarge = f.bottomRightOffset;
	}

	public void CopyFromLarge()
	{
		uvs = uvsLarge;
		scaleFactor = scaleFactorLarge;
		topLeftOffset = topLeftOffsetLarge;
		bottomRightOffset = bottomRightOffsetLarge;
	}

	public SPRITE_FRAME ToStruct()
	{
		SPRITE_FRAME result = default(SPRITE_FRAME);
		result.uvs = uvs;
		result.scaleFactor = scaleFactor;
		result.topLeftOffset = topLeftOffset;
		result.bottomRightOffset = bottomRightOffset;
		return result;
	}
}
