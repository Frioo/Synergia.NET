using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergia.NET.Models
{
    public class GradeCategory
    {
        public string ID { get; }
        public string Name { get; }
        public int Weight { get; }
        // Color

        public GradeCategory(string id, string name, int weight)
        {
            this.ID = id;
            this.Name = name;
            this.Weight = weight;
        }
    }
}
