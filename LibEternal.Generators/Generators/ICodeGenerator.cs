using LibEternal.JetBrains.Annotations;
using System.Collections.Generic;

namespace LibEternal.Generators
{
	public interface ICodeGenerator
	{
		[Pure, CanBeNull]
		IList<CodeGeneratorOutput> GenerateOutput();
	}
}