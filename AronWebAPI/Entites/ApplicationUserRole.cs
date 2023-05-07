using AronWebAPI.Entites;
using Microsoft.AspNetCore.Identity;
using System.Security.Principal;
using System.Text.Json.Serialization;

namespace AtonWebAPI.Entites
{
    public class ApplicationUserRole : IdentityUserRole<Guid>
    {
        [JsonIgnore]
        public virtual User User { get; set; }
        [JsonIgnore]
        public virtual ApplicationRole Role { get; set; }
    }
}
