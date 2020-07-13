extern alias Unity;
using System;
using System.Reflection;
using JetBrains.Annotations;

namespace LibEternal.Unity
{
	[PublicAPI]
	public static class DebugHelper
	{
#if UNITY_EDITOR
		private static readonly MethodInfo ClearMethod = Type.GetType("UnityEditor.LogEntries, UnityEditor.dll")
			//Get the method
			?.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public);
#endif
		internal static void ClearConsole()
		{
#if UNITY_EDITOR
			ClearMethod?.Invoke(null, null);
#endif
		}
	}
}