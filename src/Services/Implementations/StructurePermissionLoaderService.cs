using Core.Infrastructure.Permissions;
using Core.Web.Constants;
using Domain.Contracts;
using Domain.Enums;
using Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Services.Implementations;

/// <summary>
/// AuthApi dan ushbu mikroservisga tegishli strukturalar (rollar) va ularning ruxsat kodlarini
/// Code-First gRPC orqali qabul qilib, StaticCache.Permissions ga yuklovchi xizmat.
/// </summary>
public class StructurePermissionLoaderService(
    IServiceProvider serviceProvider,
    ILogger<StructurePermissionLoaderService> logger) : IStructurePermissionLoaderService
{
    private const string ServiceName = "ReferenceBookBase";
    private const int ServiceCode = 30;

    public async Task<int> LoadStructurePermissionsAsync(CancellationToken cancellationToken = default)
    {
        var grpcClient = serviceProvider.GetService<IStructurePermissionGrpcService>();
        if (grpcClient == null)
        {
            logger.LogWarning("IStructurePermissionGrpcService mijoz topilmadi. Struktura ruxsatlarini AuthApi dan yuklab bo'lmadi.");
            return 0;
        }

        var definitions = PermissionDiscoveryHelper.DiscoverFromEnum<ReferenceBookPermissions>();
        var permissionCodes = definitions.Select(p => p.Code).ToList();

        var request = new GetServiceStructurePermissionsRequest
        {
            ServiceName = ServiceName,
            ServiceCode = ServiceCode,
            PermissionCodes = permissionCodes
        };

        logger.LogInformation(
            "AuthApi dan struktura ruxsatlari so'ralmoqda (gRPC). Servis: {ServiceName}, Kodlar soni: {CodesCount}",
            ServiceName,
            permissionCodes.Count);

        try
        {
            var response = await grpcClient.GetServiceStructurePermissionsAsync(request);

            if (!response.Success)
            {
                logger.LogWarning("AuthApi dan struktura ruxsatlarini olishda xatolik: {Message}", response.Message);
                return 0;
            }

            int totalPermissionsCount = 0;
            foreach (var item in response.Items)
            {
                StaticCache.Permissions[item.StructureId] = item.PermissionCodes;
                totalPermissionsCount += item.PermissionCodes.Count;
            }

            logger.LogInformation(
                "AuthApi dan struktura ruxsatlari muvaffaqiyatli yuklandi: {ItemCount} ta struktura, {TotalPermissions} ta ruxsat kodlari StaticCache.Permissions ga yozildi.",
                response.Items.Count,
                totalPermissionsCount);

            return response.Items.Count;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "AuthApi dan struktura ruxsatlarini gRPC orqali olishda ulanish xatosi");
            throw;
        }
    }

    public async Task<int> ReloadAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Struktura ruxsatlari qayta yuklanmoqda...");
        return await LoadStructurePermissionsAsync(cancellationToken);
    }
}
