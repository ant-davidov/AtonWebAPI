using AronWebAPI.Data;
using AronWebAPI.DTOs;
using AronWebAPI.Entites;
using AtonWebAPI.DTOs;
using AtonWebAPI.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AronWebAPI.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public UserController(UserManager<User> userManager, ITokenService tokenService, IMapper mapper) : base(userManager)
        {
            _tokenService = tokenService;
            _mapper = mapper;  
        }

        [HttpGet("[action]")]
        public new async Task<ActionResult> Update([FromBody]UserUpdateDTO userUpdate)
        {
            var resultVerification = await UserVerification(userUpdate.Login);
            if (!resultVerification.isSuccessful) return resultVerification.actionResult;
            return await base.Update(userUpdate);

        }
        [HttpPut("[action]")]
        public new async Task<ActionResult> ChangePassword(UpdatePasswordDTO userDTO)
        {
            var resultVerification = await UserVerification(userDTO.Login);
            if (!resultVerification.isSuccessful) return resultVerification.actionResult;
            return await base.ChangePassword(userDTO);
        }
        [HttpPut("[action]")]
        public new async Task<ActionResult> ChangeLogin(UpdateLoginDTO userDTO)
        {
            var resultVerification = await UserVerification(userDTO.OldLogin);
            if (!resultVerification.isSuccessful) return resultVerification.actionResult;
            return await base.ChangeLogin(userDTO);
        }
        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<UserResponseForUser>> Authentication(UserRequest userDTO)
        {    
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Login == userDTO.Login);
            if (user == null) 
                return NotFound("User not found");
            if(!await _userManager.CheckPasswordAsync(user, userDTO.Password))
                return NotFound("User not found");
            if (user.RevokedBy != null) 
                return BadRequest("User is blocked");

            var token = _tokenService.CreateToken(user);
            var response = _mapper.Map<UserResponseForUser>(user);
            response.Token = await token;
            return response;
        }
        private async Task<(bool isSuccessful, ActionResult actionResult)> UserVerification(string login)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Login == login);
            if (user == null) return(false, NotFound("User not found"));
            if (user.RevokedBy != null) return (false, BadRequest("User is blocked"));
            return (true, Ok());
        }

    }
}
