using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Base.Web.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Base.Web.Areas.Secure.Models.Category
{
    /// <summary>
    /// Represents a discount search model
    /// </summary>
    public partial class CategorySearchModel : BaseSearchModel
    {

        #region Properties
        public string CategoryName { get; set; }

        #endregion
    }
}
