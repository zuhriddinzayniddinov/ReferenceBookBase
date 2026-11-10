using Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Services.HostedServices;
using Services.Implementations;
using Services.Mappings;

namespace Services.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<ReferenceBookMappingProfile>();
        });

        services.AddScoped<IReferenceBookService, ReferenceBookService>();
        services.AddScoped<IReferenceBookItemService, ReferenceBookItemService>();
        services.AddScoped<IStructurePermissionLoaderService, StructurePermissionLoaderService>();

        services.AddHostedService<StructurePermissionLoaderHostedService>();

        return services;
    }
}
