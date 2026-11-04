# VialSchemas

JSON Schemas (draft 2020-12) that define the `.vial` file format, plus examples and tests that validate them.

Part of the TestingVial workspace: cross-repo rules, issue tracking and the propagation checklist are in `../CLAUDE.md`
(present when this repo is cloned inside the `TestingVial/workspace` checkout). Every change needs an issue on
<https://github.com/orgs/TestingVial/projects/1> referenced from its PR.

## Layout

- `schemas/v<version>.json` — one file per schema version. `schemaVersion` in a `.vial` file selects it.
- `versions.json` — index of published versions and their raw URLs.
- `examples/*.vial` — sample files. Every file here is validated against the schema named by its `schemaVersion`.
- `tests/VialSchemas.Tests` — xunit (net8.0, JsonSchema.Net); test cases are discovered from `examples/` at run time.

## Build and test

```bash
dotnet test tests/VialSchemas.Tests
```

CI: `.github/workflows/validate-schemas.yml` runs the same on push/PR to `main`.

## Versioning rules

- **Never change a published schema file.** A change to the format means a new `schemas/vX.Y.Z.json`, a new `versions.json`
  entry, and at least one example using it.
- Keep `$id` and `versions.json` URLs pointing at `https://raw.githubusercontent.com/TestingVial/VialSchemas/main/...`
  and at files that actually exist.
- Consumers of the schema: Pipette.NET writes it (and embeds a copy in its tests); TestingVial.NET decides which test types users can
  express. A new version usually needs follow-up issues in both.
