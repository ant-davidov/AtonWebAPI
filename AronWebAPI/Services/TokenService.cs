using AronWebAPI.Data;
using AronWebAPI.Entites;
using AtonWebAPI.Entites;
using AtonWebAPI.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AtonWebAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        private readonly UserManager<User> _userManager;
        private readonly DataContext _context;
        private readonly IMemoryCache _cache;
        public TokenService(IConfiguration config, UserManager<User> userManager, DataContext context, IMemoryCache cache)
        {
            _userManager = userManager;
            _context = context;
            _cache = cache;
            string tokenKey = config["Jwt:SecretKey"] ?? String.Empty;
            if (string.IsNullOrEmpty(tokenKey)) throw new NullReferenceException(nameof(tokenKey));
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        }
        public async Task<string> CreateToken(User user)
        {
            if (_cache.TryGetValue(user.Login, out String stringToken))
            {
                return stringToken;
            }
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Guid.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.Login),
            };
            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(60),
                SigningCredentials = creds,
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokesnAsString = tokenHandler.WriteToken(token);
            var tokenToDb = new ActiveToken()
            {
                Token = tokesnAsString,
                UserName = user.Login,
                IsActive = true,
            };
            _context.Tokens.Add(tokenToDb);
            _context.SaveChanges();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(15));
            _cache.Set(user.Login, tokesnAsString, cacheEntryOptions);
            return tokesnAsString;
        }
        public void RevokeToken(string login)
        {
            _context.RemoveRange(_context.Tokens.Where(x=>x.UserName == login).ToList());
            _context.SaveChanges();
            _cache.Remove(login);
        }
    }
}
