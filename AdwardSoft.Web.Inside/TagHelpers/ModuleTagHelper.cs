using AdwardSoft.Web.Inside.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdwardSoft.Web.Inside.TagHelpers
{
    [HtmlTargetElement("ads-module")]
    public class ModuleTagHelper : TagHelper
    {
        private const string AttributeData = "data";
        private const string AttributeUrlForm = "url_form";

        [HtmlAttributeName(AttributeData)]
        public List<ModuleViewModel> DataSource { get; set; }
        [HtmlAttributeName(AttributeUrlForm)]
        public string URL_Form { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "ads-module";
            output.TagMode = TagMode.StartTagAndEndTag;

            List<ModuleNestable> data = new List<ModuleNestable>();
            data = DataSource.Select(x => new ModuleNestable()
            {
                Id = x.Id,
                Title = x.Title,
                ParentId = x.ParentId,
                Types = x.Types == null ? "mtype-public" : x.Types.Replace(",", " mtype-")
            }).ToList();

            string contentTag = GenerateStringHtml(data);

            output.Content.SetHtmlContent(contentTag);
            
        }
        private string GenerateStringHtml(List<ModuleNestable> data)
        {
            var content = new StringBuilder();
            content.AppendFormat(@"<div class='mn-tree'><div class='mn' id='{0}' form='{1}'>", "menutable", URL_Form);
            GenerateChildHtml(content, data, null);
            content.AppendLine("</div></div>");

            return content.ToString();
        }

        private void GenerateChildHtml(StringBuilder content, List<ModuleNestable> data, int? parentId)
        {
            var childs = (parentId == null ? data.Where(x => x.ParentId == x.Id) : data.Where(x => x.ParentId == parentId)).ToList();
            if (childs != null && childs.Count() > 0)
            {
                content.Append(@"<ol class='mn-list'>");
                for (int i = childs.Count - 1; i >= 0; i--)
                {
                    content.AppendFormat(@"<li class='mn-item {2}' data-id='{0}'><div class='mn-handle'><span>{1}</span></div>
                        <button data-action='detail' type='button'></button><div class='mn-handle-details' style='overflow-y: auto;' data-id='handleCollapse-{0}'></div>"
                        , childs[i].Id, childs[i].Title, childs[i].Types);
                    data.Remove(childs[i]);
                    GenerateChildHtml(content, data, childs[i].Id);
                    content.Append(@"</li>");
                }
                content.Append(@"</ol>");
            }

        }
    }
}
