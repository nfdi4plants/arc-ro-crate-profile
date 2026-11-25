#r "nuget: ARCtrl, 3.1.0"

open ARCtrl
open ARCtrl.ROCrate
open ARCtrl.Json

let protein_identfier = LDDefinedTerm.create(
    name="Protein identifier",
    termCode ="http://purl.obolibrary.org/obo/NCIT_C165059"
)

let molecule_count = LDDefinedTerm.create(
    name="molecule count",
    termCode ="http://purl.obolibrary.org/obo/UO_0000192"
)

let mmol_p_kg = LDDefinedTerm.create(
    name="Millimole per Kilogram",
    termCode ="http://purl.obolibrary.org/obo/NCIT_C68892"
)

let string_type = LDDefinedTerm.create(
    name="string",
    termCode ="http://purl.obolibrary.org/obo/NCIT_C45253"
)

let float_type = LDDefinedTerm.create(
    name="float",
    termCode ="http://purl.obolibrary.org/obo/NCIT_C48150"
)

let proteinIdentifierColumn = LDFile.create(
    name = "processed_data.csv#col=1",
    encodingFormat = "text/csv",
    usageInfo = "https://datatracker.ietf.org/doc/html/rfc7111"
)

proteinIdentifierColumn.SetProperty(LDFile.pattern, string_type)

let proteinAbundanceColumn1 = LDFile.create(
    name = "processed_data.csv#col=2",
    encodingFormat = "text/csv",
    usageInfo = "https://datatracker.ietf.org/doc/html/rfc7111"
)

proteinAbundanceColumn1.SetProperty(LDFile.pattern, float_type)

let proteinAbundanceColumn2 = LDFile.create(
    name = "processed_data.csv#col=3",
    encodingFormat = "text/csv",
    usageInfo = "https://datatracker.ietf.org/doc/html/rfc7111"
)

proteinAbundanceColumn2.SetProperty(LDFile.pattern, float_type)

// Mark the columns as data fragments in the sense of the Datamap profile.
for fragment in [proteinIdentifierColumn; proteinAbundanceColumn1; proteinAbundanceColumn2] do
    fragment.AdditionalType <- ResizeArray ["DataFragment"]

// A datamap entry is modelled as defined by the Datamap profile: a descriptor (`Statement`, `ItemList`,
// marked as `DataContext`) about the data fragment, holding the explication of the fragment as its
// assertion (`PropertyValue`).
// ARCtrl's `LDPropertyValue.createFragmentDescriptor` still merges both into a single `PropertyValue`.

/// The assertion of a datamap entry: an interpretation of what the data fragment contains.
let createExplication (fragment : LDNode) (value : string) (valueReference : string) (alternateName : string) (unit : (string * string) option) =
    let explication = LDPropertyValue.create(
        name = "Explication",
        value = value,
        id = $"#Explication_{fragment.Id}",
        propertyID = "https://purl.org/nfdi4plants/ontology/dpbo/DPBO_0000111",
        ?unitText = Option.map fst unit,
        ?unitCode = Option.map snd unit,
        valueReference = valueReference
    )
    explication.AdditionalType <- ResizeArray ["Interpretation"]
    LDPropertyValue.setAlternateNameAsString(explication, alternateName)
    explication

/// The descriptor of a datamap entry. It points to its data fragment through `about`,
/// the fragment points back through `subjectOf`.
let createDescriptor (fragment : LDNode) (explication : LDNode) =
    let descriptor = LDNode(
        $"#Descriptor_{fragment.Id}",
        schemaType = ResizeArray ["http://schema.org/Statement"; "http://schema.org/ItemList"]
    )
    descriptor.AdditionalType <- ResizeArray ["DataContext"]
    // References instead of nested nodes, as fragment and descriptor point to each other.
    descriptor.SetProperty(LDDataset.about, LDRef(fragment.Id))
    descriptor.SetProperty("http://schema.org/itemListElement", ResizeArray [explication])
    fragment.SetProperty(LDPropertyValue.subjectOf, LDRef(descriptor.Id))
    descriptor

let millimolePerKilogram = Some ("Millimole per Kilogram", "http://purl.obolibrary.org/obo/NCIT_C68892")

let proteinIdentifierExplication =
    createExplication proteinIdentifierColumn "Protein identifier" "http://purl.obolibrary.org/obo/NCIT_C165059" "protID" None

let proteinAbundanceExplication1 =
    createExplication proteinAbundanceColumn1 "molecule count" "http://purl.obolibrary.org/obo/UO_0000192" "quant1" millimolePerKilogram

let proteinAbundanceExplication2 =
    createExplication proteinAbundanceColumn2 "molecule count" "http://purl.obolibrary.org/obo/UO_0000192" "quant2" millimolePerKilogram

let proteinIdentifierDescriptor = createDescriptor proteinIdentifierColumn proteinIdentifierExplication
let proteinAbundanceDescriptor1 = createDescriptor proteinAbundanceColumn1 proteinAbundanceExplication1
let proteinAbundanceDescriptor2 = createDescriptor proteinAbundanceColumn2 proteinAbundanceExplication2

let file = LDFile.create(
    name = "processed_data.csv",
    encodingFormat = "text/csv"
)

file.SetProperty(LDDataset.hasPart, ResizeArray [proteinIdentifierColumn; proteinAbundanceColumn1; proteinAbundanceColumn2])

let dataset = LDDataset.create(
    id = "./",
    hasParts = ResizeArray [file],
    mentions = ResizeArray [proteinIdentifierDescriptor; proteinAbundanceDescriptor1; proteinAbundanceDescriptor2],
    datePublished = System.DateTime.Now
)

// Declare conformance to the profile on the root data entity, as RO-Crate 1.2 recommends,
// and describe the profile as a contextual entity of type Profile.

let profile = LDCreativeWork.create(
    id = "https://w3id.org/arc/ro-crate/datamap/0.1",
    name = "Datamap Crate Profile"
)

profile.SchemaType.Add("http://www.w3.org/ns/dx/prof/Profile")
profile.SetProperty("http://schema.org/version", "0.1")

dataset.SetProperty(LDDataset.conformsTo, profile)

let graph = ARC.ROCrate.packDatasetAsCrate(
    dataset, datasetName = "ARC Datamap Crate Example", 
    datasetDescription = "An example of a ROCrate with a datamap, including annotation of a tabular data file.", 
    metadataFileDescriptor = None,
    license = License.GetDefaultLicense()
    )

// Mark the dataset as the datamap. Done on the packed crate, as packing does not carry over
// the `AdditionalType` of the root dataset.
graph.GetNode(dataset.Id).AdditionalType <- ResizeArray ["Datamap"]

// Compaction unwraps singleton lists; keep the descriptor items as arrays, as documented in the profile.
[
    proteinIdentifierDescriptor, proteinIdentifierExplication
    proteinAbundanceDescriptor1, proteinAbundanceExplication1
    proteinAbundanceDescriptor2, proteinAbundanceExplication2
]
|> List.iter (fun (descriptor, explication) ->
    graph.GetNode(descriptor.Id).SetProperty("itemListElement", ResizeArray [LDRef(explication.Id)])
)

let jsonString = graph.ToROCrateJsonString(spaces = 2)
System.IO.File.WriteAllText(__SOURCE_DIRECTORY__ + "/../docs/examples/datamap/ro-crate-metadata.json", jsonString)
