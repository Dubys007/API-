using Microsoft.AspNetCore.Identity;

namespace Sq016FirstApi.Data.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
    }
}
