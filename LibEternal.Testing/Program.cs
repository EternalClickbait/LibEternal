using Serilog;
using Serilog.Enrichers;
using Serilog.Exceptions;
using System;
using System.Text;

namespace LibEternal.Testing
{
	internal static class Program
	{
		private static string BuildOutputTemplate(TimeStampMode timeStampMode, LogLevelMode logLevelMode = LogLevelMode.Full,
		                                          bool includeProperties = false)
		{
			//The string builder we'll use for out output template
			StringBuilder builder = new StringBuilder(100);
			builder
				.Append('[');
			//E.g. 31/03/2020 (31st march 2020)
			const string dateFormat = "dd/MM/yyyy";
			const string timeFormat = "HH:mm:ss";
			switch (timeStampMode)
			{
				case TimeStampMode.Date:
					builder.AppendFormat("{{Timestamp:{0}}}", dateFormat);
					break;
				case TimeStampMode.Time:
					builder.AppendFormat("{{Timestamp:{0}}}", timeFormat);
					break;
				case TimeStampMode.DateAndTime:
					builder.AppendFormat("{{Timestamp:{0} {1}}}", dateFormat, timeFormat);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(timeStampMode), timeStampMode, null);
			}

			builder.Append(' ');

			switch (logLevelMode)
			{
				case LogLevelMode.Full:
					builder.Append("{Level:t}]\t");
					break;
				case LogLevelMode.Short:
					builder.Append("{Level:t4}]");
					break;
				case LogLevelMode.Tiny:
					builder.Append("{Level:t3}]");
					break;
			}

			builder.Append(" [{ThreadName} (ID {ThreadId})]\t");

			builder.Append(" {Message:j}");
			if (includeProperties)
				builder.Append(" {Properties}");
			builder.Append("{NewLine}");
			builder.Append("{Exception}");

			return builder.ToString();
		}

		private static void Main()
		{
			//Timestamp is DateTimeOffset
			Log.Logger = new LoggerConfiguration()
				.WriteTo.Console(outputTemplate: BuildOutputTemplate(TimeStampMode.Time))
				.MinimumLevel.Verbose()
				.Enrich.FromLogContext()
				.Enrich.WithThreadId()
				.Enrich.WithThreadName()
				//If the thread name is unknown, displays it as "Unknown thread" instead of the default null
				.Enrich.WithProperty(ThreadNameEnricher.ThreadNamePropertyName, "Unknown thread")
				.Enrich.WithExceptionDetails()
				.CreateLogger();

			const string msg = "TEST";

			for (int i = 0; i < 3; i++)
			{
				Log.Fatal(msg);
				Log.Error(msg);
				Log.Warning(msg);
				Log.Information(msg);
				Log.Debug(msg);
				Log.Verbose(msg);

				Log.Information(new Exception("Yeet", new NullReferenceException("Internal exception")), "");
				Log.Information(new Exception("Yeet", new NullReferenceException("Internal exception")), "");
			}


			Log.CloseAndFlush();
		}

		private enum LogLevelMode
		{
			Short,
			Full,
			Tiny
		}

		private enum TimeStampMode
		{
			Date,
			Time,
			DateAndTime
		}
	}
}