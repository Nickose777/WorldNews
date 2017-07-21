using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorldNews.Attributes;
using WorldNews.Logic.Contracts.Services;
using WorldNews.Logic.DTO.Registration;
using WorldNews.Logic.Infrastructure;
using WorldNews.Models;

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
                string message = String.Join(Environment.NewLine, serviceMessage.Errors);
                return Content(message);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult RegisterUser()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("List", "Article");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult RegisterUser(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            UserRegisterDTO userDTO = Mapper.Map<RegisterViewModel, UserRegisterDTO>(model);
            ServiceMessage serviceMessage = service.RegisterUser(userDTO);

            if (serviceMessage.Succeeded)
            {
                return RedirectToAction("Login");
            }
            else
            {
                AddModelErrors(serviceMessage.Errors);
            }

            return View(model);
        }

        [HttpGet]
        [AdminAuthorize]
        public ActionResult RegisterModerator()
        {
            return View();
        }

        [HttpPost]
        [AdminAuthorize]
        public ActionResult RegisterModerator(ModeratorRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string fileName = String.Format("{0}{1}", model.Login, System.IO.Path.GetExtension(model.Photo.FileName));
            string serverPath = Server.MapPath("~/Images/Uploads");
            string path = System.IO.Path.Combine(serverPath, fileName);

            ModeratorRegisterDTO moderatorDTO = Mapper.Map<ModeratorRegisterViewModel, ModeratorRegisterDTO>(model);
            moderatorDTO.PhotoLink = path;

            ServiceMessage serviceMessage = service.RegisterModerator(moderatorDTO);

            if (serviceMessage.Succeeded)
            {
                model.Photo.SaveAs(path);
                return RedirectToAction("Login");
            }
            else
            {
                AddModelErrors(serviceMessage.Errors);
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (!User.Identity.IsAuthenticated)
            {
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
            else
            {
                return RedirectToAction("List", "Article");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ServiceMessage serviceMessage = service.LogIn(model.Login, model.Password);
            if (serviceMessage.Succeeded)
            {
                return RedirectToLocal(returnUrl);
            }
            else
            {
                AddModelErrors(serviceMessage.Errors);
                return View();
            }
        }

        [Authorize]
        public ActionResult LogOff()
        {
            service.LogOff();

            return RedirectToAction("Login");
        }

        [AdminAuthorize]
        public ActionResult Ban(string login)
        {
            if (login != null)
            {
                ServiceMessage serviceMessage = service.BanUser(login);
                if (!serviceMessage.Succeeded)
                {
                    return Content(String.Join(Environment.NewLine, serviceMessage.Errors));
                }
            }

            return RedirectToAction("List", "Moderator");
        }

        [AdminAuthorize]
        public ActionResult Unban(string login)
        {
            if (login != null)
            {
                ServiceMessage serviceMessage = service.UnbanUser(login);
                if (!serviceMessage.Succeeded)
                {
                    return Content(String.Join(Environment.NewLine, serviceMessage.Errors));
                }
            }

            return RedirectToAction("List", "Moderator");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            return Url.IsLocalUrl(returnUrl) ? Redirect(returnUrl) as ActionResult : RedirectToAction("List", "Article");
        }
    }
}