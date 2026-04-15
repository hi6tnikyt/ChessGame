using Microsoft.AspNetCore.Identity;

namespace Chess.Models
{
    public class ApplicationUser :IdentityUser
    {
        public string Nickname { get; set; } = null!;
    }
}
