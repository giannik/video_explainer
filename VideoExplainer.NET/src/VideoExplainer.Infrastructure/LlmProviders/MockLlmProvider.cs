/// <summary>
/// Mock LLM provider for testing without requiring actual LLM API calls.
/// Migrated from: src/understanding/llm_provider.py - MockLLMProvider
/// </summary>
namespace VideoExplainer.Infrastructure.LlmProviders;

using System.Text.Json;
using VideoExplainer.Core.Interfaces;
using VideoExplainer.Core.Models;

/// <summary>
/// Mock LLM provider that returns realistic but generic responses for testing the pipeline.
/// This provider is useful for development and testing without incurring API costs.
/// </summary>
public class MockLlmProvider : ILlmProvider
{
    /// <summary>
    /// Generate a mock text response.
    /// </summary>
    public Task<string> GenerateAsync(
        string prompt,
        string? systemPrompt = null,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult("This is a mock LLM response for testing purposes.");
    }

    /// <summary>
    /// Generate a mock JSON response based on prompt patterns.
    /// </summary>
    public Task<T> GenerateJsonAsync<T>(
        string prompt,
        string? systemPrompt = null,
        CancellationToken cancellationToken = default) where T : class
    {
        var promptLower = prompt.ToLower();

        // Determine type and return appropriate mock data
        if (typeof(T) == typeof(ContentAnalysis))
        {
            return Task.FromResult((T)(object)GenerateMockContentAnalysis());
        }
        else if (typeof(T) == typeof(Script))
        {
            return Task.FromResult((T)(object)GenerateMockScript());
        }

        throw new NotSupportedException($"Mock generation for type {typeof(T).Name} not implemented");
    }

    /// <summary>
    /// Analyze document content - returns mock analysis.
    /// </summary>
    public Task<ContentAnalysis> AnalyzeContentAsync(
        ParsedDocument document,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(GenerateMockContentAnalysis());
    }

    /// <summary>
    /// Generate a video script - returns mock script.
    /// </summary>
    public Task<Script> GenerateScriptAsync(
        ParsedDocument document,
        ContentAnalysis analysis,
        int targetDurationSeconds = 180,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(GenerateMockScript());
    }

    private ContentAnalysis GenerateMockContentAnalysis()
    {
        return new ContentAnalysis(
            CoreThesis: "This document explains a technical concept with practical applications.",
            KeyConcepts: new List<Concept>
            {
                new Concept(
                    Name: "Core Concept",
                    Explanation: "The fundamental idea that drives the topic.",
                    Complexity: 5,
                    Prerequisites: new List<string> { "basic understanding" },
                    Analogies: new List<string> { "Like a simple real-world example" },
                    VisualPotential: VisualPotential.High
                ),
                new Concept(
                    Name: "Supporting Concept",
                    Explanation: "A related idea that helps understand the core concept.",
                    Complexity: 4,
                    Prerequisites: new List<string> { "core concept" },
                    Analogies: new List<string> { "Similar to another familiar concept" },
                    VisualPotential: VisualPotential.Medium
                ),
                new Concept(
                    Name: "Application",
                    Explanation: "How this concept is used in practice.",
                    Complexity: 6,
                    Prerequisites: new List<string> { "core concept", "supporting concept" },
                    Analogies: new List<string> { "Like using a tool for a job" },
                    VisualPotential: VisualPotential.High
                )
            },
            TargetAudience: "Technical professionals and enthusiasts",
            SuggestedDurationSeconds: 180,
            ComplexityScore: 5
        );
    }

    private Script GenerateMockScript()
    {
        return new Script(
            Title: "Understanding the Core Concept",
            TotalDurationSeconds: 110,
            Scenes: new List<ScriptScene>
            {
                new ScriptScene(
                    SceneId: 1,
                    SceneType: SceneType.Hook,
                    Title: "The Problem",
                    Voiceover: "Every day, we encounter this challenge. What if there was a better way?",
                    VisualCue: new VisualCue(
                        Description: "Show the problem visually",
                        VisualType: VisualType.Animation,
                        Elements: new List<string> { "problem_illustration" },
                        DurationSeconds: 15.0
                    ),
                    DurationSeconds: 15.0,
                    Notes: "Build intrigue"
                ),
                new ScriptScene(
                    SceneId: 2,
                    SceneType: SceneType.Context,
                    Title: "Background",
                    Voiceover: "To understand the solution, we first need to understand the context.",
                    VisualCue: new VisualCue(
                        Description: "Show background context",
                        VisualType: VisualType.Animation,
                        Elements: new List<string> { "context_diagram" },
                        DurationSeconds: 20.0
                    ),
                    DurationSeconds: 20.0,
                    Notes: "Set the stage"
                ),
                new ScriptScene(
                    SceneId: 3,
                    SceneType: SceneType.Explanation,
                    Title: "The Core Concept",
                    Voiceover: "Here's how it works. The key insight is understanding the relationship between components.",
                    VisualCue: new VisualCue(
                        Description: "Explain the core concept with visuals",
                        VisualType: VisualType.Animation,
                        Elements: new List<string> { "concept_visualization" },
                        DurationSeconds: 30.0
                    ),
                    DurationSeconds: 30.0,
                    Notes: "Main explanation"
                ),
                new ScriptScene(
                    SceneId: 4,
                    SceneType: SceneType.Insight,
                    Title: "The Key Insight",
                    Voiceover: "This is the breakthrough. Once you understand this, everything else falls into place.",
                    VisualCue: new VisualCue(
                        Description: "Highlight the key insight",
                        VisualType: VisualType.Animation,
                        Elements: new List<string> { "insight_highlight" },
                        DurationSeconds: 25.0
                    ),
                    DurationSeconds: 25.0,
                    Notes: "Aha moment"
                ),
                new ScriptScene(
                    SceneId: 5,
                    SceneType: SceneType.Conclusion,
                    Title: "Putting It Together",
                    Voiceover: "Now you understand the concept. Let's see how it applies in practice.",
                    VisualCue: new VisualCue(
                        Description: "Summary and application",
                        VisualType: VisualType.Animation,
                        Elements: new List<string> { "summary" },
                        DurationSeconds: 20.0
                    ),
                    DurationSeconds: 20.0,
                    Notes: "Wrap up"
                )
            },
            SourceDocument: "document.md"
        );
    }
}
