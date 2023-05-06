namespace AronWebAPI.DTOs
{
    public class UserRegistrationDTO
    {
        string Login { get; set; }
        string Password { get; set; }
        string Name { get; set; }
        ushort Gender { get; set; }
        DateTime? Birthday { get; set; }
    }
}
