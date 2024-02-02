using Base.Web.Framework.Models;

namespace Base.Web.Areas.Secure.Models.ServiceCenter
{
    public class ServiceCenterSearchModel : BaseSearchModel
    {

        public string SearchServiceCenterName { get; set; }
        public int SearchServiceCenterbyThana{ get; set; }

    }
}
