/// <summary>
/// Narration for a specific scene.
/// Migrated from: src/project/loader.py - SceneNarration
/// </summary>
namespace VideoExplainer.Core.Models;

/// <summary>
/// Narration script for a single scene with timing information.
/// </summary>
/// <param name="SceneId">Unique scene identifier</param>
/// <param name="Title">Scene title</param>
/// <param name="DurationSeconds">Expected duration in seconds</param>
/// <param name="Narration">The actual narration text to be spoken</param>
public record NarrationScene(
    string SceneId,
    string Title,
    int DurationSeconds,
    string Narration
)
{
    public NarrationScene() : this(string.Empty, string.Empty, 0, string.Empty) { }
}

/// <summary>
/// Complete narration document containing all scene narrations.
/// </summary>
/// <param name="Scenes">List of narration scenes</param>
/// <param name="TotalDurationSeconds">Total duration of all narrations</param>
public record Narration(
    List<NarrationScene> Scenes,
    double TotalDurationSeconds
)
{
    public Narration() : this(new List<NarrationScene>(), 0) { }
}
