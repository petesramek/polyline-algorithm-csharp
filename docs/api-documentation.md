# API Documentation

This document explains how API documentation is generated and published for PolylineAlgorithm.

## Toolchain

Documentation is built with [DocFX](https://dotnet.github.io/docfx/), a static site generator for .NET API references and Markdown guides.

## Repository Layout

```
api-reference/
├── api-reference.json        # DocFX build manifest (site generation)
├── assembly-metadata.json    # DocFX metadata manifest (API extraction only)
├── toc.yml                   # Top-level table of contents
├── index.md                  # Landing page
├── favicon.ico
├── media/                    # Images and other static assets
├── guide/                    # Markdown guide articles
│   ├── toc.yml
│   ├── introduction.md
│   ├── getting-started.md
│   ├── configuration.md
│   ├── advanced-scenarios.md
│   ├── sample.md
│   └── faq.md
├── 0.0/                      # Auto-generated API metadata for version 0.0
├── 1.0/                      # Auto-generated API metadata for version 1.0
│   └── *.yml                 # DocFX apiPage YAML files (one per type)
└── _docs/                    # Build output (excluded from DocFX content, gitignored)
```

## Two DocFX Manifests

### `assembly-metadata.json` — API Extraction

Used by the `documentation/docfx-metadata` composite action during CI builds to extract XML documentation comments from source code and produce DocFX-compatible YAML files.

```json
{
  "metadata": [{
    "src": [{ "src": "../src", "files": ["**/*.csproj"] }],
    "dest": "temp",
    "outputFormat": "apiPage"
  }]
}
```

- **Input:** All `.csproj` files under `src/`
- **Output:** YAML files in `api-reference/temp/`, then copied to `api-reference/<version>/`
- **When it runs:** Automatically after every successful `build` workflow run. The resulting YAML files are committed to the repository under `api-reference/<friendly-version>/` (e.g., `api-reference/1.2/`).

### `api-reference.json` — Site Build

Used by the `documentation/docfx-build` composite action to build the full documentation site.

```json
{
  "build": {
    "content": [
      { "files": ["index.md", "toc.yml", "guide/*.{md,yml}"], "exclude": ["_docs/**"] },
      { "dest": "", "files": ["*.yml"], "group": "v1.0", "src": "1.0" },
      { "dest": "", "files": ["*.yml"], "group": "v1.1", "src": "1.1" }
    ],
    "output": "_docs",
    "template": ["default", "modern"]
  }
}
```

- **Input:** Markdown guide articles + versioned API YAML files
- **Output:** Static HTML site in `api-reference/_docs/`
- **When it runs:** During `release.yml` (automatic on every push to `preview/**` or `release/**`) and `publish-documentation.yml` (manual trigger).

## Publishing Flow

```
src/ changed
    │
    ▼
[build.yml] → docfx metadata → commit YAML to api-reference/<version>/
                                        │
                                        ▼
                              [release.yml] or [publish-documentation.yml]
                                        │
                                        ▼
                              docfx build → api-reference/_docs/
                                        │
                                        ▼
                              GitHub Pages → petesramek.github.io/polyline-algorithm-csharp
```

## Adding a New Version to the Site

When bumping the version to `X.Y`:

1. The `build` workflow automatically generates metadata YAML files into `api-reference/X.Y/` after the first build on the new branch.
2. Add a new entry to `api-reference.json` under `build.content`:
   ```json
   { "dest": "", "files": ["*.yml"], "group": "vX.Y", "src": "X.Y", "rootTocPath": "~/toc.html" }
   ```
3. Add a matching group definition:
   ```json
   "vX.Y": { "dest": "X.Y" }
   ```
4. Add the new version to `api-reference/toc.yml` so it appears in the navigation dropdown:
   ```yaml
   - name: vX.Y
     href: X.Y/PolylineAlgorithm.html
   ```

## Writing API Documentation

All public types, interfaces, and members must have XML doc comments. DocFX picks these up automatically:

```csharp
/// <summary>
/// Encodes a sequence of coordinates into a polyline string.
/// </summary>
/// <param name="coordinates">The coordinates to encode.</param>
/// <returns>An encoded polyline string.</returns>
public string Encode(IEnumerable<(double Latitude, double Longitude)> coordinates) { ... }
```

After merging a change, verify the rendered documentation at the [API Reference Site](https://petesramek.github.io/polyline-algorithm-csharp/).

## Local Preview

To preview the documentation locally:

```bash
dotnet tool update -g docfx
docfx build ./api-reference/api-reference.json --serve
```

Then open `http://localhost:8080` in your browser.
