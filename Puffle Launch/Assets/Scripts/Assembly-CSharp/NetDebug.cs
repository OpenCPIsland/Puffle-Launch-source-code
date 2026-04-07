using System.Diagnostics;
using UnityEngine;
using UnityDebug = UnityEngine.Debug;

public class NetDebug
{
	[Conditional("ENABLE_NET_DEBUG")]
	public static void Log(string message)
	{
		UnityDebug.Log(message);
	}

	[Conditional("ENABLE_NET_DEBUG")]
	public static void LogWarning(string message)
	{
		UnityDebug.LogWarning(message);
	}
}
