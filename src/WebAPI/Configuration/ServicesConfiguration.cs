using CoursesWebAPI.Core.Contracts.Repositories;
using CoursesWebAPI.Core.Identity;
using CoursesWebAPI.Application.Contracts.Data;
using CoursesWebAPI.Application.Contracts.EntityServices;
using CoursesWebAPI.Application.Contracts.IdentityServices;
using CoursesWebAPI.Application.Services.EntityServices;
using CoursesWebAPI.Application.Services.IdentityServices;
using CoursesWebAPI.Infrastructure.Authentication;
using CoursesWebAPI.Infrastructure.ConfigurationModels;
using CoursesWebAPI.Persistence;
using CoursesWebAPI.Persistence.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Text;
using CoursesWebAPI.Infrastructure.Authorization;
using CoursesWebAPI.Application.Contracts.ExternalServices;
using CoursesWebAPI.Infrastructure.Services;

namespace WebAPI.Configuration;

public static class ServicesConfiguration
{
    public static void ConfigureCors(this IServiceCollection services) =>
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .WithExposedHeaders("X-Pagination"));
        });

    public static void ConfigureIISIntegration(this IServiceCollection services) =>
        services.Configure<IISOptions>(options => { });

    public static void ConfigureLoggerService(this IHostBuilder host, IConfiguration configuration) =>
        host.UseSerilog(new LoggerConfiguration()
                        .ReadFrom.Configuration(configuration)
                        .CreateLogger());

    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
        services.AddDbContextPool<ApplicationDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("DatabaseConnection")));

    public static void ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, Role>(o =>
        {
            o.SignIn.RequireConfirmedEmail = true;

            o.Password.RequireDigit = false; //for dev
            o.Password.RequireLowercase = false; //for dev
            o.Password.RequireUppercase = false; //for dev
            o.Password.RequireNonAlphanumeric = false; //for dev
            o.Password.RequiredLength = 5; //for dev

            o.User.RequireUniqueEmail = true;
        }).AddEntityFrameworkStores<ApplicationDbContext>()
          .AddDefaultTokenProviders();
        services.Configure<DataProtectionTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromHours(24);
        });
    }
        

    public static void AddJWTAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var secret = configuration[JwtConfiguration.SecretEnvironmentVariableName]
            ?? throw new NullReferenceException("Environment variable 'SECRET' was not found.");

        //services.Configure<JwtConfiguration>(options =>
        //{
        //    configuration.GetSection(JwtConfiguration.Section).Bind(options);
        //    options.Secret = secret;
        //});

        services.AddOptions<JwtConfiguration>()
                .BindConfiguration(JwtConfiguration.Section)
                .Configure(options => { options.Secret = secret; })
                .ValidateDataAnnotations()
                .Validate(config => 
                {
                    return config.ExpiryTimeFrame > TimeSpan.Zero;
                });
       
        var jwtConfiguration = services.BuildServiceProvider()
                                       .GetService<IOptions<JwtConfiguration>>()?.Value
            ?? throw new Exception($"{nameof(JwtConfiguration)} is not configured.");

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfiguration.ValidIssuer,
            ValidAudience = jwtConfiguration.ValidAudience,
            RequireExpirationTime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.Secret)),
            ClockSkew = TimeSpan.FromSeconds(10)
        };
        services.AddSingleton(tokenValidationParameters);
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = tokenValidationParameters;
        });
    }

    public static void AddAuthorizationServices(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
    }

    public static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IContractRepository, ContractRepository>();
        services.AddScoped<IActivityRepository, ActivityRepository>();
        services.AddScoped<IActivityTypeRepository, ActivityTypeRepository>();
        services.AddScoped<IRepositoryManager, RepositoryManager>();
    }

    public static void ConfigureAuthenticationServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IUserManagementService, UserManagementService>();
        services.AddScoped<IRoleService, ApplicationRoleManager>();
    }

    public static void ConfigureApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IActivityTypeService, ActivityTypeService>();
        services.AddScoped<IActivityService, ActivityService>();
        services.AddScoped<IContractService, ContractService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
    }

    public static void ConfigureExternalServices(this IServiceCollection services)
    {
        services.AddOptions<EmailProviderConfiguration>()
                .BindConfiguration(EmailProviderConfiguration.Section)
                .ValidateDataAnnotations();
        services.AddScoped<IEmailService, EmailService>();
    }

    public static void AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(CoursesWebAPI.Application.ApplicationAssemblyReference).Assembly);
        services.AddFluentValidationAutoValidation(configuration =>
        {
            configuration.EnablePathBindingSourceAutomaticValidation = true;
            configuration.EnableBodyBindingSourceAutomaticValidation = true;
            configuration.EnableQueryBindingSourceAutomaticValidation = true;
        });
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v0.1", new OpenApiInfo
            {
                Title = "CoursesWebAPI",
                Version = "v0.1",
                Description = "Web API application",
                TermsOfService = new Uri("https://example.com/terms"),
                Contact = new OpenApiContact
                {
                    Name = "Ivan Buyeuski",
                    Email = "ivan.buyeuskii@gmail.com",
                    Url = new Uri("https://github.com/Ivan-B-V")
                },
                License = new OpenApiLicense
                {
                    Name = "CoursesWebAPI API LICX",
                    Url = new Uri("https://github.com/Ivan-B-V")
                }
            });
            var xmlFile = $"{typeof(CoursesWebAPI.Presentation.PresentationAssemblyReference).Assembly.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Place to add JWT with bearer.",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Name = "Bearer"
                    },
                    Array.Empty<string>()
                }
            });
        });
    }
}
