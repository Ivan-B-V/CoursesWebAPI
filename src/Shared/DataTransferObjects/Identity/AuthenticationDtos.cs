namespace CoursesWebAPI.Shared.DataTransferObjects.Identity;

public record AuthResult
{
    public AccessRefreshTokenDto? Tokens { get; init; }
    public bool Result { get; init; }
    public IEnumerable<string> Errors { get; init; } = Enumerable.Empty<string>();
}

public record AccessRefreshTokenDto(string AccessToken, Guid RefreshToken);

public record TokensForRefreshDto(string AccessToken, Guid RefreshToken, string Fingerprint) : AccessRefreshTokenDto(AccessToken, RefreshToken);

public record AuthenticationCredentialsDto(string Email, string Password);

public record ChangePasswordDto(string CurrentPassword, string NewPassword);
