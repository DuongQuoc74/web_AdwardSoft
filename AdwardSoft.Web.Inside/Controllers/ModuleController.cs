using System.Collections.Generic;
using System.Threading.Tasks;
using AdwardSoft.Provider.API;
using AdwardSoft.Provider.Common;
using AdwardSoft.Web.Inside.Models;
using Microsoft.AspNetCore.Mvc;
using AdwardSoft.Provider.Helper;
using AdwardSoft.Web.Inside.Models.Common;
using AdwardSoft.Security.Authorization;
using System.Linq;

namespace AdwardSoft.Web.Inside.Controllers
{
    [AdsAuthorization]
    public class ModuleController : Controller
    {
        #region Structure
        private IUserSession _userSession;
        private IAPIFactory _apiFactory;

        public ModuleController(
            IUserSession userSession,
            IAPIFactory apiFactory)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
        }
        #endregion

        #region View
        public async Task<IActionResult> Index()
        {
            return View(await Read());
        }

        public IActionResult _ViewIcons(int id)
        {
            ViewBag.IsNew = id == 0 ? true : false;
            ViewBag.Id = id;
            return PartialView();
        }

        #endregion

        #region Form
        public async Task<IActionResult> _Form(int id)
        {
            try
            {
                var model = new ModuleViewModel();
                if (id != 0)
                {
                    model = await _apiFactory.GetAsync<ModuleViewModel>("Module/ReadById?id=" + id, HostConstants.ApiCore, _userSession.BearerToken);
                    if (model.Types != null) model.UserTypes = model.Types.Split(",").Where(x => x != string.Empty).Select(short.Parse).ToList();
                }
                model.ListModule = await _apiFactory.GetAsync<List<ModuleViewModel>>("Module/Read", HostConstants.ApiCore, _userSession.BearerToken);
                return PartialView(model);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }        
        }
        #endregion

        #region Create
        [HttpPost]
        public async Task<ResponseContainer> Create(ModuleViewModel module)
        {
            module.Types = string.Join(",", module.UserTypes);
            var result = await _apiFactory.PostAsync<ModuleViewModel, bool>(module, this.ApiResources("Create"), HostConstants.ApiCore, _userSession.BearerToken);

            ResponseContainer response = new ResponseContainer();
            response.Action = "create";
            response.Activity = "Thêm mới";
            response.Succeeded = result;
            return response;
        }
        #endregion

        #region Update
        [HttpPost]
        public async Task<ResponseContainer> Update(ModuleViewModel module)
        {
            module.Types = string.Join(",", module.UserTypes);
            var result = await _apiFactory.PutAsync<ModuleViewModel, bool>(module, this.ApiResources("Update"), HostConstants.ApiCore, _userSession.BearerToken);

            ResponseContainer response = new ResponseContainer();
            response.Action = "update";
            response.Activity = "Điều chỉnh";
            response.Succeeded = result;
            return response;
        }
        #endregion

        #region Read
        [HttpGet]
        public async Task<List<ModuleViewModel>> Read()
        {
            return await _apiFactory.GetAsync<List<ModuleViewModel>>("Module/Read", HostConstants.ApiCore, _userSession.BearerToken);
        }
        #endregion

        #region Sort
        [HttpPost]
        public async Task<ResponseContainer> Sort(string json)
        {
            var response = new ResponseContainer();
            ModuleSortViewModel model = new ModuleSortViewModel() { Data = json };
            var result = await _apiFactory.PostAsync<ModuleSortViewModel, int>(model, "Module/Sort", HostConstants.ApiCore, _userSession.BearerToken);

            response.Action = "Update";
            response.Activity = "Sắp xếp module";
            return response;

        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<ResponseContainer> Delete(int id)
        {
            var response = new ResponseContainer();
            var result = await _apiFactory.DeleteAsync<bool>("Module/Delete?id=" + id, HostConstants.ApiCore, _userSession.BearerToken);

            response.Action = "delete";
            response.Activity = "Xóa menu";
            response.Succeeded = result;
            return response;
        }
        #endregion
    }
}