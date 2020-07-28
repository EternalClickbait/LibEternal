using LibEternal.Helper;
using LibEternal.JetBrains.Annotations;
using System;
using System.IO;
using UnityEngine;

namespace LibEternal.Unity
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

		[NotNull]
		private FileInfo GetUpdatedFileInfo()
		{
			return new FileInfo(filePath);
		}

		[NotNull]
		public override string ToString()
		{
			return CachedFileInfo.Value?.FullName ?? "";
		}
	}
}