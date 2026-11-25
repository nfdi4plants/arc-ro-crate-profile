#r "nuget: ROCratePCC, 0.3.0"

open ARCtrl.ROCrate
open ARCtrl.Json
open ROCratePCC

module License = 

    let mit = License(
        iri = "https://mit-license.org/",
        name = "MIT License"
    )

module Organization =
    /// RPTU Kaiserslautern-Landau
    let rptu = Organization(
        name = "RPTU Kaiserslautern-Landau",
        url = "https://ror.org/01qrts582"
    )

    /// The University of Manchester
    let uom = Organization(
        name = "The University of Manchester",
        url = "https://ror.org/027m9bs27"
    )

    /// Forschungszentrum Jülich
    let fzj = Organization(
        name = "Forschungszentrum Jülich",
        url = "https://ror.org/02nv7yv05"
    )

    let dataPLANT = Organization(
        name = "DataPLANT",
        url = "https://nfdi4plants.de/"
    )
    dataPLANT.SetProperty("http://schema.org/alternateName", "NFDI4Plants")

module People =

    let fw = Author(orcid = "0000-0002-5526-7138", name = "Florian Wetzels", affiliation = Organization.rptu)
    let hlw = Author(orcid = "0000-0003-1945-6342", name = "Heinrich Lukas Weil", affiliation = Organization.rptu)
    let ks = Author(orcid = "0000-0002-2198-5262", name = "Kevin Schneider", affiliation = Organization.rptu)
    let so = Author(orcid = "0000-0003-2130-0865", name = "Stuart Owen", affiliation = Organization.uom)
    let co = Author(orcid = "0000-0003-1512-9504", name = "Caroline Ott", affiliation = Organization.rptu)
    let sb = Author(orcid = "0000-0002-2177-8781", name = "Sebastian Beier", affiliation = Organization.fzj)
    let tm = Author(orcid = "0000-0003-3925-6778", name = "Timo Mühlhaus", affiliation = Organization.rptu)
    let cg = Author(orcid = "0000-0003-1669-8549", name = "Christoph Garth", affiliation = Organization.rptu)

    let defaultAuthors = ResizeArray [
        fw; hlw; ks; so; co; sb; tm
    ]

    let semanticAnnotationAuthors = ResizeArray [
        hlw; fw; tm; cg
    ]


module Types = 

    let dataset = UsedType(iri = "https://schema.org/Dataset", name = "Dataset")
    let labProcess = UsedType(iri = "https://bioschemas.org/types/LabProcess/0.1-DRAFT", name = "LabProcess", termCode = "LabProcess")
    let labProtocol = UsedType(iri = "https://bioschemas.org/types/LabProtocol/0.5-DRAFT", name = "LabProtocol", termCode = "LabProtocol")
    let propertyValue = UsedType(iri = "https://schema.org/PropertyValue", name = "PropertyValue")
    let definedTerm = UsedType(iri = "https://schema.org/DefinedTerm", name = "DefinedTerm")
    let statement = UsedType(iri = "https://schema.org/Statement", name = "Statement")
    let itemList = UsedType(iri = "https://schema.org/ItemList", name = "ItemList")
    let person = UsedType(iri = "https://schema.org/Person", name = "Person")
    let scholarlyArticle = UsedType(iri = "https://schema.org/ScholarlyArticle", name = "ScholarlyArticle")
    let comment = UsedType(iri = "https://schema.org/Comment", name = "Comment")
    let mediaObject = UsedType(iri = "https://schema.org/MediaObject", name = "MediaObject")
    let sample = UsedType(iri = "https://bioschemas.org/types/Sample/0.3-DRAFT", name = "Sample", termCode = "Sample")
    let organization = UsedType(iri = "https://schema.org/Organization", name = "Organization")
    let createAction = UsedType(iri = "https://schema.org/CreateAction", name = "CreateAction")
    let softwareSourceCode = UsedType(iri = "https://schema.org/SoftwareSourceCode", name = "SoftwareSourceCode")
    let softwareApplication = UsedType(iri = "https://schema.org/SoftwareApplication", name = "SoftwareApplication")
    let computationalWorkflow = UsedType(iri = "https://bioschemas.org/types/ComputationalWorkflow/1.0-RELEASE", name = "ComputationalWorkflow", termCode = "ComputationalWorkflow")
    let formalParameter = UsedType(iri = "https://bioschemas.org/types/FormalParameter/1.0-RELEASE", name = "FormalParameter", termCode = "FormalParameter")

module Profile =

    /// Version of the profile collection. Part of the permalinks and of the paths below `docs/profiles/`.
    let version = "0.1"

    /// Namespace of the persistent identifiers (permalinks) of the profile collection, registered at https://w3id.org.
    ///
    /// `<permalinkRoot>/<profile>/<version>` redirects to the published profile description and, if
    /// `application/ld+json` is requested, to `ro-crate-metadata.jsonld` next to it, a symbolic link
    /// to the profile crate (`ro-crate-metadata.json`).
    let permalinkRoot = "https://w3id.org/arc/ro-crate"

    /// Web location the `docs/` folder of this repository is published to (GitHub Pages).
    ///
    /// Only use it for resources that have no permalink of their own, e.g. the example crates.
    /// Repository URLs (`https://github.com/<org>/<repo>/tree/...`) are not identifiers:
    /// they depend on a branch name and do not serve the file content.
    let siteRoot = "https://nfdi4plants.github.io/arc-ro-crate-profile"

    /// Versioned permalink of a profile, e.g. `https://w3id.org/arc/ro-crate/isa/0.1`.
    ///
    /// This is the `@id` of the root data entity of the profile crate, the `about` of every resource
    /// describing the profile, and the IRI conforming crates reference with `conformsTo`.
    let permalink (subId : string) = $"{permalinkRoot}/{subId}/{version}"

    /// Download URL of the example crate published for a profile.
    let exampleUrl (subId : string) = $"{siteRoot}/examples/{subId}/ro-crate-metadata.json"

/// Profiles that other profiles build on. A profile states them with `ConformsTo` in its resource
/// descriptors: crates of that profile should conform to the stated profiles as well.
module RelatedProfiles =

    /// A profile of this collection, referenced by its versioned permalink.
    let private ofCollection (subId : string) (name : string) =
        RelatedProfile(iri = Profile.permalink subId, name = name, version = Profile.version)

    let administrativeCore = ofCollection "administrative-core" "Administrative Core Profile"
    let provenanceCore = ofCollection "provenance-core" "Provenance Core Profile"
    let semanticCore = ofCollection "semantic-core" "Semantic Core Profile"

    /// Workflow Run Crate, https://www.researchobject.org/workflow-run-crate/profiles/workflow_run_crate
    let workflowRunCrate = RelatedProfile(
        iri = "https://w3id.org/ro/wfrun/workflow/0.5",
        name = "Workflow Run Crate",
        version = "0.5"
    )