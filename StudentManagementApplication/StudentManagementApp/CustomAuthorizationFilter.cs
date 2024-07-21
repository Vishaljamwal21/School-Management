using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentManagementApp
{
    public class CustomAuthorizationFilter : IAuthorizationFilter
    {
        private readonly ILogger<CustomAuthorizationFilter> _logger;

        public CustomAuthorizationFilter(ILogger<CustomAuthorizationFilter> logger)
        {
            _logger = logger;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var tokenFromSession = SessionHandler.GetToken(context.HttpContext);

            if (string.IsNullOrEmpty(tokenFromSession))
            {
                context.Result = new RedirectToActionResult("Login", "User", null);
                return;
            }
            var secretKey = "thisismyjwtsecretkey";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var claimsPrincipal = tokenHandler.ValidateToken(tokenFromSession, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                var roleClaim = claimsPrincipal.FindFirst(ClaimTypes.Role);
                if (roleClaim == null)
                {
                    _logger.LogInformation("Role claim is missing in the token.");
                    context.Result = new ForbidResult();
                    return;
                }
                var userRole = roleClaim.Value;
                if (userRole == "Admin" || userRole == "Teacher")
                {
                    return;
                }
                else
                {
                    _logger.LogInformation("Unknown role found in the token.");
                    context.Result = new ForbidResult();
                    return;
                }
            }
            catch (Exception)
            {
                context.Result = new RedirectToActionResult("Login", "User", null);
            }
        }
    }
}
