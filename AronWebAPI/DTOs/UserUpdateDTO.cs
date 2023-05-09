using System.ComponentModel.DataAnnotations;

namespace AronWebAPI.DTOs
{
    public class UserUpdateDTO
    {
        [RegularExpression(@"^[a-zA-Z0-9]*$")]
        [Required(AllowEmptyStrings = false)]
        public string Login {get;set;}
        [Required(AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Zа-яА-Я]*$")]
        public string Name { get; set; }
        public ushort Gender { get; set; }
        public DateTime Birthday { get; set; }
    }
}
