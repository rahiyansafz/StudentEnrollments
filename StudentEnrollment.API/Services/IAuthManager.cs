using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace StudentEnrollment.API.Services;

public interface IAuthManager
{
    Task<AuthResponse> Login(Login login);
    Task<IEnumerable<IdentityError>> Register(Register register);
    Task<string> CreateRefreshToken();
    Task<AuthResponse> VerifyRefreshToken(AuthResponse request);
}

public class AuthManager : IAuthManager
{
    private readonly UserManager<SchoolUser> _userManager;
    private readonly IConfiguration _configuration;
    private SchoolUser? _user;

    private const string _loginProvider = "StudentEnrollmentsAPI";
    private const string _refreshToken = "RefreshToken";
    public AuthManager(UserManager<SchoolUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<string> CreateRefreshToken()
    {
        await _userManager.RemoveAuthenticationTokenAsync(_user, _loginProvider, _refreshToken);
        var newRefreshToken = await _userManager.GenerateUserTokenAsync(_user, _loginProvider, _refreshToken);
        var result = await _userManager.SetAuthenticationTokenAsync(_user, _loginProvider, _refreshToken, newRefreshToken);
        return newRefreshToken;
    }

    public async Task<AuthResponse> Login(Login login)
    {
        _user = await _userManager.FindByEmailAsync(login.EmailAddress);

        if (_user is null)
            return default!;

        bool isValidCredentials = await _userManager.CheckPasswordAsync(_user, login.Password);

        if (!isValidCredentials)
            return default!;

        // Generate Token Here......
        var token = await GenerateTokenAsync();

        return new AuthResponse
        {
            Token = token,
            UserId = _user.Id,
            RefreshToken = await CreateRefreshToken()
        };
    }

    public async Task<IEnumerable<IdentityError>> Register(Register register)
    {
        _user = new SchoolUser
        {
            DateOfBirth = register.DateOfBirth,
            Email = register.EmailAddress,
            UserName = register.EmailAddress,
            FirstName = register.FirstName,
            LastName = register.LastName
        };

        var result = await _userManager.CreateAsync(_user, register.Password);

        if (result.Succeeded)
            await _userManager.AddToRoleAsync(_user, "User");

        return result.Errors;
    }

    public async Task<AuthResponse> VerifyRefreshToken(AuthResponse request)
    {
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);

        var username = tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == JwtRegisteredClaimNames.Email)?.Value;

        _user = await _userManager.FindByNameAsync(username);

        if (_user is null || _user.Id != request.UserId)
            return default!;

        var isValidRefreshToken = await _userManager.VerifyUserTokenAsync(_user, _loginProvider, _refreshToken, request.RefreshToken);

        if (isValidRefreshToken)
        {
            var token = await GenerateTokenAsync();
            return new AuthResponse
            {
                Token = token,
                UserId = _user.Id,
                RefreshToken = await CreateRefreshToken()
            };
        }

        await _userManager.UpdateSecurityStampAsync(_user);
        return default!;
    }

    private async Task<string> GenerateTokenAsync()
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var roles = await _userManager.GetRolesAsync(_user);
        var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
        var userClaims = await _userManager.GetClaimsAsync(_user);

        var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, _user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, _user.Email),
                new Claim("userId", _user.Id),
            }.Union(userClaims).Union(roleClaims);

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(Convert.ToInt32(_configuration["JwtSettings:DurationInHours"])),
            signingCredentials: credentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}