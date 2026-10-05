using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WineMart.Data
{
    public class WineMartDbContext : IdentityDbContext<ApplicationUser>
    {
        public WineMartDbContext(
            DbContextOptions<WineMartDbContext> options)
            : base(options)
        {
        }
    }
}