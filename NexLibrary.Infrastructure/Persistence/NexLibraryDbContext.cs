using Microsoft.EntityFrameworkCore;
using NexLibrary.Domain.Entities;
using NexLibrary.Infrastructure.Persistence.Seed;

namespace NexLibrary.Infrastructure.Persistence;

public sealed class NexLibraryDbContext : DbContext
{
    public NexLibraryDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<Kitap> Kitaplar => Set<Kitap>();

    public DbSet<KitapKopya> KitapKopyalari => Set<KitapKopya>();

    public DbSet<Uye> Uyeler => Set<Uye>();

    public DbSet<Odunc> Oduncler => Set<Odunc>();

    public DbSet<FormAlani> FormAlanlari => Set<FormAlani>();

    public DbSet<DinamikAlanDegeri> DinamikAlanDegerleri => Set<DinamikAlanDegeri>();

    public DbSet<Kullanici> Kullanicilar => Set<Kullanici>();

    public DbSet<Rol> Roller => Set<Rol>();

    public DbSet<KullaniciRol> KullaniciRolleri => Set<KullaniciRol>();

    public DbSet<YetkiTanimi> YetkiTanimlari => Set<YetkiTanimi>();

    public DbSet<RolYetki> RolYetkileri => Set<RolYetki>();

    public DbSet<AuditLog> AuditLoglari => Set<AuditLog>();

    public DbSet<ApiClient> ApiClients => Set<ApiClient>();

    public DbSet<ApiClientYetki> ApiClientYetkileri => Set<ApiClientYetki>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NexLibraryDbContext).Assembly);

        DefaultFormFieldsSeeder.Seed(modelBuilder);
    }
}