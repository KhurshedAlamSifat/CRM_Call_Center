using System.Linq;
using System.Threading.Tasks;
using Base.Core;
using Base.Web.Areas.Secure.Controllers;
using Microsoft.AspNetCore.Mvc;


namespace Base.Web.Areas.Secure.Controllers
{
    public partial class HomeController : BaseAdminController
    {
        #region Fields
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public HomeController(IWorkContext workContext)
        {
            _workContext = workContext;
        }

        #endregion

        #region Methods

        public virtual IActionResult Index()
        {
            return View();
        }

        #endregion
    }
}