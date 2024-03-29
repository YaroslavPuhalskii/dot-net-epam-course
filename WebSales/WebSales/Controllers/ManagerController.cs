﻿using AutoMapper;
using NLog;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebSales.DAL;
using WebSales.DAL.Abstractions;
using WebSales.DAL.Filters;
using WebSales.DAL.Models;
using WebSales.Models.Manager;

namespace WebSales.Controllers
{
    public class ManagerController : Controller
    {
        private readonly IMapper _mapper;

        private readonly IUnitOfWork unitOfWork = new UnitOfWork();

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private const int _pageSize = 8;

        public ManagerController()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Manager, ManagerIndexView>();
                cfg.CreateMap<ManagerCreateView, Manager>();
                cfg.CreateMap<Manager, ManagerEditView>();
                cfg.CreateMap<ManagerEditView, Manager>();
                cfg.CreateMap<ManagerFilter, ManagerFilterModel>();
            });

            _mapper = new Mapper(config);
        }

        public ViewResult Index()
        {
            return View();
        }

        public PartialViewResult Load(ManagerFilter managerFilter, int? page)
        {
            var filter = _mapper.Map<ManagerFilter, ManagerFilterModel>(managerFilter);

            var managers = unitOfWork.GetManagerRepo.GetManagersByFilter(filter);

            var managerView = _mapper.Map<IEnumerable<Manager>, List<ManagerIndexView>>(managers);

            ViewBag.Filter = new ManagerFilter();

            int pageNumber = (page ?? 1);
            return PartialView(managerView.ToPagedList(pageNumber, _pageSize));
        }

        [HttpGet]
        public PartialViewResult Create()
        {
            return PartialView();
        }

        [Authorize(Roles = "Admin")]
        public async Task<JsonResult> Create(ManagerCreateView model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var manager = _mapper.Map<ManagerCreateView, Manager>(model);

                    unitOfWork.GetManagerRepo.Insert(manager);
                    await unitOfWork.Save();

                    return Json(new { result = true });
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, $"{DateTime.Now.ToLongTimeString()} : Server error, when try to add a manager!");
                    return Json(new { result = false, message = "Server error, when try to add a manager!" });
                }
            }

            _logger.Error($"{DateTime.Now.ToLongTimeString()} : Invalid model {nameof(model)}! {ModelState.Select(x => x.Value.Errors).First().First().ErrorMessage}");
            return Json(new { result = false, message = "Invalid model!" });
        }

        [Authorize(Roles = "Admin")]
        public async Task<PartialViewResult> Edit(int id)
        {
            if (id > 0)
            {
                try
                {
                    var manager = _mapper.Map<Manager, ManagerEditView>(await unitOfWork.GetManagerRepo.GetById(id));

                    return PartialView(manager);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, $"{DateTime.Now.ToLongTimeString()} : Server error, when trying to get manager by Id : {id}!");
                    return PartialView("~/Views/Shared/Error.cshtml");
                }
            }

            _logger.Error($"{DateTime.Now.ToLongTimeString()} : Id less 1!");
            return PartialView("~/Views/Shared/Error.cshtml");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<JsonResult> Edit(ManagerEditView model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var manager = _mapper.Map<ManagerEditView, Manager>(model);

                    unitOfWork.GetManagerRepo.Update(manager);
                    await unitOfWork.Save();

                    return Json(new { result = true });
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, $"{DateTime.Now.ToLongTimeString()} : Server error, when trying to edit a manager!");
                    return Json(new { result = false, message = ex.Message });
                }
            }

            _logger.Error($"{DateTime.Now.ToLongDateString()} : Invalid model {nameof(model)}! {ModelState.Select(x => x.Value.Errors).First().First().ErrorMessage}");
            return Json(new { result = false, message = "Model is invalid" });
        }

        [Authorize(Roles = "Admin")]
        public async Task<JsonResult> Delete(int id)
        {
            if (id > 0)
            {
                try
                {
                    await unitOfWork.GetManagerRepo.Delete(id);
                    await unitOfWork.Save();

                    return Json(new { result = true });
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, $"{DateTime.Now.ToLongTimeString()} : Server error, when trying to delete a manager!");
                    return Json(new { result = true, message = ex.Message });
                }
            }

            _logger.Error($"{DateTime.Now.ToLongTimeString()} : Id less 1!");
            return Json(new { result = false, message = "Id less 1!" });
        }
    }
}