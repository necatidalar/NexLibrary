using System.Globalization;
using Microsoft.EntityFrameworkCore;
using NexLibrary.Application.Interfaces.Repositories;
using NexLibrary.Application.Interfaces.Services;
using NexLibrary.Contracts.Books;
using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.DynamicForms;
using NexLibrary.Domain.Entities;
using NexLibrary.Domain.Enums;

namespace NexLibrary.Application.Services;

public sealed class BookService : IBookService
{
    private readonly IUnitOfWork _unitOfWork;

    public BookService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<PagedResponse<BookListResponse>>> GetPagedAsync(
        int pageNumber = 1,
        int pageSize = 20,
        string? search = null,
        CancellationToken cancellationToken = default)
    {
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 20 : pageSize;

        var query = _unitOfWork.Kitaplar
            .Query()
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x => x.KitapAdi.Contains(search));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var books = await query
            .OrderByDescending(x => x.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var bookIds = books.Select(x => x.Id).ToList();

        var dynamicValues = await _unitOfWork.DinamikAlanDegerleri
            .Query()
            .AsNoTracking()
            .Include(x => x.FormAlani)
            .Where(x =>
                x.ModulKodu == ModulKodu.Kitaplar &&
                bookIds.Contains(x.KayitId) &&
                x.AktifMi &&
                x.FormAlani.AktifMi &&
                x.FormAlani.ListedeGorunsunMu)
            .ToListAsync(cancellationToken);

        var items = books.Select(book =>
        {
            var values = dynamicValues
                .Where(x => x.KayitId == book.Id)
                .OrderBy(x => x.FormAlani.SiraNo)
                .ToDictionary(
                    x => x.FormAlani.AlanAdi,
                    x => GetValueAsString(x));

            return new BookListResponse
            {
                Id = book.Id,
                KitapAdi = book.KitapAdi,
                AktifMi = book.AktifMi,
                OlusturmaTarihi = book.OlusturmaTarihi,
                DinamikAlanlar = values
            };
        }).ToList();

        var response = new PagedResponse<BookListResponse>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };

