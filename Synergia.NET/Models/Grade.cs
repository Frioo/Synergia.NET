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
        public string id { get; }
        public string lessonId { get; }
        public string subjectId { get; }
        public string categoryId { get; }
        public string authorId { get; }
        public string grade { get; }
        public LocalDate date { get; }
        public LocalDateTime addDate { get; }
        public int semesterNumber { get; }
        public bool isConstituent { get; }
        public bool isSemesterGrade { get; }
        public bool isSemesterProposition { get; }
        public bool isFinalGrade { get; }
        public bool isFinalProposition { get; }
        public string GradeCommentID { get; }

        public Grade(string id, string lessonId, string subjectId, string categoryId, string authorId, string grade, LocalDate date, LocalDateTime addDate, int semesterNumber, bool isConstituent,bool isSemesterGrade, bool isSemesterProposition, bool isFinalGrade, bool isFinalProposition, string gradeCommentId)
        {
            this.id = id;
            this.lessonId = lessonId;
            this.subjectId = subjectId;
            this.categoryId = categoryId;
            this.authorId = authorId;
            this.grade = grade;
            this.date = date;
            this.addDate = addDate;
            this.semesterNumber = semesterNumber;
            this.isConstituent = isConstituent;
            this.isSemesterGrade = isSemesterGrade;
            this.isSemesterProposition = isSemesterProposition;
            this.isFinalGrade = isFinalGrade;
            this.isFinalProposition = isFinalProposition;
            this.GradeCommentID = gradeCommentId;
        }
    }
}
