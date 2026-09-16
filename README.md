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

- [`schemas/`](schemas/) contains the versioned JSON Schemas.
- [`examples/`](examples/) contains examples of `.vial` files.
- [`tests/`](tests/) contains tests that validate the schemas against the examples.
