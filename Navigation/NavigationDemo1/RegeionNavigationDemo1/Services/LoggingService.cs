using System.Reflection;
using RegeionNavigationDemo1.Interfaces;
using Serilog;
using Serilog.Core;
using Serilog.Enrichers.CallerInfo;

namespace RegeionNavigationDemo1.Services;

public class LoggingService :ILoggingService{
	public LoggingService() {
		var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
		Log.Logger = new LoggerConfiguration()
			.MinimumLevel.Debug()
			.Enrich.WithCallerInfo(
				includeFileInfo: true,
				assemblyPrefix: assemblyName,
				filePathDepth:3
			)
			.WriteTo.Debug(
				outputTemplate:
				"{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}]" +
				"({SourceFile}:{LineNumber}) " +
				"{Message:lj}{NewLine}{Exception}"
			)
			.WriteTo.File(
				path: "logs/app.log",
				rollingInterval: RollingInterval.Day,
				retainedFileCountLimit: 10,
				encoding: System.Text.Encoding.UTF8,
				outputTemplate:
				"[{Timestamp:yyyy-MM-dd HH:mm:ss}] [{Level}]  " +
				"({SourceFile}:{LineNumber}) " +
				"{Message:lj}{NewLine}{Exception}"
			)
			.CreateLogger();
	}
	public void Info(string message) => Log.Information(message);

	public void Warn(string message) => Log.Warning(message);

	public void Error(string message, Exception? ex = null) {
		if (ex != null)
			Log.Error(ex, message);
		else
			Log.Error(message);
	}

	public void Debug(string message) => Log.Debug(message);
}