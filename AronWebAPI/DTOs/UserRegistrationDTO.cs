using System.ComponentModel.DataAnnotations;

namespace AronWebAPI.DTOs
{
    public class UserRegistrationDTO
    {
        [Required(AllowEmptyStrings = false)]
        public string Login { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
        [Range(0, 2)]
        public ushort Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public bool IsAdmin { get; set; } 
    }
}
