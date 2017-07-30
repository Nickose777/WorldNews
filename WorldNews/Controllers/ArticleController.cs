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
                //Error page
                return Content(String.Join(Environment.NewLine, serviceMessage.Errors));
            }
        }

        [AjaxOnly]
        [HttpPost]
        [ModeratorAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ArticleCreateViewModel model)
        {
            bool success = false;

            if (ModelState.IsValid)
            {
                string fileName = String.Format("{0}_{1}{2}", model.CategoryName, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"), System.IO.Path.GetExtension(model.Photo.FileName));
                string serverPath = Server.MapPath("~/Images/Uploads");
                string path = System.IO.Path.Combine(serverPath, fileName);

                ArticleCreateDTO articleDTO = Mapper.Map<ArticleCreateViewModel, ArticleCreateDTO>(model);
                articleDTO.PhotoLink = path;

                ServiceMessage serviceMessage = articleService.Create(articleDTO);
                if (!serviceMessage.Succeeded)
                {
                    AddModelErrors(serviceMessage.Errors);
                }

                success = serviceMessage.Succeeded;
            }
            else
            {
                var categoryNames = GetAllCategoryNames();
                model.Categories = ConvertToSelectListItems(categoryNames);
            }

            return JsonOnFormPost(success, "~/Views/Article/Create.cshtml", model);
        }

        public ActionResult List(int? pageNumber, string categoryName)
        {
            int currentPage = pageNumber ?? 1;
            IEnumerable<ArticleListViewModel> articles = GetArticles(currentPage, ItemsPerPage, categoryName);
            ArticleOfCategoryListViewModel model = new ArticleOfCategoryListViewModel
            {
                Articles = articles,
                CategoryName = categoryName,
                PagesCount = GetPagesCount(ItemsPerPage, categoryName),
                CurrentPage = currentPage
            };

            return ActionResultDependingOnGetRequest(model);
        }

        [AdminAuthorize]
        public ActionResult ListAuthors()
        {
            IEnumerable<ArticleAuthorListViewModel> model = GetArticlesWithAuthors();
            return ActionResultDependingOnGetRequest(model);
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
                return HttpNotFound(String.Join(Environment.NewLine, serviceMessage.Errors));
            }
        }

        [AjaxOnly]
        [HttpPost]
        [ModeratorAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ArticleEditViewModel model)
        {
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
            else
            {
                var categoryNames = GetAllCategoryNames();
                model.Categories = ConvertToSelectListItems(categoryNames);
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
                return HttpNotFound(String.Join(Environment.NewLine, serviceMessage.Errors));
            }
        }

        [AjaxOnly]
        [HttpPost]
        [ModeratorAuthorize]
        public ActionResult Delete(string id)
        {
            id = HttpUtility.UrlDecode(id);
            ServiceMessage serviceMessage = articleService.Delete(id);
            if (!serviceMessage.Succeeded)
            {
                AddModelErrors(serviceMessage.Errors);
            }

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

        private IEnumerable<ArticleListViewModel> GetArticles(int pageNumber, int itemsPerPage, string categoryName = null)
        {
            DataServiceMessage<IEnumerable<ArticleListDTO>> serviceMessage = categoryName == null
                ? articleService.GetAllEnabledByPage(pageNumber, itemsPerPage)
                : articleService.GetAllByCategoryByPage(pageNumber, itemsPerPage, categoryName);

            return serviceMessage.Succeeded
                ? AutoMapperExtensions.Map<ArticleListDTO, ArticleListViewModel>(serviceMessage.Data)
                : new List<ArticleListViewModel>();
        }

        private IEnumerable<ArticleAuthorListViewModel> GetArticlesWithAuthors()
        {
            DataServiceMessage<IEnumerable<ArticleAuthorListDTO>> serviceMessage = articleService.GetAllWithAuthors();

            return serviceMessage.Succeeded
                ? AutoMapperExtensions.Map<ArticleAuthorListDTO, ArticleAuthorListViewModel>(serviceMessage.Data)
                : new List<ArticleAuthorListViewModel>();
        }
    }
}