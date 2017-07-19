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
using WorldNews.Mappings;
using WorldNews.Models.Category;

namespace WorldNews.Controllers
{
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService service;

        public CategoryController(ICategoryService service)
            : base(service)
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
                return RedirectToAction("List");
            }
            else
            {
                AddModelErrors(serviceMessage.Errors);
                return View(model);
            }
        }

        [ModeratorAuthorize]
        public ActionResult List()
        {
            DataServiceMessage<IEnumerable<CategoryListDTO>> serviceMessage = service.GetAll();
            IEnumerable<CategoryListViewModel> model = serviceMessage.Succeeded 
                ? AutoMapperExtensions.Map<CategoryListDTO, CategoryListViewModel>(serviceMessage.Data) 
                : new List<CategoryListViewModel>();

            return View(model);
        }

        [HttpGet]
        [ModeratorAuthorize]
        public ActionResult Edit(string id)
        {
            id = HttpUtility.UrlDecode(id);

            DataServiceMessage<CategoryEditDTO> serviceMessage = service.Get(id);
            if (serviceMessage.Succeeded)
            {
                CategoryEditViewModel model = Mapper.Map<CategoryEditDTO, CategoryEditViewModel>(serviceMessage.Data);
                return View(model);
            }
            else
            {
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        [ModeratorAuthorize]
        public ActionResult Edit(CategoryEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Id = HttpUtility.UrlDecode(model.Id);
            CategoryEditDTO categoryDTO = Mapper.Map<CategoryEditViewModel, CategoryEditDTO>(model);
            ServiceMessage serviceMessage = service.Edit(categoryDTO);
            if (serviceMessage.Succeeded)
            {
                return RedirectToAction("List");
            }
            else
            {
                AddModelErrors(serviceMessage.Errors);
                return View(model);
            }
        }
    }
}