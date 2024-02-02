using Base.Core.Domain.Users;
//using Base.Data.BaseModels;
//using Base.Services.Customers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services.Users
{
    public interface IUserRegistrationService
    {
        Task<IActionResult> SignInUserAsync(User user, string returnUrl, bool isPersist = false);
        Task<UserRegistrationResult> RegisterUserAsync(UserRegistrationRequest request);
    }
}
