/// <summary>
/// File system-based project manager.
/// Migrated from: src/project/loader.py
/// </summary>
namespace VideoExplainer.Infrastructure.Projects;

using System.Text.Json;
using VideoExplainer.Core.Interfaces;
using VideoExplainer.Core.Models;

/// <summary>
/// Manages video projects on the file system.
/// </summary>
public class FileSystemProjectManager : IProjectManager
{
    private readonly string _defaultProjectsDir;

    public FileSystemProjectManager(string defaultProjectsDir = "projects")
    {
        _defaultProjectsDir = defaultProjectsDir;
    }

    /// <summary>
    /// Create a new project with default structure.
    /// </summary>
    public async Task<Project> CreateProjectAsync(
        string projectId,
        string title,
        string description = "",
        CancellationToken cancellationToken = default)
    {
        var projectDir = Path.Combine(_defaultProjectsDir, projectId);
        
        if (Directory.Exists(projectDir))
        {
            throw new InvalidOperationException($"Project already exists: {projectId}");
        }

        // Create directory structure
        Directory.CreateDirectory(projectDir);
        await EnsureDirectoriesAsync(projectDir, cancellationToken);

        // Create config.json
        var config = new
        {
            id = projectId,
            title,
            description,
            version = "1.0.0",
            source = new { document = "input/source.md", type = "markdown" },
            video = new
            {
                resolution = new { width = 1920, height = 1080 },
                fps = 30,
                target_duration_seconds = 180
            },
            paths = new
            {
                script = "script/script.json",
                narration = "narration/narrations.json",
                voiceover = "voiceover/",
                storyboard = "storyboard/storyboard.json",
                output = "output/",
                final_video = "output/final.mp4"
            }
        };

        var configPath = Path.Combine(projectDir, "config.json");
        var options = new JsonSerializerOptions { WriteIndented = true };
        await File.WriteAllTextAsync(configPath, JsonSerializer.Serialize(config, options), cancellationToken);

        return await LoadProjectAsync(projectDir, cancellationToken);
    }

    /// <summary>
    /// Load an existing project.
    /// </summary>
    public async Task<Project> LoadProjectAsync(
        string projectPath,
        CancellationToken cancellationToken = default)
    {
        var configPath = File.Exists(projectPath) && projectPath.EndsWith(".json")
            ? projectPath
            : Path.Combine(projectPath, "config.json");

        if (!File.Exists(configPath))
        {
            throw new FileNotFoundException($"Project config not found: {configPath}");
        }

        var json = await File.ReadAllTextAsync(configPath, cancellationToken);
        var config = JsonSerializer.Deserialize<JsonElement>(json);

        var projectDir = Path.GetDirectoryName(configPath)!;

        return new Project(
            ProjectId: config.GetProperty("id").GetString() ?? Path.GetFileName(projectDir),
            Title: config.GetProperty("title").GetString() ?? "Untitled Project",
            Description: config.TryGetProperty("description", out var desc) ? desc.GetString() ?? "" : "",
            SourcePath: Path.Combine(projectDir, "input"),
            ParsedDocument: null,
            ContentAnalysis: null,
            Script: null,
            Storyboard: null,
            OutputPath: null,
            Status: "initialized",
            VideoConfig: new VideoConfig(
                Width: 1920,
                Height: 1080,
                Fps: 30,
                TargetDurationSeconds: 180
            )
        );
    }

    /// <summary>
    /// List all projects in the projects directory.
    /// </summary>
    public async Task<List<Project>> ListProjectsAsync(
        string projectsDirectory = "projects",
        CancellationToken cancellationToken = default)
    {
        if (!Directory.Exists(projectsDirectory))
        {
            return new List<Project>();
        }

        var projects = new List<Project>();
        
        foreach (var dir in Directory.GetDirectories(projectsDirectory))
        {
            var configPath = Path.Combine(dir, "config.json");
            if (File.Exists(configPath))
            {
                try
                {
                    var project = await LoadProjectAsync(dir, cancellationToken);
                    projects.Add(project);
                }
                catch
                {
                    // Skip invalid projects
                }
            }
        }

        return projects;
    }

    /// <summary>
    /// Save project configuration.
    /// </summary>
    public async Task<string> SaveProjectAsync(
        Project project,
        CancellationToken cancellationToken = default)
    {
        var projectDir = Path.Combine(_defaultProjectsDir, project.ProjectId);
        var configPath = Path.Combine(projectDir, "config.json");

        var config = new
        {
            id = project.ProjectId,
            title = project.Title,
            description = project.Description,
            version = "1.0.0",
            video = new
            {
                resolution = new { width = project.VideoConfig.Width, height = project.VideoConfig.Height },
                fps = project.VideoConfig.Fps,
                target_duration_seconds = project.VideoConfig.TargetDurationSeconds
            }
        };

        var options = new JsonSerializerOptions { WriteIndented = true };
        await File.WriteAllTextAsync(configPath, JsonSerializer.Serialize(config, options), cancellationToken);

        return configPath;
    }

    /// <summary>
    /// Delete a project.
    /// </summary>
    public Task<bool> DeleteProjectAsync(
        string projectId,
        CancellationToken cancellationToken = default)
    {
        var projectDir = Path.Combine(_defaultProjectsDir, projectId);
        
        if (Directory.Exists(projectDir))
        {
            Directory.Delete(projectDir, recursive: true);
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }

    /// <summary>
    /// Ensure all required directories exist.
    /// </summary>
    public Task EnsureDirectoriesAsync(
        string projectPath,
        CancellationToken cancellationToken = default)
    {
        var dirs = new[]
        {
            "input",
            "script",
            "narration",
            "voiceover",
            "storyboard",
            "scenes",
            "output",
            "output/preview"
        };

        foreach (var dir in dirs)
        {
            Directory.CreateDirectory(Path.Combine(projectPath, dir));
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Get path to a specific project file.
    /// </summary>
    public string GetProjectFilePath(string projectPath, string fileKey)
    {
        return fileKey switch
        {
            "script" => Path.Combine(projectPath, "script", "script.json"),
            "narration" => Path.Combine(projectPath, "narration", "narrations.json"),
            "storyboard" => Path.Combine(projectPath, "storyboard", "storyboard.json"),
            "final_video" => Path.Combine(projectPath, "output", "final.mp4"),
            _ => Path.Combine(projectPath, fileKey)
        };
    }
}
