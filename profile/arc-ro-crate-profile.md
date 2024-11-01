# ARC RO-Crate Profile

* Version: 0.1
* Permalink: 
* Authors
  *  - https://orcid.org/

## Overview

In the ARC RO-Crate Profile, the RO-Crate [Root Data Entity](https://www.researchobject.org/ro-crate/specification/1.1/root-data-entity.html) MUST follow the [ISA Investigation profile](https://github.com/nfdi4plants/isa-ro-crate-profile/blob/release/profile/isa_ro_crate.md#investigation) from the [ISA RO-Crate Profile](https://github.com/nfdi4plants/isa-ro-crate-profile). 

[Studies](https://github.com/nfdi4plants/isa-ro-crate-profile/blob/release/profile/isa_ro_crate.md#study), [Assays](https://github.com/nfdi4plants/isa-ro-crate-profile/blob/release/profile/isa_ro_crate.md#assay), `Runs` and `Workflows` MUST be linked directly by the `Root Data Entity` through [hasPart](https://schema.org/hasPart) relationships.

