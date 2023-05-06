using AronWebAPI.Data;
using AronWebAPI.DTOs;
using AronWebAPI.Hellpers.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AronWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ValidateModel]
    [Authorize]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected readonly DataContext _dataContext;
        public BaseApiController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async void Update(UserUpdateDTO updateUser)
        {
            var user = _dataContext.Users.FirstOrDefault(x => x.Login == updateUser.Login);
            if (user == null) return;
            user.Name = updateUser.Name;
            user.Gender = user.Gender;
            user.Birthday = updateUser.Birthday;
            _dataContext.Entry(user).State = EntityState.Modified;
            await _dataContext.SaveChangesAsync();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async void ChangeLogin(string login, string newLogin)
        {
            var user = _dataContext.Users.FirstOrDefault(x => x.Login == login);
            if (user == null) return;
            if (await _dataContext.Users.AnyAsync(x => x.Login == newLogin)) return;
            user.Login = newLogin;
            _dataContext.Entry(user).State = EntityState.Modified;
            await _dataContext.SaveChangesAsync();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async void ChangePassword(string login, string newPassword)
        {
            var user = _dataContext.Users.FirstOrDefault(x => x.Login == login);
            if (user == null) return;
            user.Password = newPassword;
            _dataContext.Entry(user).State = EntityState.Modified;
            await _dataContext.SaveChangesAsync();
        }
    }
}
