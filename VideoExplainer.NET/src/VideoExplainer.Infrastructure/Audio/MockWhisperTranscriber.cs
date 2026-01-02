/// <summary>
/// Mock Whisper transcriber that simulates word-level timestamps.
/// Migrated from: src/audio/transcribe.py - WhisperTranscriber, FasterWhisperTranscriber
/// 
/// ⚠️ THIS IS A MOCK IMPLEMENTATION
/// 
/// Python Implementation Details:
/// The original Python code uses OpenAI's Whisper model for speech-to-text transcription.
/// - Models: openai-whisper (tiny, base, small, medium, large)
/// - Alternative: faster-whisper for improved performance
/// - Framework: PyTorch with CUDA/MPS support
/// - Functionality: Transcribes audio with word-level timestamps
/// - Features: Multi-language support, speaker diarization, confidence scores
/// - Output: Full transcript with precise word timings for video synchronization
/// 
/// Why This Is Mocked:
/// 1. No Native PyTorch: Whisper models are PyTorch-based, no native .NET runtime
/// 2. ONNX Limitations: While Whisper ONNX exports exist, they often lack word-level timestamps
/// 3. Model Size: Models range from 75MB (tiny) to 3GB (large)
/// 4. Specialized Processing: Requires audio preprocessing and post-processing pipelines
/// 
/// Future Implementation Options:
/// 
/// OPTION 1: Python Microservice (Recommended)
/// - Host original Whisper/faster-whisper as HTTP service
/// - Use existing Python packages for stability
/// - Call from .NET via HttpClient
/// - Pros: Full functionality, proven reliability, word-level timestamps
/// - Cons: Additional service deployment
/// - Example:
///   ```
///   public async Task<TranscriptionResult> TranscribeAsync(string audioPath, ...)
///   {
///       using var audioStream = File.OpenRead(audioPath);
///       using var content = new MultipartFormDataContent();
///       content.Add(new StreamContent(audioStream), "audio", Path.GetFileName(audioPath));
///       
///       var response = await _httpClient.PostAsync("http://whisper-service:8000/transcribe", content);
///       return await response.Content.ReadFromJsonAsync<TranscriptionResult>();
///   }
///   ```
/// - Existing Services: whisper-api (github.com/schibsted/whisper-api)
/// 
/// OPTION 2: Azure Speech Services (Best for Enterprise)
/// - Use Azure.AI.Speech NuGet package
/// - Full .NET integration with official Microsoft support
/// - Provides word-level timestamps natively
/// - Excellent accuracy and performance
/// - Pros: Native .NET, enterprise support, no infrastructure
/// - Cons: Azure dependency, recurring costs
/// - Example:
///   ```csharp
///   using Azure.AI.Speech;
///   
///   public async Task<TranscriptionResult> TranscribeAsync(string audioPath, ...)
///   {
///       var config = SpeechConfig.FromSubscription(apiKey, region);
///       using var audioConfig = AudioConfig.FromWavFileInput(audioPath);
///       using var recognizer = new SpeechRecognizer(config, audioConfig);
///       
///       var result = await recognizer.RecognizeOnceAsync();
///       var wordTimestamps = result.GetDetailedResults()
///           .SelectMany(r => r.Words.Select(w => new WordTimestamp(
///               w.Word, 
///               w.Offset.TotalSeconds, 
///               (w.Offset + w.Duration).TotalSeconds
///           )))
///           .ToList();
///           
///       return new TranscriptionResult(result.Text, wordTimestamps, ...);
///   }
///   ```
/// 
/// OPTION 3: AssemblyAI API
/// - Use AssemblyAI .NET SDK
/// - Simple REST API integration
/// - Excellent word-level timestamps
/// - Competitive pricing
/// - Pros: Easy integration, good accuracy, reasonable cost
/// - Cons: Third-party dependency
/// - Example:
///   ```csharp
///   using AssemblyAI;
///   
///   public async Task<TranscriptionResult> TranscribeAsync(string audioPath, ...)
///   {
///       var client = new AssemblyAIClient(apiKey);
///       var transcript = await client.Transcripts.TranscribeAsync(
///           new TranscriptParams
///           {
///               AudioUrl = await UploadAudioAsync(audioPath),
///               WordTimestamps = true
///           }
///       );
///       
///       return new TranscriptionResult(
///           transcript.Text,
///           transcript.Words.Select(w => new WordTimestamp(
///               w.Text, w.Start / 1000.0, w.End / 1000.0
///           )).ToList(),
///           transcript.AudioDuration
///       );
///   }
///   ```
/// 
/// OPTION 4: Whisper.NET (Experimental)
/// - Use whisper.net NuGet package (community-maintained)
/// - ONNX-based Whisper models
/// - Limited word-level timestamp support
/// - Pros: Native .NET, no external dependencies
/// - Cons: Experimental, may lack features
/// - Note: Check package maturity before production use
/// 
/// Current Mock Behavior:
/// This implementation uses FFprobe to get audio duration and generates
/// fake word timestamps based on text analysis. Useful for testing the
/// synchronization logic without actual transcription.
/// </summary>
namespace VideoExplainer.Infrastructure.Audio;

