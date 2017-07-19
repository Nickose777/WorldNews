using AutoMapper;
using System.Web.Mvc;
using WorldNews.Attributes;
using WorldNews.Logic.Contracts.Services;
using WorldNews.Logic.DTO.ReasonOfBan;
using WorldNews.Logic.Infrastructure;
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
    }
}