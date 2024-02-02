using AutoMapper;
using Base.Core;
using Base.Core.Domain.Districts;
using Base.Core.Domain.Thanas;
using Base.Services.Districts;
using Base.Services.Notification;
using Base.Services.Thanas;
using Base.Web.Areas.Admin.Models.BusinessUnit;
using Base.Web.Areas.Admin.Models.Users;
using Base.Web.Areas.Secure.Models.BusinessUnit;
using Base.Web.Areas.Secure.Models.Districts;
using Base.Web.Areas.Secure.Models.Thanas;
using Base.Web.Areas.Secure.Models.Users;
using Base.Web.Framework.Controllers;
using Base.Web.Framework.Models.Extensions;
using Base.Web.Framework.MVC.Filters;
using DocumentFormat.OpenXml.EMMA;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Base.Web.Areas.Secure.Controllers
{
    public class DistrictController : BaseAdminController
    {
        private readonly IDistrictService _districtService;
        private readonly IThanaService _thanaService;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        private readonly IValidator<DistrictModel> _districtModelValidator;

        public DistrictController(
            IDistrictService districtService,
            IThanaService thanaService,
            INotificationService notificationService,
            IMapper mapper,
            IValidator<DistrictModel> districtModelValidator)
        {
            _districtService = districtService;
            _thanaService = thanaService;
            _notificationService = notificationService;
            _mapper = mapper;
            _districtModelValidator = districtModelValidator;
        }

        public virtual IActionResult List()
        {
            var model = new DistrictSearchModel();

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> DistrictList(DistrictSearchModel searchModel)
        {
            var allDistricts = await _districtService.GetAllDistrictsAsync(searchModel.DistrictName);

            //prepare list model
            Func<IEnumerable<DistrictModel>> dataFillFunction = () =>
            {
                return allDistricts.Select(u =>
                {
                    var districtModel = _mapper.Map<DistrictModel>(u);

                    return districtModel;
                });
            };

            var model = new DistrictListModel().PrepareToGrid(searchModel, allDistricts, dataFillFunction);

            return Json(model);
        }



        public IActionResult Create()
        {
            var model = new DistrictModel();
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DistrictModel model, bool continueEditing)
        {
            var validationResult = await _districtModelValidator.ValidateAsync(model);  
            if (ModelState.IsValid)
            {
                var district = _mapper.Map<District>(model);

                await _districtService.InsertDistrictAsync(district);
                _notificationService.SuccessNotification("Information has been saved successfully.");
                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Create");
            }
            return View(model);
        }

        public virtual async Task<IActionResult> Edit(int id)
        {
            var district = await _districtService.GetDistrictAsync(id);

            if (district == null)
            {
                return NotFound();
            }

            var districtModel = _mapper.Map<DistrictModel>(district);
            return View(districtModel);
        }


        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Edit(DistrictModel districtModel, bool continueEditing)
        {
            var validationResult = await _districtModelValidator.ValidateAsync(districtModel);

            var manageDistrict = await _districtService.GetDistrictAsync(districtModel.Id);


            var updatedDistrict = _mapper.Map<District>(districtModel);
            await _districtService.UpdateDistrictAsync(updatedDistrict);
            _notificationService.SuccessNotification("Information has been updated successfully.");
            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = manageDistrict.Id });

        }

        [HttpPost]
        [Route("Secure/District/Delete")]
        public virtual async Task<IActionResult> Delete(int Id)
        {

            var district = await _districtService.GetDistrictAsync(Id);
            if (district != null)
            {
                try
                {
                    await _districtService.DeleteDistrictAsync(district);
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
