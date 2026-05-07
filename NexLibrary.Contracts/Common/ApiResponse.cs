namespace NexLibrary.Contracts.Common;

public class ApiResponse<T>
{
    public bool BasariliMi { get; set; }

    public string Mesaj { get; set; } = string.Empty;

    public T? Veri { get; set; }

    public List<ValidationError> Hatalar { get; set; } = new();

    public static ApiResponse<T> Success(T data, string message = "İşlem başarılı.")
    {
        return new ApiResponse<T>
        {
            BasariliMi = true,
            Mesaj = message,
            Veri = data
        };
    }

    public static ApiResponse<T> Fail(string message)
    {
        return new ApiResponse<T>
        {
            BasariliMi = false,
            Mesaj = message
        };
    }

    public static ApiResponse<T> ValidationFail(List<ValidationError> errors, string message = "Doğrulama hatası oluştu.")
    {
        return new ApiResponse<T>
        {
            BasariliMi = false,
            Mesaj = message,
            Hatalar = errors
        };
    }
}