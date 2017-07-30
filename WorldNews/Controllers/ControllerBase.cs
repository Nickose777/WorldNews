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

        protected ActionResult JsonOnFormPost(bool success, string partialViewName, object model)
        {
            return Json(new
            {
                success = success,
                html = RenderHelper.PartialView(this, partialViewName, model)
            });
        }

        protected ActionResult JsonOnActionPost(ServiceMessage serviceMessage)
        {
            return JsonOnActionPost(serviceMessage.Succeeded, serviceMessage.Errors);
        }

        protected ActionResult JsonOnActionPost(bool success, IEnumerable<string> errors)
        {
            return Json(new
            {
                success = success,
                errors = errors
            });
        }
    }
}