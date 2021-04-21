using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;

namespace BPMWebConsole.Factory.CustomAttribute
{
    /// <summary>
    /// 有標註"asp-for"屬性之"input"元素控制類別
    /// </summary>
    [HtmlTargetElement("input", Attributes = "asp-for", TagStructure = TagStructure.WithoutEndTag)]
    public class ShortenTagNameAttribute : InputTagHelper
    {
        #region =====[Public] Property=====

        /// <summary>
        /// 自定義此Html Tag之Name屬性名稱是否以縮短表示
        /// </summary>
        [HtmlAttributeName("asp-short-name")]
        public bool IsShortName { get; set; }

        #endregion

        #region =====[Public] Constructor & Destructor=====

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="generator">Tag Helper產生控制物件</param>
        public ShortenTagNameAttribute(IHtmlGenerator generator) : base(generator) { }

        #endregion

        #region =====[Public|Override] Method=====

        /// <summary>
        /// 縮短並替換Name屬性之內容值
        /// </summary>
        /// <param name="context">目前HTML Tag的內容資訊</param>
        /// <param name="output">經TagHelper產出之Html Tag內容</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (IsShortName)
            {
                string nameAttrValue = output.Attributes.Single(a => a.Name == "name").Value as string;
                output.Attributes.SetAttribute("name", nameAttrValue.Split('.').Last());
            }
            base.Process(context, output);
        }

        #endregion
    }
}
