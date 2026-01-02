/// <summary>
/// Interface for Text-to-Speech providers.
/// Implementations: ElevenLabsTtsProvider, AzureTtsProvider, MockTtsProvider
/// Migrated from: src/audio/tts.py
/// </summary>
namespace VideoExplainer.Core.Interfaces;

/// <summary>
/// Word-level timestamp for audio synchronization.
/// </summary>
/// <param name="Word">The spoken word</param>
/// <param name="StartSeconds">Start time in seconds</param>
/// <param name="EndSeconds">End time in seconds</param>
public record WordTimestamp(
    string Word,
    double StartSeconds,
    double EndSeconds
);

/// <summary>
/// Result of TTS generation including audio path and timing information.
/// </summary>
/// <param name="AudioPath">Path to generated audio file</param>
/// <param name="DurationSeconds">Total audio duration in seconds</param>
/// <param name="WordTimestamps">Word-level timestamps for synchronization</param>
public record TtsResult(
    string AudioPath,
    double DurationSeconds,
    List<WordTimestamp> WordTimestamps
);

/// <summary>
/// Text-to-Speech provider for generating voiceover audio with word-level timestamps.
/// </summary>
public interface ITtsProvider
{
    /// <summary>
    /// Generate speech from text and save to file.
    /// </summary>
    /// <param name="text">Text to convert to speech</param>
    /// <param name="outputPath">Path where audio file should be saved</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Path to generated audio file</returns>
    Task<string> GenerateAsync(
        string text,
        string outputPath,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Generate speech with word-level timestamps for precise synchronization.
    /// </summary>
    /// <param name="text">Text to convert to speech</param>
    /// <param name="outputPath">Path where audio file should be saved</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>TTS result with audio path, duration, and word timestamps</returns>
    Task<TtsResult> GenerateWithTimestampsAsync(
        string text,
        string outputPath,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Get list of available voices from the TTS provider.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of available voice information</returns>
    Task<List<VoiceInfo>> GetAvailableVoicesAsync(
        CancellationToken cancellationToken = default
    );
}

/// <summary>
/// Information about an available voice.
/// </summary>
/// <param name="VoiceId">Unique voice identifier</param>
/// <param name="Name">Human-readable voice name</param>
/// <param name="Category">Voice category (e.g., "male", "female", "neutral")</param>
/// <param name="Description">Voice description</param>
public record VoiceInfo(
    string VoiceId,
    string Name,
    string Category,
    string Description
);
