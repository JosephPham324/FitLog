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
    public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddScoped<IUserTokenService, CurrentUserFromToken>();
        services.AddScoped<IUser, CurrentUser>();

        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddExceptionHandler<CustomExceptionHandler>();

        var emailSettings = configuration.GetSection("EmailSettings");

        services.AddFluentEmail(emailSettings["FromEmail"])
           .AddRazorRenderer()
           .AddSmtpSender(new SmtpClient(emailSettings["SmtpHost"])
           {
               UseDefaultCredentials = false,
               Port = Int32.Parse(emailSettings["SmtpPort"]??"0"),
               Credentials = new NetworkCredential(emailSettings["SmtpUser"], emailSettings["SmtpPass"]),
               EnableSsl = true,
           });
        services.AddSingleton<INotificationService, SignalRNotificationService>();


        services.AddSingleton<IEmailService, SmtpEmailService>();

        services.AddSignalR();

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
