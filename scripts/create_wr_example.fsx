#r "nuget: ARCtrl, 3.1.0"

open ARCtrl
open ARCtrl.ROCrate
open ARCtrl.Json

// People and organizations involved in the example

let timo = LDPerson.create(
    givenName = "Timo",
    familyName = "Mühlhaus",
    orcid = "https://orcid.org/0000-0003-3925-6778"
)

let caroline = LDPerson.create(
    givenName = "Caroline",
    familyName = "Ott",
    orcid = "https://orcid.org/0000-0003-1512-9504"
)

// The Administrative Core profile recommends the full name of a person next to givenName and familyName.
// LDPerson.create has no parameter for it, so it is set on the created nodes.

timo.SetProperty("http://schema.org/name", "Timo Mühlhaus")
caroline.SetProperty("http://schema.org/name", "Caroline Ott")

let dataHub = LDOrganization.create(
    name = "DataPLANT DataHub",
    id = "https://git.nfdi4plants.org/explore"
)

// Languages the workflow is written in

let fsharp = LDComputerLanguage.create(
    id = "#FSharp",
    name = "F Sharp",
    alternateName = "F#",
    url = "https://dotnet.microsoft.com/en-us/languages/fsharp"
)

let cwl = LDComputerLanguage.createCWL()

// RO-Crate 1.2 requires a version on every ComputerLanguage.
// LDComputerLanguage.create has no parameter for it, so it is set on the created nodes.

fsharp.SetProperty("http://schema.org/version", "11.0")
cwl.SetProperty("http://schema.org/version", "v1.2")

// Encoding formats of the parameters, taken from EDAM

let csv = LDDefinedTerm.create(
    name = "Comma-separated values",
    termCode = "http://edamontology.org/format_3752"
)

let tsv = LDDefinedTerm.create(
    name = "Tab-separated values",
    termCode = "http://edamontology.org/format_3475"
)

// Prospective provenance: the parameter slots the workflow declares

let intensityTable = LDFormalParameter.create(
    additionalType = "File",
    id = "#intensity_table",
    name = "intensity_table",
    encodingFormats = ResizeArray [csv]
)

let fileName = LDFormalParameter.create(
    additionalType = "Text",
    id = "#file_name",
    name = "file_name"
)

let summedIntensities = LDFormalParameter.create(
    additionalType = "File",
    id = "#summed_intensities",
    name = "summed_intensities",
    encodingFormats = ResizeArray [tsv]
)

// The workflow description itself, a multi-type object of
// MediaObject, SoftwareSourceCode, ComputationalWorkflow and LabProtocol

let workflowProtocol = LDWorkflowProtocol.create(
    id = "workflows/FixedScript/workflow.cwl",
    name = "Column Addition",
    inputs = ResizeArray [intensityTable; fileName],
    outputs = ResizeArray [summedIntensities],
    creator = timo,
    programmingLanguages = ResizeArray [fsharp; cwl],
    sdPublisher = dataHub,
    url = "https://git.nfdi4plants.org/muehlhaus/ArcPrototype/-/blob/main/workflows/FixedScript/workflow.cwl",
    version = "0.1",
    description = "Sums up the intensity columns of a table and writes the result to a new file."
)

// The workflow folder of the ARC, bundling the workflow description

let arcWorkflow = LDDataset.createARCWorkflow(
    identifier = "FixedScript",
    mainEntities = ResizeArray [workflowProtocol],
    id = "workflows/FixedScript/",
    name = "Column Addition",
    creators = ResizeArray [timo],
    hasParts = ResizeArray [workflowProtocol]
)

// Retrospective provenance: the values the parameter slots were filled with

let intensityTableValue = LDFile.createCWLParameter(
    name = "assays/measurement1/dataset/table.csv",
    exampleOfWork = intensityTable
)

let fileNameValue = LDPropertyValue.createCWLParameter(
    exampleOfWork = fileName,
    name = "file_name",
    values = ResizeArray ["summed_intensities.csv"]
)

