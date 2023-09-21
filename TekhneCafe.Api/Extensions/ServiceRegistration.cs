using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;
using TekhneCafe.Api.LoggerEnrichers;

namespace TekhneCafe.Api.Extensions
{
    public static class ServiceRegistration
    {
        public static void AddApiServices(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
            services.AddHttpContextAccessor();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddEndpointsApiExplorer();
            services.AddSingleton<UserEnricher>();
            builder.WebHost.UseSentry();

            #region Swagger Configuration
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "TekhneCafe API",
                    Version = "v1",
                    Description = "An API for TekhneCafe",
                });
                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                config.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                config.IncludeXmlComments(xmlPath);
            });
            #endregion

            #region Serilog Configuration
            //SqlColumn sqlColumn = new SqlColumn();
            //sqlColumn.ColumnName = "UserId";
            //sqlColumn.DataType = System.Data.SqlDbType.UniqueIdentifier;
            //sqlColumn.PropertyName = "UserId";
            //sqlColumn.AllowNull = true;

            //ColumnOptions columnOpt = new ColumnOptions();
            //columnOpt.AdditionalColumns = new Collection<SqlColumn> { sqlColumn };

            //using var log = new LoggerConfiguration()
            //            .Enrich.FromLogContext()
            //            .Enrich.With(new UserEnricher())
            //            .MinimumLevel.Warning()
            //            .WriteTo.Console()
            //            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
            //            .WriteTo.MSSqlServer(
            //                connectionString: builder.Configuration.GetConnectionString("EfTekhneCafeContext"),
            //                sinkOptions: new MSSqlServerSinkOptions { TableName = "LogEvents", AutoCreateSqlTable = true },
            //                columnOptions: columnOpt
            //                )
            //            .CreateLogger();
            //builder.Host.UseSerilog(log);
            #endregion

            #region Cors Configurations
            var allowedHosts = builder.Configuration.GetValue<string>("AllowedHosts");
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("_myAllowSpecificOrigins",
                    builder =>
                    {
                        builder.WithOrigins(allowedHosts) // Burada erişime izin vermek istediğiniz orijinleri belirtin
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });
            #endregion
        }
    }
}
