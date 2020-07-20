using LibEternal.JetBrains.Annotations;
using System.IO;
using System.Text.RegularExpressions;

namespace LibEternal.Extensions
{
	[PublicAPI]
	public class IoExtensions
	{
		private static readonly Regex InvalidFilePathChars =
			new Regex($"[{Regex.Escape(new string(Path.GetInvalidPathChars()))}]", RegexOptions.Compiled);

		/// <summary>
		///     Returns <see langword="true" /> if the supplied string is a valid file name
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		[Pure]
		public static bool IsValidFilename(string fileName)
		{
			if (string.IsNullOrWhiteSpace(fileName)) return false;
			//From https://stackoverflow.com/a/62855/
			return !InvalidFilePathChars.IsMatch(fileName);

			//TODO: Add other checks for UNC, drive-path format, etc
		}
	}
}