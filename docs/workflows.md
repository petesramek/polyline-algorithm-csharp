# Workflows

This document describes all six CI/CD workflows and explains how they connect.

## Overview

```
Feature branch push
       │
       ▼
   [build.yml]  ──── format → compile → test → pack → publish-dev + assembly metadata
       │
       │  promote-branch (manual)
       ▼
   preview/X.Y branch
       │
       ▼
   [pull-request.yml]  ──── compile → test → pack → publish-dev + benchmark (optional)
       │
       │  merge PR
       ▼
   [release.yml]  ──── compile → test → pack → publish-NuGet → GitHub Release → documentation
       │
       │  publish-documentation (manual)
       ▼
   GitHub Pages (petesramek.github.io/polyline-algorithm-csharp)
```

Version bumping runs independently via `bump-version.yml` (manual trigger).

---

## build.yml

**Trigger:** Push to any branch except `preview/**` and `release/**`, when files under `src/` change.

**Jobs (in order):**

| Job | Depends on | Description |
|---|---|---|
| `workflow-variables` | — | Sets `is-release` and `is-preview` flags |
| `versioning` | — | Extracts version from branch name, builds semver strings |
| `format` | — | Runs `dotnet format` and pushes changes back to the branch |
| `build` | `versioning`, `format` | Compiles source, uploads `build` artifact |
| `test` | `build` | Runs MSTest suite, generates test + coverage reports |
| `pack` | `versioning`, `build` | Creates `.nupkg` / `.snupkg`, uploads `package` artifact |
| `publish-package` | `pack` | Pushes package to Azure Artifacts (Development environment) |
| `generate-assembly-metadata` | `versioning`, `build` | Runs `docfx metadata`, commits versioned YAML to `api-reference/` |

**Purpose:** Continuous validation of feature branches. Also keeps the `api-reference/` directory up-to-date with every successful build.

---

## pull-request.yml

**Trigger:** Pull requests targeting `preview/**` or `release/**` branches (`opened`, `synchronize`, `reopened`).

**Jobs (in order):**

| Job | Depends on | Description |
|---|---|---|
| `workflow-variables` | — | Sets `is-release` and `is-preview` flags |
| `versioning` | `workflow-variables` | Extracts version from **base** branch |
| `build` | `versioning` | Compiles source |
| `test` | `build` | Runs tests, generates reports |
| `pack` | `versioning`, `build` | Packages binaries |
| `publish-development-package` | `pack` | Pushes to Azure Artifacts (Development environment) |
| `benchmark` | `build` | BenchmarkDotNet run on Ubuntu, Windows, macOS — only when `BENCHMARKDOTNET_RUN_OVERRIDE=true` or building a release branch |

**Purpose:** Validates the change before it merges into a stabilization branch. Benchmark results are uploaded as per-OS artifacts and appended to the step summary.

---

## release.yml

**Trigger:** Push to `preview/**` or `release/**` branches when files under `src/` change (i.e., after a PR is merged).

**Jobs (in order):**

| Job | Depends on | Description |
|---|---|---|
| `workflow-variables` | — | Sets `is-release` / `is-preview` flags |
| `validate-release` | `workflow-variables` | Fails fast if the branch is neither `preview/**` nor `release/**` |
| `versioning` | `workflow-variables`, `validate-release` | Extracts and formats version from the branch name |
| `build` | `versioning` | Compiles source |
| `test` | `build` | Runs tests |
| `pack` | `versioning`, `build` | Packages binaries |
| `publish-package` | `pack` | Publishes to NuGet.org (Production environment) |
| `create-release` | `versioning`, `publish-package` | Creates a git tag + GitHub release with auto-generated notes |
| `generate-docs` | `versioning` | Builds DocFX site |
| `publish-docs` | `generate-docs` | Deploys to GitHub Pages |

**Purpose:** Full release pipeline. Triggered by merging a PR into `preview/**` (pre-release) or `release/**` (stable release).

---

## bump-version.yml

**Trigger:** Manual (`workflow_dispatch`).

**Inputs:**

| Input | Values | Description |
|---|---|---|
| `bump-type` | `minor` / `major` | Type of version bump |

**What it does:**

1. Reads the current version from the default branch.
2. Increments the `MINOR` or `MAJOR` component.
3. Unlocks the target branches (via `branch-protection/unlock`), commits the new version number, and re-locks them.
4. Creates a pull request for the version bump change.

**Purpose:** Controlled version increment without manual editing of version files.

---

## promote-branch.yml

**Trigger:** Manual (`workflow_dispatch`).

**Inputs:**

| Input | Values | Description |
|---|---|---|
| `promotion-type` | `preview` / `release` | Target stabilization tier |
| `base-branch` | string | Branch to branch off from (default: `main`) |

**Validation rules:**

- `preview` promotion requires source to be a `develop/**` or `support/**` branch.
- `release` promotion requires source to be a `preview/**` branch.
- Source and target branches must be different.
- An open PR from source → target must not already exist.

**What it does:**

1. Extracts the version from the current branch name.
2. Derives the target branch name (`preview/X.Y` or `release/X.Y`).
3. Creates the target branch if it does not exist, then locks it.
4. Opens a pull request from the current branch into the target branch.

**Purpose:** Promotes code from a development branch into a stabilization branch following the [branch strategy](./branch-strategy.md).

---

## publish-documentation.yml

**Trigger:** Manual (`workflow_dispatch`).

**Jobs:**

| Job | Description |
|---|---|
| `workflow-variables` | Captures `github.run_number` |
| `versioning` | Extracts version from the current branch |
| `generate-docs` | Runs `docfx build` from `api-reference/api-reference.json`, uploads result to `api-reference/_docs/`, then to `github-pages` artifact |
| `publish-docs` | Deploys `github-pages` artifact to GitHub Pages |

**Purpose:** Re-publishes the documentation site on demand, without needing a code change. Useful after updating guide articles or fixing doc rendering issues.

---

## Shared Environment Variables

All workflows share these top-level `env` defaults:

| Variable | Value |
|---|---|
| `dotnet-sdk-version` | `10.x` |
| `build-configuration` | `Release` |
| `build-platform` | `Any CPU` |
| `test-result-directory` | `test-results` |
| `nuget-packages-directory` | `nuget-packages` |

## Concurrency

Each workflow uses a `concurrency` group keyed on the branch or ref to prevent parallel runs from conflicting. Most use `cancel-in-progress: false` to avoid canceling in-flight releases; only `pull-request` uses `cancel-in-progress: true` to discard stale runs quickly.
