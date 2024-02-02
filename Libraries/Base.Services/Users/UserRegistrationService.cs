using Base.Core;
using Base.Core.Domain.Users;
using Base.Data;
using Base.Services.Authentication;
using Base.Services.Security;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Base.Services.Users
{
    public class UserRegistrationService : IUserRegistrationService
    {
        private readonly IEncryptionService _encryptionService;
        private readonly IWorkContext _workContext;
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;

        public UserRegistrationService(IEncryptionService encryptionService,
            IWorkContext workContext, IUserService userService, IAuthenticationService authenticationService)
        {
            this._encryptionService = encryptionService;
            _workContext = workContext;
            _userService = userService;
            _authenticationService = authenticationService;
        }

        public virtual async Task<IActionResult> SignInUserAsync(User user, string returnUrl, bool isPersist = false)
        {
            //sign in new customer
            await _authenticationService.SignInAsync(user, isPersist);


            var customerToValidate = user;
            var isAdmin = await _userService.IsAdminAsync(user);

            if (!isAdmin)
            {
                return new RedirectToRouteResult("CallCenterAgentPage", null);
            }

            //redirect to the return URL if it's specified
            if (!string.IsNullOrEmpty(returnUrl))
                return new RedirectResult(returnUrl);

            return new RedirectResult("Secure");
        }

        public virtual async Task<UserRegistrationResult> RegisterUserAsync(UserRegistrationRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.User == null)
                throw new ArgumentException("Can't load current customer");

            var result = new UserRegistrationResult();

            if (await _userService.IsCallCenterAgentAsync(request.User))
            {
                result.AddError("Current user is already registered");
                return result;
            }
            if (await _userService.IsAdminAsync(request.User))
            {
                result.AddError("Current user is already registered");
                return result;
            }

            if (string.IsNullOrEmpty(request.Email))
            {
                result.AddError("Email Is Not Provided");
                return result;
            }

            if (!CommonHelper.IsValidEmail(request.Email))
            {
                result.AddError("Wrong Email");
                return result;
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                result.AddError("Password Is Not Provided");
                return result;
            }


            //validate unique user
            if (await _userService.GetUserByEmailAsync(request.Email) != null)
            {
                result.AddError("Email Already Exists");
                return result;
            }



            var registeredRole = await _userService.GetUserRoleBySystemNameAsync(CrmUserDefaults.AdministratorsRoleName);
            if (registeredRole == null)
                throw new Exception("'Registered' role could not be loaded");

            //at this point request is valid
            request.User.UserGuid = Guid.NewGuid();
            request.User.Email = request.Email;
            request.User.Name = request.Name;
            request.User.Phone = request.Phone;
            request.User.HomeAddress = request.Address;
            request.User.CreatedDate = DateTime.UtcNow.AddHours(6);

            await _userService.InsertUserAsync(request.User);

            var userPassword = new UserPassword
            {
                UserId = request.User.Id,
                PasswordFormat = PasswordFormat.Encrypted,
                CreatedOnUtc = DateTime.UtcNow
            };
            userPassword.Password = _encryptionService.EncryptText(request.Password);
            userPassword.UserId = request.User.Id;

            await _userService.InsertUserPasswordAsync(userPassword);

            await _userService.AddUserRoleMappingAsync(new UserUserRoleMapping { UserId = request.User.Id, UserRoleId = registeredRole.Id });


            await _workContext.SetCurrentUserAsync(request.User);

            return result;
        }


    }
}
