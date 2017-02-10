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
        public string id { get; }
        public string description { get; }
        public LocalDate date { get; }
        public string eventCategoryId { get; }
        public int lessonNumber { get; }
        public string authorId { get; }
        public LocalDateTime addDate { get; }

        public Event(string id, string description, LocalDate date, string eventCategoryId, int lessonNumber, string authorId, LocalDateTime addDate)
        {
            this.id = id;
            this.description = description;
            this.date = date;
            this.eventCategoryId = eventCategoryId;
            this.lessonNumber = lessonNumber;
            this.authorId = authorId;
            this.addDate = addDate;
        }
    }
}
