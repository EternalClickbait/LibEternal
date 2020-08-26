using LibEternal.Threading;
using UnityEditor;

namespace LibEternal.Unity.Editor
{
	internal static class UnityThreadContextCapturer
	{
		//Just here so that the context gets capture when in the editor
		[InitializeOnLoadMethod]
		internal static void Capture()
		{
			UnityThread.Capture();
		}
	}
}