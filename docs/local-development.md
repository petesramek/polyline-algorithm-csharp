# Local Development

This guide explains how to build, test, and format the PolylineAlgorithm codebase locally.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download) (or newer)
- A terminal / shell

## Building

Build the main library using the solution file:

```bash
dotnet build PolylineAlgorithm.slnx
```

To build in Release configuration (required before running tests or benchmarks):

```bash
dotnet build PolylineAlgorithm.slnx --configuration Release
```

## Running Tests

Run all unit tests:

```bash
dotnet test ./tests/PolylineAlgorithm.Tests/PolylineAlgorithm.Tests.csproj --configuration Release
```

> **Note:** Always use Release configuration when running tests. The Debug configuration contains a `Debug.Assert` in `AbstractPolylineEncoderTest` that will crash the test runner.

To collect code coverage at the same time:

```bash
dotnet test ./tests/PolylineAlgorithm.Tests/PolylineAlgorithm.Tests.csproj \
  --configuration Release \
  --coverage \
  --coverage-output-format cobertura \
  --coverage-settings ./code-coverage-settings.xml
```

## Running Benchmarks

See [Benchmarks](./benchmarks.md) for full details. Quick run:

```bash
dotnet run --project ./benchmarks/PolylineAlgorithm.Benchmarks/PolylineAlgorithm.Benchmarks.csproj \
  --configuration Release \
  --framework net10.0 \
  -- --filter '*'
```

## Formatting

The project uses `dotnet format` for code style enforcement. Run all format steps before committing:

```bash
# Fix whitespace
dotnet format whitespace

# Fix code style
dotnet format style

# Fix analyzer warnings (optional — run when you want to fix diagnostics)
dotnet format analyzers
```

The CI `format` job also runs `dotnet format` automatically on every push to non-release branches and pushes the formatted result back to the branch.

## Editor Configuration

Code style rules are stored in `.editorconfig` at the repository root. Any compliant IDE (Visual Studio, VS Code with C# Dev Kit, Rider) will pick these up automatically.
