#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class GameViewGizmoSilencer
{
	static GameViewGizmoSilencer()
	{
		EditorApplication.delayCall += DisableGameViewGizmos;
		EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
	}

	private static void OnPlayModeStateChanged(PlayModeStateChange aState)
	{
		if (aState == PlayModeStateChange.EnteredPlayMode || aState == PlayModeStateChange.EnteredEditMode)
		{
			EditorApplication.delayCall += DisableGameViewGizmos;
		}
	}

	private static void DisableGameViewGizmos()
	{
		try
		{
			Type type = Type.GetType("UnityEditor.GameView,UnityEditor");
			if (type == null)
			{
				return;
			}
			UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(type);
			if (array == null || array.Length == 0)
			{
				return;
			}
			EditorWindow editorWindow = array[0] as EditorWindow;
			if (editorWindow == null)
			{
				return;
			}
			PropertyInfo property = type.GetProperty("showGizmos", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (property != null && property.CanWrite)
			{
				property.SetValue(editorWindow, false, null);
				editorWindow.Repaint();
				return;
			}
			FieldInfo field = type.GetField("m_ShowGizmos", BindingFlags.Instance | BindingFlags.NonPublic);
			if (field != null)
			{
				field.SetValue(editorWindow, false);
				editorWindow.Repaint();
			}
		}
		catch
		{
		}
	}
}
#endif
