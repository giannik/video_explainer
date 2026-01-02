/// <summary>
/// Storyboard scene with detailed visual specifications.
/// Migrated from: src/models.py - AnimationElement, StoryboardScene, Storyboard
/// </summary>
namespace VideoExplainer.Core.Models;

/// <summary>
/// An element in an animation with appearance timing and properties.
/// </summary>
/// <param name="Id">Unique element identifier</param>
/// <param name="ElementType">Type of element (Shape, Text, Code, etc.)</param>
/// <param name="Properties">Element-specific properties as key-value pairs</param>
/// <param name="AppearAt">Time in seconds when element should appear</param>
/// <param name="Animation">Animation type (e.g., "fade_in", "slide_in")</param>
public record AnimationElement(
    string Id,
    AnimationElementType ElementType,
    Dictionary<string, object> Properties,
    double AppearAt = 0.0,
    string Animation = "fade_in"
)
{
    public AnimationElement() : this(string.Empty, AnimationElementType.Shape,
        new Dictionary<string, object>(), 0.0, "fade_in") { }
}

/// <summary>
/// A scene in the storyboard with complete visual and audio specifications.
/// </summary>
/// <param name="SceneId">Unique scene identifier</param>
/// <param name="TimestampStart">Start time in seconds</param>
/// <param name="TimestampEnd">End time in seconds</param>
/// <param name="VoiceoverText">Narration text for this scene</param>
/// <param name="VisualType">Type of visual (Animation, Diagram, etc.)</param>
/// <param name="VisualDescription">Text description of the visual</param>
/// <param name="Elements">List of animation elements</param>
/// <param name="Transitions">Transition effects as key-value pairs</param>
/// <param name="AudioPath">Path to audio file (optional)</param>
public record StoryboardScene(
    int SceneId,
    double TimestampStart,
    double TimestampEnd,
    string VoiceoverText,
    string VisualType,
    string VisualDescription,
    List<AnimationElement> Elements,
    Dictionary<string, string> Transitions,
    string? AudioPath = null
)
{
    public StoryboardScene() : this(0, 0, 0, string.Empty, string.Empty,
        string.Empty, new List<AnimationElement>(),
        new Dictionary<string, string>(), null) { }
}

/// <summary>
/// Complete storyboard with all scenes and style guide.
/// </summary>
/// <param name="Title">Storyboard title</param>
/// <param name="Scenes">List of storyboard scenes</param>
/// <param name="StyleGuide">Style guide settings as key-value pairs</param>
/// <param name="TotalDurationSeconds">Total duration of the storyboard</param>
public record Storyboard(
    string Title,
    List<StoryboardScene> Scenes,
    Dictionary<string, object> StyleGuide,
    double TotalDurationSeconds
)
{
    public Storyboard() : this(string.Empty, new List<StoryboardScene>(),
        new Dictionary<string, object>(), 0) { }
}
