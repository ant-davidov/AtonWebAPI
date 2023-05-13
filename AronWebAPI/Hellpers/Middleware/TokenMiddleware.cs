using AronWebAPI.Data;
using Microsoft.Extensions.Caching.Memory;

namespace AtonWebAPI.Hellpers.Middleware
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;
        private DataContext _dbContext;
        private IMemoryCache _cache;

        public TokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, DataContext dbContext, IMemoryCache cache)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                _dbContext = dbContext;
                _cache = cache;
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (InCache(token))
                {
                    await _next(context);
                    return;
                }

                if (!IsTokenRevoked(token))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Token revoked");
                    return;
                }
            }
            await _next(context);
        }

        private bool IsTokenRevoked(string token)
        {
            var revokedToken = _dbContext.Tokens.FirstOrDefault(t => t.Token == token);

            if (revokedToken != null && revokedToken.IsActive)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(20));
                _cache.Set(revokedToken.Token, true, cacheEntryOptions);
                return true;

            }
            return false;
        }

        private bool InCache(string token)
        {

            if (_cache.TryGetValue(token, out bool active))
                return active;
            return false;

        }
    }

}
