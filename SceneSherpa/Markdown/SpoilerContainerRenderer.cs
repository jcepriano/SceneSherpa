using Markdig.Extensions.CustomContainers;
using Markdig.Renderers.Html;
using Markdig.Renderers;

public class SpoilerContainerRenderer : HtmlObjectRenderer<CustomContainer>
{
    protected override void Write(HtmlRenderer renderer, CustomContainer obj)
    {
        renderer.EnsureLine();
        if (string.IsNullOrWhiteSpace(obj.Info))
        {
            var attr = obj.GetAttributes();
            attr.AddClass("spoiler");
            obj.SetAttributes(attr);
        }
        if (renderer.EnableHtmlForBlock)
        {
            renderer.Write("<div").WriteAttributes(obj).Write('>');
        }
        // We don't escape a CustomContainer
        renderer.WriteChildren(obj);
        if (renderer.EnableHtmlForBlock)
        {
            renderer.WriteLine("</div>");
        }
    }
}