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
    public class CustomerOrderController : Controller
    {

        #region Constructor

        private int _CustomerId;
        private int _BranchId;
        private IUserSession _userSession;
        private IAPIFactory _apiFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomerOrderController(IUserSession userSession, IAPIFactory apiFactory, IHttpContextAccessor httpContextAccessor)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
            _httpContextAccessor = httpContextAccessor;
            var customerId = ClaimsPrincipalIdentity.GetClaim("customerId", _httpContextAccessor.HttpContext);
            var branchId = ClaimsPrincipalIdentity.GetClaim("branch_id", _httpContextAccessor.HttpContext);

            Int32.TryParse(customerId, out _CustomerId);
            Int32.TryParse(branchId, out _BranchId);
        }

        #endregion

        #region View
        public IActionResult PayOrder()
        {
            return View();
        }

        public async Task<IActionResult> Index()
        {
            var model = new CustomerOrderViewModel();
            var currentCustomer = new CustomerViewModel { Id = _CustomerId };
            // Get current customer info
            if (currentCustomer.Id > 0)
                currentCustomer = await _ReadByCustomerIdAsync(_CustomerId);

            model.CustomerId = _CustomerId;
            model.BranchId = _BranchId;
            model.Receiver = currentCustomer.Name;
            model.ReceiverPhone = currentCustomer.Phone;
            model.ReceiverAddress = currentCustomer.Address;
            model.OrderDetail = (await ReadProducts());
            model.ModifiedUser = long.Parse(_userSession.UserId);
            model.CreatedUser = long.Parse(_userSession.UserId);

            ViewBag.PaymentMethods = (await _ReadPaymentMethodsSelect())
               .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });

            ViewBag.DeliveryVehicles = (await _ReadDeliveryVehiclesSelect())
               .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });

            ViewBag.Locations = (await _ReadLocationSelect()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Text });

            return View(model);
        }

        #endregion

        #region Form


        #endregion

        #region Read
        [HttpPost("ReadDataTable")]
        public async Task<IActionResult> ReadDataTable(DataTableAjaxPostModel model, string fromDate, string toDate, short paymentStatus = 0)
        {
            if (String.IsNullOrEmpty(model.Search.Value))
                model.Search.Value = "NULL";

            fromDate = fromDate.IsNullOrEmpty() ? DateTime.Today.ToString("yyyy/MM/dd") : fromDate;
            toDate = toDate.IsNullOrEmpty() ? DateTime.Today.ToString("yyyy/MM/dd") : toDate;

            // If Unvalid Payment Status OR PaymentStatus Is STATUS_PAID => Set to default value view all
            // Because we not allow to view PAID Status
            if (!Enum.IsDefined(typeof(ECustomerOrderPaymentStatus), (int)paymentStatus) ||
                paymentStatus == (short)ECustomerOrderPaymentStatus.PAYMENT_STATUS_PAID)
            {
                paymentStatus = (short)ECustomerOrderPaymentStatus.PAYMENT_STATUS_ALL;
            }

            var response = await _apiFactory.GetAsync<List<CustomerOrderDatatableViewModel>>(
                apiUrl: this.ApiResources($"ReadDataTable?pageSkip={model.Start}&pageSize={model.Length}" +
                    $"&paymentStatus={paymentStatus}&fromDate={fromDate}&toDate={toDate}" +
                    $"&searchBy={model.Search.Value}"),

                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            int recordsTotalCount = response.Any() ? response.Count() : 0;

            return Json(new { draw = model.Draw, recordsFiltered = recordsTotalCount, recordsTotal = recordsTotalCount, data = response });
        }

        [HttpGet]
        public async Task<IEnumerable<CustomerOrderViewModel>> Read()
        {
            var response = await _apiFactory.GetAsync<IEnumerable<CustomerOrderViewModel>>(
                apiUrl: this.ApiResources($"Read"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }

        [HttpPost]
        public async Task<IActionResult> ReadChildSelect(int parentId)
        {
            var response = (await _ReadLocationChildSelect(parentId)).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Text });

            return Json(new { data = response });
        }

        [HttpPost]
        public async Task<IActionResult> ReadDeliveryPointSelect(int locationId)
        {
            var response = (await _ReadDeliveryPointSelect(locationId)).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Text });

            return Json(new { data = response });
        }

        [HttpPost]
        public async Task<List<CustomerOrderDetailDatatableViewModel>> ReadProducts()
        {
            var productList = await _apiFactory.GetAsync<List<CustomerOrderDetailDatatableViewModel>>
            (
                apiUrl: $"Product/ReadProductOrder",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
             );

            return productList;
        }

        [HttpGet]
        public async Task<CustomerOrderViewModel> ReadById(int id) => await _ReadByIdAsync(id);

        #endregion 

        #region Create

        [HttpPost]
        public async Task<ResponseContainer> Create(CustomerOrderViewModel model)
        {
            string result = null;
            if (model.OrderDetail.Count > 0)
            {
                result = await _apiFactory.PostAsync<CustomerOrderViewModel, string>(
                    data: model,
                    apiUrl: this.ApiResources("Create"),
                    endpoint: Endpoints.ApiPOS,
                    token: _userSession.BearerToken);

                if (!String.IsNullOrEmpty(result))
                {
                    model.OrderDetail.Select(p => {
                        if (/*p.IsOrder &&*/ p.QuantityReg > 0)
                        {
                            p.OrderId = result;
                        }
                        return p;
                    }).ToList();
                    var resultOrderDetail = await _apiFactory.PostAsync<List<CustomerOrderDetailDatatableViewModel>, int>(
                     data: model.OrderDetail,
                     apiUrl: this.ApiResources("CreateCustomerOrderDetail"),
                     endpoint: Endpoints.ApiPOS,
                     token: _userSession.BearerToken);
                }
            }


            var response = new ResponseContainer
            {
                Activity = "Đặt hàng",
                Action = "Create",
                Succeeded = !String.IsNullOrEmpty(result) ? true : false
            };


            return response;
        }

        #endregion

        #region Update

        [HttpPut]
        public async Task<ResponseContainer> Update(CustomerOrderViewModel model)
        {
            var result = await _apiFactory.PutAsync<CustomerOrderViewModel, int>
            (
                 data: model,
                 apiUrl: this.ApiResources("Update"),
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

        [HttpPut]
        public async Task<ResponseContainer> PayOrder(List<string> orderIDs)
        {
            ResponseContainer response = new ResponseContainer
            {
                Activity = "Thanh toán đơn hàng",
                Action = "Thanh toán đơn hàng",
                Succeeded = false
            };


            // Step 1: Get Account Balance By UserID And Validate
            var accountBalance = await _apiFactory.GetAsync<long>(
                apiUrl: $"CustomerWallet/ReadAccountBalance?customerId={_userSession.UserId}",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            if (accountBalance == 0)
            {
                response.CustomMessage = $"Số dư hiện tại bằng {accountBalance}. Không đủ số dư thanh toán.";
                return response;
            }

            // Step 2: Get List Of CustomerOrder Base On orderIDs list
            var resultGetListOrder = await _apiFactory.PostAsync<List<string>, List<CustomerOrderViewModel>>(
                data: orderIDs,
                apiUrl: "CustomerOrder/ReadOrderIds",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            // If Can't find any order or in orderIds List contain at least 1 wrong order id => Fail
            if (resultGetListOrder.Count == 0 || orderIDs.Count != resultGetListOrder.Count)
            {
                response.CustomMessage = "Không tìm thấy đơn hàng hợp lệ!";
                return response;
            }

            // Step 3: Validate and filter out order that don't have enough balance to pay
            List<CustomerOrderViewModel> affordAbleOrder = new List<CustomerOrderViewModel>();

            for (int i = 0; i < resultGetListOrder.Count; i++)
            {
                var order = resultGetListOrder[i];

                if (order.PaymentStatus != (short)ECustomerOrderPaymentStatus.PAYMENT_STATUS_UNPAY)
                {
                    response.CustomMessage = "Không thể thanh toán đơn hàng đã bị từ chối hoặc đã được thanh toán!";
                    return response;
                }

                if (order.TotalAmount <= accountBalance)
                {
                    accountBalance = accountBalance - (long)order.TotalAmount;

                    // Update PaymentUser = Current UserID
                    order.PaymentUser = long.Parse(_userSession.UserId);

                    // Add to afford
                    affordAbleOrder.Add(order);
                }
                else
                {
                    // If not enough money => Break
                    break;
                }
            }

            if (affordAbleOrder.Count == 0)
            {
                // Không đủ số dư thanh toán
                response.CustomMessage = "Số dư hiện tại không đủ để thanh toán.";
                return response;
            }

            // Step 4: Update List Of Order Set PaymentStatus to PAID
            var resultUpdateListOrder = await _apiFactory.PutAsync<List<CustomerOrderViewModel>, int>(
                data: affordAbleOrder,
                apiUrl: this.ApiResources("PayOrder"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            if (resultUpdateListOrder <= 0)
            {
                response.CustomMessage = $"Xảy ra lỗi trong quá trình thanh toán vui lòng thử lại sau!";
                return response;
            }

            // Step 5: Create CustomerWallet Base On affordAbleOrder
            List<CustomerWalletViewModel> customerWalletList = affordAbleOrder.Select((order) =>
            {
                CustomerWalletViewModel customerWallet = new CustomerWalletViewModel();

                // Parse To Int Because In Customer and CustomerOrder CustomerID is Int
                customerWallet.CustomerId = Int32.Parse(_userSession.UserId);
                customerWallet.BankAccountId = 0;
                customerWallet.Status = (short)ECustomerWalletStatus.Succeed;
                customerWallet.Type = 1;
                customerWallet.Note = $"Thanh toán đơn hàng số: {order.Id}";
                customerWallet.Date = DateTime.Now;
                customerWallet.Amount = order.TotalAmount;

                return customerWallet;
            }).ToList();

            // Step 6: Create Multi CustomerWallet
            var result = await _apiFactory.PostAsync<List<CustomerWalletViewModel>, bool>(
                data: customerWalletList,
                apiUrl: "CustomerWallet/CreateMulti",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            if (!result)
            {
                response.CustomMessage = "Thanh toán đơn hàng không thành công!";
                return response;
            }

            response.Succeeded = true;
            response.CustomMessage = $"Thanh toán thành công {affordAbleOrder.Count}/{orderIDs.Count} đơn hàng!";

            return response;
        }
        #endregion

        #region Delete

        [HttpPost]
        public async Task<ResponseContainer> Delete(int id)
        {
            var result = await _apiFactory.DeleteAsync<int>
            (
                apiUrl: this.ApiResources($"Delete?Id={id}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            var response = new ResponseContainer
            {
                Activity = result == 1 ? "Xóa" : "Tài khoản ngân hàng đang được sử dụng. Xóa ",
                Succeeded = result == 1 ? true : false,
                Action = "Delete"
            };

            return response;
        }

        #endregion

        #region Check

        //public async Task<JsonResult> CheckDuplicateBankNo(int id, string bankNo)
        //{
        //    var duplicateCustomerOrder = await _ReadByBankNoAsync(id, bankNo);

        //    if (string.IsNullOrWhiteSpace(duplicateCustomerOrder.BankNo))
        //        return Json(true);
        //    return Json(false);
        //}

        #endregion

        #region Methods

        private async Task<CustomerOrderViewModel> _ReadByIdAsync(int id)
        {
            var response = await _apiFactory.GetAsync<CustomerOrderViewModel>(
                apiUrl: this.ApiResources($"ReadById?id={id}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }

        private async Task<CustomerViewModel> _ReadByCustomerIdAsync(int id)
        {
            var response = await _apiFactory.GetAsync<CustomerViewModel>(
                apiUrl: $"Customer/ReadById?id={id}",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
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

        private async Task<IEnumerable<Select>> _ReadDeliveryVehiclesSelect()
        {
            var deliveryVehiclesSelect = await _apiFactory.GetAsync<IEnumerable<Select>>
            (
                apiUrl: $"DeliveryVehicle/ReadSelect?customerId={_CustomerId}",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return deliveryVehiclesSelect;
        }

        private async Task<List<Select>> _ReadLocationSelect()
        {
            var locationList = await _apiFactory.GetAsync<List<Select>>
            (
                apiUrl: "Location/ReadSelect",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return locationList;
        }
        private async Task<List<Select>> _ReadLocationChildSelect(int parentId)
        {
            var locationList = await _apiFactory.GetAsync<List<Select>>
            (
                apiUrl: $"Location/ReadChildSelect?parentId={parentId}",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return locationList;
        }
        private async Task<List<Select>> _ReadDeliveryPointSelect(int locationId)
        {
            var deliveryPointList = await _apiFactory.GetAsync<List<Select>>
            (
                apiUrl: $"DeliveryPoint/ReadSelectByLocationId?locationId={locationId}",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return deliveryPointList;
        }

        #endregion
    }
}
