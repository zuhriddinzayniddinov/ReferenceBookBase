using Core.Domain.Models.Api;
using Core.Web.Attributes;
using Core.Web.Controllers;
using Domain.DTOs;
using Domain.Enums;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApis.Controllers;

[ApiController]
[Route("api/v1/reference-books")]
public class ReferenceBooksController(IReferenceBookService referenceBookService) : ApiControllerBase
{
    [HttpGet]
    [ApiGroup("Client", "Admin")]
    [PermissionAuthorize(ReferenceBookPermissions.ViewReferenceBooks)]
    public async Task<ResponseModel<List<ReferenceBookDto>>> GetAll([FromQuery] MetaQueryModel metaQuery, CancellationToken ct)
    {
        return await referenceBookService.GetAllAsync(metaQuery, ct);
    }

    [HttpGet("{id:long}")]
    [ApiGroup("Client", "Admin")]
    [PermissionAuthorize(ReferenceBookPermissions.ViewReferenceBooks)]
    public async Task<ResponseModel<ReferenceBookDto>> GetById([FromRoute] long id, CancellationToken ct)
    {
        return await referenceBookService.GetByIdAsync(id, ct);
    }

    [HttpGet("code/{code}")]
    [ApiGroup("Client", "Admin")]
    [PermissionAuthorize(ReferenceBookPermissions.ViewReferenceBooks)]
    public async Task<ResponseModel<ReferenceBookDto>> GetByCode([FromRoute] string code, CancellationToken ct)
    {
        return await referenceBookService.GetByCodeAsync(code, ct);
    }

    [HttpPost]
    [ApiGroup("Admin")]
    [PermissionAuthorize(ReferenceBookPermissions.CreateReferenceBook)]
    public async Task<ResponseModel<ReferenceBookDto>> Create([FromBody] CreateReferenceBookDto dto, CancellationToken ct)
    {
        return await referenceBookService.CreateAsync(dto, ct);
    }

    [HttpPut("{id:long}")]
    [ApiGroup("Admin")]
    [PermissionAuthorize(ReferenceBookPermissions.UpdateReferenceBook)]
    public async Task<ResponseModel<ReferenceBookDto>> Update([FromRoute] long id, [FromBody] UpdateReferenceBookDto dto, CancellationToken ct)
    {
        return await referenceBookService.UpdateAsync(id, dto, ct);
    }

    [HttpDelete("{id:long}")]
    [ApiGroup("Admin")]
    [PermissionAuthorize(ReferenceBookPermissions.DeleteReferenceBook)]
    public async Task<ResponseModel<bool>> Delete([FromRoute] long id, CancellationToken ct)
    {
        return await referenceBookService.DeleteAsync(id, ct);
    }
}
