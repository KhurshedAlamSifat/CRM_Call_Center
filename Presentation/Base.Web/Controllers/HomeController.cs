using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Base.Web.Models;
using Base.Services.Authentication;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.CodeAnalysis.Options;
using Base.Services.Users;
using Base.Services.ComplainTypes;
using Base.Core.Domain.ComplainTypes;

namespace Base.Web.Controllers
{
    public partial class HomeController : Controller
    {
        private readonly IUserService _userService;
        
        public HomeController(IUserService userService)
        {
            _userService = userService;
        }

        public virtual IActionResult Index()
        {
            return View();
        }

    }
}
