using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Provider.API;
using AdwardSoft.Provider.Common;
using AdwardSoft.Provider.Helper;
using AdwardSoft.Security.Authorization;
using AdwardSoft.Security.Claims;
using AdwardSoft.Utilities.Extensions;
using AdwardSoft.Web.Inside.Models;
using AdwardSoft.Web.Inside.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdwardSoft.Web.Inside.Controllers
{
    [AdsAuthorization]
    public class CustomerWalletController : Controller
    {
        #region Constructor

        private int _CustomerId;
        private readonly IUserSession _userSession;
        private readonly IAPIFactory _apiFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomerWalletController(IUserSession userSession, IAPIFactory apiFactory, IHttpContextAccessor httpContextAccessor)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
            _httpContextAccessor = httpContextAccessor;
            var customerId = ClaimsPrincipalIdentity.GetClaim("customerId", _httpContextAccessor.HttpContext);

            Int32.TryParse(customerId, out _CustomerId);
        }

        #endregion

        #region View
        // Nạp tiền thanh toán
        // [Route("/Recharge")]
        public IActionResult Recharge()
        {
            return View();
        }

        // Cấp tín dụng
        // [Route("/TopupCredit")]
        public async Task<IActionResult> TopupCredit()
        {
            ViewBag.Customers = (await _ReadCustomersSelectByCreditLimit())
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });
            return View();
        }

        // Nạp tiền thanh toán hộ
        // [Route("/InternalRecharge")]
        public async Task<IActionResult> InternalRecharge()
        {
            ViewBag.Customers = (await _ReadCustomersSelect())
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });
            return View();
        }

        // Duyệt cấp tín dụng
        // [Route("/CreditApproval")]
        public IActionResult CreditApproval()
        {
            return View();
        }

        // Duyệt nạp tiền thanh toán
        // [Route("/RechargeApproval")]
        public IActionResult RechargeApproval()
        {
            return View();
        }

        #endregion

        #region Form

        public async Task<IActionResult> _FormRecharge(string id)
        {
            var model = new CustomerWalletViewModel { Id = id };

            if (!String.IsNullOrEmpty(model.Id))
                model = await _ReadByIdAsync(id);

            ViewBag.BankAccounts = (await _ReadBankAccountsSelect())
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });

            return PartialView(model);
        }

        public async Task<IActionResult> _FormTopupCredit(string id)
        {
            var model = new CustomerWalletViewModel { Id = id };

            if (!String.IsNullOrEmpty(model.Id))
                model = await _ReadByIdAsync(id);

            ViewBag.Customers = (await _ReadCustomersSelectByCreditLimit())
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });

            return PartialView(model);
        }

        public async Task<IActionResult> _FormInternalRecharge(string id)
        {
            var model = new CustomerWalletViewModel { Id = id };

            if (!String.IsNullOrEmpty(model.Id))
                model = await _ReadByIdAsync(id);

            ViewBag.Customers = (await _ReadCustomersSelect())
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });
            ViewBag.BankAccounts = (await _ReadBankAccountsSelect())
              .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });

            return PartialView(model);
        }

        #endregion

        #region Read

        [HttpPost]
        public async Task<IEnumerable<CustomerWalletDatatableViewModel>> ReadRecharge(string date, decimal amountFrom, decimal amountTo)
        {
            var dateConverted = _CovertStrToDateRange(date);

            var response = await _apiFactory.GetAsync<IEnumerable<CustomerWalletDatatableViewModel>>(
                apiUrl: this.ApiResources($"ReadDatatableRecharge?dateFrom={_ConvertDateToShortDateStr(dateConverted.dateFrom)}&dateTo={_ConvertDateToShortDateStr(dateConverted.dateTo)}&amountFrom={amountFrom}&amountTo={amountTo}&customerId={_CustomerId}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }

        [HttpPost]
        public async Task<IEnumerable<CustomerWalletDatatableViewModel>> ReadTopupCredit(string date, decimal amountFrom, decimal amountTo, int customerId)
        {
            var dateConverted = _CovertStrToDateRange(date);

            var response = await _apiFactory.GetAsync<IEnumerable<CustomerWalletDatatableViewModel>>(
                apiUrl: this.ApiResources($"ReadDatatableTopupCredit?dateFrom={_ConvertDateToShortDateStr(dateConverted.dateFrom)}&dateTo={_ConvertDateToShortDateStr(dateConverted.dateTo)}&amountFrom={amountFrom}&amountTo={amountTo}&customerId={customerId}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }

        [HttpPost]
        public async Task<IEnumerable<CustomerWalletDatatableViewModel>> ReadInternalRecharge(string date, decimal amountFrom, decimal amountTo, int customerId)
        {
            var dateConverted = _CovertStrToDateRange(date);

            var response = await _apiFactory.GetAsync<IEnumerable<CustomerWalletDatatableViewModel>>(
                apiUrl: this.ApiResources($"ReadDatatableInternalRecharge?dateFrom={_ConvertDateToShortDateStr(dateConverted.dateFrom)}&dateTo={_ConvertDateToShortDateStr(dateConverted.dateTo)}&amountFrom={amountFrom}&amountTo={amountTo}&customerId={customerId}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }

        [HttpPost]
        public async Task<IEnumerable<CustomerWalletDatatableViewModel>> ReadCreditApproval()
        {

            var response = await _apiFactory.GetAsync<IEnumerable<CustomerWalletDatatableViewModel>>(
                apiUrl: this.ApiResources($"ReadDatatableCreditApproval"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }

        [HttpPost]
        public async Task<IEnumerable<CustomerWalletDatatableViewModel>> ReadRechargeApproval()
        {

            var response = await _apiFactory.GetAsync<IEnumerable<CustomerWalletDatatableViewModel>>(
                apiUrl: this.ApiResources($"ReadDatatableRechargeApproval"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }

        [HttpGet]
        public async Task<CustomerWalletViewModel> ReadById(string id) => await _ReadByIdAsync(id);

        [HttpPost]
        public async Task<long> ReadAcocuntBalance()
        {
            var accountBalance = await _apiFactory.PostAsync<string, long>(
                data: _userSession.UserId,
                apiUrl: "/CustomerWallet/ReadAccountBalance",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return accountBalance;
        }
        #endregion

        #region Create

        [HttpPost]
        public async Task<ResponseContainer> Create(CustomerWalletViewModel vm)
        {
            if  (vm.CustomerId == 0)
            {
                vm.CustomerId = _CustomerId;
            }
            var result = await _apiFactory.PostAsync<CustomerWalletViewModel, string>(
                data: vm,
                apiUrl: this.ApiResources("Create"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken);

            var response = new ResponseContainer
            {
                Activity = "Thêm mới",
                Action = "Create",
                Succeeded = String.IsNullOrEmpty(result) ? false : true
            };

            return response;
        }

        #endregion

        #region Update

        [HttpPut]
        public async Task<ResponseContainer> Approval(string id, short status)
        {
            var model = new CustomerWalletViewModel { Id = id };

            if (!String.IsNullOrEmpty(model.Id))
            {
                model = await _ReadByIdAsync(id);
                model.Status = status;
            }

            var result = await _apiFactory.PutAsync<CustomerWalletViewModel, string>(
                data: model,
                apiUrl: this.ApiResources($"Approval"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken);

            var response = new ResponseContainer
            {
                Activity = "Duyệt",
                Action = "Update",
                Succeeded = String.IsNullOrEmpty(result) ? false : true
            };

            return response;
        }

        [HttpPut]
        public async Task<ResponseContainer> Update(CustomerWalletViewModel vm)
        {
            var result = await _apiFactory.PutAsync<CustomerWalletViewModel, string>(
                data: vm,
                apiUrl: this.ApiResources("Update"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken);

            var response = new ResponseContainer
            {
                Activity = "Cập nhật",
                Action = "Update",
                Succeeded = String.IsNullOrEmpty(result) ? false : true
            };

            return response;
        }

        #endregion

        #region Delete

        [HttpPost]
        public async Task<ResponseContainer> Delete(string id)
        {
            var result = await _apiFactory.DeleteAsync<int>(
                apiUrl: this.ApiResources($"Delete?Id={id}"),
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

        private async Task<CustomerWalletViewModel> _ReadByIdAsync(string id)
        {
            var response = await _apiFactory.GetAsync<CustomerWalletViewModel>
            (
                apiUrl: this.ApiResources($"ReadById?id={id}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }

        private async Task<IEnumerable<Select>> _ReadBankAccountsSelect()
        {
            var bankAccountsSelect = await _apiFactory.GetAsync<IEnumerable<Select>>
            (
                apiUrl: $"BankAccount/ReadSelect",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return bankAccountsSelect;
        }

        private async Task<IEnumerable<Select>> _ReadCustomersSelectByCreditLimit()
        {
            var customersSelect = await _apiFactory.GetAsync<IEnumerable<Select>>
            (
                apiUrl: $"Customer/ReadSelectByCreditLimit",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return customersSelect;
        }

        private async Task<IEnumerable<Select>> _ReadCustomersSelect()
        {
            var customersSelect = await _apiFactory.GetAsync<IEnumerable<Select>>
            (
                apiUrl: $"Customer/ReadSelect",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return customersSelect;
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
