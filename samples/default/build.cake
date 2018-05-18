#addin nuget:?package=Cake.XmlConfigStructureBuilder&version=0.6.0-alpha&prerelease&loaddependencies=true
#addin nuget:?package=Cake.PickAndRoll&version=0.2.3-alpha&prerelease&loaddependencies=true

var target = Argument("target", "Default");

Task("Default")
    .Does(() => {
        MakeConfigs("Release", ".");
        PickAndRoll();
    });

RunTarget(target);
