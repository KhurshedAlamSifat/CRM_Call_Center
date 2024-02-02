using Base.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Base.Core
{
    public interface IWorkContext
    {
        Task<User> GetCurrentUserAsync();
        Task SetCurrentUserAsync(User user = null);
    }
}
