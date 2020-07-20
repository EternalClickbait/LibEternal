using System;
using UnityEngine;

namespace LibEternal.Unity
{
	[CreateAssetMenu]
	public class ExternalLibraryGroup : ScriptableObject
	{
		[Serializable]
		public class CompiledLibrary
		{
			//This prevents reordering of the fields. I prefer having the source first, then the destination
			// @formatter:off
			[Tooltip("The path to the output of the build (the output .dll). Often in <project path>/bin/Debug/netcoreappXXX.")]
			public FileInfoWrapper sourceLocation;

			[Tooltip("Where the binary should be copied to. Should be somewhere in your assets folder")]
			public FileInfoWrapper assetDestination;
			// @formatter:on
		}

		// @formatter:off
		[Tooltip("The path to the solution to build")]
		public FileInfoWrapper[] solutions;

		[Tooltip("A list of libraries to import")]
		public CompiledLibrary[] libraries;
		// @formatter:on
	}
}