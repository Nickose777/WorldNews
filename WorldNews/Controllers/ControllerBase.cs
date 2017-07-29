using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WorldNews.Helpers;
using WorldNews.Logic.Contracts.Services;
using WorldNews.Logic.Infrastructure;

namespace WorldNews.Controllers
{
    public abstract class ControllerBase : Controller
    {
        private readonly ICategoryService service;

        public ControllerBase(ICategoryService service)
        {
            this.service = service;
        }

        protected void AddModelErrors(IEnumerable<string> errors)
        {
            foreach (string error in errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.Categories = GetEnabledCategoryNames();

            base.OnActionExecuting(filterContext);
        }

        protected IEnumerable<string> GetEnabledCategoryNames()
        {
            DataServiceMessage<IEnumerable<string>> serviceMessage = service.GetEnabledNames();

            return serviceMessage.Succeeded ?
                serviceMessage.Data :
                new List<string>();
        }

        protected ActionResult RedirectToLocal(string returnUrl)
        {
            return Url.IsLocalUrl(returnUrl) ? Redirect(returnUrl) as ActionResult : RedirectToAction("List", "Article");
        }

        protected ActionResult ActionResultDependingOnGetRequest()
        {
            return Request.IsAjaxRequest()
                ? PartialView() as ActionResult
                : View();
        }

        protected ActionResult ActionResultDependingOnGetRequest(object model)
        {
            return Request.IsAjaxRequest()
                ? PartialView(model) as ActionResult
                : View(model);
        }

        protected ActionResult ActionResultDependingOnAjaxPostRequest(bool success, string partialViewName)
        {
            return Request.IsJsonRequest()
                    ? JsonOnPost(success, partialViewName)
                    : PartialView(partialViewName);
        }

        protected ActionResult ActionResultDependingOnAjaxPostRequest(bool success, string partialViewName, object model)
        {
            return Request.IsAjaxRequest()
                ? (Request.IsJsonRequest()
                    ? JsonOnPost(success, partialViewName, model)
                    : PartialView(partialViewName, model)) as ActionResult
                : View(model);
        }

        private ActionResult JsonOnPost(bool success, string partialViewName)
        {
            return Json(new
            {
                success = success,
                html = partialViewName != null
                    ? RenderHelper.PartialView(this, partialViewName)
                    : String.Empty
            });
        }

        private ActionResult JsonOnPost(bool success, string partialViewName, object model)
        {
            return Json(new
            {
                success = success,
                html = partialViewName != null
                    ? RenderHelper.PartialView(this, partialViewName, model)
                    : String.Empty
            });
        }
    }
}