/// <summary>
/// Interface for audio processing operations using FFmpeg.
/// Migrated from: src/audio/ (various audio utilities)
/// </summary>
namespace VideoExplainer.Core.Interfaces;

/// <summary>
/// Audio processor for mixing, converting, and manipulating audio files.
/// Uses FFmpeg as the underlying engine.
/// </summary>
public interface IAudioProcessor
{
    /// <summary>
    /// Mix multiple audio tracks together.
    /// </summary>
    /// <param name="inputPaths">List of input audio file paths</param>
    /// <param name="outputPath">Output path for mixed audio</param>
    /// <param name="volumes">Volume levels for each input (0.0 to 1.0)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Path to mixed audio file</returns>
    Task<string> MixAudioAsync(
        List<string> inputPaths,
        string outputPath,
        List<double>? volumes = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Convert audio file to different format.
    /// </summary>
    /// <param name="inputPath">Input audio file path</param>
    /// <param name="outputPath">Output path with desired extension</param>
    /// <param name="bitrate">Target bitrate (e.g., "192k")</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Path to converted audio file</returns>
    Task<string> ConvertAudioAsync(
        string inputPath,
        string outputPath,
        string bitrate = "192k",
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Get audio file duration using ffprobe.
    /// </summary>
    /// <param name="audioPath">Path to audio file</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Duration in seconds</returns>
    Task<double> GetDurationAsync(
        string audioPath,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Apply fade in/out effects to audio.
    /// </summary>
    /// <param name="inputPath">Input audio file path</param>
    /// <param name="outputPath">Output path for processed audio</param>
    /// <param name="fadeInSeconds">Fade in duration in seconds</param>
    /// <param name="fadeOutSeconds">Fade out duration in seconds</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Path to processed audio file</returns>
    Task<string> ApplyFadeAsync(
        string inputPath,
        string outputPath,
        double fadeInSeconds = 0,
        double fadeOutSeconds = 0,
        CancellationToken cancellationToken = default
    );
}
