using Base.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Base.Web.Areas.Secure.Models.ServiceCenter
{
    public class ServiceCenterModel : BaseCrmEntityModel
    {

        public ServiceCenterModel()
        {
            Thana = new List<SelectListItem>();
        }

        public string ServiceCenterName { get; set; }

        public string ThanaName { get; set; }
        public int Thana_Id { get; set; }

        public List<SelectListItem> Thana { get; set; }
    }
}
