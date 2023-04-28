> **Note** This repository is developed for .netstandard2.1

[![NuGet Version](https://img.shields.io/nuget/v/MockAsyncEnumerable.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/MockAsyncEnumerable/)
[![Nuget Downloads](https://img.shields.io/nuget/dt/MockAsyncEnumerable.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/MockAsyncEnumerable)

This repository results from the necessity to implement paged grid results as in some projects/solutions grid result was provided by stored procedures.

A simple implementation for a list of data transforms to async enumerable, allowing the use of paginated grid results or any action or dynamic aggregated query using EF Core (`Microsoft.EntityFrameworkCore`) through Expressions (`System.Linq.Expressions`).

**In case you wish to use it in your project, u can install the package from <a href="https://www.nuget.org/packages/MockAsyncEnumerable" target="_blank">nuget.org</a>** or specify what version you want:


> `Install-Package MockAsyncEnumerable -Version x.x.x.x`

## Content
1. [USING](docs/usage.md)
1. [CHANGELOG](docs/CHANGELOG.md)
1. [BRANCH-GUIDE](docs/branch-guide.md)