using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AdwardSoft.Provider.API;
using AdwardSoft.Provider.Common;
using AdwardSoft.Security.Authorization;
using AdwardSoft.Web.Inside.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace AdwardSoft.Web.Inside.Pages
{
    [AdsAuthorization]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class StatusCode : PageModel
    {
        private readonly ILogger _logger;
        private IAPIFactory _apiFactory;
        private readonly IUserSession _userSession;

        public string RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string ErrorStatusCode { get; set; }

        public string OriginalURL { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool ShowOriginalURL => !string.IsNullOrEmpty(OriginalURL);

        public StatusCode(ILogger<StatusCode> logger, IAPIFactory apiFactory, IUserSession userSession)
        {
            _logger = logger;
            _apiFactory = apiFactory;
            _userSession = userSession;
        }
        public async System.Threading.Tasks.Task OnGet(string code)
        {
            var lstLang = await _apiFactory.GetAsync<List<LanguageViewModel>>("Language/Read", Endpoints.ApiCore, _userSession.BearerToken);
            var languageCode = "vi";
            if (lstLang != null && lstLang.Count() > 0)
            {
                languageCode = lstLang.Where(x => x.IsDefault == true).FirstOrDefault().Code;
            }
            var model = await _apiFactory.GetAsync<HandleErrorViewModel>("HandleError/ReadByCode?languageCode=" + languageCode + "&code="+ code, HostConstants.ApiCore, _userSession.BearerToken);
            Title = !String.IsNullOrEmpty(model.Title) ? model.Title : "Có lỗi xảy ra !";
            Message = !String.IsNullOrEmpty(model.Message) ? model.Message : "Không thể truy cập trang mong muốn !";
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            ErrorStatusCode = code;

            #region snippet_StatusCodeReExecute
            var statusCodeReExecuteFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            if (statusCodeReExecuteFeature != null)
            {
                OriginalURL =
                    statusCodeReExecuteFeature.OriginalPathBase
                    + statusCodeReExecuteFeature.OriginalPath
                    + statusCodeReExecuteFeature.OriginalQueryString;
            }
            #endregion
        }
    }
}
