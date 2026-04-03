# Branch Strategy

This document describes the branch model, the purpose of each branch type, and how a change moves from a feature branch all the way to a stable release.

## Branch Types

| Pattern | Purpose | Protected |
|---|---|---|
| `main` | Latest stable source of truth | ✅ Yes |
| `develop/**` | Active feature development | ❌ No |
| `support/**` | Maintenance / backport development | ❌ No |
| `preview/X.Y` | Pre-release stabilization | ✅ Yes (1 approval required) |
| `release/X.Y` | Release stabilization | ✅ Yes (1 approval required) |

## Change Lifecycle

```
1. Feature work
   └─ develop/my-feature  (or support/my-fix for backports)
          │
          │  push to src/ → [build.yml] runs: format, compile, test, pack, publish-dev
          │
2. Promote to preview
   └─ promote-branch.yml (manual) → creates preview/X.Y + PR: develop → preview/X.Y
          │
          │  PR open → [pull-request.yml]: compile, test, pack, benchmark (optional)
          │  PR merged → [release.yml]: compile, test, pack, publish-NuGet (pre-release), GitHub release, docs
          │
3. Promote to release
   └─ promote-branch.yml (manual) → creates release/X.Y + PR: preview/X.Y → release/X.Y
          │
          │  PR open → [pull-request.yml]
          │  PR merged → [release.yml]: publish-NuGet (stable), GitHub release, docs
          │
4. Back-merge (optional)
   └─ Manual PR: release/X.Y → main
```

## Rules Per Branch Type

### `main`

- Represents the current stable release.
- Direct pushes are not allowed (protected).
- Updated by merging from `release/X.Y` after a stable release.
- The `build.yml` workflow does **not** trigger on `main` pushes (branch-ignore pattern excludes `preview/**` and `release/**`, and `main` does not match `src/**` changes by default in the context of the ignore rules — check the workflow for current specifics).

### `develop/**`

- Naming convention: `develop/<description>` (e.g. `develop/async-decoder`, `develop/1.2`).
- The `build.yml` CI pipeline runs on every push to `src/`.
- When ready for stabilization, use `promote-branch.yml` to create a `preview/X.Y` branch and open a PR.

### `support/**`

- Used for backport and maintenance work against older versions.
- Same CI behavior as `develop/**`.
- Can be promoted to `preview/X.Y` for a patch release.

### `preview/X.Y`

- Created automatically by `promote-branch.yml`.
- Locked immediately: requires at least one PR approval before any merge.
- The `pull-request.yml` workflow runs on every PR targeting this branch.
- On merge, `release.yml` publishes a **pre-release** NuGet package.
- When all pre-release validation is done, promote to `release/X.Y`.

### `release/X.Y`

- Created automatically by `promote-branch.yml` from `preview/X.Y`.
- Locked immediately: requires at least one PR approval.
- On merge, `release.yml` publishes a **stable** NuGet package and a GitHub release.

## Version in Branch Names

The `X.Y` in `preview/X.Y` and `release/X.Y` drives the version pipeline. See [Versioning](./versioning.md) for details.

## Environments

| GitHub Environment | Used by | NuGet feed |
|---|---|---|
| `Development` | `build.yml`, `pull-request.yml` | Azure Artifacts |
| `Production` | `release.yml` | NuGet.org |

## Locking and Unlocking Branches

`preview/**` and `release/**` branches are locked via the [`github/branch-protection/lock`](./composite-actions.md#githubbranch-protectionlock) composite action when created. The [`github/branch-protection/unlock`](./composite-actions.md#githubbranch-protectionunlock) action temporarily removes protection when a workflow needs to push directly (e.g., `bump-version.yml`). Branches are always re-locked immediately after.
