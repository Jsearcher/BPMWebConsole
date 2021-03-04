using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;

namespace BPMWebConsole.TagHelpers.Common
{
    /// <summary>
    /// ASP腳本標籤小幫手剪貼參數類別
    /// </summary>
    /// <remarks>非同步剪貼</remarks>
    public class TagHelperCutPaste
    {
        /// <summary>
        /// 項目儲存金鑰
        /// </summary>
        public const string ItemStorageKey = "a2b459c4-3c62-4a90-977a-5999eb5978c5";

        /// <summary>
        /// 腳本標籤剪貼對應之金鑰
        /// </summary>
        public string CutPasteKey { get; set; }

        /// <summary>
        /// 剪下之腳本標籤的內容
        /// </summary>
        public TagHelperContent TagHelperContent { get; set; }

        /// <summary>
        /// 剪下之腳本的屬性集合列表
        /// </summary>
        public List<TagHelperAttribute> Attributes { get; set; }
    }
}
