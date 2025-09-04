using MyLogging;

namespace MyServices;

public class DeviceSerivce {
	private readonly IMyLogger _logger;

	public DeviceSerivce(IMyLogger logger) {
		_logger = logger;

	}

	public void CheckDevice() {
		if (Random.Shared.NextDouble() > 0.5) {
			_logger.LogInformation("Device is Ok");
			
		}
		else {
			_logger.LogError("Device is Error");
		}
	}
}