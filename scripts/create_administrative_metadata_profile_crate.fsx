// Script for creation of the profile crate

#load "profile_crate_helpers.fsx"

open ARCtrl.ROCrate
open ARCtrl.Json
open ROCratePCC
open Profile_crate_helpers

let types = ResizeArray [
    Types.dataset
    Types.person
    Types.scholarlyArticle
    Types.organization
    Types.propertyValue
]

let subId = "administrative_metadata_crate"

let id = $"{Profile.profilesRoot}/{subId}"

let name = "Administrative Metadata Crate Profile"

let description = "An RO-Crate profile for representing the Administrative Metadata of the ARC metadata framework in Research Object Crates (RO-Crates)."

let keywords = ResizeArray [
    "RO-Crate"
    "Research Object Crate"
    "Administrative Metadata"
    "Metadata Standard"
    "Bioschemas"
    "FAIR Data"
    "ARC"
    "Annotated Research Context"
    "Data Management"
]

let specifications = ResizeArray[
    TextualResource(
        name = "Administrative Metadata RO-Crate Profile description",
        filePath = "index.md",
        encodingFormat = "text/markdown",
        rootDataEntityId = id
    )
]

let guidances = ResizeArray[
    // TextualResource(
    //     name = "ISA RO-Crate ISA Tab/Json mapping guidance",
    //     filePath = "isa_ro_crate_mapping.md",
    //     encodingFormat = "text/markdown",
    //     rootDataEntityId = id
    // )
    TextualResource(
        name = "ARC RO-Crate Annotation Modelling guidance",
        filePath = "https://arc-rdm.org/details/documentation-principle/",
        encodingFormat = "text/html",
        rootDataEntityId = id
    )
]

let examples = ResizeArray[
    TextualResource(
        name = "Administrative Metadata RO-Crate Example",
        filePath = $"{Profile.examplesRoot}/{subId}/ro-crate-metadata.json",
        encodingFormat = "application/json",
        rootDataEntityId = id
    )
]

let resourceDescriptors = ResizeArray [
    Specification(specifications) :> ResourceDescriptor
    Guidance(guidances) :> ResourceDescriptor
    Example(examples) :> ResourceDescriptor
]

let rootEntity = 
    RootDataEntity(
        id = id,
        name = name,
        description = description,
        license = License.mit,
        authors = ResizeArray People.defaultAuthors,
        version = Profile.version,
        keywords = keywords,
        usedTypes = types,
        resourceDescriptors = resourceDescriptors,
        publisher = Organization.dataPLANT
    )

let profile = 
    Profile(
        rootEntity,
        license = License.mit
    )

let string = profile.ToROCrateJsonString(spaces = 2)

System.IO.File.WriteAllText(__SOURCE_DIRECTORY__ + $"/../docs/profiles/{Profile.version}/{subId}/ro-crate-metadata.json", string)