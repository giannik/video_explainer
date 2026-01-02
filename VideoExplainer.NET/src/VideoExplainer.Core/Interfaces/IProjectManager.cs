/// <summary>
/// Interface for managing video projects on the file system.
/// Migrated from: src/project/loader.py
/// </summary>
namespace VideoExplainer.Core.Interfaces;

using VideoExplainer.Core.Models;

/// <summary>
/// Project manager for CRUD operations on video projects.
/// Handles project directory structure and configuration files.
/// </summary>
public interface IProjectManager
{
    /// <summary>
    /// Create a new project with default structure.
    /// </summary>
    /// <param name="projectId">Unique project identifier (used as directory name)</param>
    /// <param name="title">Human-readable project title</param>
    /// <param name="description">Optional project description</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created project</returns>
    Task<Project> CreateProjectAsync(
        string projectId,
        string title,
        string description = "",
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Load an existing project from directory.
    /// </summary>
    /// <param name="projectPath">Path to project directory or config file</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Loaded project</returns>
    Task<Project> LoadProjectAsync(
        string projectPath,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// List all projects in the projects directory.
    /// </summary>
    /// <param name="projectsDirectory">Path to projects directory</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of projects</returns>
    Task<List<Project>> ListProjectsAsync(
        string projectsDirectory = "projects",
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Save project configuration to disk.
    /// </summary>
    /// <param name="project">Project to save</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Path to saved config file</returns>
    Task<string> SaveProjectAsync(
        Project project,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Delete a project and all its files.
    /// </summary>
    /// <param name="projectId">Project identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if deleted successfully</returns>
    Task<bool> DeleteProjectAsync(
        string projectId,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Ensure all required directories exist for a project.
    /// </summary>
    /// <param name="projectPath">Project root directory</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task</returns>
    Task EnsureDirectoriesAsync(
        string projectPath,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Get path to a specific project file or directory.
    /// </summary>
    /// <param name="projectPath">Project root path</param>
    /// <param name="fileKey">File key (e.g., "narration", "storyboard")</param>
    /// <returns>Absolute path to the file</returns>
    string GetProjectFilePath(string projectPath, string fileKey);
}
