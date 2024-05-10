using System.ComponentModel.DataAnnotations;

namespace CoursesWebAPI.Infrastructure.ConfigurationModels;

public sealed class EmailProviderConfiguration
{
    public EmailProviderConfiguration() { }

    public EmailProviderConfiguration(string section)
    {
        if (!string.IsNullOrWhiteSpace(section))
        {
            Section = section;
        }
    }

    public static string Section { get; private set; } = nameof(EmailProviderConfiguration);

    [Required]
    public string From { get; set; } = string.Empty;

    [Required]
    public int Port { get; set; }

    [Required]
    public string SmtpServer { get; set; } = string.Empty;

    [Required]
    public string UserName { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
