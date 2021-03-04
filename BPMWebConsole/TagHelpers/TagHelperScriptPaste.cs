using BPMWebConsole.TagHelpers.Common;
using Lib.Log;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPMWebConsole.TagHelpers
{
    /// <summary>
    /// ASP腳本標籤"script"貼上處理類別
    /// </summary>
    [HtmlTargetElement("script", Attributes = "asp-paste-key")]
    public class TagHelperScriptPaste : TagHelper
    {
        #region =====[Public] Property=====

        /// <summary>
        /// 目標貼上之腳本標籤金鑰
        /// </summary>
        [HtmlAttributeName("asp-paste-key")]
        public string DeferDestinationId { get; set; }

        /// <summary>
        /// 此貼上之腳本標籤的文本內容
        /// </summary>
        [HtmlAttributeNotBound, ViewContext]
        public ViewContext ViewContext { get; set; }

        #endregion

        #region =====[Private] Variable=====

        /// <summary>
        /// 
        /// </summary>
        private IHtmlGenerator Generator { get; set; }

        #endregion

        #region =====[Public] Constructor & Destructor=====

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="generator"></param>
        public TagHelperScriptPaste(IHtmlGenerator generator)
        {
            Generator = generator;
        }

        #endregion

        #region =====[Public] Method=====

        /// <summary>
        /// 非同步執行ASP腳本標籤功能
        /// </summary>
        /// <param name="context">ASP腳本標籤文本</param>
        /// <param name="output">ASP腳本標籤文本(包含貼上屬性)</param>
        /// <returns>處理結果</returns>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            await base.ProcessAsync(context, output);

            if (ViewContext.HttpContext.Items.ContainsKey(TagHelperCutPaste.ItemStorageKey))
            {
                // Get list of script contents
                object storage = ViewContext.HttpContext.Items[TagHelperCutPaste.ItemStorageKey];

                List<TagHelperCutPaste> CutPastes = storage as List<TagHelperCutPaste>;
                if (CutPastes == null)
                {
                    // The key was found but conversion failed
                    ErrorLog.Log("TagHelperScriptPaste", new ApplicationException($"Conversion failed for item type {storage.GetType()} to type" + typeof(Dictionary<string, TagHelperCutPaste>)));
                }

                if (CutPastes.Count == 0)
                {
                    return;
                }
                // Get those items that match the key for this tag helper
                List<TagHelperCutPaste> ComponentsWithStorageKey = CutPastes.Where(x => x.CutPasteKey == DeferDestinationId).ToList();

                if (ComponentsWithStorageKey == null || ComponentsWithStorageKey.Count == 0)
                {
                    return;
                }

                // Render first item in place of this script
                TagHelperCutPaste FirstScript = ComponentsWithStorageKey.First();
                output.Content.SetHtmlContent(FirstScript.TagHelperContent.GetContent());
                foreach (TagHelperAttribute attr in FirstScript.Attributes)
                {
                    output.Attributes.Add(attr);
                }

                // Add the rest of script items after the current one
                if (ComponentsWithStorageKey.Count > 0)
                {
                    for (int i = 1; i < ComponentsWithStorageKey.Count; i++)
                    {
                        TagHelperCutPaste script = ComponentsWithStorageKey[i];
                        TagBuilder builder = new TagBuilder("script");
                        builder.MergeAttributes(script.Attributes.ToDictionary(x => x.Name, x => x.Value));
                        builder.InnerHtml.AppendHtml(script.TagHelperContent.GetContent());
                        output.PostElement.AppendHtml(builder);
                    }
                }
            }

            return;
        }

        #endregion
    }
}
