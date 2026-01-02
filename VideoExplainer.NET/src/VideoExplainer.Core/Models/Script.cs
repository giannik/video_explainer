/// <summary>
/// Complete video script with all scenes.
/// Migrated from: src/models.py - Script
/// </summary>
namespace VideoExplainer.Core.Models;

/// <summary>
/// The complete video script containing all scenes and metadata.
/// </summary>
/// <param name="Title">Script title</param>
/// <param name="TotalDurationSeconds">Total duration of all scenes</param>
/// <param name="Scenes">List of script scenes</param>
/// <param name="SourceDocument">Path to source document</param>
public record Script(
    string Title,
    double TotalDurationSeconds,
    List<ScriptScene> Scenes,
    string SourceDocument
)
{
    public Script() : this(string.Empty, 0, new List<ScriptScene>(), string.Empty) { }
}
