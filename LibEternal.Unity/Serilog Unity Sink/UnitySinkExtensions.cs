using LibEternal.JetBrains.Annotations;
using Serilog.Configuration;
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
		/// <param name="formatProvider">An <see cref="IFormatProvider" /> used to render the messages</param>
		/// <returns>A <see cref="LoggerConfiguration" /> that can be method-chained</returns>
		public static LoggerConfiguration Unity3D(this LoggerSinkConfiguration loggerSinkConfiguration, string template, IFormatProvider formatProvider = null)
		{
			return loggerSinkConfiguration.Sink(new UnitySink(template, formatProvider));
		}
	}
}