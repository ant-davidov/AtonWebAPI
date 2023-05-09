using AronWebAPI.DTOs;
using AronWebAPI.Entites;
using AtonWebAPI.Interfaces;
using AtonWebAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AtonWebAPI.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;

        public UserRepository(UserManager<User> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<bool> Add(User user, string password)
        {
            return (await _userManager.CreateAsync(user, password)).Succeeded;
        }

        public async Task<bool> AddRole(User user, string role)
        {
            return (await _userManager.AddToRoleAsync(user, role)).Succeeded;
        }

        public async Task<bool> Delete(User user, string adminLogin)
        {
            user.RevokedOn = DateTime.Now;
            user.RevokedBy = adminLogin;
            _tokenService.RevokeToken(user.Login);
            return (await _userManager.UpdateAsync(user)).Succeeded;
        }

        public async Task<List<User>> GetAllActiveUsers()
        {
            return await _userManager.Users.Where(x => x.RevokedOn == null).ToListAsync();
        }

        public async Task<User> GetByLogin(string login)
        {
            return await _userManager.Users.FirstOrDefaultAsync(x => x.Login == login);
        }

        public async Task<User> GetByLoginAndPassword(string login, string password)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Login == login);
            if (user == null) return null;
            if (!await _userManager.CheckPasswordAsync(user, password)) return null;
            return user;
        }

        public async Task<bool> Update(User user, UserUpdateDTO updateUser)
        {
            user.Name = updateUser.Name;
            user.UserName = updateUser.Name;
            user.Gender = updateUser.Gender;
            user.Birthday = updateUser.Birthday;
            return (await _userManager.UpdateAsync(user)).Succeeded;
        }

        public async Task<bool> UpdateLogin(User user, string newLogin)
        {
            user.Login = newLogin;
            user.UserName = newLogin;
            _tokenService.RevokeToken(user.Login);
            return (await _userManager.UpdateAsync(user)).Succeeded;
        }
        public async Task<bool> Recovery(User user)
        {
            user.RevokedOn = null;
            user.RevokedBy = null;
            return (await _userManager.UpdateAsync(user)).Succeeded;
                
        }

        public async Task<bool> UpdatePassword(User user, string password, string newPassword)
        {
            _tokenService.RevokeToken(user.Login);
            return(await _userManager.ChangePasswordAsync(user, password, newPassword)).Succeeded;

        } 
        public async Task<bool> LoginIsFree(string login) => await _userManager.Users.AnyAsync(x => x.Login == login);
        public async Task<List<User>> GetUserByAge(int age) => await _userManager.Users.Where(x => x.Birthday.AddYears(age) >= DateTime.Today).ToListAsync();

    }
}

