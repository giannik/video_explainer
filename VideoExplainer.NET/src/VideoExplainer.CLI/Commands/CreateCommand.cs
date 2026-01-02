/// <summary>
/// Create command for creating new projects.
/// </summary>
namespace VideoExplainer.CLI.Commands;

using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using VideoExplainer.Core.Interfaces;

public static class CreateCommand
{
    public static Command Build(ServiceProvider serviceProvider)
    {
        var command = new Command("create", "Create a new video project");

        var projectIdArg = new Argument<string>("project-id", "Unique project identifier");
        var titleOption = new Option<string?>("--title", "Project title");
        var descOption = new Option<string?>("--description", "Project description");

        command.AddArgument(projectIdArg);
        command.AddOption(titleOption);
        command.AddOption(descOption);

        command.SetHandler(async (string projectId, string? title, string? description) =>
        {
            var projectManager = serviceProvider.GetRequiredService<IProjectManager>();
            
            try
            {
                var actualTitle = title ?? projectId.Replace("-", " ").Replace("_", " ");
                var project = await projectManager.CreateProjectAsync(
                    projectId,
                    actualTitle,
                    description ?? ""
                );

                Console.WriteLine($"✓ Created project: {project.ProjectId}");
                Console.WriteLine($"  Title: {project.Title}");
                Console.WriteLine($"  Path: projects/{project.ProjectId}");
                Console.WriteLine();
                Console.WriteLine("Next steps:");
                Console.WriteLine($"  1. Add source document to projects/{project.ProjectId}/input/");
                Console.WriteLine($"  2. Run: dotnet run --project src/VideoExplainer.CLI script {projectId} --mock");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }, projectIdArg, titleOption, descOption);

        return command;
    }
}
