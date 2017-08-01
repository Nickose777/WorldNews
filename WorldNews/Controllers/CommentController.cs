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
        public ActionResult Ban(string commentId, string articleId)
        {
            bool success = true;
            string html = "";

            if (commentId != null && articleId != null)
            {
                DataServiceMessage<IEnumerable<BanReasonListDTO>> serviceMessage = banReasonService.GetEnabled();

                string partialViewName = serviceMessage.Succeeded
                    ? "Ban"
                    : "~/Views/Shared/Error.cshtml";
                object model = serviceMessage.Succeeded
                    ? new CommentBanViewModel
                        {
                            Id = commentId,
                            ArticleId = articleId,
                            BanReasons = GetSelectListItems(serviceMessage.Data)
                        } as object
                    : serviceMessage.Errors;

                success = serviceMessage.Succeeded;
                html = PartialConverter.PartialViewToString(this, partialViewName, model);
            }
            else
            {
                success = false;
                html = "Comment or article were not identified";
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

            DataServiceMessage<IEnumerable<BanReasonListDTO>> dataServiceMessage = banReasonService.GetEnabled();
            model.BanReasons = GetSelectListItems(dataServiceMessage.Data);

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

        private IEnumerable<SelectListItem> GetSelectListItems(IEnumerable<BanReasonListDTO> banReasons)
        {
            return banReasons.Select(banReason =>
                new SelectListItem
                {
                    Disabled = !banReason.IsEnabled,
                    Text = banReason.Name,
                    Value = banReason.Id
                });
        }
    }
}