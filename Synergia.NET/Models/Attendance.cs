using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergia.NET.Models
{
    public class Attendance
    {
        public string id { get; }
        public string lessonId { get; }
        //trip
        public LocalDate date { get; }
        public LocalDateTime addDate { get; }
        public int lessonNumber { get; }
        public int semesterNumber { get; }
        public string typeId { get; }
        public string authorId { get; }

        public Attendance(string id, string lessonId, LocalDate date, LocalDateTime addDate, int lessonNumber, int semesterNumber, string typeId, string authorId)
        {
            this.id = id;
            this.lessonId = lessonId;
            this.date = date;
            this.addDate = addDate;
            this.lessonNumber = lessonNumber;
            this.semesterNumber = semesterNumber;
            this.typeId = typeId;
            this.authorId = authorId;
        }
    }
}
