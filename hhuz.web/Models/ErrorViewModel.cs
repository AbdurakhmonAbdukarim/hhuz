namespace hhuz.Models;

public class ErrorViewModel
{
    /// <summary>
    /// Xatolik yuz bergan so'rovning ID raqami.
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// RequestId mavjudligini tekshiradi.
    /// </summary>
    public bool ShowRequestId =>
        !string.IsNullOrEmpty(RequestId);

    /// <summary>
    /// Xatolik haqida foydalanuvchiga ko'rsatiladigan xabar.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// HTTP status kodi.
    /// Masalan: 404, 500, 400.
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Xatolik sodir bo'lgan vaqt.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}