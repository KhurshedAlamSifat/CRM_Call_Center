using Base.Core.Domain.Users;
using Base.Services.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services.Authentication
{
    public class CookieAuthenticationService : IAuthenticationService
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private User _cachedUser;

        public CookieAuthenticationService(IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }


        public virtual async Task SignInAsync(User user, bool isPersistent)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            var claims = new List<Claim>();

            if (!string.IsNullOrEmpty(user.Phone))
            {
                claims.Add(new Claim(ClaimTypes.Sid, user.Id.ToString(), ClaimValueTypes.String, CrmAuthenticationDefaults.ClaimsIssuer));
                claims.Add(new Claim(ClaimTypes.MobilePhone, user.Phone.ToString(), ClaimValueTypes.String, CrmAuthenticationDefaults.ClaimsIssuer));

            }


            //crea el esquema de autenticacion principal
            var userIdentity = new ClaimsIdentity(claims, AuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            //coloca el valor indicando si la sesion es persistente y el tiempo en que la autenticacion tendr'a validez
            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = isPersistent,
                IssuedUtc = DateTime.UtcNow
            };

            //sign in
            await _httpContextAccessor.HttpContext.SignInAsync(AuthenticationDefaults.AuthenticationScheme, userPrincipal, authenticationProperties);
            _cachedUser = user;

        }


        public virtual async Task<User> GetAuthenticatedUserAsync()
        {
            //whether there is a cached customer
            if (_cachedUser != null)
                return _cachedUser;

            //try to get authenticated user identity
            var authenticateResult = await _httpContextAccessor.HttpContext.AuthenticateAsync(CrmAuthenticationDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
                return null;

            User user = null;
            var phoneClaim = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.MobilePhone
                && claim.Issuer.Equals(CrmAuthenticationDefaults.ClaimsIssuer, StringComparison.InvariantCultureIgnoreCase));
            if (phoneClaim != null)
                user = await _userService.GetUserByPhoneAsync(phoneClaim.Value);

            //whether the found customer is available
            if (user == null || !await _userService.IsAdminAsync(user))
            {
                if (!await _userService.IsCallCenterAgentAsync(user))
                    return null;
            }

            //cache authenticated customer
            _cachedUser = user;

            return _cachedUser;
        }


        public virtual async Task SignOut()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(AuthenticationDefaults.AuthenticationScheme);
        }


    }
}
