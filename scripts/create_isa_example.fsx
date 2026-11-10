#r "nuget: ARCtrl, 3.1.0"

open System
open System.IO
open ARCtrl
open ARCtrl.ROCrate
open ARCtrl.Json

// This example uses ARCtrl's typed JSON-LD entities. Study and Assay are
// represented as LDDataset objects with the ISA additionalType values.
// Protocol parameters are implicit through the parameterValue properties
// of the corresponding LDLabProcess objects.

let one value = ResizeArray [value]
let many values = ResizeArray values

// -----------------------------------------------------------------------------
// Controlled terms
// -----------------------------------------------------------------------------

let plantGrowthIntendedUse =
    LDDefinedTerm.create(
        id = "http://purl.org/nfdi4plants/ontology/dpbo/DPBO_1000164",
        name = "plant growth protocol",
        termCode = "DPBO_1000164",
        inDefinedTermSet = "http://purl.org/nfdi4plants/ontology/dpbo/"
    )

let extractionIntendedUse =
    LDDefinedTerm.create(
        id = "http://purl.org/nfdi4plants/ontology/dpbo/DPBO_1000171",
        name = "extraction protocol",
        termCode = "DPBO_1000171",
        inDefinedTermSet = "http://purl.org/nfdi4plants/ontology/dpbo/"
    )

let chromatographyIntendedUse =
    LDDefinedTerm.create(
        id = "http://purl.org/nfdi4plants/ontology/dpbo/DPBO_1000175",
        name = "chromatography protocol",
        termCode = "DPBO_1000175",
        inDefinedTermSet = "http://purl.org/nfdi4plants/ontology/dpbo/"
    )

let assayIntendedUse =
    LDDefinedTerm.create(
        id = "http://purl.org/nfdi4plants/ontology/dpbo/DPBO_1000177",
        name = "assay protocol",
        termCode = "DPBO_1000177",
        inDefinedTermSet = "http://purl.org/nfdi4plants/ontology/dpbo/"
    )

let dataProcessingIntendedUse =
    LDDefinedTerm.create(
        id = "http://purl.obolibrary.org/obo/MS_1000544",
        name = "data processing protocol",
        termCode = "MS_1000544",
        inDefinedTermSet = "http://purl.obolibrary.org/obo/"
    )

let measurementMethod =
    LDDefinedTerm.create(
        id = "https://www.ebi.ac.uk/ols4/ontologies/ms/classes/http%253A%252F%252Fpurl.obolibrary.org%252Fobo%252FMS_1000490",
        name = "Agilent instrument model",
        termCode = "http%253A%252F%252Fpurl.obolibrary.org%252Fobo%252FMS_1000490",
        inDefinedTermSet = "https://www.ebi.ac.uk/ols4/ontologies/ms/"
    )

let measurementTechnique =
    LDDefinedTerm.create(
        id = "https://www.ebi.ac.uk/ols4/ontologies/ms/classes/http%253A%252F%252Fpurl.obolibrary.org%252Fobo%252FMS_1003542",
        name = "7200 Q-TOF GC/MS",
        termCode = "http%253A%252F%252Fpurl.obolibrary.org%252Fobo%252FMS_1003542",
        inDefinedTermSet = "https://www.ebi.ac.uk/ols4/ontologies/ms/"
    )

// -----------------------------------------------------------------------------
// Administrative entities
// -----------------------------------------------------------------------------

let hhuPlantBiochemistry =
    LDOrganization.create(
        id = "#org-hhu",
        name = "Institute of Plant Biochemistry, HHU Düsseldorf"
    )

let dominik =
    LDPerson.create(
        id = "https://orcid.org/0000-0001-9021-3197",
        givenName = "Dominik",
        familyName = "Brilhaus",
        affiliation = one hhuPlantBiochemistry
    )

// -----------------------------------------------------------------------------
// Characteristics, factor value, variable, and process parameter values
// -----------------------------------------------------------------------------

let organismCharacteristic =
    LDPropertyValue.create(
        id = "#characteristic-organism",
        name = "organism",
        value = "Talinum fruticosum",
        propertyID = "http://purl.obolibrary.org/obo/OBI_0100026",
        valueReference = "http://purl.obolibrary.org/obo/NCBITaxon_215506"
    )

organismCharacteristic.SetProperty(
    LDPropertyValue.additionalType,
    "CharacteristicValue"
)

let organismPartCharacteristic =
    LDPropertyValue.create(
        id = "#characteristic-organism-part",
        name = "organism part",
        value = "leaf",
        propertyID = "http://purl.obolibrary.org/obo/OBI_0000953",
        valueReference = "http://purl.obolibrary.org/obo/PO_0025034"
    )

organismPartCharacteristic.SetProperty(
    LDPropertyValue.additionalType,
    "CharacteristicValue"
)

let wateringFactor =
    LDPropertyValue.create(
        id = "#factor-watering",
        name = "watering exposure",
        value = "12 days drought",
        propertyID = "http://purl.obolibrary.org/obo/PECO_0007383"
    )

