using AutoMapper;
using System.Web.Mvc;
using WorldNews.Attributes;
using WorldNews.Logic.Contracts.Services;
using WorldNews.Logic.DTO.Profile;
using WorldNews.Logic.Infrastructure;
using WorldNews.Models;

namespace WorldNews.Controllers
{
    [AdminAuthorize]
    public class AdminController : ControllerBase
    {
        private readonly IProfileService service;

        public AdminController(IProfileService service, ICategoryService categoryService)
            : base(categoryService)
        {
            this.service = service;
        }

        public ActionResult Index()
        {
            return RedirectToAction("Edit");
        }
        
        [HttpGet]
        public ActionResult Edit()
        {
            DataServiceMessage<ProfileBaseDTO> serviceMessage = service.GetAdminProfile(User.Identity.Name);
            if (serviceMessage.Succeeded)
            {
                ProfileViewModel model = Mapper.Map<ProfileBaseDTO, ProfileViewModel>(serviceMessage.Data);
                return View(model);
            }
            else
            {
                AddModelErrors(serviceMessage.Errors);
                return View();
            }
        }

        [HttpPost]
        public ActionResult Edit(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ProfileBaseDTO profileDTO = Mapper.Map<ProfileViewModel, ProfileBaseDTO>(model);
            ServiceMessage serviceMessage = service.UpdateAdminProfile(profileDTO);

            if (serviceMessage.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                AddModelErrors(serviceMessage.Errors);
                return View();
            }
        }
    }
}