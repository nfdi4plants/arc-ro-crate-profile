# Contributing

This repository contains the ARC RO-Crate profile collection and the scripts used to generate the checked-in profile and example crates.

## Cloning On Windows

The stable paths in `docs/profiles/` are symbolic links to the latest versioned profiles. On Windows, enable **Developer Mode** before cloning so Git can create these links without an elevated terminal. Then clone the repository with symbolic-link support enabled from the first checkout:

```powershell
git clone -c core.symlinks=true https://github.com/nfdi4plants/arc-ro-crate-profile.git
cd arc-ro-crate-profile
```

Check that the links were created correctly:

```powershell
Get-Item docs\profiles\administrative_crate | Select-Object Name, LinkType, Target
```

`LinkType` should be `SymbolicLink`. If the profile links are regular text files instead, setting `core.symlinks` after the checkout will not repair them. Remove that working copy and clone it again with the command above. If Windows still reports that creating symbolic links requires permission, run the clone from an elevated terminal.

## Project Structure

The main source content lives under `docs/`:

- `docs/profiles/` contains the latest version of the profile crates.
- `docs/examples/` contains the example crates referenced by the profiles.
- `docs/_site/` contains the generated site output that mirrors the published documentation.

The current profile collection includes:

Core Crates:
  - `ARC Administrative Crate`
  - `ARC Datamap Crate`
  - `ARC Process Core Crate`
Decorator Crates:
  - `ARC ISA RO-Crate`
  - `ARC Workflow Run RO-Crate`

Each profile has a matching example crate collection under `docs/examples/` with the same folder name.

## Updating The Latest Profile Symlinks

The links directly under `docs/profiles/` provide stable paths to the latest version. After the complete `docs/profiles/x.y/` (`x.y` as version placeholder) profile collection has been added, update all of these links together so they do not point at a mixture of versions.

On Windows, run the following commands from the repository root in PowerShell:

```powershell
$profileDirectoryLinks = @(
    "administrative_crate"
    "datamap_crate"
    "img"
    "isa_ro_crate"
    "process_core_crate"
    "wr_ro_crate"
)

Push-Location docs\profiles

foreach ($link in $profileDirectoryLinks) {
    Remove-Item -LiteralPath $link -Force
    cmd /c mklink /D $link "x.y\$link"
}

Remove-Item -LiteralPath index.md -Force
cmd /c mklink index.md x.y\index.md

Pop-Location
```

`Remove-Item` removes each symbolic link, not the directory or file it points to. Verify and stage the updated links:

```powershell
Get-Item docs\profiles\administrative_crate, docs\profiles\index.md |
    Select-Object Name, LinkType, Target

git add docs/profiles
git ls-files -s docs/profiles
```

The link targets should start with `x.y\`, and each link should have Git mode `120000` in the `git ls-files` output.

## Scripts

The F# scripts in `scripts/` generate two kinds of crate metadata files:

- `profile crates` located in `docs/profiles/`:
    - `create_administrative_profile_crate.fsx` generates the Administrative profile crate metadata in `docs/profiles/0.1/administrative_crate/`.
    - `create_process_core_profile_crate.fsx` generates the Process Core profile crate metadata in `docs/profiles/0.1/process_core_crate/`.
    - `create_datamap_profile_crate.fsx` generates the Datamap profile crate metadata in `docs/profiles/0.1/datamap_crate/`.
    - `create_isa_profile_crate.fsx` generates the ISA profile crate metadata in `docs/profiles/0.1/isa_ro_crate/`.
    - `create_wr_profile_crate.fsx` generates the Workflow Run profile crate metadata in `docs/profiles/0.1/wr_ro_crate/`.

- `example crates` located in `docs/examples/`:
    - `create_administrative_example.fsx` generates the Administrative example crate metadata in `docs/examples/administrative_crate/`.
    - `create_datamap_example.fsx` generates the Datamap example crate metadata in `docs/examples/datamap_crate/`.
    - `create_process_core_example.fsx` generates the Process Core example crate metadata in `docs/examples/process_core_crate/`.
    - `create_process_core_dfs_example.fsx` generates the Process Core DFS example data.
    - `create_wr_example.fsx` generates the Workflow Run example crate metadata in `docs/examples/wr_ro_crate/`.

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
