# Contributing to PolylineAlgorithm

Thank you for your interest in improving this library!

## Developer Documentation

In-depth developer guides are in the [`/docs`](./docs/README.md) folder:

- [Local Development](./docs/local-development.md) — build, test, and format commands
- [Testing](./docs/testing.md) — how to write unit tests
- [Benchmarks](./docs/benchmarks.md) — how to write and run benchmarks
- [Composite Actions](./docs/composite-actions.md) — reusable CI actions catalogue
- [Workflows](./docs/workflows.md) — CI/CD pipeline overview
- [Branch Strategy](./docs/branch-strategy.md) — branch lifecycle and environments
- [Versioning](./docs/versioning.md) — branch naming and the version pipeline
- [API Documentation](./docs/api-documentation.md) — DocFX and the API reference site

## Guidelines

- **Follow code style:** Use `.editorconfig` and run `dotnet format`.
- **Add unit tests:** Place all tests in `/tests`, following naming conventions.
- **Benchmark updates:** Add or update `/benchmarks` for major changes.

## Issue and PR Templates

Please use the provided templates in `.github` for all new issues or pull requests.

## API Documentation

API reference is auto-generated from XML comments and published at  
👉 [API Reference](https://petesramek.github.io/polyline-algorithm-csharp/)

- All public classes, interfaces, and methods require XML doc comments.
- After merging, verify that documentation renders correctly.
- Add usage samples where applicable.

## Submitting a Change

1. Fork the repo and create a new branch.
2. Implement your changes, tests, and update doc comments.
3. Run `dotnet format`, and all tests/benchmarks.
4. Submit a pull request, using the provided template.

## Contact

For help or questions, open an issue and tag `@petesramek`.

## License

MIT License &copy; Pete Sramek
