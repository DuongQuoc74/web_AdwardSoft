using AdwardSoft.Web.Inside.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.TagHelpers
{
    [HtmlTargetElement("menu-menu", Attributes = AttributeData)]
    public class MenuTagHelper : TagHelper
    {
        private const string AttributeData = "data";
        private const string AttributeUrlForm = "url_form";
        private const string AttributeUrlFormLang = "url_form_lang";

        [HtmlAttributeName(AttributeData)]
        public List<MenuViewModel> dataSource { get; set; }

        [HtmlAttributeName(AttributeUrlForm)]
        public string URL_Form { get; set; }

        [HtmlAttributeName(AttributeUrlFormLang)]
        public string URL_Form_Lang { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "menu-menu";
            output.TagMode = TagMode.StartTagAndEndTag;

            List<Nestable> data = new List<Nestable>();
            data = dataSource.Select(x => new Nestable()
            {
                Id = x.Id,
                Title = x.NavigationLabel,
                ParentId = x.ParentId
            }).ToList();

            string contentTag = GenerateStringHtml(data);

            output.Content.SetHtmlContent(contentTag);
            //base.Process(context, output);
        }

        private string GenerateStringHtml(List<Nestable> data)
        {
            var content = new StringBuilder();
            content.AppendFormat(@"<div class='mn-tree'><div class='mn' id='{0}' form='{1}' form-lang='{2}'>", "menutable", URL_Form, URL_Form_Lang);
            GenerateChildHtml(content, data, null);
            content.AppendLine("</div></div>");

            return content.ToString();
        }

        private void GenerateChildHtml(StringBuilder content, List<Nestable> data, int? parentId)
        {
            var childs = (parentId == null ? data.Where(x => x.ParentId == x.Id) : data.Where(x => x.ParentId == parentId)).ToList();
            if (childs != null && childs.Count() > 0)
            {
                content.Append(@"<ol class='mn-list'>");
                for (int i = childs.Count - 1; i >= 0; i--)
                {
                    content.AppendFormat(@"
                        <li class='mn-item' data-id='{0}'>
                            <div class='mn-handle'>
                                <span>{1}</span>
                            </div>
                            <button data-action='detail' type='button'></button>
                            <div class='mn-handle-details' data-id='handleCollapse-{0}'>
                            </div>", childs[i].Id, childs[i].Title);
                    data.Remove(childs[i]);
                    GenerateChildHtml(content, data, childs[i].Id);
                    content.Append(@"</li>");

                }
                content.Append(@"</ol>");
            }

        }
    }
}
