using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Lib.Log;
using BPMWebConsole.TagHelpers.Common;

namespace BPMWebConsole.TagHelpers
{
    /// <summary>
    /// ASP腳本標籤"script"剪下處理類別
    /// </summary>
    [HtmlTargetElement("script", Attributes = "asp-cut-key")]
    public class TagHelperScriptCut : TagHelper
    {
        #region =====[Public] Property=====

        /// <summary>
        /// 腳本標籤剪下使用之金鑰
        /// </summary>
        [HtmlAttributeName("asp-cut-key")]
        public string CutKey { get; set; }

        /// <summary>
        /// 此剪下之腳本標籤的文本內容
        /// </summary>
        [HtmlAttributeNotBound, ViewContext]
        public ViewContext ViewContext { get; set; }

        #endregion

        #region =====[Public] Method=====

        /// <summary>
        /// 非同步執行ASP腳本標籤功能
        /// </summary>
        /// <param name="context">ASP腳本標籤文本</param>
        /// <param name="output">ASP腳本標籤文本(去除剪下屬性)</param>
        /// <returns>處理結果</returns>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            List<TagHelperCutPaste> DeferredScripts = new List<TagHelperCutPaste>();

            if (ViewContext.HttpContext.Items.ContainsKey(TagHelperCutPaste.ItemStorageKey))
            {
                DeferredScripts = ViewContext.HttpContext.Items[TagHelperCutPaste.ItemStorageKey] as List<TagHelperCutPaste>;
                if (DeferredScripts == null)
                {
                    // Conversion failed
                    ErrorLog.Log("TagHelperScriptCut", new ApplicationException("Duplicate Items key: " + TagHelperCutPaste.ItemStorageKey));
                }
            }
            else
            {
                DeferredScripts = new List<TagHelperCutPaste>();
                ViewContext.HttpContext.Items.Add(TagHelperCutPaste.ItemStorageKey, DeferredScripts);
            }

            // Solve content
            TagHelperContent result = await output.GetChildContentAsync();

            // Add content to the dictionary
            DeferredScripts.Add(new TagHelperCutPaste
            {
                CutPasteKey = CutKey,
                TagHelperContent = result,
                Attributes = context.AllAttributes.Where(x => x.Name != "asp-cut-key").ToList() // Pass the attributes
            });

            // Disable this tag
            output.TagName = "!script";

            // Do not render content in this section
            output.Content.Clear();
            return;
        }

        #endregion
    }
}
