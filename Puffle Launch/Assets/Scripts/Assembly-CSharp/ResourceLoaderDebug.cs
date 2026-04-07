using System.Diagnostics;
using UnityEngine;
using UnityDebug = UnityEngine.Debug;

public class ResourceLoaderDebug
{
	[Conditional("ENABLE_RESOUCELOADER_DEBUG")]
	public static void Log(string message)
	{
		UnityDebug.Log(message);
	}

	[Conditional("ENABLE_RESOUCELOADER_DEBUG")]
	public static void LogWarning(string message)
	{
		UnityDebug.LogWarning(message);
	}
}
