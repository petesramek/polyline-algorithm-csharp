# FAQ

Frequently Asked Questions for PolylineAlgorithm

---

## General

**Q: What coordinate ranges are valid?**  
A: Latitude must be between -90 and 90; longitude must be between -180 and 180. Passing out-of-range values throws `ArgumentOutOfRangeException`.

**Q: Which .NET versions are supported?**  
A: Any platform supporting `netstandard2.1`, including .NET Core, .NET 5+, Xamarin, Unity, and Blazor.

**Q: Can the library be used in Unity, Xamarin, Blazor, or other .NET-compatible platforms?**  
A: Yes! Any environment that supports `netstandard2.1` can use this library.

---

## Usage & Extensibility

**Q: How do I add a new polyline algorithm or coordinate type?**  
A: Implement your own encoder/decoder using `AbstractPolylineEncoder<TCoordinate, TPolyline>` and `AbstractPolylineDecoder<TPolyline, TCoordinate>`. Add unit tests and XML doc comments, then submit a PR.

**Q: How do I customize encoding options (e.g., buffer size, logging)?**  
A: Use `PolylineEncodingOptionsBuilder` to set options, and pass the result to the encoder or decoder constructor.

**Q: Is the library thread-safe?**  
A: Yes, main encoding/decoding APIs are stateless and thread-safe. If you extend using shared mutable resources, ensure proper synchronization.

**Q: What happens if I pass invalid or malformed input to the decoder?**  
A: The decoder throws descriptive exceptions for malformed polyline strings. Ensure proper exception handling in your application.

**Q: Does the library support streaming or incremental decoding of polylines?**  
A: Currently, only batch encode/decode is supported. For streaming scenarios, implement your own logic using `PolylineEncoding` utilities.

---

## Features & Support

**Q: Is there support for elevation, timestamps, or third coordinate values?**  
A: Not currently, and not planned for the core library. You may implement your own encoder/decoder using `PolylineEncoding` methods for extended coordinate data.

**Q: How do I contribute documentation improvements?**  
A: Update XML doc comments in the codebase and submit a pull request. To improve guides, update relevant markdown files in `/api-reference/guide`.

**Q: Where can I report bugs or request features?**  
A: Open a GitHub issue using the provided templates and tag `@petesramek`.

---

## Documentation & Community

**Q: Where can I find detailed API documentation?**  
A: [API Reference](https://petesramek.github.io/polyline-algorithm-csharp/)

**Q: How do I contribute?**  
A: Read [CONTRIBUTING.md](../CONTRIBUTING.md), follow coding style and testing guidelines, and use issue/PR templates.

**Q: Need more help?**  
A: Open an issue in the [GitHub repository](https://github.com/petesramek/polyline-algorithm-csharp/issues).

---
