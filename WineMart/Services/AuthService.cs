using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WineMart.Data;
using WineMart.Models;
using WineMart.Repositories;

namespace WineMart.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;

        public AuthService(
            IAuthRepository authRepository,
            IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        public async Task<(bool Success, string[] Errors)> RegisterAsync(
            RegisterDto model)
        {
            // Check whether email already exists
            var existingUser =
                await _authRepository.GetUserByEmailAsync(model.Email);

            if (existingUser != null)
            {
                return (
                    false,
                    new[] { "Email is already registered." }
                );
            }

            // Create ApplicationUser
            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
                Email = model.Email,
                CreatedAt = DateTime.UtcNow
            };

            // Create user through repository
            var result = await _authRepository.CreateUserAsync(
                user,
                model.Password);

            return result;
        }

        public async Task<(bool Success, string? Token, string Message)> LoginAsync(
            LoginDto model)
        {
            // Find user
            var user =
                await _authRepository.GetUserByEmailAsync(model.Email);

            if (user == null)
            {
                return (
                    false,
                    null,
                    "Invalid email or password."
                );
            }

            // Validate password
            var passwordValid =
                await _authRepository.CheckPasswordAsync(
                    user,
                    model.Password);

            if (!passwordValid)
            {
                return (
                    false,
                    null,
                    "Invalid email or password."
                );
            }

            // Generate JWT
            var token = GenerateJwtToken(user);

            return (
                true,
                token,
                "Login successful."
            );
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var jwtSettings =
                _configuration.GetSection("JwtSettings");

            var secretKey = jwtSettings["Secret"]
                ?? throw new InvalidOperationException(
                    "JWT Secret not configured.");

            var key = Encoding.UTF8.GetBytes(secretKey);

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.Id),

                new Claim(
                    ClaimTypes.Email,
                    user.Email ?? ""),

                new Claim(
                    ClaimTypes.Name,
                    $"{user.FirstName} {user.LastName}")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),

                Expires = DateTime.UtcNow.AddHours(2),

                Issuer = jwtSettings["Issuer"],

                Audience = jwtSettings["Audience"],

                SigningCredentials =
                    new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler =
                new JwtSecurityTokenHandler();

            var token =
                tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
