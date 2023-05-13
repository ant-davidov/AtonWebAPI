using AtonWebAPI.Entites;
using Microsoft.AspNetCore.Identity;

namespace AronWebAPI.Entites
{
    public class User : IdentityUser<Guid>
    {
        public string Login { get; set; }
        public string Name { get; set; }
        public ushort Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public bool Admin { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? RevokedOn { get; set; }
        public string RevokedBy { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
