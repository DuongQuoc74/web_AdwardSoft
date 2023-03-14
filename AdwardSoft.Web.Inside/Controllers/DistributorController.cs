using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AdwardSoft.Provider.API;
using AdwardSoft.Provider.Common;
using AdwardSoft.Provider.Helper;
using AdwardSoft.Security.Authorization;
using AdwardSoft.Security.Claims;
using AdwardSoft.Web.Inside.Helper;
using AdwardSoft.Web.Inside.Models;
using AdwardSoft.Web.Inside.Models.Common;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdwardSoft.Web.Inside.Controllers
{
    public class DistributorController : Controller
    {

        #region Constructor

        private int _CustomerId;
        private readonly IUserSession _userSession;
        private readonly IAPIFactory _apiFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DistributorController(IUserSession userSession, IAPIFactory apiFactory, IHttpContextAccessor httpContextAccessor)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
            _httpContextAccessor = httpContextAccessor;
            var customerId = ClaimsPrincipalIdentity.GetClaim("customerId", _httpContextAccessor.HttpContext);

            Int32.TryParse(customerId, out _CustomerId);
        }


        #endregion

        #region View

        public async Task<IActionResult> Index()
        {
            var model = new CustomerDatatableViewModel();

            if (_CustomerId != 0)
            {
                model = await _ReadByIdAsync();
            }

            return View(model);
        }

        #endregion

        #region Form

        public async Task<IActionResult> _Form()
        {
            var model = new CustomerDatatableViewModel();

            if (_CustomerId != 0)
            {
                model = await _ReadByIdAsync();
            }
            
            // View bag

            ViewBag.CustomerGroups = (await _ReadCustomerGroupsSelect())
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });

            ViewBag.PaymentMethods = (await _ReadPaymentMethodsSelect())
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });

            ViewBag.Genders = (await _ReadGendersSelect())
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });

            ViewBag.Stocks = (await _StockSelect())
                .Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Text });

            return PartialView(model);
        }

        #endregion

        #region Read

        [HttpGet]
        public async Task<CustomerDatatableViewModel> ReadById() => await _ReadByIdAsync();

        #endregion 

        #region Create

        [HttpPost]
        public async Task<ResponseContainer> Create(CustomerViewModel model)
        {
            if (model.DoBStr != null)
            {
                model.DoB = DateHelper.StringToDate(model.DoBStr);
            }
            else
            {
                model.DoB = DateTime.Now;
            }
            model.UserId = long.Parse(_userSession.UserId);
            var result = await _apiFactory.PostAsync<CustomerViewModel, int>
            (
                 data: model,
                 apiUrl: $"Customer/Create",
                 endpoint: Endpoints.ApiPOS,
                 token: _userSession.BearerToken
            );

            if (result > 0 && _CustomerId == 0)
            {
                var claim = new Claim("customerId", result.ToString(), ClaimValueTypes.Integer32);
                ClaimsPrincipalIdentity.UpdateClaims(claim, HttpContext);
            }

            var response = new ResponseContainer
            {
                Activity = "Cập nhật",
                Action = "Create",
                Succeeded = result > 0 ? true : false
            };

            return response;
        }

        #endregion

        #region Update

        [HttpPut]
        public async Task<ResponseContainer> Update(CustomerViewModel model)
        {
            if (model.DoBStr != null)
            {
                model.DoB = DateHelper.StringToDate(model.DoBStr);
            }
            else
            {
                model.DoB = DateTime.Now;
            }
            var result = await _apiFactory.PutAsync<CustomerViewModel, int>
            (
                 data: model,
                 apiUrl: $"Customer/Update",
                 endpoint: Endpoints.ApiPOS,
                 token: _userSession.BearerToken
            );

            var response = new ResponseContainer
            {
                Activity = "Điều chỉnh",
                Action = "Update",
                Succeeded = result > 0 ? true : false
            };

            return response;
        }

        #endregion

        #region Methods

        private async Task<CustomerDatatableViewModel> _ReadByIdAsync()
        {
            var response = await _apiFactory.GetAsync<CustomerDatatableViewModel>(
                apiUrl: $"Customer/ReadById?id={_CustomerId}",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }

        private async Task<IEnumerable<Select>> _ReadCustomerGroupsSelect()
        {
            var customerGroupsSelect = await _apiFactory.GetAsync<IEnumerable<Select>>
            (
                apiUrl: $"CustomerGroup/ReadSelect?",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return customerGroupsSelect;
        }

        private async Task<IEnumerable<Select>> _ReadPaymentMethodsSelect()
        {
            var paymentMethodsSelect = await _apiFactory.GetAsync<IEnumerable<Select>>
            (
                apiUrl: "PaymentMethod/ReadSelect",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return paymentMethodsSelect;
        }

        private async Task<IEnumerable<Select>> _ReadGendersSelect()
        {
            var gendersSelect = await _apiFactory.GetAsync<IEnumerable<Select>>
            (
                apiUrl: "Gender/ReadSelect",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return gendersSelect;
        }

        private async Task<List<Select>> _StockSelect()
        {
            var StockList = await _apiFactory.GetAsync<List<Select>>
            (
                apiUrl: "Stock/ReadSelect",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return StockList;
        }

        #endregion
    }
}
