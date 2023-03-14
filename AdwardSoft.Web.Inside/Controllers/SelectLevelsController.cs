using System.Collections.Generic;
using System.Threading.Tasks;
using AdwardSoft.Provider.API;
using AdwardSoft.Provider.Common;
using AdwardSoft.Security.Authorization;
using AdwardSoft.Web.Inside.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdwardSoft.Web.Inside.Controllers
{
    [AdsAuthorization]
    public class SelectLevelsController : Controller
    {
        private IUserSession _userSession;
        private IAPIFactory _apiFactory;
        public SelectLevelsController(IUserSession userSession, IAPIFactory apiFactory)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
        }

        [HttpGet]
        public async Task<List<SelectLevels>> Department()
        {
            var response = await _apiFactory.GetAsync<List<SelectLevels>>("Department/ReadSelect", Endpoints.ApiPOS, _userSession.BearerToken);
            return response;
        }

        [HttpGet]
        public async Task<List<SelectLevels>> Branch()
        {
            var response = await _apiFactory.GetAsync<List<SelectLevels>>("Branch/ReadSelect", Endpoints.ApiPOS, _userSession.BearerToken);
            return response;
        }

        [HttpGet]
        public async Task<List<SelectLevels>> BranchByUser()
        {
            var response = await _apiFactory.GetAsync<List<SelectLevels>>("Branch/ReadSelectUser?userId=" + _userSession.UserId, Endpoints.ApiPOS, _userSession.BearerToken);
            return response;
        }

        [HttpGet]
        public async Task<List<Select>> Category()
        {
            var response = await _apiFactory.GetAsync<List<Select>>("Category/ReadSelect", Endpoints.ApiPOS, _userSession.BearerToken);
            return response;
        }

        [HttpGet]
        public async Task<List<Select>> Module()
        {
            var response = await _apiFactory.GetAsync<List<Select>>("Module/ReadSelect", Endpoints.ApiCore, _userSession.BearerToken);
            return response;
        }
    }
}