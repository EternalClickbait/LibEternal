using JetBrains.Annotations;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace LibEternal.Extensions
{
	[UsedImplicitly]
	public static class ProcessExtensions
	{
		/// <summary>
		/// Returns an asynchronous task that returns once a process has exited
		/// </summary>
		/// <param name="process">The process whose exit to wait for</param>
		/// <param name="cancellationToken"></param>
		/// <returns>A completed task once the process has exited</returns>
		public static async Task WaitForExitAsync(this Process process, CancellationToken cancellationToken = default)
		{
			TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

			void ProcessExited(object sender, EventArgs e)
			{
				tcs.TrySetResult(true);
			}

			process.EnableRaisingEvents = true;
			process.Exited += ProcessExited;

			try
			{
				if (process.HasExited) return;

				using (cancellationToken.Register(() => tcs.TrySetCanceled()))
				{
					await tcs.Task.ConfigureAwait(false);
				}
			}
			finally
			{
				process.Exited -= ProcessExited;
			}
		}
	}
}