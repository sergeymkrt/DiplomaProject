using DiplomaProject.Application;
using DiplomaProject.Application.Extensions;
using DiplomaProject.Domain.AggregatesModel.Directories;
using DiplomaProject.Domain.AggregatesModel.FileAggregate;
using DiplomaProject.Domain.AggregatesModel.Groups;
using DiplomaProject.Domain.AggregatesModel.Keys;
using DiplomaProject.Domain.Entities.User;
using DiplomaProject.Domain.Services.DomainServices.Directories;
using DiplomaProject.Domain.Services.DomainServices.Files;
using DiplomaProject.Domain.Services.DomainServices.Groups;
using DiplomaProject.Domain.Services.DomainServices.Keys;
using DiplomaProject.Domain.Services.Encryption;
using DiplomaProject.Infrastructure.Persistence.Extensions;
using DiplomaProject.Infrastructure.Shared.Configs;
using DiplomaProject.Infrastructure.Shared.Encryption;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography;

namespace DiplomaProject.WebApi.Extensions;

public static class ConfigureServices
{
    public static IServiceCollection AddWebAPIServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddHealthChecks();
        services.AddControllers();

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "DiplomaProject", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder => builder
                    .WithOrigins("http://127.0.0.1:5173", "http://localhost:5173")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());

            options.AddPolicy("Production",
                builder => builder
                    .WithOrigins("http://localhost:5173", "https://diploma-project-frontend.azurewebsites.net", "https://diploma-project-api.azurewebsites.net")
                    // .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());

            options.DefaultPolicyName = "CorsPolicy";
        });


        return services;
    }

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionsString = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ?
            configuration.GetConnectionString("DefaultConnection")
            : Environment.GetEnvironmentVariable("SQLAZURECONNSTR");

        services.AddSqlServerDbContext<AppContext>(connectionsString, true);

        services.AddIdentity<User, Role>()
            .AddRoles<Role>()
            .AddEntityFrameworkStores<AppContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IDbInitializer, DbInitializer>();

        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<IKeyRepository, KeyRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<IDirectoryRepository, DirectoryRepository>();

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMapster();

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie(x =>
            {
                x.Cookie.Name = "authorization";
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;

                var securityKey = new ECDsaSecurityKey(ECDsa.Create(ECCurve.NamedCurves.nistP521));
                securityKey.ECDsa.ImportFromPem(configuration["JWT:PrivateKey"]);

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:ValidAudience"],
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = securityKey
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies.ContainsKey("authorization"))
                        {
                            context.Token = context.Request.Cookies["authorization"];
                        }
                        return Task.CompletedTask;
                    }
                };
            });

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 12;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
        });

        services.AddAuthorizationBuilder()
            .AddPolicy("CanPurge", policy => policy.RequireRole(Domain.Enums.Role.ADMIN.ToString("F")));

        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IEncryptionService, EncryptionService>();
        services.AddScoped<IFileManagementService, AzureStorageManagementService>();
        services.AddScoped<ICurrentUser, CurrentUser>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));

        services.AddScoped<IFileDomainService, FileDomainService>();
        services.AddScoped<IKeyDomainService, KeyDomainService>();
        services.AddScoped<IGroupDomainService, GroupDomainService>();
        services.AddScoped<IDirectoryDomainService, DirectoryDomainService>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(BaseCommand).GetTypeInfo().Assembly));


        return services;
    }

    public static IServiceCollection ConfigureExternalServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions();

        services.Configure<JwtConfig>(configuration.GetSection("JWT"));
        services.Configure<AzureStorageConfig>(configuration.GetSection("AzureStorage"));

        return services;
    }
}