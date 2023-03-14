using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Provider.API;
using AdwardSoft.Provider.Common;
using AdwardSoft.Provider.Helper;
using AdwardSoft.Security.Authorization;
using AdwardSoft.Web.Inside.Models;
using AdwardSoft.Web.Inside.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdwardSoft.Web.Inside.Controllers
{
    [AdsAuthorization]
    public class ServiceDisplayController : Controller
    {
        #region Constructor

        private readonly IUserSession _userSession;
        private readonly IAPIFactory _apiFactory;

        public ServiceDisplayController(IUserSession userSession, IAPIFactory apiFactory)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
        }

        #endregion

        #region View

        public async Task<IActionResult> Index()
        {
            ViewBag.Suppliers = (await _SupplierSelect()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Text });
            return View();
        }

        #endregion

        #region Form

        public async Task<IActionResult> _Form(int id)
        {
            ViewBag.Suppliers = (await _SupplierSelect()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Text });
            var model = new ServiceDisplayViewModel { Id = id };
            model.DateFrom = DateTime.Now;
            model.DateTo = DateTime.Now;
            if (model.Id > 0)
            {
                model = await _ReadByIdAsync(id);
            }

            return PartialView(model);
        }

        #endregion

        #region Read
        [HttpPost]
        public async Task<IActionResult> Read(DataTableAjaxPostModel model, string date, int supplierId, short status)
        {
            if (String.IsNullOrEmpty(model.Search.Value))
                model.Search.Value = "NULL";

            var dateConverted = _CovertStrToDateRange(date);

            var response = await _apiFactory.GetAsync<List<ServiceDisplayDataViewModel>>(
                $"ServiceDisplay/Read?pageSize={model.Length}&pageSkip={model.Start}&searchBy={model.Search.Value}" +
                    $"&dateFrom={dateConverted.dateFrom}&dateTo={dateConverted.dateTo}" +
                    $"&supplierId={supplierId}&status={status}",
                Endpoints.ApiPOS,
                _userSession.BearerToken
            );

            int recordsTotal = response.Any() ? response.FirstOrDefault().Count : 0;

            return Json(new { draw = model.Draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = response });
        }


        [HttpGet]
        public async Task<IEnumerable<ServiceDisplayViewModel>> Read()
        {
            var response = await _apiFactory.GetAsync<IEnumerable<ServiceDisplayViewModel>>(
                apiUrl: this.ApiResources($"Read"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }

        [HttpGet]
        public async Task<ServiceDisplayViewModel> ReadById(int id) => await _ReadByIdAsync(id);
        #endregion

        #region Create
        [HttpPost]
        public async Task<ResponseContainer> Create(ServiceDisplayViewModel model)
        {
            model.DateFrom = _CovertStrToDate(model.DateFromStr);
            model.DateTo = _CovertStrToDate(model.DateToStr);
            var result = await _apiFactory.PostAsync<ServiceDisplayViewModel, int>(
                data: model,
                apiUrl: this.ApiResources("Create"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken);

            var response = new ResponseContainer
            {
                Activity = "Create",
                Action = "Create",
                Succeeded = result > 0 ? true : false
            };

            return response;
        }
        #endregion

        #region Update
        [HttpPut]
        public async Task<ResponseContainer> Update(ServiceDisplayViewModel model)
        {
            model.DateFrom = _CovertStrToDate(model.DateFromStr);
            model.DateTo = _CovertStrToDate(model.DateToStr);
            var result = await _apiFactory.PutAsync<ServiceDisplayViewModel, int>(
                data: model,
                apiUrl: this.ApiResources("Update"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken);

            var response = new ResponseContainer
            {
                Activity = "Update",
                Action = "Update",
                Succeeded = result > 0 ? true : false
            };

            return response;
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<ResponseContainer> Delete(int id)
        {
            var result = await _apiFactory.DeleteAsync<int>(
                apiUrl: this.ApiResources($"Delete?Id={id}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken);

            var response = new ResponseContainer
            {
                Action = "Delete",
                Activity = "Delete",
                Succeeded = result > 0 ? true : false
            };
            return response;
        }

        #endregion

        #region Methods
        private async Task<ServiceDisplayViewModel> _ReadByIdAsync(int id)
        {
            var response = await _apiFactory.GetAsync<ServiceDisplayViewModel>(
                apiUrl: this.ApiResources($"ReadById?id={id}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }
        #endregion

        #region Function
        private async Task<List<Select>> _SupplierSelect()
        {
            var StockList = await _apiFactory.GetAsync<List<Select>>
            (
                apiUrl: $"Supplier/ReadSelect",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return StockList;
        }

        private (DateTime dateFrom, DateTime dateTo) _CovertStrToDateRange(string date)
        {
            string[] datespl = date.Split(" ");
            string[] datesplForm = datespl[0].Split("/");
            string[] datesplTo = datespl[2].Split("/");

            DateTime dateFrom = DateTime.Parse(datesplForm[2] + "-" + datesplForm[1] + "-" + datesplForm[0]);
            DateTime dateTo = DateTime.Parse(datesplTo[2] + "-" + datesplTo[1] + "-" + datesplTo[0]);

            return (dateFrom, dateTo);
        }

        private DateTime _CovertStrToDate(string dateStr)
        {
            string[] DateStr = dateStr.Split("/");
            DateTime date = DateTime.Parse(DateStr[2] + "-" + DateStr[1] + "-" + DateStr[0]);
            return date;
        }
        #endregion
    }
}
