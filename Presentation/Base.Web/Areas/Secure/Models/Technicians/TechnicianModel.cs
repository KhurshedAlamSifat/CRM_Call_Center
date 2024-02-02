using Base.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Syncfusion.Pdf.Lists;
using System.Collections.Generic;

namespace Base.Web.Areas.Secure.Models.Technicians
{
    public class TechnicianModel : BaseCrmEntityModel
    {
        public TechnicianModel()
        {
            thana = new List<SelectListItem>();
            serviceCenter = new List<SelectListItem>();
            productCategory=new List<SelectListItem>();
        }

        public string TechnicianName { get; set; }
        public string TechnicianPhoneNo { get; set; }

        public string ThanaName { get; set; }
        public int? Thana_Id { get; set; } 

        public string ServiceCenterName { get; set; }
        public int? ServiceCenter_Id { get; set; }


        public string ProductCategoryName { get; set; }
        public int? Category_Id { get; set; }

        public List<SelectListItem> thana { get; set; }
        public List<SelectListItem> serviceCenter { get; set; }
        public List<SelectListItem> productCategory { get; set; }

    }

}

