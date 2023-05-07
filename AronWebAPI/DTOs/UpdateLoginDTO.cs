using System.ComponentModel.DataAnnotations;

namespace AtonWebAPI.DTOs
{
    public class UpdateLoginDTO
    {
        [Required(AllowEmptyStrings = false)]
        public string OldLogin { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string NewLogin { get; set; }
    }
}
