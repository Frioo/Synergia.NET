using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergia.NET.Models
{
    public class EventCategory
    {
        public string ID { get; }
        public string Name { get; }

        public EventCategory(string id, string name)
        {
            this.ID = id;
            this.Name = name;
        }
    }
}
