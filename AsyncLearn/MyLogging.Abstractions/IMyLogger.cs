namespace MyLogging;

public interface IMyLogger {
	void LogInformation(string message);
	void LogError(string message);
	void LogWarning(string message);
}