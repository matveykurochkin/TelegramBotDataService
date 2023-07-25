namespace TelegramBotDataService.Configuration;

public class StorageConfiguration
{
    public FileStorageConfiguration? Directory { get; init; }
}

public class FileStorageConfiguration
{
    public string? PathDirectory { get; init; }
}