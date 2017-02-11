using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergia.NET.Models
{
    public class GradeCategory
    {
        public string id { get; }
        public string name { get; }
        public int weight { get; }
        // Color

        public GradeCategory(string id, string name, int weight)
        {
            this.id = id;
            this.name = name;
            this.weight = weight;
        }
    }
}
