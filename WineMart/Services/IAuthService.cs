
using WineMart.Models;

namespace WineMart.Services
{
    public interface IAuthService
    {
        Task<(bool Success, string[] Errors)> RegisterAsync(
            RegisterDto model);

        Task<(bool Success, string? Token, string Message)> LoginAsync(
            LoginDto model);
    }
}