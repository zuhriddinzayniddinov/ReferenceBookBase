using Core.Domain.Common;
using Core.Domain.DTOs;

namespace Domain.DTOs;

public class ReferenceBookDto : BaseDto<long>
{
    public string Code { get; set; } = string.Empty;
    public MultiLanguageField Name { get; set; } = new();
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<ReferenceBookItemDto> Items { get; set; } = [];
}

public class CreateReferenceBookDto
{
    public string Code { get; set; } = string.Empty;
    public MultiLanguageField Name { get; set; } = new();
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}

public class UpdateReferenceBookDto
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public MultiLanguageField Name { get; set; } = new();
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}

public class ReferenceBookItemDto : BaseDto<long>
{
    public long ReferenceBookId { get; set; }
    public string Code { get; set; } = string.Empty;
    public MultiLanguageField Name { get; set; } = new();
    public string? Value { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? CreatedAt { get; set; }
}

public class CreateReferenceBookItemDto
{
    public long ReferenceBookId { get; set; }
    public string Code { get; set; } = string.Empty;
    public MultiLanguageField Name { get; set; } = new();
    public string? Value { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
}

public class UpdateReferenceBookItemDto
{
    public long Id { get; set; }
    public long ReferenceBookId { get; set; }
    public string Code { get; set; } = string.Empty;
    public MultiLanguageField Name { get; set; } = new();
    public string? Value { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
