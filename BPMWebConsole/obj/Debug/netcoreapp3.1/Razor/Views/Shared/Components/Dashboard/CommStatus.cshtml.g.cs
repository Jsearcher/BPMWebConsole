#pragma checksum "D:\VS Code\Webs\BPMWebConsole\BPMWebConsole\Views\Shared\Components\Dashboard\CommStatus.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4e4479a10e1d54923b39f2293600802439cd40a3"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_Components_Dashboard_CommStatus), @"mvc.1.0.view", @"/Views/Shared/Components/Dashboard/CommStatus.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\VS Code\Webs\BPMWebConsole\BPMWebConsole\Views\_ViewImports.cshtml"
using BPMWebConsole;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\VS Code\Webs\BPMWebConsole\BPMWebConsole\Views\_ViewImports.cshtml"
using BPMWebConsole.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\VS Code\Webs\BPMWebConsole\BPMWebConsole\Views\_ViewImports.cshtml"
using Newtonsoft.Json;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\VS Code\Webs\BPMWebConsole\BPMWebConsole\Views\_ViewImports.cshtml"
using BPMWebConsole.TagHelpers;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4e4479a10e1d54923b39f2293600802439cd40a3", @"/Views/Shared/Components/Dashboard/CommStatus.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e878d076b7d40566fadf14c3a89fbd0e9e2ae031", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_Components_Dashboard_CommStatus : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<BPMWebConsole.Models.Source.CommStatus>>
    {
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n<div class=\"grid_area\">\r\n    <div class=\"table_setting\">\r\n        <div class=\"table_name\">");
#nullable restore
#line 5 "D:\VS Code\Webs\BPMWebConsole\BPMWebConsole\Views\Shared\Components\Dashboard\CommStatus.cshtml"
                           Write(ViewData["status_table_name"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n        <table>\r\n            <thead>\r\n                <tr>\r\n                    <th>狀態</th>\r\n                    <th>");
#nullable restore
#line 10 "D:\VS Code\Webs\BPMWebConsole\BPMWebConsole\Views\Shared\Components\Dashboard\CommStatus.cshtml"
                   Write(ViewData["status_column_name"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                </tr>\r\n            </thead>\r\n            <tbody>\r\n");
#nullable restore
#line 14 "D:\VS Code\Webs\BPMWebConsole\BPMWebConsole\Views\Shared\Components\Dashboard\CommStatus.cshtml"
                 foreach (BPMWebConsole.Models.Source.CommStatus item in Model)
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <tr>\r\n");
#nullable restore
#line 17 "D:\VS Code\Webs\BPMWebConsole\BPMWebConsole\Views\Shared\Components\Dashboard\CommStatus.cshtml"
                         if (@item.Status ?? false)
                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <td class=\"status active\">\r\n");
#nullable restore
#line 20 "D:\VS Code\Webs\BPMWebConsole\BPMWebConsole\Views\Shared\Components\Dashboard\CommStatus.cshtml"
                                 if ((int)item.Type == 1)
                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                    <i class=\"fa fa-cog\"></i>\r\n");
#nullable restore
#line 23 "D:\VS Code\Webs\BPMWebConsole\BPMWebConsole\Views\Shared\Components\Dashboard\CommStatus.cshtml"
                                }
                                else
                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                    <i class=\"fa fa-link\"></i>\r\n");
#nullable restore
#line 27 "D:\VS Code\Webs\BPMWebConsole\BPMWebConsole\Views\Shared\Components\Dashboard\CommStatus.cshtml"
                                }

#line default
#line hidden
#nullable disable
            WriteLiteral("                            </td>\r\n");
#nullable restore
#line 29 "D:\VS Code\Webs\BPMWebConsole\BPMWebConsole\Views\Shared\Components\Dashboard\CommStatus.cshtml"
                        }
                        else
                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <td class=\"status\">\r\n");
#nullable restore
#line 33 "D:\VS Code\Webs\BPMWebConsole\BPMWebConsole\Views\Shared\Components\Dashboard\CommStatus.cshtml"
                                 if ((int)item.Type == 1)
                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                    <i class=\"fa fa-cog\"></i>\r\n");
#nullable restore
#line 36 "D:\VS Code\Webs\BPMWebConsole\BPMWebConsole\Views\Shared\Components\Dashboard\CommStatus.cshtml"
                                }
                                else
                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                    <i class=\"fa fa-link\"></i>\r\n");
#nullable restore
#line 40 "D:\VS Code\Webs\BPMWebConsole\BPMWebConsole\Views\Shared\Components\Dashboard\CommStatus.cshtml"
                                }

#line default
#line hidden
#nullable disable
            WriteLiteral("                            </td>\r\n");
#nullable restore
#line 42 "D:\VS Code\Webs\BPMWebConsole\BPMWebConsole\Views\Shared\Components\Dashboard\CommStatus.cshtml"
                        }

#line default
#line hidden
#nullable disable
            WriteLiteral("                        <td>\r\n");
#nullable restore
#line 44 "D:\VS Code\Webs\BPMWebConsole\BPMWebConsole\Views\Shared\Components\Dashboard\CommStatus.cshtml"
                             if ((int)item.Type == 2)
                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "4e4479a10e1d54923b39f2293600802439cd40a38141", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "src", 2, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            AddHtmlAttributeValue("", 1782, "~/image/airline/GIF/", 1782, 20, true);
#nullable restore
#line 46 "D:\VS Code\Webs\BPMWebConsole\BPMWebConsole\Views\Shared\Components\Dashboard\CommStatus.cshtml"
AddHtmlAttributeValue("", 1802, item.Name_Code + ".gif", 1802, 26, false);

#line default
#line hidden
#nullable disable
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "alt", 4, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            AddHtmlAttributeValue("", 1835, "Airline", 1835, 7, true);
            AddHtmlAttributeValue(" ", 1842, "Brand", 1843, 6, true);
            AddHtmlAttributeValue(" ", 1848, "of", 1849, 3, true);
#nullable restore
#line 46 "D:\VS Code\Webs\BPMWebConsole\BPMWebConsole\Views\Shared\Components\Dashboard\CommStatus.cshtml"
AddHtmlAttributeValue(" ", 1851, item.Name_Code, 1852, 17, false);

#line default
#line hidden
#nullable disable
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
#nullable restore
#line 47 "D:\VS Code\Webs\BPMWebConsole\BPMWebConsole\Views\Shared\Components\Dashboard\CommStatus.cshtml"
                            }

#line default
#line hidden
#nullable disable
            WriteLiteral("                            ");
#nullable restore
#line 48 "D:\VS Code\Webs\BPMWebConsole\BPMWebConsole\Views\Shared\Components\Dashboard\CommStatus.cshtml"
                       Write(item.Name_Desc);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        </td>\r\n                    </tr>\r\n");
#nullable restore
#line 51 "D:\VS Code\Webs\BPMWebConsole\BPMWebConsole\Views\Shared\Components\Dashboard\CommStatus.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("            </tbody>\r\n        </table>\r\n    </div>\r\n</div>\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<BPMWebConsole.Models.Source.CommStatus>> Html { get; private set; }
    }
}
#pragma warning restore 1591