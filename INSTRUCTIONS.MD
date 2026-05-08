# Development Instructions

While you are in this folder, here is the way we work.

## 📋 Table of Contents

- [Project Overview](#project-overview)
- [Development Setup](#development-setup)
- [Code Organization](#code-organization)
- [Coding Standards](#coding-standards)
- [Building & Running](#building--running)
- [Testing](#testing)
- [Git Workflow](#git-workflow)
- [Logging](#logging)
- [Contributing](#contributing)

---

## Project Overview

**FindDuplicates** is a .NET 10 console application designed to identify and remove duplicate files from directory trees using content-based hashing.

### Technology Stack

- **.NET 10** - Target framework
- **Microsoft.Extensions.*** - DI, Configuration, Logging
- **Serilog** - Structured logging
- **xUnit** - Unit testing framework
- **FluentAssertions** - Fluent assertion library
- **coverlet** - Code coverage

### Architecture

The project follows a **layered architecture** with:
- **Models** - Domain objects (e.g., `DuplicateGroup`)
- **Services** - Business logic (interfaces-first design)
- **Utilities** - Helper functions (hashing, formatting, filtering)
- **Application** - DI setup and app bootstrap

---

## Development Setup

### Prerequisites

- **.NET 10 SDK** or later ([download](https://dotnet.microsoft.com/download/dotnet/10.0))
- **Visual Studio 2022+** or **VS Code** with C# extension
- **Git** for version control

### Initial Setup

```powershell
# Clone the repository
git clone https://github.com/adipuscasu/FindDuplicates.git
cd FindDuplicates

# Restore dependencies
dotnet restore

# Build the solution
dotnet build
```

### Verify Setup

```powershell
# Run tests to verify everything works
dotnet test

# Run the application
dotnet run find "C:\YourTestFolder"
```

---

## Code Organization

### Folder Structure

```
src/
├── FindDuplicates/
│   ├── Application/          # App bootstrap & DI container setup
│   │   └── ServiceContainer.cs
│   │   └── App.cs
│   ├── Services/             # Business logic & interfaces
│   │   ├── ICommandHandler.cs
│   │   ├── CommandHandler.cs
│   │   ├── IDuplicateFinder.cs
│   │   ├── DuplicateFinder.cs
│   │   ├── DuplicateDisplayer.cs
│   │   ├── DuplicateRemover.cs
│   │   ├── ExtensionFileFilter.cs
│   │   ├── IFileFilter.cs
│   │   ├── IFileProcessor.cs
│   │   └── UsageDisplay.cs
│   ├── Models/               # Domain objects
│   │   └── DuplicateGroup.cs
│   ├── Utilities/            # Helper functions
│   │   ├── FileHasher.cs
│   │   ├── FileFormatter.cs
│   │   └── ExtensionNormalizer.cs
│   ├── Program.cs            # Entry point
│   ├── FindDuplicates.csproj # Main project file
│   └── appsettings.json      # Serilog configuration
├── FindDuplicatesTests/
│   ├── (test files)
│   └── FindDuplicates.Tests.csproj # Test project file
└── FindDuplicates.sln        # Solution file
```

### Key Design Principles

1. **Interface-First Design** - Always define interfaces before implementations
2. **Dependency Injection** - Use `ServiceCollection` for all dependencies
3. **Single Responsibility** - Each class has one reason to change
4. **Separation of Concerns** - Services handle business logic, utilities handle cross-cutting concerns

### Example Service Structure

```csharp
// Step 1: Define interface
public interface IMyService
{
    void DoSomething();
}

// Step 2: Implement service
public class MyService : IMyService
{
    private readonly ILogger<MyService> _logger;
    
    public MyService(ILogger<MyService> logger)
    {
        _logger = logger;
    }
    
    public void DoSomething()
    {
        _logger.LogInformation("Doing something");
    }
}

// Step 3: Register in ServiceContainer
services.AddSingleton<IMyService, MyService>();
```

---

## Coding Standards

### C# Style Guidelines

- **Enable Nullable Reference Types** - Project has `<Nullable>enable</Nullable>`
- **Use Implicit Usings** - Project has `<ImplicitUsings>enable</ImplicitUsings>`
- **Use async/await** - For I/O operations (where applicable)
- **Use modern C# features** - Records, nullable reference types, etc.

### Naming Conventions

| Type | Convention | Example |
|------|-----------|---------|
| **Interfaces** | `I` prefix | `IFileProcessor`, `IDuplicateFinder` |
| **Classes** | PascalCase | `DuplicateRemover`, `FileHasher` |
| **Methods** | PascalCase | `FindDuplicates()`, `ComputeHash()` |
| **Fields (private)** | `_camelCase` | `_logger`, `_configuration` |
| **Constants** | `UPPER_SNAKE_CASE` | `MAX_FILE_SIZE`, `BUFFER_SIZE` |
| **Local variables** | `camelCase` | `result`, `duplicateGroups` |

### Best Practices

- ✅ Use dependency injection for all services
- ✅ Log important operations and errors
- ✅ Write descriptive method/class names
- ✅ Handle exceptions appropriately
- ✅ Keep methods small and focused
- ❌ Avoid static classes (use DI instead)
- ❌ Avoid magic numbers (use constants)
- ❌ Don't suppress warnings without good reason

---

## Building & Running

### Build Commands

```powershell
# Build in Debug mode (default)
dotnet build

# Build in Release mode
dotnet build --configuration Release

# Clean build
dotnet clean
dotnet build
```

### Run Commands

```powershell
# Run with dotnet run (development)
dotspanish