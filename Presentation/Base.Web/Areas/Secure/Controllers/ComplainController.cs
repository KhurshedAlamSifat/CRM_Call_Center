using AutoMapper;
using Base.Core;
using Base.Core.Domain.ComplainTypes;
using Base.Services.ComplainTypes;
using Base.Services.Notification;
using Base.Web.Areas.Secure.Models.ComplainTypes;
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
    public class ComplainController : BaseAdminController
    {
        #region Fields
        private readonly IMapper _mapper;
        private readonly IWorkContext _workContext;
        private readonly IComplainTypeService _complainTypeService;
        private readonly INotificationService _notificationService;
        private readonly IValidator<ComplainTypeModel> _complainTypeModelValidator;
        #endregion

        #region Ctor
        public ComplainController(
            IWorkContext workContext,
            IComplainTypeService complainTypeService,
            IMapper mapper,
            INotificationService notificationService,
            IValidator<ComplainTypeModel> complainTypeModelValidator)
        {
            _workContext = workContext;
            _complainTypeService = complainTypeService;
            _mapper = mapper;
            _notificationService = notificationService;
            _complainTypeModelValidator = complainTypeModelValidator;
        }
        #endregion

        #region Methods
        public virtual IActionResult ComplainList()
        {
            var model = new ComplainTypeSearchModel();

            return View(model);
        }

        [HttpPost]
        public IActionResult ComplainList(ComplainTypeSearchModel searchModel)
        {
            var allComplains = _complainTypeService.GetAllComplainAsync(complainTypeName: searchModel.SearchComplainTypeName).Result;

            // Create a function to return an IEnumerable<ComplainTypeModel>
            Func<IEnumerable<ComplainTypeModel>> dataFillFunction = () =>
            {
                return allComplains.Select(u =>
                {
                    // Map your entity to ComplainTypeModel using AutoMapper
                    var complainModel = _mapper.Map<ComplainTypeModel>(u);

                    return complainModel;
                });
            };

            // Call the PrepareToGrid method with the dataFillFunction
            var model = new ComplainListModel().PrepareToGrid(searchModel, allComplains, dataFillFunction);

            return Json(model);
        }
        public IActionResult Create()
        {
            var model = new ComplainTypeModel();
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ComplainTypeModel model)
        {
            var validationResult = await _complainTypeModelValidator.ValidateAsync(model);
            if (ModelState.IsValid)
            {
                var complainType = _mapper.Map<ComplainType>(model);

                await _complainTypeService.InsertComplainTypeAsync(complainType);
                _notificationService.SuccessNotification("Information has been created successfully.");
                return RedirectToAction("ComplainList");
            }
            return View(model);
        }

        public virtual async Task<IActionResult> Edit(int id)
        {
            var complainType = await _complainTypeService.GetComplainTypeAsync(id);

            if (complainType == null)
            {
                return NotFound();
            }

            var complainModel = _mapper.Map<ComplainTypeModel>(complainType);

            return View(complainModel);
        }


        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Edit(int id, ComplainTypeModel complainModel)
        {
            var validationResult = await _complainTypeModelValidator.ValidateAsync(complainModel);
            if (id != complainModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var updatedComplainType = _mapper.Map<ComplainType>(complainModel);

                await _complainTypeService.UpdateComplainTypeAsync(updatedComplainType);
                _notificationService.SuccessNotification("Information has been updated successfully.");
                return RedirectToAction("ComplainList");
            }
            return View(complainModel);
        }


        [HttpPost]
        [Route("Secure/Complain/Delete")]
        public virtual async Task<IActionResult> Delete(int Id)
        {
            var thana = await _complainTypeService.GetComplainTypeAsync(Id);
            if (thana != null)
            {
                try
                {
                    await _complainTypeService.DeleteComplainTypeAsync(thana);
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
            return RedirectToAction("ComplainList");
        }
        #endregion
    }
}
