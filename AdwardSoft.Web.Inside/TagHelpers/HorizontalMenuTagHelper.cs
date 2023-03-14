using AdwardSoft.Web.Inside.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.TagHelpers
{
    [HtmlTargetElement("ads-horizontal-menu")]
    public class HorizontalMenuTagHelper : TagHelper
    {
        private const string AttributeData = "data";
        private const string AttributeUrlForm = "url_form";
        protected HttpRequest Request => ViewContext.HttpContext.Request;
        protected HttpResponse Response => ViewContext.HttpContext.Response;

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName(AttributeData)]
        public List<ModuleViewModel> DataSource { get; set; }
        [HtmlAttributeName(AttributeUrlForm)]
        public string URL_Form { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "ads-horizontal-menu";
            output.TagMode = TagMode.StartTagAndEndTag;

            //List<MenuTagHelperModel> data = new List<MenuTagHelperModel>();
            //data = DataSource.Select(x => new MenuTagHelperModel()
            //{
            //    Id = x.Id,
            //    Title = x.Title,
            //    ParentId = x.ParentId
            //}).ToList();

            string contentTag = GenerateStringHtml(DataSource);

            output.Content.SetHtmlContent(contentTag);

        }

        private string GenerateStringHtml(List<ModuleViewModel> data)
        {
            var content = new StringBuilder();
            content.AppendFormat(@"<div class='navbar navbar-expand-md navbar-light'>
                                        <div class='text-center d-md-none w-100'>
                                            <button type = 'button' class='navbar-toggler dropdown-toggle' data-toggle='collapse' data-target='#navbar-navigation'>
                                                <i class='icon-unfold mr-2'></i>
                                                Navigation
                                            </button>
                                        </div>
                                        <div class='navbar-collapse collapse' id='navbar-navigation'><ul class='navbar-nav'>
                                             <li class='nav-item'>
                                                <a href='/' class='navbar-nav-link " + (Request.Path.Value == "/" ? "active" : "") + @" '>
                                                    <i class='icon-home4 mr-2 '></i>
                                                    Dashboard
                                                </a>
                                            </li>");
            GenerateChildHtml(content, data, null);
            content.AppendLine("</ul></div></div>");

            return content.ToString();
        }

        private void GenerateChildHtml(StringBuilder content, List<ModuleViewModel> data, int? parentId)
        {
            var childs = (parentId == null ? data.Where(x => x.ParentId == x.Id) : data.Where(x => x.ParentId == parentId)).ToList();

            if (childs != null && childs.Count() > 0)
            {
                for (int i = childs.Count - 1; i >= 0; i--)
                {
                    var activeLink = "";
                    if(Request.Path.Value.Replace(@"/","") == childs[i].Link)
                    {
                        activeLink = "active";
                    }
                    bool hasChild = false;

                    if (parentId == null)
                    {
                        content.AppendFormat(@"<li class='nav-item dropdown'>
                                                    <a href='/{0}' class='navbar-nav-link dropdown-toggle {3}' data-toggle='dropdown'>
                                                        <i class='{1} mr-2'></i>
                                                        {2}
                                                    </a>", childs[i].Link, childs[i].ClassName, childs[i].Title, activeLink);
                        if (data.Any(x => x.ParentId == childs[i].Id))
                        {
                            hasChild = true;
                            var lstChild = data.Where(x => x.ParentId == childs[i].Id && x.ParentId != x.Id);
                            int count = lstChild.Count() / 10 + (lstChild.Count()%10 > 0 ? 1 : 0);
                            if (count > 1)
                            {
                                int width = 180 * count;
                                content.AppendFormat("<div class='dropdown-menu' style='width:" + width + "px;'>");
                                content.AppendFormat(@"<div class='row'>");
                                for (int j = 0; j < count; j++)
                                {
                                    int skip = 10 * j;
                                    content.AppendFormat(@"<div class='col-auto' style='max-width: 180px;'>");
                                    GenerateChildHtml(content, lstChild.Skip(skip).Take(10).ToList(), childs[i].Id);
                                    content.AppendFormat(@"</div>");
                                }
                                content.AppendFormat(@"</div>");
                                data.Remove(childs[i]);

                                
                            }
                            else
                            {
                                content.AppendFormat(@"<div class='dropdown-menu'>");

                                data.Remove(childs[i]);

                                GenerateChildHtml(content, data, childs[i].Id);
                            }

                        }
                    }
                    else
                    {
                        content.AppendFormat(@"<a href='/{0}' class='dropdown-item {3}'><i class='{1}'></i> {2}</a>", childs[i].Link, childs[i].ClassName, childs[i].Title, activeLink);
                        data.Remove(childs[i]);

                        GenerateChildHtml(content, data, childs[i].Id);
                    }

                    if (parentId == null)
                    {
                        if (hasChild) content.Append("</div>");
                        content.Append("</li>");
                    }
                }
            }
        }
    }
}
