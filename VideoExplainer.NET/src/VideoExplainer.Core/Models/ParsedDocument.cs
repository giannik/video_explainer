/// <summary>
/// Parsed source document with sections and metadata.
/// Migrated from: src/models.py - ParsedDocument, Section
/// </summary>
namespace VideoExplainer.Core.Models;

/// <summary>
/// A section of the source document with heading, content, and extracted elements.
/// </summary>
/// <param name="Heading">Section heading text</param>
/// <param name="Level">Heading level (1-6)</param>
/// <param name="Content">Main content of the section</param>
/// <param name="CodeBlocks">List of code blocks found in this section</param>
/// <param name="Equations">List of mathematical equations</param>
/// <param name="Images">List of image references</param>
public record Section(
    string Heading,
    int Level,
    string Content,
    List<string> CodeBlocks,
    List<string> Equations,
    List<string> Images
)
{
    public Section() : this(string.Empty, 1, string.Empty, new List<string>(), new List<string>(), new List<string>()) { }
}

/// <summary>
/// A parsed source document containing structured content ready for script generation.
/// </summary>
/// <param name="Title">Document title</param>
/// <param name="SourceType">Type of source (Markdown, PDF, etc.)</param>
/// <param name="SourcePath">Path to the original source file</param>
/// <param name="Sections">List of parsed sections</param>
/// <param name="RawContent">Original raw content</param>
/// <param name="Metadata">Additional metadata about the document</param>
public record ParsedDocument(
    string Title,
    SourceType SourceType,
    string SourcePath,
    List<Section> Sections,
    string RawContent,
    Dictionary<string, object> Metadata
)
{
    public ParsedDocument() : this(string.Empty, SourceType.Markdown, string.Empty,
        new List<Section>(), string.Empty, new Dictionary<string, object>()) { }
}
