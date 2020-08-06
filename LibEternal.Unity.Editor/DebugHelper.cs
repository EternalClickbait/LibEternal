using LibEternal.JetBrains.Annotations;
using System;
using System.Reflection;

namespace LibEternal.Unity.Editor
{
	/// <summary>
	///     A class containing methods to help with debugging in the unity editor
	/// </summary>
	[PublicAPI]
	public static class DebugHelper
	{
		/// <summary>
		///     The <see cref="MethodInfo" /> that represents the built in clear method for the unity console
		/// </summary>
		private static readonly MethodInfo ClearMethod = Type.GetType("UnityEditor.LogEntries, UnityEditor.dll")
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