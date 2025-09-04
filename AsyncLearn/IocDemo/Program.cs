using Microsoft.Extensions.DependencyInjection;

var builder = new ServiceCollection	();
builder.AddSingleton<ILogger, MyLogger>();
var services = builder.BuildServiceProvider();
var logger = services.GetService<ILogger>();
logger.Log("Hello, World!");

interface ILogger {
	void Log(string message);
}

class MyLogger :ILogger{
	public void Log(string message) {
		Console.WriteLine(message);
	}
}