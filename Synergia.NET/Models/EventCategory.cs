using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergia.NET.Models
{
    public class EventCategory
    {
        public string id { get; }
        public string name { get; }

        public EventCategory(string id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
