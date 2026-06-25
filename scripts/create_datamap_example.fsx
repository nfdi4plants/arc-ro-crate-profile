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

let proteinIdentifierDescriptor = LDPropertyValue.createFragmentDescriptor(
    fileName = proteinIdentifierColumn.Id,
    value = "Protein identifier",
    valueReference = "http://purl.obolibrary.org/obo/NCIT_C165059",
    subjectOf = proteinIdentifierColumn,
    alternateName = "protID")

let proteinAbundanceDescriptor1 = LDPropertyValue.createFragmentDescriptor(
    fileName = proteinAbundanceColumn1.Id,
    value = "molecule count",
    valueReference = "http://purl.obolibrary.org/obo/UO_0000192",
    subjectOf = proteinAbundanceColumn1,
    unitText = "Millimole per Kilogram",
    unitCode = "http://purl.obolibrary.org/obo/NCIT_C68892",
    alternateName = "quant1")

let proteinAbundanceDescriptor2 = LDPropertyValue.createFragmentDescriptor(
    fileName = proteinAbundanceColumn2.Id,
    value = "molecule count",
    valueReference = "http://purl.obolibrary.org/obo/UO_0000192",
    subjectOf = proteinAbundanceColumn2,
    unitText = "Millimole per Kilogram",
    unitCode = "http://purl.obolibrary.org/obo/NCIT_C68892",
    alternateName = "quant2")

proteinIdentifierColumn.SetProperty(LDDataset.about, ResizeArray [LDRef(proteinIdentifierDescriptor.Id)])
proteinAbundanceColumn1.SetProperty(LDDataset.about, ResizeArray [LDRef(proteinAbundanceDescriptor1.Id)])
proteinAbundanceColumn2.SetProperty(LDDataset.about, ResizeArray [LDRef(proteinAbundanceDescriptor2.Id)])

let file = LDFile.create(
    name = "processed_data.csv",
    encodingFormat = "text/csv"
)

file.SetProperty(LDDataset.hasPart, ResizeArray [proteinIdentifierColumn; proteinAbundanceColumn1; proteinAbundanceColumn2])

let dataset = LDDataset.create(
    id = "./",
    hasParts = ResizeArray [file],
    variableMeasureds = ResizeArray [proteinIdentifierDescriptor; proteinAbundanceDescriptor1; proteinAbundanceDescriptor2]
)

let graph = ARC.ROCrate.packDatasetAsCrate(
    dataset, datasetName = "ARC Datamap Crate Example", 
    datasetDescription = "An example of a ROCrate with a datamap, including annotation of a tabular data file.", 
    metadataFileDescriptor = None,
    license = License.GetDefaultLicense()
    )

let jsonString = graph.ToROCrateJsonString(spaces = 2)
System.IO.File.WriteAllText(__SOURCE_DIRECTORY__ + "/../docs/examples/datamap_crate/ro-crate-metadata.json", jsonString)
