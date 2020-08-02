namespace LibEternal.Generators
{
	public class CodeGeneratorOutput
	{
		public readonly string Output;
		public readonly string RelativeOutputPath;

		public CodeGeneratorOutput(string output, string relativeOutputPath)
		{
			Output = output;
			RelativeOutputPath = relativeOutputPath;
		}

		public override string ToString()
		{
			return $"{RelativeOutputPath}:{Output}";
		}
	}
}