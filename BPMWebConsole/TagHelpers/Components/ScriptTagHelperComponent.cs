using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Threading.Tasks;

namespace BPMWebConsole.TagHelpers.Components
{
    public class ScriptTagHelperComponent : TagHelperComponent
    {
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (string.Equals(context.TagName, "script", StringComparison.OrdinalIgnoreCase))
            {
                output.PostContent.AppendHtml("<script>console.log('script tag');</script>");
            }

            return Task.CompletedTask;
        }
    }
}
