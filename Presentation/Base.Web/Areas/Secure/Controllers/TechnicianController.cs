using AutoMapper;
using Base.Core.Domain.ServiceCenters;
using Base.Core.Domain.Technicians;
using Base.Services.Categories;
using Base.Services.Notification;
using Base.Services.ServiceCenters;
using Base.Services.Technicians;
using Base.Services.Thanas;
using Base.Web.Areas.Secure.Models.ServiceCenter;
using Base.Web.Areas.Secure.Models.Technicians;
using Base.Web.Framework.Controllers;
using Base.Web.Framework.Models.Extensions;
using Base.Web.Framework.MVC.Filters;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Base.Web.Areas.Secure.Controllers
{
    public class TechnicianController : BaseAdminController
    {
        private readonly ITechnicianService _technicianService;
        private readonly IThanaService _thanaService;
        private readonly IServiceCenterService _serviceCenterService;
        private readonly INotificationService _notificationService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IValidator<TechnicianModel> _technicianModelValidator;

        public TechnicianController(
            ITechnicianService technicianService, 
            IServiceCenterService serviceCenterService,
            INotificationService notificationService, 
            IThanaService thanaService, 
            ICategoryService categoryService,
            IMapper mapper,
            IValidator<TechnicianModel> technicianModelValidator)
        {
            _technicianService = technicianService;
            _thanaService = thanaService;
            _serviceCenterService = serviceCenterService;
            _notificationService = notificationService;
            _categoryService = categoryService;
            _mapper = mapper;
            _technicianModelValidator = technicianModelValidator;
        }

        public IActionResult List()
        {
            var model = new TechnicianSearchModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> TechnicianList(TechnicianSearchModel searchModel)
        {
            var allTechnicians = await _technicianService.GetAllTechnicianAsync(
                technicianname: searchModel.SearchTechnicianName,
                thana: searchModel.SearchTechinicianbyThana, // Assuming this is used for filtering by district
                serviceCenter: searchModel.SearchTechnicianbyServiceCenter
               );

            // Prepare list model
            Func<IEnumerable<TechnicianModel>> dataFillFunction = () =>
            {
                return allTechnicians.Select(u =>
                {
                    var technicianModel = _mapper.Map<TechnicianModel>(u);

                    if (u.Thana_Id.HasValue)
                    {
                        technicianModel.Thana_Id = u.Thana_Id.Value;
                        technicianModel.ThanaName = _thanaService.GetThanaAsync(u.Thana_Id.Value).Result?.ThanaName;
                    }
                    if (u.ServiceCenter_Id.HasValue)
                    {
                        technicianModel.ServiceCenter_Id = u.ServiceCenter_Id.Value;
                        technicianModel.ServiceCenterName = _serviceCenterService.GetServiceCenterAsync(u.ServiceCenter_Id.Value).Result?.ServiceCenterName;
                    }
                    if (u.Category_Id.HasValue)
                    {
                        technicianModel.Category_Id = u.Category_Id.Value;
                        technicianModel.ProductCategoryName = _categoryService.GetCategoryByIdAsync(u.Category_Id.Value).Result?.CategoryName;
                    }

                    return technicianModel;
                });
            };

            var model = new TechnicianListModel().PrepareToGrid(searchModel, allTechnicians, dataFillFunction);

            return Json(model);
        }


        public async Task<IActionResult> Create()
        {
            var model = new TechnicianModel();

            model.serviceCenter.Add(new SelectListItem
            {
                Value = "-1",
                Text = "Select One"
            });

            var allServiceCenters = await _serviceCenterService.GetServiceCenterAsync();
            foreach (var serviceCenter in allServiceCenters)
            {
                model.serviceCenter.Add(new SelectListItem
                {
                    Value = serviceCenter.Id.ToString(),
                    Text = serviceCenter.ServiceCenterName
                });
            }

            model.thana.Add(new SelectListItem
            {
                Value = "-1",
                Text = "Select One"
            });

            var allThanas = await _thanaService.GetThanaAsync();
            foreach (var thana in allThanas)
            {
                model.thana.Add(new SelectListItem
                {
                    Value = thana.Id.ToString(),
                    Text = thana.ThanaName
                });
            }
            model.productCategory.Add(new SelectListItem
            {
                Value = "-1",
                Text = "Select One"
            });

            var allCategories =await _categoryService.GetAllCategoryAsync();
            foreach (var category in allCategories)
            {
                model.productCategory.Add(new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.CategoryName
                });
            }

            return View(model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TechnicianModel model, bool continueEditing)
        {
            var validationResult = _technicianModelValidator.ValidateAsync(model);

            var technician = _mapper.Map<Technician>(model);
            technician.Thana_Id = model.Thana_Id == -1 ? null : model.Thana_Id;
            technician.ServiceCenter_Id = model.ServiceCenter_Id == -1 ? null : model.ServiceCenter_Id;
            technician.Category_Id = model.Category_Id == -1 ? null : model.Category_Id;
            technician.TechnicianName = model.TechnicianName;


            await _technicianService.InsertTechnicianAsync(technician);
            _notificationService.SuccessNotification("Information has been saved successfully.");

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Create");
           
        }

        public async Task<IActionResult> Edit(int id)
        {
            var technician = await _technicianService.GetTechnicianAsync(id);

            if (technician == null)
                return RedirectToAction("List");

            var model = _mapper.Map<TechnicianModel>(technician);

            // Populate dropdown lists
            PopulateDropdownLists(model);

            return View(model);
        }

        private async void PopulateDropdownLists(TechnicianModel model)
        {
            model.serviceCenter.Add(new SelectListItem
            {
                Value = "-1",
                Text = "Please Select One"
            });

            var allServiceCenters = await _serviceCenterService.GetServiceCenterAsync();
            foreach (var serviceCenter in allServiceCenters)
            {
                model.serviceCenter.Add(new SelectListItem
                {
                    Value = serviceCenter.Id.ToString(),
                    Text = serviceCenter.ServiceCenterName
                });
            }
            model.thana.Add(new SelectListItem
            {
                Value = "-1",
                Text = "Please Select One"
            });

            var allThanas = await _thanaService.GetThanaAsync();
            foreach (var thana in allThanas)
            {
                model.thana.Add(new SelectListItem
                {
                    Value = thana.Id.ToString(),
                    Text = thana.ThanaName
                });
            }

            model.productCategory.Add(new SelectListItem
            {
                Value = "-1",
                Text = "Please Select One"
            });

            var allCategories = await _categoryService.GetAllCategoryAsync();
            foreach (var category in allCategories)
            {
                model.productCategory.Add(new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.CategoryName
                });
            }
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TechnicianModel model, bool continueEditing)
        {
            var validationResult = _technicianModelValidator.ValidateAsync(model);
            var technician = await _technicianService.GetTechnicianAsync(model.Id);

            var updatedTechnician = _mapper.Map<Technician>(model);

            await _technicianService.UpdateTechnicianAsync(updatedTechnician);
            _notificationService.SuccessNotification("Information has been updated successfully.");
            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = technician.Id });
            
        }


        [HttpPost]
        [Route("Secure/Technician/Delete")]
        public async Task<IActionResult> Delete(int Id)
        {
            var technician = await _technicianService.GetTechnicianAsync(Id);
            if (technician != null)
            {
                try
                {
                    await _technicianService.DeleteTechnicianAsync(technician);
                    _notificationService.SuccessNotification("Deleted.");
                }
                catch (Exception ex)
                {
                    _notificationService.SuccessNotification($"Can not delete. See the msg {ex.Message}");

                }

            }
            else
            {
                _notificationService.ErrorNotification("Not Exist");

            }
            return RedirectToAction("List");
        }
    }


}
