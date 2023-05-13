using System.ComponentModel.DataAnnotations;

namespace AronWebAPI.DTOs
{
    public class UserRegistrationDTO
    {
        [Required(AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z0-9]*$")]
        public string Login { get; set; }

        [Required(AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z0-9]*$")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Zа-яА-Я]*$")]
        public string Name { get; set; }
        [Range(0, 2)]
        public ushort Gender { get; set; }
        public DateTime? Birthday { get; set; } = null;
        public bool IsAdmin { get; set; }
    }
}
