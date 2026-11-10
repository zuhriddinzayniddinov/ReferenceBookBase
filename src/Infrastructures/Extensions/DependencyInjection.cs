using Infrastructures.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructures.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IReferenceBookRepository, ReferenceBookRepository>();
        services.AddScoped<IReferenceBookItemRepository, ReferenceBookItemRepository>();

        return services;
    }
}
