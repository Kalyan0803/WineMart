using WineMart.Data;

namespace WineMart.Repositories
{
    public interface IAuthRepository
    {
        Task<ApplicationUser?> GetUserByEmailAsync(string email);

        Task<(bool Succeeded, string[] Errors)> CreateUserAsync(
            ApplicationUser user,
            string password);

        Task<bool> CheckPasswordAsync(
            ApplicationUser user,
            string password);
    }
}