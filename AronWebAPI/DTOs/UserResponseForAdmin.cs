namespace AtonWebAPI.DTOs
{
    public class UserResponseForAdmin
    {
        public string Login { get; set; }
        public string Name { get; set; }
        public ushort Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public bool IsActive { get; set; }

    }
}
