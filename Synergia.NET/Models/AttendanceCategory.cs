using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergia.NET.Models
{
    public class AttendanceCategory
    {
        public string id { get; }
        public string name { get; }
        public string shortName { get; }
        // ColorRGB

        public AttendanceCategory(string id, string name, string shortName)
        {
            this.id = id;
            this.name = name;
            this.shortName = shortName;
        }
    }
}
