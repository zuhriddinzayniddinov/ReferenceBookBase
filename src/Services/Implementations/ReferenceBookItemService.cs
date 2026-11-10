using System.Net;
using AutoMapper;
using Core.Domain.Exceptions;
using Core.Domain.Models.Api;
using Core.Infrastructure.Data.Extensions;
using Domain.DTOs;
using Domain.Entities;
using Domain.Services;
using Infrastructures.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Services.Implementations;

public class ReferenceBookItemService(
    IReferenceBookItemRepository itemRepository,
    IReferenceBookRepository bookRepository,
    IMapper mapper,
    ILogger<ReferenceBookItemService> logger) : IReferenceBookItemService
{
    public async Task<ResponseModel<List<ReferenceBookItemDto>>> GetAllAsync(MetaQueryModel metaQuery, CancellationToken ct = default)
    {
        var query = itemRepository.GetAllAsQueryable(asNoTracking: true);

        var filtered = query.FilterByExpressions(metaQuery).Sort(metaQuery);

        var count = await filtered.CountAsync(ct);

        var entities = await filtered
            .Skip(metaQuery.Skip)
            .Take(metaQuery.Take > 0 ? metaQuery.Take : 10)
            .ToReadOnlyListAsync(ct);

        var dtos = mapper.Map<List<ReferenceBookItemDto>>(entities);
        return new ResponseModel<List<ReferenceBookItemDto>>(dtos, HttpStatusCode.OK, count);
    }

    public async Task<ResponseModel<List<ReferenceBookItemDto>>> GetByReferenceBookIdAsync(long referenceBookId, CancellationToken ct = default)
    {
        var items = await itemRepository.GetByReferenceBookIdAsync(referenceBookId, asNoTracking: true, ct: ct);
        var dtos = mapper.Map<List<ReferenceBookItemDto>>(items);
        return new ResponseModel<List<ReferenceBookItemDto>>(dtos, HttpStatusCode.OK, items.Count);
    }

    public async Task<ResponseModel<ReferenceBookItemDto>> GetByIdAsync(long id, CancellationToken ct = default)
    {
        var item = await itemRepository.GetByIdAsync(id, asNoTracking: true);
        if (item == null)
            throw new NotFoundException($"Ma'lumotnoma elementi topilmadi (ID: {id})");

        return new ResponseModel<ReferenceBookItemDto>(mapper.Map<ReferenceBookItemDto>(item));
    }

    public async Task<ResponseModel<ReferenceBookItemDto>> CreateAsync(CreateReferenceBookItemDto dto, CancellationToken ct = default)
    {
        var book = await bookRepository.GetByIdAsync(dto.ReferenceBookId, asNoTracking: true);
        if (book == null)
            throw new NotFoundException($"Asosiy ma'lumotnoma topilmadi (ID: {dto.ReferenceBookId})");

        var existing = await itemRepository.GetByCodeAsync(dto.ReferenceBookId, dto.Code, asNoTracking: true, ct: ct);
        if (existing != null)
            throw new AlreadyExistsException($"Ushbu ma'lumotnomada '{dto.Code}' kodli element mavjud!");

        var entity = mapper.Map<ReferenceBookItem>(dto);
        var created = await itemRepository.AddAsync(entity);

        logger.LogInformation("Yangi ma'lumotnoma elementi yaratildi: {Code} (ID: {Id})", created.Code, created.Id);

        return new ResponseModel<ReferenceBookItemDto>(mapper.Map<ReferenceBookItemDto>(created));
    }

    public async Task<ResponseModel<ReferenceBookItemDto>> UpdateAsync(long id, UpdateReferenceBookItemDto dto, CancellationToken ct = default)
    {
        var item = await itemRepository.GetByIdAsync(id, asNoTracking: false);
        if (item == null)
            throw new NotFoundException($"Ma'lumotnoma elementi topilmadi (ID: {id})");

        if (item.Code != dto.Code || item.ReferenceBookId != dto.ReferenceBookId)
        {
            var conflict = await itemRepository.GetByCodeAsync(dto.ReferenceBookId, dto.Code, asNoTracking: true, ct: ct);
            if (conflict != null && conflict.Id != id)
                throw new AlreadyExistsException($"Ushbu ma'lumotnomada '{dto.Code}' kodli element mavjud!");
        }

        item.ReferenceBookId = dto.ReferenceBookId;
        item.Code = dto.Code;
        item.Name = dto.Name;
        item.Value = dto.Value;
        item.SortOrder = dto.SortOrder;
        item.IsActive = dto.IsActive;

        var updated = await itemRepository.UpdateAsync(item);

        logger.LogInformation("Ma'lumotnoma elementi yangilandi: {Code} (ID: {Id})", updated.Code, updated.Id);

        return new ResponseModel<ReferenceBookItemDto>(mapper.Map<ReferenceBookItemDto>(updated));
    }

    public async Task<ResponseModel<bool>> DeleteAsync(long id, CancellationToken ct = default)
    {
        var item = await itemRepository.GetByIdAsync(id, asNoTracking: false);
        if (item == null)
            throw new NotFoundException($"Ma'lumotnoma elementi topilmadi (ID: {id})");

        await itemRepository.RemoveAsync(item);

        logger.LogInformation("Ma'lumotnoma elementi o'chirildi (ID: {Id})", id);

        return new ResponseModel<bool>(true);
    }
}
