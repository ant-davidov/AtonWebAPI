using AronWebAPI.Data;
using AronWebAPI.DTOs;
using AronWebAPI.Entites;
using AtonWebAPI.Data;
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
        public UserController(IUserRepository userRepository, ITokenService tokenService, IMapper mapper) : base(userRepository, mapper, tokenService) { }
       

        [HttpPut("[action]")]
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
        public new async Task<ActionResult<UserResponseForUser>> ChangeLogin(UpdateLoginDTO userDTO)
        {
            var resultVerification = await UserVerification(userDTO.OldLogin);
            if (!resultVerification.isSuccessful) return resultVerification.actionResult;
            return await base.ChangeLogin(userDTO);
        }
        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<UserResponseForUser>> Authentication(UserRequest userDTO)
        {
            var user = await _userRepository.GetByLoginAndPassword(userDTO.Login, userDTO.Password);
            if (null == user) return NotFound("User not found");
            if (user.RevokedBy != null) return BadRequest("User is blocked");
            var token = _tokenService.CreateToken(user);
            var response = _mapper.Map<UserResponseForUser>(user);
            response.Token = await token;
            return response;
        }
        private async Task<(bool isSuccessful, ActionResult actionResult)> UserVerification(string login)
        {
            if(User.Claims.FirstOrDefault(c => c.Type == "name")?.Value == login && !String.IsNullOrEmpty(login))
                return (false,BadRequest ("Invalid user data"));
            var user = await _userRepository.GetByLogin(login);
            if (user == null) return(false, NotFound("User not found"));
            if (user.RevokedBy != null) return (false, BadRequest("User is blocked"));
            return (true, Ok());
        }

    }
}
