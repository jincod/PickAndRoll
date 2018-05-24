#addin nuget:?package=Cake.XmlConfigStructureBuilder&version=1.0.0&loaddependencies=true
#addin nuget:?package=Cake.PickAndRoll&version=0.2.4-alpha&prerelease&loaddependencies=true

var target = Argument("target", "Default");

Task("Default")
    .Does(() => {
        MakeConfigs("Release", ".");
        PickAndRoll();
    });

RunTarget(target);
