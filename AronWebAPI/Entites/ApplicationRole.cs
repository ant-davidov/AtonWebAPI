using Microsoft.AspNetCore.Identity;

namespace AtonWebAPI.Entites
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
