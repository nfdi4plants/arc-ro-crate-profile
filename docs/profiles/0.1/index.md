# ARC RO-Crate Profile Collection

The Annotated Research Context (ARC) RO-Crate Profile Collection is a set of profiles that focus on providing meaning and context to research data. Each of the profiles in this collection focuses on a distinct aspect of this context, and they can be used independently or in combination to provide a more complete understanding of the research data.

- [Administrative Core](./administrative_core/index.md) describes the administrative information associated with research data, including information about the creators, contributors, and rights holders of the data.
- [Semantic Core](./semantic_core/index.md) describes two core principles for semantic annotation of data and contextual entities, as well as where to use them.
- [Provenance Core](./provenance_core/index.md) describes provenance of research entities by capturing the processes that generate and transform them.

In addition to the base profiles above, the following profiles are built on top of them to provide additional context and meaning to research data following specific formats or models:

- [ISA RO-Crate](./isa_ro_crate/index.md) is a decoration of Administrative Core and Provenance Core to describe datasets following the Investigation-Study-Assay (ISA) model.
- [ARC Workflow Run RO-Crate](./wr_ro_crate/index.md) is a decoration of Provenance Core to describe computational workflows and their invocations (runs), merging the Workflow Run Crate profile collection with the ISA process model.
- [Datamap Crate](./datamap_crate/index.md) is a decoration of Semantic Core to describe the structural and semantic details of data files and fragments of data files.
