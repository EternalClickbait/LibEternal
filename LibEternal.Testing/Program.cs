using Serilog;
using Serilog.Context;
using System;

namespace LibEternal.Testing
{
	class Program
	{
		static void Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.WriteTo.Console(outputTemplate:"[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
				.Enrich.WithProcessId()
				.Enrich.WithProcessName()
				.Enrich.FromLogContext()
				.CreateLogger();
			
			Log.Information("No contextual properties");

			using (LogContext.PushProperty("A", 1))
			{
				Log.Information("Carries property A = 1");

				using (LogContext.PushProperty("A", 2))
					using (LogContext.PushProperty("B", 1))
					{
						Log.Information("Carries A = 2 and B = 1");
					}

				Log.Information("Carries property A = 1, again");
			}
			
			Log.Information("TEST");
			
			Log.CloseAndFlush();
		}
	}
}