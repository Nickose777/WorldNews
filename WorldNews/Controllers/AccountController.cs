using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorldNews.Logic.Contracts;
using WorldNews.Logic.DTO.Registration;
using WorldNews.Logic.Infrastructure;
using WorldNews.Models;

namespace WorldNews.Controllers
{
    [Authorize]
    public class AccountController : Controller
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
                return RedirectToAction("Register");
            }
            else
            {
                string message = String.Join(Environment.NewLine, serviceMessage.Errors);
                return Content(message);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return Content("You are logged in!");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(RegisterViewModel model)
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
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return Content("You are logged in!");
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
                return Content("Logged in!");
            }
            else
            {
                AddModelErrors(serviceMessage.Errors);
            }

            return View();
        }

        private void AddModelErrors(IEnumerable<string> errors)
        {
            foreach (string error in errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}