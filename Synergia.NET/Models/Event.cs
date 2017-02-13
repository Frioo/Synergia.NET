using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergia.NET.Models
{
    public class Event
    {
        public string ID { get; }
        public string Description { get; }
        public LocalDate Date { get; }
        public string EventCategoryID { get; }
        public int LessonNumber { get; }
        public string AuthorID { get; }
        public LocalDateTime AddDate { get; }

        public Event(string id, string description, LocalDate date, string eventCategoryId, int lessonNumber, string authorId, LocalDateTime addDate)
        {
            this.ID = id;
            this.Description = description;
            this.Date = date;
            this.EventCategoryID = eventCategoryId;
            this.LessonNumber = lessonNumber;
            this.AuthorID = authorId;
            this.AddDate = addDate;
        }
    }
}
