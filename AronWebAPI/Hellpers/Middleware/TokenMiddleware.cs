using AronWebAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace AtonWebAPI.Hellpers.Middleware
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;
        private DataContext _context;

        public TokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, DataContext dbContext)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                _context = dbContext;
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (!IsTokenRevoked(token))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Токен отозван");
                    return;
                }
            }
            await _next(context);
        }

        private bool IsTokenRevoked(string token)
        {
            var revokedToken = _context.Tokens.FirstOrDefault(t => t.Token == token);

            if (revokedToken != null && revokedToken.IsActive)
            {
                return true;
            }
            return false;
        }
    }

}
