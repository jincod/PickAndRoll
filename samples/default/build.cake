#addin nuget:file://localhost/packages/?package=Cake.PickAndRoll&prerelease

Task("Default")
    .Does(() => {
        PickAndRoll();
    });