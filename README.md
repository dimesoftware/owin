# Owin extensions

![Build Status](https://dev.azure.com/dimenicsbe/Utilities/_apis/build/status/dimenics.owin?branchName=master) ![Code coverage](https://img.shields.io/azure-devops/coverage/dimenicsbe/Utilities/153/master)

## Introduction

Extensions to the Owin namespace.

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

![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)

Pull requests are welcome. Please check out the contribution and code of conduct guidelines.

## License

[![License](http://img.shields.io/:license-mit-blue.svg?style=flat-square)](http://badges.mit-license.org)