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
    abouts = ResizeArray [preparation; measurement]
)

let graph = ARC.ROCrate.packDatasetAsCrate(
    dataset, datasetName = "Experimental ARC Process Core Crate Example", 
    datasetDescription = "An example of a ROCrate with a core process model, including preparation and measurement processes.", 
    metadataFileDescriptor = None,
    license = License.GetDefaultLicense()
    )

let jsonString = graph.ToROCrateJsonString(spaces = 2)
System.IO.File.WriteAllText(__SOURCE_DIRECTORY__ + "/../docs/examples/process_core_crate/ro-crate-metadata.json", jsonString)
