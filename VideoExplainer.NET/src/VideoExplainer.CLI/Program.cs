/// <summary>
/// Video Explainer CLI Application
/// Migrated from: src/cli/main.py
/// </summary>
using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using VideoExplainer.CLI.Commands;
using VideoExplainer.Core.Interfaces;
using VideoExplainer.Infrastructure.LlmProviders;
using VideoExplainer.Infrastructure.TtsProviders;
using VideoExplainer.Infrastructure.Audio;
using VideoExplainer.Infrastructure.Parsing;
using VideoExplainer.Infrastructure.Projects;

// Set up dependency injection
var services = new ServiceCollection();

// Register services
services.AddSingleton<ILlmProvider, MockLlmProvider>();
services.AddSingleton<ITtsProvider, MockTtsProvider>();
services.AddSingleton<IMusicGenerator, MockMusicGenerator>();
services.AddSingleton<IWhisperTranscriber, MockWhisperTranscriber>();
services.AddSingleton<IDocumentParser, MarkdownDocumentParser>();
services.AddSingleton<IProjectManager>(sp => new FileSystemProjectManager("projects"));

var serviceProvider = services.BuildServiceProvider();

// Build root command
var rootCommand = new RootCommand("Video Explainer Pipeline CLI");

// Add commands
rootCommand.AddCommand(CreateCommand.Build(serviceProvider));
rootCommand.AddCommand(ListCommand.Build(serviceProvider));
rootCommand.AddCommand(InfoCommand.Build(serviceProvider));
rootCommand.AddCommand(ScriptCommand.Build(serviceProvider));

// Run CLI
return await rootCommand.InvokeAsync(args);
