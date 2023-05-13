using AronWebAPI.Entites;
using AtonWebAPI.Entites;
using Microsoft.AspNetCore.Identity;

namespace AtonWebAPI.Data
{
    public class SeedUsers
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<User> _userManager;
        public SeedUsers(UserManager<User> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async void Initialize()
        {
            if (_userManager.Users.FirstOrDefault(x => x.Login == "admin") != null) return;
            var role = new ApplicationRole { Name = "Admin" };

            await _roleManager.CreateAsync(role);

            var user = new User
            {
                Login = "admin",
                UserName = "admin",
                Admin = true,
                Birthday = DateTime.UtcNow,
                Name = "admin",
                CreatedBy = "Server",
                CreatedOn = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, "admin");
            await _userManager.AddToRoleAsync(user, "Admin");

        }
    }
}
