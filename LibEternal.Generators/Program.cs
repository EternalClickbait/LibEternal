using LibEternal.JetBrains.Annotations;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LibEternal.Generators
{
	internal static class Program
	{
		private static void Main()
		{
			//Set up the logger
			Log.Logger = new LoggerConfiguration()
				.WriteTo.Console(theme: AnsiConsoleTheme.Literate)
				.MinimumLevel.Verbose()
				.CreateLogger();

			List<ICodeGenerator> codeGenerators = GetAllGenerators();
			Log.Information("List of generators: {Generators}", codeGenerators);

			RunAllGenerators(codeGenerators);
			Log.Information("Code generation complete!");

			//Finish up
			Log.CloseAndFlush();
			Console.ReadKey();
		}

		private static void RunAllGenerators([NotNull] IReadOnlyList<ICodeGenerator> generators)
		{
			Log.Information("");
			for (int i = 0; i < generators.Count; i++)
			{
				ICodeGenerator generator = generators[i];
				Log.Debug("Running {Generator} code generator", generator);

				string output = generator.GenerateOutput();
				Log.Information("Output for {Generator} was:\n {Output}", generator, output);
				Log.Information("");
			}
		}

		[Pure]
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
					Log.Verbose("Skipping abstract type {@type}", type);
					continue;
				}

				if (type.IsInterface)
				{
					Log.Verbose("Skipping interface type {@type}", type);
					continue;
				}

				const BindingFlags constructorFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
				ICodeGenerator instance = null;
				if (type.IsValueType)
				{
					Log.Verbose("Type {@type} is value type", type);
					try
					{
						object obj = Activator.CreateInstance(type, true);
						instance = (ICodeGenerator) obj;
					}
					catch (Exception e)
					{
						Log.Debug(e, "Error instantiating value type {@type}", type);
					}
				}
				else
				{
					Log.Verbose("Type {@type} is reference type", type);

					//Thanks stackoverflow (https://stackoverflow.com/q/4681031)
					//Gets the first constructor that takes no parameters or has all optional parameters
					ConstructorInfo ctor = type.GetConstructors(constructorFlags)
						.OrderBy(x => x.GetParameters().Length - x.GetParameters().Count(p => p.IsOptional))
						.FirstOrDefault();

					if (ctor is null)
					{
						Log.Debug("Reference type {@type} does not have an empty constructor", type);
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
						Log.Debug(e, "Error instantiating reference type {@type}", type);
					}
				}

				//Only add the generator if we successfully created one
				if (instance != null) generators.Add(instance);
			}

			return generators;
		}
	}
}