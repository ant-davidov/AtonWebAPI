using System.ComponentModel.DataAnnotations;

namespace AtonWebAPI.DTOs
{
    public class UpdatePasswordDTO
    {
        [Required(AllowEmptyStrings = false)]
        public string Login { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string OldPassword { get; set; }
        [Required(AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z0-9]*$")]
        public string NewPassword { get; set; }
    }
}
