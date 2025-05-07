# Frends.Edifact

Frends tasks for Edifact format.

# Tasks

- [Frends.Edifact.ConvertToJson](Frends.Edifact.ConvertToJson/README.md)
- [Frends.Edifact.CreateFromJson](Frends.Edifact.CreateFromJson/README.md)
- [Frends.Edifact.ConvertToXml](Frends.Edifact.ConvertToXml/README.md)
- [Frends.Edifact.CreateFromXml](Frends.Edifact.CreateFromXml/README.md)

# Special setup

In Edifact tasks we have conversions from Edifact to JSON and back and to XML and back. In order to make sure that those tasks work well in conjunction we have a special setup for unit testing.

- In a single task's folder we have a solution file called `SolutionFileForBuildPipeline.sln`, which is just a simple solution to make sure that GitHub Actions work when they call `dotnet build` or `dotnet test` (it will fail without a solution or project file).
- __In the repository root we have the `Frends.Edifact.Json.sln`, which should be used for local development.__
- The above is an example for JSON, but the same setup applies to XML too.

# Contributing
When contributing to this repository, please first discuss the change you wish to make via issue, email, or any other method with the owners of this repository before making a change.

1. Fork the repository on GitHub
2. Clone the project to your own machine
3. Commit changes to your own branch
4. Push your work back up to your fork
5. Submit a Pull request so that we can review your changes

NOTE: Be sure to merge the latest from "upstream" before making a pull request!
