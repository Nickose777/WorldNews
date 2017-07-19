using AutoMapper;
using System.Collections.Generic;
using System.Web.Mvc;
using WorldNews.Attributes;
using WorldNews.Logic.Contracts.Services;
using WorldNews.Logic.DTO.BanReason;
using WorldNews.Logic.Infrastructure;
using WorldNews.Mappings;
using WorldNews.Models.BanReason;

namespace WorldNews.Controllers
{
    public class BanReasonController : ControllerBase
    {
        private readonly IBanReasonService service;

        public BanReasonController(IBanReasonService service, ICategoryService categoryService)
            : base(categoryService)
        {
            this.service = service;
        }

        [HttpGet]
        [AdminAuthorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AdminAuthorize]
        public ActionResult Create(BanReasonCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            BanReasonCreateDTO banReasonDTO = Mapper.Map<BanReasonCreateViewModel, BanReasonCreateDTO>(model);
            ServiceMessage serviceMessage = service.Create(banReasonDTO);
            if (serviceMessage.Succeeded)
            {
                return RedirectToAction("Create");
            }
            else
            {
                AddModelErrors(serviceMessage.Errors);
                return View(model);
            }
        }

        [AdminAuthorize]
        public ActionResult List()
        {
            IEnumerable<BanReasonListViewModel> model = GetAllBanReasons();

            return View(model);
        }

        private IEnumerable<BanReasonListViewModel> GetAllBanReasons()
        {
            DataServiceMessage<IEnumerable<BanReasonListDTO>> serviceMessage = service.GetAll();

            return serviceMessage.Succeeded
                ? AutoMapperExtensions.Map<BanReasonListDTO, BanReasonListViewModel>(serviceMessage.Data)
                : new List<BanReasonListViewModel>();
        }
    }
}