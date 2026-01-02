/// <summary>
/// Interface for rendering videos using Remotion.
/// Migrated from: src/animation/renderer.py
/// </summary>
namespace VideoExplainer.Core.Interfaces;

/// <summary>
/// Rendering options for video output.
/// </summary>
/// <param name="Width">Video width in pixels</param>
/// <param name="Height">Video height in pixels</param>
/// <param name="Fps">Frames per second</param>
/// <param name="Codec">Video codec (e.g., "h264")</param>
/// <param name="Quality">Quality preset (e.g., "medium", "high")</param>
public record RenderOptions(
    int Width = 1920,
    int Height = 1080,
    int Fps = 30,
    string Codec = "h264",
    string Quality = "medium"
);

/// <summary>
/// Result of video rendering operation.
/// </summary>
/// <param name="Success">Whether rendering succeeded</param>
/// <param name="OutputPath">Path to rendered video file</param>
/// <param name="DurationSeconds">Video duration in seconds</param>
/// <param name="ErrorMessage">Error message if rendering failed</param>
public record RenderResult(
    bool Success,
    string? OutputPath,
    double DurationSeconds,
    string? ErrorMessage
);

/// <summary>
/// Remotion renderer for executing Remotion video rendering.
/// Wraps calls to `npx remotion render` command.
/// </summary>
public interface IRemotionRenderer
{
    /// <summary>
    /// Render a video using Remotion.
    /// </summary>
    /// <param name="projectPath">Path to project directory containing storyboard.json</param>
    /// <param name="outputPath">Path where rendered video should be saved</param>
    /// <param name="options">Rendering options (resolution, FPS, etc.)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Render result</returns>
    Task<RenderResult> RenderAsync(
        string projectPath,
        string outputPath,
        RenderOptions? options = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Check if Remotion and Node.js are available.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if Remotion is available</returns>
    Task<bool> IsAvailableAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get version information for Remotion and Node.js.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Dictionary with version information</returns>
    Task<Dictionary<string, string>> GetVersionInfoAsync(
        CancellationToken cancellationToken = default
    );
}
