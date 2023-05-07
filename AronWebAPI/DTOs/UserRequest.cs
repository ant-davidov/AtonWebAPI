using System.ComponentModel.DataAnnotations;

namespace AtonWebAPI.DTOs
{
    public class UserRequest
    {
        [Required(AllowEmptyStrings = false)]
        public string Login { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }
    }
}
