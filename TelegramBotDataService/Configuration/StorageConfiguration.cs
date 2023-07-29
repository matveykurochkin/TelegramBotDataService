namespace TelegramBotDataService.Configuration;

/// <summary>
/// Класс, предоставляющий собой конфигурацию хранилища,
/// имеет одно свойство Directory типа PathConfiguration,
/// которое содержит в себе все необходимые пути для осуществляния работы сервиса
/// и метод ValidateConfiguration, который проверяет валидность файла конфигурации
/// </summary>
public class StorageConfiguration
{
    public PathConfiguration? Directory { get; init; }

    /// <summary>
    /// Метод, проверяющий на пустоту пути, необходимых для работы приложения, в файле конфигурации
    /// </summary>
    /// <returns>true, если все проверки выполнены успешно</returns>
    internal bool ValidateConfiguration()
    {
        if (string.IsNullOrEmpty(Directory!.PathDirectoryToLog))
            throw new InvalidOperationException("Path to bot log files is not set. Please provide a valid path.");

        if (string.IsNullOrEmpty(Directory!.PathDirectoryToListUsers))
            throw new InvalidOperationException("Path to bot user list file is not set. Please provide a valid path.");

        if (string.IsNullOrEmpty(Directory!.PathDirectoryToServiceLog))
            throw new InvalidOperationException("Path to service log files is not set. Please provide a valid path.");

        return true;
    }
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