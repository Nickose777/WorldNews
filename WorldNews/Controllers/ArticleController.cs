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

                return View(model);
            }
            else
            {
                return Content(String.Join(Environment.NewLine, serviceMessage.Errors));
            }
        }

        [HttpPost]
        [ModeratorAuthorize]
        public ActionResult Create(ArticleCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var categoryNames = GetAllCategoryNames();
                model.Categories = ConvertToSelectListItems(categoryNames);
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
                return RedirectToAction("List");
            }
            else
            {
                AddModelErrors(serviceMessage.Errors);
                return View(model);
            }
        }

        public ActionResult List()
        {
            var model = GetArticles();
            return View(model);
        }

        public ActionResult ListByCategory(string categoryName)
        {
            var model = GetArticles(categoryName);
            return View("List", model);
        }

        public ActionResult Details(string id)
        {
            id = HttpUtility.UrlDecode(id);
            DataServiceMessage<ArticleDetailsDTO> serviceMessage = articleService.Get(id);
            if (serviceMessage.Succeeded)
            {
                ArticleDetailsViewModel model = Mapper.Map<ArticleDetailsDTO, ArticleDetailsViewModel>(serviceMessage.Data);
                return View(model);
            }
            else
            {
                return HttpNotFound(String.Join(Environment.NewLine, serviceMessage.Errors));
            }
        }

        private IEnumerable<string> GetAllCategoryNames()
        {
            IEnumerable<string> categoryNames;

            DataServiceMessage<IEnumerable<string>> serviceMessage = categoryService.GetAllNames();
            if (serviceMessage.Succeeded)
            {
                categoryNames = serviceMessage.Data;
            }
            else
            {
                categoryNames = new List<string>();
            }

            return categoryNames;
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

        private IEnumerable<ArticleListViewModel> GetArticles(string categoryName = null)
        {
            IEnumerable<ArticleListViewModel> articles;

            DataServiceMessage<IEnumerable<ArticleListDTO>> serviceMessage = categoryName == null
                ? articleService.GetAllEnabled()
                : articleService.GetAllByCategory(categoryName);
            if (serviceMessage.Succeeded)
            {
                articles = AutoMapperExtensions.Map<ArticleListDTO, ArticleListViewModel>(serviceMessage.Data);
            }
            else
            {
                articles = new List<ArticleListViewModel>();
            }

            return articles;
        }
    }
}