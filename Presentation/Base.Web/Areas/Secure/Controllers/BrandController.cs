using AutoMapper;
using Base.Core;
using Base.Core.Domain.Brands;
using Base.Core.Domain.ComplainTypes;
using Base.Services.Brands;
using Base.Services.BusinessUnits;
using Base.Services.Notification;
using Base.Web.Areas.Admin.Models.Brand;
using Base.Web.Areas.Secure.Models.Brand;
using Base.Web.Areas.Secure.Models.Thanas;
using Base.Web.Areas.Secure.Validators;
using Base.Web.Framework.Controllers;
using Base.Web.Framework.Models.Extensions;
using Base.Web.Framework.Mvc.Filters;
using Base.Web.Framework.MVC.Filters;
using DocumentFormat.OpenXml.Office2010.Excel;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Base.Web.Areas.Secure.Controllers
{

    public partial class BrandController : BaseAdminController
    {
        #region Fields
        private readonly IBrandService _brandService;
        private readonly IBusinessUnitService _businessUnitService;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        private readonly IValidator<BrandModel> _brandModelValidator;
        #endregion

        #region Ctor

        public BrandController(
            IBrandService brandService,
            IBusinessUnitService businessUnitService,
            INotificationService notificationService,
            IMapper mapper,
            IValidator<BrandModel> brandModelValidator)
        {
            _brandService = brandService;
            _businessUnitService = businessUnitService;
            _notificationService = notificationService;
            _mapper = mapper;
            _brandModelValidator = brandModelValidator;
        }

        #endregion

        #region Methods

        public virtual IActionResult List()
        {
            var model = new BrandSearchModel();

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> BrandList(BrandSearchModel searchModel)
        {
            var allBrands = await _brandService.GetAllBrandPaggedAsync(searchModel.BrandName);

            Func<IEnumerable<BrandModel>> dataFillFunction = () =>
            {
                return allBrands.Select(u =>
                {
                    var brandModel = _mapper.Map<BrandModel>(u);
                    brandModel.BusinessUnitName = _businessUnitService.GetBusinessUnitByIdAsync(u.BusinessUnit_Id).Result?.BusinessUnitName;
                    return brandModel;
                });
            };

            // Prepare list model
            var model = new BrandListModel().PrepareToGrid(searchModel, allBrands, dataFillFunction);
            return Json(model);
        }

        public virtual async Task<IActionResult> Create()
        {
            var model = new BrandModel();
            PopulateBusinessUnitsDropdown(model);
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Create(BrandModel model)
        {
            var validationResult = await _brandModelValidator.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                PopulateBusinessUnitsDropdown(model);
                return View(model );
            }

            var band = _mapper.Map<Brand>(model);
            await _brandService.InsertBrandAsync(band);
            _notificationService.SuccessNotification("Information has been saved successfully.");

            return RedirectToAction("List");
        }

        private async void PopulateBusinessUnitsDropdown(BrandModel model)
        {
           var allBusinessUnits = await _businessUnitService.GetAllBusinessUnitAsync();

            model.BusinessUnits.Add(new SelectListItem
            {
                Value = "-1",
                Text = "Please Select One"
            });
            foreach (var district in allBusinessUnits)
            {
                model.BusinessUnits.Add(new SelectListItem
                {
                    Value = district.Id.ToString(),
                    Text = district.BusinessUnitName.ToString()
                });
            }
        }

        public virtual async Task<IActionResult> Edit(int id)
        {

            var brand = await _brandService.GetBrandByIdAsync(id);

            if (brand == null)
                return RedirectToAction("List");

            var model = _mapper.Map<BrandModel>(brand);
            model.BusinessUnitName = _businessUnitService.GetBusinessUnitByIdAsync(brand.BusinessUnit_Id).Result?.BusinessUnitName;

            var allBusinessUnits = await _businessUnitService.GetAllBusinessUnitAsync();
            foreach (var businessUnit in allBusinessUnits)
            {
                if (businessUnit.Id == brand.BusinessUnit_Id)
                {
                    model.BusinessUnits.Add(new SelectListItem
                    {
                        Value = businessUnit.Id.ToString(),
                        Text = businessUnit.BusinessUnitName,
                        Selected = true
                    });
                }
                else
                {
                    model.BusinessUnits.Add(new SelectListItem
                    {
                        Value = businessUnit.Id.ToString(),
                        Text = businessUnit.BusinessUnitName
                    });
                }
            }
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BrandModel model, bool continueEditing)
        {
            var validationResult = await _brandModelValidator.ValidateAsync(model);

            var brand = await _brandService.GetBrandByIdAsync(model.Id);


            var updatedBrand = _mapper.Map<Brand>(model);

            await _brandService.UpdateBrandAsync(updatedBrand);
            _notificationService.SuccessNotification("Information has been updated successfully.");
            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = brand.Id });

        }


        [HttpPost]
        [Route("Secure/Brand/Delete")]
        public virtual async Task<IActionResult> Delete(int Id)
        {

            var brand = await _brandService.GetBrandByIdAsync(Id);
            if (brand != null)
            {
                try
                {
                    await _brandService.DeleteBrandAsync(brand);
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
        #endregion

    }
}