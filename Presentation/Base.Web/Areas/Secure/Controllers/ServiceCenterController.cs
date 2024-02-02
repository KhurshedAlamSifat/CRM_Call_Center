using AutoMapper;
using Base.Core.Domain.Brands;
using Base.Core.Domain.ServiceCenters;
using Base.Core.Domain.Thanas;
using Base.Services.Notification;
using Base.Services.ServiceCenters;
using Base.Services.Thanas;
using Base.Web.Areas.Secure.Models.ServiceCenter;
using Base.Web.Areas.Secure.Models.Thanas;
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
    public class ServiceCenterController : BaseAdminController
    {
        private readonly IServiceCenterService _serviceCenterService;
        private readonly IThanaService _thanaService;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        private readonly IValidator<ServiceCenterModel> _serviceCenterModelValidator;

        public ServiceCenterController(
            IServiceCenterService serviceCentetService, 
            IThanaService thanaService, 
            INotificationService notificationService,
            IMapper mapper,
            IValidator<ServiceCenterModel> serviceCenterModelValidator)
        {

            _thanaService = thanaService;
            _serviceCenterService = serviceCentetService;
            _notificationService = notificationService;
            _mapper = mapper;
            _serviceCenterModelValidator = serviceCenterModelValidator;
        }



        public virtual IActionResult List()
        {
            var model = new ServiceCenterSearchModel();
            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> ServiceCenterList(ServiceCenterSearchModel searchModel)
        {

            var allServiceCenters = await _serviceCenterService.GetAllServiceCenterAsync(searchModel.SearchServiceCenterName);

            Func<IEnumerable<ServiceCenterModel>> dataFillFunction = () =>
            {
                return allServiceCenters.Select(u =>
                {
                    var serviceCenterModel = _mapper.Map<ServiceCenterModel>(u);
                    serviceCenterModel.ThanaName = _thanaService.GetThanaAsync(u.Thana_Id).Result?.ThanaName;
                    return serviceCenterModel;
                });
            };

            var model = new ServiceCenterListModel().PrepareToGrid(searchModel, allServiceCenters, dataFillFunction);
            return Json(model);
        }



        public async Task<IActionResult> Create()
        {
            var model = new ServiceCenterModel();
            PopulateThanasDropdown(model);
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceCenterModel model, bool continueEditing)
        {
            var validationResult = await _serviceCenterModelValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                PopulateThanasDropdown(model);
                return View(model);
            }
            var serviceCenter = _mapper.Map<ServiceCenter>(model);

            await _serviceCenterService.InsertServiceCenterAsync(serviceCenter);
            _notificationService.SuccessNotification("Information has been saved successfully.");

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Create");
            
        }

        private async void PopulateThanasDropdown(ServiceCenterModel model)
        {
            model.Thana.Add(new SelectListItem
            {
                Value = "-1",
                Text = "Select One"
            });
            var allThanas = await _thanaService.GetThanaAsync();
            foreach (var thana in allThanas)
            {
                model.Thana.Add(new SelectListItem
                {
                    Value = thana.Id.ToString(),
                    Text = thana.ThanaName
                });
            }
        }

        public virtual async Task<IActionResult> Edit(int id)
        {

            var serviceCenter = await _serviceCenterService.GetServiceCenterAsync(id);

            if (serviceCenter == null)
                return RedirectToAction("List");

            var model = _mapper.Map<ServiceCenterModel>(serviceCenter);
            model.ThanaName = _thanaService.GetThanaAsync(serviceCenter.Thana_Id).Result?.ThanaName;
            var allThanas = await _thanaService.GetThanaAsync();
            foreach (var thana in allThanas)
            {
                if (thana.Id == serviceCenter.Thana_Id)
                {
                    model.Thana.Add(new SelectListItem
                    {
                        Value = thana.Id.ToString(),
                        Text = thana.ThanaName,
                        Selected = true
                    });
                }
                else
                {
                    model.Thana.Add(new SelectListItem
                    {
                        Value = thana.Id.ToString(),
                        Text = thana.ThanaName
                    });
                }
            }
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ServiceCenterModel model, bool continueEditing)
        {
            var validationResult = await _serviceCenterModelValidator.ValidateAsync(model);
            var serviceCenter = await _serviceCenterService.GetServiceCenterAsync(model.Id);


            var updatedServiceCenter = _mapper.Map<ServiceCenter>(model); 
            await _serviceCenterService.UpdateServiceCenterAsync(updatedServiceCenter);
            _notificationService.SuccessNotification("Information has been updated successfully.");
            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = serviceCenter.Id });

        }

        [HttpPost]
        [Route("Secure/ServiceCenter/Delete")]
        public async Task<IActionResult> Delete(int Id)
        {
            var serviceCenter = await _serviceCenterService.GetServiceCenterAsync(Id);

            if (serviceCenter != null)
            {
                try
                {
                    await _serviceCenterService.DeleteServiceCenterAsync(serviceCenter);
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
