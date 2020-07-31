using LibEternal.JetBrains.Annotations;

namespace LibEternal.Generators.Delegates
{
	public class SafeActionGenerator :ICodeGenerator
	{
		[NotNull]
		public string GenerateOutput()
		{
			return "<OUTPUT>";
		}
	}
}