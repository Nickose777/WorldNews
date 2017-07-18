using System.Collections.Generic;
using System.Web.Mvc;
using WorldNews.Attributes;
using WorldNews.Logic.Contracts.Services;
using WorldNews.Logic.DTO.Profile;
using WorldNews.Logic.Infrastructure;
using WorldNews.Mappings;
using WorldNews.Models.Profile;

namespace WorldNews.Controllers
{
    public class ModeratorController : ControllerBase
    {
        private readonly IModeratorService service;

        public ModeratorController(IModeratorService service, ICategoryService categoryService)
            : base(categoryService)
        {
            this.service = service;
        }

        [AdminAuthorize]
        public ActionResult List()
        {
            DataServiceMessage<IEnumerable<ModeratorListDTO>> serviceMessage = service.GetAll();
            if (serviceMessage.Succeeded)
            {
                IEnumerable<ModeratorListViewModel> model = AutoMapperExtensions.Map<ModeratorListDTO, ModeratorListViewModel>(serviceMessage.Data);
                return View(model);
            }
            else
            {
                AddModelErrors(serviceMessage.Errors);
            }

            return View();
        }
    }
}