using System.Collections.Generic;

namespace PickAndRoll.Models
{
    public class PickAndRollConfig
    {
        public string MasterConfigPath { get; set; }
        public string CustomConfigPath { get; set; }
        public IEnumerable<string> FilePatterns { get; set; }
        public IEnumerable<string> ExtraConfigsPath { get; set; }
    }
}