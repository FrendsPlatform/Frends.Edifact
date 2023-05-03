# Frends.Edifact.ConvertToJson
Frends Task to convert Edifact to JSON.

[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT) 
[![Build](https://github.com/FrendsPlatform/Frends.Edifact/actions/workflows/Frends.Edifact.ConvertToJson_main.yml/badge.svg)](https://github.com/FrendsPlatform/Frends.Edifact/actions)
![Coverage](https://app-github-custom-badges.azurewebsites.net/Badge?key=FrendsPlatform/Frends.Edifact/Frends.Edifact.ConvertToJson|main)

# Installing

You can install the Task via Frends UI Task View.

## Building

NB! Use the solution file `Frends.Edifact.Json.sln` to make sure that tests work, do not open `SolutionFileForBuildPipeline.sln`. This is because `dotnet build` and `dotnet test` command need a solution to succeed (and we call those in GitHub Actions during build), but for development in Visual Studio we have a shared solution file so that Visual Studio works propertly. 

Rebuild the project

`dotnet build`

Run tests
 
`dotnet test`


Create a NuGet package

`dotnet pack --configuration Release`