let summedIntensitiesValue = LDFile.createCWLParameter(
    name = "runs/fsResult1/summed_intensities.csv",
    exampleOfWork = summedIntensities
)

// The execution of the workflow, a multi-type object of CreateAction and LabProcess

let workflowInvocation = LDWorkflowInvocation.create(
    name = "Column Addition with fixed script",
    instrument = workflowProtocol,
    objects = ResizeArray [intensityTableValue; fileNameValue],
    results = ResizeArray [summedIntensitiesValue],
    agents = ResizeArray [caroline],
    id = "#wfrun-fixed-script"
)

// The profile requires executesLabProtocol to be present and equal to instrument
LDLabProcess.setExecutesLabProtocol(workflowInvocation, workflowProtocol)

// The run folder of the ARC, bundling the workflow invocation.
// The profile links the run to its invocation(s) via equal about and mentions properties.

let arcRun = LDDataset.createARCRun(
    identifier = "fsResult1",
    mainEntities = ResizeArray [workflowInvocation],
    id = "runs/fsResult1/",
    name = "Column Addition Result",
    creators = ResizeArray [caroline],
    hasParts = ResizeArray [summedIntensitiesValue]
)

arcRun.RemoveProperty(LDDataset.mainEntity) |> ignore
LDDataset.setAbouts(arcRun, ResizeArray [workflowInvocation])
arcRun.SetProperty(LDDataset.mentions, ResizeArray [workflowInvocation])

let dataset = LDDataset.create(
    id = "./",
    hasParts = ResizeArray [arcWorkflow; arcRun; intensityTableValue],
    datePublished = System.DateTime.Now
)

// Declare which profile, and which section of it, each entity follows.
// The base IRI is the permalink of the profile; the anchors are the headings of its index.md.
// As RO-Crate 1.2 recommends, the profile as a whole is declared on the root data entity.

let profileId = "https://w3id.org/arc/ro-crate/wr/0.1"

/// Appends a conformsTo reference, keeping any the node already declares.
let addConformsTo (target: string) (node: LDNode) =
    let refs = node.GetPropertyValues(LDDataset.conformsTo)
    refs.Add(box (LDRef(target)))
    node.SetProperty(LDDataset.conformsTo, refs)

dataset            |> addConformsTo profileId
arcWorkflow        |> addConformsTo $"{profileId}#arc-workflow"
workflowProtocol   |> addConformsTo $"{profileId}#workflow-protocol"
arcRun             |> addConformsTo $"{profileId}#arc-run"
workflowInvocation |> addConformsTo $"{profileId}#workflow-invocation"

let graph = ARC.ROCrate.packDatasetAsCrate(
    dataset, datasetName = "ARC Workflow Run Crate Example",
    datasetDescription = "An example of a ROCrate describing a computational workflow and its invocation, covering both prospective and retrospective provenance.",
    metadataFileDescriptor = None,
    license = License.GetDefaultLicense()
    )

// Contextual entities of type Profile for this profile and for the
// external profiles the ARC Run declares conformance to

let referencedProfiles = [
    profileId, "ARC Workflow Run RO-Crate Profile", "0.1"
    "https://w3id.org/ro/wfrun/process/0.5", "Process Run Crate", "0.5"
    "https://w3id.org/ro/wfrun/workflow/0.5", "Workflow Run Crate", "0.5"
    "https://w3id.org/workflowhub/workflow-ro-crate/1.0", "Workflow RO-Crate", "1.0"
]

let graphContext = graph.TryGetContext() |> Option.get

for id, name, version in referencedProfiles do
    let profile = LDCreativeWork.create(id = id, name = name)
    profile.SchemaType.Add("http://www.w3.org/ns/dx/prof/Profile")
    profile.SetProperty("http://schema.org/version", version)
    profile.Compact_InPlace(graphContext)
    graph.AddNode(profile)

let jsonString = graph.ToROCrateJsonString(spaces = 2)
System.IO.File.WriteAllText(__SOURCE_DIRECTORY__ + "/../docs/examples/wr/ro-crate-metadata.json", jsonString)
