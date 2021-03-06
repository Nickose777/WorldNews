﻿using AutoMapper;
using System.Collections.Generic;
using System.Web.Mvc;
using WorldNews.Attributes;
using WorldNews.Logic.Contracts.Services;
using WorldNews.Logic.DTO.Profile;
using WorldNews.Logic.Infrastructure;
using WorldNews.Mappings;
using WorldNews.Models.Profile;

namespace WorldNews.Controllers
{
    [AdminAuthorize]
    public class ModeratorController : ControllerBase
    {
        private readonly IModeratorService service;

        public ModeratorController(IModeratorService service, ICategoryService categoryService)
            : base(categoryService)
        {
            this.service = service;
        }

        public ActionResult List()
        {
            DataServiceMessage<IEnumerable<ModeratorListDTO>> serviceMessage = service.GetAll();
            if (serviceMessage.Succeeded)
            {
                IEnumerable<ModeratorListViewModel> model = AutoMapperExtensions.Map<ModeratorListDTO, ModeratorListViewModel>(serviceMessage.Data);
                return ActionResultDependingOnGetRequest(model);
            }
            else
            {
                return Error(serviceMessage.Errors);
            }
        }

        [HttpGet]
        public ActionResult Edit(string login)
        {
            DataServiceMessage<ModeratorEditDTO> serviceMessage = service.Get(login);
            if (serviceMessage.Succeeded)
            {
                ModeratorEditViewModel model = Mapper.Map<ModeratorEditDTO, ModeratorEditViewModel>(serviceMessage.Data);
                return ActionResultDependingOnGetRequest(model);
            }
            else
            {
                return Error(serviceMessage.Errors);
            }
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ModeratorEditViewModel model)
        {
            bool succeeded = false;

            if (ModelState.IsValid)
            {
                ModeratorEditDTO moderatorDTO = Mapper.Map<ModeratorEditViewModel, ModeratorEditDTO>(model);
                ServiceMessage serviceMessage = service.Edit(moderatorDTO);
                if (!serviceMessage.Succeeded)
                {
                    AddModelErrors(serviceMessage.Errors);
                }

                succeeded = serviceMessage.Succeeded;
            }

            return JsonOnFormPost(succeeded, "~/Views/Moderator/Edit.cshtml", model);
        }

        public ActionResult Details(string login)
        {
            DataServiceMessage<ModeratorDetailsDTO> serviceMessage = service.GetDetails(login);
            if (serviceMessage.Succeeded)
            {
                ModeratorDetailsViewModel model = Mapper.Map<ModeratorDetailsDTO, ModeratorDetailsViewModel>(serviceMessage.Data);
                return ActionResultDependingOnGetRequest(model);
            }
            else
            {
                return Error(serviceMessage.Errors);
            }
        }
    }
}