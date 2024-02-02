using Base.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services.Authentication
{    
    public partial interface IAuthenticationService
    {
        Task<User> GetAuthenticatedUserAsync();
        Task SignInAsync(User user, bool isPersistent);
        Task SignOut();
        
    }
}
