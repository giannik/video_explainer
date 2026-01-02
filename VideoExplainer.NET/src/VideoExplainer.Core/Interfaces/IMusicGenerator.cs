/// <summary>
/// Interface for AI-powered background music generation.
/// Migrated from: src/music/generator.py
/// 
/// ⚠️ IMPLEMENTATION STATUS: MOCK ONLY
/// 
/// The Python implementation uses Meta's MusicGen model via HuggingFace Transformers:
/// - facebook/musicgen-small (~300MB)
/// - facebook/musicgen-medium (~1.5GB)
/// - facebook/musicgen-large (~3.3GB)
/// 
/// .NET Blockers:
/// - No native PyTorch runtime in .NET
/// - No HuggingFace Transformers equivalent
/// - MusicGen has no official ONNX export
/// 
/// Future Implementation Options:
/// 1. Python microservice via HTTP/gRPC (recommended for production)
///    - Host the Python MusicGen service separately
///    - Call it via REST API or gRPC from .NET
///    - Maintains full functionality while keeping .NET architecture
/// 
/// 2. ONNX Runtime if MusicGen gets ONNX support
///    - Wait for official ONNX export from Meta
///    - Use Microsoft.ML.OnnxRuntime package
///    - Would enable native .NET execution
/// 
/// 3. Third-party music generation API
///    - Mubert API (mubert.com/api)
///    - AIVA API (aiva.ai)
///    - Soundraw API (soundraw.io)
///    - These provide REST APIs with similar functionality
/// 
/// 4. Azure AI custom model deployment
///    - Deploy MusicGen to Azure ML
///    - Call via Azure ML endpoints
///    - Good for enterprise scenarios
/// 
/// See: src/music/generator.py for original Python implementation
/// </summary>
namespace VideoExplainer.Core.Interfaces;

/// <summary>
/// Configuration for music generation.
/// </summary>
/// <param name="ModelSize">Model size: Small, Medium, or Large</param>
/// <param name="SegmentDurationSeconds">Duration of each generated segment (max ~30s)</param>
/// <param name="TargetDurationSeconds">Target total duration (will loop/extend)</param>
/// <param name="Style">Music style prompt (e.g., "ambient electronic, subtle, professional")</param>
/// <param name="Volume">Volume level from 0.0 to 1.0</param>
public record MusicConfig(
    string ModelSize = "small",
    int SegmentDurationSeconds = 30,
    int? TargetDurationSeconds = null,
    string Style = "ambient electronic, subtle, no vocals, professional tech documentary",
    double Volume = 0.3
);

/// <summary>
/// Result of music generation operation.
/// </summary>
/// <param name="Success">Whether generation succeeded</param>
/// <param name="OutputPath">Path to generated music file</param>
/// <param name="DurationSeconds">Actual duration of generated music</param>
/// <param name="PromptUsed">The style prompt that was used</param>
/// <param name="SegmentsGenerated">Number of segments generated</param>
/// <param name="ErrorMessage">Error message if generation failed</param>
public record MusicGenerationResult(
    bool Success,
    string? OutputPath,
    double DurationSeconds,
    string PromptUsed,
    int SegmentsGenerated,
    string? ErrorMessage
);

/// <summary>
/// AI-powered background music generator using generative models.
/// ⚠️ Currently only mock implementation available - see class documentation for migration options.
/// </summary>
public interface IMusicGenerator
{
    /// <summary>
    /// Generate background music for a video.
    /// </summary>
    /// <param name="outputPath">Path where music file should be saved</param>
    /// <param name="topic">Video topic to determine music style</param>
    /// <param name="durationSeconds">Target duration in seconds</param>
    /// <param name="customStyle">Optional custom style prompt override</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Music generation result</returns>
    Task<MusicGenerationResult> GenerateAsync(
        string outputPath,
        string topic,
        int durationSeconds,
        string? customStyle = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Get a music style prompt based on video topic.
    /// </summary>
    /// <param name="topic">Video topic</param>
    /// <param name="customStyle">Optional custom style override</param>
    /// <returns>Music style prompt</returns>
    string GetMusicPrompt(string topic, string? customStyle = null);
}
