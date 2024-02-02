using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Base.Web.Models;
using Base.Data;
using Base.Services.Users;
using Base.Services.Authentication;
using Base.Core.Domain.Users;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using Base.Core;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Text.RegularExpressions;
using Base.Web.Models.User;
using Base.Services.Common;
using DocumentFormat.OpenXml.EMMA;

namespace Base.Web.Controllers
{
    public partial class UserController : Controller
    {
        private readonly IWorkContext _workContext;
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserRegistrationService _userRegistrationService;

        public UserController(IWorkContext workContext, IUserService userService, 
            IAuthenticationService authenticationService, IUserRegistrationService userRegistrationService)
        {
            _workContext = workContext;
            _userService = userService;
            _authenticationService = authenticationService;
            _userRegistrationService = userRegistrationService;
        }
        public virtual IActionResult Login()
        {
            var model = new LoginModel();
            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Login(LoginModel model, string returnUrl, bool captchaValid)
        {
            if (!model.Phone.IsValidPhoneNumber())
            {
                ModelState.AddModelError("", "Invalid PhoneNumber");
            }

            if (ModelState.IsValid)
            {
                var loginResult = await _userService.ValidateUserAsync(model.Phone, model.Password);
                switch (loginResult)
                {
                    case UserLoginResults.Successful:
                        {
                            var user = await _userService.GetUserByPhoneAsync(model.Phone);

                            return await _userRegistrationService.SignInUserAsync(user, returnUrl, model.RememberMe);
                        }
                }

            }
            return View(model);
        }

        public virtual IActionResult Logout()
        {
            //standard logout 
            _authenticationService.SignOut();
            return RedirectToRoute("Login");
        }
        public virtual async Task<IActionResult> AgentAccount()
        {
            var user=await _workContext.GetCurrentUserAsync();
            if(!await _userService.IsCallCenterAgentAsync(user) && !await _userService.IsAdminAsync(user))
            {
                return RedirectToRoute("Login");
            }
            return View();
        }

        

    }
}
