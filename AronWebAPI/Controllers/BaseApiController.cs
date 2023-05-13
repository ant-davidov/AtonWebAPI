using AronWebAPI.DTOs;
using AronWebAPI.Hellpers.Filters;
using AtonWebAPI.DTOs;
using AtonWebAPI.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AronWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ValidateModel]
    [Authorize]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected readonly IUserRepository _userRepository;
        protected readonly IMapper _mapper;
        protected readonly ITokenService _tokenService;

        public BaseApiController(IUserRepository userRepository, IMapper mapper, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> Update(UserUpdateDTO updateUser)
        {
            var user = await _userRepository.GetByLogin(updateUser.Login);
            if (user == null) return NotFound("User not found");
            if (await _userRepository.Update(user, updateUser, User.Claims.FirstOrDefault(c => c.Type == "name")?.Value)) return Ok();
            return BadRequest("Update error");

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<UserResponseForUser>> ChangeLogin(UpdateLoginDTO userDTO)
        {
            var user = await _userRepository.GetByLogin(userDTO.OldLogin);
            if (user == null) return NotFound("User not found");
            if (await _userRepository.LoginIsFree(userDTO.NewLogin)) return BadRequest("Login is busy"); ;

            if (await _userRepository.UpdateLogin(user, userDTO.NewLogin, User.Claims.FirstOrDefault(c => c.Type == "name")?.Value))
            {
                var response = _mapper.Map<UserResponseForUser>(user);
                if (User.Claims.FirstOrDefault(c => c.Type == "name")?.Value == userDTO.OldLogin)
                    response.Token = await _tokenService.CreateToken(user);
                return Ok(response);
            }
            return BadRequest("Update error");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<UserResponseForUser>> ChangePassword(UpdatePasswordDTO userDTO)
        {
            var user = await _userRepository.GetByLogin(userDTO.Login);
            if (user == null) return NotFound("User not found");
            if (await _userRepository.UpdatePassword(user, userDTO.OldPassword, userDTO.NewPassword, User.Claims.FirstOrDefault(c => c.Type == "name")?.Value))
            {
                var response = _mapper.Map<UserResponseForUser>(user);
                if (User.Claims.FirstOrDefault(c => c.Type == "name")?.Value == userDTO.Login)
                    response.Token = await _tokenService.CreateToken(user);
                return response;
            }
            return BadRequest("Error Update Password");
        }
    }
}
