using AronWebAPI.Data;
using AronWebAPI.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using AronWebAPI.Entites;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using AtonWebAPI.Interfaces;
using AtonWebAPI.DTOs;
using Microsoft.AspNetCore.Identity;
using System.Security.Principal;
using System.Data;
using System.Security.Claims;

namespace AronWebAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseApiController
    {
        private readonly IMapper _mapper;
        public AdminController(UserManager<User> userManager, IMapper mapper) : base(userManager)
        {
            _mapper = mapper;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<UserResponseForAdmin>> CreateUser(UserRegistrationDTO userDTO)
        {
            if (await _userManager.Users.AnyAsync(x => x.Login == userDTO.Login)) return BadRequest("Login is busy");
            var user = _mapper.Map<User>(userDTO);
            user.CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
            user.CreatedOn = DateTime.UtcNow;
            if (!(await _userManager.CreateAsync(user, userDTO.Password)).Succeeded)  return BadRequest();
            if(userDTO.IsAdmin)  await _userManager.AddToRoleAsync(user, "Admin");
            var response = _mapper.Map<UserResponseForAdmin>(user);
            return response;
        }
        [HttpGet("[action]")]
        public async Task<ActionResult<List<UserResponseForAdmin>>> AllActiveUsers()
        {
            var userList = await _userManager.Users.ToListAsync();
            return _mapper.Map<List<UserResponseForAdmin>>(userList);
        }
        [HttpGet("{login}")]
        public async Task<ActionResult<UserResponseForAdmin>> GetUserByLogin(string login)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Login == login);
            if (user == null) return NotFound("User not found");
            return _mapper.Map<UserResponseForAdmin>(user);
        }

        [HttpGet("users/{age}")]
        public async Task<ActionResult<List<UserResponseForAdmin>>> GetUserByAge(int age)
        {
            var userList = await _userManager.Users.Where(x => x.Birthday.AddYears(age) >= DateTime.Today).ToListAsync();
            if (userList == null || userList.Count < 1) return NotFound("There are no such users");
            return _mapper.Map<List<UserResponseForAdmin>>(userList);
        }

        [HttpDelete("{login}")]
        public async void DeleteUser(string login)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Login == login);
            if (user == null) return;
            user.RevokedOn = DateTime.Now;
            user.RevokedBy = HttpContext.User.Identity?.Name;
        }

    }


}
