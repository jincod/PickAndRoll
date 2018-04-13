# Cake.PickAndRoll

Cake Addin for working with PickAndRoll


## Usage

```csharp
#addin "Cake.PickAndRoll"

Task("Default")
    .Does(() => {
        PickAndRoll();
    });
```