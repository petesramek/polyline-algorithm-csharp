# Composite Actions

All reusable GitHub Actions live under `.github/actions/`. They are referenced with `uses: './.github/actions/<path>'` inside workflows. This document catalogs each action, its inputs, outputs, and typical use.

## documentation/docfx-build

**Path:** `.github/actions/documentation/docfx-build`  
**Description:** Installs the `docfx` global tool, builds a DocFX site from a JSON manifest, and uploads the generated output as a workflow artifact.

| Input | Required | Default | Description |
|---|---|---|---|
| `artifact-name` | ✅ | — | Name of the uploaded artifact |
| `docfx-json-manifest` | ✅ | — | Path to the `docfx.json` build manifest |
| `output-directory` | ✅ | — | Target directory where DocFX writes output |
| `dotnet_sdk_version` | ❌ | `10.x` | .NET SDK version to use |

**Used by:** `publish-documentation` workflow.

---

## documentation/docfx-metadata

**Path:** `.github/actions/documentation/docfx-metadata`  
**Description:** Runs `docfx metadata` to extract API metadata from source (`.yml` files), copies the result to an output directory, and uploads it as a workflow artifact.

| Input | Required | Default | Description |
|---|---|---|---|
| `artifact-name` | ✅ | — | Name of the uploaded artifact |
| `docfx-json-manifest` | ✅ | — | Path to the metadata-only `docfx.json` manifest |
| `temporary-directory` | ✅ | — | Temp folder DocFX writes raw metadata into |
| `output-directory` | ✅ | — | Final output directory for the metadata |
| `dotnet_sdk_version` | ❌ | `10.x` | .NET SDK version to use |

**Used by:** `build` workflow (to regenerate versioned assembly metadata).

---

## git/push-changes

**Path:** `.github/actions/git/push-changes`  
**Description:** Optionally downloads an artifact, stages all changes in a working directory, and pushes them to the current (or a specified target) branch. Skips the commit if there are no staged changes.

| Input | Required | Default | Description |
|---|---|---|---|
| `commit-message` | ✅ | — | Commit message |
| `artifact-name` | ❌ | `''` | Artifact to download before staging |
| `working-directory` | ❌ | `.` | Directory to stage and commit from |
| `target-branch` | ❌ | `''` | Branch to push to (creates it if absent) |
| `dotnet_sdk_version` | ❌ | `10.x` | .NET SDK version |

**Used by:** `source/format`, `build` (assembly metadata), and other workflows that commit generated artefacts.

---

## github/branch-protection/lock

**Path:** `.github/actions/github/branch-protection/lock`  
**Description:** Applies branch protection rules to a branch via the GitHub API: requires at least one PR approval, disables force pushes and deletions.

| Input | Required | Description |
|---|---|---|
| `branch` | ✅ | Branch name to protect |

**Requires:** `administration: write` permission on the workflow token.  
**Used by:** `promote-branch` workflow (after creating a new `preview/**` or `release/**` branch).

---

## github/branch-protection/unlock

**Path:** `.github/actions/github/branch-protection/unlock`  
**Description:** Removes all branch protection rules from a branch so a workflow can push directly to it. Always re-lock immediately after.

| Input | Required | Description |
|---|---|---|
| `branch` | ✅ | Branch name to unprotect |

**Requires:** `administration: write` permission on the workflow token.  
**Used by:** Workflows that need to push commits directly to protected branches (e.g., `bump-version`).

---

## github/create-release

**Path:** `.github/actions/github/create-release`  
**Description:** Creates a git tag and a GitHub release with auto-generated release notes. Supports pre-release flag and a notes-start-tag for scoping the changelog.

| Input | Required | Default | Description |
|---|---|---|---|
| `release-version` | ✅ | — | SemVer string used for both the tag and the release name |
| `is-preview` | ✅ | — | `'true'` marks the release as pre-release |
| `notes-start-tag` | ❌ | `''` | Git tag from which to start auto-generated notes |

**Used by:** `release` workflow.

---

## github/write-file-to-summary

**Path:** `.github/actions/github/write-file-to-summary`  
**Description:** Appends the contents of a file (matched by glob) to the GitHub step summary (`GITHUB_STEP_SUMMARY`).

