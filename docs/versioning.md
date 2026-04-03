# Versioning

This document explains how version numbers are derived from branch names and how they flow through the build pipeline.

## Branch Naming Convention

The version is embedded in the branch name for `preview/**` and `release/**` branches:

| Branch | Example | Extracted version |
|---|---|---|
| `preview/X.Y` | `preview/1.2` | `1.2` |
| `release/X.Y` | `release/1.2` | `1.2` |
| Any other branch | `develop/my-feature` | `0.0` (default fallback) |

The regex used to extract the version is `(\d+).(\d+)` (configurable via the `version-format` input of `versioning/extract-version`).

## Version Pipeline

Each versioning-aware workflow runs two composite actions in sequence:

```
Branch name
    │
    ▼
[versioning/extract-version]
    │  output: version = "X.Y"
    ▼
[versioning/format-version]
    │  inputs: version, patch (run number), build-number, sha, pre-release-tag
    ▼
  outputs: friendly-version, assembly-version, assembly-informational-version,
           file-version, release-version
```

### Step 1 — Extract version (`versioning/extract-version`)

Applies the regex against the branch name. Falls back to `0.0` for non-versioned branches.

### Step 2 — Determine pre-release tag

Before calling `format-version`, the workflow computes a pre-release tag:

| Branch type | Pre-release tag |
|---|---|
| `release/**` | _(empty — stable release)_ |
| `preview/**` | `preview` |
| Any other | Branch name slug (e.g. `develop-my-feature`) |

### Step 3 — Format version (`versioning/format-version`)

Uses `version`, `patch` (= `github.run_number`), `build-number` (commits ahead of `main`), `sha`, and `pre-release-tag` to produce:

| Output | Formula | Example |
|---|---|---|
| `friendly-version` | `X.Y` | `1.2` |
| `assembly-version` | `X.Y.patch.buildNumber` | `1.2.42.7` |
| `assembly-informational-version` | `X.Y.patch+sha` | `1.2.42+abc1234` |
| `file-version` | `X.Y.patch.buildNumber` | `1.2.42.7` |
| `release-version` (stable) | `X.Y.patch.buildNumber` | `1.2.42.7` |
| `release-version` (pre-release) | `X.Y.patch-preTag.buildNumber` | `1.2.42-preview.7` |

## Where Versions Are Used

| Version string | Used as |
|---|---|
| `assembly-version` | `/p:Version` and `/p:AssemblyVersion` in `dotnet build` / `dotnet pack` |
| `assembly-informational-version` | `/p:AssemblyInformationalVersion` in `dotnet build` |
| `file-version` | `/p:FileVersion` in `dotnet build` |
| `release-version` | `/p:PackageVersion` in `dotnet pack` (the NuGet package version) |
| `friendly-version` | Human-readable label in release names, artifact names, and documentation paths |

## Build Number Calculation

`build-number` counts the commits on the current branch that are not on `main`:

```bash
git fetch --unshallow --filter=tree:0
build_number=$(git rev-list --count origin/<branch> ^origin/main)
```

This means two builds from the same branch will produce different `assembly-version` and `release-version` strings only if new commits are added.

## Examples

Given branch `preview/1.2`, run number `55`, 3 commits ahead of `main`, and SHA `abc1234f`:

| Output | Value |
|---|---|
| `friendly-version` | `1.2` |
| `assembly-version` | `1.2.55.3` |
| `assembly-informational-version` | `1.2.55+abc1234f` |
| `file-version` | `1.2.55.3` |
| `release-version` | `1.2.55-preview.3` |

Given branch `release/1.2`, same inputs:

| Output | Value |
|---|---|
| `release-version` | `1.2.55.3` _(no pre-release tag)_ |
