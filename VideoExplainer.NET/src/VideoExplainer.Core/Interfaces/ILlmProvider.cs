/// <summary>
/// Interface for Large Language Model providers.
/// Implementations: AnthropicLlmProvider, OpenAiLlmProvider, MockLlmProvider
/// Migrated from: src/understanding/llm_provider.py
/// </summary>
namespace VideoExplainer.Core.Interfaces;

using VideoExplainer.Core.Models;

/// <summary>
/// LLM provider for generating content analysis, scripts, and other text-based content.
/// </summary>
public interface ILlmProvider
{
    /// <summary>
    /// Generate a text response from the LLM.
    /// </summary>
    /// <param name="prompt">The user prompt</param>
    /// <param name="systemPrompt">Optional system prompt for context</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Generated text response</returns>
    Task<string> GenerateAsync(
        string prompt,
        string? systemPrompt = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Generate a structured JSON response from the LLM.
    /// </summary>
    /// <typeparam name="T">Type to deserialize the JSON response to</typeparam>
    /// <param name="prompt">The user prompt</param>
    /// <param name="systemPrompt">Optional system prompt for context</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Deserialized JSON response</returns>
    Task<T> GenerateJsonAsync<T>(
        string prompt,
        string? systemPrompt = null,
        CancellationToken cancellationToken = default
    ) where T : class;

    /// <summary>
    /// Analyze document content to extract key concepts and generate metadata.
    /// </summary>
    /// <param name="document">Parsed document to analyze</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Content analysis with key concepts</returns>
    Task<ContentAnalysis> AnalyzeContentAsync(
        ParsedDocument document,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Generate a video script from analyzed content.
    /// </summary>
    /// <param name="document">Parsed source document</param>
    /// <param name="analysis">Content analysis</param>
    /// <param name="targetDurationSeconds">Target video duration</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Generated script with scenes</returns>
    Task<Script> GenerateScriptAsync(
        ParsedDocument document,
        ContentAnalysis analysis,
        int targetDurationSeconds = 180,
        CancellationToken cancellationToken = default
    );
}
