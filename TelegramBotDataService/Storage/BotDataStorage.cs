using System.Text.RegularExpressions;
using NLog;
using Npgsql;
using TelegramBotDataService.Configuration;

namespace TelegramBotDataService.Storage;

public class BotDataStorage
{
    private static readonly NLog.ILogger Logger = LogManager.GetCurrentClassLogger();

    private const int IntervalInDays = 21;
    private readonly PathConfiguration _configuration;

    public BotDataStorage(PathConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Метод, получающий конкретный log-файл, в зависимости от переданного параметра пути
    /// если не передать дату, то по умолчанию дата будет сегодняшней 
    /// </summary>
    /// <param name="cancellationToken">токен отмены операции</param>
    /// <param name="date">необязательный параметр, по умолчанию установленный на сегодняшний день</param>
    /// <returns></returns>
    public Task<Stream?> GetByDate(DateTime date, CancellationToken cancellationToken)
    {
        if (date == default)
            date = DateTime.Now.Date;

        Logger.Info("Start method: {0}", nameof(GetByDate));
        var fullPath = Path.Combine(_configuration.PathToLog!, $"{date:yyyy-MM-dd}.log");

        if (File.Exists(fullPath))
        {
            Logger.Info("File found {0}", fullPath);
            var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            return Task.FromResult(fs as Stream)!;
        }

        Logger.Info("File not exist {0}", fullPath);
        return Task.FromResult(null as Stream);
    }

    /// <summary>
    /// Метод, получающий список log-файлов, в зависимости от переданного параметра пути
    /// если не передать параметры дат, то за dateTo будет принята сегодняшняя дата,
    /// а за dateFrom дата 3 недели назад (значение по умолчанию, можно изменить путем изменения значения IntervalInDays),
    /// если передать только dateFrom, то dateTo будет считать как дата на 3 недели вперед 
    /// если передать только dateTo, то dateFrom будет считать как дата на 3 недели назад 
    /// </summary>
    /// <param name="cancellationToken">токен отмены операции</param>
    /// <param name="dateFrom">дата в формате yyyy-MM-dd, необязательный параметр, по умолчанию установлен на разницу IntervalInDays от dateTo</param>
    /// <param name="dateTo">дата в формате yyyy-MM-dd, необязательный параметр, по умолчанию установленный в текущую дату</param>
    /// <returns></returns>
    public Task<List<string>> GetListAvailableByDate(DateTime dateFrom, DateTime dateTo, CancellationToken cancellationToken)
    {
        Logger.Info("Start method: {0}", nameof(GetListAvailableByDate));

        if (dateFrom == default && dateTo == default)
        {
            // Если пользователь не указал dateFrom и dateTo, устанавливаем их значения с недельным интервалом относительно текущей даты
            dateFrom = DateTime.Now.Date.AddDays(-IntervalInDays);
            dateTo = DateTime.Now.Date;
        }
        else if (dateFrom == default)
        {
            // Если пользователь указал только dateTo, устанавливаем dateFrom на неделю назад относительно dateTo
            dateFrom = dateTo.Date.AddDays(-IntervalInDays);
        }
        else if (dateTo == default)
        {
            // Если пользователь указал только dateFrom, устанавливаем dateTo на неделю вперед относительно dateFrom
            dateTo = dateFrom.Date.AddDays(IntervalInDays);
        }

        List<string> listLogFiles = new();

        if (Directory.Exists(_configuration.PathToLog!))
        {
            var files = Directory.GetFiles(_configuration.PathToLog!);

            listLogFiles = files
                .Select(file => Regex.Match(file, @"\d{4}-\d{2}-\d{2}").Value)
                .Where(date =>
                {
                    if (DateTime.TryParse(date, out var currentDate))
                        return currentDate >= dateFrom && currentDate <= dateTo;
                    return false;
                })
                .OrderBy(_ => files)
                .ToList();

            Logger.Info("Files in the specified range are found {0}", _configuration.PathToLog!);
            return Task.FromResult(listLogFiles);
        }

        Logger.Info("Files not exist {0}", _configuration.PathToLog!);
        return Task.FromResult(listLogFiles);
    }

    /// <summary>
    /// Получает поток (Stream) для списка пользователей
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Поток (Stream) для списка пользователей или null, если файл не найден</returns>
    public Task<Stream?> GetListUsers(CancellationToken cancellationToken)
    {
        Logger.Info("Start method: {0}", nameof(GetListUsers));

        var fullPath = Path.Combine(_configuration.PathToListUsers!, "ListUsers.txt");

        if (File.Exists(fullPath))
        {
            Logger.Info("File found {0}", fullPath);
            var fileStream = new FileStream(fullPath, FileMode.Open);
            return Task.FromResult(fileStream as Stream)!;
        }

        Logger.Info("File not exist {0}", _configuration.PathToListUsers);
        return Task.FromResult(null as Stream);
    }

    /// <summary>
    /// Получает список пользователей из базы данных и возвращает их в виде списка строк
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Список строк, представляющих пользователей, или null в случае ошибки</returns>
    public async Task<List<string>?> GetListUsersFromDb(CancellationToken cancellationToken)
    {
        try
        {
            if (!_configuration.IsWorkWithDb()) return null;

            await using var connection = new NpgsqlConnection(_configuration.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string selectFromBotUsers = "SELECT * FROM botusers";

            await using var command = new NpgsqlCommand(selectFromBotUsers, connection);
            await using var reader = command.ExecuteReader();

            var listOfBotUsers = new List<string>();
            while (await reader.ReadAsync(cancellationToken))
                listOfBotUsers.Add($"{reader["name"]} {reader["surname"]} ({reader["username"]}) {reader["id"]}" + Environment.NewLine);

            return listOfBotUsers;
        }
        catch (Exception ex)
        {
            Logger.Error("Error view users list from DB. {method}: {error}", nameof(GetListUsersFromDb), ex);
            return null;
        }
    }

    /// <summary>
    /// Получает количество сообщений из базы данных и возвращает его в виде целого числа
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Количество сообщений или -1 в случае ошибки</returns>
    public async Task<long> GetCountMessageFromDb(CancellationToken cancellationToken)
    {
        try
        {
            if (!_configuration.IsWorkWithDb()) return -1;

            await using var connection = new NpgsqlConnection(_configuration.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string countMessagesFromDb = "SELECT count(*) FROM messages";

            await using var commandCountMessages = new NpgsqlCommand(countMessagesFromDb, connection);
            var countMessage = (long)(await commandCountMessages.ExecuteScalarAsync(cancellationToken))!;

            return countMessage;
        }
        catch (Exception ex)
        {
            Logger.Error("Error view count messages from DB. {method}: {error}", nameof(GetListUsersFromDb), ex);
            return -1;
        }
    }

    /// <summary>
    /// Получает информацию о последнем пользователе из базы данных и возвращает ее в виде строки
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Строка, представляющая информацию о последнем пользователе, или "No data available", если нет данных, или null в случае ошибки</returns>
    public async Task<string?> GetLastUserFromDb(CancellationToken cancellationToken)
    {
        try
        {
            if (!_configuration.IsWorkWithDb()) return null;

            await using var connection = new NpgsqlConnection(_configuration.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            const string selectLastUserFromDb = "SELECT * FROM botusers ORDER BY ctid DESC LIMIT 1;";

            await using var command = new NpgsqlCommand(selectLastUserFromDb, connection);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            if (!reader.HasRows || !await reader.ReadAsync(cancellationToken)) return "No data available";

            return $"{reader["name"]} {reader["surname"]} ({reader["username"]}) {reader["id"]}";
        }
        catch (Exception ex)
        {
            Logger.Error("Error view last user from DB. {method}: {error}", nameof(GetListUsersFromDb), ex);
            return null;
        }
    }
}