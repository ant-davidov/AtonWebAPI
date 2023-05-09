using System.ComponentModel.DataAnnotations;

namespace AtonWebAPI.Entites
{
    public class ActiveToken
    {
        [Key]
        public string Token { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        
    }
}