        return ApiResponse<PagedResponse<BookListResponse>>.Success(response);
    }

    public async Task<ApiResponse<BookDetailResponse>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var book = await _unitOfWork.Kitaplar
            .Query()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (book is null)
        {
            return ApiResponse<BookDetailResponse>.Fail("Kitap bulunamadı.");
        }

        var dynamicValues = await _unitOfWork.DinamikAlanDegerleri
            .Query()
            .AsNoTracking()
            .Include(x => x.FormAlani)
            .Where(x =>
                x.ModulKodu == ModulKodu.Kitaplar &&
                x.KayitId == id &&
                x.AktifMi &&
                x.FormAlani.AktifMi)
            .OrderBy(x => x.FormAlani.SiraNo)
            .ToListAsync(cancellationToken);

        var response = new BookDetailResponse
        {
            Id = book.Id,
            KitapAdi = book.KitapAdi,
            AktifMi = book.AktifMi,
            OlusturmaTarihi = book.OlusturmaTarihi,
            GuncellemeTarihi = book.GuncellemeTarihi,
            DinamikAlanlar = dynamicValues.Select(x => new DynamicFieldValueResponse
            {
                FormAlaniId = x.FormAlaniId,
                AlanKodu = x.FormAlani.AlanKodu,
                AlanAdi = x.FormAlani.AlanAdi,
                AlanTipi = x.FormAlani.AlanTipi.ToString(),
                Deger = GetValueAsString(x)
            }).ToList()
        };

        return ApiResponse<BookDetailResponse>.Success(response);
    }

    public async Task<ApiResponse<BookDetailResponse>> CreateAsync(
        BookCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        var validationErrors = await ValidateBookRequestAsync(
            request.KitapAdi,
            request.DinamikAlanlar,
            cancellationToken);

        if (validationErrors.Count > 0)
        {
            return ApiResponse<BookDetailResponse>.ValidationFail(validationErrors);
        }

        BookDetailResponse? createdBook = null;

        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var book = new Kitap
            {
                KitapAdi = request.KitapAdi.Trim(),
                AktifMi = true,
                OlusturmaTarihi = DateTime.UtcNow
            };

            await _unitOfWork.Kitaplar.AddAsync(book, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var activeFields = await GetActiveBookDynamicFieldsAsync(cancellationToken);

            foreach (var fieldValue in request.DinamikAlanlar)
            {
                var field = activeFields.FirstOrDefault(x =>
                    x.AlanKodu.Equals(fieldValue.AlanKodu, StringComparison.OrdinalIgnoreCase));

                if (field is null || field.SistemAlaniMi)
                {
                    continue;
                }

                var dynamicValue = CreateDynamicValueEntity(book.Id, field, fieldValue.Deger);

                if (dynamicValue is not null)
                {
                    await _unitOfWork.DinamikAlanDegerleri.AddAsync(dynamicValue, cancellationToken);
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var detailResult = await GetByIdAsync(book.Id, cancellationToken);
            createdBook = detailResult.Veri;
        }, cancellationToken);

        return ApiResponse<BookDetailResponse>.Success(
            createdBook!,
            "Kitap başarıyla oluşturuldu.");
    }

    public async Task<ApiResponse<BookDetailResponse>> UpdateAsync(
        int id,
        BookUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        var book = await _unitOfWork.Kitaplar.GetByIdAsync(id, cancellationToken);

        if (book is null)
        {
            return ApiResponse<BookDetailResponse>.Fail("Kitap bulunamadı.");
        }

        var validationErrors = await ValidateBookRequestAsync(
            request.KitapAdi,
            request.DinamikAlanlar,
            cancellationToken);

        if (validationErrors.Count > 0)
        {
            return ApiResponse<BookDetailResponse>.ValidationFail(validationErrors);
        }

        BookDetailResponse? updatedBook = null;

        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            book.KitapAdi = request.KitapAdi.Trim();
            book.AktifMi = request.AktifMi;
            book.GuncellemeTarihi = DateTime.UtcNow;

            _unitOfWork.Kitaplar.Update(book);

            var activeFields = await GetActiveBookDynamicFieldsAsync(cancellationToken);

            var existingValues = await _unitOfWork.DinamikAlanDegerleri
                .Query()
                .Where(x => x.ModulKodu == ModulKodu.Kitaplar && x.KayitId == id)
                .ToListAsync(cancellationToken);

            foreach (var fieldValue in request.DinamikAlanlar)
            {
                var field = activeFields.FirstOrDefault(x =>
                    x.AlanKodu.Equals(fieldValue.AlanKodu, StringComparison.OrdinalIgnoreCase));

                if (field is null || field.SistemAlaniMi)
                {
                    continue;
                }

                var existingValue = existingValues.FirstOrDefault(x => x.FormAlaniId == field.Id);

                if (string.IsNullOrWhiteSpace(fieldValue.Deger))
                {
                    if (existingValue is not null)
                    {
                        ClearDynamicValue(existingValue);
                        existingValue.GuncellemeTarihi = DateTime.UtcNow;
                        _unitOfWork.DinamikAlanDegerleri.Update(existingValue);
                    }

                    continue;
                }

                if (existingValue is null)
                {
                    var newValue = CreateDynamicValueEntity(id, field, fieldValue.Deger);

                    if (newValue is not null)
                    {
                        await _unitOfWork.DinamikAlanDegerleri.AddAsync(newValue, cancellationToken);
                    }
                }
                else
                {
                    SetDynamicValue(existingValue, field, fieldValue.Deger);
                    existingValue.AktifMi = true;
                    existingValue.GuncellemeTarihi = DateTime.UtcNow;

                    _unitOfWork.DinamikAlanDegerleri.Update(existingValue);
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var detailResult = await GetByIdAsync(id, cancellationToken);
            updatedBook = detailResult.Veri;
        }, cancellationToken);

        return ApiResponse<BookDetailResponse>.Success(
            updatedBook!,
            "Kitap başarıyla güncellendi.");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var book = await _unitOfWork.Kitaplar.GetByIdAsync(id, cancellationToken);

        if (book is null)
        {
            return ApiResponse<bool>.Fail("Kitap bulunamadı.");
        }

        book.AktifMi = false;
        book.GuncellemeTarihi = DateTime.UtcNow;

        _unitOfWork.Kitaplar.Update(book);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<bool>.Success(true, "Kitap pasif hale getirildi.");
    }

    private async Task<List<FormAlani>> GetActiveBookDynamicFieldsAsync(
        CancellationToken cancellationToken)
    {
        return await _unitOfWork.FormAlanlari
            .Query()
            .Where(x => x.ModulKodu == ModulKodu.Kitaplar && x.AktifMi)
            .OrderBy(x => x.SiraNo)
            .ToListAsync(cancellationToken);
    }

    private async Task<List<ValidationError>> ValidateBookRequestAsync(
        string kitapAdi,
        List<DynamicFieldValueRequest> dynamicValues,
        CancellationToken cancellationToken)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(kitapAdi))
        {
            errors.Add(new ValidationError
            {
                Field = nameof(BookCreateRequest.KitapAdi),
                Message = "Kitap adı zorunludur."
            });
        }
        else if (kitapAdi.Length > 200)
        {
            errors.Add(new ValidationError
            {
                Field = nameof(BookCreateRequest.KitapAdi),
                Message = "Kitap adı en fazla 200 karakter olabilir."
            });
        }

        var activeFields = await GetActiveBookDynamicFieldsAsync(cancellationToken);

        var dynamicFieldDictionary = dynamicValues
            .GroupBy(x => x.AlanKodu, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(x => x.Key, x => x.Last().Deger, StringComparer.OrdinalIgnoreCase);

        foreach (var field in activeFields.Where(x => !x.SistemAlaniMi))
        {
            dynamicFieldDictionary.TryGetValue(field.AlanKodu, out var value);

            if (field.ZorunluMu && string.IsNullOrWhiteSpace(value))
            {
                errors.Add(new ValidationError
                {
                    Field = field.AlanKodu,
                    Message = $"{field.AlanAdi} alanı zorunludur."
                });

                continue;
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                continue;
            }

            if (field.MinimumKarakter.HasValue && value.Length < field.MinimumKarakter.Value)
            {
                errors.Add(new ValidationError
                {
                    Field = field.AlanKodu,
                    Message = $"{field.AlanAdi} en az {field.MinimumKarakter.Value} karakter olmalıdır."
                });
            }

            if (field.MaksimumKarakter.HasValue && value.Length > field.MaksimumKarakter.Value)
            {
                errors.Add(new ValidationError
                {
                    Field = field.AlanKodu,
                    Message = $"{field.AlanAdi} en fazla {field.MaksimumKarakter.Value} karakter olmalıdır."
                });
            }

            if (!IsValueCompatibleWithFieldType(field.AlanTipi, value))
            {
                errors.Add(new ValidationError
                {
                    Field = field.AlanKodu,
                    Message = $"{field.AlanAdi} alanı {field.AlanTipi} tipine uygun değil."
                });
            }

            if (field.BenzersizMi)
            {
                var isUniqueViolation = await IsUniqueViolationAsync(field, value, cancellationToken);

                if (isUniqueViolation)
                {
                    errors.Add(new ValidationError
                    {
                        Field = field.AlanKodu,
                        Message = $"{field.AlanAdi} değeri daha önce kullanılmış."
                    });
                }
            }
        }

        return errors;
    }

    private async Task<bool> IsUniqueViolationAsync(
        FormAlani field,
        string value,
        CancellationToken cancellationToken)
    {
        var normalizedValue = value.Trim();

        return field.AlanTipi switch
        {
            AlanTipi.Sayi or AlanTipi.OndalikliSayi or AlanTipi.Para
                => TryParseDecimal(normalizedValue, out var decimalValue) &&
                   await _unitOfWork.DinamikAlanDegerleri.AnyAsync(
                       x => x.FormAlaniId == field.Id && x.DegerSayi == decimalValue && x.AktifMi,
                       cancellationToken),

            AlanTipi.Tarih or AlanTipi.TarihSaat
                => DateTime.TryParse(normalizedValue, out var dateValue) &&
                   await _unitOfWork.DinamikAlanDegerleri.AnyAsync(
                       x => x.FormAlaniId == field.Id && x.DegerTarih == dateValue && x.AktifMi,
                       cancellationToken),

            AlanTipi.EvetHayir
                => TryParseBool(normalizedValue, out var boolValue) &&
                   await _unitOfWork.DinamikAlanDegerleri.AnyAsync(
                       x => x.FormAlaniId == field.Id && x.DegerBool == boolValue && x.AktifMi,
                       cancellationToken),

            _ => await _unitOfWork.DinamikAlanDegerleri.AnyAsync(
                x => x.FormAlaniId == field.Id && x.DegerMetin == normalizedValue && x.AktifMi,
                cancellationToken)
        };
    }

    private static DinamikAlanDegeri? CreateDynamicValueEntity(
        int bookId,
        FormAlani field,
        string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var entity = new DinamikAlanDegeri
        {
            ModulKodu = ModulKodu.Kitaplar,
            KayitId = bookId,
            FormAlaniId = field.Id,
            AktifMi = true,
            OlusturmaTarihi = DateTime.UtcNow
        };

        SetDynamicValue(entity, field, value);

        return entity;
    }

    private static void SetDynamicValue(
        DinamikAlanDegeri entity,
        FormAlani field,
        string? value)
    {
        ClearDynamicValue(entity);

        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        var trimmedValue = value.Trim();

        switch (field.AlanTipi)
        {
            case AlanTipi.Sayi:
            case AlanTipi.OndalikliSayi:
            case AlanTipi.Para:
                if (TryParseDecimal(trimmedValue, out var decimalValue))
                {
                    entity.DegerSayi = decimalValue;
                }
                break;

            case AlanTipi.Tarih:
            case AlanTipi.TarihSaat:
                if (DateTime.TryParse(trimmedValue, out var dateValue))
                {
                    entity.DegerTarih = dateValue;
                }
                break;

            case AlanTipi.EvetHayir:
                if (TryParseBool(trimmedValue, out var boolValue))
                {
                    entity.DegerBool = boolValue;
                }
                break;

            case AlanTipi.CokluListe:
                entity.DegerJson = trimmedValue;
                break;

            default:
                entity.DegerMetin = trimmedValue;
                break;
        }
    }

    private static void ClearDynamicValue(DinamikAlanDegeri entity)
    {
        entity.DegerMetin = null;
        entity.DegerSayi = null;
        entity.DegerTarih = null;
        entity.DegerBool = null;
        entity.DegerJson = null;
    }

    private static string? GetValueAsString(DinamikAlanDegeri entity)
    {
        if (entity.DegerMetin is not null)
        {
            return entity.DegerMetin;
        }

        if (entity.DegerSayi.HasValue)
        {
            return entity.DegerSayi.Value.ToString(CultureInfo.InvariantCulture);
        }

        if (entity.DegerTarih.HasValue)
        {
            return entity.DegerTarih.Value.ToString("yyyy-MM-dd HH:mm:ss");
        }

        if (entity.DegerBool.HasValue)
        {
            return entity.DegerBool.Value ? "true" : "false";
        }

        if (entity.DegerJson is not null)
        {
            return entity.DegerJson;
        }

        return null;
    }

    private static bool IsValueCompatibleWithFieldType(
        AlanTipi fieldType,
        string value)
    {
        return fieldType switch
        {
            AlanTipi.Sayi or AlanTipi.OndalikliSayi or AlanTipi.Para
                => TryParseDecimal(value, out _),

            AlanTipi.Tarih or AlanTipi.TarihSaat
                => DateTime.TryParse(value, out _),

            AlanTipi.EvetHayir
                => TryParseBool(value, out _),

            _ => true
        };
    }

    private static bool TryParseDecimal(string value, out decimal result)
    {
        return decimal.TryParse(
                   value,
                   NumberStyles.Any,
                   CultureInfo.GetCultureInfo("tr-TR"),
                   out result)
               ||
               decimal.TryParse(
                   value,
                   NumberStyles.Any,
                   CultureInfo.InvariantCulture,
                   out result);
    }

    private static bool TryParseBool(string value, out bool result)
    {
        var normalizedValue = value.Trim().ToLowerInvariant();

        if (normalizedValue is "true" or "1" or "evet" or "e")
        {
            result = true;
            return true;
        }

        if (normalizedValue is "false" or "0" or "hayır" or "hayir" or "h")
        {
            result = false;
            return true;
        }

        result = false;
        return false;
    }
}