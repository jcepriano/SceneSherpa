using Markdig.Extensions.CustomContainers;
using Markdig.Renderers.Html;
using Markdig.Renderers;

public class SpoilerContainerInlineRenderer : HtmlObjectRenderer<CustomContainerInline>
{
    protected override void Write(HtmlRenderer renderer, CustomContainerInline obj)
    {
        var attr = obj.TryGetAttributes() ?? new();
        if ((attr.Classes?.Count ?? 0) == 0)
        {
            attr.AddClass("spoiler");
            obj.SetAttributes(attr);
        }
        renderer.Write("<span").WriteAttributes(obj).Write('>');
        renderer.WriteChildren(obj);
        renderer.Write("</span>");
    }
}