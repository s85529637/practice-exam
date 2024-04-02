using Serilog;
using StackExchange.Redis;
using takeanexam;
using takeanexam.InterfaceService;
using takeanexam.Service;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();
var redisConfig = configuration.GetSection("Redis").Get<Redis>();

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = ConfigurationOptions.Parse($"{redisConfig.Host}:{redisConfig.Port},password={redisConfig.Password},defaultDatabase={redisConfig.Database}");
    return ConnectionMultiplexer.Connect(configuration);
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();
builder.Services.AddScoped<RedisService>();
builder.Services.AddScoped<IWeatherForecastApiService, WeatherForecastApiService>();
builder.Services.Configure<DBConnection>(configuration.GetSection("DBConnection"));
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

app.UseMiddleware<LoggingMiddleware>();
app.Run();
