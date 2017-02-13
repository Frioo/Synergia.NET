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
        public string ID { get; }
        public string LessonID { get; }
        //trip
        public LocalDate Date { get; }
        public LocalDateTime AddDate { get; }
        public int LessonNumber { get; }
        public int SemesterNumber { get; }
        public string TypeID { get; }
        public string AuthorID { get; }

        public Attendance(string id, string lessonId, LocalDate date, LocalDateTime addDate, int lessonNumber, int semesterNumber, string typeId, string authorId)
        {
            this.ID = id;
            this.LessonID = lessonId;
            this.Date = date;
            this.AddDate = addDate;
            this.LessonNumber = lessonNumber;
            this.SemesterNumber = semesterNumber;
            this.TypeID = typeId;
            this.AuthorID = authorId;
        }
    }
}
