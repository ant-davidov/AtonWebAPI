using System.ComponentModel.DataAnnotations;

namespace AtonWebAPI.DTOs
{
    public class UpdatePasswordDTO
    {
        [Required(AllowEmptyStrings = false)]
        public string Login { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
