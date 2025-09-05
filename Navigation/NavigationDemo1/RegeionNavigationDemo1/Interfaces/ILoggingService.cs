﻿namespace RegeionNavigationDemo1.Interfaces;

public interface ILoggingService {
	void Info(string message);
	void Warn(string message);
	void Error(string message, Exception? ex = null);
	void Debug(string message);
}