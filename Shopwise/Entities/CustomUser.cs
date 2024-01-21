using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;

namespace Shopwise.Entities
{
    public class CustomUser:IdentityUser
    {
        public string Fullname { get; set; }
        public ICollection<WishList> WishLists { get; set; }
    }
}
