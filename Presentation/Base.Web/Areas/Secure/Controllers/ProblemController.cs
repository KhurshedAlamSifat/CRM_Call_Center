using Base.Core.Domain.ComplainTypes;
using Base.Core;
using Base.Services.ComplainTypes;
using Base.Web.Areas.Secure.Models.ComplainTypes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Base.Services.Problems;
using Base.Web.Areas.Secure.Models.Problems;
using Base.Web.Framework.Models.Extensions;
using System.Linq;
using Base.Core.Domain.Problems;
using Base.Core.Domain.Thanas;
using Base.Services.Notification;
using System;
using Base.Web.Framework.Controllers;
using Base.Web.Framework.MVC.Filters;
using Base.Services.Categories;
using Base.Web.Areas.Secure.Models.Category;
using Microsoft.AspNetCore.Mvc.Rendering;
using DocumentFormat.OpenXml.EMMA;
using AutoMapper;
using FluentValidation;
using Base.Web.Areas.Secure.Models.ServiceCenter;
using System.Collections.Generic;
using Base.Core.Domain.ServiceCenters;
using Base.Web.Areas.Secure.Models.Thanas;

namespace Base.Web.Areas.Secure.Controllers
{
    public class ProblemController : BaseAdminController
    {
        private readonly IProblemService _problemService;
        private readonly INotificationService _notificationService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IValidator<ProblemTypeModel> _problemTypeModelVlidator;

        public ProblemController(
            IProblemService problemService, 
            INotificationService notificationService, 
            ICategoryService categoryService,
            IMapper mapper,
            IValidator<ProblemTypeModel> problemTypeModelVlidator)
        {
            _problemService = problemService;
            _notificationService = notificationService;
            _categoryService = categoryService;
            _mapper = mapper;
            _problemTypeModelVlidator = problemTypeModelVlidator;
        }

        public virtual IActionResult ProblemList()
        {
            var model = new ProblemTypeSearchModel();

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> ProblemList(ProblemTypeSearchModel searchModel)
        {
            var allProblems = await _problemService.GetAllProblemAsync(searchModel.SearchProblemName);
            Func<IEnumerable<ProblemTypeModel>> dataFillFunction = () =>
            {
                return allProblems.Select(u =>
                {
                    var problemModel = _mapper.Map<ProblemTypeModel>(u);
                    problemModel.CategoryName = _categoryService.GetCategoryByIdAsync(u.CategoryId).Result?.CategoryName;
                    return problemModel;
                });
            };

            var model = new ProblemListModel().PrepareToGrid(searchModel, allProblems, dataFillFunction);
            return Json(model);
        }

        public virtual async Task<IActionResult> Create()
        {
            var model = new ProblemTypeModel();
            PopulateCategoriesDropdown(model);
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProblemTypeModel model, bool continueEditing)
        {
            var validationResult = await _problemTypeModelVlidator.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                PopulateCategoriesDropdown(model);

                return View(model);
            }

            var problem = _mapper.Map<Problem>(model);

            await _problemService.InsertProblemAsync(problem);
            _notificationService.SuccessNotification("Information has been saved successfully.");
            if (!continueEditing)
                return RedirectToAction("ProblemList");

            return RedirectToAction("Create");
           
        }

        private async void PopulateCategoriesDropdown(ProblemTypeModel model)
        {
            var allCategories = await _categoryService.GetAllCategoryAsync();

            model.Categories.Add(new SelectListItem
            {
                Value = "-1",
                Text = "Please Select One"
            });
            foreach (var category in allCategories)
            {
                model.Categories.Add(new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.CategoryName.ToString()
                });
            }
        }

        public virtual async Task<IActionResult> Edit(int id)
        {
            var problem = await _problemService.GetProblemByIdAsync(id);

            if (problem == null)
            {
                return NotFound();
            }

            var problemModel = _mapper.Map<ProblemTypeModel>(problem);
            problemModel.CategoryName = _categoryService.GetCategoryByIdAsync(problem.CategoryId).Result?.CategoryName;
            var allCategories = await _categoryService.GetAllCategoryAsync();
            foreach (var category in allCategories)
            {
                if (category.Id == problem.CategoryId)
                {
                    problemModel.Categories.Add(new SelectListItem
                    {
                        Value = category.Id.ToString(),
                        Text = category.CategoryName,
                        Selected = true
                    });
                }
                else
                {
                    problemModel.Categories.Add(new SelectListItem
                    {
                        Value = category.Id.ToString(),
                        Text = category.CategoryName
                    });
                }
            }
            return View(problemModel);
        }


        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Edit(ProblemTypeModel problemModel, bool continueEditing)
        {
            var validationResult = await _problemTypeModelVlidator.ValidateAsync(problemModel);
            var problem = await _problemService.GetProblemAsync(problemModel.Id);


            var updatedProblem = _mapper.Map<Problem>(problemModel);

            await _problemService.UpdateProblemAsync(updatedProblem);
            _notificationService.SuccessNotification("Information has been updated successfully.");
            if (!continueEditing)
                return RedirectToAction("ProblemList");

            return RedirectToAction("Edit", new { id = problem.Id });
        }

        [HttpPost]
        [Route("Secure/Problem/Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var problem = await _problemService.GetProblemAsync(id);

            if (problem != null)
            {
                try
                {
                    await _problemService.DeleteProblemAsync(problem);
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
            return RedirectToAction("ProblemList");
        }
    }
}
