# ARC RO-Crate Profile Collection

The Annotated Research Context (ARC) RO-Crate Profile Collection is a set of profiles that focus on providing meaning and context to research data. Each of the profiles in this collection focuses on a distinct aspect of this context, and they can be used independently or in combination to provide a more complete understanding of the research data.

- [Administrative Crate](./administrative_crate/index.md) describes the administrative information associated with research data, including information about the creators, contributors, and rights holders of the data.
- [Semantic Designation](./0.1/semantic_designation/index.md) describes the semantic annotation of data and contextual entities.
- [Process Core Crate](./process_core_crate/index.md) describes provenance of research entities by capturing the processes that generate and transform them.

In addition to the base profiles above, the following profiles are built on top of them to provide additional context and meaning to research data following specific formats or models:

- [ISA RO-Crate](./isa_ro_crate/index.md) is an instatiation of Administrative Crate and Process Core Crate to describe datasets following the Investigation-Study-Assay (ISA) model.
- [ARC Workflow Run RO-Crate](./wr_ro_crate/index.md) is an instantiation of Process Core Crate to describe computational workflows and their invocations (runs), merging the Workflow Run Crate profile collection with the ISA process model.
- [Datamap Crate](./datamap_crate/index.md) is an instantiation of Semantic Designation to describe the structural and semantic details of data files and fragments of data files.
