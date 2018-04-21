#addin nuget:?package=Cake.XmlConfigStructureBuilder&prerelease&version=0.5.2-alpha
#addin nuget:?package=Cake.PickAndRoll&prerelease&version=0.1.9-alpha

var target = Argument("target", "Default");

Task("Default")
    .Does(() => {
        MakeConfigs("Release", ".");
        PickAndRoll();
    });

RunTarget(target);
