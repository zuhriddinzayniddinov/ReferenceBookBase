using Core.Domain.Models.Api;
using Domain.DTOs;

namespace Domain.Services;

public interface IReferenceBookService
{
    Task<ResponseModel<List<ReferenceBookDto>>> GetAllAsync(MetaQueryModel metaQuery, CancellationToken ct = default);
    Task<ResponseModel<ReferenceBookDto>> GetByIdAsync(long id, CancellationToken ct = default);
    Task<ResponseModel<ReferenceBookDto>> GetByCodeAsync(string code, CancellationToken ct = default);
    Task<ResponseModel<ReferenceBookDto>> CreateAsync(CreateReferenceBookDto dto, CancellationToken ct = default);
    Task<ResponseModel<ReferenceBookDto>> UpdateAsync(long id, UpdateReferenceBookDto dto, CancellationToken ct = default);
    Task<ResponseModel<bool>> DeleteAsync(long id, CancellationToken ct = default);
}

public interface IReferenceBookItemService
{
    Task<ResponseModel<List<ReferenceBookItemDto>>> GetAllAsync(MetaQueryModel metaQuery, CancellationToken ct = default);
    Task<ResponseModel<List<ReferenceBookItemDto>>> GetByReferenceBookIdAsync(long referenceBookId, CancellationToken ct = default);
    Task<ResponseModel<ReferenceBookItemDto>> GetByIdAsync(long id, CancellationToken ct = default);
    Task<ResponseModel<ReferenceBookItemDto>> CreateAsync(CreateReferenceBookItemDto dto, CancellationToken ct = default);
    Task<ResponseModel<ReferenceBookItemDto>> UpdateAsync(long id, UpdateReferenceBookItemDto dto, CancellationToken ct = default);
    Task<ResponseModel<bool>> DeleteAsync(long id, CancellationToken ct = default);
}

public interface IStructurePermissionLoaderService
{
    /// <summary>
    /// AuthApi dan ushbu mikroservisga tegishli struktura va ruxsat kodlarini yuklab, StaticCache.Permissions ga yozadi.
    /// </summary>
    Task<int> LoadStructurePermissionsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Xotiradagi keshni majburiy qayta yuklash.
    /// </summary>
    Task<int> ReloadAsync(CancellationToken cancellationToken = default);
}
