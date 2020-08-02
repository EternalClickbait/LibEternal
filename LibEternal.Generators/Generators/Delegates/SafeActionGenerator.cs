using LibEternal.JetBrains.Annotations;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;

namespace LibEternal.Generators.Delegates
{
	public class SafeActionGenerator : ICodeGenerator
	{
		public const byte MaxGenericTypeArgs = 3;

		[NotNull]
		public IList<CodeGeneratorOutput> GenerateOutput()
		{
			List<CodeGeneratorOutput> outputs = new List<CodeGeneratorOutput>();

			for (int i = 0; i <= MaxGenericTypeArgs; i++)
			{
				outputs.Add(GenerateSafeAction(i));
			}

			return outputs;
		}

		private static CodeGeneratorOutput GenerateSafeAction(int numTypeArgs)
		{
			string fileName = $"SafeAction`{numTypeArgs}";
			string fullPath = Path.Combine(Paths.DelegatePath, fileName);


			return null;
		}
	}
}