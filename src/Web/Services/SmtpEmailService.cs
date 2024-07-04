using System.Net.Mail;
using System.Net;
using FitLog.Application.Common.Interfaces;

namespace FitLog.Web.Services;

public class SmtpEmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public SmtpEmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendAsync(string to, string subject, string body)
    {
        var emailMessage = new MailMessage()
        {
            From = new MailAddress(_configuration["EmailSettings:FromEmail"]??""),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        emailMessage.To.Add(to);

        using (var client = new SmtpClient(_configuration["EmailSettings:SmtpHost"], int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "")))
        {
            client.Credentials = new NetworkCredential(_configuration["EmailSettings:SmtpUser"], _configuration["EmailSettings:SmtpPass"]);
            client.EnableSsl = true;

            await client.SendMailAsync(emailMessage);
        }
    }
}

