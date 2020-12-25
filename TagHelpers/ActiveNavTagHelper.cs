using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

[HtmlTargetElement("li", Attributes = "asp-controller")]
public class ActiveItemTagHelper : TagHelper
{
    [HtmlAttributeName("asp-controller")]
    public string Controller { get; set; }

    [HtmlAttributeName("asp-action")]
    public string Action { get; set; }

    [HtmlAttributeName("asp-class")]
    public string Class { get; set; } = "active";

    [ViewContext]
    public ViewContext ViewContext { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        string currentController = (string)ViewContext.RouteData.Values["controller"];
        string currentAction = (string)ViewContext.RouteData.Values["action"];
        if (currentController == Controller && currentAction == (Action ?? currentAction))
        {
            if (output.Attributes.ContainsName("class"))
                output.Attributes.SetAttribute("class", output.Attributes["class"].Value + " " + Class);
            else
                output.Attributes.Add("class", Class);
        }
    }
}
