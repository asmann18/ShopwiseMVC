using Microsoft.AspNetCore.Identity;

namespace Shopwise.Entities
{
    public class CustomUser:IdentityUser
    {
        public string Fullname { get; set; }
    }
}