using System.Diagnostics;
using VideoExplainer.Core.Interfaces;

/// <summary>
/// Mock Whisper transcriber that generates placeholder transcriptions.
/// See class documentation for production implementation options.
/// </summary>
public class MockWhisperTranscriber : IWhisperTranscriber
{
    /// <summary>
    /// Transcribe audio file - returns mock transcription with simulated timestamps.
    /// Useful for testing audio synchronization logic.
    /// </summary>
    public async Task<TranscriptionResult> TranscribeAsync(
        string audioPath,
        CancellationToken cancellationToken = default)
    {
        if (!File.Exists(audioPath))
        {
            throw new FileNotFoundException($"Audio file not found: {audioPath}");
        }

        Console.WriteLine($"⚠️  MockWhisperTranscriber: Processing {Path.GetFileName(audioPath)}");
        Console.WriteLine($"   Note: Using mock implementation - see MockWhisperTranscriber.cs for production options");

        // Get actual audio duration
        var duration = await GetAudioDurationAsync(audioPath, cancellationToken);

        // Generate mock transcript based on duration
        var mockText = GenerateMockTranscript(duration);

        // Generate simulated word timestamps
        var words = mockText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var wordTimestamps = new List<WordTimestamp>();
        var currentTime = 0.0;
        var avgWordDuration = duration / Math.Max(words.Length, 1);

        foreach (var word in words)
        {
            var wordDuration = avgWordDuration * (0.8 + 0.4 * word.Length / 10.0);
            wordTimestamps.Add(new WordTimestamp(
                Word: word,
                StartSeconds: currentTime,
                EndSeconds: currentTime + wordDuration
            ));
            currentTime += wordDuration + 0.05;
        }

        return new TranscriptionResult(
            Text: mockText,
            WordTimestamps: wordTimestamps,
            DurationSeconds: duration,
            Language: "en"
        );
    }

    /// <summary>
    /// Get audio duration using ffprobe.
    /// </summary>
    public async Task<double> GetAudioDurationAsync(
        string audioPath,
        CancellationToken cancellationToken = default)
    {
        if (!File.Exists(audioPath))
        {
            throw new FileNotFoundException($"Audio file not found: {audioPath}");
        }

        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "ffprobe",
                Arguments = $"-v error -show_entries format=duration -of default=noprint_wrappers=1:nokey=1 \"{audioPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(startInfo);
            if (process != null)
            {
                var output = await process.StandardOutput.ReadToEndAsync(cancellationToken);
                await process.WaitForExitAsync(cancellationToken);

                if (process.ExitCode == 0 && double.TryParse(output.Trim(), out var duration))
                {
                    return duration;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Warning: ffprobe failed: {ex.Message}");
        }

        // Fallback: estimate from file size
        var fileInfo = new FileInfo(audioPath);
        // Rough estimate: ~16KB per second for MP3 at 128kbps
        return fileInfo.Length / 16000.0;
    }

    /// <summary>
    /// Generate mock transcript based on audio duration.
    /// </summary>
    private string GenerateMockTranscript(double durationSeconds)
    {
        // Generate approximately 150 words per minute
        var wordsNeeded = (int)(durationSeconds / 60.0 * 150);
        wordsNeeded = Math.Max(10, wordsNeeded); // At least 10 words

        var mockWords = new List<string>
        {
            "This", "is", "a", "mock", "transcription", "of", "the", "audio", "file",
            "that", "simulates", "what", "the", "actual", "speech", "to", "text",
            "output", "would", "look", "like", "with", "proper", "word", "level",
            "timestamps", "for", "video", "synchronization", "purposes"
        };

        var result = new List<string>();
        for (int i = 0; i < wordsNeeded; i++)
        {
            result.Add(mockWords[i % mockWords.Count]);
        }

        return string.Join(" ", result);
    }
}