wateringFactor.SetProperty(
    LDPropertyValue.additionalType,
    "FactorValue"
)

let metabolitesVariable =
    LDPropertyValue.create(
        id = "#variable-metabolites",
        name = "targeted polar metabolites",
        value = "relative metabolite abundance"
    )

let dayTemperature =
    LDPropertyValue.create(
        id = "#ParameterValue_temperature_day_30",
        name = "temperature day",
        value = "30",
        propertyID = "http://purl.org/nfdi4plants/ontology/dpbo/DPBO_0000007",
        unitCode = "https://bioregistry.io/UO:0000027",
        unitText = "degree celsius"
    )

dayTemperature.SetProperty(
    LDPropertyValue.additionalType,
    "ParameterValue"
)

let growthDayLength =
    LDPropertyValue.create(
        id = "#ParameterValue_growth_day_length_12_hr_light_/_12_hr_dark",
        name = "growth day length",
        value = "12 hr light / 12 hr dark",
        propertyID = "http://purl.org/nfdi4plants/ontology/dpbo/DPBO_0000041"
    )

growthDayLength.SetProperty(
    LDPropertyValue.additionalType,
    "ParameterValue"
)

let extractionBuffer =
    LDPropertyValue.create(
        id = "#ParameterValue_Extraction_buffer_water:methanol:chloroform_1:2.5:1_(v:v:v)",
        name = "Extraction buffer",
        value = "water:methanol:chloroform 1:2.5:1 (v:v:v)",
        propertyID = "http://purl.org/nfdi4plants/ontology/dpbo/DPBO_0000050"
    )

extractionBuffer.SetProperty(
    LDPropertyValue.additionalType,
    "ParameterValue"
)

// -----------------------------------------------------------------------------
// Material entities
// -----------------------------------------------------------------------------

let plantSource =
    LDSample.create(
        id = "#source-db097",
        name = "DB_097 plant source"
    )

plantSource.SetProperty(LDSample.additionalType, "Source")
plantSource.SetProperty(
    LDSample.additionalProperty,
    many [organismCharacteristic; organismPartCharacteristic]
)

let leafSample =
    LDSample.create(
        id = "#sample-cam01",
        name = "CAM_01 drought-treated leaf sample"
    )

leafSample.SetProperty(LDSample.additionalType, "Sample")
leafSample.SetProperty(LDSample.additionalProperty, one wateringFactor)

let metaboliteExtract =
    LDSample.create(
        id = "#sample-extract",
        name = "CAM_01 metabolite extract"
    )

metaboliteExtract.SetProperty(LDSample.additionalType, "Sample")

let chromatographicEffluent =
    LDSample.create(
        id = "#sample-gc-effluent",
        name = "CAM_01 chromatographic effluent"
    )

chromatographicEffluent.SetProperty(LDSample.additionalType, "Sample")

// -----------------------------------------------------------------------------
// Reagent and equipment
// -----------------------------------------------------------------------------

let ribitol =
    LDChemicalSubstance.create(
        id = "#reagent-ribitol",
        name = "ribitol internal standard",
        identifier = "CHEBI:15963"
    )

let gasChromatograph =
    LDProduct.create(
        id = "#equipment-gc",
        name = "Agilent 7890 gas chromatograph"
    )

let massSpectrometer =
    LDProduct.create(
        id = "#equipment-ms",
        name = "Agilent 7200 GC-QTOF mass spectrometer"
    )

// -----------------------------------------------------------------------------
// Protocols
// -----------------------------------------------------------------------------

let plantMaterialProtocol =
    LDLabProtocol.create(
        id = "#protocol-plant-material",
        name = "Plant growth and drought treatment",
        intendedUses = one plantGrowthIntendedUse
    )

let extractionProtocol =
    LDLabProtocol.create(
        id = "#protocol-extraction",
        name = "Metabolite extraction",
        intendedUses = one extractionIntendedUse
    )

let gasChromatographyProtocol =
    LDLabProtocol.create(
        id = "#protocol-gc",
        name = "Gas chromatography separation",
        intendedUses = one chromatographyIntendedUse
    )

let massSpectrometryProtocol =
    LDLabProtocol.create(
        id = "#protocol-ms",
        name = "GC-QTOF mass spectrometry acquisition",
        intendedUses = one assayIntendedUse
    )

let peakDetectionProtocol =
    LDLabProtocol.create(
        id = "#protocol-peak-detection",
        name = "Peak detection and quantification workflow",
        intendedUses = one dataProcessingIntendedUse
    )

// -----------------------------------------------------------------------------
// Data entities
// -----------------------------------------------------------------------------

let rawPeakFile =
    LDFile.create(
        id = "./assays/gc-ms/data/raw-run/MSPeak.bin",
        name = "Raw mass spectral peaks",
        contentSize = "243430500 B",
        encodingFormat = "application/octet-stream"
    )

let acquisitionMethodFile =
    LDFile.create(
        id = "./assays/gc-ms/data/raw-run/method.txt",
        name = "Instrument acquisition method",
        contentSize = "1446 B",
        encodingFormat = "text/plain"
    )

