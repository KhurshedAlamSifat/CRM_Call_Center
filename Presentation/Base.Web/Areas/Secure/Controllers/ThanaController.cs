using AutoMapper;
using Base.Core;
using Base.Core.Domain.Districts;
using Base.Core.Domain.Thanas;
using Base.Core.Domain.Users;
using Base.Services.Districts;
using Base.Services.Notification;
using Base.Services.Thanas;
using Base.Web.Areas.Admin.Models.Brand;
using Base.Web.Areas.Admin.Models.Users;
using Base.Web.Areas.Secure.Models.Brand;
using Base.Web.Areas.Secure.Models.Districts;
using Base.Web.Areas.Secure.Models.Thanas;
using Base.Web.Areas.Secure.Models.Users;
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
    public class ThanaController : BaseAdminController
    {
        private readonly IThanaService _thanaService;
        private readonly IDistrictService _districtService;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        private readonly IValidator<ThanaModel> _thanaModelValidator;

        public ThanaController(
            IThanaService thanaService,
            IDistrictService districtService,
            INotificationService notificationService,
            IMapper mapper,
            IValidator<ThanaModel> thanaModelValidator)
        {
            _thanaService = thanaService;
            _districtService = districtService;
            _notificationService = notificationService;
            _mapper = mapper;
            _thanaModelValidator = thanaModelValidator;
        }

        public virtual IActionResult List()
        {
            var model = new ThanaSearchModel();

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> ThanaList(ThanaSearchModel searchModel)
        {
            var allThanas = await _thanaService.GetAllThanaAsync(searchModel.SearchThanaName);

            Func<IEnumerable<ThanaModel>> dataFillFunction = () =>
            {
                return allThanas.Select(u =>
                {
                    var thanaModel = _mapper.Map<ThanaModel>(u);
                    thanaModel.DistrictName = _districtService.GetDistrictAsync(u.District_Id).Result?.DistrictName;
                    return thanaModel;
                });
            };

            // Prepare list model (assuming ThanaListModel inherits from BasePagedListModel<ThanaModel>)
            var model = new ThanaListModel().PrepareToGrid(searchModel, allThanas, dataFillFunction);
            return Json(model);
        }




        public async Task<IActionResult> Create()
        {
            var model = new ThanaModel();
            PopulateDistrictsDropdown(model);
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ThanaModel model, bool continueEditing)
        {
            var validationResult = await _thanaModelValidator.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                PopulateDistrictsDropdown(model);
                return View(model);
            }

            var thana = _mapper.Map<Thana>(model);
            await _thanaService.InsertThanaAsync(thana);
            _notificationService.SuccessNotification("Information has been saved successfully.");

            if (!continueEditing)
                return RedirectToAction("List");
            
            return RedirectToAction("Create");
        }

        private async void PopulateDistrictsDropdown(ThanaModel model)
        {
            var allDistricts = await _districtService.GetAllDistrictWithoutPaggingAsync();

            model.Districts.Add(new SelectListItem
            {
                Value = "-1",
                Text = "Please Select One"
            });
            foreach (var district in allDistricts)
            {
                model.Districts.Add(new SelectListItem
                {
                    Value = district.Id.ToString(),
                    Text = district.DistrictName.ToString()
                });
            }
        }

        public virtual async Task<IActionResult> Edit(int id)
        {

            var thana = await _thanaService.GetThanaAsync(id);

            if (thana == null)
                return RedirectToAction("List");

            var model = _mapper.Map<ThanaModel>(thana);
            model.DistrictName = _districtService.GetDistrictAsync(thana.District_Id).Result?.DistrictName;
            
            var allDistricts = await _districtService.GetAllDistrictWithoutPaggingAsync();
            foreach (var district in allDistricts)
            {
                if (district.Id == thana.District_Id)
                {
                    model.Districts.Add(new SelectListItem
                    {
                        Value = district.Id.ToString(),
                        Text = district.DistrictName,
                        Selected = true
                    });
                }
                else
                {
                    model.Districts.Add(new SelectListItem
                    {
                        Value = district.Id.ToString(),
                        Text = district.DistrictName
                    });
                }
            }
            return View(model);
        }






        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ThanaModel model, bool continueEditing)
        {
            var validationResult = await _thanaModelValidator.ValidateAsync(model);

            var thana = await _thanaService.GetThanaAsync(model.Id);


            var updatedThana = _mapper.Map<Thana>(model);

            await _thanaService.UpdateThanaAsync(updatedThana); 
            _notificationService.SuccessNotification("Information has been updated successfully.");
            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = thana.Id });

        }

        [HttpPost]
        [Route("Secure/Thana/Delete")]
        public async Task<IActionResult> Delete(int Id)
        {
            var thana = await _thanaService.GetThanaAsync(Id);
            if (thana != null)
            {
                try
                {
                    await _thanaService.DeleteThanaAsync(thana);
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
