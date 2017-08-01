using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorldNews.Attributes;
using WorldNews.Helpers;
using WorldNews.Logic.Contracts.Services;
using WorldNews.Logic.DTO.Account;
using WorldNews.Logic.DTO.Registration;
using WorldNews.Logic.Infrastructure;
using WorldNews.Models;
using WorldNews.Models.Account;

namespace WorldNews.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly IAccountService service;

        public AccountController(IAccountService service, ICategoryService categoryService)
            : base(categoryService)
        {
            this.service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            ServiceMessage serviceMessage = service.InitializeRoles();
            if (serviceMessage.Succeeded)
            {
                return RedirectToAction("RegisterUser");
            }
            else
            {
                return Error(serviceMessage.Errors);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult RegisterUser()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return ActionResultDependingOnGetRequest();
            }
            else
            {
                return RedirectToAction("List", "Article");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult RegisterUserPartial()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return ActionResultDependingOnGetRequest();
            }
            else
            {
                return RedirectToAction("List", "Article");
            }
        }

        [HttpPost]
        [AjaxOnly]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterUser(RegisterViewModel model)
        {
            bool succeeded = false;

            if (ModelState.IsValid)
            {
                UserRegisterDTO userDTO = Mapper.Map<RegisterViewModel, UserRegisterDTO>(model);
                ServiceMessage serviceMessage = service.RegisterUser(userDTO);
                if (!serviceMessage.Succeeded)
                {
                    AddModelErrors(serviceMessage.Errors);
                }

                succeeded = serviceMessage.Succeeded;
            }

            return JsonOnFormPost(succeeded, "~/Views/Account/RegisterUser.cshtml", model);
        }

        [HttpGet]
        [AdminAuthorize]
        public ActionResult RegisterModerator()
        {
            return ActionResultDependingOnGetRequest();
        }

        [HttpPost]
        [AdminAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterModerator(ModeratorRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                string fileName = String.Format("{0}{1}", model.Login, System.IO.Path.GetExtension(model.Photo.FileName));
                string serverPath = Server.MapPath("~/Images/Uploads");
                string path = System.IO.Path.Combine(serverPath, fileName);

                ModeratorRegisterDTO moderatorDTO = Mapper.Map<ModeratorRegisterViewModel, ModeratorRegisterDTO>(model);
                moderatorDTO.PhotoLink = path;

                ServiceMessage serviceMessage = service.RegisterModerator(moderatorDTO);
                if (serviceMessage.Succeeded)
                {
                    model.Photo.SaveAs(path);
                    return View();
                }
                else
                {
                    AddModelErrors(serviceMessage.Errors);
                }
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (!User.Identity.IsAuthenticated)
            {
                if (returnUrl != null)
                {
                    ViewBag.ReturnUrl = returnUrl;
                }
                return ActionResultDependingOnGetRequest();
            }
            else
            {
                return RedirectToAction("List", "Article");
            }
        }

        [AjaxOnly]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            bool succeeded = false;

            if (ModelState.IsValid)
            {
                ServiceMessage serviceMessage = service.LogIn(model.Login, model.Password);
                if (!serviceMessage.Succeeded)
                {
                    AddModelErrors(serviceMessage.Errors);
                }

                succeeded = serviceMessage.Succeeded;
            }

            return JsonOnFormPost(succeeded, "~/Views/Account/Login.cshtml", model);
        }

        [Authorize]
        public ActionResult LogOff()
        {
            service.LogOff();
            return RedirectToAction("List", "Article");
        }

        [HttpPost]
        [AjaxOnly]
        [AdminAuthorize]
        public ActionResult Ban(string login)
        {
            ServiceMessage serviceMessage = service.BanUser(login);
            return JsonOnActionPost(serviceMessage);
        }

        [HttpPost]
        [AjaxOnly]
        [AdminAuthorize]
        public ActionResult Unban(string login)
        {
            ServiceMessage serviceMessage = service.UnbanUser(login);
            return JsonOnActionPost(serviceMessage);
        }

        [HttpGet]
        [Authorize]
        public ActionResult ChangePassword()
        {
            return ActionResultDependingOnGetRequest();
        }

        [HttpPost]
        [AjaxOnly]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            bool succeeded = false;

            if (ModelState.IsValid)
            {
                ChangePasswordDTO changePasswordDTO = Mapper.Map<ChangePasswordViewModel, ChangePasswordDTO>(model);
                ServiceMessage serviceMessage = service.ChangePassword(changePasswordDTO);
                if (!serviceMessage.Succeeded)
                {
                    AddModelErrors(serviceMessage.Errors);
                }

                succeeded = serviceMessage.Succeeded;
            }

            return JsonOnFormPost(succeeded, "~/Views/Account/ChangePassword.cshtml", model);
        }
    }
}