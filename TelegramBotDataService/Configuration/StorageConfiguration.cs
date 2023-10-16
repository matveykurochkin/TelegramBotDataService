using System.Data;
using NLog;
using Npgsql;

namespace TelegramBotDataService.Configuration;

/// <summary>
/// Класс, предоставляющий собой конфигурацию хранилища,
/// имеет одно свойство Directory типа PathConfiguration,
/// которое содержит в себе все необходимые пути для осуществляния работы сервиса
/// и метод ValidateConfiguration, который проверяет валидность файла конфигурации
/// </summary>
public class StorageConfiguration
{

    public PathConfiguration? Storages { get; init; }

    /// <summary>
    /// Метод, проверяющий на пустоту пути, необходимых для работы приложения, в файле конфигурации
    /// </summary>
    /// <returns>true, если все проверки выполнены успешно</returns>
    internal bool ValidateConfiguration()
    {
        if (string.IsNullOrEmpty(Storages!.PathToLog))
            throw new InvalidOperationException("Path to bot log files is not set. Please provide a valid path.");

        if (string.IsNullOrEmpty(Storages!.PathToListUsers))
            throw new InvalidOperationException("Path to bot user list file is not set. Please provide a valid path.");
        
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
    private static readonly NLog.ILogger Logger = LogManager.GetCurrentClassLogger();
    public string? PathToLog { get; init; }
    public string? PathToListUsers { get; init; }
    public string? ConnectionString { get; init; }
    
    /// <summary>
    /// Метод, для проверки строки подключения не пустая ли они и правильного ли формата
    /// </summary>
    /// <returns></returns>
    public bool IsWorkWithDb()
    {
        if (string.IsNullOrEmpty(ConnectionString))
            return false; // Если строка подключения пуста, не выводим ошибку, а сразу возвращаем false

        try
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();
            return connection.State == ConnectionState.Open;
        }
        catch (Exception ex)
        {
            Logger.Error($"The error is in the method that checks the connection to the database, " +
                         $"it occurs if you tried to connect the bot to the database, but this did not happen, " +
                         $"the bot works in the default mode (working with files). Error message: {ex.Message}");
        }

        return false;
    }
}