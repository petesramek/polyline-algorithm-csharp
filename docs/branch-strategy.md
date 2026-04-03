# Branch Strategy

This document describes the branch model, the purpose of each branch type, and how a change moves from a feature branch all the way to a stable release.

## Branch Types

| Pattern | Purpose | Protected |
|---|---|---|
| `main` | Latest stable source of truth | ✅ Yes |
| `develop/X.Y` | Active feature development sink for version X.Y | ✅ Yes (PR only) |
| `support/X.Y` | Maintenance / backport development sink for version X.Y | ✅ Yes (PR only) |
| `feature/<id>-<description>` | Individual feature work, merged into `develop/X.Y` via PR | ❌ No |
| `bugfix/<id>-<description>` | Bug fix work, merged into `support/X.Y` via PR | ❌ No |
| `preview/X.Y` | Pre-release stabilization | ✅ Yes (1 approval required) |
| `release/X.Y` | Release stabilization | ✅ Yes (1 approval required) |

## Change Lifecycle

```
1. Feature work
   └─ feature/123-my-feature
          │
          │  PR → develop/X.Y
          │
2. Bug fix work
   └─ bugfix/124-my-fix
          │
          │  PR → support/X.Y
          │
3. Promote to preview
   └─ promote-branch.yml (manual) → creates preview/X.Y + PR: develop/X.Y → preview/X.Y
          │
          │  PR open → [pull-request.yml]: compile, test, pack, benchmark (optional)
          │  PR merged → [release.yml]: compile, test, pack, publish-NuGet (pre-release), GitHub release, docs
          │
4. Promote to release
   └─ promote-branch.yml (manual) → creates release/X.Y + PR: preview/X.Y → release/X.Y
          │
          │  PR open → [pull-request.yml]
          │  PR merged → [release.yml]: publish-NuGet (stable), GitHub release, docs, creates support/X.Y
          │
5. Back-merge (optional)
   └─ Manual PR: release/X.Y → main
```

## Rules Per Branch Type

### `main`

- Represents the current stable release.
- Direct pushes are not allowed (protected).
- Updated by merging from `release/X.Y` after a stable release.
- The `build.yml` workflow does **not** trigger on `main` pushes (branch-ignore pattern excludes `preview/**` and `release/**`, and `main` does not match `src/**` changes by default in the context of the ignore rules — check the workflow for current specifics).

### `develop/X.Y`

- Naming convention: `develop/<major>.<minor>` (e.g. `develop/1.2`).
- Protected: all changes are merged via pull request from `feature/**` branches.
- The `build.yml` CI pipeline runs on every push to `src/`.
- When ready for stabilization, use `promote-branch.yml` to create a `preview/X.Y` branch and open a PR.

### `support/X.Y`

- Naming convention: `support/<major>.<minor>` (e.g. `support/1.0`).
- Auto-created when the first stable release from `release/X.Y` is published.
- Protected: all changes are merged via pull request from `bugfix/**` branches.
- Can be promoted to `preview/X.Y` for a patch release.

### `feature/<id>-<description>`

- Short-lived branch for individual feature work (e.g. `feature/123-async-decoder`).
- Merged into the appropriate `develop/X.Y` via pull request.
- Not protected — deleted after merging.

### `bugfix/<id>-<description>`

- Short-lived branch for bug fixes (e.g. `bugfix/124-decode-overflow`).
- Merged into the appropriate `support/X.Y` via pull request.
- Not protected — deleted after merging.

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
- After the first stable release, a corresponding `support/X.Y` branch is auto-created.

## Version in Branch Names

The `X.Y` in `preview/X.Y` and `release/X.Y` drives the version pipeline. See [Versioning](./versioning.md) for details.

## Environments

| GitHub Environment | Used by | NuGet feed |
|---|---|---|
| `Development` | `build.yml`, `pull-request.yml` | Azure Artifacts |
| `Production` | `release.yml` | NuGet.org |

## Locking and Unlocking Branches

`preview/**` and `release/**` branches are locked via the [`github/branch-protection/lock`](./composite-actions.md#githubbranch-protectionlock) composite action when created. `develop/X.Y` and `support/X.Y` branches must be manually configured as protected in repository settings (PR required, no direct pushes). The [`github/branch-protection/unlock`](./composite-actions.md#githubbranch-protectionunlock) action temporarily removes protection when a workflow needs to push directly (e.g., `bump-version.yml`). Branches are always re-locked immediately after.
