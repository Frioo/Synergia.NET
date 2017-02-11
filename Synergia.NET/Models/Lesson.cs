using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergia.NET.Models
{
    public class Lesson
    {
        public string id { get; }
        public string teacherId { get; }
        public string subjectId { get; }

        public Lesson(string id, string teacherId, string subjectId)
        {
            this.id = id;
            this.teacherId = teacherId;
            this.subjectId = subjectId;
        }
    }
}
