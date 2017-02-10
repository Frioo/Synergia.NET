using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergia.NET.Models
{
    public class Subject
    {
        public int id { get; }
        public string name { get; }
        public string shortName { get; }
        public int number { get; }
        public bool isExtracurricular { get; }

        public Subject(int id, string name,string shortName, int number, bool isExtracurricular)
        {
            this.id = id;
            this.name = name;
            this.shortName = shortName;
            this.number = number;
            this.isExtracurricular = isExtracurricular;
        }
    }
}
