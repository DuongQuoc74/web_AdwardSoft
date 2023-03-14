using AdwardSoft.Web.Inside.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdwardSoft.Web.Inside.TagHelpers
{
    [HtmlTargetElement("branch-menu", Attributes = AttributeData)]
    public class BranchTagHelper : TagHelper
    {
        private const string AttributeData = "data";
        private const string AttributeUrlForm = "url_form";
        private const string AttributeShowDetail = "showdetail";

        [HtmlAttributeName(AttributeData)]
        public List<BranchViewModel> DataSource { get; set; }

        [HtmlAttributeName(AttributeUrlForm)]
        public string URL_Form { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "Branch-menu";
            output.TagMode = TagMode.StartTagAndEndTag;

            List<Nestable> data = new List<Nestable>();
            data = DataSource.Select(x => new Nestable()
            {
                Id = x.Id,
                Title = x.Name,
                ParentId = x.ParentId
            }).ToList();

            string contentTag = GenerateStringHtml(data);

            output.Content.SetHtmlContent(contentTag);
        }

        private string GenerateStringHtml(List<Nestable> data)
        {
            var content = new StringBuilder();
            content.AppendFormat(@"<div class='mn-tree'><div class='mn' id='{0}' form='{1}'>", "menutable", URL_Form);
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
                    content.AppendFormat(@"<li class='mn-item' data-id='{0}'><div class='mn-handle'><span>{1}</span></div><button data-action='detail' type='button'></button><div class='mn-handle-details' data-id='handleCollapse-{0}'></div>", childs[i].Id, childs[i].Title);
                    data.Remove(childs[i]);
                    GenerateChildHtml(content, data, childs[i].Id);
                    content.Append(@"</li>");

                }
                content.Append(@"</ol>");
            }

        }
    }
}
