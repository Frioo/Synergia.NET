using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergia.NET.Models
{
    public class Lesson
    {
        public string ID { get; }
        public string TeacherID { get; }
        public string SubjectID { get; }

        public Lesson(string id, string teacherId, string subjectId)
        {
            this.ID = id;
            this.TeacherID = teacherId;
            this.SubjectID = subjectId;
        }
    }
}
