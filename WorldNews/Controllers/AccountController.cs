using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorldNews.Attributes;
using WorldNews.Logic.Contracts;
using WorldNews.Logic.DTO.Registration;
using WorldNews.Logic.Infrastructure;
using WorldNews.Models;

namespace WorldNews.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly IAccountService service;

        public AccountController(IAccountService service)
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
                return RedirectToAction("Index", "Home");
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

            string fileName = System.IO.Path.GetFileName(model.Photo.FileName);
            string path = System.IO.Path.Combine(Server.MapPath("~/Images/Uploads"), fileName);
            model.Photo.SaveAs(path);

            ModeratorRegisterDTO moderatorDTO = Mapper.Map<ModeratorRegisterViewModel, ModeratorRegisterDTO>(model);
            moderatorDTO.PhotoLink = path;

            ServiceMessage serviceMessage = service.RegisterModerator(moderatorDTO);

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
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ServiceMessage serviceMessage = service.LogIn(model.Login, model.Password);
            if (serviceMessage.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                AddModelErrors(serviceMessage.Errors);
            }

            return View();
        }

        [Authorize]
        public ActionResult LogOff()
        {
            service.LogOff();

            return RedirectToAction("Login");
        }
    }
}