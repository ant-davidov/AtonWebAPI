using AronWebAPI.DTOs;
using AronWebAPI.Entites;
using AtonWebAPI.DTOs;
using AtonWebAPI.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AronWebAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseApiController
    {

        public AdminController(IUserRepository userRepository, IMapper mapper, ITokenService tokenService) : base(userRepository, mapper, tokenService) { }

        [HttpPost("[action]")]
        public async Task<ActionResult<UserResponseForAdmin>> CreateUser(UserRegistrationDTO userDTO)
        {
            if (await _userRepository.LoginIsFree(userDTO.Login)) return BadRequest("Login is busy");
            var user = _mapper.Map<User>(userDTO);
            user.CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "name")?.Value ?? "";
            user.CreatedOn = DateTime.UtcNow;
            if (!await _userRepository.Add(user, userDTO.Password)) return BadRequest("Creation error");
            if (userDTO.IsAdmin) await _userRepository.AddRole(user, "Admin");
            var response = _mapper.Map<UserResponseForAdmin>(user);
            var uri = Url.Action("GetUserByLogin", new { login = userDTO.Login });
            return Created(uri, response);
        }
        [HttpGet("[action]")]
        public async Task<ActionResult<List<UserResponseForAdmin>>> AllActiveUsers()
        {
            var userList = await _userRepository.GetAllActiveUsers();
            return _mapper.Map<List<UserResponseForAdmin>>(userList);
        }
        [HttpGet("{login}")]
        public async Task<ActionResult<UserResponseForAdmin>> GetUserByLogin(string login)
        {
            var user = await _userRepository.GetByLogin(login);
            if (user == null) return NotFound("User not found");
            return _mapper.Map<UserResponseForAdmin>(user);
        }

        [HttpGet("users/{age}")]
        public async Task<ActionResult<List<UserResponseForAdmin>>> GetUserByAge(int age)
        {
            var userList = await _userRepository.GetUserByAge(age);
            if (userList == null || userList.Count < 1) return NotFound("There are no such users");
            return _mapper.Map<List<UserResponseForAdmin>>(userList);
        }

        [HttpDelete("{login}")]
        public async Task<ActionResult> DeleteUser(string login)
        {
            var user = await _userRepository.GetByLogin(login);
            if (user == null) return NotFound("User not found");
            if (user.Admin) return BadRequest("Can not block the admin");
            if (_userRepository.Delete(user, User.Claims.FirstOrDefault(c => c.Type == "name")?.Value)) return Ok();
            return BadRequest();
        }
        [HttpPut("[action]/{login}")]
        public async Task<ActionResult> Recovery(string login)
        {
            var user = await _userRepository.GetByLogin(login);
            if (user == null) return NotFound("User not found");
            if (user.RevokedBy == null && user.RevokedOn == null) return BadRequest("Already unblocked");
            if (await _userRepository.Recovery(user)) return Ok();
            return BadRequest("Error unblocked");
        }

        [HttpPut("[action]")]
        public new async Task<ActionResult> Update(UserUpdateDTO updateUser)
        {
            return await base.Update(updateUser);
        }

        [HttpPut("[action]")]
        public new async Task<ActionResult<UserResponseForUser>> ChangePassword(UpdatePasswordDTO userDTO)
        {
            return await base.ChangePassword(userDTO);
        }

        [HttpPut("[action]")]
        public new async Task<ActionResult<UserResponseForUser>> ChangeLogin(UpdateLoginDTO userDTO)
        {
            return await base.ChangeLogin(userDTO);
        }


    }


}
