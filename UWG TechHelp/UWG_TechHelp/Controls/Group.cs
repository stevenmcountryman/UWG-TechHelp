using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWG_TechHelp.Controls
{
    public class Group
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return Name.Replace("__b", " ");
        }
    }
}
