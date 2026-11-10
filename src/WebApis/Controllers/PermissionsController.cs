using System.Net;
using Core.Domain.Models.Api;
using Core.Web.Attributes;
using Core.Web.Constants;
using Core.Web.Controllers;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApis.Controllers;

[ApiController]
[Route("api/v1/permissions")]
public class PermissionsController(IStructurePermissionLoaderService permissionLoader) : ApiControllerBase
{
    /// <summary>
    /// AuthApi dan ushbu mikroservisga tegishli struktura va ruxsatlarni majburiy qayta yuklash.
    /// </summary>
    [HttpPost("reload-structures")]
    [ApiGroup("Admin")]
    public async Task<ResponseModel<string>> ReloadStructures(CancellationToken ct)
    {
        var count = await permissionLoader.ReloadAsync(ct);
        return new ResponseModel<string>($"{count} ta struktura uchun ruxsatlar AuthApi dan muvaffaqiyatli qayta yuklandi.");
    }

    /// <summary>
    /// Xotiradagi (StaticCache.Permissions) kesh holatini tekshirish.
    /// </summary>
    [HttpGet("cache-status")]
    [ApiGroup("Admin")]
    public Task<ResponseModel<object>> GetCacheStatus()
    {
        var structuresCount = StaticCache.Permissions.Count;
        var totalPermissions = StaticCache.Permissions.Values.Sum(v => v?.Count ?? 0);

        var info = new
        {
            LoadedStructuresCount = structuresCount,
            TotalPermissionsCount = totalPermissions,
            StructureIds = StaticCache.Permissions.Keys.ToList()
        };

        return Task.FromResult(new ResponseModel<object>(info));
    }
}
