using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Base.Web.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Base.Web.Areas.Secure.Models.Users
{
    /// <summary>
    /// Represents a discount search model
    /// </summary>
    public partial class UserSearchModel : BaseSearchModel
    {
        #region Ctor

        public UserSearchModel()
        {
            AvailableUserRole = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        public IList<SelectListItem> AvailableUserRole { get; set; }

        public string SearchUserPhone { get; set; }
        public int SearchUserRoleId { get; set; }

        #endregion
    }
}