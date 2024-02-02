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
using Base.Core;
using Base.Web.Models.Ticket;
using Base.Services.Customers;
using Base.Services.Districts;
using Base.Services.Thanas;
using Base.Services.BusinessUnits;
using Base.Services.Categories;
using Base.Services.ServiceCenters;
using Base.Services.Technicians;
using Base.Core.Domain.Technicians;

namespace Base.Web.Controllers
{
    public partial class TicketController : Controller
    {
        private readonly IWorkContext _workContext;
        private readonly IUserService _userService;
        private readonly IComplainTypeService _complainTypeService;
        private readonly ICustomerService _customerService;
        private readonly IDistrictService _districtService;
        private readonly IThanaService _thanaService;
        private readonly IBusinessUnitService _businessUnitService;
        private readonly ICategoryService _categoryService;
        private readonly IServiceCenterService _serviceCenterService;
        private readonly ITechnicianService _technicianService;

        public TicketController(IWorkContext workContext, IUserService userService,
            IComplainTypeService complainTypeService, ICustomerService customerService,
            IDistrictService districtService, IThanaService thanaService,
            IBusinessUnitService businessUnitService, ICategoryService categoryService,
            IServiceCenterService serviceCenterService, ITechnicianService technicianService)
        {
            _workContext = workContext;
            _userService = userService;
            _complainTypeService = complainTypeService;
            _customerService = customerService;
            _districtService = districtService;
            _thanaService = thanaService;
            _businessUnitService = businessUnitService;
            _categoryService = categoryService;
            _serviceCenterService = serviceCenterService;
            _technicianService = technicianService;
        }

        public virtual async Task<IActionResult> TicketEntryForm(string phonenumber)
        {
            var user = await _workContext.GetCurrentUserAsync();
            if (!await _userService.IsCallCenterAgentAsync(user) && !await _userService.IsAdminAsync(user))
            {
                return RedirectToRoute("Login");
            }
            var allComplains = await _complainTypeService.GetAllComplainTypeAsync();
            var allDistrict = await _districtService.GetAllDistrictWithoutPaggingAsync();
            var customer = await _customerService.GetCustomerByPhoneNo(phonenumber);
            var allCategories = await _categoryService.GetAllCategoryAsync();

            var model = new TicketModel();
            model.CustomerPhoneNo = customer?.PhoneNumber ?? phonenumber;
            model.CustomerId = customer?.Id ?? 0;
            model.CustomerName = customer?.Name ?? string.Empty;
            model.District_Id = customer?.District_Id ?? 0;




            model.ComplainTypes.Add(new SelectListItem
            {
                Value = "-1",
                Text = "Select One"
            });
            foreach (var complain in allComplains)
            {
                model.ComplainTypes.Add(new SelectListItem
                {
                    Value = complain.Id.ToString(),
                    Text = complain.ComplainTypeName
                });
            }
            model.Districts.Add(new SelectListItem
            {
                Value = "-1",
                Text = "Select One"
            });
            foreach (var district in allDistrict)
            {
                model.Districts.Add(new SelectListItem
                {
                    Value = district.Id.ToString(),
                    Text = district.DistrictName
                });
            }
            model.Thanas.Add(new SelectListItem
            {
                Value = "-1",
                Text = "Select One"
            });
            if (model.Thana_Id.HasValue && model.District_Id.HasValue)
            {
                model.Thana_Id = customer.Thana_Id;
                var allThanas = await _thanaService.GetThanaByDistrictAsync(model.District_Id.Value);
                foreach (var thana in allThanas)
                {
                    model.Thanas.Add(new SelectListItem
                    {
                        Value = thana.Id.ToString(),
                        Text = thana.ThanaName
                    });
                }
            }

            model.Categories.Add(new SelectListItem
            {
                Value = "-1",
                Text = "Select One"
            });
            foreach (var category in allCategories)
            {
                model.Categories.Add(new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.CategoryName
                });
            }

            model.BusinessUnits.Add(new SelectListItem
            {
                Value = "-1",
                Text = "Select One"
            });

            model.Brands.Add(new SelectListItem
            {
                Value = "-1",
                Text = "Select One"
            });

            model.ServiceCenters.Add(new SelectListItem
            {
                Value = "-1",
                Text = "Select One"
            });

            model.Problems.Add(new SelectListItem
            {
                Value = "-1",
                Text = "Select One"
            });

            model.Technicians.Add(new SelectListItem
            {
                Value = "-1",
                Text = "Select One"
            });
            if (model.Thana_Id.HasValue)
            {
                var allServiceCenters = await _serviceCenterService.GetServiceCenterByThanaAsync(model.Thana_Id.Value);
                foreach (var serviceCenters in allServiceCenters)
                {
                    model.ServiceCenters.Add(new SelectListItem
                    {
                        Value = serviceCenters.Id.ToString(),
                        Text = serviceCenters.ServiceCenterName
                    });
                }
            }
            if (model.Thana_Id.HasValue)
            {
                var allTechnicians = await _technicianService.GetTechnicianByThanaAsynce(model.Thana_Id.Value);
                foreach (var technician in allTechnicians)
                {
                    model.Technicians.Add(new SelectListItem
                    {
                        Value = technician.Id.ToString(),
                        Text = technician.TechnicianName
                    });
                }
            }

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> TicketEntryForm(TicketModel model)
        {
            return View(model);
        }
    }
}
