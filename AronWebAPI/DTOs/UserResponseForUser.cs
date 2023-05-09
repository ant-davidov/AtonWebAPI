namespace AronWebAPI.DTOs
{
    public class UserResponseForUser
    {
        public string Login { get; set; }
        public string Name { get; set; }
        public ushort Gender { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string Token { get; set; }
    }
}
