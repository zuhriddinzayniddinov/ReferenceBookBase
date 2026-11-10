using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain.Common;
using Core.Domain.Common.Context;

namespace Domain.Entities;

/// <summary>
/// Ma'lumotnoma asosiy turi (masalan: Jins, Mamlakat, Hujjat turi, Valyuta).
/// </summary>
[Table("reference_books")]
public class ReferenceBook : AuditableModelBase<long>, ITenantEntity, ISoftDelete
{
    [Column("code")]
    public string Code { get; set; } = string.Empty;

    [Column("name")]
    public MultiLanguageField Name { get; set; } = new();

    [Column("description")]
    public string? Description { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("tenant_id")]
    public long? TenantId { get; set; }

    public virtual ICollection<ReferenceBookItem> Items { get; set; } = new List<ReferenceBookItem>();
}
