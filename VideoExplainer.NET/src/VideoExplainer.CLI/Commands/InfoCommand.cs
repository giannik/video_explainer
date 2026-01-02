/// <summary>
/// Info command for displaying project information.
/// </summary>
namespace VideoExplainer.CLI.Commands;

using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using VideoExplainer.Core.Interfaces;

public static class InfoCommand
{
    public static Command Build(ServiceProvider serviceProvider)
    {
        var command = new Command("info", "Show project information");

        var projectIdArg = new Argument<string>("project-id", "Project identifier");
        command.AddArgument(projectIdArg);

        command.SetHandler(async (string projectId) =>
        {
            var projectManager = serviceProvider.GetRequiredService<IProjectManager>();
            
            try
            {
                var project = await projectManager.LoadProjectAsync($"projects/{projectId}");

                Console.WriteLine($"Project: {project.ProjectId}");
                Console.WriteLine($"Title: {project.Title}");
                Console.WriteLine($"Description: {project.Description}");
                Console.WriteLine($"Status: {project.Status}");
                Console.WriteLine();
                Console.WriteLine("Video Settings:");
                Console.WriteLine($"  Resolution: {project.VideoConfig.Width}x{project.VideoConfig.Height}");
                Console.WriteLine($"  FPS: {project.VideoConfig.Fps}");
                Console.WriteLine($"  Target Duration: {project.VideoConfig.TargetDurationSeconds}s");
                Console.WriteLine();
                Console.WriteLine("Files:");
                
                var scriptPath = projectManager.GetProjectFilePath($"projects/{projectId}", "script");
                Console.WriteLine($"  Script: {(File.Exists(scriptPath) ? "[exists]" : "[missing]")}");
                
                var narrationPath = projectManager.GetProjectFilePath($"projects/{projectId}", "narration");
                Console.WriteLine($"  Narration: {(File.Exists(narrationPath) ? "[exists]" : "[missing]")}");
                
                var storyboardPath = projectManager.GetProjectFilePath($"projects/{projectId}", "storyboard");
                Console.WriteLine($"  Storyboard: {(File.Exists(storyboardPath) ? "[exists]" : "[missing]")}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }, projectIdArg);

        return command;
    }
}
