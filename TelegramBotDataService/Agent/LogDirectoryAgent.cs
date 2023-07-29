using System.Text.RegularExpressions;
using NLog;

namespace TelegramBotDataService.Agent;

internal class LogDirectoryAgent
{
    private static readonly NLog.ILogger Logger = LogManager.GetCurrentClassLogger();

    private const int IntervalInDays = 21;

    /// <summary>
    /// Метод, получающий конкретный log-файл, в зависимости от переданного параметра пути
    /// если не передать дату, то по умолчанию дата будет сегодняшней 
    /// </summary>
    /// <param name="pathDirectory">путь до папки с log-файлами</param>
    /// <param name="cancellationToken">токен отмены операции</param>
    /// <param name="date">необязательный параметр, по умолчанию установленный на сегодняшний день</param>
    /// <returns></returns>
    public Task<Stream?> LogFileByDate(string pathDirectory, CancellationToken cancellationToken, DateTime date = default)
    {
        if (date == default)
            date = DateTime.Now.Date;

        Logger.Info("Start method: {0}", nameof(LogFileByDate));
        var fullPath = Path.Combine(pathDirectory, $"{date:yyyy-MM-dd}.log");

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
    /// <param name="pathDirectory">абсолютный путь до директории где находятся log-файлы</param>
    /// <param name="cancellationToken">токен отмены операции</param>
    /// <param name="dateFrom">дата в формате yyyy-MM-dd, необязательный параметр, по умолчанию установлен на разницу IntervalInDays от dateTo</param>
    /// <param name="dateTo">дата в формате yyyy-MM-dd, необязательный параметр, по умолчанию установленный в текущую дату</param>
    /// <returns></returns>
    public Task<List<string>> ListAvailableLogFileByDate(string pathDirectory, CancellationToken cancellationToken, DateTime dateFrom = default, DateTime dateTo = default)
    {
        Logger.Info("Start method: {0}", nameof(ListAvailableLogFileByDate));

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

        if (Directory.Exists(pathDirectory))
        {
            var files = Directory.GetFiles(pathDirectory);

            listLogFiles = files
                .Select(file => Regex.Match(file, @"\d{4}-\d{2}-\d{2}").Value)
                .Where(date =>
                {
                    if (DateTime.TryParse(date, out var currentDate))
                        return currentDate >= dateFrom && currentDate <= dateTo;
                    return false;
                })
                .ToList();

            Logger.Info("Files in the specified range are found {0}", pathDirectory);
            return Task.FromResult(listLogFiles);
        }

        Logger.Info("Files not exist {0}", pathDirectory);
        return Task.FromResult(listLogFiles);
    }
}