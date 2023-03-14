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
using Newtonsoft.Json;

namespace AdwardSoft.Web.Inside.Controllers
{
    [AdsAuthorization]
    public class MenuController : Controller
    {
        #region Construction

        private IUserSession _userSession;
        private IAPIFactory _apiFactory;

        public MenuController(IUserSession userSession, IAPIFactory apiFactory)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
        }

        #endregion Construction

        #region Views

        public async Task<IActionResult> Index()
        {
            List<string> listType = new List<string>();
            foreach (string typeName in Enum.GetNames(typeof(EMenuType)))
            {
                listType.Add(typeName);
            }

            var menuGroups = await GetMenuGroups();

            var language = await ReadLangDefault();
            ViewBag.Titles = listType;
            ViewBag.Language = language;
            ViewBag.MenuGroups = menuGroups.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name });
            
            return View();
        }

        public async Task<IActionResult> _FormLang(int id, string languageCode = null)
        {
            var languages = await _apiFactory.GetAsync<List<LanguageViewModel>>("Language/Read", Endpoints.ApiCore, _userSession.BearerToken);
            languageCode = languageCode ?? languages.Where(x => x.IsDefault == true).First().Code;

            ViewBag.Languages = languages.Select(x => new SelectListItem() { Value = x.Code, Text = x.Name });

            var model = await _apiFactory.GetAsync<MenuViewModel>(this.ApiResources($"ReadByLanguage?languagecode={languageCode}&id={id}"), Endpoints.ApiCore, _userSession.BearerToken);

            return PartialView(model);
        }

        public async Task<IActionResult> _TagHelperBody(int menuGroupId)
        {
            var model = await GetMenus(menuGroupId);

            return PartialView(model);
        }

        public async Task<IActionResult> _Post(int menuGroupId)
        {
            var posts = await GetPosts(menuGroupId);

            return PartialView(posts);
        }

        public async Task<IActionResult> _Page(int menuGroupId)
        {
            var pages = await GetPages(menuGroupId);

            return PartialView(pages);
        }

        public async Task<IActionResult> _Category(int menuGroupId)
        {
            var categories = await GetCategories(menuGroupId);

            return PartialView(categories);
        }

        public async Task<IActionResult> _Custom(int menuGroupId)
        {
            var menus = await GetMenus(menuGroupId);
            var menuGroups = await GetMenuGroups();
            ViewBag.MenuGroups = menuGroups.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Menus = menus.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.NavigationLabel });

            return PartialView();
        }

        #endregion Views

        #region GET
        [HttpGet]
        public async Task<MenuViewModel> ReadById(int id)
        {
            var menu = await _apiFactory.GetAsync<MenuViewModel>($"Menu/ReadById?id={id}", Endpoints.ApiCore);
            return menu;
        }

        #endregion GET

        #region POST

        [HttpPost]
        public async Task<ResponseContainer> Create(string json)
        {
            List<MenuViewModel> models = new List<MenuViewModel>();
            var result = JsonConvert.DeserializeObject<List<MenuJson>>(json);

            foreach (var item in result)
            {
                models.Add(new MenuViewModel()
                {
                    NavigationLabel = item.Label,
                    Type = item.Type,
                    ReferenceId = item.ReferenceId,
                    URL = string.IsNullOrWhiteSpace(item.Url) ? "#" : item.Url,
                    LanguageCode = item.LanguageCode,
                    MenuGroupId = item.MenuGroupId,
                    IsInMenu = item.IsInMenu
                });
            }

            var resultC = await _apiFactory.PostAsync<List<MenuViewModel>, int>(
                models, 
                this.ApiResources("Create"), 
                Endpoints.ApiCore, 
                _userSession.BearerToken
            );

            return new ResponseContainer
            {
                Action = "create",
                Activity = "Thêm mới",
                Succeeded = resultC > 0 ? true : false
            };
        }

        [HttpPost]
        public async Task<ResponseContainer> CreateCustom(MenuViewModel model)
        {
            var lang = await ReadLangDefault();

            model.Type = 3;
            model.LanguageCode = lang.Code;

            var resultC = await _apiFactory.PostAsync<MenuViewModel, int>(
                model,
                this.ApiResources("CreateCustom"),
                Endpoints.ApiCore,
                _userSession.BearerToken
            );

            return new ResponseContainer
            {
                Action = "create",
                Activity = "Thêm mới",
                Succeeded = resultC > 0 ? true : false
            };
        }

        #endregion POST

        #region PUT

        [HttpPut]
        public async Task<ResponseContainer> Update(MenuViewModel model)
        {
            var result = await _apiFactory.PutAsync<MenuViewModel, int>(model, "Menu/Update", Endpoints.ApiCore, _userSession.BearerToken);

            return new ResponseContainer
            {
                Action = "update",
                Activity = "Điều chỉnh",
                Succeeded = result > 0 ? true : false,
            };
        }

        [HttpPut]
        public async Task<ResponseContainer> UpdateTrans(MenuViewModel model)
        {
            var transModel = new MenuTranslationViewModel
            {
                MenuId = model.Id,
                NavigationLabel = model.NavigationLabel,
                LanguageCode = model.LanguageCode,
                URL = model.URL
            };

            var result = await _apiFactory.PutAsync<MenuTranslationViewModel, int>(transModel, this.ApiResources("UpdateTranslation"), Endpoints.ApiCore, _userSession.BearerToken);

            return new ResponseContainer
            {
                Action = "update",
                Activity = "Điều chỉnh ngôn ngữ",
                Succeeded = result > 0 ? true : false,
            };
        }

        #endregion PUT

        #region DELETE

        [HttpPost]
        public async Task<ResponseContainer> Delete(int id)
        {
            var result = await _apiFactory.DeleteAsync<bool>($"Menu/Delete?id={id}", Endpoints.ApiCore, _userSession.BearerToken);

            return new ResponseContainer
            {
                Action = "delete",
                Activity = "Xóa",
                Succeeded = result,
            };
        }

        #endregion DELETE

        #region SORT

        [HttpPost]
        public async Task<ResponseContainer> Sort(string json)
        {
            var Json = new JsonData { Data = json };

            var result = await _apiFactory.PostAsync<JsonData, int>(Json, "Menu/Sort", Endpoints.ApiCore, _userSession.BearerToken);

            return new ResponseContainer
            {
                Action = "update",
                Activity = "Sắp xếp phòng ban",
                Succeeded = result > 0 ? true : false,
            };

        }

        #endregion SORT

        #region Remote

        public async Task<JsonResult> CheckMenuGroup_IsExisting(int id)
        {
            var existingMenuGroup = await _apiFactory.GetAsync<MenuGroupViewModel>("MenuGroup/ReadById?Id=" + id, Endpoints.ApiCore, _userSession.BearerToken);

            if (existingMenuGroup.Id == 0)
                return Json(false);
            return Json(true);
        }

        public async Task<JsonResult> CheckMenu_IsExisting(int id)
        {
            if (id == 0)
                return Json(true);

            var existingMenu = await ReadById(id);

            if (existingMenu.Id == 0)
                return Json(false);
            return Json(true);
        }

        #endregion Remote

        #region METHOD

        public async Task<List<MenuViewModel>> GetCategories(int menuGroupId)
        {
            return await _apiFactory
                    .GetAsync<List<MenuViewModel>>(
                        this.ApiResources($"ReadCategory?menuGroupId={menuGroupId}"),
                        Endpoints.ApiCore,
                        _userSession.BearerToken
                    );
        } 

        public async Task<List<MenuViewModel>> GetPages(int menuGroupId)
        {
            return await _apiFactory
                    .GetAsync<List<MenuViewModel>>(
                        this.ApiResources($"ReadPage?menuGroupId={menuGroupId}"),
                        Endpoints.ApiCore,
                        _userSession.BearerToken
                    );
        }

        public async Task<List<MenuViewModel>> GetPosts(int menuGroupId)
        {
            return await _apiFactory
                .GetAsync<List<MenuViewModel>>(
                    this.ApiResources($"ReadPost?menuGroupId={menuGroupId}"),
                    Endpoints.ApiCore,
                    _userSession.BearerToken
                );
        }

        public async Task<List<MenuGroupViewModel>> GetMenuGroups()
        {
            return await _apiFactory.GetAsync<List<MenuGroupViewModel>>(
                $"MenuGroup/Read?pageSize={Int32.MaxValue}&skip={0}&searchBy={"NULL"}",
                Endpoints.ApiCore,
                _userSession.BearerToken
                );
        }

        public async Task<List<MenuViewModel>> GetMenus(int menuGroupId = 0)
        {
            var language = await ReadLangDefault();

            return await _apiFactory
                .GetAsync<List<MenuViewModel>>(
                    $"Menu/Read?languageCode={language.Code}&menuGroupId={menuGroupId}",
                    Endpoints.ApiCore,
                    _userSession.BearerToken
                );
        }

        public async Task<LanguageViewModel> ReadLangDefault()
        {
            var languages = await _apiFactory.GetAsync<List<LanguageViewModel>>("Language/Read", Endpoints.ApiCore, _userSession.BearerToken);
            var filted = languages.Where(x => x.IsDefault == true).First();
            return filted;
        }

        #endregion METHOD
    }
}