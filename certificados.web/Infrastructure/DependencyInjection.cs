using System.Reflection;

namespace certificados.web.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services) {


            // Cargar los assemblies específicos que necesitamos
            var assemblies = new[]
            {
                Assembly.Load("certificados.services"),
                Assembly.Load("certificados.dal")
            };

            // Registrar servicios
            var serviceTypes = assemblies
                .SelectMany(s => s.GetTypes())
                .Where(t => t.Name.EndsWith("Service") && !t.IsInterface && !t.IsAbstract);

            foreach (var serviceType in serviceTypes)
            {
                services.AddScoped(serviceType);
                Console.WriteLine($"Registered service: {serviceType.Name}");
            }

            // Registrar Data Access
            var daTypes = assemblies
                .SelectMany(s => s.GetTypes())
                .Where(t => t.Name.EndsWith("DA") && !t.IsInterface && !t.IsAbstract);

            foreach (var daType in daTypes)
            {
                services.AddScoped(daType);
                Console.WriteLine($"Registered DA: {daType.Name}");
            }

            return services;
        }
    }
}
