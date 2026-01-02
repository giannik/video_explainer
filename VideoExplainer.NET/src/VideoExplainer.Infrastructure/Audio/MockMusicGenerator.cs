/// <summary>
/// Mock music generator that creates silent placeholder audio.
/// Migrated from: src/music/generator.py - MusicGenerator
/// 
/// ⚠️ THIS IS A MOCK IMPLEMENTATION
/// 
/// Python Implementation Details:
/// The original Python code uses Meta's MusicGen model for AI-powered background music generation.
/// - Model: facebook/musicgen-small/medium/large via HuggingFace Transformers
/// - Framework: PyTorch with CUDA/MPS support
/// - Functionality: Generates ambient music from text prompts
/// - Features: Multiple segments, crossfading, style presets
/// - Output: High-quality MP3 files with generated music
/// 
/// Why This Is Mocked:
/// 1. No Native PyTorch Support: .NET lacks a native PyTorch runtime
/// 2. No Transformers Library: HuggingFace Transformers is Python-only
/// 3. No ONNX Export: MusicGen doesn't have official ONNX model exports
/// 4. Model Complexity: Generative audio models require specialized ML infrastructure
/// 
/// Future Implementation Options:
/// 
/// OPTION 1: Python Microservice (Recommended for Production)
/// - Deploy original Python MusicGen as a separate service
/// - Expose via REST API or gRPC
/// - Call from .NET using HttpClient
/// - Pros: Full functionality, maintained Python code
/// - Cons: Additional deployment complexity
/// - Example:
///   ```
///   public async Task<MusicGenerationResult> GenerateAsync(...)
///   {
///       var response = await _httpClient.PostAsJsonAsync(
///           "http://music-service:5000/generate",
///           new { topic, duration, style }
///       );
///       return await response.Content.ReadFromJsonAsync<MusicGenerationResult>();
///   }
///   ```
/// 
/// OPTION 2: Third-Party Music API
/// - Use commercial music generation APIs:
///   * Mubert API (mubert.com/api) - AI music generation
///   * AIVA API (aiva.ai) - AI composer
///   * Soundraw API (soundraw.io) - Royalty-free AI music
/// - Pros: No infrastructure needed, professionally maintained
/// - Cons: Recurring costs, less control over output
/// - Example:
///   ```
///   public async Task<MusicGenerationResult> GenerateAsync(...)
///   {
///       var request = new MubertRequest
///       {
///           Mode = "track",
///           Tags = new[] { "ambient", "electronic", "tech" },
///           Duration = durationSeconds
///       };
///       return await _mubertClient.GenerateAsync(request);
///   }
///   ```
/// 
/// OPTION 3: Azure AI Services
/// - Deploy MusicGen to Azure Machine Learning
/// - Create managed endpoint
/// - Call via Azure.AI.ML SDK
/// - Pros: Enterprise-grade, scalable
/// - Cons: Azure-specific, higher cost
/// 
/// OPTION 4: ONNX Runtime (Future)
/// - Wait for official MusicGen ONNX export
/// - Use Microsoft.ML.OnnxRuntime
/// - Pros: Native .NET execution
/// - Cons: Requires ONNX support (not yet available)
/// 
/// Current Mock Behavior:
/// This implementation generates silent MP3 files as placeholders.
/// Duration and file structure match what the real implementation would produce.
/// </summary>
namespace VideoExplainer.Infrastructure.Audio;

using System.Diagnostics;
using VideoExplainer.Core.Interfaces;

/// <summary>
/// Mock music generator that creates silent placeholder audio files.
/// See class documentation for implementation options.
/// </summary>
public class MockMusicGenerator : IMusicGenerator
{
    /// <summary>
    /// Music style presets for different video topics.
    /// </summary>
    private static readonly Dictionary<string, string> StylePresets = new()
    {
        ["tech"] = "ambient electronic, subtle synthesizers, no vocals, professional tech documentary, modern, clean",
        ["science"] = "ambient electronic, ethereal pads, no vocals, science documentary, wonder, discovery",
        ["tutorial"] = "lo-fi beats, calm, no vocals, background music, focused, minimal",
        ["dramatic"] = "cinematic ambient, building tension, no vocals, documentary score, epic subtle",
        ["default"] = "ambient electronic, subtle, no vocals, professional documentary background music"
    };

    /// <summary>
    /// Generate placeholder background music.
    /// Creates silent MP3 file with appropriate duration.
    /// </summary>
    public async Task<MusicGenerationResult> GenerateAsync(
        string outputPath,
        string topic,
        int durationSeconds,
        string? customStyle = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var prompt = GetMusicPrompt(topic, customStyle);

            Console.WriteLine($"⚠️  MockMusicGenerator: Generating {durationSeconds}s of silent audio");
            Console.WriteLine($"   Topic: {topic}");
            Console.WriteLine($"   Style: {prompt}");
            Console.WriteLine($"   Note: Using mock implementation - see MockMusicGenerator.cs for production options");

            // Generate silent audio file
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
            await GenerateSilentAudioAsync(outputPath, durationSeconds, cancellationToken);

            return new MusicGenerationResult(
                Success: true,
                OutputPath: outputPath,
                DurationSeconds: durationSeconds,
                PromptUsed: prompt,
                SegmentsGenerated: 1,
                ErrorMessage: null
            );
        }
        catch (Exception ex)
        {
            return new MusicGenerationResult(
                Success: false,
                OutputPath: null,
                DurationSeconds: 0,
                PromptUsed: GetMusicPrompt(topic, customStyle),
                SegmentsGenerated: 0,
                ErrorMessage: ex.Message
            );
        }
    }

    /// <summary>
    /// Get music style prompt based on video topic.
    /// </summary>
    public string GetMusicPrompt(string topic, string? customStyle = null)
    {
        if (!string.IsNullOrEmpty(customStyle))
        {
            return customStyle;
        }

        var topicLower = topic.ToLower();

        // Determine best preset based on topic keywords
        if (ContainsAny(topicLower, "llm", "ai", "machine learning", "neural", "gpu", "inference"))
        {
            return StylePresets["tech"];
        }
        else if (ContainsAny(topicLower, "science", "physics", "biology", "chemistry", "research"))
        {
            return StylePresets["science"];
        }
        else if (ContainsAny(topicLower, "tutorial", "how to", "guide", "learn"))
        {
            return StylePresets["tutorial"];
        }
        else if (ContainsAny(topicLower, "dramatic", "impact", "revolution", "breakthrough"))
        {
            return StylePresets["dramatic"];
        }

        return StylePresets["default"];
    }

    private bool ContainsAny(string text, params string[] keywords)
    {
        return keywords.Any(keyword => text.Contains(keyword));
    }

    /// <summary>
    /// Generate silent audio file using FFmpeg.
    /// </summary>
    private async Task GenerateSilentAudioAsync(
        string outputPath,
        int durationSeconds,
        CancellationToken cancellationToken)
    {
        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-y -f lavfi -i anullsrc=r=32000:cl=mono:d={durationSeconds} -c:a libmp3lame -b:a 192k \"{outputPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(startInfo);
            if (process != null)
            {
                await process.WaitForExitAsync(cancellationToken);
                
                if (process.ExitCode != 0 || !File.Exists(outputPath))
                {
                    // Fallback to placeholder file
                    await File.WriteAllBytesAsync(outputPath, new byte[1000], cancellationToken);
                }
            }
        }
        catch
        {
            // FFmpeg not available - create placeholder
            await File.WriteAllBytesAsync(outputPath, new byte[1000], cancellationToken);
        }
    }
}
