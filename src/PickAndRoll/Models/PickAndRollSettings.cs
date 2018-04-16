using System.Collections.Generic;

namespace PickAndRoll.Models
{
    public class PickAndRollSettings
    {
        public string Pwd { get; set; }
        public string ParConfigFileName { get; set; }
        public string ConfigFileName { get; set; }
        public IEnumerable<string> Files { get; set; } = new string[] { };
        public IEnumerable<string> ExtraConfigsPath { get; set; } = new string[] { };
    }
}