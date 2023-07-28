namespace TelegramBotDataService.Storage;

internal interface ILogStorage
{
    /// <summary>
    /// Метод для получения log-файла по дате 
    /// </summary>
    /// <param name="date">дата в формате yyyy-MM-dd</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<Stream?> GetByDate(DateTime date, CancellationToken cancellationToken);

    /// <summary>
    /// Метод возвращающий список доступных сервисных log-файлов, лежащих в промежутке указанных дат
    /// </summary>
    /// <param name="dateFrom">дата в формате yyyy-MM-dd</param>
    /// <param name="dateTo">дата в формате yyyy-MM-dd</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<List<string>> GetListAvailableByDate(DateTime dateFrom, DateTime dateTo, CancellationToken cancellationToken);
}