using Microsoft.EntityFrameworkCore;
using NexLibrary.Domain.Entities;
using NexLibrary.Infrastructure.Persistence.Seed;

namespace NexLibrary.Infrastructure.Persistence;

public sealed class NexLibraryDbContext : DbContext
{
    public NexLibraryDbContext(DbContextOptions<NexLibraryDbContext> options) : base(options)
    {
    }

    public DbSet<Kitap> Kitaplar => Set<Kitap>();

    public DbSet<Uye> Uyeler => Set<Uye>();

    public DbSet<FormAlani> FormAlanlari => Set<FormAlani>();

    public DbSet<DinamikAlanDegeri> DinamikAlanDegerleri => Set<DinamikAlanDegeri>();

    public DbSet<Kullanici> Kullanicilar => Set<Kullanici>();

    public DbSet<Rol> Roller => Set<Rol>();

    public DbSet<KullaniciRol> KullaniciRolleri => Set<KullaniciRol>();

    public DbSet<AuditLog> AuditLoglari => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NexLibraryDbContext).Assembly);

        DefaultFormFieldsSeeder.Seed(modelBuilder);
    }
}