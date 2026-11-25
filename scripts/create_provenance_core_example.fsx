#r "nuget: ARCtrl, 3.1.0"

open ARCtrl
open ARCtrl.ROCrate
open ARCtrl.Json


let arabidopsis = LDPropertyValue.createCharacteristicValue(
    name = "organism",
    propertyID = "https://bioregistry.io/OBI:0100026",
    value = "Arabidopsis thaliana",
    valueReference = "http://purl.obolibrary.org/obo/NCBITAXON_3702"
)

let temperature = LDPropertyValue.createCharacteristicValue(
    name = "Temperature",
    propertyID = "https://bioregistry.io/NCIT:C25206",
    value = "25",
    unitText = "degree Celsius",
    unitCode = "http://purl.obolibrary.org/obo/UO_0000027"
)

let sourceMaterial = LDSample.createMaterial(
    name = "MyObservable", 
    additionalProperties = ResizeArray [arabidopsis]
)

let sample = LDSample.createSample(
    name = "MySample"
)

let data = LDFile.create(
    name = "datafile.wiff"
)

let preparation = LDLabProcess.create(
    name = "Preparation",
    objects = ResizeArray [sourceMaterial],
    results = ResizeArray [sample],
    parameterValues = ResizeArray [temperature]
)

let measurement = LDLabProcess.create(
    name = "Measurement",
    objects = ResizeArray [sample],
    results = ResizeArray [data]
)

let dataset = LDDataset.create(
    id = "./",
    hasParts = ResizeArray [data],
    abouts = ResizeArray [preparation; measurement],
    datePublished = System.DateTime.Now
)

// Declare conformance to the profile on the root data entity, as RO-Crate 1.2 recommends,
// and describe the profile as a contextual entity of type Profile.

let profile = LDCreativeWork.create(
    id = "https://w3id.org/arc/ro-crate/provenance-core/0.1",
    name = "Provenance Core Profile"
)

profile.SchemaType.Add("http://www.w3.org/ns/dx/prof/Profile")
profile.SetProperty("http://schema.org/version", "0.1")

dataset.SetProperty(LDDataset.conformsTo, profile)

let graph = ARC.ROCrate.packDatasetAsCrate(
    dataset, datasetName = "Experimental ARC Provenance Core Example", 
    datasetDescription = "An example of a ROCrate with a core process model, including preparation and measurement processes.", 
    metadataFileDescriptor = None,
    license = License.GetDefaultLicense()
    )

let jsonString = graph.ToROCrateJsonString(spaces = 2)
System.IO.File.WriteAllText(__SOURCE_DIRECTORY__ + "/../docs/examples/provenance-core/ro-crate-metadata.json", jsonString)
