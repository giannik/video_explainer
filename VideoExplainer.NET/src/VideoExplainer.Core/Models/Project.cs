/// <summary>
/// Complete video project state and configuration.
/// Migrated from: src/models.py - VideoProject
/// </summary>
namespace VideoExplainer.Core.Models;

/// <summary>
/// Complete video project containing all assets and metadata.
/// This is the main domain model representing a video creation project.
/// </summary>
/// <param name="ProjectId">Unique project identifier</param>
/// <param name="Title">Project title</param>
/// <param name="Description">Project description</param>
/// <param name="SourcePath">Path to source document</param>
/// <param name="ParsedDocument">Parsed document (optional)</param>
/// <param name="ContentAnalysis">Content analysis (optional)</param>
/// <param name="Script">Generated script (optional)</param>
/// <param name="Storyboard">Generated storyboard (optional)</param>
/// <param name="OutputPath">Path to output video (optional)</param>
/// <param name="Status">Project status (initialized, parsed, analyzed, etc.)</param>
/// <param name="VideoConfig">Video rendering configuration</param>
public record Project(
    string ProjectId,
    string Title,
    string Description,
    string SourcePath,
    ParsedDocument? ParsedDocument,
    ContentAnalysis? ContentAnalysis,
    Script? Script,
    Storyboard? Storyboard,
    string? OutputPath,
    string Status,
    VideoConfig VideoConfig
)
{
    public Project() : this(
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        null,
        null,
        null,
        null,
        null,
        "initialized",
        new VideoConfig()
    ) { }
}

/// <summary>
/// Project status values.
/// </summary>
public static class ProjectStatus
{
    public const string Initialized = "initialized";
    public const string Parsed = "parsed";
    public const string Analyzed = "analyzed";
    public const string Scripted = "scripted";
    public const string Storyboarded = "storyboarded";
    public const string Rendered = "rendered";
}
