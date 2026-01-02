/// <summary>
/// Content analysis models for document understanding.
/// Migrated from: src/models.py - Concept, ContentAnalysis
/// </summary>
namespace VideoExplainer.Core.Models;

/// <summary>
/// A key concept extracted from the document with explanation and metadata.
/// </summary>
/// <param name="Name">Name of the concept</param>
/// <param name="Explanation">Detailed explanation of the concept</param>
/// <param name="Complexity">Complexity score from 1 (simple) to 10 (complex)</param>
/// <param name="Prerequisites">List of prerequisite concepts</param>
/// <param name="Analogies">List of analogies to help explain the concept</param>
/// <param name="VisualPotential">Visual potential level (High, Medium, Low)</param>
public record Concept(
    string Name,
    string Explanation,
    int Complexity,
    List<string> Prerequisites,
    List<string> Analogies,
    VisualPotential VisualPotential
)
{
    public Concept() : this(string.Empty, string.Empty, 5,
        new List<string>(), new List<string>(), VisualPotential.Medium) { }
}

/// <summary>
/// Analysis of the document content including key concepts and metadata.
/// </summary>
/// <param name="CoreThesis">The central thesis or main idea of the document</param>
/// <param name="KeyConcepts">List of key concepts to explain</param>
/// <param name="TargetAudience">Description of the target audience</param>
/// <param name="SuggestedDurationSeconds">Suggested video duration in seconds</param>
/// <param name="ComplexityScore">Overall complexity score from 1 to 10</param>
public record ContentAnalysis(
    string CoreThesis,
    List<Concept> KeyConcepts,
    string TargetAudience,
    int SuggestedDurationSeconds,
    int ComplexityScore
)
{
    public ContentAnalysis() : this(string.Empty, new List<Concept>(),
        string.Empty, 180, 5) { }
}
