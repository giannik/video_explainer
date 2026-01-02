/// <summary>
/// A scene in the video script with narration and visual cues.
/// Migrated from: src/models.py - ScriptScene
/// </summary>
namespace VideoExplainer.Core.Models;

/// <summary>
/// A single scene in the video script containing voiceover text and visual direction.
/// </summary>
/// <param name="SceneId">Unique scene identifier</param>
/// <param name="SceneType">Type of scene (Hook, Context, Explanation, etc.)</param>
/// <param name="Title">Scene title</param>
/// <param name="Voiceover">Narration text to be spoken</param>
/// <param name="VisualCue">Visual direction for this scene</param>
/// <param name="DurationSeconds">Expected duration in seconds</param>
/// <param name="Notes">Additional production notes</param>
public record ScriptScene(
    int SceneId,
    SceneType SceneType,
    string Title,
    string Voiceover,
    VisualCue VisualCue,
    double DurationSeconds,
    string Notes = ""
)
{
    public ScriptScene() : this(0, SceneType.Explanation, string.Empty,
        string.Empty, new VisualCue(), 0, string.Empty) { }
}
