#addin nuget:?package=Cake.XmlConfigStructureBuilder&version=1.1.0&loaddependencies=true
#addin nuget:?package=Cake.PickAndRoll&version=0.3.0-alpha&prerelease&loaddependencies=true

var target = Argument("target", "Default");

Task("Default")
    .Does(() => {
        MakeConfigs("Release", ".");
        PickAndRoll();
    });

RunTarget(target);
