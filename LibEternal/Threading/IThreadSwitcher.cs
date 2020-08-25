using LibEternal.JetBrains.Annotations;
using System.Runtime.CompilerServices;
#pragma warning disable 1591

namespace LibEternal.Threading
{
	/// <summary>
	///     Defines an object that switches to a thread.
	/// </summary>
	[PublicAPI]
	public interface IThreadSwitcher : INotifyCompletion
	{
		bool IsCompleted { get; }

		IThreadSwitcher GetAwaiter();

		void GetResult();
	}
}