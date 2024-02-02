using Base.Web.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Base.Web.Areas.Secure.Models.Districts
{
    public class DistrictSearchModel : BaseSearchModel
    {
        public string DistrictName { get; set; }
    }
}

