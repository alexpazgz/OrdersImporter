using Asp.Versioning;
using Businnes.Clients;
using Businnes.Common.Interfaces;
using Businnes.Csv;
using Businnes.FireForgetService;
using Businnes.Implementations;
using Businnes.Interfaces;
using Businnes.Repositories.Interfaces;
using Domain.Configuration;
using Domain.Configuration.Clients;
using Domain.Response;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Z.EntityFramework.Extensions;

namespace WebApi.Configuration
{
    [ExcludeFromCodeCoverage]
    public static class CustomServiceCollectionExtensions
    {
        /// <summary>
        /// Inyecta el versionado de la API
        /// </summary>
        public static IServiceCollection AddVersioning(this IServiceCollection services, IConfiguration configuration)
        {
            // Version format: 'v'major[.minor][-status]
            services.AddApiVersioning(
                options =>
                {
                    options.DefaultApiVersion = new ApiVersion(1);
                    options.ReportApiVersions = true;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.ApiVersionReader = ApiVersionReader.Combine(
                        new UrlSegmentApiVersionReader()
                        /*new HeaderApiVersionReader("X-Api-Version")*/);
                }).AddApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'V";
                    options.SubstituteApiVersionInUrl = true;
                });

            return services;
        }

        /// <summary>
        /// Inyecta las entidades del fichero de configuración necesarias
        /// </summary>
        public static IServiceCollection AddCustomConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RequestLocalizationOptions>(
               opts =>
               {
                   var supportedCultures = new List<CultureInfo>
                   {
                        new CultureInfo("es"),
                        new CultureInfo("es-ES")
                   };

                   opts.DefaultRequestCulture = new RequestCulture("es");
                   opts.SetDefaultCulture("es");
                   // Formatting numbers, dates, etc.
                   opts.SupportedCultures = supportedCultures;
                   // UI strings that we have localized.
                   opts.SupportedUICultures = supportedCultures;
               });
            services.Configure<AppSettingsConfiguration>(configuration.GetSection("AppSettings"));
            services.Configure<ApiKataEsPublicoConfiguration>(configuration.GetSection(ApiKataEsPublicoConfiguration.SectionName));

            return services;
        }

        /// <summary>
        /// Inyecta las entidades de la capa de dominio necesarias
        /// </summary>
        public static IServiceCollection AddDomainLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IResponseService, ResponseService>();


            return services;
        }

        /// <summary>
        /// Inyecta las entidades de la capa de aplicación necesarias
        /// </summary>
        public static IServiceCollection AddBusinnesLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IOnlineOrderService, OnlineOrderService>();
            services.AddScoped<IOrderImporterService, OrderImporterService>();
            services.AddScoped<IApiKataEsPublicoService, ApiKataEsPublicoService>();
            services.AddScoped<IApiKataEsPublicoClient, ApiKataEsPublicoClient>();
            services.AddScoped<IFileGeneratorService, FileGeneratorService>();
            services.AddScoped<ICsvService, CsvService>();
            services.AddTransient<IFireForgetService, FireForgetService>();
            services.AddScoped<ISummaryService, SummaryService>();


            return services;
        }

        /// <summary>
        /// Inyecta las clases de los servicios SOA y clientes Http de la capa de persistencia
        /// </summary>
        public static IServiceCollection AddSoapServiceReference(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddHttpClient();

            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            bool.TryParse(configuration.GetSection("UseInMemoryDatabase").Value, out bool memoryDatabase);

            if (memoryDatabase)
            {
                services.AddDbContext<ApplicationDbContext>(options =>
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
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)), ServiceLifetime.Transient);
            }

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            services.AddScoped<IOrderRepository, OrderRepository>();

            return services;
        }

    }
}
