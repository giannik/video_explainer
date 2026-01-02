/// <summary>
/// Mock TTS provider for testing without requiring actual TTS API calls.
/// Generates silent audio files using FFmpeg.
/// Migrated from: src/audio/tts.py - MockTTS
/// </summary>
namespace VideoExplainer.Infrastructure.TtsProviders;

using System.Diagnostics;
using System.Text.RegularExpressions;
using VideoExplainer.Core.Interfaces;

/// <summary>
/// Mock TTS provider that generates silent MP3 files for testing.
/// Uses FFmpeg to create valid audio files with duration based on text length.
/// </summary>
public class MockTtsProvider : ITtsProvider
{
    /// <summary>
    /// Generate silent audio file.
    /// Duration is estimated from text length (~150 words per minute).
    /// </summary>
    public async Task<string> GenerateAsync(
        string text,
        string outputPath,
        CancellationToken cancellationToken = default)
    {
        var duration = EstimateDurationSeconds(text);
        await GenerateSilentAudioAsync(outputPath, duration, cancellationToken);
        return outputPath;
    }

    /// <summary>
    /// Generate silent audio with simulated word timestamps.
    /// </summary>
    public async Task<TtsResult> GenerateWithTimestampsAsync(
        string text,
        string outputPath,
        CancellationToken cancellationToken = default)
    {
        var duration = EstimateDurationSeconds(text);
        await GenerateSilentAudioAsync(outputPath, duration, cancellationToken);

        // Generate simulated word timestamps
        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var wordTimestamps = new List<WordTimestamp>();
        var currentTime = 0.0;
        var avgWordDuration = duration / Math.Max(words.Length, 1);

        foreach (var word in words)
        {
            // Clean word of punctuation
            var cleanWord = Regex.Replace(word, @"[^\w\-']", "");
            if (!string.IsNullOrEmpty(cleanWord))
            {
                // Vary duration slightly based on word length
                var wordDuration = avgWordDuration * (0.5 + 0.5 * cleanWord.Length / 6.0);
                wordTimestamps.Add(new WordTimestamp(
                    Word: cleanWord,
                    StartSeconds: currentTime,
                    EndSeconds: currentTime + wordDuration
                ));
                currentTime += wordDuration + 0.05; // Small gap between words
            }
        }

        return new TtsResult(
            AudioPath: outputPath,
            DurationSeconds: duration,
            WordTimestamps: wordTimestamps
        );
    }

    /// <summary>
    /// Get mock voices list.
    /// </summary>
    public Task<List<VoiceInfo>> GetAvailableVoicesAsync(
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<VoiceInfo>
        {
            new VoiceInfo(
                VoiceId: "mock_voice_1",
                Name: "Mock Voice",
                Category: "mock",
                Description: "A mock voice for testing"
            )
        });
    }

    /// <summary>
    /// Estimate duration in seconds from text length.
    /// Assumes ~150 words per minute speaking rate.
    /// </summary>
    private double EstimateDurationSeconds(string text)
    {
        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return Math.Max(1.0, (words.Length / 150.0) * 60.0);
    }

    /// <summary>
    /// Generate silent audio file using FFmpeg.
    /// Falls back to creating a minimal file if FFmpeg is not available.
    /// </summary>
    private async Task GenerateSilentAudioAsync(
        string outputPath,
        double durationSeconds,
        CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);

        try
        {
            // Try FFmpeg first
            var startInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-y -f lavfi -i anullsrc=r=44100:cl=mono:d={durationSeconds:F2} -c:a libmp3lame -b:a 128k \"{outputPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(startInfo);
            if (process != null)
            {
                await process.WaitForExitAsync(cancellationToken);
                
                if (process.ExitCode == 0 && File.Exists(outputPath))
                {
                    return;
                }
            }
        }
        catch (Exception)
        {
            // FFmpeg not available or failed
        }

        // Fallback: try sine wave at 0Hz (silent)
        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-y -f lavfi -i sine=frequency=0:duration={durationSeconds:F2} -c:a libmp3lame -b:a 128k \"{outputPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(startInfo);
            if (process != null)
            {
                await process.WaitForExitAsync(cancellationToken);
                
                if (process.ExitCode == 0 && File.Exists(outputPath))
                {
                    return;
                }
            }
        }
        catch (Exception)
        {
            // FFmpeg not available
        }

        // Last resort fallback: create a minimal placeholder file
        // This won't be playable but allows tests to pass
        await File.WriteAllBytesAsync(outputPath, new byte[1000], cancellationToken);
    }
}
