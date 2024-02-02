using AutoMapper;
using Base.Core;
using Base.Core.Domain.Brands;
using Base.Core.Domain.Categories;
using Base.Services.Brands;
using Base.Services.Categories;
using Base.Services.Notification;
using Base.Web.Areas.Admin.Models.Brand;
using Base.Web.Areas.Admin.Models.Category;
using Base.Web.Areas.Secure.Models.Brand;
using Base.Web.Areas.Secure.Models.Category;
using Base.Web.Areas.Secure.Models.Thanas;
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

    public partial class CategoryController : BaseAdminController
    {
        #region Fields
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        private readonly IValidator<CategoryModel> _categoryModelValidator;
        #endregion

        #region Ctor

        public CategoryController(
            ICategoryService categoryService, 
            IBrandService brandService, 
            INotificationService notificationService,
            IMapper mapper,
            IValidator<CategoryModel> categoryModelValidator)
        {
            _brandService = brandService;
            _categoryService = categoryService;
            _notificationService = notificationService;
            _mapper = mapper;
            _categoryModelValidator = categoryModelValidator;
        }

        #endregion

        #region Methods

        public virtual IActionResult List()
        {
            var model = new CategorySearchModel();

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> CategoryList(CategorySearchModel searchModel)
        {
            var allCategorys = await _categoryService.GetAllCategoryPaggedAsync(searchModel.CategoryName);

            //prepare list model
            Func<IEnumerable<CategoryModel>> dataFillFunction = () =>
            {
                return allCategorys.Select(u =>
                {
                    var categoryModel = _mapper.Map<CategoryModel>(u);
                    categoryModel.BrandName = _brandService.GetBrandByIdAsync(u.Brand_Id).Result?.BrandName;
                    return categoryModel;
                });
            };

            // Prepare list model
            var model = new CategoryListModel().PrepareToGrid(searchModel, allCategorys, dataFillFunction);

            return Json(model);
        }

        public virtual async Task<IActionResult> Create()
        {
            var model = new CategoryModel();
            PopulateBrandsDropdown(model);
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Create(CategoryModel model, bool continueEditing)
        {
            var validationResult = await _categoryModelValidator.ValidateAsync(model);
            
            if(!validationResult.IsValid)
            {
                PopulateBrandsDropdown(model);
                return View(model);
            }

            var cat = _mapper.Map<Category>(model);
            await _categoryService.InsertCategoryAsync(cat);
            _notificationService.SuccessNotification("Information has been saved successfully.");
            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Create");
        }

        private async void PopulateBrandsDropdown(CategoryModel model)
        {
            var allBrands = await _brandService.GetAllBrandAsync();
            model.Brands.Add(new SelectListItem
            {
                Value = "0",
                Text = "Please Select One"
            });
            foreach (var brand in allBrands)
            {
                model.Brands.Add(new SelectListItem
                {
                    Value = brand.Id.ToString(),
                    Text = brand.BrandName
                });
            }
        }

        public virtual async Task<IActionResult> Edit(int id)
        {

            var category = await _categoryService.GetCategoryByIdAsync(id);

            if (category == null)
                return RedirectToAction("List");

            var model = _mapper.Map<CategoryModel>(category);
            model.BrandName = _brandService.GetBrandByIdAsync(category.Brand_Id).Result?.BrandName;

            var allBrands = await _brandService.GetAllBrandAsync();
            foreach (var brand in allBrands)
            {
                if (brand.Id == category.Brand_Id)
                {
                    model.Brands.Add(new SelectListItem
                    {
                        Value = brand.Id.ToString(),
                        Text = brand.BrandName,
                        Selected = true
                    });
                }
                else
                {
                    model.Brands.Add(new SelectListItem
                    {
                        Value = brand.Id.ToString(),
                        Text = brand.BrandName
                    });
                }
            }
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryModel model, bool continueEditing)
        {
            var validationResult = await _categoryModelValidator.ValidateAsync(model);

            var category = await _categoryService.GetCategoryByIdAsync(model.Id);


            var updatedCategory = _mapper.Map<Category>(model);

            await _categoryService.UpdateCategoryAsync(updatedCategory);
            _notificationService.SuccessNotification("Information has been updated successfully.");
            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = category.Id });
        }


        [HttpPost]
        [Route("Secure/Category/Delete")]
        public virtual async Task<IActionResult> Delete(int Id)
        {

            var category = await _categoryService.GetCategoryByIdAsync(Id);
            if (category != null)
            {
                try
                {
                    await _categoryService.DeleteCategoryAsync(category);
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