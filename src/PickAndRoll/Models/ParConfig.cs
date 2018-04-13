using System.Collections.Generic;

namespace PickAndRoll.Models
{
    public class ParConfig
    {
        public string CustomDir { get; set; }
        public IEnumerable<string> Files { get; set; }
    }
}