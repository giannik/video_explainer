/// <summary>
/// Interface for parsing source documents into structured format.
/// Migrated from: src/ingestion/parser.py, src/ingestion/markdown.py
/// </summary>
namespace VideoExplainer.Core.Interfaces;

using VideoExplainer.Core.Models;

/// <summary>
/// Document parser for extracting structured content from source files.
/// Supports Markdown, PDF, URLs, and plain text.
/// </summary>
public interface IDocumentParser
{
    /// <summary>
    /// Parse a document from file path.
    /// </summary>
    /// <param name="filePath">Path to the document file</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Parsed document with structured sections</returns>
    Task<ParsedDocument> ParseFileAsync(
        string filePath,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Parse markdown content directly from string.
    /// </summary>
    /// <param name="markdown">Markdown content</param>
    /// <param name="sourcePath">Optional source path for reference</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Parsed document with structured sections</returns>
    Task<ParsedDocument> ParseMarkdownAsync(
        string markdown,
        string sourcePath = "",
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Check if the parser supports a given file type.
    /// </summary>
    /// <param name="filePath">File path to check</param>
    /// <returns>True if file type is supported</returns>
    bool SupportsFileType(string filePath);

    /// <summary>
    /// Get supported file extensions.
    /// </summary>
    /// <returns>List of supported extensions (e.g., [".md", ".markdown"])</returns>
    List<string> GetSupportedExtensions();
}
