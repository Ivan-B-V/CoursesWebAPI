using CoursesWebAPI.Shared.Models;
using FluentResults;

namespace CoursesWebAPI.Application.Contracts.ExternalServices
{
    public interface IEmailService
    {
        public Task<Result> SendEmailAsync(MailMessage message, CancellationToken cancellationToken = default);
        public Task<Result> SendVerificationEmailAsync(string address, string token, CancellationToken cancellationToken = default);
        public Task<Result> SendChangeEmailAsync(string address, string currentEmail, string token, CancellationToken cancellationToken = default);
        public Task<Result> SendResetPasswordEmailAsync(string address, string token, CancellationToken cancellationToken = default);
    }
}
