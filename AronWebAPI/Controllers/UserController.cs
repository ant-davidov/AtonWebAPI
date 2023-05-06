using AronWebAPI.Data;
using AronWebAPI.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AronWebAPI.Controllers
{
    public class UserController : BaseApiController
    {
        
        public UserController(DataContext dataContext) : base(dataContext)
        {
            
        }

        [HttpGet("[action]")]
        public new async void Update([FromBody]UserUpdateDTO userUpdate)
        {
            var resultVerification = await UserVerification(userUpdate.Login);
            if (!resultVerification.isSuccessful) return;
            base.Update(userUpdate);

        }
        [HttpPut("[action]")]
        public new async void ChangePassword([FromQuery] string login, [FromQuery] string newPassword)
        {
            var resultVerification = await UserVerification(login);
            if (!resultVerification.isSuccessful) return;
            base.ChangePassword(login, newPassword);
        }
        [HttpPut("[action]")]
        public new async void ChangeLogin([FromQuery] string login, [FromQuery] string newLogin)
        {
            var resultVerification = await UserVerification(login);
            if (!resultVerification.isSuccessful) return;
            base.ChangeLogin(login, newLogin);
        }
        [HttpGet("[action]")]
        public async void GetUser([FromQuery] string login, [FromQuery] string password)
        {
            //проверка на авторизацию
            var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Login == login && x.Password == password);
            if (user == null) return;
            if (user.RevokedBy != null) return;

        }
        private async Task<(bool isSuccessful, ActionResult actionResult)> UserVerification(string login)
        {
            // проверка на авторизацию
            var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Login == login);
            if (user == null) return(false, NotFound("User not found"));
            if (user.RevokedBy != null) return (false, NotFound("User is blocked"));
            return (true, Ok());
        }

    }
}
