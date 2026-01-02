/// <summary>
/// Markdown document parser.
/// Migrated from: src/ingestion/markdown.py
/// </summary>
namespace VideoExplainer.Infrastructure.Parsing;

using System.Text.RegularExpressions;
using VideoExplainer.Core.Interfaces;
using VideoExplainer.Core.Models;

/// <summary>
/// Parser for Markdown documents with code block and heading extraction.
/// </summary>
public class MarkdownDocumentParser : IDocumentParser
{
    private static readonly string[] SupportedExtensions = { ".md", ".markdown" };

    /// <summary>
    /// Parse a markdown file from disk.
    /// </summary>
    public async Task<ParsedDocument> ParseFileAsync(
        string filePath,
        CancellationToken cancellationToken = default)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }

        var markdown = await File.ReadAllTextAsync(filePath, cancellationToken);
        return await ParseMarkdownAsync(markdown, filePath, cancellationToken);
    }

    /// <summary>
    /// Parse markdown content directly from string.
    /// </summary>
    public Task<ParsedDocument> ParseMarkdownAsync(
        string markdown,
        string sourcePath = "",
        CancellationToken cancellationToken = default)
    {
        var lines = markdown.Split('\n');
        var sections = new List<Section>();
        var currentSection = new Section();
        var contentBuilder = new List<string>();

        // Extract title (first H1)
        var title = "Untitled Document";
        var titleMatch = Regex.Match(markdown, @"^#\s+(.+)$", RegexOptions.Multiline);
        if (titleMatch.Success)
        {
            title = titleMatch.Groups[1].Value.Trim();
        }

        foreach (var line in lines)
        {
            var headingMatch = Regex.Match(line, @"^(#{1,6})\s+(.+)$");
            
            if (headingMatch.Success)
            {
                // Save previous section if it has content
                if (!string.IsNullOrEmpty(currentSection.Heading))
                {
                    currentSection = currentSection with { Content = string.Join("\n", contentBuilder) };
                    sections.Add(currentSection);
                    contentBuilder.Clear();
                }

                // Start new section
                var level = headingMatch.Groups[1].Value.Length;
                var heading = headingMatch.Groups[2].Value.Trim();
                currentSection = new Section(
                    Heading: heading,
                    Level: level,
                    Content: "",
                    CodeBlocks: new List<string>(),
                    Equations: new List<string>(),
                    Images: new List<string>()
                );
            }
            else
            {
                contentBuilder.Add(line);
            }
        }

        // Add final section
        if (!string.IsNullOrEmpty(currentSection.Heading))
        {
            currentSection = currentSection with { Content = string.Join("\n", contentBuilder) };
            sections.Add(currentSection);
        }

        // Extract code blocks, equations, and images for each section
        for (int i = 0; i < sections.Count; i++)
        {
            sections[i] = ExtractElements(sections[i]);
        }

        return Task.FromResult(new ParsedDocument(
            Title: title,
            SourceType: SourceType.Markdown,
            SourcePath: sourcePath,
            Sections: sections,
            RawContent: markdown,
            Metadata: new Dictionary<string, object>
            {
                ["section_count"] = sections.Count,
                ["word_count"] = markdown.Split(' ').Length
            }
        ));
    }

    /// <summary>
    /// Check if the parser supports a given file type.
    /// </summary>
    public bool SupportsFileType(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLowerInvariant();
        return SupportedExtensions.Contains(extension);
    }

    /// <summary>
    /// Get supported file extensions.
    /// </summary>
    public List<string> GetSupportedExtensions()
    {
        return SupportedExtensions.ToList();
    }

    /// <summary>
    /// Extract code blocks, equations, and images from section content.
    /// </summary>
    private Section ExtractElements(Section section)
    {
        var codeBlocks = new List<string>();
        var equations = new List<string>();
        var images = new List<string>();

        // Extract code blocks (```...```)
        var codeBlockMatches = Regex.Matches(section.Content, @"```[\s\S]*?```");
        foreach (Match match in codeBlockMatches)
        {
            codeBlocks.Add(match.Value.Trim('`').Trim());
        }

        // Extract inline code (`...`)
        var inlineCodeMatches = Regex.Matches(section.Content, @"`([^`]+)`");
        foreach (Match match in inlineCodeMatches)
        {
            if (!codeBlocks.Contains(match.Groups[1].Value))
            {
                codeBlocks.Add(match.Groups[1].Value);
            }
        }

        // Extract equations ($...$  or $$...$$)
        var equationMatches = Regex.Matches(section.Content, @"\$\$?[\s\S]*?\$\$?");
        foreach (Match match in equationMatches)
        {
            equations.Add(match.Value.Trim('$').Trim());
        }

        // Extract images (![alt](url))
        var imageMatches = Regex.Matches(section.Content, @"!\[([^\]]*)\]\(([^\)]+)\)");
        foreach (Match match in imageMatches)
        {
            images.Add(match.Groups[2].Value);
        }

        return section with
        {
            CodeBlocks = codeBlocks,
            Equations = equations,
            Images = images
        };
    }
}