| Input | Required | Default | Description |
|---|---|---|---|
| `file-glob-pattern` | ✅ | — | Glob pattern for the file(s) to append |
| `working-directory` | ❌ | `${{ github.workspace }}` | Directory to resolve the glob against |

---

## nuget/publish-package

**Path:** `.github/actions/nuget/publish-package`  
**Description:** Downloads a NuGet package artifact and pushes it to either a public NuGet feed or an Azure Artifacts feed. Validates the `nuget-feed-server` value before proceeding.

| Input | Required | Default | Description |
|---|---|---|---|
| `package-artifact-name` | ✅ | — | Name of the artifact containing `.nupkg` files |
| `nuget-feed-url` | ✅ | — | Feed endpoint URL |
| `nuget-feed-api-key` | ✅ | — | API key / PAT for the feed |
| `nuget-feed-server` | ✅ | — | `'NuGet'` or `'AzureArtifacts'` |
| `dotnet-sdk-version` | ❌ | `10.x` | .NET SDK version |
| `working-directory` | ❌ | `${{ github.workspace }}` | Directory containing `.nupkg` files |

**Used by:** `build` (Development environment) and `release` (NuGet.org) workflows.

---

## source/compile

**Path:** `.github/actions/source/compile`  
**Description:** Builds a project in Release configuration, injecting version properties, and uploads the binary output as a workflow artifact.

| Input | Required | Default | Description |
|---|---|---|---|
| `assembly-version` | ✅ | — | `AssemblyVersion` MSBuild property |
| `assembly-informational-version` | ✅ | — | `AssemblyInformationalVersion` MSBuild property |
| `file-version` | ✅ | — | `FileVersion` MSBuild property |
| `treat-warnins-as-error` | ✅ | — | When `'true'`, runs `dotnet format analyzers --verify-no-changes` |
| `project-path` | ✅ | — | Glob pattern for project files |
| `dotnet_sdk_version` | ❌ | `10.x` | .NET SDK version |
| `build-configuration` | ❌ | `Release` | Build configuration |
| `build-platform` | ❌ | `Any CPU` | MSBuild platform |
| `upload-build-artifacts` | ❌ | `true` | Whether to upload binary output |
| `build-artifacts-name` | ❌ | `build` | Artifact name |

**Used by:** `build` and `pull-request` workflows.

---

## source/format

**Path:** `.github/actions/source/format`  
**Description:** Runs `dotnet format whitespace`, `dotnet format style`, and optionally `dotnet format analyzers` on the codebase, then pushes any changes back to the branch via `git/push-changes`.

| Input | Required | Default | Description |
|---|---|---|---|
| `project-path` | ✅ | — | Path or glob for the project/solution |
| `dotnet_sdk_version` | ❌ | `10.x` | .NET SDK version |
| `format-whitespace` | ❌ | `true` | Run `dotnet format whitespace` |
| `format-style` | ❌ | `true` | Run `dotnet format style` |
| `format-analyzers` | ❌ | `false` | Run `dotnet format analyzers` |
| `format-analyzers-diagnostics-parameter` | ❌ | `''` | Extra `--diagnostics` argument |

**Used by:** `build` workflow (`format` job).

---

## testing/test

**Path:** `.github/actions/testing/test`  
**Description:** Runs `dotnet test` with optional TRX logging and code coverage collection, then uploads all test result files as a workflow artifact.

| Input | Required | Default | Description |
|---|---|---|---|
| `project-path` | ✅ | — | Glob pattern for test project files |
| `test-results-directory` | ❌ | `test-results` | Directory where test outputs are written |
| `code-coverage-settings-file` | ❌ | `''` | Path to the coverage settings XML |
| `use-trf-logger` | ❌ | `true` | Enable TRX logger (`--report-trx`) |
| `collect-code-coverage` | ❌ | `true` | Enable code coverage (`--coverage`) |
| `code-coverage-output-format` | ❌ | `cobertura` | Coverage output format |
| `upload-test-artifacts` | ❌ | `true` | Upload collected test result files |
| `test-artifacts-name` | ❌ | `test-results` | Artifact name |
| `dotnet_sdk_version` | ❌ | `10.x` | .NET SDK version |

**Used by:** `build` and `pull-request` workflows (`test` job).

---

## testing/test-report

**Path:** `.github/actions/testing/test-report`  
**Description:** Installs `LiquidTestReports.Cli` and converts `.trx` files in the test result folder into a single Markdown report.

