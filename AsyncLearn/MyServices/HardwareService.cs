using MyLogging;

namespace MyServices;

public class HardwareService {
	private readonly IMyLogger _myLogger;

	public HardwareService(IMyLogger myLogger) {
		_myLogger = myLogger;
	}

}