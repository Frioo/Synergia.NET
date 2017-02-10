using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergia.NET.Models
{
    public class Lesson
    {
        public int id { get; }
        public int teacherId { get; }
        public int subjectId { get; }

        public Lesson(int id, int teacherId, int subjectId)
        {
            this.id = id;
            this.teacherId = teacherId;
            this.subjectId = subjectId;
        }
    }
}
