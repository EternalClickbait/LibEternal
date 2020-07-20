using LibEternal.Helper;
using System;
using System.IO;
using UnityEngine;

namespace LibEternal.Unity.Editor
{
	/// <summary>
	///     A wrapper for displaying a <see cref="System.IO.FileInfo" /> in a unity inspector
	/// </summary>
	[Serializable]
	public class FileInfoWrapper
	{
		[NonSerialized] public Cached<FileInfo> CachedFileInfo;
		[HideInInspector] [SerializeField] public string filePath;

		public FileInfoWrapper()
		{
			CachedFileInfo = new Cached<FileInfo>(GetUpdatedFileInfo, null);
		}

		private FileInfo GetUpdatedFileInfo() => new FileInfo(filePath);
	}
}