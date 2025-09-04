using Microsoft.Extensions.DependencyInjection;

namespace MyServices;

public static class MyServicesExtensions {
	public static void AddMyServices(this IServiceCollection services) {
		services.AddSingleton<DeviceSerivce>();
		services.AddSingleton<HardwareService>();
	}

}