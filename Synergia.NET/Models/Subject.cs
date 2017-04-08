using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergia.NET.Models
{
    public class Subject
    {
        public string ID { get; }
        public string Name { get; }
        public string ShortName { get; }
        public int Number { get; }
        public bool IsExtracurricular { get; }

        public Subject(string id, string name,string shortName, int number, bool isExtracurricular)
        {
            this.ID = id;
            this.Name = name;
            this.ShortName = shortName;
            this.Number = number;
            this.IsExtracurricular = isExtracurricular;
        }
    }
}
