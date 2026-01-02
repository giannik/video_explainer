# Video Explainer .NET

This is the .NET 9 migration of the Video Explainer project, following clean architecture principles with interface-based programming.

## Architecture

The solution is organized into four main projects:

### VideoExplainer.Core
Domain models and interfaces. Contains:
- **Models/**: C# record types for all domain entities (Video Config, Script, Storyboard, etc.)
- **Interfaces/**: Service interfaces (ILlmProvider, ITtsProvider, etc.)

### VideoExplainer.Infrastructure
Concrete implementations of core interfaces:
- **LlmProviders/**: Language model providers (Mock, Anthropic, OpenAI)
- **TtsProviders/**: Text-to-speech providers (Mock, ElevenLabs, Azure)
- **Audio/**: Audio processing (Mock Music Generator, Mock Whisper Transcriber, FFmpeg processor)
- **Parsing/**: Document parsers (Markdown)
- **Projects/**: File system project management
- **External/**: External tool wrappers (Remotion renderer)

### VideoExplainer.Pipeline
Orchestration services for the video generation pipeline

### VideoExplainer.CLI
Command-line interface using System.CommandLine

## Building

```bash
cd VideoExplainer.NET
dotnet build
```

## Running

```bash
# Create a new project
dotnet run --project src/VideoExplainer.CLI create my-project --title "My Video"

# List all projects
dotnet run --project src/VideoExplainer.CLI list

# Show project info
dotnet run --project src/VideoExplainer.CLI info my-project

# Generate script (with mock LLM)
dotnet run --project src/VideoExplainer.CLI script my-project --mock --duration 180
```

## Configuration

Edit `src/VideoExplainer.CLI/appsettings.json` to configure:
- LLM provider (mock, anthropic, openai)
- TTS provider (mock, elevenlabs, azure)
- Video settings (resolution, fps, duration)
- Output paths

## Mock Implementations

Several services are currently mock implementations due to .NET compatibility limitations:

### IMusicGenerator (Mock Only)
**Python Implementation**: Meta's MusicGen via HuggingFace Transformers + PyTorch

**Why Mocked**:
- No native PyTorch runtime in .NET
- No HuggingFace Transformers equivalent
- MusicGen has no official ONNX export

**Production Options**:
1. **Python Microservice** (Recommended): Host the Python MusicGen service separately, expose via REST API
2. **Third-Party API**: Use Mubert, AIVA, or Soundraw APIs
3. **Azure AI Services**: Deploy MusicGen to Azure ML
4. **ONNX Runtime**: If MusicGen gets ONNX support in the future

See `src/VideoExplainer.Infrastructure/Audio/MockMusicGenerator.cs` for detailed implementation notes.

### IWhisperTranscriber (Mock Only)
**Python Implementation**: OpenAI Whisper or faster-whisper for speech-to-text with word-level timestamps

**Why Mocked**:
- No native PyTorch runtime in .NET
- Whisper models are PyTorch-based
- ONNX exports often lack word-level timestamp support

**Production Options**:
1. **Python Microservice** (Recommended): Host Whisper as HTTP service (e.g., whisper-api)
2. **Azure Speech Services**: Native .NET SDK with word-level timestamps
3. **AssemblyAI API**: .NET SDK with excellent transcription quality
4. **Whisper.NET**: Experimental ONNX-based package (limited features)

See `src/VideoExplainer.Infrastructure/Audio/MockWhisperTranscriber.cs` for detailed implementation notes.

## Real Implementations

The following services have working implementations:

### ILlmProvider
- ✅ MockLlmProvider: For testing without API costs
- 🚧 AnthropicLlmProvider: (Planned) HTTP client to Claude API
- 🚧 OpenAiLlmProvider: (Planned) HTTP client to OpenAI API

### ITtsProvider
- ✅ MockTtsProvider: Generates silent audio files using FFmpeg
- 🚧 ElevenLabsTtsProvider: (Planned) HTTP client to ElevenLabs API
- 🚧 AzureTtsProvider: (Planned) Azure Speech Services integration

### IDocumentParser
- ✅ MarkdownDocumentParser: Full Markdown parsing with code blocks, headings, etc.

### IProjectManager
- ✅ FileSystemProjectManager: Complete CRUD operations for projects

### IAudioProcessor
- 🚧 FFmpegAudioProcessor: (Planned) Wrapper for ffmpeg commands

### IRemotionRenderer
- 🚧 RemotionRenderer: (Planned) Process wrapper for `npx remotion render`

## Development Roadmap

### Phase 1: Core Infrastructure ✅
- [x] Solution structure
- [x] Core models and interfaces
- [x] Mock implementations
- [x] Basic CLI commands

### Phase 2: Real Implementations (In Progress)
- [ ] Anthropic LLM Provider
- [ ] OpenAI LLM Provider  
- [ ] ElevenLabs TTS Provider
- [ ] Azure TTS Provider
- [ ] FFmpeg Audio Processor
- [ ] Remotion Renderer

### Phase 3: Pipeline Services
- [ ] Script Generator Service
- [ ] Narration Generator Service
- [ ] Scene Generator Service
- [ ] Storyboard Generator Service
- [ ] Voiceover Generator Service
- [ ] Music Generator Service
- [ ] Render Service

### Phase 4: Testing & Documentation
- [ ] Unit tests
- [ ] Integration tests
- [ ] Docker support
- [ ] CI/CD pipeline

## Dependencies

### Required
- .NET 9.0 or later
- FFmpeg (for audio/video processing)
- Node.js + Remotion (for video rendering)

### Optional (for production services)
- Anthropic API key (for Claude LLM)
- OpenAI API key (for GPT LLM)
- ElevenLabs API key (for TTS)
- Azure subscription (for Azure services)

## NuGet Packages

- `System.CommandLine` (2.0.0-beta4): CLI framework
- `Spectre.Console`: Rich console output
- `YamlDotNet`: YAML configuration support
- `Microsoft.Extensions.DependencyInjection`: Dependency injection
- `Microsoft.Extensions.Configuration`: Configuration management
- `Microsoft.Extensions.Http`: HttpClient factory

## Migration Notes

### From Python to .NET

This project was migrated from Python using the following approach:

1. **Models**: Pydantic models → C# records with primary constructors
2. **Interfaces**: Python duck typing → Explicit C# interfaces
3. **Async/Await**: Python asyncio → .NET Task-based asynchrony
4. **Dependency Injection**: Python manual wiring → Microsoft.Extensions.DI
5. **CLI**: Python argparse → System.CommandLine
6. **Configuration**: Python YAML → .NET appsettings.json + YamlDotNet

### Key Differences

- **Type Safety**: Strongly typed throughout vs. Python's dynamic typing
- **Null Safety**: C# nullable reference types for compile-time null checking
- **Performance**: Generally faster execution, smaller memory footprint
- **Ecosystem**: Different package ecosystem (NuGet vs. PyPI)
- **ML Libraries**: Limited ML library support (major gap for MusicGen/Whisper)

### Advantages of .NET Version

✅ Better performance and lower memory usage
✅ Compile-time type checking
✅ Strong IDE support (IntelliSense, refactoring)
✅ Easy deployment (self-contained executables)
✅ Better integration with Azure/Microsoft ecosystem

### Challenges

⚠️ Limited ML library support (PyTorch, Transformers)
⚠️ Need external services for MusicGen and Whisper
⚠️ Smaller ecosystem for media processing compared to Python

## Contributing

When adding new implementations:

1. Define interface in `VideoExplainer.Core/Interfaces/`
2. Implement in `VideoExplainer.Infrastructure/`
3. Register in DI container (`Program.cs`)
4. Add CLI command if needed
5. Update this README

## License

Same license as the original Python project.
