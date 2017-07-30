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

        [HttpGet]
        public ActionResult Edit()
        {
            DataServiceMessage<ProfileBaseDTO> serviceMessage = service.GetAdminProfile(User.Identity.Name);
            if (serviceMessage.Succeeded)
            {
                ProfileViewModel model = Mapper.Map<ProfileBaseDTO, ProfileViewModel>(serviceMessage.Data);
                return ActionResultDependingOnGetRequest(model);
            }
            else
            {
                AddModelErrors(serviceMessage.Errors);
                return ActionResultDependingOnGetRequest();
            }
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProfileViewModel model)
        {
            bool success = false;

            if (ModelState.IsValid)
            {
                ProfileBaseDTO profileDTO = Mapper.Map<ProfileViewModel, ProfileBaseDTO>(model);
                ServiceMessage serviceMessage = service.UpdateAdminProfile(profileDTO);

                if (!serviceMessage.Succeeded)
                {
                    AddModelErrors(serviceMessage.Errors);
                }

                success = serviceMessage.Succeeded;
            }

            return JsonOnFormPost(success, "~/Views/Admin/Edit.cshtml", model);
        }
    }
}