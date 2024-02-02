using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Base.Core;
using Base.Core.Domain.Users;
using Base.Services.Notification;
using Base.Services.Users;
using Base.Web.Areas.Admin.Models.Users;
using Base.Web.Areas.Secure.Models.Users;
using Base.Web.Framework.Controllers;
using Base.Web.Framework.Models.Extensions;
using Base.Web.Framework.MVC.Filters;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;


namespace Base.Web.Areas.Secure.Controllers
{
    public partial class UsersController : BaseAdminController
    {
        #region Fields

        private readonly IMapper _mapper;
        private readonly IWorkContext _workContext;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        private readonly IValidator<UserModel> _userModelValidator;

        #endregion

        #region Ctor
        public UsersController(
            IWorkContext workContext, 
            IUserService userService, 
            INotificationService notificationService,
            IMapper mapper,
            IValidator<UserModel> userModelValidator)
        {
            _workContext = workContext;
            _userService = userService;
            _notificationService = notificationService;
            _mapper = mapper;
            _userModelValidator = userModelValidator;
        }
        #endregion

        #region Methods

        public virtual IActionResult List()
        {
            var model = new UserSearchModel();
            model.AvailableUserRole.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = "Admin",
                Value = "1"
            });
            model.AvailableUserRole.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = "CallCenter",
                Value = "2"
            });
            return View(model);
        }
        [HttpPost]
        public virtual async Task<IActionResult> List(UserSearchModel searchModel)
        {
            var allUsers = await _userService.GetAllUsersAsync(phone:searchModel.SearchUserPhone);

            Func<IEnumerable<UserModel>> dataFillFunction = () =>
            {
                return allUsers.Select(u =>
                {
                    var userModel = _mapper.Map<UserModel>(u);
                    return userModel;
                });
            };
            var model = new UserListModel().PrepareToGrid(searchModel, allUsers, dataFillFunction);

            return Json(model);
        }


        public IActionResult Create()
        {
            var model = new UserModel();
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserModel model, bool continueEditing)
        {
            var validationResult = await _userModelValidator.ValidateAsync(model);
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(model);

                await _userService.InsertUserAsync(user);
                _notificationService.SuccessNotification("Information has been saved successfully.");
                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Create");

            }
            return View(model);
        }

        public virtual async Task<IActionResult> Edit(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            var userModel = _mapper.Map<UserModel>(user);

            return View(userModel);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Edit(UserModel userModel, bool continueEditing)
        {
            var validationResult = await _userModelValidator.ValidateAsync(userModel);
            if (ModelState.IsValid)
            {
                var updateduser = _mapper.Map<User>(userModel);

                await _userService.UpdateUserAsync(updateduser);
                _notificationService.SuccessNotification("Information has been updated successfully.");
                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = userModel.Id });
            }
            return View(userModel);
        }

        [HttpPost]
        [Route("Secure/Users/Delete")]
        public async Task<IActionResult> Delete(int Id)
        {
            var user = await _userService.GetUserByIdAsync(Id);
            if (user != null)
            {
                try
                {
                    await _userService.HardDeleteUser(user);
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