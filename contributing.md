# Contributing

This repository contains the ARC RO-Crate profile collection and the scripts used to generate the checked-in profile and example crates.

## Project Structure

The main source content lives under `docs/`:

- `docs/profiles/0.1/` contains the profile crates.
- `docs/examples/` contains the example crates referenced by the profiles.
- `docs/_site/` contains the generated site output that mirrors the published documentation.

The current profile collection includes:

- `ARC Administrative Crate`
- `ARC Datamap Crate`
- `ARC Process Core Crate`

Each profile has a matching example crate collection under `docs/examples/` with the same folder name.

## Scripts

The F# scripts in `scripts/` generate two kinds of crate metadata files:

- `profile crates` located in `docs/profiles/`:
    - `create_administrative_profile_crate.fsx` generates the Administrative profile crate metadata in `docs/profiles/0.1/administrative_crate/`.
    - `create_process_core_profile_crate.fsx` generates the Process Core profile crate metadata in `docs/profiles/0.1/process_core_crate/`.
    - `create_datamap_profile_crate.fsx` generates the Datamap profile crate metadata in `docs/profiles/0.1/datamap_crate/`.

- `example crates` located in `docs/examples/`:
    - `create_administrative_example.fsx` generates the Administrative example crate metadata in `docs/examples/administrative_crate/`.
    - `create_datamap_example.fsx` generates the Datamap example crate metadata in `docs/examples/datamap_crate/`.
    - `create_process_core_example.fsx` generates the Process Core example crate metadata in `docs/examples/process_core_crate/`.
    - `create_process_core_dfs_example.fsx` generates the Process Core DFS example data.

The Administrative scripts currently write to these paths:

- `docs/profiles/0.1/administrative_crate/ro-crate-metadata.json`
- `docs/examples/administrative_crate/ro-crate-metadata.json`

When you regenerate an example crate, update the matching `index.md` files as well so the published example pages stay in sync.

## How To Run The Scripts

Install [.NET](https://dotnet.microsoft.com/en-us/download) first if needed, then run the scripts from the repository root with F# Interactive:

```powershell
dotnet fsi scripts/create_administrative_profile_crate.fsx
```

## Editing Guidance

- Treat `scripts/` as the source of truth for generated crate metadata.
- Update the matching files in `docs/profiles/` and `docs/examples/` when the generation logic changes.
