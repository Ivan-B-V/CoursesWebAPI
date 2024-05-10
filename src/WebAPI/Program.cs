using Microsoft.AspNetCore.HttpOverrides;
using Serilog;
using WebAPI.Configuration;
using WebAPI.Extensions;

namespace WebAPI;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        #region Services configuration

        builder.Host.ConfigureLoggerService(builder.Configuration);
        builder.Services.ConfigureCors();
        builder.Services.AddHttpContextAccessor();
        builder.Services.ConfigureIISIntegration();
        builder.Services.ConfigureSqlContext(builder.Configuration);
        builder.Services.ConfigureIdentity();
        builder.Services.AddJWTAuthentication(builder.Configuration);
        builder.Services.AddAuthorizationServices();
        builder.Services.ConfigureRepositories();
        builder.Services.ConfigureAuthenticationServices();
        builder.Services.ConfigureApplicationServices();
        builder.Services.ConfigureExternalServices();
        builder.Services.AddValidators();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddControllers(config =>
        {
            config.RespectBrowserAcceptHeader = true;
            config.ReturnHttpNotAcceptable = true;
            //config.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
            //config.CacheProfiles.Add("120secondsDuration", new CacheProfile
            //{
            //    Duration = 120
            //});
        }).AddApplicationPart(typeof(CoursesWebAPI.Presentation.PresentationAssemblyReference).Assembly)
        .AddNewtonsoftJson();
        builder.Services.ConfigureSwagger();

        var app = builder.Build();

        #endregion

        #region Application configuration

        app.UseSerilogRequestLogging();
        app.ConfigureExceptionHandler(app.Services.GetRequiredService<Serilog.ILogger>());
        //app.UseExceptionHandler();

        if (app.Environment.IsProduction())
            app.UseHsts();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v0.1/swagger.json", "CoursesWebAPI v0.1");
            });
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.All
        });
        app.UseCors("CorsPolicy");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();

        #endregion
    }
}