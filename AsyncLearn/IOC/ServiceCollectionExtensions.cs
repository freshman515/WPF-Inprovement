using Microsoft.Extensions.DependencyInjection;

namespace IOC;

public static class ServiceCollectionExtensions {
    public static void AddServices(this IServiceCollection containter) {
        containter.Scan(scan => scan.FromAssemblyOf<App>()
            .AddClasses(cls => cls.Where(type => type.Name.EndsWith("Service")))
            .AsImplementedInterfaces()
            .WithSingletonLifetime());
    }
}