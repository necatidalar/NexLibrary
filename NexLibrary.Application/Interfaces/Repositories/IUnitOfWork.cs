using NexLibrary.Domain.Entities;

namespace NexLibrary.Application.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Kitap> Kitaplar { get; }

    IGenericRepository<KitapKopya> KitapKopyalari { get; }

    IGenericRepository<Uye> Uyeler { get; }

    IGenericRepository<Odunc> Oduncler { get; }

    IGenericRepository<FormAlani> FormAlanlari { get; }

    IGenericRepository<DinamikAlanDegeri> DinamikAlanDegerleri { get; }

    IGenericRepository<Kullanici> Kullanicilar { get; }

    IGenericRepository<Rol> Roller { get; }

    IGenericRepository<KullaniciRol> KullaniciRolleri { get; }

    IGenericRepository<AuditLog> AuditLoglari { get; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task ExecuteInTransactionAsync(
        Func<Task> action,
        CancellationToken cancellationToken = default);

    Task<T> ExecuteInTransactionAsync<T>(
        Func<Task<T>> action,
        CancellationToken cancellationToken = default);
}