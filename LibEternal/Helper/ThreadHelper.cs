using System;
using System.Collections.Generic;
using System.Threading;

namespace LibEternal.Helper
{
	/// <summary>
	///     An extension class for <see cref="Thread" />s
	/// </summary>
	public static class ThreadHelper
	{
		private static readonly Dictionary<Thread, string> ThreadNames = new Dictionary<Thread, string>();
		
		public static string GetThreadName(this Thread thread)
		{
			return ThreadNames.GetValueOrDefault(thread);
		}
		
		public static void SetThreadName(this Thread thread, string name) => ThreadNames[thread] = name;
	}
}