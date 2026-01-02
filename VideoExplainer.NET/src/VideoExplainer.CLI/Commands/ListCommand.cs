/// <summary>
/// List command for listing all projects.
/// </summary>
namespace VideoExplainer.CLI.Commands;

using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using VideoExplainer.Core.Interfaces;

public static class ListCommand
{
    public static Command Build(ServiceProvider serviceProvider)
    {
        var command = new Command("list", "List all video projects");

        command.SetHandler(async () =>
        {
            var projectManager = serviceProvider.GetRequiredService<IProjectManager>();
            
            try
            {
                var projects = await projectManager.ListProjectsAsync();

                if (projects.Count == 0)
                {
                    Console.WriteLine("No projects found in projects/");
                    return;
                }

                Console.WriteLine($"Found {projects.Count} project(s):\n");
                
                foreach (var project in projects)
                {
                    Console.WriteLine($"  {project.ProjectId}");
                    Console.WriteLine($"    Title: {project.Title}");
                    Console.WriteLine($"    Status: {project.Status}");
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        });

        return command;
    }
}
