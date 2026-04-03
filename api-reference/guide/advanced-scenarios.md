# Advanced Usage

PolylineAlgorithm is designed for extensibility and integration with advanced .NET scenarios.  
This guide covers custom types, integrations, and best practices for power users.

---

## Custom Coordinate and Polyline Types

You can encode and decode custom coordinate or polyline representations by extending the abstract base classes:

- `AbstractPolylineEncoder<TCoordinate, TPolyline>`
- `AbstractPolylineDecoder<TPolyline, TCoordinate>`

### Example: Custom Encoder

```csharp
public sealed class MyPolylineEncoder : AbstractPolylineEncoder<(double Latitude, double Longitude), string>
{
    public MyPolylineEncoder() : base() { }

    public MyPolylineEncoder(PolylineEncodingOptions options)
        : base(options) { }

    protected override double GetLatitude((double Latitude, double Longitude) coordinate)
        => coordinate.Latitude;

    protected override double GetLongitude((double Latitude, double Longitude) coordinate)
        => coordinate.Longitude;

    protected override string CreatePolyline(ReadOnlyMemory<char> polyline)
        => polyline.ToString();
}
```

---

## Example: Custom Decoder

```csharp
public sealed class MyPolylineDecoder : AbstractPolylineDecoder<string, (double Latitude, double Longitude)>
{
    public MyPolylineDecoder() : base() { }

    public MyPolylineDecoder(PolylineEncodingOptions options)
        : base(options) { }

    protected override (double Latitude, double Longitude) CreateCoordinate(double latitude, double longitude)
        => (latitude, longitude);

    protected override ReadOnlyMemory<char> GetReadOnlyMemory(ref string polyline)
        => polyline.AsMemory();
}
```

---

# Registering Custom Encoder and Decoder with `IServiceCollection`

For ASP.NET Core or DI-enabled .NET applications, you can easily register your custom polyline encoder and decoder as services with `IServiceCollection` by defining an extension method. This enables constructor injection and central DI management.

---

## Example: Register Custom Polyline Encoder/Decoder

Suppose you have the following custom encoder and decoder (see [Advanced Usage](./advanced.md)):

```csharp
public sealed class MyPolylineEncoder : AbstractPolylineEncoder<(double Latitude, double Longitude), string>
{
    public MyPolylineEncoder(PolylineEncodingOptions options = null)
        : base(options) { }

    // ... override required members ...
}

public sealed class MyPolylineDecoder : AbstractPolylineDecoder<string, (double Latitude, double Longitude)>
{
    public MyPolylineDecoder(PolylineEncodingOptions options = null)
        : base(options) { }

    // ... override required members ...
}
```

---

## IServiceCollection Extension Method

```csharp
using Microsoft.Extensions.DependencyInjection;
using PolylineAlgorithm;

public static class PolylineServiceCollectionExtensions
{
    public static IServiceCollection AddMyPolylineEncoderDecoder(
        this IServiceCollection services,
        PolylineEncodingOptions options = null)
    {
        // Register encoder and decoder as singletons (adjust lifetime as needed)
        services.AddSingleton<IPolylineEncoder<(double Latitude, double Longitude), string>>(
            _ => new MyPolylineEncoder(options));
        services.AddSingleton<IPolylineDecoder<string, (double Latitude, double Longitude)>>(
            _ => new MyPolylineDecoder(options));
        return services;
    }
}
```

---

## Usage

In your application startup (e.g., `Program.cs` or `Startup.cs`):

```csharp
using PolylineAlgorithm;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMyPolylineEncoderDecoder(
    PolylineEncodingOptionsBuilder.Create()
        .WithStackAllocLimit(1024)
        .Build()
);

// Now you can inject IPolylineEncoder<(double, double), string> and IPolylineDecoder<string, (double, double)>
```

---

## Benefits

- **Central DI management** for polyline components
- Plug-and-play integration with ASP.NET Core and modern .NET project styles
- Easily swap out or configure encoders/decoders for different environments

---

> **Tip:**  
> You can generalize the extension method for different encoder/decoder types or include multiple algorithms by adding extra parameters.

---

## Integration Guidance

- **Batch or incremental processing:**  
  For large datasets, control the stack allocation limit via `PolylineEncodingOptions.StackAllocLimit`.
- **Thread safety:**  
  Default encoders/decoders are stateless and thread-safe. If extending for mutable types, ensure synchronization.
- **Logging:**  
  Integrate with .NET's `ILoggerFactory` when diagnostics or audit trails are needed.

---

## Best Practices

- Always validate input data—leverage built-in validation or extend for custom rules.
- Document all public APIs using XML comments for seamless integration with the auto-generated docs.
- For non-standard coordinate systems or precision, clearly specify semantics in your custom encoder/decoder.

---

## More Resources

- [Configuration](./configuration.md)
- [FAQ](./faq.md)
- [API Reference](https://petesramek.github.io/polyline-algorithm-csharp/)
