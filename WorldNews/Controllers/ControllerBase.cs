using System.Collections.Generic;
using System.Web.Mvc;
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
            ViewBag.Categories = GetAllCategories();

            base.OnActionExecuting(filterContext);
        }

        protected IEnumerable<string> GetAllCategories()
        {
            DataServiceMessage<IEnumerable<string>> serviceMessage = service.GetAllNames();

            return serviceMessage.Succeeded ?
                serviceMessage.Data :
                new List<string>();
        }
    }
}