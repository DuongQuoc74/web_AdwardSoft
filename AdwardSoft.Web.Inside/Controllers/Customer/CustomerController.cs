using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Provider.API;
using AdwardSoft.Provider.Common;
using AdwardSoft.Provider.Helper;
using AdwardSoft.Security.Authorization;
using AdwardSoft.Web.Inside.Helper;
using AdwardSoft.Web.Inside.Models;
using AdwardSoft.Web.Inside.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdwardSoft.Web.Inside.Controllers
{
    [AdsAuthorization]
    public class CustomerController : Controller
    {
        #region Constructor

        private readonly IUserSession _userSession;
        private readonly IAPIFactory _apiFactory;

        public CustomerController(IUserSession userSession, IAPIFactory apiFactory)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
        }

        #endregion

        #region View
        public async Task<IActionResult> Index()
        {
            ViewBag.CustomerGroups = (await _ReadCustomerGroupsSelect())
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });

            ViewBag.MembershipClasses = (await _ReadMembershipClassesSelect())
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });

            return View();
        }

        #endregion

        #region Form

        public async Task<IActionResult> _Form(int id)
        {
            var model = new CustomerViewModel { Id = id };

            if (model.Id > 0)
                model = await _ReadByIdAsync(id);


            // View bag
            ViewBag.MembershipClasses = (await _ReadMembershipClassesSelectDefault());

            ViewBag.CustomerGroups = (await _ReadCustomerGroupsSelect())
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });

            ViewBag.PaymentMethods = (await _ReadPaymentMethodsSelect())
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });
            
            ViewBag.Genders = (await _ReadGendersSelect())
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });

            return PartialView(model);
        }

        #endregion

        #region Read

        [HttpPost]
        public async Task<IActionResult> Read(DataTableAjaxPostModel model, int customerGroupId = 0, int type = -1)
        {
            if (String.IsNullOrEmpty(model.Search.Value))
                model.Search.Value = "NULL";

            var response = await _apiFactory.GetAsync<List<CustomerDatatableViewModel>>(
                apiUrl: this.ApiResources($"ReadDatatable?pageSize={model.Length}&pageSkip={model.Start}&searchBy={model.Search.Value}" +
                    $"&CustomerGroupId={customerGroupId}&type={type}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            int recordsTotal = response.Any() ? response.FirstOrDefault().Count : 0;

            return Json(new { draw = model.Draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = response });
        }

        [HttpGet]
        public async Task<CustomerDatatableViewModel> ReadById(int id) => await _ReadByIdAsync(id);

        #endregion

        #region Create

        [HttpPost]
        public async Task<ResponseContainer> Create(CustomerViewModel vm)
        {
            vm.DoB = DateTime.Now;

            var result = await _apiFactory.PostAsync<CustomerViewModel, int>(
                data: vm,
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
        public async Task<ResponseContainer> Update(CustomerViewModel vm)
        {
            if (vm.DoBStr != null)
            {
                vm.DoB = DateHelper.StringToDate(vm.DoBStr);
            }
            else
            {
                vm.DoB = DateTime.Now;
            }

            var result = await _apiFactory.PutAsync<CustomerViewModel, int>(
                data: vm,
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

        #region Remote

        public async Task<JsonResult> CheckPhone(int id, string phone)
        {
            // => Check phone
            var isExisting = await _apiFactory.GetAsync<int>
            (
                apiUrl: this.ApiResources($"CheckPhoneIsExisting?id={id}&phone={phone}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            if (isExisting > 0)
                return Json("The phone is existing");

            return Json(true);
        }

        public async Task<JsonResult> CheckIdentity(int id, string identityCode)
        {
            // => Check phone
            var isExisting = await _apiFactory.GetAsync<int>
            (
                apiUrl: this.ApiResources($"CheckIdentityIsExisting?id={id}&identityCode={identityCode}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            if (isExisting > 0)
                return Json("The identity is existing");

            return Json(true);
        }

        #endregion

        #region Methods

        private async Task<CustomerDatatableViewModel> _ReadByIdAsync(int id)
        {
            var response = await _apiFactory.GetAsync<CustomerDatatableViewModel>(
                apiUrl: this.ApiResources($"ReadById?id={id}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }

        private async Task<IEnumerable<SelectDefault>> _ReadMembershipClassesSelectDefault()
        {
            var membershipClassesSelectDefualt = await _apiFactory.GetAsync<IEnumerable<SelectDefault>>
            (
                apiUrl: $"MembershipClass/ReadSelectDefault",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return membershipClassesSelectDefualt;
        }

        private async Task<IEnumerable<Select>> _ReadMembershipClassesSelect()
        {
            var membershipClassesSelect = await _apiFactory.GetAsync<IEnumerable<Select>>
            (
                apiUrl: $"MembershipClass/ReadSelect",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return membershipClassesSelect;
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

        #endregion

        #region Search
        [HttpGet]
        public async Task<JsonResult> Search(string searchTerm, int pageSize, int pageNumber)
        {
            if (String.IsNullOrEmpty(searchTerm))
            {
                return Json(new CustomerSearch());
            }
            string strRequest = "Customer/Search?pageSize=" + pageSize + "&pagenumber=" + (pageNumber - 1)
                + "&keyword=" + searchTerm;
            var result = await _apiFactory.GetAsync<CustomerSearch>(strRequest, Endpoints.ApiPOS, _userSession.BearerToken);
            return Json(result);
        }
        #endregion
    }
}
