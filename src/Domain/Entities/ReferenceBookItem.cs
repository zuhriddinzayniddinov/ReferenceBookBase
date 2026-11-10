using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain.Common;
using Core.Domain.Common.Context;

namespace Domain.Entities;

/// <summary>
/// Ma'lumotnoma elementi (masalan: Erkak/Ayol, O'zbekiston, Pasport).
/// </summary>
[Table("reference_book_items")]
public class ReferenceBookItem : AuditableModelBase<long>, ITenantEntity, ISoftDelete
{
    [Column("reference_book_id")]
    public long ReferenceBookId { get; set; }

    [ForeignKey(nameof(ReferenceBookId))]
    public virtual ReferenceBook? ReferenceBook { get; set; }

    [Column("code")]
    public string Code { get; set; } = string.Empty;

    [Column("name")]
    public MultiLanguageField Name { get; set; } = new();

    [Column("value")]
    public string? Value { get; set; }

    [Column("sort_order")]
    public int SortOrder { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("tenant_id")]
    public long? TenantId { get; set; }
}
