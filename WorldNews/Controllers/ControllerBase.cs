using System.Collections.Generic;
using System.Web.Mvc;

namespace WorldNews.Controllers
{
    public class ControllerBase : Controller
    {
        protected void AddModelErrors(IEnumerable<string> errors)
        {
            foreach (string error in errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}