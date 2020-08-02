namespace LibEternal.Generators
{
	internal class Paths
	{
		/// <summary>
		///     Should output to LibEternal directory (The one with the solution)
		///     Goes up 4 levels to the base path (LibEternal/LibEternal.Generators/bin/Debug/netcoreapp3.1)
		/// </summary>
		public const string PathToSolutionDirectory = "../../../../";

		/// <summary>
		/// The path for the delegate files to be output to. These include SafeAction and SafeFunc
		/// </summary>
		public const string DelegatePath = PathToSolutionDirectory + "LibEternal/Callbacks/Generic/";
	}
}