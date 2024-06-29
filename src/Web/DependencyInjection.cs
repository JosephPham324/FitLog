using System.Net.Mail;
using System.Net;
using Azure.Identity;
using FitLog.Application.Common.Interfaces;
using FitLog.Infrastructure.Data;
using FitLog.Web.Services;
using Microsoft.AspNetCore.Mvc;


namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddScoped<IUserTokenService, CurrentUserFromToken>();
        services.AddScoped<IUser, CurrentUser>();

        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddExceptionHandler<CustomExceptionHandler>();

        services.AddFluentEmail("nhatquangvl2003@gmail.com")
        .AddRazorRenderer()
        .AddSmtpSender(new SmtpClient("smtp.gmail.com")
        {
            UseDefaultCredentials = false,
            Port = 587,
            Credentials = new NetworkCredential("nhatquangvl2003@gmail.com", "ltls ondo iulg rmsm"),
            EnableSsl = true,
        });


        services.AddSingleton<IEmailService, SmtpEmailService>();

        services.AddRazorPages();


        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddEndpointsApiExplorer();

        services.AddOpenApiDocument((configure, sp) =>
        {
            configure.Title = "FitLog API";

        });

        services.Configure<ExceptionHandlerOptions>(options =>
        {
            options.AllowStatusCode404Response = true;
        });


        return services;
    }

    public static IServiceCollection AddKeyVaultIfConfigured(this IServiceCollection services, ConfigurationManager configuration)
    {
        var keyVaultUri = configuration["KeyVaultUri"];
        if (!string.IsNullOrWhiteSpace(keyVaultUri))
        {
            configuration.AddAzureKeyVault(
                new Uri(keyVaultUri),
                new DefaultAzureCredential());
        }

        return services;
    }
}
