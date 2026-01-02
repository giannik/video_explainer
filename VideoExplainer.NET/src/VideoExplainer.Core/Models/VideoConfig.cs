/// <summary>
/// Video rendering configuration.
/// Migrated from: src/project/loader.py - VideoConfig
/// </summary>
namespace VideoExplainer.Core.Models;

/// <summary>
/// Video rendering configuration with resolution, FPS, and other settings.
/// </summary>
/// <param name="Width">Video width in pixels (default: 1920)</param>
/// <param name="Height">Video height in pixels (default: 1080)</param>
/// <param name="Fps">Frames per second (default: 30)</param>
/// <param name="TargetDurationSeconds">Target video duration in seconds (default: 180)</param>
/// <param name="Format">Video format (default: mp4)</param>
/// <param name="Codec">Video codec (default: h264)</param>
public record VideoConfig(
    int Width = 1920,
    int Height = 1080,
    int Fps = 30,
    int TargetDurationSeconds = 180,
    string Format = "mp4",
    string Codec = "h264"
);
