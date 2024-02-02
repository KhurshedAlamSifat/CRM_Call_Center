using Base.Web.Areas.Secure.Models.Users;
using Base.Web.Framework.Models;

namespace Base.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents a customer list model
    /// </summary>
    public partial class UserListModel : BasePagedListModel<UserModel>
    {
    }
}