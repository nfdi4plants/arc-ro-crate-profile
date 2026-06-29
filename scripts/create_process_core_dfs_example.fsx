#r "nuget: ARCtrl, 3.1.0"

open ARCtrl
open ARCtrl.ROCrate
open ARCtrl.Json


let rawData1 = LDFile.create(
    name = "raw_data1.wiff"
)

let rawData2 = LDFile.create(
    name = "raw_data2.wiff"
)

let processedDataColumn1 = LDFile.create(
    name = "processed_data.csv#col=1",
    encodingFormat = "text/csv",
    usageInfo = "https://datatracker.ietf.org/doc/html/rfc7111"
)

let processedDataColumn2 = LDFile.create(
    name = "processed_data.csv#col=2",
    encodingFormat = "text/csv",
    usageInfo = "https://datatracker.ietf.org/doc/html/rfc7111"
)

let dataProcessing1 = LDLabProcess.create(
    name = "DataProcessing_1",
    objects = ResizeArray [rawData1],
    results = ResizeArray [processedDataColumn1]
)

let dataProcessing2 = LDLabProcess.create(
    name = "DataProcessing_2",
    objects = ResizeArray [rawData2],
    results = ResizeArray [processedDataColumn2]
)

let dataset = LDDataset.create(
    id = "./",
    hasParts = ResizeArray [rawData1; rawData2; processedDataColumn1; processedDataColumn2],
    abouts = ResizeArray [dataProcessing1; dataProcessing2]
)

let graph = ARC.ROCrate.packDatasetAsCrate(
    dataset, datasetName = "Data Fragment ARC Process Core Crate Example", 
    datasetDescription = "An example of a ROCrate with a core process model, including processing from raw data to processed data fragments.", 
    metadataFileDescriptor = None,
    license = License.GetDefaultLicense()
    )

let jsonString = graph.ToROCrateJsonString(spaces = 2)
System.IO.File.WriteAllText(__SOURCE_DIRECTORY__ + "/../docs/examples/process_core_crate/dfs_example.json", jsonString)
