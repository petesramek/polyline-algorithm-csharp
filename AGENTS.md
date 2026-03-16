# Polyline Algorithm Agents Instructions

## Purpose
This document provides clear instructions for automated agents (bots, CI, and code review tools) and contributors interacting with the Polyline Algorithm library.

---

## General Guidelines

- All contributions and automation **must adhere to the project's code style**, enforced via `.editorconfig` and `dotnet format`.
- **Unit tests are mandatory** for new features and bug fixes. Use the `tests/` directory and follow existing naming conventions.
- **Benchmarks** must be updated or added for significant performance changes. Use the `benchmarks/` directory.

---

## Pull Requests

Agents should:
- **Attach benchmark results** if encoding/decoding performance is affected.
- Ensure **public API changes are documented** in code comments and `api-reference/`.
- Run code format and static analysis tools before submitting (`dotnet format`, analyzers).
- Update **README.md** and `/samples` for new or changed public APIs.

---

## Error Handling and Logging

- All new code must:
    - Throw **descriptive exceptions** for invalid input or edge cases.
    - Use internal logging helpers for operational status (`LogInfoExtensions`, `LogWarningExtensions`).

---

## Encoding/Decoding Agents

- Use the abstraction interfaces (`IPolylineEncoder`, and *if available* `IPolylineDecoder`).
- Prefer extension methods for collections and arrays.
- Validate latitude and longitude ranges to avoid malformed polylines.

---

## Issue and PR Templates

Agents should reference standardized templates from `.github` as appropriate. Contributors should always use these for new issues or PRs.

---

## Extensibility

- When adding encoding schemes or new coordinate types, **use separate classes/files** and register via a factory pattern.
- Do not mix logic between Google Polyline versions or custom coordinate types.

---

## Future-proofing

- If adding support for precision, custom coordinate fields, or options, update `PolylineEncodingOptions` and provide clear doc comments.

---

## Documentation

- Keep XML comments, `api-reference/`, and `README.md` up-to-date.
- Add code samples for any new features.

---

## Agent File Format (for `.github/agents`)

Each file must specify:

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

For questions or clarifications, please open a GitHub issue and tag `@petesramek`.

---
