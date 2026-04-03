# Testing

This guide explains the test project structure, naming conventions, and how to write new tests.

## Project Structure

All tests live in the `tests/` directory:

```
tests/
└── PolylineAlgorithm.Tests/
    ├── Abstraction/                        # Tests for abstract base classes
    │   ├── AbstractPolylineDecoderTests.cs
    │   └── AbstractPolylineEncoderTests.cs
    ├── Internal/                           # Tests for internal helpers
    │   ├── CoordinateDeltaTests.cs
    │   ├── Pow10Tests.cs
    │   └── Diagnostics/
    ├── Extensions/                         # Tests for extension methods
    ├── InvalidPolylineExceptionTests.cs
    ├── PolylineEncodingTests.cs
    ├── PolylineEncodingOptionsBuilderTests.cs
    └── PolylineAlgorithm.Tests.csproj
```

## Test Framework

Tests use **MSTest** with `Microsoft.Testing.Platform` runner. The project targets `net8.0`, `net9.0`, and `net10.0`.

Key NuGet packages:

| Package | Purpose |
|---|---|
| `MSTest` | Test attributes and assertions |
| `Microsoft.NET.Test.Sdk` | Test runner integration |
| `Microsoft.Testing.Extensions.CodeCoverage` | Code coverage collection |
| `Microsoft.Testing.Extensions.TrxReport` | TRX report generation |
| `Microsoft.Extensions.Diagnostics.Testing` | Logging test helpers |

## Naming Conventions

Follow the existing pattern: `{Subject}_{Scenario}_{ExpectedResult}`.

Examples:

```
Decode_With_Null_Polyline_Throws_ArgumentNullException
Normalize_ZeroValue_ReturnsZero
Normalize_With_Value_And_Precision_Returns_Expected_Normalized_Value
```

## Writing a New Test Class

1. Create a new `.cs` file in the appropriate subdirectory of `tests/PolylineAlgorithm.Tests/`.
2. Use the `[TestClass]` attribute and mark it `sealed`.
3. Add the standard copyright header (copy from an existing file).
4. Annotate every public test method with `[TestMethod]` and an XML doc comment.

Example structure:

```csharp
//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

/// <summary>
/// Tests for <see cref="MyClass"/>.
/// </summary>
[TestClass]
public sealed class MyClassTests {
    /// <summary>
    /// Tests that <see cref="MyClass.MyMethod"/> returns the expected result.
    /// </summary>
    [TestMethod]
    public void MyMethod_WithValidInput_ReturnsExpected() {
        // Arrange
        var sut = new MyClass();

        // Act
        var result = sut.MyMethod("input");

        // Assert
        Assert.AreEqual("expected", result);
    }
}
```

## Data-Driven Tests

Use `[DataRow]` to test multiple cases in a single method:

```csharp
[TestMethod]
[DataRow(37.78903, 5u, 3778903)]
[DataRow(-122.4123, 5u, -12241230)]
public void Normalize_With_Value_And_Precision_Returns_Expected(double value, uint precision, int expected) {
    int result = PolylineEncoding.Normalize(value, precision);
    Assert.AreEqual(expected, result);
}
```

## Testing Abstract Base Classes

Internal test implementations are defined as private sealed classes inside the test class. This avoids polluting the public namespace:

```csharp
[TestClass]
public sealed class AbstractPolylineDecoderTests {
    private sealed class TestStringDecoder : AbstractPolylineDecoder<string, (double, double)> {
        protected override ReadOnlyMemory<char> GetReadOnlyMemory(in string polyline) => polyline.AsMemory();
        protected override (double, double) CreateCoordinate(double lat, double lon) => (lat, lon);
    }

    [TestMethod]
    public void Decode_With_Null_Polyline_Throws_ArgumentNullException() {
        var decoder = new TestStringDecoder();
        Assert.ThrowsExactly<ArgumentNullException>(() => decoder.Decode((string?)null!).ToList());
    }
}
```

## Running Tests Locally

```bash
dotnet test ./tests/PolylineAlgorithm.Tests/PolylineAlgorithm.Tests.csproj --configuration Release
```

Always use `--configuration Release`. See [Local Development](./local-development.md) for more options including code coverage.

## Code Coverage

Coverage is configured via `code-coverage-settings.xml` at the repository root. The CI pipeline uses `dotnet-coverage` to merge multiple coverage files and `reportgenerator` to produce a Markdown summary posted to the workflow step summary.
