using System.ComponentModel.DataAnnotations;

namespace AtonWebAPI.DTOs
{
    public class UpdateLoginDTO
    {
        [Required(AllowEmptyStrings = false)]
        public string OldLogin { get; set; }
        [Required(AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z0-9]*$")]
        public string NewLogin { get; set; }
    }
}
