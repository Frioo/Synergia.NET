using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergia.NET.Models
{
    public class Grade
    {
        public string ID { get; }
        public string LessonID { get; }
        public string SubjectID { get; }
        public string CategoryID { get; }
        public string AuthorID { get; }
        public string Value { get; }
        public LocalDate Date { get; }
        public LocalDateTime AddDate { get; }
        public int SemesterNumber { get; }
        public bool IsConstituent { get; }
        public bool IsSemesterGrade { get; }
        public bool IsSemesterProposition { get; }
        public bool IsFinalGrade { get; }
        public bool IsFinalProposition { get; }
        public string GradeCommentID { get; }

        public Grade(string id, string lessonId, string subjectId, string categoryId, string authorId, string grade, LocalDate date, LocalDateTime addDate, int semesterNumber, bool isConstituent,bool isSemesterGrade, bool isSemesterProposition, bool isFinalGrade, bool isFinalProposition, string gradeCommentId)
        {
            this.ID = id;
            this.LessonID = lessonId;
            this.SubjectID = subjectId;
            this.CategoryID = categoryId;
            this.AuthorID = authorId;
            this.Value = grade;
            this.Date = date;
            this.AddDate = addDate;
            this.SemesterNumber = semesterNumber;
            this.IsConstituent = isConstituent;
            this.IsSemesterGrade = isSemesterGrade;
            this.IsSemesterProposition = isSemesterProposition;
            this.IsFinalGrade = isFinalGrade;
            this.IsFinalProposition = isFinalProposition;
            this.GradeCommentID = gradeCommentId;
        }
    }
}
