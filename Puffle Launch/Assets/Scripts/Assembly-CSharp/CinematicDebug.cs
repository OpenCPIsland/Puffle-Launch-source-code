using System.Diagnostics;
using UnityEngine;
using UnityDebug = UnityEngine.Debug;

public class CinematicDebug
{
	[Conditional("ENABLE_CINEMETIC_DEBUG")]
	public static void Log(string message)
	{
		UnityDebug.Log(message);
	}

	[Conditional("ENABLE_CINEMETIC_DEBUG")]
	public static void LogWarning(string message)
	{
		UnityDebug.LogWarning(message);
	}
}
