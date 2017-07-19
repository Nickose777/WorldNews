using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorldNews.Logic.Contracts.Services;
using WorldNews.Logic.DTO.Comment;
using WorldNews.Logic.Infrastructure;
using WorldNews.Models.Comment;

namespace WorldNews.Controllers
{
    public class CommentController : ControllerBase
    {
        private readonly ICommentService service;

        public CommentController(ICommentService service, ICategoryService categoryService)
            : base(categoryService)
        {
            this.service = service;
        }

        [HttpGet]
        [Authorize]
        public ActionResult Create(string articleId)
        {
            CommentCreateViewModel model = new CommentCreateViewModel { ArticleId = articleId };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(CommentCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string articleId = model.ArticleId;
            model.ArticleId = HttpUtility.UrlDecode(model.ArticleId);
            CommentCreateDTO commentDTO = Mapper.Map<CommentCreateViewModel, CommentCreateDTO>(model);
            ServiceMessage serviceMessage = service.Create(commentDTO);
            if (serviceMessage.Succeeded)
            {
                return RedirectToAction("Details", "Article", new { id = articleId });
            }
            else
            {
                AddModelErrors(serviceMessage.Errors);
                return View(model);
            }
        }
    }
}