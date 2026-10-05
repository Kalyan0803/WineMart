using Microsoft.AspNetCore.Identity;
using WineMart.Data;

namespace WineMart.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthRepository(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ApplicationUser?> GetUserByEmailAsync(
            string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<(bool Succeeded, string[] Errors)> CreateUserAsync(
            ApplicationUser user,
            string password)
        {
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                var errors = result.Errors
                    .Select(e => e.Description)
                    .ToArray();

                return (false, errors);
            }

            return (true, Array.Empty<string>());
        }

        public async Task<bool> CheckPasswordAsync(
            ApplicationUser user,
            string password)
        {
            var result = await _signInManager.CheckPasswordSignInAsync(
                user,
                password,
                lockoutOnFailure: false);

            return result.Succeeded;
        }
    }
}