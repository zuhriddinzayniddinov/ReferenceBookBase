using Core.Domain.Models.Api;
using Core.Web.Attributes;
using Core.Web.Controllers;
using Domain.DTOs;
using Domain.Enums;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApis.Controllers;

[ApiController]
[Route("api/v1/reference-book-items")]
public class ReferenceBookItemsController(IReferenceBookItemService itemService) : ApiControllerBase
{
    [HttpGet]
    [ApiGroup("Client", "Admin")]
    [PermissionAuthorize(ReferenceBookPermissions.ViewReferenceBookItems)]
    public async Task<ResponseModel<List<ReferenceBookItemDto>>> GetAll([FromQuery] MetaQueryModel metaQuery, CancellationToken ct)
    {
        return await itemService.GetAllAsync(metaQuery, ct);
    }

    [HttpGet("by-book/{referenceBookId:long}")]
    [ApiGroup("Client", "Admin")]
    [PermissionAuthorize(ReferenceBookPermissions.ViewReferenceBookItems)]
    public async Task<ResponseModel<List<ReferenceBookItemDto>>> GetByReferenceBookId([FromRoute] long referenceBookId, CancellationToken ct)
    {
        return await itemService.GetByReferenceBookIdAsync(referenceBookId, ct);
    }

    [HttpGet("{id:long}")]
    [ApiGroup("Client", "Admin")]
    [PermissionAuthorize(ReferenceBookPermissions.ViewReferenceBookItems)]
    public async Task<ResponseModel<ReferenceBookItemDto>> GetById([FromRoute] long id, CancellationToken ct)
    {
        return await itemService.GetByIdAsync(id, ct);
    }

    [HttpPost]
    [ApiGroup("Admin")]
    [PermissionAuthorize(ReferenceBookPermissions.CreateReferenceBookItem)]
    public async Task<ResponseModel<ReferenceBookItemDto>> Create([FromBody] CreateReferenceBookItemDto dto, CancellationToken ct)
    {
        return await itemService.CreateAsync(dto, ct);
    }

    [HttpPut("{id:long}")]
    [ApiGroup("Admin")]
    [PermissionAuthorize(ReferenceBookPermissions.UpdateReferenceBookItem)]
    public async Task<ResponseModel<ReferenceBookItemDto>> Update([FromRoute] long id, [FromBody] UpdateReferenceBookItemDto dto, CancellationToken ct)
    {
        return await itemService.UpdateAsync(id, dto, ct);
    }

    [HttpDelete("{id:long}")]
    [ApiGroup("Admin")]
    [PermissionAuthorize(ReferenceBookPermissions.DeleteReferenceBookItem)]
    public async Task<ResponseModel<bool>> Delete([FromRoute] long id, CancellationToken ct)
    {
        return await itemService.DeleteAsync(id, ct);
    }
}
