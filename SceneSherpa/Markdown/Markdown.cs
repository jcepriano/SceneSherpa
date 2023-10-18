using Markdig;

using Microsoft.AspNetCore.Html;
using Westwind.Web.Markdown.MarkdownParser;
public static class Markdown
{
    public static string Parse(string markdown)
    {
        var builder = new MarkdownPipelineBuilder();
        builder.Extensions.Insert(0, new SpoilerContainerExtension());
        builder.UseAbbreviations()
            .UseAutoIdentifiers()
            .UseCitations()
            .UseDefinitionLists()
            .UseEmphasisExtras()
            .UseFigures()
            .UseFooters()
            .UseFootnotes()
            .UseGridTables()
            .UseMathematics()
            .UseMediaLinks()
            .UsePipeTables()
            .UseListExtras()
            .UseTaskLists()
            .UseDiagrams()
            .UseAutoLinks()
            .UseGenericAttributes();
        var mdPipeline = builder.Build();
        if(markdown != null)
        {
            return Markdig.Markdown.ToHtml(markdown, mdPipeline);
        }
        return markdown;
    }

    public static HtmlString ParseHtmlString(string markdown)
    {
        return new HtmlString(Parse(markdown));
    }
}