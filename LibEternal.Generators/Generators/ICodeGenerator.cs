using LibEternal.JetBrains.Annotations;

namespace LibEternal.Generators
{
	public interface ICodeGenerator
	{
		[Pure, CanBeNull]
		string GenerateOutput();
	}
}