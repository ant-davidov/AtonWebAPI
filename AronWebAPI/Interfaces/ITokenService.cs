using AronWebAPI.Entites;

namespace AtonWebAPI.Interfaces
{
    public interface ITokenService
    {
        public Task<string> CreateToken(User user);
    }
}
