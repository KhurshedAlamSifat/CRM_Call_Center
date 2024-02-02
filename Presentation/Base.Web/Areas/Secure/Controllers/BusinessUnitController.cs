using AutoMapper;
using Base.Core;
using Base.Core.Domain.Brands;
using Base.Core.Domain.BusinessUnits;
using Base.Services.BusinessUnits;
using Base.Services.Notification;
using Base.Web.Areas.Admin.Models.BusinessUnit;
using Base.Web.Areas.Secure.Models.BusinessUnit;
using Base.Web.Framework.Controllers;
using Base.Web.Framework.Models.Extensions;
using Base.Web.Framework.Mvc.Filters;
using Base.Web.Framework.MVC.Filters;
using DocumentFormat.OpenXml.Office2010.Excel;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Base.Web.Areas.Secure.Controllers
{

    public partial class BusinessUnitController : BaseAdminController
    {
        #region Fields
        private readonly IMapper _mapper;
        private readonly IBusinessUnitService _businessUnitService;
        private readonly INotificationService _notificationService;
        private readonly IValidator<BusinessUnitModel> _businessUnitModelValidator;

        #endregion

        #region Ctor

        public BusinessUnitController(
            IBusinessUnitService businessUnitService,
            INotificationService notificationService,
            IMapper mapper,
            IValidator<BusinessUnitModel> businessUnitModelValidator)
        {
            _businessUnitService = businessUnitService;
            _notificationService = notificationService;
            _mapper = mapper;
            _businessUnitModelValidator = businessUnitModelValidator;
        }

        #endregion

        #region Methods

        public virtual IActionResult List()
        {
            var model = new BusinessUnitSearchModel();

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> BusinessUnitList(BusinessUnitSearchModel searchModel)
        {
            var allBusinessUnits = await _businessUnitService.GetAllBusinessUnitPaggedAsync(searchModel.BusinessUnitName);

            //prepare list model
            Func<IEnumerable<BusinessUnitModel>> dataFillFunction = () =>
            {
                return allBusinessUnits.Select(u =>
                {
                    var businessUnitModel = _mapper.Map<BusinessUnitModel>(u);

                    return businessUnitModel;
                });
            };

            var model = new BusinessUnitListModel().PrepareToGrid(searchModel, allBusinessUnits, dataFillFunction);

            return Json(model);
        }

        public virtual IActionResult Create()
        {
            //prepare model
            var model = new BusinessUnitModel();

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Create(BusinessUnitModel model)
        {
            var validationResult = await _businessUnitModelValidator.ValidateAsync(model);
            if (ModelState.IsValid)
            {
                var businessUnit = _mapper.Map<BusinessUnit>(model);
                await _businessUnitService.InsertBusinessUnitAsync(businessUnit);
                _notificationService.SuccessNotification("Information has been saved successfully.");

                return RedirectToAction("List");
            }
            return View(model);
        }

        public virtual async Task<IActionResult> Edit(int id)
        {

            var manageBusinessUnit = await _businessUnitService.GetBusinessUnitByIdAsync(id);

            if (manageBusinessUnit == null)
                return RedirectToAction("List", "BusinessUnit");

            var model = _mapper.Map<BusinessUnitModel>(manageBusinessUnit);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BusinessUnitModel model, bool continueEditing)
        {
            var validationResult = await _businessUnitModelValidator.ValidateAsync(model);

            var manageBusinessUnit = await _businessUnitService.GetBusinessUnitByIdAsync(model.Id);


            var updatedBusinessUnit = _mapper.Map<BusinessUnit>(model);

            await _businessUnitService.UpdateBusinessUnitAsync(updatedBusinessUnit); ;
            _notificationService.SuccessNotification("Information has been updated successfully.");
            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = manageBusinessUnit.Id });

        }


        [HttpPost]
        [Route("Secure/BusinessUnit/Delete")]
        public virtual async Task<IActionResult> Delete(int Id)
        {

            var businessUnit = await _businessUnitService.GetBusinessUnitByIdAsync(Id);
            if (businessUnit != null)
            {
                try
                {
                    await _businessUnitService.DeleteBusinessUnitAsync(businessUnit);
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