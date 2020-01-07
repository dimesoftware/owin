# Dime.Owi

[![Build Status](https://dev.azure.com/dimenicsbe/Utilities/_apis/build/status/OWIN%20-%20MAIN%20-%20CI?branchName=master)](https://dev.azure.com/dimenicsbe/Utilities/_build/latest?definitionId=92&branchName=master)

## Introduction

Extensions to the OWIN namespace.

## Getting Started

- You must have Visual Studio 2019 Community or higher.
- The dotnet cli is also highly recommended.

## About this project

Generates expressions on the fly

## Build and Test

- Run dotnet restore
- Run dotnet build
- Run dotnet test

## Installation

Use the package manager NuGet to install Dime.Owin:

- dotnet cli: `dotnet add package Dime.Owin`
- Package manager: `Install-Package Dime.Owin`

## Usage

``` csharp
using Microsoft.Owin;

public static IAppBuilder UseLicenseMiddleware(this IAppBuilder appBuilder, bool mapRequest, params string[] excludePaths)
    => appBuilder.MapWhen(x => x.IsAjaxRequest());

```

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.
Please make sure to update tests as appropriate.

## License

MIT