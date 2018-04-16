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

            foreach (var filename in filePatterns)
            {
                var content = File.ReadAllText(filename);
                var rolledFileName = Regex.Replace(filename, ".generic", string.Empty, RegexOptions.IgnoreCase);
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
            string ReplaceKey(string result, KeyValuePair<string, object> item)
            {
                return Regex.Replace(result, $"@@{item.Key}@@", $"{item.Value}", RegexOptions.IgnoreCase);
            }

            return config.Aggregate(content, ReplaceKey);
        }

        private static JObject Pick(PickAndRollConfig config)
        {
            var result = new JObject();

            var files = config.ExtraConfigsPath
                .Concat(new[]
                {
                    config.MasterConfigPath,
                    config.CustomConfigPath
                })
                .Where(f => !string.IsNullOrEmpty(f))
                .Where(File.Exists);


            foreach (var extraConfigPath in files) result.Merge(JObject.Parse(File.ReadAllText(extraConfigPath)));

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

            var parConfig = GetParConfig(parConfigFileName, settings.Files ?? new string[] { });
            var extraConfigsPath = settings.ExtraConfigsPath != null
                ? settings.ExtraConfigsPath.Select(f => Path.GetFullPath(Path.Combine(pwd, parConfig.CustomDir, f)))
                : new string[] { };
            var customConfigFileName = $"{settings.ConfigFileName ?? Environment.MachineName}.json";

            return new PickAndRollConfig
            {
                FilePatterns = parConfig.Files.Select(f => Path.GetFullPath(Path.Combine(pwd, f))),
                ExtraConfigsPath = extraConfigsPath,
                MasterConfigPath = Path.GetFullPath(Path.Combine(pwd, parConfig.CustomDir, "config.json")),
                CustomConfigPath = Path.GetFullPath(Path.Combine(pwd, parConfig.CustomDir, customConfigFileName))
            };
        }
    }
}