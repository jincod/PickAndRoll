using System.Collections.Generic;

namespace PickAndRoll.Models
{
    public class PickAndRollConfig
    {
        public IEnumerable<string> FilePatterns { get; set; }
        public IEnumerable<string> ConfigFileNames { get; set; }
    }
}