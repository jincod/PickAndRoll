using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using PickAndRoll.Models;

namespace PickAndRoll
{
    public class PickAndRoll
    {
        public void Go(PickAndRollSettings settings = null)
        {
            var config = ConfigProcess(settings);
            var extendedConfig = Pick(config);
            Roll(config.FilePatterns, extendedConfig);
        }

        private static void Roll(IEnumerable<string> filePatterns, JObject extendedConfig)
        {
            var flatConfig = FlatConfig(extendedConfig);

            //Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(flatConfig));

            foreach (var filename in filePatterns)
            {
                var content = File.ReadAllText(filename);
                var rolledFileName = Regex.Replace(filename, ".generic", string.Empty, RegexOptions.IgnoreCase);
                //Console.WriteLine($"Roll file: {filename}");
                var newContent = RollFile(flatConfig, content);
                File.WriteAllText(rolledFileName, newContent);
            }
        }

        private static Dictionary<string, object> FlatConfig(JObject extendedConfig)
        {
            return JsonHelper.DeserializeAndFlatten(extendedConfig);
        }

        private static string RollFile(Dictionary<string, object> config, string content)
        {
            return config.Aggregate(content,
                (result, item) => Regex.Replace(result, $"@@{item.Key}@@", $"{item.Value}", RegexOptions.IgnoreCase));
        }

        private static JObject Pick(PickAndRollConfig config)
        {
            var result = new JObject();

            if (!string.IsNullOrEmpty(config.MasterConfigPath) && File.Exists(config.MasterConfigPath))
                result.Merge(JObject.Parse(File.ReadAllText(config.MasterConfigPath)));

            if (!string.IsNullOrEmpty(config.CustomConfigPath) && File.Exists(config.CustomConfigPath))
                result.Merge(JObject.Parse(File.ReadAllText(config.CustomConfigPath)));

            foreach (var extraConfigPath in config.ExtraConfigsPath)
            {
                if (!string.IsNullOrEmpty(extraConfigPath) && File.Exists(extraConfigPath))
                    result.Merge(JObject.Parse(File.ReadAllText(extraConfigPath)));
            }

            return result;
        }

        private static ParConfig GetParConfig(string parConfigFileName, IEnumerable<string> settingsFiles)
        {
            var config = JObject.Parse(File.ReadAllText(parConfigFileName));

            var customDir = config.ContainsKey("customDir") ? config.GetValue("customDir").ToString() : "_configs";
            var files = config.ContainsKey("files") ? config.GetValue("files").ToObject<string[]>() : new string[] { };

            return new ParConfig
            {
                CustomDir = customDir,
                Files = settingsFiles.Union(files)
            };
        }

        private static PickAndRollConfig ConfigProcess(PickAndRollSettings settings)
        {
            var pwd = settings.Pwd ?? ".";
            var parConfigFileName =
                Path.GetFullPath(Path.Combine(pwd, settings.ParConfigFileName ?? ".parconfig"));

            var parConfig = GetParConfig(parConfigFileName, settings.Files ?? new string[]{});
            var customConfigFileName = $"{settings.ConfigFileName ?? Environment.MachineName}.json";

            return new PickAndRollConfig
            {
                FilePatterns = parConfig.Files.Select(f => Path.GetFullPath(Path.Combine(pwd, f))),
                ExtraConfigsPath = settings.ExtraConfigsPath.Select(f => Path.GetFullPath(Path.Combine(pwd, parConfig.CustomDir, f))),
                MasterConfigPath = Path.GetFullPath(Path.Combine(pwd, parConfig.CustomDir, "config.json")),
                CustomConfigPath = Path.GetFullPath(Path.Combine(pwd, parConfig.CustomDir, customConfigFileName))
            };
        }
    }
}