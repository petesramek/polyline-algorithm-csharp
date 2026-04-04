# Configuration

PolylineAlgorithm offers flexible configuration for encoding and decoding polylines, allowing you to fine-tune performance, control validation, and integrate diagnostics and logging.

---

## PolylineEncodingOptions

Most configuration is handled via the `PolylineEncodingOptions` object, which you can build using the fluent `PolylineEncodingOptionsBuilder`.

### Example: Customizing Stack Alloc Limit

```csharp
using PolylineAlgorithm;

var options = PolylineEncodingOptionsBuilder.Create()
    .WithStackAllocLimit(1024) // Set stack allocation threshold (bytes)
    .Build();

var encoder = new PolylineEncoder(options);
```

---

## Logging and Diagnostics

PolylineAlgorithm supports internal logging for advanced scenarios and diagnostic purposes.

- Use your preferred .NET logging framework (`ILoggerFactory`)
- Attach loggers for encoding/decoding diagnostics, especially in automated or agent-based environments

---

## Validation

Input validation is always enabled by default:

- Latitude: must be between -90 and 90
- Longitude: must be between -180 and 180
- Invalid or malformed coordinates throw descriptive exceptions

For custom validation (e.g., for custom coordinate types), extend the provided interfaces or abstract base classes.

---

## Advanced Configuration Options

When using `PolylineEncodingOptionsBuilder`, you may set:

- **Stack alloc limit:** Configure the threshold below which buffers are stack-allocated vs. rented from `ArrayPool<char>`
- **Logging hooks:** Integrate your logger for troubleshooting/instrumentation
- **(Future)** Custom precision, additional metadata (as needed)

See the XML API documentation for all available builder methods.

---

## Example: Full Custom Encoder with Options

```csharp
var options = PolylineEncodingOptionsBuilder.Create()
    .WithStackAllocLimit(512)
    .WithLoggerFactory(myLoggerFactory)
    .Build();

var encoder = new PolylineEncoder(options);
```

---

## Further Reading

- [Getting Started Guide](./guide.md)
- [Advanced Usage](./advanced.md)
- [API Reference](https://petesramek.github.io/polyline-algorithm-csharp/)
