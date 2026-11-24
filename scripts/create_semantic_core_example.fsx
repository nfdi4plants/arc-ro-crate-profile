#r "nuget: ARCtrl, 3.1.0"

open ARCtrl.ROCrate
open ARCtrl.Json

let mass = LDPropertyValue.create(
    id = "#mass",
    name = "mass",
    propertyID = "http://purl.obolibrary.org/obo/PATO_0000125",
    unitText = "g",
    unitCode = "http://purl.obolibrary.org/obo/UO_0000021"
)

// The constructor accepts strings; retain the numeric value from the example.
mass.SetProperty(LDPropertyValue.value, 2.3)

let controlGroupDesignation = LDPropertyValue.create(
    id = "#controlGroupDesignation",
    name = "experimental role",
    // Retain the placeholder used in the profile's markdown example.
    propertyID = "TODO",
    value = "control group",
    valueReference = "http://purl.bioontology.org/ontology/MESH/D035061"
)

controlGroupDesignation.AdditionalType <- ResizeArray ["Designation"]

let temperatureInterpretation = LDPropertyValue.create(
    id = "#temperatureInterpretation",
    name = "physical quantity represented",
    value = "temperature",
    valueReference = "http://purl.obolibrary.org/obo/PATO_0000146",
    unitText = "°C",
    unitCode = "http://purl.obolibrary.org/obo/UO_0000027"
)

temperatureInterpretation.AdditionalType <- ResizeArray ["Interpretation"]

let sample = LDNode("#sample1", schemaType = ResizeArray ["Sample"])
sample.SetProperty("name", "Sample 1")
sample.SetProperty("additionalProperty", LDRef(mass.Id))
sample.SetProperty("subjectOf", LDRef("#semanticDescriptor1"))

let measurements = LDFile.create(
    name = "measurements.csv",
    encodingFormat = "text/csv"
)

measurements.SetProperty("subjectOf", LDRef("#semanticDescriptor2"))

// Descriptors bundle external assertions about an entity.
let sampleDescriptor = LDNode(
    "#semanticDescriptor1",
    schemaType = ResizeArray ["Statement"; "ItemList"]
)

sampleDescriptor.SetProperty("about", LDRef(sample.Id))
sampleDescriptor.SetProperty("itemListElement", ResizeArray [LDRef(controlGroupDesignation.Id)])

let measurementsDescriptor = LDNode(
    "#semanticDescriptor2",
    schemaType = ResizeArray ["Statement"; "ItemList"]
)

measurementsDescriptor.SetProperty("about", LDRef(measurements.Id))
measurementsDescriptor.SetProperty("itemListElement", ResizeArray [LDRef(temperatureInterpretation.Id)])

let dataset = LDDataset.create(
    id = "./",
    name = "Semantic Core Example",
    description = "An example of a ROCrate with semantic annotations, attaching a designation to a sample and an interpretation to a data file.",
    datePublished = System.DateTime.Now
)
dataset.SetProperty("mentions", ResizeArray [LDRef(sampleDescriptor.Id); LDRef(measurementsDescriptor.Id)])
dataset.SetProperty("hasPart", LDRef(measurements.Id))

// The same license ARCtrl assigns to the other example crates.
let license = ARC.ROCrate.createLicenseNode(Some (ARCtrl.License.GetDefaultLicense()))
dataset.SetProperty("license", LDRef(license.Id))

// Declare conformance to the profile on the root data entity, as RO-Crate 1.2 recommends,
// and describe the profile as a contextual entity of type Profile.
let profile = LDNode(
    "https://w3id.org/arc/ro-crate/semantic-core/0.1",
    schemaType = ResizeArray ["CreativeWork"; "Profile"]
)

profile.SetProperty("name", "Semantic Core Profile")
profile.SetProperty("version", "0.1")

dataset.SetProperty("conformsTo", LDRef(profile.Id))

let metadata = LDNode("ro-crate-metadata.json", schemaType = ResizeArray ["CreativeWork"])
metadata.SetProperty("conformsTo", LDRef("https://w3id.org/ro/crate/1.2"))
metadata.SetProperty("about", LDRef(dataset.Id))

let context = LDContext(baseContexts = ResizeArray [
    Context.initV1_2()
    LDContext.fromMappingSeq ["Sample", "https://bioschemas.org/Sample"]
])

// Build the graph explicitly to preserve the minimal example in the markdown.
let graph = LDGraph(
    context = context,
    nodes = ResizeArray [
        dataset; sample; sampleDescriptor; measurementsDescriptor; mass
        controlGroupDesignation; measurements; temperatureInterpretation; profile; license; metadata
    ]
)

graph.Compact_InPlace()

// Compaction unwraps singleton lists; keep descriptor items as arrays, as documented.
sampleDescriptor.SetProperty("itemListElement", ResizeArray [LDRef(controlGroupDesignation.Id)])
measurementsDescriptor.SetProperty("itemListElement", ResizeArray [LDRef(temperatureInterpretation.Id)])

let jsonString = graph.ToROCrateJsonString(spaces = 2)
let outputDirectory = System.IO.Path.Combine(__SOURCE_DIRECTORY__, "../docs/examples/semantic-core")
System.IO.Directory.CreateDirectory(outputDirectory) |> ignore
System.IO.File.WriteAllText(System.IO.Path.Combine(outputDirectory, "ro-crate-metadata.json"), jsonString)
