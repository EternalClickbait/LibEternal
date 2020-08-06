using System;
using UnityEngine;

namespace LibEternal.Unity
{
	/// <summary>
	///     A <see cref="ScriptableObject" /> that enables external projects/solutions to be built and imported from within the Unity Editor
	/// </summary>
	[CreateAssetMenu]
	public sealed class ExternalLibraryGroup : ScriptableObject
	{
		/// <summary>
		///     A class that represents a compiled library that should be imported into the assets folder
		/// </summary>
		[Serializable]
		public class CompiledLibrary
		{
			//This prevents reordering of the fields. I prefer having the source first, then the destination
			// @formatter:off
			/// <summary>///     The path to the output of the build (the output .dll). Often in &lt;project path&gt;/bin/Debug/netcoreappXXX./// </summary>
			[Tooltip("The path to the output of the build (the output .dll). Often in <project path>/bin/Debug/netcoreappXXX.")]
			public FileInfoWrapper sourceLocation;

			/// <summary>///     Where the binary should be copied to. Should be somewhere in your assets folder./// </summary>
			[Tooltip("Where the binary should be copied to. Should be somewhere in your assets folder")]
			public FileInfoWrapper assetDestination;
			// @formatter:on
		}

		// @formatter:off
		/// <summary>///     The path to the solution to build/// </summary>
		[Tooltip("The path to the solution to build")]
		public FileInfoWrapper[] solutions;

		/// <summary>///     A list of libraries to import/// </summary>
		[Tooltip("A list of libraries to import")]
		public CompiledLibrary[] libraries;
		// @formatter:on
	}
}