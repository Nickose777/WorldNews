using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorldNews.Attributes;
using WorldNews.Helpers;
using WorldNews.Logic.Contracts.Services;
using WorldNews.Logic.DTO.BanReason;
using WorldNews.Logic.DTO.Comment;
using WorldNews.Logic.Infrastructure;
using WorldNews.Models.Comment;

namespace WorldNews.Controllers
{
    public class CommentController : ControllerBase
    {
        private readonly ICommentService commentService;
        private readonly IBanReasonService banReasonService;

        public CommentController(ICommentService commentService, IBanReasonService banReasonService, ICategoryService categoryService)
            : base(categoryService)
        {
            this.commentService = commentService;
            this.banReasonService = banReasonService;
        }

        [HttpGet]
        [Authorize]
        public ActionResult Create(string articleId)
        {
            CommentCreateViewModel model = new CommentCreateViewModel { ArticleId = articleId };
            return View(model);
        }

        [AjaxOnly]
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CommentCreateViewModel model)
        {
            bool succeeded = false;

            if (ModelState.IsValid)
            {
                model.ArticleId = HttpUtility.UrlDecode(model.ArticleId);
                CommentCreateDTO commentDTO = Mapper.Map<CommentCreateViewModel, CommentCreateDTO>(model);
                ServiceMessage serviceMessage = commentService.Create(commentDTO);
                if (!serviceMessage.Succeeded)
                {
                    AddModelErrors(serviceMessage.Errors);
                }

                succeeded = serviceMessage.Succeeded;
            }

            return JsonOnFormPost(succeeded, "~/Views/Comment/Create.cshtml", model);
        }

        [HttpGet]
        [ModeratorAuthorize]
        //TODO
        public ActionResult Ban(string commentId, string articleId)
        {
            bool success = commentId != null;
            string html = "";
            if (!success)
            {
                html = "Id cannot be null";
            }
            else
            {
                CommentBanViewModel model = new CommentBanViewModel
                {
                    Id = commentId,
                    ArticleId = articleId,
                    BanReasons = GetSelectListItems()
                };
                html = RenderHelper.PartialView(this, "Ban", model);
            }

            return Json(new
            {
                success = success,
                html = html
            }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpPost]
        [ModeratorAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Ban(CommentBanViewModel model)
        {
            bool succeeded = false;
            model.BanReasons = GetSelectListItems();

            if (ModelState.IsValid)
            {
                model.Id = HttpUtility.UrlDecode(model.Id);
                CommentBanDTO commentDTO = Mapper.Map<CommentBanViewModel, CommentBanDTO>(model);
                ServiceMessage serviceMessage = commentService.Ban(commentDTO);
                if (!serviceMessage.Succeeded)
                {
                    AddModelErrors(serviceMessage.Errors);
                }

                succeeded = serviceMessage.Succeeded;
            }

            return JsonOnFormPost(succeeded, "~/Views/Comment/Ban.cshtml", model);
        }

        private IEnumerable<SelectListItem> GetSelectListItems()
        {
            var banReasons = GetEnablesBanReasons();

            return banReasons.Select(banReason =>
                new SelectListItem
                {
                    Disabled = !banReason.IsEnabled,
                    Text = banReason.Name,
                    Value = banReason.Id
                });
        }

        private IEnumerable<BanReasonListDTO> GetEnablesBanReasons()
        {
            DataServiceMessage<IEnumerable<BanReasonListDTO>> serviceMessage = banReasonService.GetEnabled();
            return serviceMessage.Succeeded
                ? serviceMessage.Data 
                : new List<BanReasonListDTO>();
        }
    }
}