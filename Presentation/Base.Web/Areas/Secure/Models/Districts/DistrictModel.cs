using Base.Core.Domain.Thanas;
using Base.Framework.Models;
using Base.Web.Areas.Secure.Models.Thanas;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace Base.Web.Areas.Secure.Models.Districts
{
    public class DistrictModel: BaseCrmEntityModel
    {
        public string DistrictName { get; set; }
        public List<ThanaModel> Thanas { get; set; }
    }
}
