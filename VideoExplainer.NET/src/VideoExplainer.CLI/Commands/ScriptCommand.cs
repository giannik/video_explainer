/// <summary>
/// Script command for generating video scripts.
/// </summary>
namespace VideoExplainer.CLI.Commands;

using System.CommandLine;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using VideoExplainer.Core.Interfaces;

public static class ScriptCommand
{
    public static Command Build(ServiceProvider serviceProvider)
    {
        var command = new Command("script", "Generate video script from input documents");

        var projectIdArg = new Argument<string>("project-id", "Project identifier");
        var mockOption = new Option<bool>("--mock", "Use mock LLM provider");
        var durationOption = new Option<int?>("--duration", "Target duration in seconds");

        command.AddArgument(projectIdArg);
        command.AddOption(mockOption);
        command.AddOption(durationOption);

        command.SetHandler(async (string projectId, bool useMock, int? duration) =>
        {
            var projectManager = serviceProvider.GetRequiredService<IProjectManager>();
            var llmProvider = serviceProvider.GetRequiredService<ILlmProvider>();
            var docParser = serviceProvider.GetRequiredService<IDocumentParser>();
            
            try
            {
                Console.WriteLine($"Generating script for {projectId}...");
                
                var project = await projectManager.LoadProjectAsync($"projects/{projectId}");

                // Find input files
                var inputDir = $"projects/{projectId}/input";
                if (!Directory.Exists(inputDir))
                {
                    Console.Error.WriteLine($"Error: Input directory not found: {inputDir}");
                    Console.Error.WriteLine("Add source documents to the input/ directory first.");
                    return;
                }

                var inputFiles = Directory.GetFiles(inputDir, "*.md");
                if (inputFiles.Length == 0)
                {
                    Console.Error.WriteLine($"Error: No markdown files found in {inputDir}");
                    return;
                }

                Console.WriteLine($"Found {inputFiles.Length} input file(s)");

                // Parse document
                Console.WriteLine($"Parsing: {Path.GetFileName(inputFiles[0])}");
                var document = await docParser.ParseFileAsync(inputFiles[0]);
                
                // Analyze content
                Console.WriteLine("Analyzing content...");
                var analysis = await llmProvider.AnalyzeContentAsync(document);
                Console.WriteLine($"  Thesis: {analysis.CoreThesis.Substring(0, Math.Min(60, analysis.CoreThesis.Length))}...");
                Console.WriteLine($"  Concepts: {analysis.KeyConcepts.Count}");

                // Generate script
                Console.WriteLine("Generating script...");
                var targetDuration = duration ?? project.VideoConfig.TargetDurationSeconds;
                var script = await llmProvider.GenerateScriptAsync(document, analysis, targetDuration);
                Console.WriteLine($"  Generated {script.Scenes.Count} scenes");
                Console.WriteLine($"  Total duration: {script.TotalDurationSeconds}s");

                // Save script
                var scriptPath = projectManager.GetProjectFilePath($"projects/{projectId}", "script");
                Directory.CreateDirectory(Path.GetDirectoryName(scriptPath)!);
                
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(new
                {
                    title = script.Title,
                    total_duration_seconds = script.TotalDurationSeconds,
                    source_document = script.SourceDocument,
                    scenes = script.Scenes.Select(s => new
                    {
                        scene_id = s.SceneId,
                        scene_type = s.SceneType.ToString().ToLower(),
                        title = s.Title,
                        voiceover = s.Voiceover,
                        visual_cue = new
                        {
                            description = s.VisualCue.Description,
                            visual_type = s.VisualCue.VisualType.ToString().ToLower(),
                            elements = s.VisualCue.Elements,
                            duration_seconds = s.VisualCue.DurationSeconds
                        },
                        duration_seconds = s.DurationSeconds,
                        notes = s.Notes
                    })
                }, options);
                
                await File.WriteAllTextAsync(scriptPath, json);
                Console.WriteLine($"\n✓ Script saved to: {scriptPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }, projectIdArg, mockOption, durationOption);

        return command;
    }
}
