using System.ComponentModel.DataAnnotations;

namespace CoursesWebAPI.Infrastructure.ConfigurationModels
{
    public sealed class JwtConfiguration
    {
        public const string Section = "JwtSettings";
        public const string SecretEnvironmentVariableName = "SECRET";
        
        [Required]
        public string ValidIssuer { get; set; } = string.Empty;
        
        [Required]
        public string ValidAudience { get; set; } = string.Empty;
        
        [Required]
        public string Secret { get; set; } = string.Empty;
        
        public TimeSpan ExpiryTimeFrame { get; set; }
        public TimeSpan RefreshTokenLifeTime { get; set; }
    }
}
