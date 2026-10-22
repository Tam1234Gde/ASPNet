namespace HIDE.RestAndRant.App_Notify_Booking;

/// <summary>
/// Конфигурация сервера уведомлений для Firebase
/// </summary>
public static class AppConfig_FirebaseAuthParams
{
    /// <summary>
    /// Приватный ключ доступа (RSA)
    /// </summary>
    public static string PrivateKey { get; set; } = null!;

    /// <summary>
    /// Данные клиента под которыми будет происходить авторизация к серверу
    /// </summary>
    public static string ClientData { get; set; } = null!;

    /// <summary>
    /// Ссылка для получения токена авторизации
    /// </summary>
    public static string TokenUri { get; set; } = null!;

    /// <summary>
    /// Идентификатор проекта
    /// </summary>
    public static string ProjectId { get; set; } = null!;
}