using AdwardSoft.Provider.API;
using AdwardSoft.Provider.Common;
using AdwardSoft.Web.Inside.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.ViewComponents
{
    public class HorizontalMenuViewComponent : ViewComponent
    {
        private IUserSession _userSession;
        private IAPIFactory _apiFactory;

        public HorizontalMenuViewComponent(IUserSession userSession, IAPIFactory apiFactory)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _apiFactory.GetAsync<List<ModuleViewModel>>("Module/ReadByUser?UserId=" + _userSession.UserId, HostConstants.ApiCore, _userSession.BearerToken);
            return View(model);
        }
    }
}
