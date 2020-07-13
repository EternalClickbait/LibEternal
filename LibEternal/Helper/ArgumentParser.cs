using JetBrains.Annotations;
using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace LibEternal.Helper
{
	// ReSharper disable CommentTypo
	/// <summary>
	///     ArgumentParser class
	///     Intelligent handling of command line arguments based on Richard Lopes' (GriffonRL's)
	///     class at http://www.codeproject.com/Articles/3111/C-NET-Command-Line-Arguments-Parser
	///     Supports both linux-style and windows-style parameter arguments.
	/// </summary>
	// ReSharper restore CommentTypo
	public class ArgumentParser
	{
		// Variables
		private readonly StringDictionary parameters;

		public ArgumentParser() : this(Environment.GetCommandLineArgs())
		{
		}

		// Constructor
		public ArgumentParser(string[] args)
		{
			parameters = new StringDictionary();
			Regex splitter = new Regex(@"^-{1,2}|^/|=|:",
				RegexOptions.IgnoreCase | RegexOptions.Compiled);

			Regex remover = new Regex(@"^['""]?(.*?)['""]?$",
				RegexOptions.IgnoreCase | RegexOptions.Compiled);

			string parameter = null;

			// Valid parameters forms:
			// {-,/,--}param{ ,=,:}((",')value(",'))
			// Examples: 
			// -param1 value1 --param2 /param3:"Test-:-work" 
			//   /param4=happy -param5 '--=nice=--'
			for (int i = 0; i < args.Length; i++)
			{
				string txt = args[i];
				// Look for new parameters (-,/ or --) and a
				// possible enclosed value (=,:)
				string[] parts = splitter.Split(txt, 3);

				// ReSharper disable once SwitchStatementMissingSomeCases
				switch (parts.Length)
				{
					// Found a value (for the last parameter 
					// found (space separator))
					case 1:
						if (parameter != null)
						{
							if (!parameters.ContainsKey(parameter))
							{
								parts[0] =
									remover.Replace(parts[0], "$1");

								parameters.Add(parameter, parts[0]);
							}

							parameter = null;
						}

						// else Error: no parameter waiting for a value (skipped)
						break;

					// Found just a parameter
					case 2:
						// The last parameter is still waiting. 
						// With no value, set it to true.
						if (parameter != null)
							if (!parameters.ContainsKey(parameter))
								parameters.Add(parameter, "true");
						parameter = parts[1];
						break;

					// Parameter with enclosed value
					case 3:
						// The last parameter is still waiting. 
						// With no value, set it to true.
						if (parameter != null)
							if (!parameters.ContainsKey(parameter))
								parameters.Add(parameter, "true");

						parameter = parts[1];

						// Remove possible enclosing characters (",')
						if (!parameters.ContainsKey(parameter))
						{
							parts[2] = remover.Replace(parts[2], "$1");
							parameters.Add(parameter, parts[2]);
						}

						parameter = null;
						break;
				}
			}

			// In case a parameter is still waiting
			if (parameter == null) return;
			if (!parameters.ContainsKey(parameter))
				parameters.Add(parameter, "true");
		}

		// Retrieve a parameter value if it exists 
		// (overriding C# indexer property)
		public string this[string param] => parameters[param];

		/// <summary>
		///     Retrieve a parameter value if it exists , otherwise the given value
		/// </summary>
		/// <param name="key">The key to retrieve</param>
		/// <param name="defaultValue">The default value</param>
		/// <returns>A parameter value or the given default value</returns>
		[Pure]
		public string GetOrDefault(string key, string defaultValue)
		{
			return parameters.ContainsKey(key) ? parameters[key] : defaultValue;
		}
	}
}