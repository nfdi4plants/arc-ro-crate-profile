#r "nuget: ARCtrl, 3.1.0"

open ARCtrl
open ARCtrl.ROCrate
open ARCtrl.Json

let dataAnalysis = LDDefinedTerm.create(
    name="data analysis",
    termCode ="http://purl.obolibrary.org/obo/T4FS_0000352"
)

let digitalCuration = LDDefinedTerm.create(
    name="data curation",
    termCode ="http://purl.obolibrary.org/obo/T4FS_0000132"
)

let supervision = LDDefinedTerm.create(
    name="Supervision",
    termCode ="https://credit.niso.org/contributor-roles/supervision"
)

let rptuKaiserslautern = LDOrganization.create(
    name = "RPTU Kaiserslautern-Landau",
    id = "https://ror.org/01qrts582"
)

rptuKaiserslautern.SetProperty(LDDataset.url, "https://www.rptu.de/en/")

let publicationDOI = LDPropertyValue.createDOI(
    value = "https://www.doi.org/10.1000/182"
)

let published = LDDefinedTerm.create(
    name="published",
    termCode ="http://www.ebi.ac.uk/efo/EFO_0001796"
)

let publication = LDScholarlyArticle.create(
    headline = "An example publication",
    identifiers = ResizeArray [publicationDOI],
    url = "https://www.doi.org/the-identifier/resources/handbook/",
    creativeWorkStatus = published
)

let lukas = LDPerson.create(
    givenName = "Heinrich Lukas",
    familyName = "Weil",
    orcid = "https://orcid.org/0000-0003-1945-6342",
    affiliation = ResizeArray [rptuKaiserslautern],
    jobTitles = ResizeArray [digitalCuration]
)

let kevin = LDPerson.create(
    givenName = "Kevin",
    familyName = "Schneider",
    orcid = "https://orcid.org/0000-0002-2198-5262",
    affiliation = ResizeArray [rptuKaiserslautern],
    jobTitles = ResizeArray [dataAnalysis]
)

let timo = LDPerson.create(
    givenName = "Timo",
    familyName = "Mühlhaus",
    orcid = "https://orcid.org/0000-0003-3925-6778",
    affiliation = ResizeArray [rptuKaiserslautern],
    jobTitles = ResizeArray [supervision],
    email = "timo.muehlhaus[at]rptu.de",
    address = "Erwin-Schrödinger-Str. 56, 67663 Kaiserslautern, Germany"
)


let dataset = LDDataset.create(
    id = "./",
    creators = ResizeArray [lukas; kevin; timo],
    citations = ResizeArray [publication]
)

let graph = ARC.ROCrate.packDatasetAsCrate(
    dataset, datasetName = "ARC Administrative Metadata Crate Example", 
    datasetDescription = "An example of a ROCrate with administrative metadata including creators and citations.", 
    metadataFileDescriptor = None,
    license = License.GetDefaultLicense()
    )

let jsonString = graph.ToROCrateJsonString(spaces = 2)
System.IO.File.WriteAllText(__SOURCE_DIRECTORY__ + "/../docs/examples/administrative_metadata_crate/ro-crate-metadata.json", jsonString)
