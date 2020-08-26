using Serilog;
using System.Threading;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace LibEternal.Threading
{
	internal static class UnityThread
	{
		public static SynchronizationContext Context { get; private set; }

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		internal static void Capture()
		{
			//Save the context as soon as unity loads
			Context = SynchronizationContext.Current;
			Log.Information("Captured Unity Context");
			//Now assign the IThreadSwitcher in ThreadHelper, otherwise we'll get NullReferenceExceptions
			ThreadHelper.SetMainThreadSwitcher(new ThreadSwitcherUnity());
		}
	}
}