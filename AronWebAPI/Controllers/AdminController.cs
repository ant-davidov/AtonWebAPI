using AronWebAPI.Data;
using AronWebAPI.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using AronWebAPI.Entites;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AronWebAPI.Controllers
{

    public class AdminController : BaseApiController
    {

        private readonly IMapper _mapper;
        public AdminController(DataContext dataContext, IMapper mapper) : base(dataContext)
        {
            _mapper = mapper;
        }

        [HttpPost("[action]")]
        public async void CreateUser(UserRegistrationDTO user)
        {
            _dataContext.Users.Add(_mapper.Map<User>(user));
            await _dataContext.SaveChangesAsync();
        }
        [HttpGet("[action]")]
        public async Task<ActionResult<List<User>>> AllActiveUsers()
        {
            var list = await _dataContext.Users.Where(x => x.RevokedOn == null).OrderBy(x => x.CreatedOn).ToListAsync();
            return list;
        }
        [HttpGet("{login}")]
        public async void GetUserByLogin(string login)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Login == login);
            if (user == null) return;
            //_mapper.Map<UserResponseDTO>(user);

        }

        [HttpGet("{age}")]
        public async void GetUserByAge(int age)
        {

            var user = await _dataContext.Users.Where(x => x.Birthday.AddYears(age) >= DateTime.Today).ToListAsync();
            if (user == null) return;
        }

        [HttpDelete("{login}")]
        public async void DeleteUser(string login)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Login == login);
            if (user == null) return;
            user.RevokedOn = DateTime.Now;
            user.RevokedBy = "dasa";
        }

    }


}
