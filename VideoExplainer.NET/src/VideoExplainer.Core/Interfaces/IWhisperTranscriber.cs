/// <summary>
/// Interface for audio transcription with word-level timestamps using Whisper.
/// Migrated from: src/audio/transcribe.py
/// 
/// ⚠️ IMPLEMENTATION STATUS: MOCK ONLY
/// 
/// The Python implementation uses OpenAI's Whisper model for speech-to-text:
/// - openai-whisper package
/// - faster-whisper for improved performance
/// - Models: tiny, base, small, medium, large
/// - Provides word-level timestamps for precise synchronization
/// 
/// .NET Blockers:
/// - No native PyTorch runtime in .NET
/// - Whisper models are PyTorch-based
/// - While ONNX exports exist, they often lack word-level timestamp support
/// 
/// Future Implementation Options:
/// 1. Python microservice via HTTP/gRPC (recommended)
///    - Host Whisper as a separate Python service
///    - Call via REST API from .NET
///    - Maintains full word-level timestamp functionality
///    - Example: whisper-api (github.com/schibsted/whisper-api)
/// 
/// 2. ONNX Runtime with Whisper.NET
///    - Use Whisper.net NuGet package (experimental)
///    - Limited word-level timestamp support
///    - May need custom post-processing
/// 
/// 3. Azure Speech Services
///    - Use Azure.AI.Speech NuGet package
///    - Provides word-level timestamps natively
///    - Enterprise-grade solution with good .NET support
///    - Requires Azure subscription
/// 
/// 4. AssemblyAI API
///    - Use AssemblyAI .NET SDK
///    - Excellent word-level timestamps
///    - Simple REST API integration
///    - Paid service but competitive pricing
/// 
/// See: src/audio/transcribe.py for original Python implementation
/// </summary>
namespace VideoExplainer.Core.Interfaces;

/// <summary>
/// Result of audio transcription operation.
/// </summary>
/// <param name="Text">Full transcribed text</param>
/// <param name="WordTimestamps">Word-level timestamps</param>
/// <param name="DurationSeconds">Audio duration in seconds</param>
/// <param name="Language">Detected language code</param>
public record TranscriptionResult(
    string Text,
    List<WordTimestamp> WordTimestamps,
    double DurationSeconds,
    string Language = "en"
);

/// <summary>
/// Audio transcriber using Whisper for word-level speech-to-text.
/// ⚠️ Currently only mock implementation available - see class documentation for migration options.
/// </summary>
public interface IWhisperTranscriber
{
    /// <summary>
    /// Transcribe audio file and extract word-level timestamps.
    /// This is used for manual voiceover recording workflows where users
    /// record their own narration and need it synchronized with visuals.
    /// </summary>
    /// <param name="audioPath">Path to audio file (mp3, wav, etc.)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Transcription result with text and word timestamps</returns>
    Task<TranscriptionResult> TranscribeAsync(
        string audioPath,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Get audio duration using ffprobe or similar tool.
    /// </summary>
    /// <param name="audioPath">Path to audio file</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Duration in seconds</returns>
    Task<double> GetAudioDurationAsync(
        string audioPath,
        CancellationToken cancellationToken = default
    );
}
