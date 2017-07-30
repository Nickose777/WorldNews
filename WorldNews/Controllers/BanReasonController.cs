using AutoMapper;
using System.Collections.Generic;
using System.Web;
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
            return ActionResultDependingOnGetRequest();
        }

        [HttpPost]
        [AjaxOnly]
        [AdminAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BanReasonCreateViewModel model)
        {
            bool succeeded = false;

            if (ModelState.IsValid)
            {
                BanReasonCreateDTO banReasonDTO = Mapper.Map<BanReasonCreateViewModel, BanReasonCreateDTO>(model);
                ServiceMessage serviceMessage = service.Create(banReasonDTO);
                if (!serviceMessage.Succeeded)
                {
                    AddModelErrors(serviceMessage.Errors);
                }

                succeeded = serviceMessage.Succeeded;
            }

            return JsonOnFormPost(succeeded, "~/Views/BanReason/Create.cshtml", model);
        }

        [AdminAuthorize]
        public ActionResult List()
        {
            IEnumerable<BanReasonListViewModel> model = GetBanReasons(false);
            return ActionResultDependingOnGetRequest(model);
        }

        [HttpGet]
        [AdminAuthorize]
        public ActionResult Edit(string id)
        {
            id = HttpUtility.UrlDecode(id);
            DataServiceMessage<BanReasonEditDTO> serviceMessage = service.Get(id);
            if (serviceMessage.Succeeded)
            {
                BanReasonEditViewModel model = Mapper.Map<BanReasonEditDTO, BanReasonEditViewModel>(serviceMessage.Data);
                return ActionResultDependingOnGetRequest(model);
            }
            else
            {
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        [AjaxOnly]
        [AdminAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BanReasonEditViewModel model)
        {
            bool succeeded = false;

            if (ModelState.IsValid)
            {
                model.Id = HttpUtility.UrlDecode(model.Id);
                BanReasonEditDTO banReasonDTO = Mapper.Map<BanReasonEditViewModel, BanReasonEditDTO>(model);
                ServiceMessage serviceMessage = service.Edit(banReasonDTO);
                if (!serviceMessage.Succeeded)
                {
                    AddModelErrors(serviceMessage.Errors);
                }

                succeeded = serviceMessage.Succeeded;
            }

            return JsonOnFormPost(succeeded, "~/Views/BanReason/Edit.cshtml", model);
        }

        private IEnumerable<BanReasonListViewModel> GetBanReasons(bool enabledOnly)
        {
            DataServiceMessage<IEnumerable<BanReasonListDTO>> serviceMessage = enabledOnly
                ? service.GetEnabled()
                : service.GetAll();

            return serviceMessage.Succeeded
                ? AutoMapperExtensions.Map<BanReasonListDTO, BanReasonListViewModel>(serviceMessage.Data)
                : new List<BanReasonListViewModel>();
        }
    }
}