using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergia.NET.Models
{
    public class TextGrade
    {
        public string ID { get; }
        public string LessonID { get; }
        public string SubjectID { get; }
        public string AuthorID { get; }
        public string CategoryID { get; }
        public string Grade { get; }
        public LocalDate Date { get; }
        public LocalDateTime AddDate { get; }
        public int SemesterNumber { get; }
        public bool IsSemester { get; }
        public bool IsSemesterProposition { get; }
        public bool IsFinal { get; }
        public bool IsFinalProposition { get; }

        public TextGrade(string id, string lessonId, string subjectId, string authorId, string categoryId, string grade, LocalDate date, LocalDateTime addDate, int semesterNumber, bool isSemester, bool isSemesterProposition, bool isFinal, bool isFinalProposition)
        {
            this.ID = id;
            this.LessonID = lessonId;
            this.SubjectID = subjectId;
            this.AuthorID = authorId;
            this.CategoryID = categoryId;
            this.Grade = grade;
            this.Date = date;
            this.AddDate = addDate;
            this.SemesterNumber = semesterNumber;
            this.IsSemester = isSemester;
            this.IsSemesterProposition = isSemesterProposition;
            this.IsFinal = isFinal;
            this.IsFinalProposition = isFinalProposition;
        }
    }
}
