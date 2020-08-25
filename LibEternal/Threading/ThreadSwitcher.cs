using LibEternal.JetBrains.Annotations;
using System.Threading;

// ReSharper disable once CheckNamespace
namespace LibEternal.Threading
{
	/// <summary>
	///     Switches to a particular thread.
	/// </summary>
	/// <example>
	/// <code>
	///private static async Task DoWork(CancellationToken token)
	///{
	///    token.ThrowIfCancellationRequested();
	///    var gameObjects = new List&lt;GameObject&gt;();
	///    await ThreadSwitcher.ResumeMainThreadAsync();
	///    for (var i = 0; i &lt; 25; i++)
	///    {
	///        if (token.IsCancellationRequested)
	///            token.ThrowIfCancellationRequested();
	///        await Task.Delay(125, token);
	///        var gameObject = new GameObject(i.ToString());
	///        gameObjects.Add(gameObject);
	///    }
	///}</code>
	/// </example>
	[PublicAPI]
	public static class ThreadSwitcher
	{
		
	}
}