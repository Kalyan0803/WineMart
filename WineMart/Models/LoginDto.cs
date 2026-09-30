using System.ComponentModel.DataAnnotations;

namespace WineMart.Models
{
    public class LoginDto
    {
        [Required]
        [StringLength(100, MinimumLength = 5)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
    }
}