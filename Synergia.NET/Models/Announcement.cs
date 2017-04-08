using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergia.NET.Models
{
    public class Announcement
    {
        public string ID { get; }
        public LocalDate StartDate { get; }
        public LocalDate EndDate { get; }
        public string Subject { get; }
        public string Content { get; }
        public string AuthorID { get; }

        public Announcement(string id, LocalDate startDate, LocalDate endDate, string subject, string content, string authorId)
        {
            this.ID = id;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.Subject = subject;
            this.Content = content;
            this.AuthorID = authorId;
        }
    }
}
