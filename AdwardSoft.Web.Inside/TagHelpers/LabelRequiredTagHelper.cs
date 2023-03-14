using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AdwardSoft.Web.Inside.TagHelpers
{
    [HtmlTargetElement("label", Attributes = "asp-for")]
    public class LabelRequiredTagHelper : LabelTagHelper
    {
        public LabelRequiredTagHelper(IHtmlGenerator generator)
            : base(generator) { }
        public override async Task ProcessAsync(TagHelperContext context,
            TagHelperOutput output)
        {
            await base.ProcessAsync(context, output);
            bool isBool = For.Metadata.ModelType.Name == "Boolean";
            if (isBool)
                return;
            if (For.Metadata.IsRequired)
            {
                var curentClass = output.Attributes["class"]?.Value?.ToString();
                if(curentClass!= null && curentClass.Contains("ads-control"))
                {
                    var sup = new TagBuilder("span class='text-danger'");
                    sup.InnerHtml.Append(" *");
                    output.Content.AppendHtml(sup);
                }
                
            }
        }
    }
}
