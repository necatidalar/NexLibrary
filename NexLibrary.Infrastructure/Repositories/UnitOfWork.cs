using NexLibrary.Application.Interfaces.Repositories;
using NexLibrary.Domain.Entities;
using NexLibrary.Infrastructure.Persistence;

namespace NexLibrary.Infrastructure.Repositories;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly NexLibraryDbContext _context;
    public IGenericRepository<Odunc> Oduncler { get; }
    public IGenericRepository<Uye> Uyeler { get; }
    public IGenericRepository<KitapKopya> KitapKopyalari { get; }

    public IGenericRepository<YetkiTanimi> YetkiTanimlari { get; }

    public IGenericRepository<RolYetki> RolYetkileri { get; }

    public UnitOfWork(NexLibraryDbContext context)
    {
        _context = context;

        Kitaplar = new GenericRepository<Kitap>(_context);
        KitapKopyalari = new GenericRepository<KitapKopya>(_context);
        FormAlanlari = new GenericRepository<FormAlani>(_context);
        DinamikAlanDegerleri = new GenericRepository<DinamikAlanDegeri>(_context);
        Kullanicilar = new GenericRepository<Kullanici>(_context);
        Roller = new GenericRepository<Rol>(_context);
        KullaniciRolleri = new GenericRepository<KullaniciRol>(_context);
        AuditLoglari = new GenericRepository<AuditLog>(_context);
        Uyeler = new GenericRepository<Uye>(_context);
        Oduncler = new GenericRepository<Odunc>(_context);
        YetkiTanimlari = new GenericRepository<YetkiTanimi>(_context);
        RolYetkileri = new GenericRepository<RolYetki>(_context);
    }

    public IGenericRepository<Kitap> Kitaplar { get; }

    public IGenericRepository<FormAlani> FormAlanlari { get; }

    public IGenericRepository<DinamikAlanDegeri> DinamikAlanDegerleri { get; }

    public IGenericRepository<Kullanici> Kullanicilar { get; }

    public IGenericRepository<Rol> Roller { get; }

    public IGenericRepository<KullaniciRol> KullaniciRolleri { get; }

    public IGenericRepository<AuditLog> AuditLoglari { get; }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task ExecuteInTransactionAsync(
        Func<Task> action,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await action();
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<T> ExecuteInTransactionAsync<T>(
        Func<Task<T>> action,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var result = await action();
            await transaction.CommitAsync(cancellationToken);
            return result;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}