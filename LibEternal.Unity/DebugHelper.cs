extern alias Unity;
using LibEternal.JetBrains.Annotations;
using System;
using System.Reflection;

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
		public static void ClearConsole()
		{
#if UNITY_EDITOR
			ClearMethod?.Invoke(null, null);
#endif
		}
	}
}