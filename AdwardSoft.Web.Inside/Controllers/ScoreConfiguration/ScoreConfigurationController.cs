using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Provider.API;
using AdwardSoft.Provider.Common;
using AdwardSoft.Provider.Helper;
using AdwardSoft.Web.Inside.Models;
using AdwardSoft.Web.Inside.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace AdwardSoft.Web.Inside.Controllers
{
    public class ScoreConfigurationController : Controller
    {
        #region Contructor
        private IUserSession _userSession;
        private IAPIFactory _apiFactory;

        public ScoreConfigurationController(IUserSession userSession, IAPIFactory apiFactory)
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
        public async Task<IActionResult> _Form(int id)
        {
            var model = new ScoreConfigurationViewModel();
            ViewBag.Action = "Create";
            if (id != 0)
            {
                ViewBag.Action = "Update";
                model = await _apiFactory.GetAsync<ScoreConfigurationViewModel>("ScoreConfiguration/ReadById?Id=" + id, Endpoints.ApiPOS, _userSession.BearerToken);
                model.DateStr = model.EffectiveDate.ToShortDateString();
            }
            return PartialView(model);
        }
        #endregion

        #region Read

        [HttpPost]
        public async Task<IActionResult> Read(DataTableAjaxPostModel model, int status)
        {
            if (String.IsNullOrEmpty(model.Search.Value))
            {
                model.Search.Value = "NULL";
            }

            var response = await _apiFactory.GetAsync<List<ScoreConfigurationDatatableViewModel>>(
                $"ScoreConfiguration/ReadDatatable?pageSize={model.Length}&pageSkip={model.Start}&searchBy={model.Search.Value}&status={status}",
                Endpoints.ApiPOS,
                _userSession.BearerToken
            );

            int recordsTotal = response.Any() ? response.FirstOrDefault().Count : 0;

            return Json(new { draw = model.Draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = response });
        }

        #endregion Read

        #region Create
        [HttpPost]
        public async Task<ResponseContainer> Create(ScoreConfigurationViewModel model)
        {
            string[] str = model.DateStr.Split("/");
            model.EffectiveDate = DateTime.Parse(str[2] + "-" + str[1] + "-" + str[0]);
            var result = await _apiFactory.PostAsync<ScoreConfigurationViewModel, int>(
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
        [HttpPost]
        public async Task<ResponseContainer> Update(ScoreConfigurationViewModel model)
        {
            string[] str = model.DateStr.Split("/");
            model.EffectiveDate = DateTime.Parse(str[2] + "-" + str[1] + "-" + str[0]);

            var result = await _apiFactory.PutAsync<ScoreConfigurationViewModel, int>(
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
            var response = new ResponseContainer();
            response.Activity = "Delete";
            if (result == 1)
            {
                response.Succeeded = true;
            }
            else
            {
                response.Activity = "ScoreConfiguration is using. Remove ";
                response.Succeeded = false;
            }
            return response;
        }
        #endregion

        #region Remote CheckDate
        public async Task<JsonResult> CheckDate(int id, string DateStr)
        {


            // => Check phone
            var isExisting = await _apiFactory.GetAsync<int>
            (
                apiUrl: this.ApiResources($"CheckDateIsExisting?id={id}&date={DateStr}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            if (isExisting > 0)
                return Json("The Date is existing");

            return Json(true);
        }
        #endregion

    }
}
