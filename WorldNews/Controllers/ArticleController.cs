﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorldNews.Attributes;
using WorldNews.Logic.Contracts.Services;
using WorldNews.Logic.DTO.Article;
using WorldNews.Logic.Infrastructure;
using WorldNews.Mappings;
using WorldNews.Models.Article;

namespace WorldNews.Controllers
{
    public class ArticleController : ControllerBase
    {
        private const int ItemsPerPage = 6;

        private readonly IArticleService articleService;
        private readonly ICategoryService categoryService;

        public ArticleController(IArticleService articleService, ICategoryService categoryService)
            : base(categoryService)
        {
            this.articleService = articleService;
            this.categoryService = categoryService;
        }

        [HttpGet]
        [ModeratorAuthorize]
        public ActionResult Create()
        {
            DataServiceMessage<IEnumerable<string>> serviceMessage = categoryService.GetAllNames();
            if (serviceMessage.Succeeded)
            {
                var categoryNames = GetAllCategoryNames();
                ArticleCreateViewModel model = new ArticleCreateViewModel
                {
                    Categories = ConvertToSelectListItems(categoryNames)
                };

                return ActionResultDependingOnGetRequest(model);
            }
            else
            {
                return Error(serviceMessage.Errors);
            }
        }

        [HttpPost]
        [ModeratorAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ArticleCreateViewModel model)
        {
            var categoryNames = GetAllCategoryNames();
            model.Categories = ConvertToSelectListItems(categoryNames);
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string fileName = String.Format("{0}_{1}{2}", model.CategoryName, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"), System.IO.Path.GetExtension(model.Photo.FileName));
            string serverPath = Server.MapPath("~/Images/Uploads");
            string path = System.IO.Path.Combine(serverPath, fileName);

            ArticleCreateDTO articleDTO = Mapper.Map<ArticleCreateViewModel, ArticleCreateDTO>(model);
            articleDTO.PhotoLink = path;

            ServiceMessage serviceMessage = articleService.Create(articleDTO);
            if (serviceMessage.Succeeded)
            {
                model.Photo.SaveAs(path);
            }
            else
            {
                AddModelErrors(serviceMessage.Errors);
                return View(model);
            }

            ModelState.Clear();
            return View(new ArticleCreateViewModel
            {
                Categories = ConvertToSelectListItems(categoryNames)
            });
        }

        public ActionResult List(int? pageNumber, string categoryName)
        {
            int currentPage = pageNumber ?? 1;
            DataServiceMessage<IEnumerable<ArticleListDTO>> serviceMessage = GetArticles(currentPage, ItemsPerPage, categoryName);
            if (serviceMessage.Succeeded)
            {
                ArticleOfCategoryListViewModel model = new ArticleOfCategoryListViewModel
                {
                    Articles = AutoMapperExtensions.Map<ArticleListDTO, ArticleListViewModel>(serviceMessage.Data),
                    CategoryName = categoryName,
                    PagesCount = GetPagesCount(ItemsPerPage, categoryName),
                    CurrentPage = currentPage
                };

                return ActionResultDependingOnGetRequest(model);
            }
            else
            {
                return Error(serviceMessage.Errors);
            }
        }

        [AdminAuthorize]
        public ActionResult ListAuthors()
        {
            DataServiceMessage<IEnumerable<ArticleAuthorListDTO>> serviceMessage = articleService.GetAllWithAuthors();
            if (serviceMessage.Succeeded)
            {
                IEnumerable<ArticleAuthorListViewModel> model = AutoMapperExtensions.Map<ArticleAuthorListDTO, ArticleAuthorListViewModel>(serviceMessage.Data);
                return ActionResultDependingOnGetRequest(model);
            }
            else
            {
                return Error(serviceMessage.Errors);
            }
        }

        [HttpGet]
        [ModeratorAuthorize]
        public ActionResult Edit(string id)
        {
            id = HttpUtility.UrlDecode(id);
            DataServiceMessage<ArticleEditDTO> serviceMessage = articleService.Get(id);
            if (serviceMessage.Succeeded)
            {
                ArticleEditViewModel model = Mapper.Map<ArticleEditDTO, ArticleEditViewModel>(serviceMessage.Data);
                var categoryNames = GetAllCategoryNames();
                model.Categories = ConvertToSelectListItems(categoryNames);
                return ActionResultDependingOnGetRequest(model);
            }
            else
            {
                return Error(serviceMessage.Errors);
            }
        }

        [AjaxOnly]
        [HttpPost]
        [ModeratorAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ArticleEditViewModel model)
        {
            var categoryNames = GetAllCategoryNames();
            model.Categories = ConvertToSelectListItems(categoryNames);

            bool success = false;

            if (ModelState.IsValid)
            {
                model.Id = HttpUtility.UrlDecode(model.Id);
                ArticleEditDTO articleDTO = Mapper.Map<ArticleEditViewModel, ArticleEditDTO>(model);
                ServiceMessage serviceMessage = articleService.Edit(articleDTO);
                if (!serviceMessage.Succeeded)
                {
                    AddModelErrors(serviceMessage.Errors);
                }

                success = serviceMessage.Succeeded;
            }

            return JsonOnFormPost(success, "~/Views/Article/Edit.cshtml", model);
        }

        public ActionResult Details(string id)
        {
            id = HttpUtility.UrlDecode(id);
            DataServiceMessage<ArticleDetailsDTO> serviceMessage = articleService.GetDetails(id);
            if (serviceMessage.Succeeded)
            {
                ArticleDetailsViewModel model = Mapper.Map<ArticleDetailsDTO, ArticleDetailsViewModel>(serviceMessage.Data);
                return ActionResultDependingOnGetRequest(model);
            }
            else
            {
                return Error(serviceMessage.Errors);
            }
        }

        [AjaxOnly]
        [HttpPost]
        [ModeratorAuthorize]
        public ActionResult Delete(string id)
        {
            id = HttpUtility.UrlDecode(id);
            ServiceMessage serviceMessage = articleService.Delete(id);
            return JsonOnActionPost(serviceMessage);
        }

        private IEnumerable<string> GetAllCategoryNames()
        {
            DataServiceMessage<IEnumerable<string>> serviceMessage = categoryService.GetAllNames();

            return serviceMessage.Succeeded
                ? serviceMessage.Data
                : new List<string>();
        }

        private IEnumerable<SelectListItem> ConvertToSelectListItems(IEnumerable<string> categoryNames)
        {
            return categoryNames.Select(categoryName =>
                new SelectListItem
                {
                    Value = categoryName,
                    Text = categoryName
                });
        }

        private int GetPagesCount(int itemsPerPage, string categoryName = null)
        {
            return categoryName == null
                ? articleService.GetPagesCount(itemsPerPage).Data
                : articleService.GetPagesCountByCategory(itemsPerPage, categoryName).Data;
        }

        private DataServiceMessage<IEnumerable<ArticleListDTO>> GetArticles(int pageNumber, int itemsPerPage, string categoryName = null)
        {
            return categoryName == null
                ? articleService.GetAllEnabledByPage(pageNumber, itemsPerPage)
                : articleService.GetAllByCategoryByPage(pageNumber, itemsPerPage, categoryName);
        }
    }
}