using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergia.NET.Models
{
    public class AttendanceCategory
    {
        public string ID { get; }
        public string Name { get; }
        public string ShortName { get; }
        public bool IsPresenceType { get; }
        // ColorRGB

        public AttendanceCategory(string id, string name, string shortName, bool isPresenceType)
        {
            this.ID = id;
            this.Name = name;
            this.ShortName = shortName;
            this.IsPresenceType = isPresenceType;
        }
    }
}
