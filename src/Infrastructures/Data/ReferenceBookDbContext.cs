using Core.Domain.Common.Context;
using Core.Infrastructure.Data.Context;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Data;

/// <summary>
/// ReferenceBookBase mikroservisi uchun asosiy ma'lumotlar bazasi konteksti.
/// CoreDbContext ning barcha imkoniyatlari (SoftDelete, Multi-Tenancy, Auditing, xmin optimistik qulflash) mavjud.
/// </summary>
public class ReferenceBookDbContext : CoreDbContext
{
    public ReferenceBookDbContext(
        DbContextOptions<ReferenceBookDbContext> options,
        IServiceProvider? serviceProvider = null,
        ITenantContext? tenantContext = null)
        : base(options, serviceProvider, tenantContext)
    {
    }

    public DbSet<ReferenceBook> ReferenceBooks { get; set; } = null!;
    public DbSet<ReferenceBookItem> ReferenceBookItems { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ReferenceBook>(entity =>
        {
            entity.HasIndex(e => new { e.TenantId, e.Code })
                .IsUnique()
                .HasFilter("\"is_delete\" = false");

            entity.HasMany(e => e.Items)
                .WithOne(e => e.ReferenceBook)
                .HasForeignKey(e => e.ReferenceBookId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ReferenceBookItem>(entity =>
        {
            entity.HasIndex(e => new { e.TenantId, e.ReferenceBookId, e.Code })
                .IsUnique()
                .HasFilter("\"is_delete\" = false");
        });
    }
}
