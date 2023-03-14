using AdwardSoft.Provider.API;
using AdwardSoft.Provider.Common;
using AdwardSoft.Provider.Helper;
using AdwardSoft.Security.Authorization;
using AdwardSoft.Web.Inside.Models.Common;
using AdwardSoft.Web.Inside.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using DocumentFormat.OpenXml.EMMA;

namespace AdwardSoft.Web.Inside.Controllers
{
    [AdsAuthorization]
    public class PriceListController : Controller
    {
        #region Constructor

        private readonly IUserSession _userSession;
        private readonly IAPIFactory _apiFactory;

        public PriceListController(IUserSession userSession, IAPIFactory apiFactory)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
        }

        #endregion

        #region View

        public IActionResult Index()
        {
            return View();
        }

        #endregion

        #region Form

        public async Task<IActionResult> _Form(DateTime date)
        {
            var model = new PriceListViewModel { Date = date };

            if (model.Date.CompareTo(default(DateTime)) > 0)
            {
                model = await _ReadByDateAsync(model.Date);
            } 

            return PartialView(model);
        }

        #endregion

        #region Read

        [HttpPost]
        public async Task<IEnumerable<PriceListViewModel>> Read(string date)
        {
            var dateConverted = _CovertStrToDateRange(date);

            var response = await _apiFactory.GetAsync<List<PriceListViewModel>>(
                $"PriceList/Read?dateFrom={_ConvertDateToShortDateStr(dateConverted.dateFrom)}&dateTo={_ConvertDateToShortDateStr(dateConverted.dateTo)}",
                Endpoints.ApiPOS,
                _userSession.BearerToken
            );

            return response;
        }

        [HttpGet]
        public async Task<PriceListViewModel> ReadByDate(DateTime date) => await _ReadByDateAsync(date);
        #endregion

        #region Create
        [HttpPost]
        public async Task<ResponseContainer> Create(PriceListViewModel vm)
        {

            var result = await _apiFactory.PostAsync<PriceListViewModel, int>(
                data: vm,
                apiUrl: this.ApiResources("Create"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken);

            var response = new ResponseContainer
            {
                Activity = "Thêm mới",
                Action = "Create",
                Succeeded = result > 0 ? true : false
            };

            return response;
        }
        #endregion

        #region Update
        [HttpPut]
        public async Task<ResponseContainer> Update(PriceListViewModel vm)
        {

            var result = await _apiFactory.PutAsync<PriceListViewModel, int>(
                data: vm,
                apiUrl: this.ApiResources("Update"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken);

            var response = new ResponseContainer
            {
                Activity = "Điều chỉnh",
                Action = "Update",
                Succeeded = result > 0 ? true : false
            };

            return response;
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<ResponseContainer> Delete(DateTime date)
        {
            var result = await _apiFactory.DeleteAsync<int>(
                apiUrl: this.ApiResources($"Delete?Date={_ConvertDateToShortDateStr(date)}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken);

            var response = new ResponseContainer
            {
                Action = "Delete",
                Activity = "Xóa",
                Succeeded = result > 0 ? true : false
            };
            return response;
        }

        #endregion

        #region Methods
        private async Task<PriceListViewModel> _ReadByDateAsync(DateTime date)
        {
            var response = await _apiFactory.GetAsync<PriceListViewModel>(
                apiUrl: this.ApiResources($"ReadByDate?Date={_ConvertDateToShortDateStr(date)}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
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

        private string _ConvertDateToShortDateStr(DateTime date)
        {
            string[] DateStr = date.ToShortDateString().Split("/");

            return DateStr[2] + "-" + DateStr[1] + "-" + DateStr[0];
        }
        #endregion
    }
}
