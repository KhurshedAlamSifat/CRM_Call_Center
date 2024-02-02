using Base.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Base.Web.Areas.Secure.Models.Thanas
{
    public class ThanaModel : BaseCrmEntityModel
    {
        public ThanaModel() 
        {
            Districts = new List<SelectListItem>();
        }

        public string ThanaName { get; set; }

        public string DistrictName { get; set; }    
        public int District_Id { get; set; }

        public List<SelectListItem> Districts { get; set; }
    }
}
