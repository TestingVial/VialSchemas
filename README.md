# Vial Schemas

This repository exposes the JSON Schemas used to validate [Testing Vial](https://www.code4it.dev/blog/testing-vial/) files.

[![Validate JSON Schemas](https://github.com/TestingVial/VialSchemas/actions/workflows/validate-schemas.yml/badge.svg)](https://github.com/TestingVial/VialSchemas/actions/workflows/validate-schemas.yml)

## What is the Testing Vial?

The Testing Vial is a testing strategy that organises tests around business entities and business value, rather than around the number of unit, integration, or end-to-end tests. Each vial groups the tests and architectural information related to an area of the system:

- End-to-end tests cover the most critical user flows.
- Integration tests cover business requirements, including happy paths and common unhappy paths.
- Unit tests cover edge cases that are difficult to reproduce through integration tests.
- Architectural tests help ensure that the system design and dependencies remain maintainable.

This approach is intended as a business-focused alternative to the Testing Pyramid and Testing Diamond. Read more in [Introducing the Testing Vial: a (better?) alternative to Testing Diamond and Testing Pyramid](https://www.code4it.dev/blog/testing-vial/).

## Repository Contents

- [`schemas/`](schemas/) contains the versioned JSON Schemas. Each schema's `$id` is its raw URL,
  `https://raw.githubusercontent.com/TestingVial/VialSchemas/main/schemas/v<version>.json`.
- [`versions.json`](versions.json) lists the published versions and their URLs.
- [`examples/`](examples/) contains examples of `.vial` files.
- [`tests/`](tests/) contains tests that validate the schemas against the examples.

## The `.vial` file format

A `.vial` file is a JSON document with a `schemaVersion` (which selects the schema in [`schemas/`](schemas/)) and a list of
`vials`. The current version is **[v0.0.2](schemas/v0.0.2.json)**; see [`examples/example02.vial`](examples/example02.vial).

| Field | Required | Value |
|---|---|---|
| `name` | yes | The vial (business entity or feature) the test covers, e.g. `Cart`. |
| `description` | yes | What the test verifies. Non-empty. |
| `type` | no | The test category, as free text: `unit`, `integration` and `e2e` are the common values, and custom categories such as `performance` are allowed. Write it in lowercase. Omit it for vials that have no category. |
| `technology` | no | The language or tool of the test, e.g. `csharp` or `playwright`. |
| `location` | yes | Either a source location (`className`, the fully qualified class name, and `methodName`, the method or `*` for the whole class) or a URL location (`url`). |

### Version history

| Version | Changes |
|---|---|
| [v0.0.2](schemas/v0.0.2.json) | `type` is optional free text instead of `unit` / `integration` / `e2e`. `location.filePath` is removed: a source location is `className` + `methodName`. |
| [v0.0.1](schemas/v0.0.1.json) | First version. |
