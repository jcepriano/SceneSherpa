using Markdig.Extensions.CustomContainers;
using Markdig.Parsers.Inlines;
using Markdig.Renderers;
using Markdig;

public class SpoilerContainerExtension : IMarkdownExtension
{
    public void Setup(MarkdownPipelineBuilder pipeline)
    {
        if (!pipeline.BlockParsers.Contains<CustomContainerParser>())
        {
            // Insert the parser before any other parsers
            pipeline.BlockParsers.Insert(0, new CustomContainerParser());
        }

        // Plug the inline parser for CustomContainerInline
        var inlineParser = pipeline.InlineParsers.Find<EmphasisInlineParser>();
        if (inlineParser != null && !inlineParser.HasEmphasisChar(':'))
        {
            inlineParser.EmphasisDescriptors.Add(new EmphasisDescriptor(':', 2, 2, true));
            inlineParser.TryCreateEmphasisInlineList.Add((emphasisChar, delimiterCount) =>
            {
                if (delimiterCount == 2 && emphasisChar == ':')
                {
                    return new CustomContainerInline();
                }
                return null;
            });
        }
    }

    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
    {
        if (renderer is HtmlRenderer htmlRenderer)
        {
            if (!htmlRenderer.ObjectRenderers.Contains<SpoilerContainerRenderer>())
            {
                // Must be inserted before CodeBlockRenderer
                htmlRenderer.ObjectRenderers.Insert(0, new SpoilerContainerRenderer());
            }
            if (!htmlRenderer.ObjectRenderers.Contains<SpoilerContainerInlineRenderer>())
            {
                // Must be inserted before EmphasisRenderer
                htmlRenderer.ObjectRenderers.Insert(0, new SpoilerContainerInlineRenderer());
            }
        }
    }
}