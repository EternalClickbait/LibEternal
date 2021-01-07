using LibEternal.JetBrains.Annotations;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using System;

// ReSharper disable once CheckNamespace
namespace Serilog.Sinks.Unity
{
	[PublicAPI]
#pragma warning disable 1591
	public static class UnitySinkExtensions
#pragma warning restore 1591
	{
		/// <summary>
		///     Writes log events to Unity's <see cref="UnityEngine.Debug" /> class;
		/// </summary>
		/// <param name="loggerSinkConfiguration">Logger sink configuration</param>
		/// <param name="template">The template used to format messages before they are logged</param>
		/// <param name="levelSwitch">A switch allowing the pass-through minimum level to be changed at runtime.</param>
		/// <param name="formatProvider">An <see cref="IFormatProvider" /> used to render the messages</param>
		/// <param name="logEventLevel">The minimum level for events passed through the sink. Ignored when <paramref name="levelSwitch" /> is specified.</param>
		/// <returns>A <see cref="LoggerConfiguration" /> that can be method-chained</returns>
		public static LoggerConfiguration Unity3D(this LoggerSinkConfiguration loggerSinkConfiguration, string template, LogEventLevel logEventLevel = LogEventLevel.Information, LoggingLevelSwitch levelSwitch = null, IFormatProvider formatProvider = null)
		{
			return loggerSinkConfiguration.Sink(new UnitySink(template, formatProvider), logEventLevel, levelSwitch);
		}
	}
}