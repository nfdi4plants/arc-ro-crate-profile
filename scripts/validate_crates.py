"""Validate every RO-Crate under docs/ with rocrate-validator and --extra-checks.

    pip install roc-validator==0.13.1     # as pinned in .github/workflows/validate-crates.yml
    python scripts/validate_crates.py

A crate is a directory holding `ro-crate-metadata.json` (examples and profile
crates alike), or any other JSON file that carries its own metadata descriptor,
such as `docs/examples/provenance-core/dfs_example.json`. The validator only
accepts the canonical file name, so those are validated from a temporary copy.

The exit code is non-zero if any crate fails. Findings of the extra checks are
reported as warnings: the validator defines them as not affecting conformance.
"""

from __future__ import annotations

import json
import os
import shutil
import subprocess
import sys
import tempfile
from dataclasses import dataclass, field
from pathlib import Path

REPO = Path(__file__).resolve().parent.parent
DOCS = REPO / "docs"
EXAMPLES = DOCS / "examples"
METADATA = "ro-crate-metadata.json"

# Local build output and tooling; never part of the sources.
IGNORED_DIRS = {"_site", "vendor", ".jekyll-cache", ".bundle"}

# The example crates are published as metadata only: the files they describe are
# not in this repository, so the payload-presence check can never pass for them.
# `--metadata-only` would skip it as well, but it also disables the extra checks.
# Check ids are positional: confirm this one when changing the validator version
# (`rocrate-validator profiles describe ro-crate-1.2 -v`).
EXAMPLE_SKIPPED_CHECKS = ["ro-crate-1.2_18.1"]  # Data Entity: REQUIRED resource availability


@dataclass
class Result:
    crate: str  # metadata file, relative to the repository root
    passed: bool = False
    issues: list[str] = field(default_factory=list)
    extra_findings: list[str] = field(default_factory=list)
    error: str = ""  # validator output, if it produced no report


def find_validator() -> str:
    # Look next to the running interpreter first, so a venv works without being activated.
    search_path = os.pathsep.join([str(Path(sys.executable).parent), os.environ.get("PATH", "")])
    validator = shutil.which("rocrate-validator", path=search_path)
    if validator is None:
        sys.exit("rocrate-validator not found. Install it with: pip install roc-validator")
    return validator


def has_metadata_descriptor(path: Path) -> bool:
    try:
        graph = json.loads(path.read_text(encoding="utf-8"))["@graph"]
    except (ValueError, KeyError, TypeError):
        return False
    return any(isinstance(entity, dict) and entity.get("@id") == METADATA for entity in graph)


def find_crates() -> list[Path]:
    """Metadata files of all crates in docs/, in a stable order."""
    crates = []
    # os.walk does not follow the `docs/profiles/<name>` symlinks, so each profile
    # crate is found once, in its versioned directory.
    for directory, subdirectories, files in os.walk(DOCS):
        subdirectories[:] = sorted(d for d in subdirectories if d not in IGNORED_DIRS)
        for name in sorted(files):
            path = Path(directory, name)
            if name == METADATA or (name.endswith(".json") and has_metadata_descriptor(path)):
                crates.append(path)
    return crates


def describe(finding: dict, check_id: str, with_property: bool = False) -> str:
    text = f"[{check_id}] {finding['message']}"
    entity = finding.get("violatingEntity")
    if entity:
        text += f" (entity: {entity}"
        if with_property and finding.get("violatingProperty"):
            text += f", property: {finding['violatingProperty']}"
        text += ")"
    return " ".join(text.split())


