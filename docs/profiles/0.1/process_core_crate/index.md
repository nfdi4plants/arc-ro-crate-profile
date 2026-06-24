---
title: ARC Process Core Crate
---


# ARC Process Core Crate

* Version: 0.1
<!-- * Permalink: <https://w3id.org/ro/wfrun/process/0.5> -->
<!-- * Authors: [Workflow Run RO-Crate working group](https://www.researchobject.org/workflow-run-crate/#community) -->
* License: [Apache License, version 2.0](https://www.apache.org/licenses/LICENSE-2.0) (SPDX: [`Apache-2.0`](http://spdx.org/licenses/Apache-2.0))
* Example conforming crate: [ro-crate-metadata.json](example1/ro-crate-metadata.json) [ro-crate-preview.html](example1/ro-crate-preview.html)
* Profile Crate: [ro-crate-metadata.json](ro-crate-metadata.json)
* Extends:
  - [RO-Crate 1.2 specification](https://w3id.org/ro/crate/1.2)
* JSON-LD context: <https://www.researchobject.org/ro-terms/arc/context.jsonld>
* Vocabulary terms:  <https://w3id.org/ro/terms/arc#>

This profile uses terminology from the [RO-Crate 1.2 specification](https://w3id.org/ro/crate/1.2), and [extends it](https://www.researchobject.org/ro-crate/specification/1.2/appendix/jsonld.html#extending-ro-crate) with additional terms from the [ARC](https://github.com/ResearchObject/ro-terms/tree/master/arc) ro-terms and [Bioschemas](https://bioschemas.org/) namespace.


## Overview

The aim of the profile is to be able to fully represent [ISA-JSON](https://isa-specs.readthedocs.io/en/latest/isajson.html) as RO-Crate, fully capturing the metadata and files in a non-lossy form such that it
should be possible to convert between one to the other, in either direction, without loss of information.

The profile relies on types from [Bioschemas](https://bioschemas.org/) types:
  
**LabProtocol** - A child of [HowTo](https://schema.org/HowTo) to make it clearer that it is intended to specifically describe the planned instructions for upcoming lab processes.

**LabProcess** - A child of [Action](https://schema.org/Action), to specifically describe the details and outcomes of an executed LabProtocol.
Thereby separating the "what was planned" and "what happened" between LabProtocol and LabProcess respectively.
A working group is working on the new type and adaptations of existing types.

The following graph summarizes the Process Core model in terms of [Bioschemas](https://bioschemas.org/)/[Schema.org](https://schema.org/) vocabulary:

```mermaid
flowchart TD

dataset[Dataset]

Process[LabProcess]

Protocol[LabProtocol]

Sample[Sample]

DataFile[File]


prop[PropertyValue]

dataset --hasPart----> DataFile
dataset --processSequence--> Process

Process --"result"---> DataFile
Process --"result"--> Sample
Process --"object"--> Sample
Process --executesLabProtocol--> Protocol
Process --parameterValue---> prop

Sample --additionalProperty--> prop

Protocol --labEquipment---> prop
Protocol --reagent---> prop

```


## Example Metadata File (`ro-crate-metadata.json`)

* [ro-crate-metadata.json](../../../examples/process_core_crate/ro-crate-metadata.json)
* [ro-crate-preview.html](../../../examples/process_core_crate/ro-crate-preview.html)

<!-- Remember to update above as well as below! -->

```json
{
  "@context": [
    "https://w3id.org/ro/crate/1.2/context",
    {
      "Sample": "https://bioschemas.org/Sample",
      "LabProtocol": "https://bioschemas.org/LabProtocol",
      "LabProcess": "https://bioschemas.org/LabProcess",
      "computationalTool": "https://bioschemas.org/properties/computationalTool",
      "labEquipment": "https://bioschemas.org/properties/labEquipment",
      "reagent": "https://bioschemas.org/properties/reagent",
      "intendedUse": "https://bioschemas.org/properties/intendedUse",
      "executesLabProtocol": "https://bioschemas.org/properties/executesLabProtocol",
      "parameterValue": "https://bioschemas.org/properties/parameterValue",
      "columnIndex": "https://w3id.org/ro/terms/arc#columnIndex"
    }
  ],
  "@graph": [
    {
      "@id": "datafile.wiff",
      "@type": "File",
      "name": "datafile.wiff"
    },
    {
      "@id": "#CharacteristicValue_organism_Arabidopsis_thaliana",
      "@type": "PropertyValue",
      "additionalType": "CharacteristicValue",
      "name": "organism",
      "value": "Arabidopsis thaliana",
      "propertyID": "https://bioregistry.io/OBI:0100026",
      "valueReference": "http://purl.obolibrary.org/obo/NCBITAXON_3702"
    },
    {
      "@id": "#Material_MyObservable",
      "@type": "Sample",
      "additionalType": "Material",
      "name": "MyObservable",
      "additionalProperty": {
        "@id": "#CharacteristicValue_organism_Arabidopsis_thaliana"
      }
    },
    {
      "@id": "#Sample_MySample",
      "@type": "Sample",
      "additionalType": "Sample",
      "name": "MySample"
    },
    {
      "@id": "#CharacteristicValue_Temperature_25",
      "@type": "PropertyValue",
      "additionalType": "CharacteristicValue",
      "name": "Temperature",
      "value": "25",
      "propertyID": "https://bioregistry.io/NCIT:C25206",
      "unitCode": "http://purl.obolibrary.org/obo/UO_0000027",
      "unitText": "degree Celsius"
    },
    {
      "@id": "#Process_Preparation",
      "@type": "LabProcess",
      "name": "Preparation",
      "object": {
        "@id": "#Material_MyObservable"
      },
      "result": {
        "@id": "#Sample_MySample"
      },
      "parameterValue": {
        "@id": "#CharacteristicValue_Temperature_25"
      }
    },
    {
      "@id": "#Process_Measurement",
      "@type": "LabProcess",
      "name": "Measurement",
      "object": {
        "@id": "#Sample_MySample"
      },
      "result": {
        "@id": "datafile.wiff"
      }
    },
    {
      "@id": "LICENSE",
      "@type": "CreativeWork",
      "text": "ALL RIGHTS RESERVED BY THE AUTHORS"
    },
    {
      "@id": "./",
      "@type": "Dataset",
      "description": "An example of a ROCrate with a core process model, including preparation and measurement processes.",
      "name": "Experimental ARC Process Core Crate Example",
      "hasPart": {
        "@id": "datafile.wiff"
      },
      "about": [
        {
          "@id": "#Process_Preparation"
        },
        {
          "@id": "#Process_Measurement"
        }
      ],
      "dateCreated": "2026-06-24T23:04:48.2431278",
      "license": {
        "@id": "LICENSE"
      }
    },
    {
      "@id": "ro-crate-metadata.json",
      "@type": "CreativeWork",
      "conformsTo": {
        "@id": "https://w3id.org/ro/crate/1.2"
      },
      "about": {
        "@id": "./"
      }
    }
  ]
}
```


## Requirements

### Dataset

[schema.org/Dataset](https://schema.org/Dataset) containing and contexualizing the processes.

| Property | Required | Expected Type | Description |
|----------|----------|---------------|-------------|
|@id|MUST|Text or URL|According to ROCrate specification.|
|@type|MUST|Text|MUST be '[schema.org/Dataset](https://schema.org/Dataset)'|
|about|SHOULD|[bioschemas.org/LabProcess](#labprocess)|The processes described here and possibly leading up to the files grouped in this dataset.|
|hasPart|COULD|[File](https://schema.org/MediaObject)|Data files resulting from the process sequence.|

### LabProcess

Has the new Bioschemas DRAFT [bioschemas.org/LabProcess](https://bioschemas.org/LabProcess) type and maps to the [ISA-JSON Process](https://isa-specs.readthedocs.io/en/latest/isajson.html#process-schema-json)

| Property | Required | Expected Type | Description |
|----------|----------|---------------|-------------|
|@id|MUST|Text or URL|Could identify the process using the isa metadata filename and the protocol reference or process name.|
|@type |MUST|Text|MUST be '[bioschemas.org/LabProcess](https://bioschemas.org/LabProcess)'|
|name|MUST|Text| -|
|object|SHOULD|[bioschemas.org/Sample](#sample) or [File](https://schema.org/MediaObject)|The input of the process. If there are multiple inputs, they SHOULD be stored as a sorted list to establish correspondence with outputs. (Both lists need the same length in that case.)|
|result|SHOULD|[bioschemas.org/Sample](#sample) or [File](https://schema.org/MediaObject)|The output of the process. If there are multiple outputs, they SHOULD be stored as a sorted list to establish correspondence with inputs. (Both lists need the same length in that case.)|
|agent|SHOULD|[schema.org/Person](#person)|The performer|
|executesLabProtocol|SHOULD|[bioschemas.org/LabProtocol](https://bioschemas.org/LabProtocol)|The protocol executed|
|parameterValue|SHOULD|[schema.org/PropertyValue](https://schema.org/PropertyValue) ([Parameter](#propertyvalue---parameter))|A parameter value of the experimental process, usually a key-value pair using ontology terms|
|endTime|SHOULD|DateTime||
|disambiguatingDescription|COULD|Text|Comments|

### LabProtocol

Is based on the Bioschemas [bioschemas.org/LabProtocol](https://bioschemas.org/LabProtocol) type and maps to the [ISA-JSON Protocol](https://isa-specs.readthedocs.io/en/latest/isajson.html#protocol-schema-json)  

| Property | Required | Expected Type | Description |
|----------|----------|---------------|-------------|
|@id|MUST|Text or URL|Could be the url pointing to the protocol resource.|
|@type |MUST|Text|MUST be '[bioschemas.org/LabProtocol](https://bioschemas.org/LabProtocol)'|
|description|SHOULD|Text|A short description of the protocol (e.g. an abstract)|
|intendedUse|SHOULD|[schema.org/DefinedTerm](#definedterm) or Text or URL|The protocol type as an ontology term|
|name|SHOULD|Text|Main title of the LabProtocol.|
|comment|COULD|[schema.org/Comment](#comment)|Comment|
|computationalTool|COULD|[schema.org/DefinedTerm](#definedterm) or [schema.org/PropertyValue](https://schema.org/PropertyValue) ([Component](#propertyvalue---component)) or [schema.org/SoftwareApplication](https://schema.org/SoftwareApplication)|Software or tool used as part of the lab protocol to complete a part of it.|
|labEquipment|COULD|[schema.org/DefinedTerm](#definedterm) or [schema.org/PropertyValue](https://schema.org/PropertyValue) ([Component](#propertyvalue---component)) or Text or URL|For LabProtocols it would be a laboratory equipment use by a person to follow one or more steps described in this LabProtocol.|
|reagent|COULD|[schema.org/BioChemEntity](https://schema.org/BioChemEntity://bioschemas.org/Sample) or [schema.org/DefinedTerm](#definedterm) or [schema.org/PropertyValue](https://schema.org/PropertyValue) ([Component](#propertyvalue---component)) or Text or URL|Reagents used in the protocol.|
|url|COULD|URL|Pointer to protocol resources external to the ISA-Tab that can be accessed by their Uniform Resource Identifier (URI).|
|version|COULD|Number or Text|An identifier for the version to ensure protocol tracking.|

### Sample

Is based on the Bioschemas [bioschemas.org/Sample](https://bioschemas.org/Sample) type, and represents the ISA-JSON [Sample](https://isa-specs.readthedocs.io/en/latest/isajson.html#sample-schema-json),
[Source](https://isa-specs.readthedocs.io/en/latest/isajson.html#source-schema-json) and [Material](https://isa-specs.readthedocs.io/en/latest/isajson.html#material-schema-json)

| Property | Required | Expected Type | Description |
|----------|----------|---------------|-------------|
|@id|MUST|Text or URL|Could be the unique sample name.|
|@type |MUST|Text|MUST be '[bioschemas.org/Sample](https://bioschemas.org/Sample)'|
|name|MUST|Text|A name identifying the sample.|
|additionalProperty|SHOULD|[schema.org/PropertyValue](https://schema.org/PropertyValue) ([Characteristic](#propertyvalue---characteristic) or [Factor](#propertyvalue---factor))|characteristics or factors|

### Data

Describes and points to a Data file or a segment of a Data file (via [data fragment selectors](https://www.w3.org/TR/annotation-model/#fragment-selector)), and maps to the [ISA-JSON Data](https://isa-specs.readthedocs.io/en/latest/isajson.html#data-schema-json)

| Property | Required | Expected Type | Description |
|----------|----------|---------------|-------------|
|@id|MUST|Text or URL|Should be the path pointing to the file|
|@type |MUST|Text|MUST be 'File' or 'MediaObject'|
|name|MUST|Text or URL|The name of the file.|
|comment|COULD|[schema.org/Comment](#comment)|Comment|
|disambiguatingDescription|COULD|Text|The type of the data file (“Raw Data File", “Derived Data File" or "Image File").|
|encodingFormat|COULD|Text of URL|Media format as a MIME type|
|hasPart|COULD|Text of URL|Data fragments of this Data object, described by [data fragment selectors](https://www.w3.org/TR/annotation-model/#fragment-selector). SHOULD not be used on data fragments.|
|usageInfo|COULD|Text of URL|Description/specification of the [data fragment selector](https://www.w3.org/TR/annotation-model/#fragment-selector), if the object describes a data fragment and a selector is present in the path/`@id`. SHOULD only be used on data fragments.|

Entities referenced by an processes's [object](http://schema.org/object) or [result](http://schema.org/result) SHOULD be of type `File` (an RO-Crate alias for [MediaObject](http://schema.org/MediaObject)) for files, [Dataset](http://schema.org/Dataset) for directories and [Collection](http://schema.org/Collection) for [multi-file datasets](#representing-multi-file-objects), but MAY be a [CreativeWork](http://schema.org/CreativeWork) for other types of data (e.g. an online database); they MAY be of type [PropertyValue](http://schema.org/PropertyValue) to capture numbers/strings that are not stored as files.

Data entities involved in an application's input and output SHOULD have an `@id` that reflects the original file or directory name as processed by the application, but MAY be renamed to avoid clashes with other entities in the crate. In this case, they SHOULD refer to the original name via [alternateName](http://schema.org/alternateName). This is particularly important to support reproducibility in cases where an application expects to find input in specific locations and with specific names (see the MIRAX example in [Representing multi-file objects](#representing-multi-file-objects)).


## Multiple processes

A process crate can be used to indicate one single execution as a single `CreateAction`, or a series of processes that generate different data entities. These actions MAY form an *implicit workflow* by following the links between entities that appear as `result` in an action and as `object` in the following one, but a process crate is not required to ensure such consistency (e.g. there may be an intermediate action that has not been recorded).

<img alt="Multiple processes diagram" src="../img/multiple_processes.svg" width="800" />


## Referencing configuration files

Some applications support the modification of their behavior via configuration files. Typically, these are not part of the input interface, but are searched for by the application among a set of possible predefined file system paths. In the case of applications that support a configuration file, the specific configuration file used during a run SHOULD be added to the `object` attribute of the corresponding `CreateAction`, especially if its settings are different from the default ones.

```json
    {
        "@id": "#SepiaConversion_1",
        "@type": "CreateAction",
        "name": "Convert dog image to sepia",
        "description": "convert -sepia-tone 80% test_data/sample/pics/2017-06-11\\ 12.56.14.jpg test_data/sample/pics/sepia_fence.jpg",
        "endTime": "2018-09-19T17:01:07+10:00",
        "instrument": {"@id": "https://www.imagemagick.org/"},
        "object": [
            {"@id": "pics/2017-06-11%2012.56.14.jpg"},
            {"@id": "SepiaConversion_1/colors.xml"}
        ],
        "result": {"@id": "pics/sepia_fence.jpg"},
        "agent": {"@id": "https://orcid.org/0000-0001-9842-9718"}
    },
    {
        "@id": "SepiaConversion_1/colors.xml",
        "@type": "File",
        "description": "Imagemagick color names configuration",
        "encodingFormat": "text/xml",
        "name": "colors"
    }
```


## Representing multi-file objects

In some formats, the data belonging to a digital entity is stored in more than one file. For instance, the [Mirax2-Fluorescence-2](https://openslide.cs.cmu.edu/download/openslide-testdata/Mirax/Mirax2-Fluorescence-2.zip) image is stored as the following set of files:

```
Mirax2-Fluorescence-2.mrxs
Mirax2-Fluorescence-2/Index.dat
Mirax2-Fluorescence-2/Slidedat.ini
Mirax2-Fluorescence-2/Data0000.dat
Mirax2-Fluorescence-2/Data0001.dat
...
Mirax2-Fluorescence-2/Data0023.dat
```

An application that reads [this format](https://openslide.org/formats/mirax/) needs to be pointed to the `.mrxs` file, and expects to find a directory containing the other files in the same location as the `.mrxs` file, with the same name minus the extension. Thus, even though an application that processes MIRAX files would probably take only the `.mrxs` file as argument, the other ones must be present in the expected location and with the expected names (in CWL, this kind of relationship is expressed via `secondaryFiles`). In this case, the object SHOULD be represented by a [contextual entity](https://www.researchobject.org/ro-crate/1.1/contextual-entities.html) of type [Collection](http://schema.org/Collection) listing all files under `hasPart`, with a `mainEntity` referencing the main file. The collection SHOULD be referenced from the root data entity via `mentions`.

```json
{
    "@id": "./",
    "@type": "Dataset",
    "hasPart": [
        {"@id": "Mirax2-Fluorescence-2.mrxs"},
        {"@id": "Mirax2-Fluorescence-2/"},
        {"@id": "Mirax2-Fluorescence-2.png"}
    ],
    "mentions": [
        {"@id": "https://openslide.cs.cmu.edu/download/openslide-testdata/Mirax/Mirax2-Fluorescence-2.zip"},
        {"@id": "#conversion_1"}
    ]
},
{
    "@id": "https://openslide.org/",
    "@type": "SoftwareApplication",
    "url": "https://openslide.org/",
    "name": "OpenSlide",
    "version": "3.4.1"
},
{
    "@id": "#conversion_1",
    "@type": "CreateAction",
    "name": "Convert image to PNG",
    "endTime": "2018-09-19T17:01:07+10:00",
    "instrument": {"@id": "https://openslide.org/"},
    "object": {"@id": "https://openslide.cs.cmu.edu/download/openslide-testdata/Mirax/Mirax2-Fluorescence-2.zip"},
    "result": {"@id": "Mirax2-Fluorescence-2.png"}
},
{
    "@id": "https://openslide.cs.cmu.edu/download/openslide-testdata/Mirax/Mirax2-Fluorescence-2.zip",
    "@type": "Collection",
    "mainEntity": {"@id": "Mirax2-Fluorescence-2.mrxs"},
    "hasPart": [
        {"@id": "Mirax2-Fluorescence-2.mrxs"},
        {"@id": "Mirax2-Fluorescence-2/"}
    ]
},
{
    "@id": "Mirax2-Fluorescence-2.mrxs",
    "@type": "File"
},
{
    "@id": "Mirax2-Fluorescence-2/",
    "@type": "Dataset"
},
{
    "@id": "Mirax2-Fluorescence-2.png",
    "@type": "File"
}
```

If the collection does not have a web presence, its `@id` can be an arbitrary internal one, possibly randomly generated (as for any other contextual entity):

```json
{
    "@id": "#af0253d688f3409a2c6d24bf6b35df7c4e271292",
    "@type": "Collection",
    "mainEntity": {"@id": "Mirax2-Fluorescence-2.mrxs"},
    "hasPart": [
        {"@id": "Mirax2-Fluorescence-2.mrxs"},
        {"@id": "Mirax2-Fluorescence-2/"}
    ]
}
```

The use case shown here is an example of a situation where it's important to refer to the original names in case any renamings took place, as described in [Requirements](#requirements):

```json
{
    "@id": "#af0253d688f3409a2c6d24bf6b35df7c4e271292",
    "@type": "Collection",
    "mainEntity": {"@id": "f62aa607a75508ac5fc6a22e9c0e39ef58a2c852"},
    "hasPart": [
        {"@id": "f62aa607a75508ac5fc6a22e9c0e39ef58a2c852"},
        {"@id": "c7398fbf741b851e80ae731d60cbee9258ff81f3/"}
    ]
},
{
    "@id": "f62aa607a75508ac5fc6a22e9c0e39ef58a2c852",
    "@type": "File",
    "alternateName": "Mirax2-Fluorescence-2.mrxs"
},
{
    "@id": "c7398fbf741b851e80ae731d60cbee9258ff81f3/",
    "@type": "Dataset",
    "alternateName": "Mirax2-Fluorescence-2/",
    "hasPart": [
        {"@id": "c7398fbf741b851e80ae731d60cbee9258ff81f3/46c443af080a36000c9298b49b675eb240eeb41c"},
        ...
    ]
},
{
    "@id": "c7398fbf741b851e80ae731d60cbee9258ff81f3/46c443af080a36000c9298b49b675eb240eeb41c",
    "@type": "File",
    "alternateName": "Mirax2-Fluorescence-2/Index.dat"
},
...
```


## Representing environment variable settings

The behavior of some applications may be modified by setting appropriate environment variables. These are different from ordinary application inputs in that they are part of the environment in which the process runs, rather than parameters supplied through a command line or a graphical interface. To represent the fact that an environment variable was set to a certain value during the execution of an action, use the `environment` property from the [workflow-run](https://w3id.org/ro/terms/workflow-run#) ro-terms namespace, making it point to a `PropertyValue` that describes the setting:

```json
{
    "@context": [
        "https://w3id.org/ro/crate/1.1/context",
        "https://w3id.org/ro/terms/workflow-run/context"
    ],
    "@graph": [
        ...
        {
            "@id": "#SepiaConversion_1",
            "@type": "CreateAction",
            "instrument": {"@id": "https://www.imagemagick.org/"},
            "object": {"@id": "pics/2017-06-11%2012.56.14.jpg"},
            "result": {"@id": "pics/sepia_fence.jpg"},
            "environment": [
                {"@id": "#height-limit-pv"},
                {"@id": "#width-limit-pv"}
            ]
        },
        {
            "@id": "#width-limit-pv",
            "@type": "PropertyValue",
            "name": "MAGICK_WIDTH_LIMIT",
            "value": "4096"
        },
        {
            "@id": "#height-limit-pv",
            "@type": "PropertyValue",
            "name": "MAGICK_HEIGHT_LIMIT",
            "value": "3072"
        }
    ]
}
```

Note that we added the `workflow-run` context to the `@context` entry in order to bring in the definition of `environment`.

Environment variable settings SHOULD be listed if they are different from the default ones (usually unset) and affected the results of the action.


## Representing container images

An application may use one or more container images (e.g. [Docker](https://www.docker.com) container images) to perform its duty. An action MAY indicate that a container image was used during the execution via the `containerImage` property, defined in the [workflow-run](https://github.com/ResearchObject/ro-terms/tree/master/workflow-run) ro-terms namespace.

```json
{
    "@id": "#cb04c897-eb92-4c53-8a38-bcc1a16fd650",
    "@type": "CreateAction",
    "instrument": {"@id": "bam2fastq.cwl"},
    ...
    "containerImage": {"@id": "#samtools-image"}
},
{
    "@id": "#samtools-image",
    "@type": "ContainerImage",
    "additionalType": {"@id": "https://w3id.org/ro/terms/workflow-run#DockerImage"},
    "registry": "docker.io",
    "name": "biocontainers/samtools",
    "tag": "v1.9-4-deb_cv1",
    "sha256": "da61624fda230e94867c9429ca1112e1e77c24e500b52dfc84eaf2f5820b4a2a"
}
```

The `ContainerImage` type (note the leading lowercase "C") and most of the properties shown above are also defined in the workflow-run namespace. The `additionalType` describes the specific image type (e.g., `DockerImage`, `SIFImage`); the registry is the service that hosts the image (e.g., "docker.io", "quay.io"); the `name` is the identifier of the image within the registry; `tag` describes the image tag and `sha256` its sha256 checksum. A `ContainerImage` entity SHOULD list at least the `additionalType`, `registry` and `name` properties.

Alternatively, the `containerImage` could point to a `URL`. For instance:

```json
{
    "@id": "#cb04c897-eb92-4c53-8a38-bcc1a16fd650",
    "@type": "CreateAction",
    "instrument": {"@id": "bam2fastq.cwl"},
    ...
    "containerImage": "https://example.com/samtools.sif"
}
```


## Specifying software dependencies

Software dependencies MAY be specified using `softwareRequirements` to a `SoftwareApplication`:

```json
{
    "@id": "script.py",
    "@type": "SoftwareApplication",
    "name": "Analysis Script",
    "version": "0.1",
    "softwareRequirements": {"@id": "https://pypi.org/project/numpy/1.26.2/"}
},
{
    "@id": "https://pypi.org/project/numpy/1.26.2/",
    "@type": "SoftwareApplication",
    "name": "NumPy",
    "version": "1.26.2"
}
```