# Polyline Algorithm Agents Instructions

## Purpose

Instructions for automated agents (bots, CI, and code review tools) and contributors interacting with the Polyline Algorithm library.

---

## General Guidelines

- All contributions and automation **must adhere to code style** (`.editorconfig`, `dotnet format`).
- **Unit tests** are required for new features and bug fixes (`tests/` directory).
- **Benchmarks** must be updated for performance-impacting changes (`benchmarks/` directory).

---

## Pull Requests

Agents and contributors should:

- **Attach benchmark results** for encoding/decoding performance changes
- Document **public API changes** in XML comments and verify updates at [API Reference](https://petesramek.github.io/polyline-algorithm-csharp/)
- Run format and static analysis tools before submitting (`dotnet format`, analyzers)
- Update **README.md** and `/samples` for public API changes

---

## Error Handling and Logging

- Throw **descriptive exceptions** for invalid input/edge cases
- Use internal logging helpers for operational status (`LogInfoExtensions`, `LogWarningExtensions`)

---

## Encoding/Decoding Agents

- Use abstraction interfaces (`IPolylineEncoder`, `IPolylineDecoder` if available)
- Prefer extension methods for collections and arrays
- Validate latitude/longitude ranges

---

## Issue and PR Templates

Agents should reference standardized templates from `.github`. Contributors must use them for new issues or PRs.

---

## Extensibility

- Add encoding schemes or coordinate types in **separate classes/files**
- Register via factory pattern if supporting multiple algorithms
- Do not mix logic between different polyline versions

---

## Future-proofing

- Support for precision or custom coordinate fields: update `PolylineEncodingOptions` with clear doc comments

---

## Documentation

- Keep XML doc comments up-to-date in source files
- API reference is auto-generated and hosted at  
  [https://petesramek.github.io/polyline-algorithm-csharp/](https://petesramek.github.io/polyline-algorithm-csharp/)
- After public API changes, verify docs render correctly on the website
- Add usage samples in XML comments and `/samples` directory

---

## Agent File Format (for `.github/agents`) 

Each agent instruction file should specify:

```
# AGENT INSTRUCTIONS

- Purpose and scope
- Required tools/commands
- Coding and testing requirements
- Logging/error handling expectations
- Documentation or samples to update
```

---

## Contact & Questions

Questions or clarifications: open a GitHub issue and tag `@petesramek`.

---
