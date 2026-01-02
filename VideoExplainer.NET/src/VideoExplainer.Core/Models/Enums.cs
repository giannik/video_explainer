/// <summary>
/// Enumerations used throughout the Video Explainer application.
/// Migrated from: src/models.py
/// </summary>
namespace VideoExplainer.Core.Models;

/// <summary>
/// Type of source document for video content.
/// </summary>
public enum SourceType
{
    /// <summary>
    /// Markdown document (.md)
    /// </summary>
    Markdown,

    /// <summary>
    /// PDF document
    /// </summary>
    Pdf,

    /// <summary>
    /// Web URL
    /// </summary>
    Url,

    /// <summary>
    /// Plain text
    /// </summary>
    Text
}

/// <summary>
/// Type of scene in the video script.
/// </summary>
public enum SceneType
{
    /// <summary>
    /// Opening hook to grab attention
    /// </summary>
    Hook,

    /// <summary>
    /// Context setting scene
    /// </summary>
    Context,

    /// <summary>
    /// Explanation of concept
    /// </summary>
    Explanation,

    /// <summary>
    /// Key insight or aha moment
    /// </summary>
    Insight,

    /// <summary>
    /// Conclusion and summary
    /// </summary>
    Conclusion
}

/// <summary>
/// Type of visual element in the scene.
/// </summary>
public enum VisualType
{
    /// <summary>
    /// Animated graphic
    /// </summary>
    Animation,

    /// <summary>
    /// Diagram or chart
    /// </summary>
    Diagram,

    /// <summary>
    /// Code snippet
    /// </summary>
    Code,

    /// <summary>
    /// Mathematical equation
    /// </summary>
    Equation,

    /// <summary>
    /// Static image
    /// </summary>
    Image,

    /// <summary>
    /// Text reveal or title card
    /// </summary>
    Text
}

/// <summary>
/// Visual potential level for a concept.
/// </summary>
public enum VisualPotential
{
    /// <summary>
    /// Low visual potential
    /// </summary>
    Low,

    /// <summary>
    /// Medium visual potential
    /// </summary>
    Medium,

    /// <summary>
    /// High visual potential
    /// </summary>
    High
}

/// <summary>
/// Type of animation element.
/// </summary>
public enum AnimationElementType
{
    /// <summary>
    /// Geometric shape
    /// </summary>
    Shape,

    /// <summary>
    /// Text element
    /// </summary>
    Text,

    /// <summary>
    /// Code block
    /// </summary>
    Code,

    /// <summary>
    /// Mathematical equation
    /// </summary>
    Equation,

    /// <summary>
    /// Image element
    /// </summary>
    Image
}
