using Serilog;

namespace LibEternal.Testing
{
	internal static class Program
	{
		private static void Main()
		{
			Log.Logger = new LoggerConfiguration()
				.WriteTo.Console(outputTemplate:"[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}{NewLine}")
				.CreateLogger();
			
			

			Log.CloseAndFlush();
		}
	}
}