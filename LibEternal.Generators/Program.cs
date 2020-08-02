using LibEternal.JetBrains.Annotations;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LibEternal.Generators
{
	internal static class Program
	{
		
		public static void Main()
		{
			//Set up the logger
			Log.Logger = new LoggerConfiguration()
				.WriteTo.Console(theme: AnsiConsoleTheme.Literate)
				.MinimumLevel.Verbose()
				.CreateLogger();

			//Instantiate the generators
			List<ICodeGenerator> codeGenerators = GetAllGenerators();
			Log.Information("List of generators: {Generators}{Newline}", codeGenerators, Environment.NewLine);

			//And run them
			List<CodeGeneratorOutput> outputs = RunAllGenerators(codeGenerators);
			Log.Information("Code generation complete!{Newline}", Environment.NewLine);

			//Now write it all to file
			WriteToFile(outputs);

			//Finish up
			Log.CloseAndFlush();
			Console.ReadKey();
		}

		private static void WriteToFile(List<CodeGeneratorOutput> outputs)
		{
			foreach (CodeGeneratorOutput output in outputs)
			{
				Log.Verbose("Writing output to {RelativeFilePath}", output.RelativeOutputPath);
				try
				{
					FileInfo fileInfo = new FileInfo(output.RelativeOutputPath);

					if (!fileInfo.Exists)
						if (fileInfo.Directory != null)
							Directory.CreateDirectory(fileInfo.Directory.FullName);


					using (StreamWriter writer = File.CreateText(output.RelativeOutputPath))
					{
						writer.Write(output.Output);
					}
				}
				catch (Exception e)
				{
					Log.Warning(e, "Error outputting to file {OutputFilePath}", output.RelativeOutputPath);
				}
			}
		}

		[MustUseReturnValue]
		[NotNull]
		private static List<CodeGeneratorOutput> RunAllGenerators([NotNull] IReadOnlyList<ICodeGenerator> generators)
		{
			List<CodeGeneratorOutput> allOutput = new List<CodeGeneratorOutput>();
			for (int i = 0; i < generators.Count; i++)
			{
				ICodeGenerator generator = generators[i];
				Log.Debug("Running {Generator} code generator ({Index}/{Total})", generator, i + 1, generators.Count);

				//Get the output
				IList<CodeGeneratorOutput> output = generator.GenerateOutput();
				//And add it to the 'superlist'
				if (output != null)
					allOutput.AddRange(output);
				Log.Information("Output for {Generator} was:{Newline} {Output}", generator, Environment.NewLine, output);
				if (i != generators.Count - 1)
					Log.Information("{Newline}", Environment.NewLine);
			}

			return allOutput;
		}

		[MustUseReturnValue]
		[NotNull]
		private static List<ICodeGenerator> GetAllGenerators()
		{
			//Search for a list of all generators
			Assembly thisAssembly = Assembly.GetExecutingAssembly();
			Type generatorInterfaceType = typeof(ICodeGenerator);
			IEnumerable<Type>
				generatorTypes =
					thisAssembly.GetTypes().Where(t => generatorInterfaceType.IsAssignableFrom(t)); //Gets all types that implement 'ICodeGenerator'

			List<ICodeGenerator> generators = new List<ICodeGenerator>();

			//Instantiate them all
			foreach (Type type in generatorTypes)
			{
				//Skip abstract classes and interfaces
				if (type.IsAbstract)
				{
					Log.Verbose("Skipping abstract type {@Type}", type);
					continue;
				}

				if (type.IsInterface)
				{
					Log.Verbose("Skipping interface type {@Type}", type);
					continue;
				}

				const BindingFlags constructorFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
				ICodeGenerator instance = null;
				if (type.IsValueType)
				{
					Log.Verbose("Type {@Type} is value type", type);
					try
					{
						object obj = Activator.CreateInstance(type, true);
						instance = (ICodeGenerator) obj;
					}
					catch (Exception e)
					{
						Log.Debug(e, "Error instantiating value type {@Type}", type);
					}
				}
				else
				{
					Log.Verbose("Type {@Type} is reference type", type);

					//Thanks stackoverflow (https://stackoverflow.com/q/4681031)
					//Gets the first constructor that takes no parameters or has all optional parameters
					ConstructorInfo ctor = type.GetConstructors(constructorFlags)
						.OrderBy(x => x.GetParameters().Length - x.GetParameters().Count(p => p.IsOptional))
						.FirstOrDefault();

					if (ctor is null)
					{
						Log.Debug("Reference type {@Type} does not have an empty constructor", type);
						continue;
					}

					//Try instantiating the object
					try
					{
						object obj = ctor.Invoke(new object[0]);
						instance = (ICodeGenerator) obj;
					}
					catch (Exception e)
					{
						Log.Debug(e, "Error instantiating reference type {@Type}", type);
					}
				}

				//Only add the generator if we successfully created one
				if (instance != null) generators.Add(instance);
			}

			return generators;
		}
	}
}