| Input | Required | Default | Description |
|---|---|---|---|
| `test-result-folder` | ✅ | — | Folder containing `.trx` files |
| `test-report-filename` | ❌ | `test-report.md` | Output filename |
| `dotnet_sdk_version` | ❌ | `10.x` | .NET SDK version |

| Output | Description |
|---|---|
| `test-report-file` | Full path to the generated Markdown report |

**Used by:** `build` and `pull-request` workflows (report written to step summary).

---

## testing/code-coverage

**Path:** `.github/actions/testing/code-coverage`  
**Description:** Merges multiple Cobertura coverage files using `dotnet-coverage`, then generates a Markdown summary report with `reportgenerator`.

| Input | Required | Default | Description |
|---|---|---|---|
| `test-result-folder` | ✅ | — | Folder containing `*.cobertura.xml` files |
| `dotnet_sdk_version` | ❌ | `10.x` | .NET SDK version |

| Output | Description |
|---|---|
| `code-coverage-report-file` | Path to the generated `Summary.md` |
| `code-coverage-merge-file` | Path to the merged Cobertura XML file |

**Used by:** `build` and `pull-request` workflows (report written to step summary).

---

## versioning/extract-version

**Path:** `.github/actions/versioning/extract-version`  
**Description:** Extracts a `MAJOR.MINOR` version from a branch name using a configurable regex (default `(\d+).(\d+)`). Falls back to a default version if no match is found.

| Input | Required | Default | Description |
|---|---|---|---|
| `branch-name` | ✅ | — | Branch name to parse |
| `default-version` | ❌ | `0.0` | Fallback when no version is found |
| `version-format` | ❌ | `(\d+).(\d+)` | Regex to extract the version |
| `dotnet_sdk_version` | ❌ | `10.x` | .NET SDK version |

| Output | Description |
|---|---|
| `version` | Extracted `MAJOR.MINOR` string |

**Used by:** All versioning-aware workflows.

---

## versioning/format-version

**Path:** `.github/actions/versioning/format-version`  
**Description:** Produces all version strings used for .NET assembly metadata and NuGet package versions from a base `MAJOR.MINOR` version plus context inputs.

| Input | Required | Description |
|---|---|---|
| `version` | ✅ | Base `MAJOR.MINOR` version |
| `patch` | ✅ | GitHub run number (used as patch segment) |
| `build-number` | ✅ | Commit count ahead of `main` |
| `sha` | ✅ | Commit SHA (appended to informational version) |
| `pre-release-tag` | ✅ | Pre-release label (`preview`, branch slug, or empty for stable) |

| Output | Description |
|---|---|
| `friendly-version` | `MAJOR.MINOR` (human-readable label) |
| `assembly-version` | `MAJOR.MINOR.patch.buildNumber` |
| `assembly-informational-version` | `MAJOR.MINOR.patch+sha` |
| `file-version` | Same as `assembly-version` |
| `release-version` | `MAJOR.MINOR.patch[-preTag.buildNumber]` (NuGet version) |

See [Versioning](./versioning.md) for the full pipeline.

---

## Creating a New Composite Action

1. Create a new directory under `.github/actions/<category>/<name>/`.
2. Add an `action.yml` file with `runs.using: composite`.
3. Declare all `inputs` with `description` and `required`.
4. Declare all `outputs` (if any) with `value` expressions referencing step outputs.
5. Keep each action focused on a single responsibility.
6. Reference optional `.NET SDK` version via an `inputs.dotnet_sdk_version` input (default `10.x`) for consistency with existing actions.
7. Use `actions/checkout@v6` as the first step when the action needs file access.

```yaml
name: 'My Action'
author: 'Pete Sramek'
description: 'Short description of what this action does.'
inputs:
  my-input:
    description: 'Description of the input.'
    required: true
  dotnet_sdk_version:
    description: '.NET SDK version. Default: 10.x'
    required: false
    default: '10.x'
outputs:
  my-output:
    description: 'Description of the output.'
    value: ${{ steps.my-step.outputs.my-output }}
runs:
  using: composite
  steps:
    - name: 'Checkout ${{ github.head_ref || github.ref }}'
      uses: actions/checkout@v6
    - name: 'My step'
      id: my-step
      shell: bash
      run: echo "my-output=value" >> $GITHUB_OUTPUT
```
