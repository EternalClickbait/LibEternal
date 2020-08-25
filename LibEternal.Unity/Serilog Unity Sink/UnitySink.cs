using JetBrains.Annotations;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using System;
using System.IO;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Serilog.Sinks.Unity
{
	/// <summary>
	/// An <see cref="ILogEventSink"/> for Unity
	/// </summary>
	[PublicAPI]
	public sealed class UnitySink : ILogEventSink
	{
		private readonly ITextFormatter formatter;

		/// <summary>
		/// Constructs a new <see cref="UnitySink"/>, using the optional <see cref="IFormatProvider"/>
		/// </summary>
		/// <param name="template">The template used to format messages before they are logged</param>
		/// <param name="formatProvider"></param>
		public UnitySink([NotNull] string template, IFormatProvider formatProvider)
		{
			formatter = new MessageTemplateTextFormatter(template, formatProvider);
		}

		/// <inheritdoc />
		public void Emit(LogEvent logEvent)
		{
			using (StringWriter writer = new StringWriter())
			{
				formatter.Format(logEvent, writer);
				string message = writer.ToString();
				
				switch (logEvent.Level)
				{
					case LogEventLevel.Fatal:
					case LogEventLevel.Error:
						Debug.LogError(message);
						break;
					case LogEventLevel.Warning:
						Debug.LogWarning(message);
						break;
					case LogEventLevel.Information:
					case LogEventLevel.Debug:
					case LogEventLevel.Verbose:
						Debug.Log(message);
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(logEvent));
				}
			}
		}
	}
}