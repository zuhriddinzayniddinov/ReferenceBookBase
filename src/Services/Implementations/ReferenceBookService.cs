using System.Net;
using AutoMapper;
using Core.Domain.Exceptions;
using Core.Domain.Models.Api;
using Core.Infrastructure.Caching.Hybrid;
using Core.Infrastructure.Data.Extensions;
using Domain.DTOs;
using Domain.Entities;
using Domain.Services;
using Infrastructures.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Services.Implementations;

public class ReferenceBookService(
    IReferenceBookRepository repository,
    IMapper mapper,
    ILogger<ReferenceBookService> logger,
    IHybridCacheService? cache = null) : IReferenceBookService
{
    private const string CacheTag = "reference-books";

    public async Task<ResponseModel<List<ReferenceBookDto>>> GetAllAsync(MetaQueryModel metaQuery, CancellationToken ct = default)
    {
        var query = repository.GetAllWithDetails(["Items"], deleted: false);

        var filtered = query.FilterByExpressions(metaQuery).Sort(metaQuery);

        var count = await filtered.CountAsync(ct);

        var entities = await filtered
            .Skip(metaQuery.Skip)
            .Take(metaQuery.Take > 0 ? metaQuery.Take : 10)
            .ToReadOnlyListAsync(ct);

        var dtos = mapper.Map<List<ReferenceBookDto>>(entities);

        return new ResponseModel<List<ReferenceBookDto>>(dtos, HttpStatusCode.OK, count);
    }

    public async Task<ResponseModel<ReferenceBookDto>> GetByIdAsync(long id, CancellationToken ct = default)
    {
        if (cache != null)
        {
            var cached = await cache.GetOrCreateAsync(
                key: $"reference-book:id:{id}",
                factory: async token =>
                {
                    var entity = await repository.GetAllWithDetails(["Items"])
                        .FirstOrDefaultAsync(x => x.Id == id, token);
                    return entity != null ? mapper.Map<ReferenceBookDto>(entity) : null;
                },
                localTtl: TimeSpan.FromMinutes(2),
                remoteTtl: TimeSpan.FromMinutes(10),
                tags: [CacheTag],
                cancellationToken: ct);

            if (cached == null)
                throw new NotFoundException($"Ma'lumotnoma topilmadi (ID: {id})");

            return new ResponseModel<ReferenceBookDto>(cached);
        }

        var entity = await repository.GetAllWithDetails(["Items"])
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (entity == null)
            throw new NotFoundException($"Ma'lumotnoma topilmadi (ID: {id})");

        return new ResponseModel<ReferenceBookDto>(mapper.Map<ReferenceBookDto>(entity));
    }

    public async Task<ResponseModel<ReferenceBookDto>> GetByCodeAsync(string code, CancellationToken ct = default)
    {
        var entity = await repository.GetAllWithDetails(["Items"])
            .FirstOrDefaultAsync(x => x.Code == code, ct);

        if (entity == null)
            throw new NotFoundException($"Ma'lumotnoma topilmadi (Code: {code})");

        return new ResponseModel<ReferenceBookDto>(mapper.Map<ReferenceBookDto>(entity));
    }

    public async Task<ResponseModel<ReferenceBookDto>> CreateAsync(CreateReferenceBookDto dto, CancellationToken ct = default)
    {
        var existing = await repository.GetByCodeAsync(dto.Code, ct: ct);
        if (existing != null)
            throw new AlreadyExistsException($"'{dto.Code}' kodli ma'lumotnoma allaqachon mavjud!");

        var entity = mapper.Map<ReferenceBook>(dto);
        var created = await repository.AddAsync(entity);

        if (cache != null)
            await cache.RemoveByTagAsync(CacheTag, ct);

        logger.LogInformation("Yangi ma'lumotnoma yaratildi: {Code} (ID: {Id})", created.Code, created.Id);

        return new ResponseModel<ReferenceBookDto>(mapper.Map<ReferenceBookDto>(created));
    }

    public async Task<ResponseModel<ReferenceBookDto>> UpdateAsync(long id, UpdateReferenceBookDto dto, CancellationToken ct = default)
    {
        var entity = await repository.GetByIdAsync(id, asNoTracking: false);
        if (entity == null)
            throw new NotFoundException($"Ma'lumotnoma topilmadi (ID: {id})");

        if (entity.Code != dto.Code)
        {
            var conflict = await repository.GetByCodeAsync(dto.Code, ct: ct);
            if (conflict != null && conflict.Id != id)
                throw new AlreadyExistsException($"'{dto.Code}' kodli ma'lumotnoma allaqachon mavjud!");
        }

        entity.Code = dto.Code;
        entity.Name = dto.Name;
        entity.Description = dto.Description;
        entity.IsActive = dto.IsActive;

        var updated = await repository.UpdateAsync(entity);

        if (cache != null)
            await cache.RemoveByTagAsync(CacheTag, ct);

        logger.LogInformation("Ma'lumotnoma yangilandi: {Code} (ID: {Id})", updated.Code, updated.Id);

        return new ResponseModel<ReferenceBookDto>(mapper.Map<ReferenceBookDto>(updated));
    }

    public async Task<ResponseModel<bool>> DeleteAsync(long id, CancellationToken ct = default)
    {
        var entity = await repository.GetByIdAsync(id, asNoTracking: false);
        if (entity == null)
            throw new NotFoundException($"Ma'lumotnoma topilmadi (ID: {id})");

        await repository.RemoveAsync(entity);

        if (cache != null)
            await cache.RemoveByTagAsync(CacheTag, ct);

        logger.LogInformation("Ma'lumotnoma o'chirildi (ID: {Id})", id);

        return new ResponseModel<bool>(true);
    }
}