def validate(validator: str, metadata: Path, workdir: Path) -> Result:
    result = Result(crate=metadata.relative_to(REPO).as_posix())

    crate_root = metadata.parent
    if metadata.name != METADATA:
        crate_root = workdir / "crate"
        crate_root.mkdir()
        shutil.copyfile(metadata, crate_root / METADATA)

    report_file = workdir / "report.json"
    command = [validator, "--no-interactive", "--disable-color", "validate", "--extra-checks"]
    if EXAMPLES in metadata.parents:
        command += ["--skip-checks", ",".join(EXAMPLE_SKIPPED_CHECKS)]
    command += ["--output-format", "json", "--output-file", str(report_file), str(crate_root)]

    process = subprocess.run(
        command,
        capture_output=True,
        encoding="utf-8",
        errors="replace",
        env={**os.environ, "PYTHONUTF8": "1"},
    )
    if not report_file.is_file():
        result.error = (process.stdout + process.stderr).strip() or f"exit code {process.returncode}"
        return result

    report = json.loads(report_file.read_text(encoding="utf-8"))
    result.passed = process.returncode == 0 and report["passed"]
    # Issue messages already name the property; extra-check findings do not.
    result.issues = [describe(issue, issue["check"]["identifier"]) for issue in report["issues"]]
    result.extra_findings = [describe(finding, finding["check"], with_property=True) for finding in report["extra_checks"]]
    if not result.passed and not result.issues:
        result.error = (process.stdout + process.stderr).strip() or f"exit code {process.returncode}"
    return result


def annotate(level: str, result: Result, message: str) -> None:
    """Attach a message to the crate's file in the GitHub pull request view."""
    if os.environ.get("GITHUB_ACTIONS") == "true":
        message = message.replace("%", "%25").replace("\r", "%0D").replace("\n", "%0A")
        print(f"::{level} file={result.crate},title=rocrate-validator::{message}")


def print_result(result: Result) -> None:
    status = "PASS" if result.passed else "FAIL"
    counts = f"{len(result.issues)} issue(s), {len(result.extra_findings)} extra-check finding(s)"
    print(f"{status}  {result.crate}  ({counts})")
    for issue in result.issues:
        print(f"      {issue}")
    for finding in result.extra_findings:
        print(f"      extra check: {finding}")
    if result.error:
        print("      " + result.error.replace("\n", "\n      "))

    if not result.passed:
        annotate("error", result, "\n".join(["Not a valid RO-Crate:", *(result.issues or [result.error])]))
    if result.extra_findings:
        annotate("warning", result, "\n".join(["Extra-check findings:", *result.extra_findings]))


def write_summary(results: list[Result], version: str) -> None:
    """Render the results on the workflow run page."""
    summary_file = os.environ.get("GITHUB_STEP_SUMMARY")
    if not summary_file:
        return

    lines = [
        "## RO-Crate validation",
        "",
        f"{version}, run with `--extra-checks`.",
        "",
        "| Crate | Result | Issues | Extra-check findings |",
        "| --- | --- | --- | --- |",
    ]
    for result in results:
        status = "✅ valid" if result.passed else "❌ invalid"
        lines.append(f"| `{result.crate}` | {status} | {len(result.issues)} | {len(result.extra_findings)} |")

    for result in results:
        if result.passed and not result.extra_findings:
            continue
        lines += ["", f"### `{result.crate}`", ""]
        lines += [f"- ❌ {issue}" for issue in result.issues]
        lines += [f"- ⚠️ extra check: {finding}" for finding in result.extra_findings]
        if result.error:
            lines += ["", "```", result.error, "```"]

    with open(summary_file, "a", encoding="utf-8") as summary:
        summary.write("\n".join(lines) + "\n")


def main() -> int:
    # Validator messages quote crate content; never let an unencodable character abort the run.
    sys.stdout.reconfigure(errors="replace")

    validator = find_validator()
    version = subprocess.run([validator, "--version"], capture_output=True, text=True).stdout.strip()
    crates = find_crates()
    if not crates:
        sys.exit(f"No crates found under {DOCS}")
    print(f"{version}: validating {len(crates)} crate(s) with --extra-checks\n", flush=True)

    results = []
    for metadata in crates:
        with tempfile.TemporaryDirectory() as workdir:
            result = validate(validator, metadata, Path(workdir))
        print_result(result)
        sys.stdout.flush()
        results.append(result)

    write_summary(results, version)

    failed = [result for result in results if not result.passed]
    findings = sum(len(result.extra_findings) for result in results)
    print(f"\n{len(results) - len(failed)} of {len(results)} crate(s) valid, {findings} extra-check finding(s).")
    return 1 if failed else 0


if __name__ == "__main__":
    sys.exit(main())
