#addin nuget:?package=Cake.XmlConfigStructureBuilder&version=1.1.1&loaddependencies=true
#addin nuget:?package=Cake.PickAndRoll&version=1.1.0&loaddependencies=true

var target = Argument("target", "Default");

Task("Default")
    .Does(() => {
        MakeConfigs("Release", ".");
        PickAndRoll();
    });

RunTarget(target);
