using Businnes.AutoMapper;
using Businnes.Clients;
using Businnes.Common.Interfaces;
using Businnes.Csv;
using Businnes.FireForgetService;
using Businnes.Implementations;
using Businnes.Interfaces;
using Businnes.Repositories.Interfaces;
using Domain.Configuration;
using Domain.Configuration.Clients;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrdersImporterJob.Jobs;
using Z.EntityFramework.Extensions;

public class Program
{
    static async Task Main(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?? "Production"}.json")
            .Build();


        var hostBuilder = CreateDefaultApp(args, configuration);

        using var loggerFactory = CreateLoggerFactory();
        ILogger logger = loggerFactory.CreateLogger<Program>();

        logger.LogInformation("Start Program");

        var host = hostBuilder.Build();

        var job = ActivatorUtilities.CreateInstance<JobGetOrders>(host.Services);
        await job.Execute(args);

        logger.LogInformation("End Program");

        Console.ReadKey();

    }

    private static IHostBuilder CreateDefaultApp(string[] args, IConfiguration configuration)
    {
        var builder = Host.CreateDefaultBuilder();

        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
        });

        builder.ConfigureServices(conf =>
        {
            conf.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            conf.AddHttpClient();

            conf.AddAutoMapper(typeof(MappingProfile));

            conf.Configure<AppSettingsConfiguration>(configuration.GetSection("AppSettings"));
            conf.Configure<ApiKataEsPublicoConfiguration>(configuration.GetSection(ApiKataEsPublicoConfiguration.SectionName));

            //Businnes
            conf.AddScoped<IOnlineOrderService, OnlineOrderService>();
            conf.AddScoped<IOrderImporterService, OrderImporterService>();
            conf.AddScoped<IApiKataEsPublicoService, ApiKataEsPublicoService>();
            conf.AddScoped<IApiKataEsPublicoClient, ApiKataEsPublicoClient>();
            conf.AddScoped<IFileGeneratorService, FileGeneratorService>();
            conf.AddScoped<ICsvService, CsvService>();
            conf.AddTransient<IFireForgetService, FireForgetService>();
            conf.AddScoped<ISummaryService, SummaryService>();

            bool.TryParse(configuration.GetSection("UseInMemoryDatabase").Value, out bool memoryDatabase);

            if (memoryDatabase)
            {
                conf.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("InMemoryOrderImporterTest"));

                EntityFrameworkManager.ContextFactory = context =>
                {
                    var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                    optionsBuilder.UseInMemoryDatabase("InMemoryOrderImporterTest");
                    return new ApplicationDbContext(optionsBuilder.Options);
                };
            }
            else
            {
                conf.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)), ServiceLifetime.Transient);
            }

            conf.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            conf.AddScoped<IOrderRepository, OrderRepository>();

        });
        builder.UseConsoleLifetime();

        return builder;
    }

    private static ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddConsole();
        });
    }
}