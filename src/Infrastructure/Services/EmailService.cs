using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Text;
using Serilog;
using CoursesWebAPI.Infrastructure.ConfigurationModels;
using MimeKit;
using CoursesWebAPI.Shared.Models;
using MailKit.Net.Smtp;
using CoursesWebAPI.Application.Contracts.ExternalServices;
using FluentResults;

namespace CoursesWebAPI.Infrastructure.Services;

public sealed class EmailService : IEmailService
{
    private readonly ILogger logger;
    private readonly IOptionsSnapshot<EmailProviderConfiguration> options;
    private readonly EmailProviderConfiguration configuration;

    public EmailService(ILogger logger, IOptionsSnapshot<EmailProviderConfiguration> options)
    {
        this.logger = logger;
        this.options = options;
        configuration = options.Value;
    }

    public async Task<Result> SendChangeEmailAsync(string address, string currentEmail, string token, CancellationToken cancellationToken)
    {
        var emailBodyTemplate = "To confirm your new email <a href=\"#URL#\">Click here</a>";
        var callBackUrl = $"https://localhost:7195/api/confirmnewemail?currentemail={currentEmail}&newemail={address}&token={Convert.ToBase64String(Encoding.UTF8.GetBytes(token))}";
        var body = emailBodyTemplate.Replace("#URL#", HtmlEncoder.Default.Encode(callBackUrl));
        MailMessage mail = new(new List<string> { address }, "Change email", body);
        return await SendEmailAsync(mail, cancellationToken);
    }

    public async Task<Result> SendEmailAsync(MailMessage message, CancellationToken cancellationToken = default)
    {
        var result = await Send(CreateEmail(message), cancellationToken);
        return result.ToResult();
    }

    public async Task<Result> SendResetPasswordEmailAsync(string address, string token, CancellationToken cancellationToken = default)
    {
        var emailBodyTemplate = "To change your password <a href=\"#URL#\">Click here</a>";
        var callBackUrl = $"https://localhost:7195/api/users/resetpassword?email={address}&token={Convert.ToBase64String(Encoding.UTF8.GetBytes(token))}";
        var body = emailBodyTemplate.Replace("#URL#", HtmlEncoder.Default.Encode(callBackUrl));
        MailMessage mail = new(new List<string> { address }, "Reset password email", body);
        return await SendEmailAsync(mail, cancellationToken);
    }

    public async Task<Result> SendVerificationEmailAsync(string address, string token, CancellationToken cancellationToken = default)
    {
        var callBackUrl = $"https://localhost:7195/api/confirmemail?email={address}&token={Convert.ToBase64String(Encoding.UTF8.GetBytes(token))}";
        var body = $"To confirm your email <a href=\"{callBackUrl}\">Click here</a>";
        MailMessage mail = new(new List<string> { address }, "Email confirmation", body);
        return await SendEmailAsync(mail, cancellationToken);
    }

    private MimeMessage CreateEmail(MailMessage message)
    {
        ArgumentNullException.ThrowIfNull(message);

        MimeMessage email = new() 
        {
            Subject = message.Subject,
            Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message.Content
            }
        };
        email.From.Add(new MailboxAddress("Sender", configuration.From));
        email.To.AddRange(message.Addresses);
        return email;
    }

    private async Task<Result<string>> Send(MimeMessage message, CancellationToken cancellationToken)
    {
        using SmtpClient client = new();
        try
        {
            client.CheckCertificateRevocation = false; //only for dev
            await client.ConnectAsync(configuration.SmtpServer, configuration.Port, useSsl: true, cancellationToken);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.AuthenticateAsync(configuration.UserName, configuration.Password, cancellationToken);
            return await client.SendAsync(message);
        }
        catch (Exception exception)
        {
            logger.Warning("Exception occured. {exception}", exception.Message);
        }
        return Result.Fail("Something went wrong.");
    }
}
