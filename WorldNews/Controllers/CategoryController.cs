using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorldNews.Attributes;
using WorldNews.Logic.Contracts.Services;
using WorldNews.Logic.DTO.Category;
using WorldNews.Logic.Infrastructure;
using WorldNews.Models.Category;

namespace WorldNews.Controllers
{
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService service;

        public CategoryController(ICategoryService service)
        {
            this.service = service;
        }

        [HttpGet]
        [ModeratorAuthorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ModeratorAuthorize]
        public ActionResult Create(CategoryCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            CategoryCreateDTO categoryDTO = Mapper.Map<CategoryCreateViewModel, CategoryCreateDTO>(model);
            ServiceMessage serviceMessage = service.Create(categoryDTO);

            if (serviceMessage.Succeeded)
            {
                return Content("Created!");
            }
            else
            {
                AddModelErrors(serviceMessage.Errors);
                return View(model);
            }

            return View();
        }
    }
}