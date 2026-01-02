/// <summary>
/// Visual cue annotation in a script scene.
/// Migrated from: src/models.py - VisualCue
/// </summary>
namespace VideoExplainer.Core.Models;

/// <summary>
/// A visual cue describing what should be shown on screen during narration.
/// </summary>
/// <param name="Description">Text description of the visual</param>
/// <param name="VisualType">Type of visual element (Animation, Diagram, Code, etc.)</param>
/// <param name="Elements">List of specific elements to show</param>
/// <param name="DurationSeconds">How long to show this visual (default: 5.0)</param>
public record VisualCue(
    string Description,
    VisualType VisualType,
    List<string> Elements,
    double DurationSeconds = 5.0
)
{
    public VisualCue() : this(string.Empty, VisualType.Animation,
        new List<string>(), 5.0) { }
}
