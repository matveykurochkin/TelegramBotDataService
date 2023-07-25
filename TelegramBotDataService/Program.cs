using Microsoft.Extensions.Options;
using NLog.Web;
using TelegramBotDataService.Configuration;
using TelegramBotDataService.Storage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.WebHost.ConfigureKestrel((context, serverOptions) =>
{
    var kestrelSection = context.Configuration.GetSection("Http");
    serverOptions.Configure(kestrelSection);
});

builder.Host.ConfigureServices((hostBuilderContext, serviceCollection) =>
{
    serviceCollection.AddOptions<StorageConfiguration>()
        .Bind(hostBuilderContext.Configuration.GetSection("Storage"));

    serviceCollection.AddSingleton<FileStorage>(sp =>
    {
        var options = sp.GetRequiredService<IOptions<StorageConfiguration>>();
        var storageConfiguration = options.Value;

        var fileStorage = new FileStorage(storageConfiguration.Directory!);

        return fileStorage;
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();