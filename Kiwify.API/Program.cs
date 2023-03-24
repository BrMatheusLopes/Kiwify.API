using Kiwify.API.Services;
using Kiwify.Core.Data;
using Kiwify.Core.Repository;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);
var isProduction = builder.Environment.IsProduction();

// DOCKER service port
var servicePort = Environment.GetEnvironmentVariable("PORT");
if (servicePort != null)
{
    builder.WebHost.UseUrls("http://*:" + servicePort);
}

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Is(isProduction ? LogEventLevel.Debug : LogEventLevel.Debug)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .MinimumLevel.Override("System.Net.Http.HttpClient", LogEventLevel.Warning)
    .WriteTo.File(path: $"logs{Path.AltDirectorySeparatorChar}log-.txt",
                  rollingInterval: RollingInterval.Day,
                  flushToDiskInterval: TimeSpan.FromDays(14),
                  retainedFileCountLimit: 14)
    .WriteTo.Console()
    .CreateLogger();


var connectionString = Environment.GetEnvironmentVariable("postgreSQL") ?? builder.Configuration.GetConnectionString("postgreSQL");
builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<KiwifyPaymentHandler>();
builder.Services.AddControllers();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseCors();
app.UseRouting();
app.MapControllers();
app.Run();
