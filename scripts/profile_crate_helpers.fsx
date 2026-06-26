#r "nuget: ROCratePCC"

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

    let fw = Author(orcid = "0000-0002-5526-71389", name = "Florian Wetzels", affiliation = Organization.rptu)
    let hlw = Author(orcid = "0000-0003-1945-6342", name = "Heinrich Lukas Weil", affiliation = Organization.rptu)
    let ks = Author(orcid = "0000-0002-2198-5262", name = "Kevin Schneider", affiliation = Organization.rptu)
    let so = Author(orcid = "0000-0003-2130-0865", name = "Stuart Owen", affiliation = Organization.uom)
    let co = Author(orcid = "0000-0003-1512-9504", name = "Caroline Ott", affiliation = Organization.rptu)
    let sb = Author(orcid = "0000-0002-2177-8781", name = "Sebastian Beier", affiliation = Organization.fzj)
    let tm = Author(orcid = "0000-0003-3925-6778", name = "Timo Mühlhaus", affiliation = Organization.rptu)

    let defaultAuthors = ResizeArray [
        fw; hlw; ks; so; co; sb; tm
    ]


module Types = 

    let dataset = UsedType(iri = "https://schema.org/Dataset", name = "Dataset")
    let labProcess = UsedType(iri = "https://bioschemas.org/types/LabProcess/0.1-DRAFT", name = "LabProcess", termCode = "LabProcess")
    let labProtocol = UsedType(iri = "https://bioschemas.org/types/LabProtocol/0.5-DRAFT", name = "LabProtocol", termCode = "LabProtocol")
    let propertyValue = UsedType(iri = "https://schema.org/PropertyValue", name = "PropertyValue")
    let definedTerm = UsedType(iri = "https://schema.org/DefinedTerm", name = "DefinedTerm")
    let person = UsedType(iri = "https://schema.org/Person", name = "Person")
    let scholarlyArticle = UsedType(iri = "https://schema.org/ScholarlyArticle", name = "ScholarlyArticle")
    let comment = UsedType(iri = "https://schema.org/Comment", name = "Comment")
    let mediaObject = UsedType(iri = "https://schema.org/MediaObject", name = "MediaObject")
    let sample = UsedType(iri = "https://bioschemas.org/types/Sample/0.3-DRAFT", name = "Sample", termCode = "Sample")
    let organization = UsedType(iri = "https://schema.org/Organization", name = "Organization")

module Profile = 

    let rootID = "https://github.com/nfdi4plants/arc-ro-crate-profile/tree"
    let version = "0.1"

    let profilesRoot = $"{rootID}/profiles/{version}"

    let examplesRoot = $"{rootID}/examples"