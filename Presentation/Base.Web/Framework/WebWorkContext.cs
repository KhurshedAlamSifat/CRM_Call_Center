using Base.Core;
using Base.Core.Domain.Users;
using Base.Core.Http;
using Base.Services.Authentication;
using Base.Services.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Base.Web.Framework
{
    public class WebWorkContext : IWorkContext
    {
        private User _cachedUser;
        private readonly IAuthenticationService _authenticationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHelper _webHelper;
        private readonly IUserService _userService;
        public WebWorkContext(IAuthenticationService authenticationService,
            IHttpContextAccessor httpContextAccessor, IWebHelper webHelper, 
            IUserService userService)
        {
            _authenticationService = authenticationService;
            _httpContextAccessor = httpContextAccessor;
            _webHelper = webHelper;
            _userService = userService;
        }

        public virtual async Task<User> GetCurrentUserAsync()
        {
            //whether there is a cached value
            if (_cachedUser != null)
                return _cachedUser;

            await SetCurrentUserAsync();

            return _cachedUser;
        }
        public virtual async Task SetCurrentUserAsync(User user = null)
        {
            if (user == null)
            {
                if (user == null)
                {
                    //try to get registered user
                    user = await _authenticationService.GetAuthenticatedUserAsync();
                }

                if (user == null)
                {
                    //get guest customer
                    var userCookie = GetUserCookie();
                    if (Guid.TryParse(userCookie, out var userGuid))
                    {
                        //get customer from cookie (should not be registered)
                        var userByCookie = await _userService.GetUserByGuidAsync(userGuid);
                        if (userByCookie != null && !await _userService.IsCallCenterAgentAsync(userByCookie) && !await _userService.IsAdminAsync(userByCookie))
                            user = userByCookie;
                    }
                }

                if (user == null)
                {
                    user = new User
                    {
                        UserGuid = new Guid("3cf65c56-b461-4758-80ee-9d616696e6af")
                    };
                }
            }

            SetUserCookie(user.UserGuid);
            _cachedUser = user;
        }
        protected virtual string GetUserCookie()
        {
            var cookieName = $"{CrmCookieDefaults.Prefix}{CrmCookieDefaults.UserCookie}";
            return _httpContextAccessor.HttpContext?.Request?.Cookies[cookieName];
        }
        protected virtual void SetUserCookie(Guid customerGuid)
        {
            if (_httpContextAccessor.HttpContext?.Response?.HasStarted ?? true)
                return;

            //delete current cookie value
            var cookieName = $"{CrmCookieDefaults.Prefix}{CrmCookieDefaults.UserCookie}";
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(cookieName);

            //get date of cookie expiration
            var cookieExpires = 8760;
            var cookieExpiresDate = DateTime.Now.AddHours(cookieExpires);

            //if passed guid is empty set cookie as expired
            if (customerGuid == Guid.Empty)
                cookieExpiresDate = DateTime.Now.AddMonths(-1);

            //set new cookie value
            var options = new CookieOptions
            {
                HttpOnly = true,
                Expires = cookieExpiresDate,
                Secure = _webHelper.IsCurrentConnectionSecured()
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName, customerGuid.ToString(), options);
        }


    }
}
