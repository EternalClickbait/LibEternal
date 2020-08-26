using LibEternal.JetBrains.Annotations;
using System.Collections.Generic;
using System.Threading;

namespace LibEternal.Threading
{
	/// <summary>
	///     An extension class for <see cref="Thread" />s
	/// </summary>
	[PublicAPI]
	public static class ThreadHelper
	{
		private static readonly Dictionary<Thread, string> ThreadNames = new Dictionary<Thread, string>();

		/// <summary>
		///     An <see cref="IThreadSwitcher" /> that switches to a <see cref="ThreadPool" /> <see cref="Thread" />
		/// </summary>
		private static readonly IThreadSwitcher ThreadPoolSwitcher = new ThreadSwitcherTask();

		/// <summary>
		///     An <see cref="IThreadSwitcher" /> that switches to the main <see cref="Thread" />
		/// </summary>
		// ReSharper disable once InconsistentNaming
		private static IThreadSwitcher MainThreadSwitcher = null;

		/// <summary>
		///     Gets the modifiable <see cref="Thread" /> name.
		/// </summary>
		/// <param name="thread">The <see cref="Thread" /> whose name to get</param>
		/// <returns>A <see cref="string" /> representing the <see cref="Thread" />'s name</returns>
		[Pure]
		[MustUseReturnValue]
		public static string GetThreadName(this Thread thread)
		{
			ThreadNames.TryGetValue(thread, out string s);
			return s;
		}

		/// <summary>
		///     Sets the <paramref name="thread" />'s name. Bear in mind this does not change the actual <see cref="Thread" />'s <see cref="Thread.Name" />
		///     property, but the value returned by <see cref="GetThreadName" />
		/// </summary>
		/// <param name="thread">The <see cref="Thread" /> whose name to set</param>
		/// <param name="name">The name to set for the <paramref name="thread" /></param>
		public static void SetThreadName(this Thread thread, string name)
		{
			ThreadNames[thread] = name;
		}

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