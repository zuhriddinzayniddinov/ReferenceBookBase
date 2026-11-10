using Core.Infrastructure.Data.Context;
using Core.Infrastructure.Data.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories;

public interface IReferenceBookRepository : IRepositoryBase<ReferenceBook, long>
{
    Task<ReferenceBook?> GetByCodeAsync(string code, bool asNoTracking = false, CancellationToken ct = default);
}

public class ReferenceBookRepository(CoreDbContext dbContext)
    : RepositoryBase<ReferenceBook, long>(dbContext), IReferenceBookRepository
{
    public async Task<ReferenceBook?> GetByCodeAsync(string code, bool asNoTracking = false, CancellationToken ct = default)
    {
        var query = GetAllAsQueryable(asNoTracking: asNoTracking);
        return await query.FirstOrDefaultAsync(x => x.Code == code, ct);
    }
}

public interface IReferenceBookItemRepository : IRepositoryBase<ReferenceBookItem, long>
{
    Task<List<ReferenceBookItem>> GetByReferenceBookIdAsync(long referenceBookId, bool asNoTracking = true, CancellationToken ct = default);
    Task<ReferenceBookItem?> GetByCodeAsync(long referenceBookId, string code, bool asNoTracking = false, CancellationToken ct = default);
}

public class ReferenceBookItemRepository(CoreDbContext dbContext)
    : RepositoryBase<ReferenceBookItem, long>(dbContext), IReferenceBookItemRepository
{
    public async Task<List<ReferenceBookItem>> GetByReferenceBookIdAsync(long referenceBookId, bool asNoTracking = true, CancellationToken ct = default)
    {
        var query = GetAllAsQueryable(asNoTracking: asNoTracking);
        return await query
            .Where(x => x.ReferenceBookId == referenceBookId)
            .OrderBy(x => x.SortOrder)
            .ToListAsync(ct);
    }

    public async Task<ReferenceBookItem?> GetByCodeAsync(long referenceBookId, string code, bool asNoTracking = false, CancellationToken ct = default)
    {
        var query = GetAllAsQueryable(asNoTracking: asNoTracking);
        return await query.FirstOrDefaultAsync(x => x.ReferenceBookId == referenceBookId && x.Code == code, ct);
    }
}
