namespace TelegramBotDataService.Configuration;

/// <summary>
/// Класс, предоставляющий собой конфигурацию хранилища,
/// имеет одно свойство Directory типа PathConfiguration,
/// которое содержит в себе все необходимые пути для осуществляния работы сервиса
/// </summary>
public class StorageConfiguration
{
    public PathConfiguration? Directory { get; init; }
    
    //TODO: сделать метод проверяющий данные на правильность
}
/// <summary>
/// Класс имеющий 3 свойства, которые представляю собой:
/// PathDirectoryToLog - путь до log-файлов бота
/// PathDirectoryToListUsers - путь до файла с пользователями бота
/// PathDirectoryToServiceLog - путь до log-файлов сервиса
/// </summary>
public class PathConfiguration
{
    public string? PathDirectoryToLog { get; init; }
    public string? PathDirectoryToListUsers { get; init; }
    public string? PathDirectoryToServiceLog { get; init; }
}