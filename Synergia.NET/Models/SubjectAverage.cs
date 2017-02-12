using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergia.NET.Models
{
    public class SubjectAverage
    {
        public string SubjectID { get; }
        public string FirstSemester { get; }
        public string SecondSemester { get; }
        public string FullYear { get; }

        public SubjectAverage(string subjectId, string firstSemester, string secondSemester, string fullYear)
        {
            this.SubjectID = subjectId;
            this.FirstSemester = firstSemester;
            this.SecondSemester = secondSemester;
            this.FullYear = fullYear;
        }
    }
}