let rawRun =
    LDDataset.create(
        id = "./assays/gc-ms/data/raw-run/",
        name = "Representative raw GC–MS run",
        hasParts = many [rawPeakFile; acquisitionMethodFile]
    )

// The original reduced crate deliberately gives this data entity both Dataset
// and File types.
rawRun.SetProperty(LDDataset.additionalType, "File")

let peakTable =
    LDFile.create(
        id = "./assays/gc-ms/data/peak-table.csv",
        name = "Quantified metabolite peak table",
        contentSize = "2048 B",
        encodingFormat = "text/csv"
    )

peakTable.SetProperty(LDFile.additionalType, "Dataset")

// -----------------------------------------------------------------------------
// Processes
// -----------------------------------------------------------------------------

let plantMaterialProcess =
    LDLabProcess.create(
        id = "#process-plant-material",
        name = "Grow and drought-treat plant material",
        objects = one plantSource,
        results = one leafSample,
        executesLabProtocol = plantMaterialProtocol,
        parameterValues = many [dayTemperature; growthDayLength]
    )

let extractionProcess =
    LDLabProcess.create(
        id = "#process-extraction",
        name = "Extract metabolites from CAM_01",
        objects = one leafSample,
        results = one metaboliteExtract,
        executesLabProtocol = extractionProtocol,
        parameterValues = one extractionBuffer,
        reagents = one ribitol
    )

let gasChromatographyProcess =
    LDLabProcess.create(
        id = "#process-gc",
        name = "Separate extract by gas chromatography",
        objects = one metaboliteExtract,
        results = one chromatographicEffluent,
        executesLabProtocol = gasChromatographyProtocol,
        labEquipment = one gasChromatograph
    )

let massSpectrometryProcess =
    LDLabProcess.create(
        id = "#process-ms",
        name = "Acquire raw GC–MS data",
        objects = one chromatographicEffluent,
        results = one rawRun,
        executesLabProtocol = massSpectrometryProtocol,
        labEquipment = one massSpectrometer
    )

let peakDetectionProcess =
    LDLabProcess.create(
        id = "#process-peak-detection",
        name = "Generate quantified metabolite peak table",
        objects = one rawRun,
        results = one peakTable,
        executesLabProtocol = peakDetectionProtocol
    )

// -----------------------------------------------------------------------------
// Study, Assay, and Investigation
// -----------------------------------------------------------------------------

let study =
    LDDataset.create(
        id = "./studies/talinum-samples/",
        identifier = "talinum-samples",
        name = "Talinum plant material study",
        description = "Growth and drought treatment of Talinum plant material. This Study contains only material-producing processes.",
        creators = one dominik
    )

study.SetProperty(LDDataset.additionalType, "Study")
study.SetProperty(LDDataset.about, one plantMaterialProcess)

let assay =
    LDDataset.create(
        id = "./assays/gc-ms/",
        identifier = "gc-ms",
        name = "GC–MS metabolite profiling assay",
        description = "Metabolite extraction, gas chromatography, mass spectrometry acquisition, and computational peak-table generation.",
        creators = one dominik,
        hasParts = many [rawRun; peakTable]
    )

assay.SetProperty(LDDataset.additionalType, "Assay")
assay.SetProperty(
    LDDataset.about,
    many [
        extractionProcess
        gasChromatographyProcess
        massSpectrometryProcess
        peakDetectionProcess
    ]
)
assay.SetProperty(LDDataset.measurementMethod, measurementMethod)
assay.SetProperty(LDDataset.measurementTechnique, measurementTechnique)
assay.SetProperty(LDDataset.variableMeasured, one metabolitesVariable)

let investigation =
    LDDataset.create(
        id = "./",
        identifier = "facultative-cam-reduced-example",
        name = "Facultative CAM in Talinum triangulare — reduced ISA RO-Crate example",
        description = "A compact, self-contained example derived from the original investigation. It preserves one complete material provenance chain in the Study and one data-producing and data-processing chain in the Assay.",
        creators = one dominik,
        datePublished = DateTime(2026, 7, 29),
        hasParts = many [study; assay]
    )

investigation.SetProperty(LDDataset.additionalType, "Investigation")
investigation.SetProperty(
    LDDataset.license,
    "https://creativecommons.org/licenses/by/4.0/"
)
investigation.SetProperty(
    LDDataset.conformsTo,
    "https://github.com/nfdi4plants/isa-ro-crate-profile"
)

// ARC.ROCrate.packDatasetAsCrate creates the metadata descriptor and base
// RO-Crate conformance entity and recursively includes linked typed entities.
let graph =
    ARC.ROCrate.packDatasetAsCrate(
        investigation,
        datasetName = investigation.Name,
        datasetDescription = investigation.Description,
        metadataFileDescriptor = None,
        license = License.GetDefaultLicense()
    )

let jsonString = graph.ToROCrateJsonString(spaces = 2)
let outputPath = Path.Combine(__SOURCE_DIRECTORY__, "ro-crate-metadata.json")

File.WriteAllText(outputPath, jsonString)
printfn "Wrote %s" outputPath