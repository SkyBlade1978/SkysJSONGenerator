using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkysJSONGenerator
{
    public class Block
    {
        public string Name { get; set; }
        public bool Side { get; set; }
        public bool Top { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
