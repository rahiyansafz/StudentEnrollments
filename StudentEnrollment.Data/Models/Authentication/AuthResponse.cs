namespace StudentEnrollment.Data.Models.Authentication;
public class AuthResponse
{
    public string UserId { get; set; } = default!;
    public string Token { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}
