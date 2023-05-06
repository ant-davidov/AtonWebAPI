using System.ComponentModel.DataAnnotations;

namespace AronWebAPI.DTOs
{
    public class UserUpdateDTO
    {
        [Required(AllowEmptyStrings = false)]
        public string Login {get;set;}
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
        public ushort Gender { get; set; }
        public DateTime Birthday { get; set; }
    }
}
