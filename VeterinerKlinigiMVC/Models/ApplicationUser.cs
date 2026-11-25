
using Microsoft.AspNetCore.Identity;

namespace VeterinerKlinigiMVC.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
