using AronWebAPI.Data;
using AronWebAPI.DTOs;
using AronWebAPI.Entites;
using AronWebAPI.Hellpers.Filters;
using AtonWebAPI.DTOs;
using AtonWebAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace AronWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ValidateModel]
    [Authorize]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected readonly UserManager<User> _userManager;

        public BaseApiController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> Update(UserUpdateDTO updateUser)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Login == updateUser.Login);
            if (user == null) return NotFound("User not found");
            user.Name = updateUser.Name;
            user.UserName = updateUser.Name;
            user.Gender = user.Gender;
            user.Birthday = updateUser.Birthday;
            if ((await _userManager.UpdateAsync(user)).Succeeded)
                return Ok();
            return BadRequest();
            
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> ChangeLogin(UpdateLoginDTO userDTO)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Login == userDTO.OldLogin);
            if (user == null)  return NotFound("User not found");
            if (await _userManager.Users.AnyAsync(x => x.Login == userDTO.NewLogin)) return BadRequest("Login is busy"); ;
            user.Login = userDTO.NewLogin;
            if ((await _userManager.UpdateAsync(user)).Succeeded)
                return Ok();
            return BadRequest();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> ChangePassword(UpdatePasswordDTO userDTO)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Login == userDTO.Login);
            if (user == null) return NotFound("User not found");
            await _userManager.ChangePasswordAsync(user, userDTO.OldPassword, userDTO.NewPassword);     
            return Ok();
        }
    }
}
