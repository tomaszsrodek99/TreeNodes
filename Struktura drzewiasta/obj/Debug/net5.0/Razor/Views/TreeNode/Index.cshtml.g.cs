#pragma checksum "F:\Programowanie\Struktura drzewiasta\Struktura drzewiasta\Views\TreeNode\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "95b144faa8ccb5577383df35c1002ab7bba18496"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_TreeNode_Index), @"mvc.1.0.view", @"/Views/TreeNode/Index.cshtml")]
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
#line 1 "F:\Programowanie\Struktura drzewiasta\Struktura drzewiasta\Views\_ViewImports.cshtml"
using Struktura_drzewiasta;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "F:\Programowanie\Struktura drzewiasta\Struktura drzewiasta\Views\_ViewImports.cshtml"
using Struktura_drzewiasta.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"95b144faa8ccb5577383df35c1002ab7bba18496", @"/Views/TreeNode/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"716282a6b91bf676a01cacb024c27adcf7a0467c", @"/Views/_ViewImports.cshtml")]
    public class Views_TreeNode_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<TreeNode>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "F:\Programowanie\Struktura drzewiasta\Struktura drzewiasta\Views\TreeNode\Index.cshtml"
  
    ViewData["Title"] = "Tree";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h2>Widok drzewa</h2>\r\n<div class=\"button-container\">\r\n    <button class=\"btn\" id=\"toggleButton\" onclick=\"toggleAllNodes()\">Rozwiń wszystkie</button>\r\n    ");
#nullable restore
#line 9 "F:\Programowanie\Struktura drzewiasta\Struktura drzewiasta\Views\TreeNode\Index.cshtml"
Write(Html.ActionLink("Odwróć kierunek sortowania", "Reverse", "TreeNode", null, new { @class = "btn my-button" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    ");
#nullable restore
#line 10 "F:\Programowanie\Struktura drzewiasta\Struktura drzewiasta\Views\TreeNode\Index.cshtml"
Write(Html.ActionLink("Dodaj węzeł", "Create", "TreeNode", null, new { @class = "btn my-button" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n</div>\r\n<div class=\"tree-container\">\r\n    <div class=\"tree\">\r\n        <ul class=\"custom-list\" id=\"tree-structure\">\r\n");
#nullable restore
#line 15 "F:\Programowanie\Struktura drzewiasta\Struktura drzewiasta\Views\TreeNode\Index.cshtml"
             foreach (var node in Model)
            {
                if (node.ParentId == null) // Węzeł na pierwszym poziomie
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <li class=\"collapsed\">\r\n                        <span class=\"tree-node\" onclick=\"showChildren(this)\" data-node-id=\"");
#nullable restore
#line 20 "F:\Programowanie\Struktura drzewiasta\Struktura drzewiasta\Views\TreeNode\Index.cshtml"
                                                                                      Write(node.Id);

#line default
#line hidden
#nullable disable
            WriteLiteral("\" >\r\n                            <i class=\"fas fa-solid fa-folder\"></i> ");
#nullable restore
#line 21 "F:\Programowanie\Struktura drzewiasta\Struktura drzewiasta\Views\TreeNode\Index.cshtml"
                                                              Write(node.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        </span>\r\n");
#nullable restore
#line 23 "F:\Programowanie\Struktura drzewiasta\Struktura drzewiasta\Views\TreeNode\Index.cshtml"
                         if (node.Children.Count > 0)
                        {
                            

#line default
#line hidden
#nullable disable
#nullable restore
#line 25 "F:\Programowanie\Struktura drzewiasta\Struktura drzewiasta\Views\TreeNode\Index.cshtml"
                       Write(await Html.PartialAsync("_TreeNode", node.Children));

#line default
#line hidden
#nullable disable
#nullable restore
#line 25 "F:\Programowanie\Struktura drzewiasta\Struktura drzewiasta\Views\TreeNode\Index.cshtml"
                                                                                // Przekaż dzieci węzła do częsciowego widoku wywoływanego rekurencyjnie
                        }

#line default
#line hidden
#nullable disable
            WriteLiteral("                    </li>\r\n");
#nullable restore
#line 28 "F:\Programowanie\Struktura drzewiasta\Struktura drzewiasta\Views\TreeNode\Index.cshtml"
                }
            }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"        </ul>
    </div>
</div>
<div id=""context-menu"" class=""context-menu text-center"">
    <div class=""context-menu-item"">
        <a onclick=""createNode(event)"" class=""context-menu-link"">Dodaj</a>
    </div>
    <div class=""context-menu-item"">
        <a onclick=""editNode(event)"" class=""context-menu-link"">Edytuj</a>
    </div>
    <div class=""context-menu-item"">
        <a onclick=""deleteNode(event)"" class=""context-menu-link"">Usuń</a>
    </div>
</div>





");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<TreeNode>> Html { get; private set; }
    }
}
#pragma warning restore 1591
