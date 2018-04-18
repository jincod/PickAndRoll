# Cake.PickAndRoll

[![Build status](https://ci.appveyor.com/api/projects/status/o67rl97q9282im8c?svg=true)](https://ci.appveyor.com/project/jincod/pickandroll)
[![NuGet](https://img.shields.io/nuget/v/cake.pickandroll.svg)](https://www.nuget.org/packages/Cake.PickAndRoll)

Cake Addin for working with PickAndRoll


## Usage

```csharp
#addin "Cake.PickAndRoll"

Task("Default")
    .Does(() => {
        PickAndRoll();
    });
```