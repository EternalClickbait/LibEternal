using LibEternal.JetBrains.Annotations;
using LibEternal.Threading;
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
			ThreadNames.TryGetValue(thread, out string s);
			return s;
		}
		
		public static void SetThreadName(this Thread thread, string name) => ThreadNames[thread] = name;
		
		/// <summary>
		///     An <see cref="IThreadSwitcher" /> that switches to a <see cref="ThreadPool" /> <see cref="Thread" />
		/// </summary>
		internal static IThreadSwitcher ThreadPoolSwitcher = new ThreadSwitcherTask();

		/// <summary>
		///     An <see cref="IThreadSwitcher" /> that switches to the main <see cref="Thread" />
		/// </summary>
		internal static IThreadSwitcher MainThreadSwitcher = null;

		/// <summary>
		///     Sets the <see cref="MainThreadSwitcher" /> used to switch to the main <see cref="Thread" />. Will only work if it has not been set already
		/// </summary>
		/// <param name="switcher"></param>
		/// <returns>True if the <see cref="MainThreadSwitcher" /> was set successfully.</returns>
		public static bool SetMainThreadSwitcher(IThreadSwitcher switcher)
		{
			//Don't set it if it's already been set
			if (MainThreadSwitcher != null) return false;
			MainThreadSwitcher = switcher;
			return true;
		}

		/// <summary>
		///     Switches to the Task thread.
		/// </summary>
		/// <returns></returns>
		[Pure]
		public static IThreadSwitcher ResumeTaskAsync()
		{
			return ThreadPoolSwitcher;
		}

		/// <summary>
		///     Switch to the main thread.
		/// </summary>
		/// <returns></returns>
		[Pure]
		public static IThreadSwitcher ResumeMainThreadAsync()
		{
			return MainThreadSwitcher;
		}
	}
}