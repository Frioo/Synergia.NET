using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergia.NET.Models
{
    public class Teacher
    {
        public int id { get; }
        public string firstName { get; }
        public string lastName { get; }
        public string fullName { get; }
        // public bool isSchoolAdministrator { get; }

        public Teacher(int id, string firstName, string lastName)
        {
            this.id = id;
            this.firstName = firstName;
            this.lastName = lastName;
            this.fullName = firstName + " " + lastName;
        }
    }
}
