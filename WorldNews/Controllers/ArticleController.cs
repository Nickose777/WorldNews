using AutoMapper;
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
                ArticleCreateViewModel model = new ArticleCreateViewModel
                {
                    Categories = GetAllCategories()
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
                model.Categories = GetAllCategories();
                return View(model);
            }

            string fileName = System.IO.Path.GetFileName(model.Photo.FileName);
            string serverPath = Server.MapPath("~/Images/Uploads");
            string path = System.IO.Path.Combine(serverPath, fileName);
            model.Photo.SaveAs(path);

            ArticleCreateDTO articleDTO = Mapper.Map<ArticleCreateViewModel, ArticleCreateDTO>(model);
            articleDTO.PhotoLink = path;

            ServiceMessage serviceMessage = articleService.Create(articleDTO);
            if (serviceMessage.Succeeded)
            {
                return Content("Article created!");
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

        public ActionResult Details(string id)
        {
            return View();
        }

        private IEnumerable<SelectListItem> GetAllCategories()
        {
            IEnumerable<SelectListItem> categories;
            DataServiceMessage<IEnumerable<string>> serviceMessage = categoryService.GetAllNames();
            if (serviceMessage.Succeeded)
            {
                categories = serviceMessage.Data.Select(category =>
                    new SelectListItem
                    {
                        Value = category,
                        Text = category
                    });
            }
            else
            {
                categories = new List<SelectListItem>();
            }

            return categories;
        }

        private IEnumerable<ArticleListViewModel> GetArticles()
        {
            IEnumerable<ArticleListViewModel> articles;

            DataServiceMessage<IEnumerable<ArticleListDTO>> serviceMessage = articleService.GetAll();
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