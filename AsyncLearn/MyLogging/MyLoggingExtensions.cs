using Microsoft.Extensions.DependencyInjection;

namespace MyLogging;

public static class MyLoggingExtensions {
	public static void AddMyLogging(this ServiceCollection services) {
		services.AddSingleton<IMyLogger, MyLogger>();

	}
}