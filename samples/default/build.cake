#addin nuget:file://localhost/packages/?package=Cake.PickAndRoll&prerelease&version=0.1.5-alpha

var target = Argument("target", "Default");

Task("Default")
    .Does(() => {
        PickAndRoll();
    });

RunTarget(target);
