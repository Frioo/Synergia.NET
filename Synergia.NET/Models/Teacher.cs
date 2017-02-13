using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergia.NET.Models
{
    public class Teacher
    {
        public int ID { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string FullName { get; }
        // public bool isSchoolAdministrator { get; }

        public Teacher(int id, string firstName, string lastName)
        {
            this.ID = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.FullName = firstName + " " + lastName;
        }
    }
}
