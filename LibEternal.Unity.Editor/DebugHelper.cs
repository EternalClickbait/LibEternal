using LibEternal.JetBrains.Annotations;
using System;
using System.Reflection;

namespace LibEternal.Unity.Editor
{
	[PublicAPI]
	public static class DebugHelper
	{
		private static readonly MethodInfo ClearMethod = Type.GetType("UnityEditor.LogEntries, UnityEditor.dll")
			//Get the method
			?.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public);

		/// <summary>
		///     Clears the unity console. Exactly the same as clicking the 'clear' button in the editor
		/// </summary>
		public static void ClearConsole()
		{
			ClearMethod?.Invoke(null, null);
		}
	}
}